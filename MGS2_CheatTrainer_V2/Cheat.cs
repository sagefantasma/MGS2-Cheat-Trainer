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
    //REWRITE STATUS: Seems error free, but this class does desperately need attention outside a rewrite.
    public struct Cheat
    {
        public string Name { get; private set; }
        public Action<bool> CheatAction { get; private set; }
        public byte[] OriginalBytes { get; private set; }
        public IntPtr CodeLocation { get; set; }
        private static CancellationTokenSource CustomFilterCancellationTokenSource { get; set; }
        private static Color CustomFilterColor { get; set; }

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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                Cheat activeCheat = Mgs2Cheat.BlackScreen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.OriginalBytes = ReadMemory(Mgs2AoB.Camera, Mgs2Offset.BlackScreen);
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.BlackScreen, 0x00);
                        Mgs2Cheat.BlackScreen = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.BlackScreen, 0x00);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.BlackScreen, 0x40);
            }

            public static void TurnOffBleedDamage(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.NoBleedDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBleedDamage, Mgs2Offset.NoBleedDmg, 7);
                        Mgs2Cheat.NoBleedDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoBleedDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoBleedDmg, Mgs2AoB.OriginalBleedDamageBytes);
            }

            public static void TurnOffBurnDamage(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.NoBurnDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBurnDamage, Mgs2Offset.NoBurnDmg, 7);
                        Mgs2Cheat.NoBurnDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoBurnDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoBleedDmg, Mgs2AoB.OriginalBurnDamageBytes);
            }

            internal static void InfiniteAmmo(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InfiniteAmmo;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteAmmo, Mgs2Offset.InfiniteAmmo, 4);
                        Mgs2Cheat.InfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteAmmo, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteAmmo, Mgs2AoB.OriginalAmmoBytes);
            }

            internal static void InfiniteLife(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InfiniteLife;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteLife, Mgs2Offset.InfiniteLife, 4);
                        Mgs2Cheat.InfiniteLife = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteLife, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteLife, Mgs2AoB.OriginalLifeBytes);
            }

            internal static void InfiniteOxygen(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InfiniteOxygen;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteO2, Mgs2Offset.InfiniteO2, 4);
                        Mgs2Cheat.InfiniteOxygen = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteO2, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteO2, Mgs2AoB.OriginalO2Bytes);
            }

            internal static void Letterboxing(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.Letterboxing;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Letterbox, 0x00);
                        Mgs2Cheat.Letterboxing = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.Letterbox, 0x01);
                    }
                }
                else
                    ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.Letterbox, 0x01);
            }

            internal static void AmmoNeverDepletes(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.NoReload;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NeverReload, Mgs2Offset.NeverReload, 2);
                        Mgs2Cheat.NoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NeverReload, 2);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NeverReload, Mgs2AoB.OriginalReloadBytes);
            }

            internal static void GripNeverDepletes(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.NoGripDamage;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.DecrementGripGauge, Mgs2Offset.NoGripDmg, 7);
                        Mgs2Cheat.NoGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoGripDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoGripDmg, Mgs2AoB.OriginalGripDamageBytes);
            }

            internal static void TurnOffPauseButton(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.DisablePauseButton;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InGamePause, Mgs2Offset.NoPauseBtn, 5);
                        Mgs2Cheat.DisablePauseButton = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoPauseBtn, 5);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoPauseBtn, Mgs2AoB.OriginalPauseButtonBytes);
            }

            internal static void TurnOffItemMenuPause(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.DisableItemMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.ItemMenuPause, Mgs2Offset.NoItemPause, 6);
                        Mgs2Cheat.DisableItemMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoItemPause, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoItemPause, Mgs2AoB.OriginalItemMenuPauseBytes);
            }

            internal static void TurnOffWeaponMenuPause(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.DisableWeaponMenuPause;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.WeaponMenuPause, Mgs2Offset.NoWeaponPause, 6);
                        Mgs2Cheat.DisableWeaponMenuPause = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.NoWeaponPause, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NoWeaponPause, Mgs2AoB.OriginalWeaponMenuPauseBytes);
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
                    Constants.PlayableCharacter currentPc = Mgs2MemoryManager.DetermineActiveCharacter();

                    lock (Mgs2Monitor.Mgs2Process)
                    {
                        using (SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process))
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
                                case Constants.PlayableCharacter.Mgs1Snake:
                                    activeCharacterAoB = Mgs2AoB.Mgs1SnakeClipping;
                                    break;
                                case Constants.PlayableCharacter.TuxedoSnake:
                                    activeCharacterAoB = Mgs2AoB.TuxedoSnakeClipping;
                                    break;
                                default:
                                    activeCharacterAoB = Mgs2AoB.VrClipping;
                                    break;
                            }

                            IntPtr pointerLocation = spp.FollowPointer(new IntPtr(Mgs2Pointer.WalkThroughWalls), false);
                            byte[] memoryContent = spp.GetMemoryFromPointer(new IntPtr(pointerLocation.ToInt64() + Mgs2Offset.NoClip.Start), Mgs2Offset.NoClip.Length);

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

                            spp.SetMemoryAtPointer(new IntPtr(pointerLocation.ToInt64() + Mgs2Offset.NoClip.Start), memoryContent);
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
                byte[] currentZoom = ReadMemory(Mgs2AoB.Camera, Mgs2Offset.Zoom);

                if (currentZoom == null)
                    return;

                Cheat activeCheat = zoomIn ? Mgs2Cheat.ZoomIn : Mgs2Cheat.ZoomOut;
                if (zoomIn)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Zoom, currentZoom[0]++);
                        Mgs2Cheat.ZoomIn = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.Zoom, currentZoom[0]++);
                    }
                }
                else
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Zoom, currentZoom[0]--);
                        Mgs2Cheat.ZoomOut = activeCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeCheat.CodeLocation, Mgs2Offset.Zoom, currentZoom[0]--);
                    }
                }
            }

            internal static void InfiniteItems(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InfiniteItems;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteItemUse, Mgs2Offset.InfiniteItems, 4);
                        Mgs2Cheat.InfiniteItems = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteItems, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InfiniteItems, Mgs2AoB.OriginalItemUseBytes);
            }

            internal static void MaxStackOnPickup(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.MaxStackOnPickup;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.MaxCountOnPickup, Mgs2Offset.MaxOnPickup, 4);
                        Mgs2Cheat.MaxStackOnPickup = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.MaxOnPickup, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.MaxOnPickup, Mgs2AoB.OriginalCountOnPickup);
            }

            internal static void InfiniteKnockout(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InfiniteKnockout;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.KnockoutDuration, Mgs2Offset.KnockoutDuration, 8);
                        Mgs2Cheat.InfiniteKnockout = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.KnockoutDuration, 8);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.KnockoutDuration, Mgs2AoB.OriginalKnockoutDuration);
                
            }

            internal static void RemovePlantFilter(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.RemovePlantFilter;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFilter, Mgs2Offset.RemovePlantFilter, 7);
                        Mgs2Cheat.RemovePlantFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.RemovePlantFilter, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.RemovePlantFilter, Mgs2AoB.OriginalRemovePlantFilterBytes);
            }

            internal static void RemovePlantFog(bool activate)
            {
                byte[] disableFog = new byte[] { 0x46 };

                Cheat activeCheat = Mgs2Cheat.RemovePlantFog;
                if (activate)
                {
                    if(activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        byte[] originalValue = ReadMemory(Mgs2AoB.RemovePlantFog, Mgs2Offset.RemovePlantFog); //this is incorrect
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFog, Mgs2Offset.RemovePlantFog, 5);
                        activeCheat.OriginalBytes = originalValue;
                        Mgs2Cheat.RemovePlantFog = activeCheat;
                    }
                    else
                    {
                        ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.RemovePlantFog, Mgs2AoB.OriginalPlantFogBytes);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.RemovePlantFog, activeCheat.OriginalBytes);
            }

            internal static void RemoveTankerEffects(bool activate)
            {
                byte[] disableFilter = new byte[] { 0x04 };
                byte[] enableFilter = new byte[] { 0x03 };

                Cheat activeCheat = Mgs2Cheat.RemoveTankerFilter;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.RemoveTankerFilter, disableFilter, Mgs2Offset.RemoveTankerFilter);
                        Mgs2Cheat.RemoveTankerFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, disableFilter, Mgs2Offset.RemoveTankerFilter);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.RemoveTankerFilter, enableFilter);
            }

            internal static void NightTime(bool activate)
            {
                byte[] nightTime = new byte[] { 0x00 };
                byte[] dayTime = new byte[] { 0xFF };

                Cheat activeCheat = Mgs2Cheat.NightTime;
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.NightTime, nightTime, Mgs2Offset.NightTime);
                        Mgs2Cheat.NightTime = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, nightTime, Mgs2Offset.NightTime);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.NightTime, dayTime);
            }

            internal static void EnableCustomFilter(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.EnableCustomFilter;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EnableCustomFiltering, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                        Mgs2Cheat.EnableCustomFilter = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static async Task ApplyColorFilter(Color chosenColor)
            {
                byte[] customColor = new byte[] { chosenColor.R, chosenColor.G, chosenColor.B };

                ReplaceWithSpecificCode(Mgs2AoB.CustomFilteringAoB, customColor, Mgs2Offset.CustomFiltering);
                
                if(!CustomFilterCancellationTokenSource.IsCancellationRequested)
                    await PeriodicTask.Run(() => ReapplyColorFilter(customColor), TimeSpan.FromMilliseconds(1000), CustomFilterCancellationTokenSource.Token);
            }

            private static void ReapplyColorFilter(byte[] chosenColor)
            {
                byte[] currentColor = ReadMemory(Mgs2AoB.CustomFilteringAoB, Mgs2Offset.CustomFiltering);

                if (!currentColor.SequenceEqual(chosenColor))
                {
                    ReplaceWithSpecificCode(Mgs2AoB.CustomFilteringAoB, chosenColor, Mgs2Offset.CustomFiltering);
                }
            }

            internal static void PauseVrTimer(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.PauseVrTimer;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.OriginalBytes = ReadMemory(Mgs2AoB.PauseVrAoB, Mgs2Offset.PauseVrTimer);
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.PauseVrAoB, Mgs2Offset.PauseVrTimer, 6, 2);
                        Mgs2Cheat.PauseVrTimer = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.PauseVrTimer, 6, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.PauseVrTimer, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void AutoCompleteVrObjectives(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrObjectiveAutoComplete;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrObjectiveAoB, Mgs2Offset.VrAutoCompleteObjectives, 6);
                        Mgs2Cheat.VrObjectiveAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrAutoCompleteObjectives, 6);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrAutoCompleteObjectives, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void AutoCompleteVrEnemies(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrEnemiesAutoComplete;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrObjectiveAoB, Mgs2Offset.VrAutoCompleteEnemies, 2);
                        Mgs2Cheat.VrEnemiesAutoComplete = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrAutoCompleteEnemies, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrAutoCompleteEnemies, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrNoHitDamage(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrNoHitDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoHitDamageAoB, Mgs2Offset.VrNoHitDmg, 4);
                        Mgs2Cheat.VrNoHitDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrNoHitDmg, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrNoHitDmg, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrNoFallDamage(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrNoFallDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoFallDamageAoB, Mgs2Offset.VrNoFallDmg, 7);
                        Mgs2Cheat.VrNoFallDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrNoFallDmg, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrNoFallDmg, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrInfiniteStrength(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrInfiniteStrength;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteStrAoB, Mgs2Offset.VrInfStr, 7);
                        Mgs2Cheat.VrInfiniteStrength = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrInfStr, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrInfStr, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrGripDamage(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrGripDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrGripDamageAoB, Mgs2Offset.VrTakeGripDmg, 7);
                        Mgs2Cheat.VrGripDamage = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrTakeGripDmg, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrTakeGripDmg, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrAimStab(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrAimStability;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.VrAimStabilityAoB, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VrAimStab);
                        Mgs2Cheat.VrAimStability = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VrAimStab);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrAimStab, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrInfiniteAmmo(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrInfiniteAmmo;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteAmmoAoB, Mgs2Offset.VrInfAmmo, 3);
                        Mgs2Cheat.VrInfiniteAmmo = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrInfAmmo, 3);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrInfAmmo, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrInfiniteItem(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrInfiniteItem;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteItemAoB, Mgs2Offset.VrInfItem, 4);
                        Mgs2Cheat.VrInfiniteItem = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrInfItem, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrInfItem, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void VrNoReload(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.VrNoReload;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoReloadAoB, Mgs2Offset.VrNoReload, 2);
                        Mgs2Cheat.VrNoReload = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.VrNoReload, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.VrNoReload, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void EmmaInfiniteHp(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.EmmaInfiniteHealth;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteHpAoB, Mgs2Offset.EmmaInfHp, 2);
                        Mgs2Cheat.EmmaInfiniteHealth = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.EmmaInfHp, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.EmmaInfHp, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void EmmaInfiniteO2(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.EmmaInfiniteO2;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteO2AoB, Mgs2Offset.EmmaInfO2, 2);
                        Mgs2Cheat.EmmaInfiniteO2 = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.EmmaInfO2, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.EmmaInfO2, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }

            internal static void InvisibleToGuards(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InvisibleToGuards;
                byte[] invisibleToGuards = new byte[] { 0xFF, 0xFF, 0x31, 0xC0, 0x48, 0x83, 0xC4, 0x20, 0x5B, 0xC3 };
                // FF FF 31 C0 48 83 C4 20 5B C3 
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToGuardsAoB, invisibleToGuards, Mgs2Offset.InvisibleToGuards);
                        Mgs2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, invisibleToGuards, Mgs2Offset.InvisibleToGuards);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InvisibleToGuards, activeCheat.OriginalBytes);
                }
            }

            internal static void InvisibleToCyphers(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InvisibleToCyphers;
                byte[] invisibleToCyphers = new byte[] { 0x48, 0x39, 0xE0, 0x0F, 0x1F, 0x40, 0x00, 0x0F, 0x85, 0x4C, 0x04 };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToCyphersAoB, invisibleToCyphers, Mgs2Offset.InvisibleToCyphers);
                        Mgs2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, invisibleToCyphers, Mgs2Offset.InvisibleToCyphers);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InvisibleToCyphers, activeCheat.OriginalBytes);
                }
            }

            internal static void InvisibleToCameras(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.InvisibleToCameras;
                byte[] invisibleToCameras = new byte[] { 0x0F, 0x1F, 0x40, 0x00, 0xE8, 0x13, 0xF8, 0x1C }; //this was from snakeswiss' original implementation, but doesnt work on 2.0.1. thankfully, disabling the first command(first 4 bytes) works instead :)
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.InvisibleToCameras, 4);
                        Mgs2Cheat.InvisibleToGuards = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.InvisibleToCameras, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.InvisibleToCameras, activeCheat.OriginalBytes);
                }
            }

            internal static void DeafenGuardsToKnocks(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.DeafenGuardsToKnocks;
                byte[] deafenedToKnocks = new byte[] { 0xA8, 0x01, 0xEB, 0x1D, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToKnocksAoB, deafenedToKnocks, Mgs2Offset.DeafenGuardsToKnocks);
                        Mgs2Cheat.DeafenGuardsToKnocks = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, deafenedToKnocks, Mgs2Offset.DeafenGuardsToKnocks);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.DeafenGuardsToKnocks, activeCheat.OriginalBytes);
                }
            }

            internal static void DeafenGuardsToGuns(bool activate)
            {
                Cheat activeCheat = Mgs2Cheat.DeafenGuardsToGuns;
                byte[] deafenedToGuns = new byte[] { 0xA9, 0x00, 0x18, 0x00, 0x00, 0xEB, 0x12, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToGunsAoB, deafenedToGuns, Mgs2Offset.DeafenGuardsToGuns);
                        Mgs2Cheat.DeafenGuardsToGuns = activeCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeCheat.CodeLocation, deafenedToGuns, Mgs2Offset.DeafenGuardsToGuns);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.DeafenGuardsToGuns, activeCheat.OriginalBytes);
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
                Cheat activeCheat = Mgs2Cheat.TurnOffMusic;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.TurnOffMusicAoB, Mgs2Offset.TurnOffMusic, 7);
                        Mgs2Cheat.TurnOffMusic = activeCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeCheat.CodeLocation, Mgs2Offset.TurnOffMusic, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeCheat.CodeLocation, Mgs2Offset.TurnOffMusic, activeCheat.OriginalBytes);
                    CustomFilterCancellationTokenSource.Cancel();
                }
            }
        }
    }    

    public class Mgs2Cheat
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
        public static Cheat PauseVrTimer { get; internal set; } = new Cheat("Pause VR Timer", Cheat.CheatActions.PauseVrTimer, Mgs2AoB.OriginalPauseVrBytes);
        public static Cheat VrObjectiveAutoComplete { get; internal set; } = new Cheat("Auto Complete VR objectives", Cheat.CheatActions.AutoCompleteVrObjectives, Mgs2AoB.OriginalVrObjectiveBytes);
        public static Cheat VrEnemiesAutoComplete { get; internal set; } = new Cheat("Auto 'Kill' VR Enemies", Cheat.CheatActions.AutoCompleteVrEnemies, Mgs2AoB.OriginalVrEnemiesBytes);
        public static Cheat VrNoHitDamage { get; internal set; } = new Cheat("Take No Damage in VR", Cheat.CheatActions.VrNoHitDamage, Mgs2AoB.OriginalVrNoHitDamageBytes);
        public static Cheat VrNoFallDamage { get; internal set; } = new Cheat("Take No Fall Damage in VR", Cheat.CheatActions.VrNoFallDamage, Mgs2AoB.OriginalVrNoFallDamageBytes);
        public static Cheat VrInfiniteStrength { get; internal set; } = new Cheat("Infinite VR Strength", Cheat.CheatActions.VrInfiniteStrength, Mgs2AoB.OriginalVrInfiniteStrBytes);
        public static Cheat VrGripDamage { get; internal set; } = new Cheat("VR Grip Damage", Cheat.CheatActions.VrGripDamage, Mgs2AoB.OriginalVrGripDamageBytes);
        public static Cheat VrAimStability { get; internal set; } = new Cheat("VR Aim Stability", Cheat.CheatActions.VrAimStab, Mgs2AoB.OriginalVrAimStabilityBytes);
        public static Cheat VrInfiniteAmmo { get; internal set; } = new Cheat("VR Infinite Ammo", Cheat.CheatActions.VrInfiniteAmmo, Mgs2AoB.OriginalVrInfiniteAmmoBytes);
        public static Cheat VrInfiniteItem { get; internal set; } = new Cheat("VR Infinite Items", Cheat.CheatActions.VrInfiniteItem, Mgs2AoB.OriginalVrInfiniteItemBytes);
        public static Cheat VrNoReload { get; internal set; } = new Cheat("VR No Reload", Cheat.CheatActions.VrNoReload, Mgs2AoB.OriginalVrNoReloadBytes);
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
                RemovePlantFog, RemoveTankerFilter, NightTime, MaxStackOnPickup, PauseVrTimer, VrObjectiveAutoComplete,
                /*VREnemiesAutoComplete,*/ VrNoHitDamage, VrNoFallDamage, VrInfiniteStrength, VrGripDamage, VrAimStability, //VR Enemies autocomplete is crashing the game
                VrInfiniteAmmo, VrInfiniteItem, VrNoReload, BlackScreen, Letterboxing, GhostMode, TurnOffMusic
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
