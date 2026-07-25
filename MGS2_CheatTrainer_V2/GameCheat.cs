using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2
{
    //REWRITE STATUS: Seems error free, but this class does desperately need attention outside making multiplatform.
    public struct GameCheat
    {
        public Action<bool> CheatAction { get; private set; }
        private byte[]? OriginalBytes { get; set; }
        private IntPtr CodeLocation { get; set; }
        public Constants.Cheat? CheatType { get; set; }
        private static CancellationTokenSource? CustomFilterCancellationTokenSource { get; set; }
        private static Color CustomFilterColor { get; set; }

        public GameCheat(Action<bool> action, byte[]? originalBytes, Constants.Cheat? cheatType)
        {
            CheatAction = action;
            OriginalBytes = originalBytes;
            CodeLocation = IntPtr.Zero;
            CheatType = cheatType;
        }

        internal static class CheatActions
        {
            private static void ReplaceWithOriginalCode(IntPtr memoryLocation, MemoryOffset offset, byte[] bytesToReplace, int startIndexToReplace = 0)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
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
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static IntPtr ReplaceWithInvalidCode(string aob, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            SimplePattern pattern = new SimplePattern(aob);
                            int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).OffsetAddress.ToInt32();

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
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }

                return IntPtr.Zero;
            }

            private static void ReplaceWithInvalidCode(IntPtr memoryLocation, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            if (memoryLocation != IntPtr.Zero)
                            {
                                byte[] memoryContent = spp.ReadProcessOffset(IntPtr.Add(memoryLocation, offset.Start), offset.Length);

                                for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace; i++)
                                {
                                    memoryContent[i] = 0x90;
                                }

                                spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                successful = true;
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
            }

            public static IntPtr ReplaceWithSpecificCode(string patternToScan, byte[] replacementBytes, MemoryOffset offset)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            SimplePattern pattern = new SimplePattern(patternToScan);
                            int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).OffsetAddress.ToInt32();

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
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
                throw new Exception("Failed to replace code, aborting the process");
            }

            private static void ReplaceWithSpecificCode(IntPtr memoryLocation, byte[] replacementBytes, MemoryOffset offset)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            if (memoryLocation != IntPtr.Zero)
                            {
                                byte[] memoryContent = spp.ReadProcessOffset(IntPtr.Add(memoryLocation, offset.Start), offset.Length);

                                for (int i = 0; i < replacementBytes.Length; i++)
                                {
                                    memoryContent[i] = replacementBytes[i];
                                }

                                spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                successful = true;
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static IntPtr ModifySingleByte(string aob, MemoryOffset offset, byte replacementValue)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            SimplePattern pattern = new SimplePattern(aob);
                            int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).OffsetAddress.ToInt32();

                            if (memoryLocation != -1)
                            {
                                spp.ModifyProcessOffset(new IntPtr(memoryLocation + offset.Start), replacementValue, true);
                                successful = true;

                                return new IntPtr(memoryLocation);
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }

                return IntPtr.Zero;
            }

            private static void ModifySingleByte(IntPtr memoryLocation, MemoryOffset offset, byte replacementValue)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            if (memoryLocation != IntPtr.Zero)
                            {
                                spp.ModifyProcessOffset(IntPtr.Add(memoryLocation, offset.Start), replacementValue, true);
                                successful = true;
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
            }

            public static byte[] ReadMemory(string aob, MemoryOffset offset)
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            SimplePattern pattern = new SimplePattern(aob);
                            int memoryLocation = spp.ScanMemoryForUniquePattern(pattern).OffsetAddress.ToInt32();

                            if(memoryLocation != -1)
                                return spp.ReadProcessOffset(new IntPtr(memoryLocation + offset.Start), offset.Length);
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);

                    throw new Exception("Failed to read process memory, aborting cheat process");
                }
            }

            public static void RestartLevel()
            {
                if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                lock (Mgs2Monitor.Mgs2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
                            spp.ModifyProcessOffset(new IntPtr(0x153F048), 1);
                            successful = true;
                        }
                        catch (Exception e)
                        {
                            retries--;
                            if (retries == 0)
                            {
                                throw new AggregateException("Failed to activate cheat, abandoning process", e);
                            }
                        }
                    } while (!successful && retries > 0);
                }
            }

            public static void TurnScreenBlack(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.BlackScreen;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.OriginalBytes = ReadMemory(Mgs2AoB.Camera, Mgs2Offset.BlackScreen);
                        activeGameCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.BlackScreen, 0x00);
                        Mgs2Cheat.BlackScreen = activeGameCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.BlackScreen, 0x00);
                    }
                }
                else
                    ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.BlackScreen, 0x40);
            }

            public static void TurnOffBleedDamage(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.NoBleedDamage;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBleedDamage, Mgs2Offset.NoBleedDmg, 7);
                        Mgs2Cheat.NoBleedDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoBleedDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoBleedDmg, Mgs2AoB.OriginalBleedDamageBytes);
            }

            public static void TurnOffBurnDamage(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.NoBurnDamage;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NoBurnDamage, Mgs2Offset.NoBurnDmg, 7);
                        Mgs2Cheat.NoBurnDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoBurnDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoBleedDmg, Mgs2AoB.OriginalBurnDamageBytes);
            }

            internal static void InfiniteAmmo(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InfiniteAmmo;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteAmmo, Mgs2Offset.InfiniteAmmo, 4);
                        Mgs2Cheat.InfiniteAmmo = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteAmmo, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteAmmo, Mgs2AoB.OriginalAmmoBytes);
            }

            internal static void InfiniteLife(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InfiniteLife;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteLife, Mgs2Offset.InfiniteLife, 4);
                        Mgs2Cheat.InfiniteLife = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteLife, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteLife, Mgs2AoB.OriginalLifeBytes);
            }

            internal static void InfiniteOxygen(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InfiniteOxygen;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteO2, Mgs2Offset.InfiniteO2, 4);
                        Mgs2Cheat.InfiniteOxygen = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteO2, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteO2, Mgs2AoB.OriginalO2Bytes);
            }

            internal static void Letterboxing(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.Letterboxing;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Letterbox, 0x00);
                        Mgs2Cheat.Letterboxing = activeGameCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.Letterbox, 0x01);
                    }
                }
                else
                    ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.Letterbox, 0x01);
            }

            internal static void AmmoNeverDepletes(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.NoReload;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.NeverReload, Mgs2Offset.NeverReload, 2);
                        Mgs2Cheat.NoReload = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NeverReload, 2);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NeverReload, Mgs2AoB.OriginalReloadBytes);
            }

            internal static void GripNeverDepletes(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.NoGripDamage;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.DecrementGripGauge, Mgs2Offset.NoGripDmg, 7);
                        Mgs2Cheat.NoGripDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoGripDmg, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoGripDmg, Mgs2AoB.OriginalGripDamageBytes);
            }

            internal static void TurnOffPauseButton(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.DisablePauseButton;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InGamePause, Mgs2Offset.NoPauseBtn, 5);
                        Mgs2Cheat.DisablePauseButton = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoPauseBtn, 5);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoPauseBtn, Mgs2AoB.OriginalPauseButtonBytes);
            }

            internal static void TurnOffItemMenuPause(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.DisableItemMenuPause;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.ItemMenuPause, Mgs2Offset.NoItemPause, 6);
                        Mgs2Cheat.DisableItemMenuPause = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoItemPause, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoItemPause, Mgs2AoB.OriginalItemMenuPauseBytes);
            }

            internal static void TurnOffWeaponMenuPause(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.DisableWeaponMenuPause;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.WeaponMenuPause, Mgs2Offset.NoWeaponPause, 6);
                        Mgs2Cheat.DisableWeaponMenuPause = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.NoWeaponPause, 6);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NoWeaponPause, Mgs2AoB.OriginalWeaponMenuPauseBytes);
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
                    Mgs2MemoryManager memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
                    Constants.PlayableCharacter currentPc = memoryManager.DetermineActiveCharacter();
                    if (Mgs2Monitor.Mgs2Process is null) throw new Exception("Not hooked into game");
                    lock (Mgs2Monitor.Mgs2Process)
                    {
                        using SimpleProcessProxy spp = new SimpleProcessProxy(Mgs2Monitor.Mgs2Process);
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

                GameCheat activeGameCheat = zoomIn ? Mgs2Cheat.ZoomIn : Mgs2Cheat.ZoomOut;
                if (zoomIn)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Zoom, currentZoom[0]++);
                        Mgs2Cheat.ZoomIn = activeGameCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.Zoom, currentZoom[0]++);
                    }
                }
                else
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ModifySingleByte(Mgs2AoB.Camera, Mgs2Offset.Zoom, currentZoom[0]--);
                        Mgs2Cheat.ZoomOut = activeGameCheat;
                    }
                    else
                    {
                        ModifySingleByte(activeGameCheat.CodeLocation, Mgs2Offset.Zoom, currentZoom[0]--);
                    }
                }
            }

            internal static void InfiniteItems(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InfiniteItems;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InfiniteItemUse, Mgs2Offset.InfiniteItems, 4);
                        Mgs2Cheat.InfiniteItems = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteItems, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InfiniteItems, Mgs2AoB.OriginalItemUseBytes);
            }

            internal static void MaxStackOnPickup(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.MaxStackOnPickup;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.MaxCountOnPickup, Mgs2Offset.MaxOnPickup, 4);
                        Mgs2Cheat.MaxStackOnPickup = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.MaxOnPickup, 4);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.MaxOnPickup, Mgs2AoB.OriginalCountOnPickup);
            }

            internal static void InfiniteKnockout(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InfiniteKnockout;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.KnockoutDuration, Mgs2Offset.KnockoutDuration, 8);
                        Mgs2Cheat.InfiniteKnockout = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.KnockoutDuration, 8);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.KnockoutDuration, Mgs2AoB.OriginalKnockoutDuration);
                
            }

            internal static void RemovePlantFilter(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.RemovePlantFilter;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFilter, Mgs2Offset.RemovePlantFilter, 7);
                        Mgs2Cheat.RemovePlantFilter = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.RemovePlantFilter, 7);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.RemovePlantFilter, Mgs2AoB.OriginalRemovePlantFilterBytes);
            }

            internal static void RemovePlantFog(bool activate)
            {
                byte[] disableFog = new byte[] { 0x46 };

                GameCheat activeGameCheat = Mgs2Cheat.RemovePlantFog;
                if (activate)
                {
                    if(activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        byte[] originalValue = ReadMemory(Mgs2AoB.RemovePlantFog, Mgs2Offset.RemovePlantFog); //this is incorrect
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.RemovePlantFog, Mgs2Offset.RemovePlantFog, 5);
                        activeGameCheat.OriginalBytes = originalValue;
                        Mgs2Cheat.RemovePlantFog = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.RemovePlantFog, Mgs2AoB.OriginalPlantFogBytes);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.RemovePlantFog, activeGameCheat.OriginalBytes ?? throw new InvalidOperationException());
            }

            internal static void RemoveTankerEffects(bool activate)
            {
                byte[] disableFilter = new byte[] { 0x04 };
                byte[] enableFilter = new byte[] { 0x03 };

                GameCheat activeGameCheat = Mgs2Cheat.RemoveTankerFilter;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.RemoveTankerFilter, disableFilter, Mgs2Offset.RemoveTankerFilter);
                        Mgs2Cheat.RemoveTankerFilter = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, disableFilter, Mgs2Offset.RemoveTankerFilter);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.RemoveTankerFilter, enableFilter);
            }

            internal static void NightTime(bool activate)
            {
                byte[] nightTime = new byte[] { 0x00 };
                byte[] dayTime = new byte[] { 0xFF };

                GameCheat activeGameCheat = Mgs2Cheat.NightTime;
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.NightTime, nightTime, Mgs2Offset.NightTime);
                        Mgs2Cheat.NightTime = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, nightTime, Mgs2Offset.NightTime);
                    }
                }
                else
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.NightTime, dayTime);
            }

            internal static void EnableCustomFilter(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.EnableCustomFilter;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EnableCustomFiltering, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                        Mgs2Cheat.EnableCustomFilter = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes.Length - 1);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes);
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static async Task ApplyColorFilter(Color chosenColor)
            {
                byte[] customColor = new byte[] { chosenColor.R, chosenColor.G, chosenColor.B };

                ReplaceWithSpecificCode(Mgs2AoB.CustomFilteringAoB, customColor, Mgs2Offset.CustomFiltering);
                
                if(CustomFilterCancellationTokenSource != null && !CustomFilterCancellationTokenSource.IsCancellationRequested)
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
                GameCheat activeGameCheat = Mgs2Cheat.PauseVrTimer;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.OriginalBytes = ReadMemory(Mgs2AoB.PauseVrAoB, Mgs2Offset.PauseVrTimer);
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.PauseVrAoB, Mgs2Offset.PauseVrTimer, 6, 2);
                        Mgs2Cheat.PauseVrTimer = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.PauseVrTimer, 6, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.PauseVrTimer, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void AutoCompleteVrObjectives(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrObjectiveAutoComplete;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrObjectiveAoB, Mgs2Offset.VrAutoCompleteObjectives, 6);
                        Mgs2Cheat.VrObjectiveAutoComplete = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrAutoCompleteObjectives, 6);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrAutoCompleteObjectives, activeGameCheat.OriginalBytes ?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void AutoCompleteVrEnemies(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrEnemiesAutoComplete;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrObjectiveAoB, Mgs2Offset.VrAutoCompleteEnemies, 2);
                        Mgs2Cheat.VrEnemiesAutoComplete = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrAutoCompleteEnemies, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrAutoCompleteEnemies, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrNoHitDamage(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrNoHitDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoHitDamageAoB, Mgs2Offset.VrNoHitDmg, 4);
                        Mgs2Cheat.VrNoHitDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoHitDmg, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoHitDmg, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrNoFallDamage(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrNoFallDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoFallDamageAoB, Mgs2Offset.VrNoFallDmg, 7);
                        Mgs2Cheat.VrNoFallDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoFallDmg, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoFallDmg, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrInfiniteStrength(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrInfiniteStrength;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteStrAoB, Mgs2Offset.VrInfStr, 7);
                        Mgs2Cheat.VrInfiniteStrength = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfStr, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfStr, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrGripDamage(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrGripDamage;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrGripDamageAoB, Mgs2Offset.VrTakeGripDmg, 7);
                        Mgs2Cheat.VrGripDamage = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrTakeGripDmg, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrTakeGripDmg, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrAimStab(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrAimStability;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.VrAimStabilityAoB, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VrAimStab);
                        Mgs2Cheat.VrAimStability = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, new byte[] { 0xE9, 0x91, 0x01, 0x00, 0x00, 0x90 }, Mgs2Offset.VrAimStab);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrAimStab, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrInfiniteAmmo(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrInfiniteAmmo;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteAmmoAoB, Mgs2Offset.VrInfAmmo, 3);
                        Mgs2Cheat.VrInfiniteAmmo = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfAmmo, 3);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfAmmo, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrInfiniteItem(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrInfiniteItem;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrInfiniteItemAoB, Mgs2Offset.VrInfItem, 4);
                        Mgs2Cheat.VrInfiniteItem = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfItem, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrInfItem, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void VrNoReload(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.VrNoReload;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.VrNoReloadAoB, Mgs2Offset.VrNoReload, 2);
                        Mgs2Cheat.VrNoReload = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoReload, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.VrNoReload, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void EmmaInfiniteHp(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.EmmaInfiniteHealth;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteHpAoB, Mgs2Offset.EmmaInfHp, 2);
                        Mgs2Cheat.EmmaInfiniteHealth = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.EmmaInfHp, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.EmmaInfHp, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void EmmaInfiniteO2(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.EmmaInfiniteO2;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.EmmaInfiniteO2AoB, Mgs2Offset.EmmaInfO2, 2);
                        Mgs2Cheat.EmmaInfiniteO2 = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.EmmaInfO2, 2);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.EmmaInfO2, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }

            internal static void InvisibleToGuards(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InvisibleToGuards;
                byte[] invisibleToGuards = new byte[] { 0xFF, 0xFF, 0x31, 0xC0, 0x48, 0x83, 0xC4, 0x20, 0x5B, 0xC3 };
                // FF FF 31 C0 48 83 C4 20 5B C3 
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToGuardsAoB, invisibleToGuards, Mgs2Offset.InvisibleToGuards);
                        Mgs2Cheat.InvisibleToGuards = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, invisibleToGuards, Mgs2Offset.InvisibleToGuards);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InvisibleToGuards, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                }
            }

            internal static void InvisibleToCyphers(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InvisibleToCyphers;
                byte[] invisibleToCyphers = new byte[] { 0x48, 0x39, 0xE0, 0x0F, 0x1F, 0x40, 0x00, 0x0F, 0x85, 0x4C, 0x04 };
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.InvisibleToCyphersAoB, invisibleToCyphers, Mgs2Offset.InvisibleToCyphers);
                        Mgs2Cheat.InvisibleToGuards = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, invisibleToCyphers, Mgs2Offset.InvisibleToCyphers);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InvisibleToCyphers, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                }
            }

            internal static void InvisibleToCameras(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.InvisibleToCameras;
                byte[] invisibleToCameras = new byte[] { 0x0F, 0x1F, 0x40, 0x00, 0xE8, 0x13, 0xF8, 0x1C }; //this was from snakeswiss' original implementation, but doesnt work on 2.0.1. thankfully, disabling the first command(first 4 bytes) works instead :)
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.InvisibleToCameras, 4);
                        Mgs2Cheat.InvisibleToGuards = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(Mgs2AoB.InvisibleToCamerasAoB, Mgs2Offset.InvisibleToCameras, 4);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.InvisibleToCameras, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                }
            }

            internal static void DeafenGuardsToKnocks(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.DeafenGuardsToKnocks;
                byte[] deafenedToKnocks = new byte[] { 0xA8, 0x01, 0xEB, 0x1D, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToKnocksAoB, deafenedToKnocks, Mgs2Offset.DeafenGuardsToKnocks);
                        Mgs2Cheat.DeafenGuardsToKnocks = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, deafenedToKnocks, Mgs2Offset.DeafenGuardsToKnocks);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.DeafenGuardsToKnocks, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                }
            }

            internal static void DeafenGuardsToGuns(bool activate)
            {
                GameCheat activeGameCheat = Mgs2Cheat.DeafenGuardsToGuns;
                byte[] deafenedToGuns = new byte[] { 0xA9, 0x00, 0x18, 0x00, 0x00, 0xEB, 0x12, 0x48, 0x8B, 0xCB };
                if (activate)
                {
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithSpecificCode(Mgs2AoB.DeafenGuardsToGunsAoB, deafenedToGuns, Mgs2Offset.DeafenGuardsToGuns);
                        Mgs2Cheat.DeafenGuardsToGuns = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithSpecificCode(activeGameCheat.CodeLocation, deafenedToGuns, Mgs2Offset.DeafenGuardsToGuns);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.DeafenGuardsToGuns, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
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
                GameCheat activeGameCheat = Mgs2Cheat.TurnOffMusic;
                if (activate)
                {
                    CustomFilterCancellationTokenSource = new CancellationTokenSource();
                    if (activeGameCheat.CodeLocation == IntPtr.Zero)
                    {
                        activeGameCheat.CodeLocation = ReplaceWithInvalidCode(Mgs2AoB.TurnOffMusicAoB, Mgs2Offset.TurnOffMusic, 7);
                        Mgs2Cheat.TurnOffMusic = activeGameCheat;
                    }
                    else
                    {
                        ReplaceWithInvalidCode(activeGameCheat.CodeLocation, Mgs2Offset.TurnOffMusic, 7);
                    }
                }
                else
                {
                    ReplaceWithOriginalCode(activeGameCheat.CodeLocation, Mgs2Offset.TurnOffMusic, activeGameCheat.OriginalBytes?? throw new InvalidOperationException());
                    CustomFilterCancellationTokenSource?.Cancel();
                }
            }
        }
    }    

    public static class Mgs2Cheat
    {
        public static GameCheat BlackScreen { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnScreenBlack, Mgs2AoB.OriginalCameraBytes, Constants.Cheat.BlackScreen);
        public static GameCheat NoBleedDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffBleedDamage, Mgs2AoB.OriginalBleedDamageBytes, Constants.Cheat.NoBleedDamage);
        public static GameCheat NoBurnDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffBurnDamage, Mgs2AoB.OriginalBurnDamageBytes, Constants.Cheat.NoBurnDamage);
        public static GameCheat InfiniteAmmo { get; internal set; } = new GameCheat(GameCheat.CheatActions.InfiniteAmmo, Mgs2AoB.OriginalAmmoBytes, Constants.Cheat.InfiniteAmmo);
        public static GameCheat InfiniteLife { get; internal set; } = new GameCheat(GameCheat.CheatActions.InfiniteLife, Mgs2AoB.OriginalLifeBytes, Constants.Cheat.InfiniteLife);
        public static GameCheat InfiniteOxygen { get; internal set; } = new GameCheat(GameCheat.CheatActions.InfiniteOxygen, Mgs2AoB.OriginalO2Bytes, Constants.Cheat.InfiniteOxygen);
        public static GameCheat Letterboxing { get; internal set; } = new GameCheat(GameCheat.CheatActions.Letterboxing, Mgs2AoB.OriginalCameraBytes, Constants.Cheat.Letterboxing);
        public static GameCheat NoReload { get; internal set; } = new GameCheat(GameCheat.CheatActions.AmmoNeverDepletes, Mgs2AoB.OriginalReloadBytes, Constants.Cheat.NoReload);
        public static GameCheat NoClipWithGravity { get; internal set; } = new GameCheat(GameCheat.CheatActions.NoClipWithGravity, Mgs2AoB.OriginalClippingBytes, Constants.Cheat.NoClipWithGravity);
        public static GameCheat NoClipNoGravity { get; internal set; } = new GameCheat(GameCheat.CheatActions.NoClipNoGravity, Mgs2AoB.OriginalClippingBytes, Constants.Cheat.NoClipNoGravity);
        public static GameCheat ZoomIn { get; internal set; } = new GameCheat(GameCheat.CheatActions.ZoomIn, Mgs2AoB.OriginalCameraBytes, Constants.Cheat.ZoomIn);
        public static GameCheat ZoomOut { get; internal set; } = new GameCheat(GameCheat.CheatActions.ZoomOut, Mgs2AoB.OriginalCameraBytes, Constants.Cheat.ZoomOut);
        public static GameCheat NoGripDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.GripNeverDepletes, Mgs2AoB.OriginalGripDamageBytes, Constants.Cheat.NoGripDamage);
        public static GameCheat DisablePauseButton { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffPauseButton, Mgs2AoB.OriginalPauseButtonBytes, Constants.Cheat.DisablePauseButton);
        public static GameCheat DisableItemMenuPause { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffItemMenuPause, Mgs2AoB.OriginalItemMenuPauseBytes, Constants.Cheat.DisableItemMenuPause);
        public static GameCheat DisableWeaponMenuPause { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffWeaponMenuPause, Mgs2AoB.OriginalWeaponMenuPauseBytes, Constants.Cheat.DisableWeaponMenuPause);
        public static GameCheat InfiniteItems { get; internal set; } = new GameCheat(GameCheat.CheatActions.InfiniteItems, Mgs2AoB.OriginalItemUseBytes, Constants.Cheat.InfiniteItems);
        public static GameCheat MaxStackOnPickup { get; internal set; } = new GameCheat(GameCheat.CheatActions.MaxStackOnPickup, Mgs2AoB.OriginalCountOnPickup, Constants.Cheat.MaxStackOnPickup);
        public static GameCheat InfiniteKnockout { get; internal set; } = new GameCheat(GameCheat.CheatActions.InfiniteKnockout, Mgs2AoB.OriginalKnockoutDuration, Constants.Cheat.InfiniteKnockout);
        public static GameCheat RemovePlantFilter { get; internal set; } = new GameCheat(GameCheat.CheatActions.RemovePlantFilter, Mgs2AoB.OriginalRemovePlantFilterBytes, Constants.Cheat.RemovePlantFilter);
        public static GameCheat RemovePlantFog { get; internal set; } = new GameCheat(GameCheat.CheatActions.RemovePlantFog, Mgs2AoB.OriginalPlantFogBytes, Constants.Cheat.RemovePlantFog);
        public static GameCheat RemoveTankerFilter { get; internal set; } = new GameCheat(GameCheat.CheatActions.RemoveTankerEffects, Mgs2AoB.OriginalRemoveTankerFilterBytes, Constants.Cheat.RemoveTankerFilter);
        public static GameCheat NightTime { get; internal set; } = new GameCheat(GameCheat.CheatActions.NightTime, Mgs2AoB.OriginalNightTimeBytes, Constants.Cheat.NightTime);
        public static GameCheat EnableCustomFilter { get; internal set; } = new GameCheat(GameCheat.CheatActions.EnableCustomFilter, Mgs2AoB.OriginalCustomFilteringBytes, null);
        public static GameCheat PauseVrTimer { get; internal set; } = new GameCheat(GameCheat.CheatActions.PauseVrTimer, Mgs2AoB.OriginalPauseVrBytes, Constants.Cheat.PauseVrTimer);
        public static GameCheat VrObjectiveAutoComplete { get; internal set; } = new GameCheat(GameCheat.CheatActions.AutoCompleteVrObjectives, Mgs2AoB.OriginalVrObjectiveBytes, Constants.Cheat.VrObjectiveAutoComplete);
        public static GameCheat VrEnemiesAutoComplete { get; internal set; } = new GameCheat(GameCheat.CheatActions.AutoCompleteVrEnemies, Mgs2AoB.OriginalVrEnemiesBytes, Constants.Cheat.VrEnemiesAutoComplete);
        public static GameCheat VrNoHitDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrNoHitDamage, Mgs2AoB.OriginalVrNoHitDamageBytes, Constants.Cheat.VrNoHitDamage);
        public static GameCheat VrNoFallDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrNoFallDamage, Mgs2AoB.OriginalVrNoFallDamageBytes, Constants.Cheat.VrNoFallDamage);
        public static GameCheat VrInfiniteStrength { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrInfiniteStrength, Mgs2AoB.OriginalVrInfiniteStrBytes, Constants.Cheat.VrInfiniteStrength);
        public static GameCheat VrGripDamage { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrGripDamage, Mgs2AoB.OriginalVrGripDamageBytes, Constants.Cheat.VrGripDamage);
        public static GameCheat VrAimStability { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrAimStab, Mgs2AoB.OriginalVrAimStabilityBytes, Constants.Cheat.VrAimStability);
        public static GameCheat VrInfiniteAmmo { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrInfiniteAmmo, Mgs2AoB.OriginalVrInfiniteAmmoBytes, Constants.Cheat.VrInfiniteAmmo);
        public static GameCheat VrInfiniteItem { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrInfiniteItem, Mgs2AoB.OriginalVrInfiniteItemBytes, Constants.Cheat.VrInfiniteItem);
        public static GameCheat VrNoReload { get; internal set; } = new GameCheat(GameCheat.CheatActions.VrNoReload, Mgs2AoB.OriginalVrNoReloadBytes, Constants.Cheat.VrNoReload);
        public static GameCheat EmmaInfiniteHealth { get; internal set; } = new GameCheat(GameCheat.CheatActions.EmmaInfiniteHp, Mgs2AoB.OriginalEmmaHpBytes, Constants.Cheat.EmmaInfiniteHealth);
        public static GameCheat EmmaInfiniteO2 { get; internal set; } = new GameCheat(GameCheat.CheatActions.EmmaInfiniteO2, Mgs2AoB.OriginalEmmaO2Bytes, Constants.Cheat.EmmaInfiniteO2);
        public static GameCheat InvisibleToGuards { get; internal set; } = new GameCheat(GameCheat.CheatActions.InvisibleToGuards, Mgs2AoB.OriginalInvisibleToGuardsBytes, null);
        public static GameCheat InvisibleToCyphers { get; internal set; } = new GameCheat(GameCheat.CheatActions.InvisibleToCyphers, Mgs2AoB.OriginalInvisibleToCyphersBytes, null);
        public static GameCheat InvisibleToCameras { get; internal set; } = new GameCheat(GameCheat.CheatActions.InvisibleToCameras, Mgs2AoB.OriginalInvisibleToCamerasBytes, null);
        public static GameCheat DeafenGuardsToKnocks { get; internal set; } = new GameCheat(GameCheat.CheatActions.DeafenGuardsToKnocks, Mgs2AoB.OriginalDeafenGuardsToKnocksBytes, null);
        public static GameCheat DeafenGuardsToGuns { get; internal set; } = new GameCheat(GameCheat.CheatActions.DeafenGuardsToGuns, Mgs2AoB.OriginalDeafenGuardsToGunsBytes, null);
        public static GameCheat GhostMode { get; internal set; } = new GameCheat(GameCheat.CheatActions.GhostMode, null, Constants.Cheat.GhostMode);
        public static GameCheat TurnOffMusic { get; internal set; } = new GameCheat(GameCheat.CheatActions.TurnOffMusic, Mgs2AoB.OriginalTurnOffMusicBytes, Constants.Cheat.TurnOffMusic);

        public static List<GameCheat> CheatList = new List<GameCheat>
        {
            NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage, 
            EmmaInfiniteHealth, EmmaInfiniteO2, NoClipWithGravity, NoClipNoGravity,  //Emma health is crashing the game and i cba to fix it
            NoReload, ZoomIn, ZoomOut, DisablePauseButton, //zoom in and out aren't working as expected, and i cant be bothered to fix them right now.
            DisableItemMenuPause, DisableWeaponMenuPause, InfiniteItems, InfiniteKnockout, RemovePlantFilter,
            RemovePlantFog, RemoveTankerFilter, NightTime, MaxStackOnPickup, PauseVrTimer, VrObjectiveAutoComplete,
            VrEnemiesAutoComplete, VrNoHitDamage, VrNoFallDamage, VrInfiniteStrength, VrGripDamage, VrAimStability, //VR Enemies autocomplete is crashing the game
            VrInfiniteAmmo, VrInfiniteItem, VrNoReload, BlackScreen, Letterboxing, GhostMode, TurnOffMusic
        };
    }
    
}
