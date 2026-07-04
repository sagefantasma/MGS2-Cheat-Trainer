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

namespace MGS2_CheatTrainer_V2
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
                                        if(memoryContent.Length > i)
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

            public static void RestartLevel()
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
                                spp.ModifyProcessOffset(new IntPtr(0x153F048), 1);
                                successful = true;
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            public static void TurnScreenBlack(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.BlackScreen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.OriginalBytes = ReadMemory(Mgs2AoB.Camera, Mgs2Offset.BLACK_SCREEN);
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.BLACK_SCREEN, 0x00);
                        MGS2Cheat.BlackScreen = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.BLACK_SCREEN, 0x00);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.BLACK_SCREEN, 0x40);
            }

            public static void TurnOffBleedDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoBleedDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBleedDamage, Mgs2Offset.NO_BLEED_DMG, 7);
                        MGS2Cheat.NoBleedDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_BLEED_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_BLEED_DMG, Mgs2AoB.OriginalBleedDamageBytes);
            }

            public static void TurnOffBurnDamage(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoBurnDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBurnDamage, Mgs2Offset.NO_BURN_DMG, 7);
                        MGS2Cheat.NoBurnDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_BURN_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_BLEED_DMG, Mgs2AoB.OriginalBurnDamageBytes);
            }

            internal static void InfiniteAmmo(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteAmmo;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteAmmo, Mgs2Offset.INFINITE_AMMO, 4);
                        MGS2Cheat.InfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_AMMO, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_AMMO, Mgs2AoB.OriginalAmmoBytes);
            }

            internal static void InfiniteLife(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteLife;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteLife, Mgs2Offset.INFINITE_LIFE, 4);
                        MGS2Cheat.InfiniteLife = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_LIFE, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_LIFE, Mgs2AoB.OriginalLifeBytes);
            }

            internal static void InfiniteOxygen(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteOxygen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteO2, Mgs2Offset.INFINITE_O2, 4);
                        MGS2Cheat.InfiniteOxygen = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_O2, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_O2, Mgs2AoB.OriginalO2Bytes);
            }

            internal static void Letterboxing(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.Letterboxing;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.LETTERBOX, 0x00);
                        MGS2Cheat.Letterboxing = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.LETTERBOX, 0x01);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.LETTERBOX, 0x01);
            }

            internal static void AmmoNeverDepletes(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoReload;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NeverReload, Mgs2Offset.NEVER_RELOAD, 2);
                        MGS2Cheat.NoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NEVER_RELOAD, 2);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NEVER_RELOAD, Mgs2AoB.OriginalReloadBytes);
            }

            internal static void GripNeverDepletes(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.NoGripDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.DecrementGripGauge, Mgs2Offset.NO_GRIP_DMG, 7);
                        MGS2Cheat.NoGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_GRIP_DMG, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_GRIP_DMG, Mgs2AoB.OriginalGripDamageBytes);
            }

            internal static void TurnOffPauseButton(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisablePauseButton;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InGamePause, Mgs2Offset.NO_PAUSE_BTN, 5);
                        MGS2Cheat.DisablePauseButton = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_PAUSE_BTN, 5);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_PAUSE_BTN, Mgs2AoB.OriginalPauseButtonBytes);
            }

            internal static void TurnOffItemMenuPause(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisableItemMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.ItemMenuPause, Mgs2Offset.NO_ITEM_PAUSE, 6);
                        MGS2Cheat.DisableItemMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_ITEM_PAUSE, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_ITEM_PAUSE, Mgs2AoB.OriginalItemMenuPauseBytes);
            }

            internal static void TurnOffWeaponMenuPause(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DisableWeaponMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.WeaponMenuPause, Mgs2Offset.NO_WEAPON_PAUSE, 6);
                        MGS2Cheat.DisableWeaponMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NO_WEAPON_PAUSE, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NO_WEAPON_PAUSE, Mgs2AoB.OriginalWeaponMenuPauseBytes);
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
                                    activeCharacterAoB = Mgs2AoB.RaidenClipping;
                                    break;
                                case Constants.PlayableCharacter.NinjaRaiden:
                                    activeCharacterAoB = Mgs2AoB.NinjaClipping;
                                    break;
                                case Constants.PlayableCharacter.NakedRaiden:
                                    activeCharacterAoB = Mgs2AoB.NakedRaidenClipping;
                                    break;
                                case Constants.PlayableCharacter.Snake:
                                    activeCharacterAoB = Mgs2AoB.SnakeClipping;
                                    break;
                                case Constants.PlayableCharacter.Pliskin:
                                    activeCharacterAoB = Mgs2AoB.PliskinClipping;
                                    break;
                                case Constants.PlayableCharacter.MGS1Snake:
                                    activeCharacterAoB = Mgs2AoB.MGS1SnakeClipping;
                                    break;
                                case Constants.PlayableCharacter.TuxedoSnake:
                                    activeCharacterAoB = Mgs2AoB.TuxedoSnakeClipping;
                                    break;
                                default:
                                    activeCharacterAoB = Mgs2AoB.VRClipping;
                                    break;
                            }

                            IntPtr pointerLocation = spp.FollowPointer(new IntPtr(Mgs2Pointer.WalkThroughWalls), false);
                            byte[] memoryContent = spp.GetMemoryFromPointer(new IntPtr(pointerLocation.ToInt64() + Mgs2Offset.NO_CLIP.Start), Mgs2Offset.NO_CLIP.Length);

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

                            spp.SetMemoryAtPointer(new IntPtr(pointerLocation.ToInt64() + Mgs2Offset.NO_CLIP.Start), memoryContent);
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
                byte[] currentZoom = ReadMemory(Mgs2AoB.Camera, Mgs2Offset.ZOOM);

                if (currentZoom == null)
                    return;

                Cheat activeCheat = zoomIn ? MGS2Cheat.ZoomIn : MGS2Cheat.ZoomOut;
                if (zoomIn)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.ZOOM, currentZoom[0]++);
                        MGS2Cheat.ZoomIn = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.ZOOM, currentZoom[0]++);
                    }
                }
                else
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.ZOOM, currentZoom[0]--);
                        MGS2Cheat.ZoomOut = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.ZOOM, currentZoom[0]--);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteItemUse, Mgs2Offset.INFINITE_ITEMS, 4);
                        MGS2Cheat.InfiniteItems = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_ITEMS, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INFINITE_ITEMS, Mgs2AoB.OriginalItemUseBytes);
            }

            internal static void MaxStackOnPickup(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.MaxStackOnPickup;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.MaxCountOnPickup, Mgs2Offset.MAX_ON_PICKUP, 4);
                        MGS2Cheat.MaxStackOnPickup = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.MAX_ON_PICKUP, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.MAX_ON_PICKUP, Mgs2AoB.OriginalCountOnPickup);
            }

            internal static void InfiniteKnockout(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InfiniteKnockout;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.KnockoutDuration, Mgs2Offset.KNOCKOUT_DURATION, 8);
                        MGS2Cheat.InfiniteKnockout = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.KNOCKOUT_DURATION, 8);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.KNOCKOUT_DURATION, Mgs2AoB.OriginalKnockoutDuration);
                
            }

            internal static void RemovePlantFilter(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.RemovePlantFilter;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFilter, Mgs2Offset.REMOVE_PLANT_FILTER, 7);
                        MGS2Cheat.RemovePlantFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.REMOVE_PLANT_FILTER, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.REMOVE_PLANT_FILTER, Mgs2AoB.OriginalRemovePlantFilterBytes);
            }

            internal static void RemovePlantFog(bool activate)
            {
                byte[] DisableFog = new byte[] { 0x46 };

                Cheat activeCheat = MGS2Cheat.RemovePlantFog;
                if (activate)
                {
                    if(activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        byte[] originalValue = ReadMemory(Mgs2AoB.RemovePlantFog, Mgs2Offset.REMOVE_PLANT_FOG); //this is incorrect
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFog, Mgs2Offset.REMOVE_PLANT_FOG, 5);
                        activeCheat.OriginalBytes = originalValue;
                        MGS2Cheat.RemovePlantFog = activeCheat;
                    }
                    else
                    {
                        ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.REMOVE_PLANT_FOG, Mgs2AoB.OriginalPlantFogBytes);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.REMOVE_PLANT_FOG, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.RemoveTankerFilter, disableFilter, Mgs2Offset.REMOVE_TANKER_FILTER);
                        MGS2Cheat.RemoveTankerFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, disableFilter, Mgs2Offset.REMOVE_TANKER_FILTER);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.REMOVE_TANKER_FILTER, enableFilter);
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
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.NightTime, nightTime, Mgs2Offset.NIGHT_TIME);
                        MGS2Cheat.NightTime = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, nightTime, Mgs2Offset.NIGHT_TIME);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NIGHT_TIME, dayTime);
            }

            internal static void EnableCustomFilter(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.EnableCustomFilter;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EnableCustomFiltering, Mgs2Offset.ENABLE_CUSTOM_FILTER, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                        MGS2Cheat.EnableCustomFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.ENABLE_CUSTOM_FILTER, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.ENABLE_CUSTOM_FILTER, Mgs2AoB.OriginalCustomFilteringBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static async Task ApplyColorFilter(Color chosenColor)
            {
                byte[] customColor = new byte[] { chosenColor.R, chosenColor.G, chosenColor.B };

                ReplaceWithSpecificCode(Mgs2AoB.CustomFilteringAoB, customColor, Mgs2Offset.CUSTOM_FILTERING);
                
                if(!customFilterCancellationTokenSource.IsCancellationRequested)
                    await PeriodicTask.Run(() => ReapplyColorFilter(customColor), TimeSpan.FromMilliseconds(1000), customFilterCancellationTokenSource.Token);
            }

            private static void ReapplyColorFilter(byte[] chosenColor)
            {
                byte[] currentColor = ReadMemory(Mgs2AoB.CustomFilteringAoB, Mgs2Offset.CUSTOM_FILTERING);

                if (!currentColor.SequenceEqual(chosenColor))
                {
                    ReplaceWithSpecificCode(Mgs2AoB.CustomFilteringAoB, chosenColor, Mgs2Offset.CUSTOM_FILTERING);
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
                        activeCheat.OriginalBytes = ReadMemory(Mgs2AoB.PauseVRAoB, Mgs2Offset.PAUSE_VR_TIMER);
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.PauseVRAoB, Mgs2Offset.PAUSE_VR_TIMER, 6, 2);
                        MGS2Cheat.PauseVRTimer = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.PAUSE_VR_TIMER, 6, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.PAUSE_VR_TIMER, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRObjectiveAoB, Mgs2Offset.VR_AUTO_COMPLETE_OBJECTIVES, 6);
                        MGS2Cheat.VRObjectiveAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_AUTO_COMPLETE_OBJECTIVES, 6);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_AUTO_COMPLETE_OBJECTIVES, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRObjectiveAoB, Mgs2Offset.VR_AUTO_COMPLETE_ENEMIES, 2);
                        MGS2Cheat.VREnemiesAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_AUTO_COMPLETE_ENEMIES, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_AUTO_COMPLETE_ENEMIES, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRNoHitDamageAoB, Mgs2Offset.VR_NO_HIT_DMG, 4);
                        MGS2Cheat.VRNoHitDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_HIT_DMG, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_HIT_DMG, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRNoFallDamageAoB, Mgs2Offset.VR_NO_FALL_DMG, 7);
                        MGS2Cheat.VRNoFallDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_FALL_DMG, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_FALL_DMG, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRInfiniteStrAoB, Mgs2Offset.VR_INF_STR, 7);
                        MGS2Cheat.VRInfiniteStrength = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_STR, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_STR, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRGripDamageAoB, Mgs2Offset.VR_TAKE_GRIP_DMG, 7);
                        MGS2Cheat.VRGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_TAKE_GRIP_DMG, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_TAKE_GRIP_DMG, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.VRAimStabilityAoB, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VR_AIM_STAB);
                        MGS2Cheat.VRAimStability = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VR_AIM_STAB);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_AIM_STAB, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRInfiniteAmmoAoB, Mgs2Offset.VR_INF_AMMO, 3);
                        MGS2Cheat.VRInfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_AMMO, 3);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_AMMO, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRInfiniteItemAoB, Mgs2Offset.VR_INF_ITEM, 4);
                        MGS2Cheat.VRInfiniteItem = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_ITEM, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_INF_ITEM, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VRNoReloadAoB, Mgs2Offset.VR_NO_RELOAD, 2);
                        MGS2Cheat.VRNoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_RELOAD, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VR_NO_RELOAD, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteHpAoB, Mgs2Offset.EMMA_INF_HP, 2);
                        MGS2Cheat.EmmaInfiniteHealth = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.EMMA_INF_HP, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.EMMA_INF_HP, activeCheat.OriginalBytes);
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
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteO2AoB, Mgs2Offset.EMMA_INF_O2, 2);
                        MGS2Cheat.EmmaInfiniteO2 = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.EMMA_INF_O2, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.EMMA_INF_O2, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void InvisibleToGuards(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InvisibleToGuards;
                byte[] invisibleToGuards = new byte[] { 0xFF, 0xFF, 0x31, 0xC0, 0x48, 0x83, 0xC4, 0x20, 0x5B, 0xC3 };
                // FF FF 31 C0 48 83 C4 20 5B C3 
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToGuardsAoB, invisibleToGuards, Mgs2Offset.INVISIBLE_TO_GUARDS);
                        MGS2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, invisibleToGuards, Mgs2Offset.INVISIBLE_TO_GUARDS);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INVISIBLE_TO_GUARDS, activeCheat.OriginalBytes);
                }
            }

            internal static void InvisibleToCyphers(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InvisibleToCyphers;
                byte[] invisibleToCyphers = new byte[] { 0x48, 0x39, 0xE0, 0x0F, 0x1F, 0x40, 0x00, 0x0F, 0x85, 0x4C, 0x04 };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToCyphersAoB, invisibleToCyphers, Mgs2Offset.INVISIBLE_TO_CYPHERS);
                        MGS2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, invisibleToCyphers, Mgs2Offset.INVISIBLE_TO_CYPHERS);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INVISIBLE_TO_CYPHERS, activeCheat.OriginalBytes);
                }
            }

            internal static void InvisibleToCameras(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.InvisibleToCameras;
                byte[] invisibleToCameras = new byte[] { 0x0F, 0x1F, 0x40, 0x00, 0xE8, 0x13, 0xF8, 0x1C }; //this was from snakeswiss' original implementation, but doesnt work on 2.0.1. thankfully, disabling the first command(first 4 bytes) works instead :)
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.INVISIBLE_TO_CAMERAS, 4);
                        MGS2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.INVISIBLE_TO_CAMERAS, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.INVISIBLE_TO_CAMERAS, activeCheat.OriginalBytes);
                }
            }

            internal static void DeafenGuardsToKnocks(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DeafenGuardsToKnocks;
                byte[] deafenedToKnocks = new byte[] { 0xA8, 0x01, 0xEB, 0x1D, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToKnocksAoB, deafenedToKnocks, Mgs2Offset.DEAFEN_GUARDS_TO_KNOCKS);
                        MGS2Cheat.DeafenGuardsToKnocks = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, deafenedToKnocks, Mgs2Offset.DEAFEN_GUARDS_TO_KNOCKS);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.DEAFEN_GUARDS_TO_KNOCKS, activeCheat.OriginalBytes);
                }
            }

            internal static void DeafenGuardsToGuns(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.DeafenGuardsToGuns;
                byte[] deafenedToGuns = new byte[] { 0xA9, 0x00, 0x18, 0x00, 0x00, 0xEB, 0x12, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToGunsAoB, deafenedToGuns, Mgs2Offset.DEAFEN_GUARDS_TO_GUNS);
                        MGS2Cheat.DeafenGuardsToGuns = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, deafenedToGuns, Mgs2Offset.DEAFEN_GUARDS_TO_GUNS);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.DEAFEN_GUARDS_TO_GUNS, activeCheat.OriginalBytes);
                }
            }

            internal static void GhostMode(bool activate)
            {
                InvisibleToGuards(activate);
                InvisibleToCyphers(activate);
                InvisibleToCameras(activate);
                DeafenGuardsToKnocks(activate);
                DeafenGuardsToGuns(activate);
            }

            internal static void TurnOffMusic(bool activate)
            {
                Cheat activeCheat = MGS2Cheat.TurnOffMusic;
                if (activate)
                {
                    customFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.TurnOffMusicAoB, Mgs2Offset.TURN_OFF_MUSIC, 7);
                        MGS2Cheat.TurnOffMusic = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.TURN_OFF_MUSIC, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.TURN_OFF_MUSIC, activeCheat.OriginalBytes);
                    customFilterCancellationTokenSource.Cancel();
                }
            }
        }
    }    

    public class MGS2Cheat
    {
        public static Cheat BlackScreen { get; internal set; } = new Cheat("Black Screen", Cheat.CheatActions.TurnScreenBlack, Mgs2AoB.OriginalCameraBytes);
        public static Cheat NoBleedDamage { get; internal set; } = new Cheat("Bleeding Causes No Damage", Cheat.CheatActions.TurnOffBleedDamage, Mgs2AoB.OriginalBleedDamageBytes);
        public static Cheat NoBurnDamage { get; internal set; } = new Cheat("Burning Causes No Damage", Cheat.CheatActions.TurnOffBurnDamage, Mgs2AoB.OriginalBurnDamageBytes);
        public static Cheat InfiniteAmmo { get; internal set; } = new Cheat("Infinite Ammo", Cheat.CheatActions.InfiniteAmmo, Mgs2AoB.OriginalAmmoBytes);
        public static Cheat InfiniteLife { get; internal set; } = new Cheat("Enemies Deal No Damage", Cheat.CheatActions.InfiniteLife, Mgs2AoB.OriginalLifeBytes);
        public static Cheat InfiniteOxygen { get; internal set; } = new Cheat("Infinite Oxygen", Cheat.CheatActions.InfiniteOxygen, Mgs2AoB.OriginalO2Bytes);
        public static Cheat Letterboxing { get; internal set; } = new Cheat("Letterboxing", Cheat.CheatActions.Letterboxing, Mgs2AoB.OriginalCameraBytes);
        public static Cheat NoReload { get; internal set; } = new Cheat("Reloading Not Required", Cheat.CheatActions.AmmoNeverDepletes, Mgs2AoB.OriginalReloadBytes);
        public static Cheat NoClipWithGravity { get; internal set; } = new Cheat("Walk Through Walls (gravity)", Cheat.CheatActions.NoClipWithGravity, Mgs2AoB.OriginalClippingBytes);
        public static Cheat NoClipNoGravity { get; internal set; } = new Cheat("Walk Through Walls (no gravity)", Cheat.CheatActions.NoClipNoGravity, Mgs2AoB.OriginalClippingBytes);
        public static Cheat ZoomIn { get; internal set; } = new Cheat("Zoom In", Cheat.CheatActions.ZoomIn, Mgs2AoB.OriginalCameraBytes);
        public static Cheat ZoomOut { get; internal set; } = new Cheat("Zoom Out", Cheat.CheatActions.ZoomOut, Mgs2AoB.OriginalCameraBytes);
        public static Cheat NoGripDamage { get; internal set; } = new Cheat("Infinite Grip Stamina", Cheat.CheatActions.GripNeverDepletes, Mgs2AoB.OriginalGripDamageBytes);
        public static Cheat DisablePauseButton { get; internal set; } = new Cheat("Disable Pause Button", Cheat.CheatActions.TurnOffPauseButton, Mgs2AoB.OriginalPauseButtonBytes);
        public static Cheat DisableItemMenuPause { get; internal set; } = new Cheat("Disable Item Menu Pause", Cheat.CheatActions.TurnOffItemMenuPause, Mgs2AoB.OriginalItemMenuPauseBytes);
        public static Cheat DisableWeaponMenuPause { get; internal set; } = new Cheat("Disable Weapon Menu Pause", Cheat.CheatActions.TurnOffWeaponMenuPause, Mgs2AoB.OriginalWeaponMenuPauseBytes);
        public static Cheat InfiniteItems { get; internal set; } = new Cheat("Infinite Item Uses", Cheat.CheatActions.InfiniteItems, Mgs2AoB.OriginalItemUseBytes);
        public static Cheat MaxStackOnPickup { get; internal set; } = new Cheat("Max Stack on Pickup", Cheat.CheatActions.MaxStackOnPickup, Mgs2AoB.OriginalCountOnPickup);
        public static Cheat InfiniteKnockout { get; internal set; } = new Cheat("Infinite Knockout/Tranq Duration", Cheat.CheatActions.InfiniteKnockout, Mgs2AoB.OriginalKnockoutDuration);
        public static Cheat RemovePlantFilter { get; internal set; } = new Cheat("Remove Plant Washout Filter", Cheat.CheatActions.RemovePlantFilter, Mgs2AoB.OriginalRemovePlantFilterBytes);
        public static Cheat RemovePlantFog { get; internal set; } = new Cheat("Remove Plant Fog", Cheat.CheatActions.RemovePlantFog, Mgs2AoB.OriginalPlantFogBytes);
        public static Cheat RemoveTankerFilter { get; internal set; } = new Cheat("Remove Tanker Filters & Effects", Cheat.CheatActions.RemoveTankerEffects, Mgs2AoB.OriginalRemoveTankerFilterBytes);
        public static Cheat NightTime { get; internal set; } = new Cheat("Make it Night-time", Cheat.CheatActions.NightTime, Mgs2AoB.OriginalNightTimeBytes);
        public static Cheat EnableCustomFilter { get; internal set; } = new Cheat("Enable Custom Filter", Cheat.CheatActions.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes);
        public static Cheat PauseVRTimer { get; internal set; } = new Cheat("Pause VR Timer", Cheat.CheatActions.PauseVRTimer, Mgs2AoB.OriginalPauseVRBytes);
        public static Cheat VRObjectiveAutoComplete { get; internal set; } = new Cheat("Auto Complete VR objectives", Cheat.CheatActions.AutoCompleteVRObjectives, Mgs2AoB.OriginalVRObjectiveBytes);
        public static Cheat VREnemiesAutoComplete { get; internal set; } = new Cheat("Auto 'Kill' VR Enemies", Cheat.CheatActions.AutoCompleteVREnemies, Mgs2AoB.OriginalVREnemiesBytes);
        public static Cheat VRNoHitDamage { get; internal set; } = new Cheat("Take No Damage in VR", Cheat.CheatActions.VRNoHitDamage, Mgs2AoB.OriginalVRNoHitDamageBytes);
        public static Cheat VRNoFallDamage { get; internal set; } = new Cheat("Take No Fall Damage in VR", Cheat.CheatActions.VRNoFallDamage, Mgs2AoB.OriginalVRNoFallDamageBytes);
        public static Cheat VRInfiniteStrength { get; internal set; } = new Cheat("Infinite VR Strength", Cheat.CheatActions.VRInfiniteStrength, Mgs2AoB.OriginalVRInfiniteStrBytes);
        public static Cheat VRGripDamage { get; internal set; } = new Cheat("VR Grip Damage", Cheat.CheatActions.VRGripDamage, Mgs2AoB.OriginalVRGripDamageBytes);
        public static Cheat VRAimStability { get; internal set; } = new Cheat("VR Aim Stability", Cheat.CheatActions.VRAimStab, Mgs2AoB.OriginalVRAimStabilityBytes);
        public static Cheat VRInfiniteAmmo { get; internal set; } = new Cheat("VR Infinite Ammo", Cheat.CheatActions.VRInfiniteAmmo, Mgs2AoB.OriginalVRInfiniteAmmoBytes);
        public static Cheat VRInfiniteItem { get; internal set; } = new Cheat("VR Infinite Items", Cheat.CheatActions.VRInfiniteItem, Mgs2AoB.OriginalVRInfiniteItemBytes);
        public static Cheat VRNoReload { get; internal set; } = new Cheat("VR No Reload", Cheat.CheatActions.VRNoReload, Mgs2AoB.OriginalVRNoReloadBytes);
        public static Cheat EmmaInfiniteHealth { get; internal set; } = new Cheat("Emma Infinite Health(CRASHES SNIPING SECTION)", Cheat.CheatActions.EmmaInfiniteHp, Mgs2AoB.OriginalEmmaHpBytes);
        public static Cheat EmmaInfiniteO2 { get; internal set; } = new Cheat("Emma Infinite O2(CRASHES SNIPING SECTION)", Cheat.CheatActions.EmmaInfiniteO2, Mgs2AoB.OriginalEmmaO2Bytes);
        public static Cheat InvisibleToGuards { get; internal set; } = new Cheat("Invisible to Guards", Cheat.CheatActions.InvisibleToGuards, Mgs2AoB.OriginalInvisibleToGuardsBytes);
        public static Cheat InvisibleToCyphers { get; internal set; } = new Cheat("Invisible to Cyphers", Cheat.CheatActions.InvisibleToCyphers, Mgs2AoB.OriginalInvisibleToCyphersBytes);
        public static Cheat InvisibleToCameras { get; internal set; } = new Cheat("Invisible to Cameras", Cheat.CheatActions.InvisibleToCameras, Mgs2AoB.OriginalInvisibleToCamerasBytes);
        public static Cheat DeafenGuardsToKnocks { get; internal set; } = new Cheat("Deafen Guards to Knocks", Cheat.CheatActions.DeafenGuardsToKnocks, Mgs2AoB.OriginalDeafenGuardsToKnocksBytes);
        public static Cheat DeafenGuardsToGuns { get; internal set; } = new Cheat("Deafen Guards to Guns", Cheat.CheatActions.DeafenGuardsToGuns, Mgs2AoB.OriginalDeafenGuardsToGunsBytes);
        public static Cheat GhostMode { get; internal set; } = new Cheat("Ghost Mode", Cheat.CheatActions.GhostMode, null);
        public static Cheat TurnOffMusic { get; internal set; } = new Cheat("Turn off Game Music(does not apply to cutscenes)", Cheat.CheatActions.TurnOffMusic, Mgs2AoB.OriginalTurnOffMusicBytes);

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage, 
                /*EmmaInfiniteHealth,*/ EmmaInfiniteO2, NoClipWithGravity, NoClipNoGravity,  //Emma health is crashing the game and i cba to fix it
                NoReload,/*ZoomIn, ZoomOut,*/ DisablePauseButton, //zoom in and out aren't working as expected, and i cant be bothered to fix them right now.
                DisableItemMenuPause, DisableWeaponMenuPause, InfiniteItems, InfiniteKnockout, RemovePlantFilter,
                RemovePlantFog, RemoveTankerFilter, NightTime, MaxStackOnPickup, PauseVRTimer, VRObjectiveAutoComplete,
                /*VREnemiesAutoComplete,*/ VRNoHitDamage, VRNoFallDamage, VRInfiniteStrength, VRGripDamage, VRAimStability, //VR Enemies autocomplete is crashing the game
                VRInfiniteAmmo, VRInfiniteItem, VRNoReload, BlackScreen, Letterboxing, GhostMode, TurnOffMusic
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
