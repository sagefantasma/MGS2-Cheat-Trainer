using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimplifiedMemoryManager;
using MGS2_MC.Helpers;

namespace MGS2_MC
{
    internal static class MGS2MemoryManager
    {
        #region Internals
        private const string loggerName = "MemoryManagerDebuglog.log";

        private static List<IntPtr> _lastKnownPlayerOffsets { get; set; } = default;
        private static List<IntPtr> _lastKnownStageOffsets { get; set; } = default;
        private static ILogger _logger { get; set; }

        internal static void StartLogger()
        {
            _logger = Logging.InitializeNewLogger(loggerName);
            _logger.Information($"Memory Manager for version {Program.AppVersion} initialized...");
            _logger.Verbose($"Instance ID: {Program.InstanceID}");
        }

        public class GameStats
        {
            public enum ModifiableStats
            {
                Alerts,
                Continues,
                DamageTaken,
                Kills,
                MechsDestroyed,
                Rations,
                Saves,
                Shots
            }

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

        private static List<IntPtr> GetStageOffsets()
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    if(_lastKnownStageOffsets != default)
                    {
                        if (ValidateLastKnownOffsets(proxy, _lastKnownStageOffsets, MGS2AoB.StageInfo))
                        {
                            _logger.Verbose($"Last known stageOffsets are still valid, reusing...");
                            return _lastKnownStageOffsets;
                        }
                    }
                    SimplePattern stageOffsetPattern = new SimplePattern(MGS2AoB.StageInfoString);
                    List<IntPtr> stageOffsets = proxy.ScanMemoryForPattern(stageOffsetPattern);

                    _logger.Verbose($"We found {stageOffsets.Count} stage offsets in memory");

                    //ignore all results except for the final two if more than 2 are found.
                    if (stageOffsets.Count > 1)
                        stageOffsets = stageOffsets.GetRange(stageOffsets.Count - 2, 2);

