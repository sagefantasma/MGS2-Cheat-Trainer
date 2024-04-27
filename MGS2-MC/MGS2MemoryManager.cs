using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimplifiedMemoryManager;

namespace MGS2_MC
{
    internal static class MGS2MemoryManager
    {
        #region Internals
        private const string loggerName = "MemoryManagerDebuglog.log";

        private static List<int> _lastKnownPlayerOffsets { get; set; } = default;
        private static List<int> _lastKnownStageOffsets { get; set; } = default;
        private static ILogger _logger { get; set; }

        internal static void StartLogger()
        {
            _logger = Logging.InitializeNewLogger(loggerName);
            _logger.Information($"Memory Manager for version {Program.AppVersion} initialized...");
            _logger.Verbose($"Instance ID: {Program.InstanceID}");
        }

        public class GameStats
        {
            public short Alerts;
            public short Continues;
            public short DamageTaken;
            public short Kills;
            public short MechsDestroyed;
            public int PlayTime;
            public short Rations;
            public short Saves;
            public short Shots;
            public short SpecialItems;

            public override string ToString()
            {
                return $"Alerts: {Alerts} -- Continues: {Continues} -- DamageTaken: {DamageTaken} -- Kills: {Kills} -- " +
                    $"MechsDestroyed: {MechsDestroyed} -- PlayTime: {PlayTime} -- Rations: {Rations} -- Saves: {Saves} -- " +
                    $"Shots: {Shots} -- SpecialItems: {SpecialItems}";
            }
        }
        #endregion

        #region Private methods
        internal static Constants.PlayableCharacter CheckIfUsable(MGS2Object mgs2Object)
        {
            Constants.PlayableCharacter currentPC = DetermineActiveCharacter();
            switch (currentPC)
            {
                case Constants.PlayableCharacter.Snake:
                    if (!Snake.UsableObjects.Contains(mgs2Object))
                    {
                        _logger.Warning($"Snake cannot use {mgs2Object.Name}");
                        throw new InvalidOperationException($"Snake cannot use {mgs2Object.Name}");
                    }
                    break;
                case Constants.PlayableCharacter.Raiden:
                    if (!Raiden.UsableObjects.Contains(mgs2Object))
                    {
                        _logger.Warning($"Raiden cannot use {mgs2Object.Name}");
                        throw new InvalidOperationException($"Raiden cannot use {mgs2Object.Name}");
                    }
                    break;
                default:
                    break;
            }

            return currentPC;
        }

        private static List<int> GetStageOffsets()
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    byte[] gameMemoryBuffer = proxy.GetProcessSnapshot();
                    //if we've retrieved a stage offset before, check the old one first
                    if (_lastKnownStageOffsets != default)
                    {
                        if (ValidateLastKnownOffsets(gameMemoryBuffer, _lastKnownStageOffsets, MGS2AoB.StageInfo))
                        {
                            _logger.Verbose($"Last known stageOffsets are still valid, reusing...");
                            return _lastKnownStageOffsets;
                        }
                    }

                    _lastKnownStageOffsets = new List<int>();

                    List<int> stageOffset = FindUniqueOffset(gameMemoryBuffer, MGS2AoB.StageInfo);
                    _logger.Verbose($"We found {stageOffset.Count} stage offsets in memory");

                    //ignore all results except for the final two if more than 2 are found.
                    if(stageOffset.Count > 1)
                        stageOffset = stageOffset.GetRange(stageOffset.Count - 2, 2);

