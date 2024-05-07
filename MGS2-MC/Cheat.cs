using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    public struct Cheat
    {
        public string Name { get; private set; }
        public Action CheatAction;

        public Cheat(string name, Action action)
        {
            Name = name;
            CheatAction = action;
        }

        internal class CheatActions
        {
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

            public static void TurnScreenBlack() //works
            {
                ModifySingleByte(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN, 0x00);
            }

            public static void TurnOffBleedDamage() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBleedDamage, MGS2Offset.NO_BLEED_DMG, 7); 
            }

            public static void TurnOffBurnDamage() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBurnDamage, MGS2Offset.NO_BURN_DMG, 7); 
            }

            internal static void InfiniteAmmo() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteAmmo, MGS2Offset.INFINITE_AMMO, 4); 
            }

            internal static void InfiniteLife() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteLife, MGS2Offset.INFINITE_LIFE, 4);
            }

            internal static void InfiniteOxygen() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteO2, MGS2Offset.INFINITE_O2, 4);
            }

            internal static void Letterboxing() //works
            {
                ModifySingleByte(MGS2AoB.Camera, MGS2Offset.LETTERBOX, 0x00);
            }

            internal static void AmmoNeverDepletes() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, 2); 
            }

            internal static void NoClipNoGravity() //doesnt exist within the game's memory, so we can't use it atm
            {
                NoClip(false);
            }

            internal static void NoClipWithGravity() //doesnt exist within the game's memory, so we can't use it atm
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
                            case Constants.PlayableCharacter.Snake:
                                activeCharacterAoB = MGS2AoB.SnakeClipping;
                                break;
                            default:
                                throw new NotImplementedException("This character is not (yet?) supported for no clip, sorry");
                        }
                        
                        SimplePattern pattern = new SimplePattern(activeCharacterAoB);
                        /*
                        int memoryLocation = spp.ScanMemoryForPattern(pattern);
                        byte[] memoryContent = spp.ReadProcessOffset(memoryLocation + MGS2Offset.NO_CLIP.Start, MGS2Offset.NO_CLIP.Length);
                        */

                        //byte[] gameMemoryBuffer = spp.GetProcessSnapshot();

                        //List<int> offsets = MGS2MemoryManager.FindUniqueOffset(gameMemoryBuffer, MGS2AoB.NeverReloadBytes);
                        IntPtr memoryLocation = spp.ScanMemoryForUniquePattern(pattern);
                        byte[] memoryContent = spp.ReadProcessOffset(memoryLocation + MGS2Offset.NO_CLIP.Start, MGS2Offset.NO_CLIP.Length);

                        if (gravity)
                        {
                            //set byte to either 15 or 25
                            if (memoryContent[0] == 0x24)
                            {
                                memoryContent[0] = 0x25;
                            }
                            else
                            {
                                memoryContent[0] = 0x15;
                            }
                        }
                        else
                        {
                            //set byte to either 13 or 23
                            if (memoryContent[0] == 0x24)
                            {
                                memoryContent[0] = 0x23;
                            }
                            else
                            {
                                memoryContent[0] = 0x13;
                            }
                        }

                        spp.ModifyProcessOffset(memoryLocation + MGS2Offset.NO_CLIP.Start, memoryContent, true);
                    }
                }
            }

            internal static void ZoomIn()
            {
                Zoom(true);
            }

            internal static void ZoomOut()
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
        public static Cheat BlackScreen { get; } = new Cheat("Black Screen", Cheat.CheatActions.TurnScreenBlack);
        public static Cheat NoBleedDamage { get; } = new Cheat("Bleeding Causes No Damage", Cheat.CheatActions.TurnOffBleedDamage);
        public static Cheat NoBurnDamage { get; } = new Cheat("Burning Causes No Damage", Cheat.CheatActions.TurnOffBurnDamage);
        public static Cheat InfiniteAmmo { get; } = new Cheat("Infinite Ammo", Cheat.CheatActions.InfiniteAmmo);
        public static Cheat InfiniteLife { get; } = new Cheat("Infinite Life", Cheat.CheatActions.InfiniteLife);
        public static Cheat InfiniteOxygen { get; } = new Cheat("Infinite Oxygen", Cheat.CheatActions.InfiniteOxygen);
        public static Cheat Letterboxing { get; } = new Cheat("Letterboxing", Cheat.CheatActions.Letterboxing);
        public static Cheat NoReload { get; } = new Cheat("Reloading Not Required", Cheat.CheatActions.AmmoNeverDepletes);
        public static Cheat NoClipWithGravity { get; } = new Cheat("Walk Through Walls (gravity)", Cheat.CheatActions.NoClipWithGravity);
        public static Cheat NoClipNoGravity { get; } = new Cheat("Walk Through Walls (no gravity)", Cheat.CheatActions.NoClipNoGravity);
        public static Cheat ZoomIn { get; } = new Cheat("Zoom In", Cheat.CheatActions.ZoomIn);
        public static Cheat ZoomOut { get; } = new Cheat("Zoom Out", Cheat.CheatActions.ZoomOut);

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                BlackScreen, NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, Letterboxing,
                NoClipWithGravity, NoClipNoGravity, //cutting these out for now, as they dont work and/or crash the game
                NoReload,ZoomIn, ZoomOut
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
