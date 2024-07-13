using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;

namespace MGS2_MC
{
    public struct Cheat
    {
        public string Name { get; private set; }
        public Action<bool> CheatAction { get; private set; }
        public byte[] OriginalBytes { get; private set; }
        public IntPtr CodeLocation { get; set; }

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

            internal static void ReplaceWithSpecificCode(string patternToScan, byte[] replacementBytes, MemoryOffset offset)
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

                                    spp.ModifyProcessOffset(new IntPtr(memoryLocation), memoryContent, true);
                                    successful = true;

                                    return;
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
                NoClip(false, activate);
            }

            internal static void NoClipWithGravity(bool activate)
            {
                NoClip(true, activate);
            }

            private static void NoClip(bool gravity, bool activate)
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

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                BlackScreen, NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage,
                Letterboxing, NoClipWithGravity, NoClipNoGravity, NoReload,/*ZoomIn, ZoomOut,*/ DisablePauseButton,
                DisableItemMenuPause, DisableWeaponMenuPause //zoom in and out aren't working as expected, and i cant be bothered to fix them right now.
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
