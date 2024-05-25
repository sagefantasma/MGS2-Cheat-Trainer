using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;

namespace MGS2_MC
{
    public struct Cheat
    {
        public string Name { get; private set; }
        public Action<bool> CheatAction;
        public byte[] OriginalBytes { get; set; }

        public Cheat(string name, Action<bool> action, byte[] originalBytes)
        {
            Name = name;
            CheatAction = action;
            OriginalBytes = originalBytes;
        }

        internal class CheatActions
        {
            private static void ReplaceWithOriginalCode(string aob, MemoryOffset offset, byte[] bytesToReplace, int startIndexToReplace = 0)
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

                                    for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace.Length; i++)
                                    {
                                        memoryContent[i] = bytesToReplace[i];
                                    }

                                    spp.ModifyProcessOffset(new IntPtr(memoryLocation), memoryContent, true);
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

            private static void ReplaceWithInvalidCode(string aob, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
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

            private static void ModifySingleByte(string aob, MemoryOffset offset, byte replacementValue)
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

            private static byte[] ReadMemory(string aob, MemoryOffset offset)
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
                if (activate)
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN, 0x00);
                else
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN, 0x01);
            }

            public static void TurnOffBleedDamage(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.NoBleedDamage, MGS2Offset.NO_BLEED_DMG, 7);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreBleedDamage, MGS2Offset.NO_BLEED_DMG, MGS2AoB.OriginalBleedDamageBytes);
            }

            public static void TurnOffBurnDamage(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.NoBurnDamage, MGS2Offset.NO_BURN_DMG, 7);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreBurnDamage, MGS2Offset.NO_BLEED_DMG, MGS2AoB.OriginalBurnDamageBytes);
            }

            internal static void InfiniteAmmo(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.InfiniteAmmo, MGS2Offset.INFINITE_AMMO, 4);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreAmmo, MGS2Offset.INFINITE_AMMO, MGS2AoB.OriginalAmmoBytes);
            }

            internal static void InfiniteLife(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.InfiniteLife, MGS2Offset.INFINITE_LIFE, 4);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreLife, MGS2Offset.INFINITE_LIFE, MGS2AoB.OriginalLifeBytes);
            }

            internal static void InfiniteOxygen(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.InfiniteO2, MGS2Offset.INFINITE_O2, 4);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreO2, MGS2Offset.INFINITE_O2, MGS2AoB.OriginalO2Bytes);
            }

            internal static void Letterboxing(bool activate)
            {
                if (activate)
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.LETTERBOX, 0x00);
                else
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.LETTERBOX, 0x01);
            }

            internal static void AmmoNeverDepletes(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, 2);
                else
                    ReplaceWithOriginalCode(MGS2AoB.RestoreReload, MGS2Offset.NEVER_RELOAD, MGS2AoB.OriginalReloadBytes);
            }

            internal static void GripNeverDepletes(bool activate)
            {
                if (activate)
                    ReplaceWithInvalidCode(MGS2AoB.DecrementGripGauge, MGS2Offset.NO_GRIP_DMG, 7);
                else
                    ReplaceWithOriginalCode(MGS2AoB.IncrementGripGauge, MGS2Offset.NO_GRIP_DMG, MGS2AoB.OriginalGripDamageBytes);
            }

            internal static void NoClipNoGravity(bool activate)
            {
                NoClip(false);
            }

            internal static void NoClipWithGravity(bool activate)
            {
                NoClip(true);
            }

            private static void NoClip(bool gravity)
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
                        bool revertToOriginal = false;
                        if(MGS2AoB.OriginalClippingBytes.Length == 0)
                        {
                            MGS2AoB.OriginalClippingBytes = memoryContent;
                        }
                        else if(MGS2AoB.OriginalClippingBytes != memoryContent)
                        {
                            revertToOriginal = true;
                        }

                        if (revertToOriginal)
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

                if (zoomIn)
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]++);
                else
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]--);
            }
        }
    }    

    public class MGS2Cheat
    {
        public static Cheat BlackScreen { get; } = new Cheat("Black Screen", Cheat.CheatActions.TurnScreenBlack, MGS2AoB.OriginalCameraBytes);
        public static Cheat NoBleedDamage { get; } = new Cheat("Bleeding Causes No Damage", Cheat.CheatActions.TurnOffBleedDamage, MGS2AoB.OriginalBleedDamageBytes);
        public static Cheat NoBurnDamage { get; } = new Cheat("Burning Causes No Damage", Cheat.CheatActions.TurnOffBurnDamage, MGS2AoB.OriginalBurnDamageBytes);
        public static Cheat InfiniteAmmo { get; } = new Cheat("Infinite Ammo", Cheat.CheatActions.InfiniteAmmo, MGS2AoB.OriginalAmmoBytes);
        public static Cheat InfiniteLife { get; } = new Cheat("Enemies Deal No Damage", Cheat.CheatActions.InfiniteLife, MGS2AoB.OriginalLifeBytes);
        public static Cheat InfiniteOxygen { get; } = new Cheat("Infinite Oxygen", Cheat.CheatActions.InfiniteOxygen, MGS2AoB.OriginalO2Bytes);
        public static Cheat Letterboxing { get; } = new Cheat("Letterboxing", Cheat.CheatActions.Letterboxing, MGS2AoB.OriginalCameraBytes);
        public static Cheat NoReload { get; } = new Cheat("Reloading Not Required", Cheat.CheatActions.AmmoNeverDepletes, MGS2AoB.OriginalReloadBytes);
        public static Cheat NoClipWithGravity { get; } = new Cheat("Walk Through Walls (gravity)", Cheat.CheatActions.NoClipWithGravity, MGS2AoB.OriginalClippingBytes);
        public static Cheat NoClipNoGravity { get; } = new Cheat("Walk Through Walls (no gravity)", Cheat.CheatActions.NoClipNoGravity, MGS2AoB.OriginalClippingBytes);
        public static Cheat ZoomIn { get; } = new Cheat("Zoom In", Cheat.CheatActions.ZoomIn, MGS2AoB.OriginalCameraBytes);
        public static Cheat ZoomOut { get; } = new Cheat("Zoom Out", Cheat.CheatActions.ZoomOut, MGS2AoB.OriginalCameraBytes);

        public static Cheat NoGripDamage { get; } = new Cheat("Infinite Grip Stamina", Cheat.CheatActions.GripNeverDepletes, MGS2AoB.OriginalGripDamageBytes);

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                BlackScreen, NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage,
                Letterboxing, NoClipWithGravity, NoClipNoGravity, NoReload,ZoomIn, ZoomOut
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