                    _lastKnownStageOffsets = new List<int>(stageOffset);
                    return _lastKnownStageOffsets;
                }
            }
        }

        private static bool ValidateLastKnownOffsets(byte[] memoryBuffer, List<int> lastKnownOffsets, byte[] finderAoB)
        {
            try
            {
                bool offsetIsValid = true;
                if(lastKnownOffsets.Count == 0)
                {
                    return false;
                }

                foreach (int previousOffset in lastKnownOffsets)
                {
                    byte[] previousOffsetBuffer = new byte[finderAoB.Length];
                    Array.Copy(memoryBuffer, previousOffset, previousOffsetBuffer, 0, finderAoB.Length);
                    for (int i = 0; i < previousOffsetBuffer.Length; i++)
                    {
                        if (previousOffsetBuffer[i] != finderAoB[i])
                        {
                            //if ANY byte does not match exactly to the offsetBytes, we know the offset has moved
                            _logger.Verbose($"Last known offset at {previousOffset} has changed since we last looked!");
                            offsetIsValid = false;
                        }
                    }
                }

                _logger.Verbose($"Last known offset(s) are still valid.");
                return offsetIsValid;
            }
            catch (Exception e)
            {
                _logger.Warning($"Something unexpected went wrong when looking at the last known player offsets: {e}");
                //we failed to look at the last known player offsets, which isn't fatal.
                return false;
            }
        }

        private static List<int> FindUniqueOffset(byte[] gameMemoryBuffer, byte[] finderAoB, int resultLimit = -1)
        {
            int byteCount = 0;
            List<int> foundOffsets = new List<int>();
            try
            {
                while (byteCount + 1 < gameMemoryBuffer.Length)
                {
                    for (int position = 0; position < finderAoB.Length; position++)
                    {
                        //now filter any out that don't match with the finderAoB
                        if (gameMemoryBuffer[byteCount + position] != finderAoB[position])
                            break;
                        //if we get all the way through the scan without finding anything "wrong", we have a match
                        else if (position == finderAoB.Length - 1)
                        {
                            _logger.Debug($"Found AoB {BitConverter.ToString(finderAoB)} at {byteCount}!");
                            foundOffsets.Add(byteCount);
                        }
                    }

                    if (foundOffsets.Count == resultLimit)
                    {
                        _logger.Debug($"{resultLimit} matches were requested, and all were found.");
                        return foundOffsets;
                    }

                    byteCount ++; //2 bytes seems to be the maximum we can reliably go without missing offsets
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to find unique offset: {e}");
                throw new AggregateException($"Failed to find unique offset: ", e);
            }

            _logger.Debug($"{resultLimit} matches were requested, and only {foundOffsets.Count} were found.");
            return foundOffsets;
        }

        private static List<int> FindNewPlayerOffsets(byte[] buffer, int offsetsToFind)
        {
            int byteCount = 0;
            List<int> playerOffsets = new List<int>();
            try
            {
                while (byteCount + 152 < buffer.Length) //this can _probably just be 144 or 148, but i want to be safe
                {
                    bool mightBeValid = false;
                    for (int position = 0; position < MGS2AoB.PlayerInfoFinder.Length; position++)
                    {
                        //the "playerOffsetBytes" is very common within the game's memory. (~60-90 matches)
                        //HOWEVER, if we limit the playerOffsets bytes by the _relative position_, we get VERY few results!
                        //#1 if you have fired 0 shots, you will have 4 of these blocks of memory.
                        //#2 if you have fired 1+ shots but HAVE NOT checkpointed, you will have 3 blocks
                        //#3 if you have fired 1+ shots and HAVE checkpointed, you will have 2 blocks

                        //the playerOffsetBytes will have a mirrored result: one directly between the currentAmmo array and
                        //maxAmmo array, and another one directly before the start of currentItem array. They will be separated
                        //by EXACTLY 72 bytes(difference between currentAmmo and maxAmmo). Ignore any sets where the current
                        //byte value does not match with the byte value 72 bytes ahead.
                        if (buffer[byteCount + position + 72] != buffer[byteCount + position])
                            break;
                        //now filter any out that don't match with the playerOffsetBytes
                        if (buffer[byteCount + position] != MGS2AoB.PlayerInfoFinder[position])
                            break;
                        //if we get all the way through the scan without finding anything "wrong", we have a possible match
                        else if (position == MGS2AoB.PlayerInfoFinder.Length - 1)
                        {
                            mightBeValid = true;
                        }
                    }

                    if (mightBeValid)
                    {
                        byte[] bufferBeingExamined = new byte[MGS2AoB.PlayerInfoFinder.Length];
                        Array.Copy(buffer, byteCount + 144, bufferBeingExamined, 0, MGS2AoB.PlayerInfoFinder.Length);

                        //to filter out scenarios #1 and #2 above, for all of the possible matches, check 144 bytes ahead.
                        //ONLY if we are matching on a file with 0 shots OR 0 shots at last checkpoint can there be a full match
                        //144 bytes ahead. if at ANY point in the 144 bytes after each position in the offset array we're scanning
                        //there is a value that DOES NOT MATCH, then we know we have a real player offset.
                        for (int position = 0; position < MGS2AoB.PlayerInfoFinder.Length; position++)
                        {
                            if (bufferBeingExamined[position] != MGS2AoB.PlayerInfoFinder[position])
                            {
                                playerOffsets.Add(byteCount);
                                break;
                            }
                        }
                    }

                    if (playerOffsets.Count == offsetsToFind)
                        break;

                    byteCount += 4; //8 bytes can result in missed offsets, 4 bytes is sufficient in accuracy & speed.
                }

                return playerOffsets;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to find player offset: {e}");
                throw new AggregateException($"Failed to find player offset: ", e);
            }
        }

        private static List<int> GetPlayerOffsets(Constants.PlayableCharacter character)
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    byte[] gameMemoryBuffer = proxy.GetProcessSnapshot();

                    //if we've retrieved a player offset before, check the old one first
                    if (_lastKnownPlayerOffsets != default)
                    {
                        if (ValidateLastKnownOffsets(gameMemoryBuffer, _lastKnownPlayerOffsets, MGS2AoB.PlayerInfoFinder))
                        {
                            _logger.Verbose("Last known player offsets are valid, reusing...");
                            return _lastKnownPlayerOffsets;
                        }
                    }

                    _lastKnownPlayerOffsets = new List<int>();
                    int offsetsToFind = 2;
                    Stage currentStage = GetStage();
                    _logger.Debug($"User is in {currentStage}");
                    if (StageNames.VRStages.PlayableStageList.Contains(currentStage))
                    {
                        _logger.Debug("Looks like this is a VR stage, will now only search for 1 player offset.");
                        offsetsToFind = 1;
                    }
                    List<int> playerOffsets = FindNewPlayerOffsets(gameMemoryBuffer, offsetsToFind);

                    _lastKnownPlayerOffsets = new List<int>(playerOffsets);
                    return _lastKnownPlayerOffsets;
                }
            }
        }

        private static byte[] ReadValueFromMemory(int offset, long bytesToRead = default)
        {
            if(bytesToRead == default)
            {
                bytesToRead = 2;
            }

            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    try
                    {
                        byte[] bytesRead = proxy.ReadProcessOffset(offset, bytesToRead);
                        if (bytesRead.Length != bytesToRead)
                        {
                            _logger.Warning($"Expected to read {bytesToRead}, but we actually read {bytesRead.Length}");
                            throw new FileLoadException($"Failed to read value at offset {offset}.");
                        }

                        return bytesRead;
                    }
                    catch (SimpleProcessProxyException e)
                    {
                        _logger.Error($"Failed to read memory: {e}");
                        throw e;
                    }
                }
            }
        }

        private static void InvertBooleanValue(int playerOffset, int objectOffset)
        {
            int combinedOffset = playerOffset + objectOffset;
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        _logger.Information($"Inverting boolean value at {combinedOffset}...");
                        proxy.InvertBooleanValue(combinedOffset, sizeof(short));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to invert boolean at offset {playerOffset}+{objectOffset}: {e}");
                throw new AggregateException("Could not invert boolean", e);
            }
        }

        private static string GetCharacterCode()
        {
            List<int> stageMemoryOffsets = GetStageOffsets();
            string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(stageMemoryOffsets.First() + MGS2Offset.CURRENT_CHARACTER.Start, MGS2Offset.CURRENT_CHARACTER.Length));

            return stringInMemory;
        }

        internal static Stage GetStage()
        {
            List<int> stageMemoryOffsets = GetStageOffsets();
            string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(stageMemoryOffsets.First() + MGS2Offset.CURRENT_STAGE.Start, MGS2Offset.CURRENT_STAGE.Length));

            Stage currentStage = Stage.Parse(stringInMemory);
            _logger.Verbose($"User is currently in stage: {stringInMemory}. Parsed as {currentStage}");
            return currentStage;
        }

        private static void SetStringValue(int stringOffset, string valueToSet)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        _logger.Information($"setting memory at offset {stringOffset} to {valueToSet}...");
                        proxy.ModifyProcessOffset(stringOffset, valueToSet, true);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to set string at offset {stringOffset}: {e}");
                throw new AggregateException($"Could not set string at offset {stringOffset}", e);
            }
        }

        private static void SetPlayerOffsetBasedByteValueObject(int objectOffset, byte[] valueToSet, Constants.PlayableCharacter character)
        {
            //TODO: this is kind of gross that this is hardcoded to be playeroffset only... i would like to fix that.
            try
            {
                List<int> playerOffsets = GetPlayerOffsets(character);

                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        foreach (int offset in playerOffsets)
                        {
                            _logger.Information($"setting playerOffsetBased value at offset: {offset} to {BitConverter.ToString(valueToSet)}...");
                            proxy.ModifyProcessOffset(offset + objectOffset, valueToSet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to set memory at offset {objectOffset}: {e}");
                throw new AggregateException($"Could not set memory at offset {objectOffset}", e);
            }
        }

        private static byte[] ReadAoBOffsetValue(byte[] arrayOfBytes, MemoryOffset memoryOffset)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        byte[] processSnapshot = proxy.GetProcessSnapshot();

                        int aobOffset = FindUniqueOffset(processSnapshot, arrayOfBytes).First();
                        return proxy.ReadProcessOffset(aobOffset + memoryOffset.Start, memoryOffset.Length);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to read memory AoB offset");
                throw new AggregateException($"Could not read memory AoB offset", e);
            }
        }

        private static void SetAoBOffsetValue(byte[] arrayOfBytes, MemoryOffset memoryOffset, dynamic valueToSet)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        byte[] processSnapshot = proxy.GetProcessSnapshot();

                        int aobOffset = FindUniqueOffset(processSnapshot, arrayOfBytes).First();
                        proxy.ModifyProcessOffset(aobOffset + memoryOffset.Start, valueToSet, true);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to set memory AoB offset");
                throw new AggregateException($"Could not set memory AoB offset", e);
            }
        }
        #endregion

        public static void UpdateGameString(MGS2Strings.MGS2String gameString, string newValue)
        {
            _logger.Debug($"Attempting to set string {gameString.Tag} to {newValue}...");
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    byte[] gameMemoryBuffer = proxy.GetProcessSnapshot();

                    List<int> offsets = FindUniqueOffset(gameMemoryBuffer, gameString.FinderAoB);

                    SetStringValue(offsets[0] + gameString.MemoryOffset.Start, newValue);
                }
            }
        }

        public static string ReadGameString(MGS2Strings.MGS2String gameString)
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    byte[] gameMemoryBuffer = proxy.GetProcessSnapshot();

                    List<int> offsets = FindUniqueOffset(gameMemoryBuffer, gameString.FinderAoB);

                    byte[] memoryValue = ReadValueFromMemory(offsets[0] + gameString.MemoryOffset.Start, gameString.MemoryOffset.Length);

                    return Encoding.UTF8.GetString(memoryValue);
                }
            }
        }

        public static byte[] GetPlayerInfoBasedValue(int valueOffset, int sizeToRead, Constants.PlayableCharacter character)
        {
            List<int> playerMemoryOffsets = GetPlayerOffsets(character);

            return ReadValueFromMemory(playerMemoryOffsets[0] + valueOffset, sizeToRead);
        }

        public static void UpdateObjectBaseValue(MGS2Object mgs2Object, short value, Constants.PlayableCharacter character)
        {
            switch (mgs2Object)
            {
                case StackableItem stackableItem:
                    _logger.Debug($"mgs2Object parsed as StackableItem, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(stackableItem.CurrentCountOffset, BitConverter.GetBytes(value), character);
                    break;
                case DurabilityItem durabilityItem:
                    _logger.Debug($"mgs2Object parsed as DurabilityItem, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(durabilityItem.DurabilityOffset, BitConverter.GetBytes(value), character);
                    break;
                case AmmoWeapon ammoWeapon:
                    _logger.Debug($"mgs2Object parsed as AmmoWeapon, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(ammoWeapon.CurrentAmmoOffset, BitConverter.GetBytes(value), character);
                    break;
                case SpecialWeapon specialWeapon:
                    _logger.Debug($"mgs2Object parsed as SpecialWeapon, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(specialWeapon.SpecialOffset, BitConverter.GetBytes(value), character);
                    break;
                case LevelableItem levelableItem:
                    _logger.Debug($"mgs2Object parsed as LevelableItem, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(levelableItem.LevelOffset, BitConverter.GetBytes(value), character);
                    break;
                case BasicItem basicItem:
                    _logger.Debug($"mgs2Object parsed as BasicItem, setting base value to: {value}");
                    SetPlayerOffsetBasedByteValueObject(basicItem.InventoryOffset, BitConverter.GetBytes(value), character);
                    break;
            }
        }

        public static void UpdateObjectMaxValue(MGS2Object mgs2Object, short count, Constants.PlayableCharacter character)
        {
            switch (mgs2Object)
            {
                case StackableItem stackableItem:
                    _logger.Debug($"mgs2Object parsed as StackableItem, setting max count to: {count}");
                    SetPlayerOffsetBasedByteValueObject(stackableItem.MaxCountOffset, BitConverter.GetBytes(count), character); 
                    break;
                case AmmoWeapon ammoWeapon:
                    _logger.Debug($"mgs2Object parsed as AmmoWeapon, setting max count to: {count}");
                    SetPlayerOffsetBasedByteValueObject(ammoWeapon.MaxAmmoOffset, BitConverter.GetBytes(count), character);
                    break;
            }
        }

        public static void ToggleObject(MGS2Object mgs2Object, Constants.PlayableCharacter character)
        {
            int objectOffset = mgs2Object.InventoryOffset;
            _logger.Debug($"Attempting to toggle {mgs2Object.Name} for {character}...");
            List<int> playerOffsets = GetPlayerOffsets(character);

            foreach(int playerOffset in playerOffsets)
            {
                short currentValue = BitConverter.ToInt16(ReadValueFromMemory(playerOffset + objectOffset, sizeof(short)), 0);
                if (currentValue == -1)
                    UpdateObjectBaseValue(mgs2Object, 1, character);
                else
                    UpdateObjectBaseValue(mgs2Object, -1, character);
            }
        }

        public static GameStats ReadGameStats()
        {
            _logger.Verbose("Reading game stats...");
            int stageOffset = GetStageOffsets().First();
            byte[] gameStatsBytes = ReadValueFromMemory(stageOffset + MGS2Offset.GAME_STATS_BLOCK.Start, MGS2Offset.GAME_STATS_BLOCK.Length);
            short continues = BitConverter.ToInt16(gameStatsBytes, 4);
            short saves = BitConverter.ToInt16(gameStatsBytes, 8);
            int playTime = BitConverter.ToInt32(gameStatsBytes, 10);
            short mechsDestroyed = BitConverter.ToInt16(gameStatsBytes, 42);
            short shots = BitConverter.ToInt16(gameStatsBytes, 18);
            short alerts = BitConverter.ToInt16(gameStatsBytes, 20);
            short kills = BitConverter.ToInt16(gameStatsBytes, 22);
            short damageTaken = BitConverter.ToInt16(gameStatsBytes, 24);
            byte[] rationsUsedBytes = ReadValueFromMemory(stageOffset + MGS2Offset.RATIONS_USED.Start, MGS2Offset.RATIONS_USED.Length);
            short rationsUsed = BitConverter.ToInt16(rationsUsedBytes, 0);
            byte[] specialItemsBytes = ReadValueFromMemory(stageOffset + MGS2Offset.SPECIAL_ITEMS_USED.Start, MGS2Offset.SPECIAL_ITEMS_USED.Length);
            short specialItems = BitConverter.ToInt16(specialItemsBytes, 0);

            GameStats gameStats = new GameStats
            {
                Continues = continues,
                Kills = kills,
                DamageTaken = damageTaken,
                PlayTime = playTime,
                Rations = rationsUsed,
                Saves = saves,
                Shots = shots,
                SpecialItems = specialItems,
                Alerts = alerts,
                MechsDestroyed = mechsDestroyed
            };

            _logger.Verbose($"Current game stats: {gameStats}");

            return gameStats;
        }

        public static Constants.PlayableCharacter DetermineActiveCharacter()
        {
            string characterCode = GetCharacterCode();
            _logger.Debug($"Found character: {characterCode}");

            if (characterCode.Contains("tnk") || characterCode.Contains("r_vr_s"))
            {
                _logger.Verbose("Currently playing as Snake");
                return Constants.PlayableCharacter.Snake;
            }
            else if (characterCode.Contains("plt"))
            {
                _logger.Verbose("Currently playing as Raiden");
                return Constants.PlayableCharacter.Raiden;
            }
            else if (characterCode.Contains("vr_1"))
            {
                _logger.Verbose("Currently playing as MGS1 Snake");
                return Constants.PlayableCharacter.MGS1Snake;
            }
            else if (characterCode.Contains("r_vr_t"))
            {
                _logger.Verbose("Currently playing as Tuxedo Snake");
                return Constants.PlayableCharacter.TuxedoSnake;
            }
            else if (characterCode.Contains("r_vr_p"))
            {
                _logger.Verbose("Currently playing as Pliskin");
                return Constants.PlayableCharacter.Pliskin;
            }
            else if (characterCode.Contains("r_vr_b"))
            {
                _logger.Verbose("Currently playing as Ninja Raiden");
                return Constants.PlayableCharacter.NinjaRaiden;
            }
            else if (characterCode.Contains("r_vr_x"))
            {
                _logger.Verbose("Currently playing as Naked Raiden");
                return Constants.PlayableCharacter.NakedRaiden;
            }
            else
            {
                _logger.Warning("Unable to determine what the active character is!");
                throw new NotImplementedException("Unknown stage! Can't safely determine what the active character is");
            }
        }
    }
}
