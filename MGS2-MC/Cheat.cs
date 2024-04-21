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
                    using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        SimplePattern pattern = new SimplePattern(aob);
                        int memoryLocation = spp.ScanMemoryForPattern(pattern);

                        byte[] memoryContent = spp.ReadProcessOffset(memoryLocation, offset.Length);

                        for (int i = startIndexToReplace; i < startIndexToReplace+bytesToReplace; i++)
                        {
                            memoryContent[i] = 0x90;
                        }

                        spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                    }
                }
            }

            public static void TurnScreenBlack()
            {

            }

            public static void TurnOffBleedDamage()
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBleedDamage, MGS2Offset.NO_BLEED_DMG, 7);
            }

            public static void TurnOffBurnDamage()
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBurnDamage, MGS2Offset.NO_BURN_DMG, 7);
            }

            internal static void InfiniteAmmo()
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteAmmo, MGS2Offset.INFINITE_AMMO, 4);
            }

            internal static void InfiniteLife()
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteLife, MGS2Offset.INFINITE_LIFE, 4);
            }

            internal static void InfiniteOxygen()
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteO2, MGS2Offset.INFINITE_O2, 4);
            }

            internal static void Letterboxing()
            {
                throw new NotImplementedException();
            }

            internal static void AmmoNeverDepletes()
            {
                ReplaceWithInvalidCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, 4);
            }

            internal static void NoClipNoGravity()
            {
                NoClip(false);
            }

            internal static void NoClipWithGravity()
            {
                NoClip(true);
            }

            private static void NoClip(bool gravity)
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                NoReload, NoClipWithGravity, NoClipNoGravity, ZoomIn, ZoomOut
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
