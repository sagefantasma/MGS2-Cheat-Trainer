using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MGS2_CheatTrainer_V2.Models;
using MGS2_CheatTrainer_V2.Models.PlayableCharacters;
using SimplifiedMemoryManager;

namespace MGS2_CheatTrainer_V2
{
    //REWRITE STATUS: Surely needs more than what I've done already?
    internal class Mgs2MemoryManager : IDisposable
    {
        #region Internals
        private static List<IntPtr>? LastKnownStageOffsets { get; set; }
        private static ILogger? Logger => Logging.Logger;
        #endregion

        #region Private methods
        internal Constants.PlayableCharacter CheckIfUsable(Constants.IMgs2Object mgs2Object)
        {
            try
            {
                Constants.PlayableCharacter currentPc = DetermineActiveCharacter();
                switch (currentPc)
                {
                    case Constants.PlayableCharacter.Snake:
                        if (!Snake.UsableObjects.Contains(mgs2Object))
                        {
                            Logger?.Warning($"Snake cannot use {mgs2Object.Name}");
                            throw new InvalidOperationException($"Snake cannot use {mgs2Object.Name}");
                        }
                        break;
                    case Constants.PlayableCharacter.Raiden:
                        if (!Raiden.UsableObjects.Contains(mgs2Object))
                        {
                            Logger?.Warning($"Raiden cannot use {mgs2Object.Name}");
                            throw new InvalidOperationException($"Raiden cannot use {mgs2Object.Name}");
                        }
                        break;
                }

                return currentPc;
            }
            catch (Exception e)
            {
                Logger?.Error($"Could not check if {mgs2Object.Name} is usable: {e}");
                throw new AggregateException("Failed to check if item is usable", e);
            }
        }

        private static IntPtr GetCurrentStageOffset()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process == null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);

