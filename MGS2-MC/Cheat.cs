using MGS2_MC.Helpers;
using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MGS2_MC
{
    public struct Cheat
    {
        public string Name { get; private set; }
        public Action<bool> CheatAction { get; private set; }
        public byte[] OriginalBytes { get; private set; }
        public IntPtr CodeLocation { get; set; }
        private static CancellationTokenSource customFilterCancellationTokenSource { get; set; }
        private static Color customFilterColor { get; set; }

        public Cheat(string name, Action<bool> action, byte[] originalBytes)
        {
            Name = name;
            CheatAction = action;
            OriginalBytes = originalBytes;
            CodeLocation = IntPtr.Zero;
        }

        internal class CheatActions
        {
            private static void ReplaceWithOriginalCode(IntPtr memoryLocation, MemoryOffset offset, byte[] bytesToReplace, int startIndexToReplace = 0)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                if (memoryLocation != IntPtr.Zero)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(IntPtr.Add(memoryLocation, offset.Start), offset.Length);

                                    for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace.Length; i++)
                                    {
                                        memoryContent[i] = bytesToReplace[i];
                                    }

                                    spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                    successful = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static IntPtr ReplaceWithInvalidCode(string aob, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).ToInt32();

                                if (memoryLocation != -1)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(new IntPtr(memoryLocation + offset.Start), offset.Length);

                                    for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace; i++)
                                    {
                                        memoryContent[i] = 0x90;
                                    }

                                    spp.ModifyProcessOffset(new IntPtr(memoryLocation), memoryContent, true);
                                    successful = true;

                                    return new IntPtr(memoryLocation);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }

                return IntPtr.Zero;
            }

            private static IntPtr ReplaceWithInvalidCode(IntPtr memoryLocation, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                if (memoryLocation != IntPtr.Zero)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(IntPtr.Add(memoryLocation, offset.Start), offset.Length);

                                    for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace; i++)
                                    {
                                        memoryContent[i] = 0x90;
                                    }

                                    spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                    successful = true;

                                    return memoryLocation;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }

                return IntPtr.Zero;
            }

            internal static IntPtr ReplaceWithSpecificCode(string patternToScan, byte[] replacementBytes, MemoryOffset offset)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(patternToScan);
                                int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).ToInt32();

                                if (memoryLocation != -1)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(new IntPtr(memoryLocation + offset.Start), offset.Length);

                                    for (int i = 0; i < replacementBytes.Length; i++)
                                    {
                                        memoryContent[i] = replacementBytes[i];
                                    }

                                    spp.ModifyProcessOffset(new IntPtr(memoryLocation + offset.Start), memoryContent, true);
                                    successful = true;

                                    return new IntPtr(memoryLocation);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
                throw new Exception("Failed to replace code, aborting the process");
            }

            internal static IntPtr ReplaceWithSpecificCode(IntPtr memoryLocation, byte[] replacementBytes, MemoryOffset offset)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                if (memoryLocation != IntPtr.Zero)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(IntPtr.Add(memoryLocation, offset.Start), offset.Length);

                                    for (int i = 0; i < replacementBytes.Length; i++)
                                    {
                                        memoryContent[i] = replacementBytes[i];
                                    }

                                    spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                    successful = true;

                                    return memoryLocation;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
                throw new Exception("Failed to replace code, aborting the process");
            }

            private static IntPtr ModifySingleByte(string aob, MemoryOffset offset, byte replacementValue)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).ToInt32();

                                if (memoryLocation != -1)
                                {
                                    spp.ModifyProcessOffset(new IntPtr(memoryLocation + offset.Start), replacementValue, true);
                                    successful = true;

                                    return new IntPtr(memoryLocation);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }

                return IntPtr.Zero;
            }

            private static void ModifySingleByte(IntPtr memoryLocation, MemoryOffset offset, byte replacementValue)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                if (memoryLocation != IntPtr.Zero)
                                {
                                    spp.ModifyProcessOffset(IntPtr.Add(memoryLocation, offset.Start), replacementValue, true);
                                    successful = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            internal static byte[] ReadMemory(string aob, MemoryOffset offset)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).ToInt32();

                                if(memoryLocation != -1)
                                    return spp.ReadProcessOffset(new IntPtr(memoryLocation + offset.Start), offset.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);

                    return null;
                }
            }

            public static void TurnScreenBlack(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.BlackScreen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.OriginalBytes = ReadMemory(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN);
                        activeCheat.CodeLocation = ModifySingleByte(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN, 0x00);
                        MGS2Cheat.BlackScreen = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.BLACK_SCREEN, 0x00);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.BLACK_SCREEN, 0x40);
            }

            public static void TurnOffBleedDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoBleedDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.NoBleedDamage, MGS2Offset.NO_BLEED_DMG, 7);
                        MGS2Cheat.NoBleedDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_BLEED_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_BLEED_DMG, MGS2AoB.OriginalBleedDamageBytes);
            }

            public static void TurnOffBurnDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoBurnDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.NoBurnDamage, MGS2Offset.NO_BURN_DMG, 7);
                        MGS2Cheat.NoBurnDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_BURN_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_BLEED_DMG, MGS2AoB.OriginalBurnDamageBytes);
            }

            internal static void InfiniteAmmo(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteAmmo;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.InfiniteAmmo, MGS2Offset.INFINITE_AMMO, 4);
                        MGS2Cheat.InfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_AMMO, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_AMMO, MGS2AoB.OriginalAmmoBytes);
            }

            internal static void InfiniteLife(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteLife;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.InfiniteLife, MGS2Offset.INFINITE_LIFE, 4);
                        MGS2Cheat.InfiniteLife = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_LIFE, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_LIFE, MGS2AoB.OriginalLifeBytes);
            }

            internal static void InfiniteOxygen(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteOxygen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.InfiniteO2, MGS2Offset.INFINITE_O2, 4);
                        MGS2Cheat.InfiniteOxygen = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_O2, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_O2, MGS2AoB.OriginalO2Bytes);
            }

            internal static void Letterboxing(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.Letterboxing;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(MGS2AoB.Camera, MGS2Offset.LETTERBOX, 0x00);
                        MGS2Cheat.Letterboxing = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.LETTERBOX, 0x01);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.LETTERBOX, 0x01);
            }

            internal static void AmmoNeverDepletes(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoReload;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, 2);
                        MGS2Cheat.NoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NEVER_RELOAD, 2);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NEVER_RELOAD, MGS2AoB.OriginalReloadBytes);
            }

            internal static void GripNeverDepletes(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoGripDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.DecrementGripGauge, MGS2Offset.NO_GRIP_DMG, 7);
                        MGS2Cheat.NoGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_GRIP_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_GRIP_DMG, MGS2AoB.OriginalGripDamageBytes);
            }

            internal static void TurnOffPauseButton(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisablePauseButton;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.InGamePause, MGS2Offset.NO_PAUSE_BTN, 5);
                        MGS2Cheat.DisablePauseButton = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_PAUSE_BTN, 5);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_PAUSE_BTN, MGS2AoB.OriginalPauseButtonBytes);
            }

            internal static void TurnOffItemMenuPause(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisableItemMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.ItemMenuPause, MGS2Offset.NO_ITEM_PAUSE, 6);
                        MGS2Cheat.DisableItemMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_ITEM_PAUSE, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_ITEM_PAUSE, MGS2AoB.OriginalItemMenuPauseBytes);
            }

            internal static void TurnOffWeaponMenuPause(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisableWeaponMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.WeaponMenuPause, MGS2Offset.NO_WEAPON_PAUSE, 6);
                        MGS2Cheat.DisableWeaponMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.NO_WEAPON_PAUSE, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NO_WEAPON_PAUSE, MGS2AoB.OriginalWeaponMenuPauseBytes);
            }

            internal static void NoClipNoGravity(bool activate)
            {
                try
                {
                    NoClip(false, activate);
                }
                catch(Exception e)
                {
                    throw new AggregateException($"Could not set noclip -nogravity to {activate}", e);
                }
            }

            internal static void NoClipWithGravity(bool activate)
            {
                try
                {
                    NoClip(true, activate);
                }
                catch(Exception e)
                {
                    throw new AggregateException($"Could not set noclip -gravity to {activate}", e);
                }
            }

            private static void NoClip(bool gravity, bool activate)
            {
                try
                {
                    Constants.PlayableCharacter currentPc = MGS2MemoryManager.DetermineActiveCharacter();

                    lock (MGS2Monitor.MGS2Process)
                    {
                        using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                        {
                            string activeCharacterAoB;
                            switch (currentPc)
                            {
                                case Constants.PlayableCharacter.Raiden:
                                    activeCharacterAoB = MGS2AoB.RaidenClipping;
                                    break;
                                case Constants.PlayableCharacter.NinjaRaiden:
                                    activeCharacterAoB = MGS2AoB.NinjaClipping;
                                    break;
                                case Constants.PlayableCharacter.NakedRaiden:
                                    activeCharacterAoB = MGS2AoB.NakedRaidenClipping;
                                    break;
                                case Constants.PlayableCharacter.Snake:
                                    activeCharacterAoB = MGS2AoB.SnakeClipping;
                                    break;
                                case Constants.PlayableCharacter.Pliskin:
                                    activeCharacterAoB = MGS2AoB.PliskinClipping;
                                    break;
                                case Constants.PlayableCharacter.MGS1Snake:
                                    activeCharacterAoB = MGS2AoB.MGS1SnakeClipping;
                                    break;
                                case Constants.PlayableCharacter.TuxedoSnake:
                                    activeCharacterAoB = MGS2AoB.TuxedoSnakeClipping;
                                    break;
                                default:
                                    activeCharacterAoB = MGS2AoB.VRClipping;
                                    break;
                            }

                            IntPtr pointerLocation = spp.FollowPointer(new IntPtr(MGS2Pointer.WalkThroughWalls), false);
                            byte[] memoryContent = spp.GetMemoryFromPointer(new IntPtr(pointerLocation.ToInt64() + MGS2Offset.NO_CLIP.Start), MGS2Offset.NO_CLIP.Length);

                            if (!activate)
                            {
                                if (memoryContent[4] == 0x15 || memoryContent[4] == 0x13)
                                {
                                    memoryContent[4] = 0x14;
                                }
                                else if (memoryContent[4] == 0x25 || memoryContent[4] == 0x23)
                                {
                                    memoryContent[4] = 0x24;
                                }
                            }
                            else
                            {
                                if (gravity)
                                {
                                    //set byte to either 15 or 25
                                    if (memoryContent[4] == 0x24)
                                    {
                                        memoryContent[4] = 0x25;
                                    }
                                    else
                                    {
                                        memoryContent[4] = 0x15;
                                    }
                                }
                                else
                                {
                                    //set byte to either 13 or 23
                                    if (memoryContent[4] == 0x24)
                                    {
                                        memoryContent[4] = 0x23;
                                    }
                                    else
                                    {
                                        memoryContent[4] = 0x13;
                                    }
                                }
                            }

                            spp.SetMemoryAtPointer(new IntPtr(pointerLocation.ToInt64() + MGS2Offset.NO_CLIP.Start), memoryContent);
                        }
                    }
                }
                catch(Exception e)
                {
                    throw new AggregateException("Could not toggle noclip functionality", e);
                }
            }

            internal static void ZoomIn(bool activate)
            {
                Zoom(true);
            }

            internal static void ZoomOut(bool activate)
            {
                Zoom(false);
            }

            private static void Zoom(bool zoomIn)
            {
                byte[] currentZoom = ReadMemory(MGS2AoB.Camera, MGS2Offset.ZOOM);

                if (currentZoom == null)
                    return;

                Cheat activeCheat = zoomIn ? MGS2Cheat.ZoomIn : MGS2Cheat.ZoomOut;
                if (zoomIn)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]++);
                        MGS2Cheat.ZoomIn = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.ZOOM, currentZoom[0]++);
                    }
                }
                else
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]--);
                        MGS2Cheat.ZoomOut = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, MGS2Offset.ZOOM, currentZoom[0]--);
                    }
                }
            }

            internal static void InfiniteItems(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteItems;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.InfiniteItemUse, MGS2Offset.INFINITE_ITEMS, 4);
                        MGS2Cheat.InfiniteItems = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_ITEMS, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.INFINITE_ITEMS, MGS2AoB.OriginalItemUseBytes);
            }

            internal static void MaxStackOnPickup(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.MaxStackOnPickup;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.MaxCountOnPickup, MGS2Offset.MAX_ON_PICKUP, 4);
                        MGS2Cheat.MaxStackOnPickup = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.MAX_ON_PICKUP, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.MAX_ON_PICKUP, MGS2AoB.OriginalCountOnPickup);
            }

            internal static void InfiniteKnockout(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteKnockout;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.KnockoutDuration, MGS2Offset.KNOCKOUT_DURATION, 8);
                        MGS2Cheat.InfiniteKnockout = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.KNOCKOUT_DURATION, 8);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.KNOCKOUT_DURATION, MGS2AoB.OriginalKnockoutDuration);
                
            }

            internal static void RemovePlantFilter(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.RemovePlantFilter;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.RemovePlantFilter, MGS2Offset.REMOVE_PLANT_FILTER, 7);
                        MGS2Cheat.RemovePlantFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.REMOVE_PLANT_FILTER, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.REMOVE_PLANT_FILTER, MGS2AoB.OriginalRemovePlantFilterBytes);
            }

            internal static void RemovePlantFog(bool activate)
            {
                byte[] DisableFog = new byte[] { 0x46 };

                Cheat activeCheat = MGS2Cheat.RemovePlantFog;
                if (activate)
                {
                    if(activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        byte[] originalValue = ReadMemory(MGS2AoB.RemovePlantFog, MGS2Offset.REMOVE_PLANT_FOG);
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.RemovePlantFog, MGS2Offset.REMOVE_PLANT_FOG, 5);
                        activeCheat.OriginalBytes = originalValue;
                        MGS2Cheat.RemovePlantFog = activeCheat;
                    }
                    else
                    {
                        ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.REMOVE_PLANT_FOG, MGS2AoB.OriginalPlantFogBytes);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.REMOVE_PLANT_FOG, activeCheat.OriginalBytes);
            }

            internal static void RemoveTankerEffects(bool activate)
            {
                byte[] disableFilter = new byte[] { 0x04 };
                byte[] enableFilter = new byte[] { 0x03 };

                Cheat activeCheat = MGS2Cheat.RemoveTankerFilter;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(MGS2AoB.RemoveTankerFilter, disableFilter, MGS2Offset.REMOVE_TANKER_FILTER);
                        MGS2Cheat.RemoveTankerFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, disableFilter, MGS2Offset.REMOVE_TANKER_FILTER);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.REMOVE_TANKER_FILTER, enableFilter);
            }

            internal static void NightTime(bool activate)
            {
                byte[] nightTime = new byte[] { 0x00 };
                byte[] dayTime = new byte[] { 0xFF };

                Cheat activeCheat = MGS2Cheat.NightTime;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(MGS2AoB.NightTime, nightTime, MGS2Offset.NIGHT_TIME);
                        MGS2Cheat.NightTime = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, nightTime, MGS2Offset.NIGHT_TIME);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.NIGHT_TIME, dayTime);
            }

            internal static void EnableCustomFilter(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.EnableCustomFilter;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.EnableCustomFiltering, MGS2Offset.ENABLE_CUSTOM_FILTER, MGS2AoB.OriginalCustomFilteringBytes.Length - 1);
                        MGS2Cheat.EnableCustomFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.ENABLE_CUSTOM_FILTER, MGS2AoB.OriginalCustomFilteringBytes.Length - 1);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.ENABLE_CUSTOM_FILTER, MGS2AoB.OriginalCustomFilteringBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static async Task ApplyColorFilter(Color chosenColor)
            {
                byte[] customColor = new byte[] { chosenColor.R, chosenColor.G, chosenColor.B };

                ReplaceWithSpecificCode(MGS2AoB.CustomFilteringAoB, customColor, MGS2Offset.CUSTOM_FILTERING);
                
                if(!customFilterCancellationTokenSource.IsCancellationRequested)
                    await PeriodicTask.Run(() => ReapplyColorFilter(customColor), TimeSpan.FromMilliseconds(100), customFilterCancellationTokenSource.Token);
            }

            private static void ReapplyColorFilter(byte[] chosenColor)
            {
                byte[] currentColor = ReadMemory(MGS2AoB.CustomFilteringAoB, MGS2Offset.CUSTOM_FILTERING);

                if (!currentColor.SequenceEqual(chosenColor))
                {
                    ReplaceWithSpecificCode(MGS2AoB.CustomFilteringAoB, chosenColor, MGS2Offset.CUSTOM_FILTERING);
                }
            }

            internal static void PauseVRTimer(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.PauseVRTimer;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.OriginalBytes = ReadMemory(MGS2AoB.PauseVRAoB, MGS2Offset.PAUSE_VR_TIMER);
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.PauseVRAoB, MGS2Offset.PAUSE_VR_TIMER, 3);
                        MGS2Cheat.PauseVRTimer = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.PAUSE_VR_TIMER, 3);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.PAUSE_VR_TIMER, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void AutoCompleteVRObjectives(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRObjectiveAutoComplete;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRObjectiveAoB, MGS2Offset.VR_AUTO_COMPLETE_OBJECTIVES, 6);
                        MGS2Cheat.VRObjectiveAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_AUTO_COMPLETE_OBJECTIVES, 6);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_AUTO_COMPLETE_OBJECTIVES, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void AutoCompleteVREnemies(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VREnemiesAutoComplete;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRObjectiveAoB, MGS2Offset.VR_AUTO_COMPLETE_ENEMIES, 2);
                        MGS2Cheat.VREnemiesAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_AUTO_COMPLETE_ENEMIES, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_AUTO_COMPLETE_ENEMIES, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRNoHitDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRNoHitDamage;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRNoHitDamageAoB, MGS2Offset.VR_NO_HIT_DMG, 4);
                        MGS2Cheat.VRNoHitDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_HIT_DMG, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_HIT_DMG, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRNoFallDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRNoFallDamage;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRNoFallDamageAoB, MGS2Offset.VR_NO_FALL_DMG, 7);
                        MGS2Cheat.VRNoFallDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_FALL_DMG, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_FALL_DMG, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRInfiniteStrength(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRInfiniteStrength;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRInfiniteStrAoB, MGS2Offset.VR_INF_STR, 7);
                        MGS2Cheat.VRInfiniteStrength = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_STR, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_STR, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRGripDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRGripDamage;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRGripDamageAoB, MGS2Offset.VR_TAKE_GRIP_DMG, 7);
                        MGS2Cheat.VRGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_TAKE_GRIP_DMG, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_TAKE_GRIP_DMG, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRAimStab(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRAimStability;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(MGS2AoB.VRAimStabilityAoB, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, MGS2Offset.VR_AIM_STAB);
                        MGS2Cheat.VRAimStability = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, MGS2Offset.VR_AIM_STAB);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_AIM_STAB, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRInfiniteAmmo(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRInfiniteAmmo;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRInfiniteAmmoAoB, MGS2Offset.VR_INF_AMMO, 3);
                        MGS2Cheat.VRInfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_AMMO, 3);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_AMMO, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRInfiniteItem(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRInfiniteItem;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRInfiniteItemAoB, MGS2Offset.VR_INF_ITEM, 4);
                        MGS2Cheat.VRInfiniteItem = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_ITEM, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_INF_ITEM, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VRNoReload(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.VRNoReload;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.VRNoReloadAoB, MGS2Offset.VR_NO_RELOAD, 2);
                        MGS2Cheat.VRNoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_RELOAD, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.VR_NO_RELOAD, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void EmmaInfiniteHp(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.EmmaInfiniteHealth;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.EmmaInfiniteHpAoB, MGS2Offset.EMMA_INF_HP, 2);
                        MGS2Cheat.EmmaInfiniteHealth = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.EMMA_INF_HP, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.EMMA_INF_HP, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void EmmaInfiniteO2(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.EmmaInfiniteO2;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(MGS2AoB.EmmaInfiniteO2AoB, MGS2Offset.EMMA_INF_O2, 2);
                        MGS2Cheat.EmmaInfiniteO2 = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, MGS2Offset.EMMA_INF_O2, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, MGS2Offset.EMMA_INF_O2, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }
        }
    }    

    public class MGS2Cheat
    {
        public static Cheat BlackScreen { get; internal set; } = new Cheat("Black Screen", Cheat.CheatActions.TurnScreenBlack, MGS2AoB.OriginalCameraBytes);
        public static Cheat NoBleedDamage { get; internal set; } = new Cheat("Bleeding Causes No Damage", Cheat.CheatActions.TurnOffBleedDamage, MGS2AoB.OriginalBleedDamageBytes);
        public static Cheat NoBurnDamage { get; internal set; } = new Cheat("Burning Causes No Damage", Cheat.CheatActions.TurnOffBurnDamage, MGS2AoB.OriginalBurnDamageBytes);
        public static Cheat InfiniteAmmo { get; internal set; } = new Cheat("Infinite Ammo", Cheat.CheatActions.InfiniteAmmo, MGS2AoB.OriginalAmmoBytes);
        public static Cheat InfiniteLife { get; internal set; } = new Cheat("Enemies Deal No Damage", Cheat.CheatActions.InfiniteLife, MGS2AoB.OriginalLifeBytes);
        public static Cheat InfiniteOxygen { get; internal set; } = new Cheat("Infinite Oxygen", Cheat.CheatActions.InfiniteOxygen, MGS2AoB.OriginalO2Bytes);
        public static Cheat Letterboxing { get; internal set; } = new Cheat("Letterboxing", Cheat.CheatActions.Letterboxing, MGS2AoB.OriginalCameraBytes);
        public static Cheat NoReload { get; internal set; } = new Cheat("Reloading Not Required", Cheat.CheatActions.AmmoNeverDepletes, MGS2AoB.OriginalReloadBytes);
        public static Cheat NoClipWithGravity { get; internal set; } = new Cheat("Walk Through Walls (gravity)", Cheat.CheatActions.NoClipWithGravity, MGS2AoB.OriginalClippingBytes);
        public static Cheat NoClipNoGravity { get; internal set; } = new Cheat("Walk Through Walls (no gravity)", Cheat.CheatActions.NoClipNoGravity, MGS2AoB.OriginalClippingBytes);
        public static Cheat ZoomIn { get; internal set; } = new Cheat("Zoom In", Cheat.CheatActions.ZoomIn, MGS2AoB.OriginalCameraBytes);
        public static Cheat ZoomOut { get; internal set; } = new Cheat("Zoom Out", Cheat.CheatActions.ZoomOut, MGS2AoB.OriginalCameraBytes);
        public static Cheat NoGripDamage { get; internal set; } = new Cheat("Infinite Grip Stamina", Cheat.CheatActions.GripNeverDepletes, MGS2AoB.OriginalGripDamageBytes);
        public static Cheat DisablePauseButton { get; internal set; } = new Cheat("Disable Pause Button", Cheat.CheatActions.TurnOffPauseButton, MGS2AoB.OriginalPauseButtonBytes);
        public static Cheat DisableItemMenuPause { get; internal set; } = new Cheat("Disable Item Menu Pause", Cheat.CheatActions.TurnOffItemMenuPause, MGS2AoB.OriginalItemMenuPauseBytes);
        public static Cheat DisableWeaponMenuPause { get; internal set; } = new Cheat("Disable Weapon Menu Pause", Cheat.CheatActions.TurnOffWeaponMenuPause, MGS2AoB.OriginalWeaponMenuPauseBytes);
        public static Cheat InfiniteItems { get; internal set; } = new Cheat("Infinite Item Uses", Cheat.CheatActions.InfiniteItems, MGS2AoB.OriginalItemUseBytes);
        public static Cheat MaxStackOnPickup { get; internal set; } = new Cheat("Max Stack on Pickup", Cheat.CheatActions.MaxStackOnPickup, MGS2AoB.OriginalCountOnPickup);
        public static Cheat InfiniteKnockout { get; internal set; } = new Cheat("Infinite Knockout/Tranq Duration", Cheat.CheatActions.InfiniteKnockout, MGS2AoB.OriginalKnockoutDuration);
        public static Cheat RemovePlantFilter { get; internal set; } = new Cheat("Remove Plant Washout Filter", Cheat.CheatActions.RemovePlantFilter, MGS2AoB.OriginalRemovePlantFilterBytes);
        public static Cheat RemovePlantFog { get; internal set; } = new Cheat("Remove Plant Fog", Cheat.CheatActions.RemovePlantFog, MGS2AoB.OriginalPlantFogBytes);
        public static Cheat RemoveTankerFilter { get; internal set; } = new Cheat("Remove Tanker Filters & Effects", Cheat.CheatActions.RemoveTankerEffects, MGS2AoB.OriginalRemoveTankerFilterBytes);
        public static Cheat NightTime { get; internal set; } = new Cheat("Make it Night-time", Cheat.CheatActions.NightTime, MGS2AoB.OriginalNightTimeBytes);
        public static Cheat EnableCustomFilter { get; internal set; } = new Cheat("Enable Custom Filter", Cheat.CheatActions.EnableCustomFilter, MGS2AoB.OriginalCustomFilteringBytes);
        public static Cheat PauseVRTimer { get; internal set; } = new Cheat("Pause VR Timer", Cheat.CheatActions.PauseVRTimer, MGS2AoB.OriginalPauseVRBytes);
        public static Cheat VRObjectiveAutoComplete { get; internal set; } = new Cheat("Auto Complete VR objectives", Cheat.CheatActions.AutoCompleteVRObjectives, MGS2AoB.OriginalVRObjectiveBytes);
        public static Cheat VREnemiesAutoComplete { get; internal set; } = new Cheat("Auto 'Kill' VR Enemies", Cheat.CheatActions.AutoCompleteVREnemies, MGS2AoB.OriginalVREnemiesBytes);
        public static Cheat VRNoHitDamage { get; internal set; } = new Cheat("Take No Damage in VR", Cheat.CheatActions.VRNoHitDamage, MGS2AoB.OriginalVRNoHitDamageBytes);
        public static Cheat VRNoFallDamage { get; internal set; } = new Cheat("Take No Fall Damage in VR", Cheat.CheatActions.VRNoFallDamage, MGS2AoB.OriginalVRNoFallDamageBytes);
        public static Cheat VRInfiniteStrength { get; internal set; } = new Cheat("Infinite VR Strength", Cheat.CheatActions.VRInfiniteStrength, MGS2AoB.OriginalVRInfiniteStrBytes);
        public static Cheat VRGripDamage { get; internal set; } = new Cheat("VR Grip Damage", Cheat.CheatActions.VRGripDamage, MGS2AoB.OriginalVRGripDamageBytes);
        public static Cheat VRAimStability { get; internal set; } = new Cheat("VR Aim Stability", Cheat.CheatActions.VRAimStab, MGS2AoB.OriginalVRAimStabilityBytes);
        public static Cheat VRInfiniteAmmo { get; internal set; } = new Cheat("VR Infinite Ammo", Cheat.CheatActions.VRInfiniteAmmo, MGS2AoB.OriginalVRInfiniteAmmoBytes);
        public static Cheat VRInfiniteItem { get; internal set; } = new Cheat("VR Infinite Items", Cheat.CheatActions.VRInfiniteItem, MGS2AoB.OriginalVRInfiniteItemBytes);
        public static Cheat VRNoReload { get; internal set; } = new Cheat("VR No Reload", Cheat.CheatActions.VRNoReload, MGS2AoB.OriginalVRNoReloadBytes);
        public static Cheat EmmaInfiniteHealth { get; internal set; } = new Cheat("Emma Infinite Health", Cheat.CheatActions.EmmaInfiniteHp, MGS2AoB.OriginalEmmaHpBytes);
        public static Cheat EmmaInfiniteO2 { get; internal set; } = new Cheat("Emma Infinite O2", Cheat.CheatActions.EmmaInfiniteO2, MGS2AoB.OriginalEmmaO2Bytes);

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage, 
                EmmaInfiniteHealth, EmmaInfiniteO2, NoClipWithGravity, NoClipNoGravity, 
                NoReload,/*ZoomIn, ZoomOut,*/ DisablePauseButton, //zoom in and out aren't working as expected, and i cant be bothered to fix them right now.
                DisableItemMenuPause, DisableWeaponMenuPause, InfiniteItems, InfiniteKnockout, RemovePlantFilter,
                RemovePlantFog, RemoveTankerFilter, NightTime, MaxStackOnPickup, PauseVRTimer, VRObjectiveAutoComplete,
                VREnemiesAutoComplete, VRNoHitDamage, VRNoFallDamage, VRInfiniteStrength, VRGripDamage, VRAimStability,
                VRInfiniteAmmo, VRInfiniteItem, VRNoReload, BlackScreen, Letterboxing
            };
        }

        public static List<Cheat> CheatList
        {
            get
            {
                if (_cheatList == null)
                {
                    BuildCheatList();
                };

                return _cheatList;
            }
        }
    }
    
}
