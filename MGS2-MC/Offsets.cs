using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MGS2_MC.CommonObjects;

namespace MGS2_MC
{
    internal class Offsets
    {
        #region Offset Finders
        //if the region is dynamic(i.e. PlayerOffsetAoB), it will change on area load. The others will only (possibly[hopefully]) change with game updates
        #region Dynamic AoBs
        public static byte[] PlayerInfoFinderAoB = new byte[30] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00 }; //TODO: determine if this breaks when we have m9 max ammo >255
        #endregion
        #region Static AoBs
        public static byte[] MenuNamesFinderAoB = new byte[5] { 0x6D, 0x61, 0x70, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] DifficultyAndAreaNamesAoB = new byte[3] { 0x2F, 0x44, 0x2A }; //TODO: prove this is valid, also this is _concerningly_ short.
        public static byte[] LifeAndGripNamesAoB = new byte[8] { 0x72, 0x61, 0x69, 0x64, 0x65, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] RayNamesAoB = new byte[10] { 0x6D, 0x69, 0x6E, 0x69, 0x5F, 0x73, 0x63, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        //weapon & item descriptions dispersed through out. seems to start around +00613CCB or so in the memory print?
        public static byte[] RationMedsBandagePentazeminDescriptionsAoB = new byte[17] { 0xA4, 0xE3, 0x81, 0xAF, 0xE3, 0x81, 0x9A, 0xE3, 0x81, 0xA0, 0xEE, 0x80, 0x80, 0xE3, 0x80, 0x82, 0x0A }; //TODO: prove this is valid
        public static byte[] SolidusNameAoB = new byte[10] { 0x69, 0x6E, 0x69, 0x74, 0x5F, 0x73, 0x6F, 0x6C, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] EmmaO2AoB = new byte[16] { 0x65, 0x6D, 0x61, 0x5F, 0x72, 0x61, 0x69, 0x5F, 0x6F, 0x6E, 0x62, 0x75, 0x5F, 0x65, 0x6E, 0x64 }; //TODO: prove this is valid
        public static byte[] FatmanNameAoB = new byte[15] { 0x77, 0x61, 0x74, 0x65, 0x72, 0x6C, 0x69, 0x6E, 0x65, 0x66, 0x61, 0x6C, 0x6C, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] OlgaNameAoB = new byte[10] { 0x6F, 0x72, 0x67, 0x61, 0x5F, 0x6C, 0x6E, 0x7A, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] HarrierNameAoB = new byte[10] { 0x68, 0x61, 0x72, 0x5F, 0x76, 0x75, 0x6C, 0x63, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] KasatkaNameAoB = new byte[12] { 0x6B, 0x63, 0x6B, 0x5F, 0x70, 0x6C, 0x61, 0x6E, 0x74, 0x5F, 0x6D, 0x74 }; //TODO: prove this is valid
        public static byte[] FortuneNameAoB = new byte[] { 0x66, 0x6F, 0x72, 0x74, 0x5F, 0x6F, 0x62, 0x6A, 0x5F, 0x69, 0x6E, 0x69, 0x2E, 0x63 }; //TODO: prove this is valid
        #endregion
        #endregion

        #region Offsets
        #region Value offsets
        #region Calculated From PlayerInfo
        public static readonly MemoryOffset BASE_WEAPON = new MemoryOffset(-42, 0); //if a "new" playerOffsetBytes is chosen, only need to update this value and the item offset will update.
        public static readonly MemoryOffset BASE_ITEM = new MemoryOffset(BASE_WEAPON.Start + 144, BASE_WEAPON.Start + 144 + 80);
        public static readonly MemoryOffset SHOT_COUNT = new MemoryOffset(-96); //TODO: prove this is valid
        public static readonly MemoryOffset HOLD_UP_COUNT = new MemoryOffset(5108); //TODO: prove this is valid, prolly isnt tbqh
        public static readonly MemoryOffset PULL_UP_COUNT = new MemoryOffset(-90); //TODO: prove this is valid 
        public static readonly MemoryOffset PLAYER_COLD = new MemoryOffset(-128); //TODO: prove this is valid
        public static readonly MemoryOffset CURRENT_EQUIPPED_ITEM = new MemoryOffset(-130); //TODO: prove this is valid
        public static readonly MemoryOffset CURRENT_EQUIPPED_WEAPON = new MemoryOffset(-68); //TODO: prove this is valid
        public static readonly MemoryOffset PLAYER_STANCE = new MemoryOffset(-134); //TODO: prove this is valid
        public static readonly MemoryOffset PLAYER_SNEEZING = new MemoryOffset(-108, -107); //TODO: prove this is valid
        #endregion

        #region Calculated from Unknown Finder AoBs
        public const int TIMES_FOUND_GAME_LAUNCH_OFFSET = 0x17B786C; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations
        public const int HOLD_UPS_GAME_LAUNCH_OFFSET = 0x1673DB0; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations
        public const int CHOKE_OUTS_GAME_LAUNCH_OFFSET = 0x1673DBC; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations

        //TODO: add more of the game stats here


        //These values are PRESENTLY unknown
        //internal const int HealthPointerOffset = 0x00AE49D8; 
        //internal const int CurrentHealthOffset = 0x684;
        //internal const int MaxHealthOffset = 0x686;
        //internal const int StaminaOffset = 0xA4A;
        //internal static IntPtr HudOffset = (IntPtr)0xADB40F;
        //internal static IntPtr CamOffset = (IntPtr)0xAE3B37;
        //internal static IntPtr AlertStatusOffset = (IntPtr)0x1D9C3D8;
        #endregion
        #endregion
        #endregion
    }
}