                    return IntPtr.Add(memoryLocation, Mgs2Offset.CurrentStage.Start);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Could not get current stage offset: {e}");
                throw new AggregateException("Failed to get current stage offset", e);
            }
        }
        
        [Obsolete("This is inefficient and slow, use GetCurrentStageOffset instead.")]
        private static List<IntPtr> GetStageOffsets()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    if (LastKnownStageOffsets != null)
                    {
                        if (ValidateLastKnownOffsets(proxy, LastKnownStageOffsets, Mgs2AoB.StageInfo))
                        {
                            Logger?.Verbose($"Last known stageOffsets are still valid, reusing...");
                            return LastKnownStageOffsets;
                        }
                    }

                    SimplePattern stageOffsetPattern = new SimplePattern(Mgs2AoB.StageInfoString);
                    List<SimpleProcessProxy.SimpleMemory> stageOffsets =
                        proxy.ScanMemoryForPattern(stageOffsetPattern);
                    List<IntPtr> stageOffsetPtrs = stageOffsets.Select(sm => sm.Offset).ToList();

                    Logger?.Verbose($"We found {stageOffsets.Count} stage offsets in memory");

                    //ignore all results except for the final two if more than 2 are found.
                    if (stageOffsetPtrs.Count > 1)
                        stageOffsetPtrs = stageOffsetPtrs.GetRange(stageOffsetPtrs.Count - 2, 2);

                    LastKnownStageOffsets = new List<IntPtr>(stageOffsetPtrs);
                    return LastKnownStageOffsets;
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Could not get stage offsets: {e}");
                throw new AggregateException("Failed to get stage offsets", e);
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
                        Logger?.Verbose($"Last known offset at {stageOffset} has changed since we last looked!");
                        lastKnownAreValid = false;
                    }
                }

                Logger?.Verbose($"Last known offset(s) are still valid.");
                return lastKnownAreValid;
            }
            catch (Exception e)
            {
                Logger?.Warning($"Something unexpected went wrong when looking at the last known offsets: {e}");
                //we failed to look at the last known offsets, which isn't fatal.
                return false;
            }
        }

        private static byte[] ReadValueFromMemory(IntPtr memoryLocation, long bytesToRead = 2)
        {
            if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game"); 
            lock (Mgs2Monitor.Mgs2Process)
            {
                using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                try
                {
                    //byte[] bytesRead = proxy.ReadProcessOffset(memoryLocation, bytesToRead);
                    byte[] bytesRead = proxy.GetMemoryFromPointer(memoryLocation, (int)bytesToRead);
                    if (bytesRead.Length != bytesToRead)
                    {
                        Logger?.Warning($"Expected to read {bytesToRead}, but we actually read {bytesRead.Length}");
                        throw new FileLoadException($"Failed to read value at memoryLocation {memoryLocation}.");
                    }

                    return bytesRead;
                }
                catch (SimpleProcessProxyException e)
                {
                    Logger?.Error($"Failed to read memory: {e}");
                    throw new AggregateException("Failed to read memory", e);
                }
            }
        }

        private static void InvertBooleanValue(int playerOffset, int objectOffset)
        {
            int combinedOffset = playerOffset + objectOffset;
            try
            {
                if(Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    Logger?.Information($"Inverting boolean value at {combinedOffset}...");
                    proxy.InvertBooleanValue(new IntPtr(combinedOffset), sizeof(short));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to invert boolean at offset {playerOffset}+{objectOffset}: {e}");
                throw new AggregateException("Could not invert boolean", e);
            }
        }

        private static string GetCharacterCode()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.CurrentCharacter.Start,
                        Mgs2Offset.CurrentCharacter.Length));

                    return stringInMemory;
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Could not get character code: {e}");
                throw new AggregateException("Failed to get character code", e);
            }
        }

        internal Stage GetStage()
        {
            try
            {
                IntPtr stageMemoryOffset = GetCurrentStageOffset();
                string stringInMemory = Encoding.UTF8.GetString(ReadValueFromMemory(stageMemoryOffset, 4));

                Stage currentStage = Stage.Parse(stringInMemory);
                Logger?.Verbose($"User is currently in stage: {stringInMemory}. Parsed as {currentStage}");
                return currentStage;
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get stage: {e}");
                throw new AggregateException($"Could not get stage", e);
            }
        }

        private void SetStringValue(IntPtr stringOffset, string valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    Logger?.Information($"setting memory at offset {stringOffset} to {valueToSet}...");
                    proxy.ModifyProcessOffset(stringOffset, valueToSet);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to set string at offset {stringOffset}: {e}");
                throw new AggregateException($"Could not set string at offset {stringOffset}", e);
            }
        }

        private static ushort GetPlayerOffsetBasedByteValueObject(int objectOffset)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr ammoOffset = proxy.FollowPointer(new IntPtr(Mgs2Pointer.CurrentAmmo), false);
                    Logger?.Information($"getting playerOffsetBased value at offset: {ammoOffset}+{objectOffset}...");
                    return BitConverter.ToUInt16(proxy.GetMemoryFromPointer(IntPtr.Add(ammoOffset, objectOffset), 2));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get memory at offset {objectOffset}: {e}");
                throw new AggregateException($"Could not get memory at offset {objectOffset}", e);
            }
        }

        private static void SetPlayerOffsetBasedByteValueObject(int objectOffset, byte[] valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr ammoOffset = proxy.FollowPointer(new IntPtr(Mgs2Pointer.CurrentAmmo), false);
                    Logger?.Information($"setting playerOffsetBased value at offset: {ammoOffset}+{objectOffset} to {BitConverter.ToString(valueToSet)}...");
                    proxy.SetMemoryAtPointer(IntPtr.Add(ammoOffset, objectOffset), valueToSet);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to set memory at offset {objectOffset}: {e}");
                throw new AggregateException($"Could not set memory at offset {objectOffset}", e);
            }
        }

        private static void SetPointerOffsetValue(IntPtr pointer, int offset, byte[] valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(pointer, false);
                    Logger?.Information($"setting pointerOffset value at offset: {pointerLocation}+{offset} to {BitConverter.ToString(valueToSet)}...");
                    proxy.SetMemoryAtPointer(IntPtr.Add(pointerLocation, offset), valueToSet);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to set memory at pointer offset [{pointer}]+{offset}: {e}");
                throw new AggregateException($"Could not set memory at pointer offset [{pointer}]+{offset}", e);
            }
        }

        private static void SetKnownStaticOffsetValue(IntPtr offset, byte[] valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    Logger?.Information($"Setting known offset value at offset: {offset} to {BitConverter.ToString(valueToSet)}...");
                    proxy.ModifyProcessOffset(offset, valueToSet, true);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to set memory at offset {offset}: {e}");
                throw new AggregateException($"Could not set memory at offset {offset}", e);
            }
        }

        private static void SetKnownStaticOffsetValue(IntPtr offset, byte valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    Logger?.Information($"Setting known offset value at offset: {offset} to {valueToSet}...");
                    proxy.ModifyProcessOffset(offset, valueToSet, true);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to set memory at offset {offset}: {e}");
                throw new AggregateException($"Could not set memory at offset {offset}", e);
            }
        }

        private static byte[] ReadAoBOffsetValue(string byteString, MemoryOffset memoryOffset)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(byteString)).Offset;
                    return proxy.ReadProcessOffset(IntPtr.Add(memoryLocation, memoryOffset.Start), memoryOffset.Length);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to read memory AoB offset");
                throw new AggregateException($"Could not read memory AoB offset", e);
            }
        }

        private static void SetAoBOffsetValue(string byteString, MemoryOffset memoryOffset, dynamic valueToSet)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryLocation = proxy.ScanMemoryForUniquePattern(new SimplePattern(byteString)).Offset;
                    proxy.ModifyProcessOffset(IntPtr.Add(memoryLocation, memoryOffset.Start), valueToSet, true);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to set memory AoB offset");
                throw new AggregateException($"Could not set memory AoB offset", e);
            }
        }
        #endregion

        public void UpdateGameString(Mgs2Strings.Mgs2String gameString, string newValue)
        {
            try
            {
                Logger?.Debug($"Attempting to set string {gameString.Tag} to {newValue}...");
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr offset = proxy.ScanMemoryForUniquePattern(new SimplePattern(gameString.FinderAoB)).Offset;

                    SetStringValue(IntPtr.Add(offset, gameString.MemoryOffset.Start), newValue);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to update game string for {gameString.Tag}: {e}");
                throw new AggregateException($"Could not update game string for {gameString.Tag}", e);
            }
        }

        public string ReadGameString(Mgs2Strings.Mgs2String gameString)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr offset = proxy.ScanMemoryForUniquePattern(new SimplePattern(gameString.FinderAoB)).Offset;

                    byte[] memoryValue = ReadValueFromMemory(IntPtr.Add(offset, gameString.MemoryOffset.Start), gameString.MemoryOffset.Length);

                    return Encoding.UTF8.GetString(memoryValue);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to read the game string for {gameString.Tag}: {e}");
                throw new AggregateException($"Could not read game string for {gameString.Tag}", e);
            }
        }

        public static byte[] GetPlayerInfoBasedValue(int valueOffset, int sizeToRead, Constants.PlayableCharacter character)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr ammoOffset = proxy.FollowPointer(new IntPtr(Mgs2Pointer.CurrentAmmo), false);
                    return proxy.GetMemoryFromPointer(IntPtr.Add(ammoOffset, valueOffset), sizeToRead);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get player info based value: {e}");
                throw new AggregateException($"Could not get player info based value", e);
            }
        }

        public void UpdateObjectBaseValue(Constants.IMgs2Object mgs2Object, ushort value, bool force = false)
        {
            try
            {
                Constants.PlayableCharacter character = DetermineActiveCharacter();
                
                switch (mgs2Object)
                {
                    case Constants.MaxableItem maxableItem:
                        Logger?.Debug($"mgs2Object parsed as MaxableItem, setting base value to: {value}");
                        SetPlayerOffsetBasedByteValueObject(maxableItem.Index + Mgs2Offset.BaseItem.Start, BitConverter.GetBytes(value));
                        break;
                    case Constants.SpecialItem specialItem:
                        Logger?.Debug($"mgs2Object parsed as SpecialItem, setting base value to: {value}");
                        SetPlayerOffsetBasedByteValueObject(specialItem.Index + Mgs2Offset.BaseItem.Start, BitConverter.GetBytes(value));
                        break;
                    case Constants.MaxableWeapon maxableWeapon:
                        if (!force)
                        {
                            if(character == Constants.PlayableCharacter.Snake)
                                if (!Snake.UsableObjects.Contains(maxableWeapon))
                                    throw new Exception(
                                        "Sorry, Snake cannot use this weapon without crashing the game. Install the " +
                                        "'All Weapons' mod from Nexus and check off the 'All Weapons Mod Installed?' " +
                                        "checkbox to force this through");
                        }
                        Logger?.Debug($"mgs2Object parsed as MaxableWeapon, setting base value to: {value}");
                        SetPlayerOffsetBasedByteValueObject(maxableWeapon.Index + Mgs2Offset.BaseWeapon.Start, BitConverter.GetBytes(value));
                        break;
                    case Constants.BooleanWeapon booleanWeapon:
                        if (!force)
                        {
                            if(character == Constants.PlayableCharacter.Snake)
                                if (!Snake.UsableObjects.Contains(booleanWeapon))
                                    throw new Exception(
                                        "Snake cannot use this weapon without crashing the game, sorry. Install the " +
                                        "'All Weapons' mod and check off the 'All Weapons Mod Installed?' checkbox to " +
                                        "force this through");
                        }
                        Logger?.Debug($"mgs2Object parsed as BooleanWeapon, setting base value to: {value}");
                        SetPlayerOffsetBasedByteValueObject(booleanWeapon.Index + Mgs2Offset.BaseWeapon.Start, BitConverter.GetBytes(value));
                        break;
                    case Constants.BooleanItem booleanItem:
                        Logger?.Debug($"mgs2Object parsed as BooleanItem, setting base value to: {value}");
                        SetPlayerOffsetBasedByteValueObject(booleanItem.Index + Mgs2Offset.BaseItem.Start, BitConverter.GetBytes(value));
                        break;
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to update the base value for {mgs2Object.Name}: {e}");
                throw new AggregateException($"Could not update base value for {mgs2Object.Name}", e);
            }
        }

        public void UpdateObjectMaxValue(Constants.IMgs2Object mgs2Object, ushort count)
        {
            //NOTE: Currently working as expected on rewrite/multiplatforming
            try
            {
                switch (mgs2Object)
                {
                    case Constants.MaxableItem maxableItem:
                        Logger?.Debug($"mgs2Object parsed as MaxableItem, setting max count to: {count}");
                        SetPlayerOffsetBasedByteValueObject(maxableItem.MaxIndex + Mgs2Offset.BaseItem.Start, BitConverter.GetBytes(count));
                        break;
                    case Constants.MaxableWeapon maxableWeapon:
                        Logger?.Debug($"mgs2Object parsed as maxableWeapon, setting max count to: {count}");
                        SetPlayerOffsetBasedByteValueObject(maxableWeapon.MaxIndex + Mgs2Offset.BaseWeapon.Start, BitConverter.GetBytes(count));
                        break;
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to update the max value for {mgs2Object.Name}: {e}");
                throw new AggregateException($"Could not update max value for {mgs2Object.Name}", e);
            }
        }

        public ushort GetObjectValue(Constants.IMgs2Object mgs2Object)
        {
            try
            {
                switch (mgs2Object)
                {
                    case Constants.MaxableItem maxableItem:
                        Logger?.Debug($"mgs2Object parsed as MaxableItem, getting base value...");
                        return GetPlayerOffsetBasedByteValueObject(maxableItem.Index + Mgs2Offset.BaseItem.Start);
                    case Constants.SpecialItem specialItem:
                        Logger?.Debug($"mgs2Object parsed as SpecialItem, getting base value...");
                        return GetPlayerOffsetBasedByteValueObject(specialItem.Index + Mgs2Offset.BaseItem.Start);
                    case Constants.MaxableWeapon maxableWeapon:
                        Logger?.Debug($"mgs2Object parsed as MaxableWeapon, getting base value...");
                        return GetPlayerOffsetBasedByteValueObject(maxableWeapon.Index + Mgs2Offset.BaseWeapon.Start);
                    case Constants.BooleanWeapon booleanWeapon:
                        Logger?.Debug($"mgs2Object parsed as BooleanWeapon, getting base value...");
                        return GetPlayerOffsetBasedByteValueObject(booleanWeapon.Index + Mgs2Offset.BaseWeapon.Start);
                    case Constants.BooleanItem booleanItem:
                        Logger?.Debug($"mgs2Object parsed as BooleanItem, getting base value...");
                        return GetPlayerOffsetBasedByteValueObject(booleanItem.Index + Mgs2Offset.BaseItem.Start);
                    default:
                        Logger?.Error("Unknown mgs2Object type, cannot continue");
                        throw new InvalidDataException("Unknown mgs2Object type");
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to update the base value for {mgs2Object.Name}: {e}");
                throw new AggregateException($"Could not update base value for {mgs2Object.Name}", e);
            }
        }

        public void ToggleObject(Constants.IMgs2Object mgs2Object,
            bool enable = true, bool force = false)
        {
            try
            {
                if (enable)
                    UpdateObjectBaseValue(mgs2Object, 1, force);
                else
                {
                    if (mgs2Object is Constants.Item)
                    {
                        UpdateObjectBaseValue(mgs2Object, 0, force);
                    }
                    else
                    {
                        UpdateObjectBaseValue(mgs2Object, ushort.MaxValue, force);
                    }
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to toggle {mgs2Object.Name}: {e}");
                throw new AggregateException($"Could not toggle {mgs2Object.Name}", e);
            }
        }

        public GameStats ReadGameStats()
        {
            try
            {
                Logger?.Verbose("Reading game stats...");
                byte[] gameStatsBytes;
                byte[] rationsUsedBytes;
                byte[] specialItemsBytes;
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    gameStatsBytes = ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.GameStatsBlock.Start,
                        Mgs2Offset.GameStatsBlock.Length);
                    rationsUsedBytes = ReadValueFromMemory(pointerLocation + Mgs2Offset.RationsUsed.Start, Mgs2Offset.RationsUsed.Length);
                    specialItemsBytes = ReadValueFromMemory(pointerLocation + Mgs2Offset.SpecialItemsUsed.Start, Mgs2Offset.SpecialItemsUsed.Length);
                }
                short continues = BitConverter.ToInt16(gameStatsBytes, 4);
                short saves = BitConverter.ToInt16(gameStatsBytes, 8);
                int playTime = BitConverter.ToInt32(gameStatsBytes, 10);
                short mechsDestroyed = BitConverter.ToInt16(gameStatsBytes, 42);
                short shots = BitConverter.ToInt16(gameStatsBytes, 18);
                short alerts = BitConverter.ToInt16(gameStatsBytes, 20);
                short kills = BitConverter.ToInt16(gameStatsBytes, 22);
                short damageTaken = BitConverter.ToInt16(gameStatsBytes, 24);
                short rationsUsed = BitConverter.ToInt16(rationsUsedBytes, 0);
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

                Logger?.Verbose($"Current game stats: {gameStats}");

                return gameStats;
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get current game stats: {e}");
                throw new AggregateException("Could not get current game stats", e);
            }
        }

        public void ChangeGameStat(GameStats.ModifiableStats gameStat, short value)
        {
            try
            {
                int gameStatOffset;
                switch (gameStat)
                {
                    case GameStats.ModifiableStats.Alerts:
                        gameStatOffset = Mgs2Offset.AlertCount.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.Continues:
                        gameStatOffset = Mgs2Offset.ContinueCount.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.DamageTaken:
                        gameStatOffset = Mgs2Offset.DamageTaken.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.Kills:
                        gameStatOffset = Mgs2Offset.KillCount.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.MechsDestroyed:
                        gameStatOffset = Mgs2Offset.MechsDestroyed.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.Rations:
                        gameStatOffset = Mgs2Offset.RationsUsed.Start;
                        break;
                    case GameStats.ModifiableStats.Saves:
                        gameStatOffset = Mgs2Offset.SaveCount.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    case GameStats.ModifiableStats.Shots:
                        gameStatOffset = Mgs2Offset.ShotCount.Start + Mgs2Offset.GameStatsBlock.Start;
                        break;
                    default:
                        throw new Exception("You must provide a valid game stat to modify");
                }
                
                SetPointerOffsetValue(Mgs2Pointer.PlayerPointer, gameStatOffset, BitConverter.GetBytes(value));
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to modify {gameStat}: {e}");
                throw new AggregateException($"Could not modify {gameStat}", e);
            }
        }

        public Difficulty ReadCurrentDifficulty()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    byte[] difficultyByte = ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.CurrentDifficulty.Start,
                        Mgs2Offset.CurrentDifficulty.Length);
                        
                    int convertedDifficulty = difficultyByte[0];

                    return (Difficulty)convertedDifficulty;
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Could not get current difficulty: {e}");
                throw new AggregateException("Failed to get current difficulty", e);
            }
        }

        public GameType ReadGameType()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    byte[] gameTypeByte = ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.CurrentGametype.Start, Mgs2Offset.CurrentGametype.Length);

                    int convertedGameType = gameTypeByte[0];

                    return (GameType)convertedGameType;
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get current game type: {e}");
                throw new AggregateException("Could not get current game type", e);
            }
        }

        public ushort GetCurrentHp()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    byte[] currentHpBytes = ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.CurrentHp.Start, Mgs2Offset.CurrentHp.Length);

                    return BitConverter.ToUInt16(currentHpBytes, 0);
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to get current HP: {e}");
                throw new AggregateException("Could not get current HP", e);
            }
        }

        public ushort GetCurrentMaxHp()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr pointerLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);
                    byte[] currentMaxHpBytes = ReadValueFromMemory(
                        pointerLocation + Mgs2Offset.CurrentMaxHp.Start, Mgs2Offset.CurrentMaxHp.Length);

                    return BitConverter.ToUInt16(currentMaxHpBytes, 0);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to get current max HP: {e}");
                throw new AggregateException("Could not get current max HP", e);
            }
        }

        public ushort GetCurrentGripGauge()
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryPointedTo = proxy.FollowPointer(new IntPtr(Mgs2Pointer.CurrentGrip), false);
                    memoryPointedTo = IntPtr.Add(memoryPointedTo, Mgs2Offset.CurrentGripGauge.Start);
                    byte[] gripGauge = proxy.GetMemoryFromPointer(memoryPointedTo, Mgs2Offset.CurrentGripGauge.Length);

                    return BitConverter.ToUInt16(gripGauge, 0);
                }
            }
            catch
            {
                return ushort.MinValue;
            }
        }

        public void ModifyCurrentGripGauge(ushort desiredGripGauge)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryPointedTo = proxy.FollowPointer(new IntPtr(Mgs2Pointer.CurrentGrip), false);
                    memoryPointedTo = IntPtr.Add(memoryPointedTo, Mgs2Offset.CurrentGripGauge.Start);
                    proxy.SetMemoryAtPointer(memoryPointedTo, BitConverter.GetBytes(desiredGripGauge));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to modify current grip: {e}");
                throw new AggregateException("Could not modify current grip", e);
            }
        }

        public void ModifyCurrentHp(ushort desiredHp)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryPointedTo = proxy.FollowPointer(new IntPtr(Mgs2Pointer.ModifiableHp), false);
                    memoryPointedTo = IntPtr.Add(memoryPointedTo, Mgs2Offset.ModifiableHp.Start);
                    proxy.SetMemoryAtPointer(memoryPointedTo, BitConverter.GetBytes(desiredHp));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to modify current hp: {e}");
                throw new AggregateException("Could not modify current hp", e);
            }
        }

        public ushort ModifyGripLevel(bool increase)
        {
            try
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    Constants.PlayableCharacter currentCharacter = DetermineActiveCharacter();

                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    IntPtr memoryLocation = proxy.FollowPointer(new IntPtr(Mgs2Pointer.PlayerPointer), false);

                    if (currentCharacter == Constants.PlayableCharacter.Snake)
                        memoryLocation = IntPtr.Add(memoryLocation, Mgs2Offset.SnakePullups.Start);
                    else
                        memoryLocation = IntPtr.Add(memoryLocation, Mgs2Offset.RaidenPullups.Start);

                    byte[] gripLevelBytes = proxy.GetMemoryFromPointer(memoryLocation, 2);
                    ushort gripLevel = BitConverter.ToUInt16(gripLevelBytes, 0);

                    switch (increase)
                    {
                        case true:
                            if (gripLevel < 200)
                            {
                                proxy.SetMemoryAtPointer(memoryLocation, BitConverter.GetBytes(gripLevel += 100));
                            }
                            return gripLevel;
                        case false:
                            //this, unfortunately, doesn't seem to actually cause the grip level to change... annoying
                            if (gripLevel > 0 && gripLevel >= 100)
                            {
                                proxy.SetMemoryAtPointer(memoryLocation, BitConverter.GetBytes(gripLevel -= 100));
                            }
                            else
                            {
                                proxy.SetMemoryAtPointer(memoryLocation, BitConverter.GetBytes(0));
                            }
                            return gripLevel;
                    }
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to modify grip level: {e}");
                throw new AggregateException("Could not modify current grip level", e);
            }
        }

        private static IntPtr FindAoBReferencedPointer(string aobToFind, MemoryOffset memoryOffset)
        {
            //TODO: remove the auto-return
            return IntPtr.Zero;
            //TODO: confirm this is working as expected
            //find the pointer referenced in memory
            byte[] aobReferencedPointer = ReadAoBOffsetValue(aobToFind, memoryOffset);
            //return it as a pointer
            return new IntPtr(BitConverter.ToInt64(aobReferencedPointer, 0));
        }

        private static void SetDataInNestedPointers(List<int> pointerOffsets, int destinationOffset, byte[] dataToSet)
        {
            try
            {
                IntPtr pointerLocation;
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock(Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    pointerLocation = spp.FollowPointer(new IntPtr(pointerOffsets[0]), false);
                }
                
                pointerLocation = FollowNestedPointers(pointerLocation, pointerOffsets.Slice(1,pointerOffsets.Count - 1));

                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    spp.SetMemoryAtPointer(IntPtr.Add(pointerLocation, destinationOffset), dataToSet);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to set data within nested pointers: {e}");
                throw new AggregateException("Could not set nested pointer data", e);
            }
        }

        private static IntPtr FollowNestedPointers(IntPtr initialPointer, List<int> pointerOffsets)
        {
            IntPtr pointerLocation = initialPointer;

            try
            {
                for (int i = 0; i < pointerOffsets.Count; i++)
                {
                    if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                    lock (Mgs2Monitor.Mgs2Process)
                    {
                        using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                        IntPtr nestedPointer = new IntPtr(pointerLocation.ToInt64() + pointerOffsets[i]);
                        pointerLocation = new IntPtr(BitConverter.ToInt64(spp.GetMemoryFromPointer(nestedPointer, 8), 0));
                    }
                }

                return pointerLocation;
            }
            catch (Exception e)
            {
                Logger?.Error($"Failed to follow nested pointers: {e}");
                throw new AggregateException("Could not follow nested pointers provided", e);
            }
        }

        private static byte[] GetDataFromNestedPointers(IntPtr initialPointer, List<int> pointerOffsets, int destinationOffset, int bytesToReadAtDestination)
        {
            IntPtr pointerLocation = IntPtr.Zero;

            //pointerLocation = initialPointer;
            if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
            for (int i = 0; i < pointerOffsets.Count; i++)
            {
                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    if (pointerLocation == IntPtr.Zero)
                    {
                        pointerLocation = spp.FollowPointer(new IntPtr(pointerOffsets[i]), false);
                    }
                    else
                    {
                        IntPtr nestedPointer = new IntPtr(pointerLocation.ToInt64() + pointerOffsets[i]);
                        pointerLocation = new IntPtr(BitConverter.ToInt64(spp.GetMemoryFromPointer(nestedPointer, 8), 0));
                    }
                }
            }
            lock (Mgs2Monitor.Mgs2Process)
            {
                using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                return spp.GetMemoryFromPointer(IntPtr.Add(pointerLocation, destinationOffset), bytesToReadAtDestination);
            }
        }

        private static byte[] GetDataFromNestedPointers(List<int> pointerOffsets, int destinationOffset, int bytesToReadAtDestination)
        {
            try
            {
                IntPtr pointerLocation;
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock(Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    pointerLocation = spp.FollowPointer(new IntPtr(pointerOffsets[0]), false);
                }
                
                pointerLocation = FollowNestedPointers(pointerLocation, pointerOffsets.Slice(1,pointerOffsets.Count - 1));

                lock (Mgs2Monitor.Mgs2Process)
                {
                    using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    return spp.GetMemoryFromPointer(IntPtr.Add(pointerLocation, destinationOffset), bytesToReadAtDestination);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to get data from nested pointers: {e}");
                throw new AggregateException("Could not get value from nested pointers provided", e);
            }
        }

        public void SetBossVitals(BossVitals updatedVitals)
        {
            try
            {
                if (updatedVitals.Boss != Constants.Boss.Fortune)
                {
                    if (updatedVitals.NestedHealthPointers != null)
                        SetDataInNestedPointers(updatedVitals.NestedHealthPointers, updatedVitals.HealthOffset,
                            BitConverter.GetBytes(updatedVitals.Health));
                    if (updatedVitals.HasStamina && updatedVitals.NestedStaminaPointers != null)
                    {
                        SetDataInNestedPointers(updatedVitals.NestedStaminaPointers, updatedVitals.StaminaOffset,
                            BitConverter.GetBytes(updatedVitals.Stamina));
                    }
                }
                else
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    if (_fortuneOffset == IntPtr.Zero)
                    {
                        _fortuneOffset = proxy.ScanMemoryForUniquePattern(new SimplePattern(Mgs2AoB.FortuneName)).Offset;
                    }

                    proxy.ModifyProcessOffset(IntPtr.Add(_fortuneOffset, Mgs2Offset.FortuneHpValue.Start), updatedVitals.Health, true);
                    proxy.ModifyProcessOffset(IntPtr.Add(_fortuneOffset, Mgs2Offset.FortuneStaminaValue.Start), updatedVitals.Stamina, true);
                }
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to set boss vitals: {e}");
                throw new AggregateException($"Could not set boss vitals", e);
            }
        }

        private static IntPtr _fortuneOffset = IntPtr.Zero;
        public BossVitals GetBossVitals(Constants.Boss selectedBoss)
        {
            try
            {
                BossVitals bossVitals = BossVitals.ParseBossVitals(selectedBoss);

                if (selectedBoss != Constants.Boss.Fortune)
                {
                    if (bossVitals.NestedHealthPointers != null)
                        bossVitals.Health =
                            BitConverter.ToInt16(
                                GetDataFromNestedPointers(bossVitals.NestedHealthPointers, bossVitals.HealthOffset, 2),
                                0);
                    if (bossVitals.HasStamina && bossVitals.NestedStaminaPointers != null)
                    {
                        bossVitals.Stamina = BitConverter.ToInt16(GetDataFromNestedPointers(bossVitals.NestedStaminaPointers, bossVitals.StaminaOffset, 2), 0);
                    }
                }
                else
                {
                    using SimpleProcessProxy proxy = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                    if (_fortuneOffset == IntPtr.Zero) 
                    {
                        _fortuneOffset = proxy.ScanMemoryForUniquePattern(new SimplePattern(Mgs2AoB.FortuneName)).Offset;
                    }

                    bossVitals.Health = BitConverter.ToInt16(proxy.ReadProcessOffset(IntPtr.Add(_fortuneOffset, Mgs2Offset.FortuneHpValue.Start), Mgs2Offset.FortuneHpValue.Length), 0);
                    bossVitals.Stamina = BitConverter.ToInt16(proxy.ReadProcessOffset(IntPtr.Add(_fortuneOffset, Mgs2Offset.FortuneStaminaValue.Start), Mgs2Offset.FortuneStaminaValue.Length), 0);
                }

                return bossVitals;
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to get boss vitals: {e}");
                throw new AggregateException($"Could not get boss vitals", e);
            }
        }

        public Constants.PlayableCharacter DetermineActiveCharacter()
        {
            //return Constants.PlayableCharacter.Pliskin;
            try
            {
                string characterCode = GetCharacterCode();
                Logger?.Debug($"Found character: {characterCode}");

                if (characterCode.Contains("tnk") || characterCode.Contains("r_vr_s"))
                {
                    Logger?.Verbose("Currently playing as Snake");
                    if (characterCode.Contains("tnk"))
                        return Constants.PlayableCharacter.Snake;
                    return Constants.PlayableCharacter.Pliskin; //technically you're not playing as Pliskin, but this fixes the VR/Snake tales issue for Snake
                }

                if (characterCode.Contains("plt"))
                {
                    Logger?.Verbose("Currently playing as Raiden");
                    return Constants.PlayableCharacter.Raiden;
                }

                if (characterCode.Contains("vr_1"))
                {
                    Logger?.Verbose("Currently playing as MGS1 Snake");
                    return Constants.PlayableCharacter.Mgs1Snake;
                }

                if (characterCode.Contains("r_vr_t"))
                {
                    Logger?.Verbose("Currently playing as Tuxedo Snake");
                    return Constants.PlayableCharacter.TuxedoSnake;
                }

                if (characterCode.Contains("r_vr_p"))
                {
                    Logger?.Verbose("Currently playing as Pliskin");
                    return Constants.PlayableCharacter.Pliskin;
                }

                if (characterCode.Contains("r_vr_b"))
                {
                    Logger?.Verbose("Currently playing as Ninja Raiden");
                    return Constants.PlayableCharacter.NinjaRaiden;
                }

                if (characterCode.Contains("r_vr_x"))
                {
                    Logger?.Verbose("Currently playing as Naked Raiden");
                    return Constants.PlayableCharacter.NakedRaiden;
                }

                Logger?.Warning("Unable to determine what the active character is!");
                throw new Exception("Unknown stage! Can't safely determine what the active character is");
            }
            catch(Exception e)
            {
                Logger?.Error($"Failed to determine active character: {e}");
                throw new AggregateException("Could not determine active character", e);
            }
        }

        public void Dispose()
        {
            //Nothing really needs to be done I think
        }
    }
}