                    _lastKnownStageOffsets = new List<IntPtr>(stageOffsets);
                    return _lastKnownStageOffsets;
                }
            }
        }

        private static bool ValidateLastKnownOffsets(SimpleProcessProxy proxy, List<IntPtr> lastKnownOffsets, byte[] finderAoB)
        {
            try
            {
                bool lastKnownAreValid = true;
                foreach (IntPtr stageOffset in lastKnownOffsets)
                {
                    byte[] currentBytesAtLastKnown = proxy.ReadProcessOffset(stageOffset, finderAoB.Length);
                    if (!currentBytesAtLastKnown.SequenceEqual(finderAoB))
                    {
                        _logger.Verbose($"Last known offset at {stageOffset} has changed since we last looked!");
                        lastKnownAreValid = false;
                    }
                }

                _logger.Verbose($"Last known offset(s) are still valid.");
                return lastKnownAreValid;
            }
            catch (Exception e)
            {
                _logger.Warning($"Something unexpected went wrong when looking at the last known offsets: {e}");
                //we failed to look at the last known offsets, which isn't fatal.
                return false;
            }
        }

        private static List<IntPtr> GetPlayerOffsets(Constants.PlayableCharacter character)
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    if(_lastKnownPlayerOffsets != default)
                    {
                        if (character == Constants.PlayableCharacter.Raiden) {
                            if (ValidateLastKnownOffsets(proxy, _lastKnownPlayerOffsets, MGS2AoB.PlayerInfoFinderRaiden))
                            {
                                _logger.Verbose($"Last known playerOffsets are still valid, reusing...");
                                return _lastKnownPlayerOffsets;
                            }
                        }
                        else if(character == Constants.PlayableCharacter.Snake)
                        {
                            if (ValidateLastKnownOffsets(proxy, _lastKnownPlayerOffsets, MGS2AoB.PlayerInfoFinderSnake))
                            {
                                _logger.Verbose($"Last known playerOffsets are still valid, reusing...");
                                return _lastKnownPlayerOffsets;
                            }
                        }
                        else
                        {
                            if (ValidateLastKnownOffsets(proxy, _lastKnownPlayerOffsets, MGS2AoB.PlayerInfoFinderGeneric))
                            {
                                _logger.Verbose($"Last known playerOffsets are still valid, reusing...");
                                return _lastKnownPlayerOffsets;
                            }
                        }
                    }

                    _lastKnownPlayerOffsets = new List<IntPtr>();
                    
                    SimplePattern pattern = new SimplePattern(MGS2AoB.PlayerInfoFinderString);
                    List<IntPtr> playerOffsets = proxy.ScanMemoryForPattern(pattern);

                    _lastKnownPlayerOffsets = new List<IntPtr>(playerOffsets);
                    return _lastKnownPlayerOffsets;
                }
            }
        }

        private static byte[] ReadValueFromMemory(IntPtr memoryLocation, long bytesToRead = default)
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
                        byte[] bytesRead = proxy.ReadProcessOffset(memoryLocation, bytesToRead);
                        if (bytesRead.Length != bytesToRead)
                        {
                            _logger.Warning($"Expected to read {bytesToRead}, but we actually read {bytesRead.Length}");
                            throw new FileLoadException($"Failed to read value at memoryLocation {memoryLocation}.");
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
                        proxy.InvertBooleanValue(new IntPtr(combinedOffset), sizeof(short));
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
            List<IntPtr> stageMemoryOffsets = GetStageOffsets();
            string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(stageMemoryOffsets.First() + MGS2Offset.CURRENT_CHARACTER.Start, MGS2Offset.CURRENT_CHARACTER.Length));

            return stringInMemory;
        }

        internal static Stage GetStage()
        {
            List<IntPtr> stageMemoryOffsets = GetStageOffsets();
            string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(stageMemoryOffsets.First() + MGS2Offset.CURRENT_STAGE.Start, MGS2Offset.CURRENT_STAGE.Length));

            Stage currentStage = Stage.Parse(stringInMemory);
            _logger.Verbose($"User is currently in stage: {stringInMemory}. Parsed as {currentStage}");
            return currentStage;
        }

        private static void SetStringValue(IntPtr stringOffset, string valueToSet)
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
                List<IntPtr> playerOffsets = GetPlayerOffsets(character);

                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        foreach (int offset in playerOffsets)
                        {
                            _logger.Information($"setting playerOffsetBased value at offset: {offset} to {BitConverter.ToString(valueToSet)}...");
                            proxy.ModifyProcessOffset(new IntPtr(offset + objectOffset), valueToSet);
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

        private static void SetKnownOffsetValue(IntPtr offset, byte[] valueToSet)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        _logger.Information($"Setting known offset value at offset: {offset} to {BitConverter.ToString(valueToSet)}...");
                        proxy.ModifyProcessOffset(offset, valueToSet, true);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to set memory at offset {offset}: {e}");
                throw new AggregateException($"Could not set memory at offset {offset}", e);
            }
        }

        private static void SetKnownOffsetValue(IntPtr offset, byte valueToSet)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        _logger.Information($"Setting known offset value at offset: {offset} to {valueToSet}...");
                        proxy.ModifyProcessOffset(offset, valueToSet, true);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to set memory at offset {offset}: {e}");
                throw new AggregateException($"Could not set memory at offset {offset}", e);
            }
        }

        private static byte[] ReadAoBOffsetValue(string byteString, MemoryOffset memoryOffset)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        IntPtr memoryLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(byteString));
                        return proxy.ReadProcessOffset(IntPtr.Add(memoryLocation, memoryOffset.Start), memoryOffset.Length);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to read memory AoB offset");
                throw new AggregateException($"Could not read memory AoB offset", e);
            }
        }

        private static void SetAoBOffsetValue(string byteString, MemoryOffset memoryOffset, dynamic valueToSet)
        {
            try
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        IntPtr memoryLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(byteString));
                        proxy.ModifyProcessOffset(IntPtr.Add(memoryLocation, memoryOffset.Start), valueToSet, true);
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
                    IntPtr offset = proxy.ScanMemoryForUniquePattern(new SimplePattern(gameString.FinderAoB));

                    SetStringValue(IntPtr.Add(offset, gameString.MemoryOffset.Start), newValue);
                }
            }
        }

        public static string ReadGameString(MGS2Strings.MGS2String gameString)
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    IntPtr offset = proxy.ScanMemoryForUniquePattern(new SimplePattern(gameString.FinderAoB));

                    byte[] memoryValue = ReadValueFromMemory(IntPtr.Add(offset, gameString.MemoryOffset.Start), gameString.MemoryOffset.Length);

                    return Encoding.UTF8.GetString(memoryValue);
                }
            }
        }

        public static byte[] GetPlayerInfoBasedValue(int valueOffset, int sizeToRead, Constants.PlayableCharacter character)
        {
            List<IntPtr> playerMemoryOffsets = GetPlayerOffsets(character);
            return ReadValueFromMemory(IntPtr.Add(playerMemoryOffsets[0], valueOffset), sizeToRead);
        }

        public static void UpdateObjectBaseValue(MGS2Object mgs2Object, ushort value, Constants.PlayableCharacter character)
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

        public static void UpdateObjectMaxValue(MGS2Object mgs2Object, ushort count, Constants.PlayableCharacter character)
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

        public static void ToggleObject(MGS2Object mgs2Object, Constants.PlayableCharacter character, bool enable = true)
        {
            _logger.Debug($"Attempting to toggle {mgs2Object.Name} for {character}...");

            if (enable)
                UpdateObjectBaseValue(mgs2Object, 1, character);
            else
            {
                if (mgs2Object is BasicItem)
                    UpdateObjectBaseValue(mgs2Object, 0, character);
                else
                    UpdateObjectBaseValue(mgs2Object, ushort.MaxValue, character);
            }
            
        }

        public static GameStats ReadGameStats()
        {
            _logger.Verbose("Reading game stats...");
            IntPtr stageOffset = GetStageOffsets().First();
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

        public static void ChangeGameStat(GameStats.ModifiableStats gameStat, short value)
        {
            IntPtr stageOffset = GetStageOffsets().First();
            MemoryOffset gameStatOffset;
            switch (gameStat)
            {
                case GameStats.ModifiableStats.Alerts:
                    gameStatOffset = MGS2Offset.ALERT_COUNT;
                    break;
                case GameStats.ModifiableStats.Continues:
                    gameStatOffset = MGS2Offset.CONTINUE_COUNT;
                    break;
                case GameStats.ModifiableStats.DamageTaken:
                    gameStatOffset = MGS2Offset.DAMAGE_TAKEN;
                    break;
                case GameStats.ModifiableStats.Kills:
                    gameStatOffset = MGS2Offset.KILL_COUNT;
                    break;
                case GameStats.ModifiableStats.MechsDestroyed:
                    gameStatOffset = MGS2Offset.MECHS_DESTROYED;
                    break;
                case GameStats.ModifiableStats.Rations:
                    gameStatOffset = MGS2Offset.RATIONS_USED;
                    break;
                case GameStats.ModifiableStats.Saves:
                    gameStatOffset = MGS2Offset.SAVE_COUNT;
                    break;
                case GameStats.ModifiableStats.Shots:
                    gameStatOffset = MGS2Offset.SHOT_COUNT;
                    break;
                default:
                    throw new Exception("You must provide a valid game stat to modify");
            }

            SetKnownOffsetValue(stageOffset + gameStatOffset.Start, (byte) value);
        }

        public static Difficulty ReadCurrentDifficulty()
        {
            IntPtr stageOffset = GetStageOffsets().First();
            byte[] difficultyByte = ReadValueFromMemory(stageOffset + MGS2Offset.CURRENT_DIFFICULTY.Start, MGS2Offset.CURRENT_DIFFICULTY.Length);

            int convertedDifficulty = difficultyByte[0];
            
            return (Difficulty) convertedDifficulty;
        }

        public static GameType ReadGameType()
        {
            IntPtr stageOffset = GetStageOffsets().First();
            byte[] gameTypeByte = ReadValueFromMemory(stageOffset + MGS2Offset.CURRENT_GAMETYPE.Start, MGS2Offset.CURRENT_GAMETYPE.Length);

            int convertedGameType = gameTypeByte[0];

            return (GameType)convertedGameType;
        }

        public static ushort GetCurrentHP()
        {
            IntPtr stageOffset = GetStageOffsets().First();
            byte[] currentHpBytes = ReadValueFromMemory(stageOffset + MGS2Offset.CURRENT_HP.Start, MGS2Offset.CURRENT_HP.Length);

            return BitConverter.ToUInt16(currentHpBytes, 0);
        }

        public static ushort GetCurrentMaxHP()
        {
            IntPtr stageOffset = GetStageOffsets().First();
            byte[] currentMaxHpBytes = ReadValueFromMemory(stageOffset + MGS2Offset.CURRENT_MAX_HP.Start, MGS2Offset.CURRENT_MAX_HP.Length);

            return BitConverter.ToUInt16(currentMaxHpBytes, 0);
        }

        public static ushort GetCurrentGripGauge()
        {
            lock (MGS2Monitor.MGS2Process)
            {
                using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                {
                    IntPtr pointerLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(MGS2AoB.CurrentGripGauge));
                    byte[] locationPointedTo = proxy.ReadProcessOffset(pointerLocation, 8);
                    long locationPointedToLong = BitConverter.ToInt64(locationPointedTo, 0);
                    locationPointedToLong += MGS2Offset.CURRENT_GRIP_GAUGE.Start; //add the offset to find the grip gauge
                    byte[] gripGauge = proxy.GetMemoryFromPointer(new IntPtr(locationPointedToLong), MGS2Offset.CURRENT_GRIP_GAUGE.Length);

                    return BitConverter.ToUInt16(gripGauge, 0);
                }
            }
        }

        public static ushort ModifyGripLevel(bool increase)
        {
            try
            {
                Constants.PlayableCharacter currentCharacter = DetermineActiveCharacter();

                lock (MGS2Monitor.MGS2Process)
                {
                    using (SimpleProcessProxy proxy = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        IntPtr memoryLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(MGS2AoB.PlayerInfoFinderString));

                        if (currentCharacter == Constants.PlayableCharacter.Snake)
                            memoryLocation = IntPtr.Add(memoryLocation, MGS2Offset.GRIP_LEVEL_SNAKE.Start);
                        else
                            memoryLocation = IntPtr.Add(memoryLocation, MGS2Offset.GRIP_LEVEL_RAIDEN.Start);

                        byte[] gripLevelBytes = proxy.ReadProcessOffset(memoryLocation, 2);
                        ushort gripLevel = BitConverter.ToUInt16(gripLevelBytes, 0);

                        switch (increase)
                        {
                            default:
                            case true:
                                if (gripLevel < 200)
                                {
                                    proxy.ModifyProcessOffset(memoryLocation, gripLevel+=100);
                                }
                                return gripLevel;
                            case false:
                                //this, unfortunately, doesn't seem to actually cause the grip level to change... annoying
                                if (gripLevel > 0 && gripLevel >= 100)
                                {
                                    proxy.ModifyProcessOffset(memoryLocation, gripLevel-=100);
                                }
                                else
                                {
                                    proxy.ModifyProcessOffset(memoryLocation, 0);
                                }
                                return gripLevel;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Failed to modify grip level: {e}");
                return ushort.MaxValue;
            }
        }

        public static Constants.PlayableCharacter DetermineActiveCharacter()
        {
            string characterCode = GetCharacterCode();
            _logger.Debug($"Found character: {characterCode}");

            if (characterCode.Contains("tnk") || characterCode.Contains("r_vr_s"))
            {
                _logger.Verbose("Currently playing as Snake");
                if (characterCode.Contains("tnk"))
                    return Constants.PlayableCharacter.Snake;
                else
                    return Constants.PlayableCharacter.Pliskin; //technically you're not playing as Pliskin, but this fixes the VR/Snake tales issue for Snake
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
