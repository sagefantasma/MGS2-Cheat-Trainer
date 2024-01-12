using System;

namespace MGS2_MC
{
    /// <summary>
    /// Details memory information related to an object.
    /// Start determines the byte where the object starts being defined.
    /// End determines the byte where the object stops being defined.
    /// Length is automatically calculated, and determines how many bytes of memory comprise the object.
    /// </summary>
    public struct MemoryOffset
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; set; }

        public MemoryOffset(int offsetStart) : this(offsetStart, offsetStart)
        {
        }

        public MemoryOffset(int offsetStart, int offsetEnd)
        {
            Start = offsetStart;
            End = offsetEnd;
            Length = Math.Abs(offsetEnd - offsetStart) + 1;
        }
    }

    internal struct MGS2Offsets
    {
        #region Offset Finders
        //if the region is dynamic(i.e. PlayerOffsetAoB), it will change on area load. The others will only (possibly[hopefully]) change with game updates
        #region Dynamic AoBs
        internal static byte[] PlayerInfoFinderAoB = new byte[30] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00 }; //TODO: determine if this breaks when we have m9 max ammo >255
        #endregion
        #region Static AoBs
        internal static byte[] MenuNamesFinderAoB = new byte[5] { 0x6D, 0x61, 0x70, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] DifficultyAndAreaNamesAoB = new byte[3] { 0x2F, 0x44, 0x2A }; //TODO: prove this is valid, also this is _concerningly_ short.
        internal static byte[] LifeAndGripNamesAoB = new byte[8] { 0x72, 0x61, 0x69, 0x64, 0x65, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] RayNamesAoB = new byte[10] { 0x6D, 0x69, 0x6E, 0x69, 0x5F, 0x73, 0x63, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        //weapon & item descriptions dispersed through out. seems to start around +00613CCB or so in the memory print?
        internal static byte[] RationMedsBandagePentazeminDescriptionsAoB = new byte[17] { 0xA4, 0xE3, 0x81, 0xAF, 0xE3, 0x81, 0x9A, 0xE3, 0x81, 0xA0, 0xEE, 0x80, 0x80, 0xE3, 0x80, 0x82, 0x0A }; //TODO: prove this is valid
        internal static byte[] SolidusNameAoB = new byte[10] { 0x69, 0x6E, 0x69, 0x74, 0x5F, 0x73, 0x6F, 0x6C, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] EmmaO2AoB = new byte[16] { 0x65, 0x6D, 0x61, 0x5F, 0x72, 0x61, 0x69, 0x5F, 0x6F, 0x6E, 0x62, 0x75, 0x5F, 0x65, 0x6E, 0x64 }; //TODO: prove this is valid
        internal static byte[] FatmanNameAoB = new byte[15] { 0x77, 0x61, 0x74, 0x65, 0x72, 0x6C, 0x69, 0x6E, 0x65, 0x66, 0x61, 0x6C, 0x6C, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] OlgaNameAoB = new byte[10] { 0x6F, 0x72, 0x67, 0x61, 0x5F, 0x6C, 0x6E, 0x7A, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] HarrierNameAoB = new byte[10] { 0x68, 0x61, 0x72, 0x5F, 0x76, 0x75, 0x6C, 0x63, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] KasatkaNameAoB = new byte[12] { 0x6B, 0x63, 0x6B, 0x5F, 0x70, 0x6C, 0x61, 0x6E, 0x74, 0x5F, 0x6D, 0x74 }; //TODO: prove this is valid
        internal static byte[] FortuneNameAoB = new byte[] { 0x66, 0x6F, 0x72, 0x74, 0x5F, 0x6F, 0x62, 0x6A, 0x5F, 0x69, 0x6E, 0x69, 0x2E, 0x63 }; //TODO: prove this is valid
        #endregion
        #endregion

        #region Offsets
        #region String offsets
        #region Calculated From LifeAndGripNames
        public static readonly MemoryOffset LIFE_TEXT = new MemoryOffset(10, 13); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv1_TEXT = new MemoryOffset(22, 29); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv2_TEXT = new MemoryOffset(-156, -149); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv3_TEXT = new MemoryOffset(-172, -165); //TODO: prove this is valid
        #endregion

        #region Calculated From RationMedsBandagePentazeminDescriptions
        //TODO: fill these out
        #endregion

        #region Calculated From RayNames
        public static readonly MemoryOffset RAY_01 = new MemoryOffset(78, 85); //TODO: prove these are valid (these values are using the endByte, cuz im a fool)
        //public static readonly MemoryBytes RAY_01 = new MemoryBytes(68, 75); //This should work if the uncommented one does not
        public static readonly MemoryOffset RAY_02 = new MemoryOffset(RAY_01.Start + 16, RAY_01.End + 16);
        public static readonly MemoryOffset RAY_03 = new MemoryOffset(RAY_02.Start + 16, RAY_02.End + 16);
        public static readonly MemoryOffset RAY_04 = new MemoryOffset(RAY_03.Start + 16, RAY_03.End + 16);
        public static readonly MemoryOffset RAY_05 = new MemoryOffset(RAY_04.Start + 16, RAY_04.End + 16);
        public static readonly MemoryOffset RAY_06 = new MemoryOffset(RAY_05.Start + 16, RAY_05.End + 16);
        public static readonly MemoryOffset RAY_07 = new MemoryOffset(RAY_06.Start + 16, RAY_06.End + 16);
        public static readonly MemoryOffset RAY_08 = new MemoryOffset(RAY_07.Start + 16, RAY_07.End + 16);
        public static readonly MemoryOffset RAY_09 = new MemoryOffset(RAY_08.Start + 16, RAY_08.End + 16);
        public static readonly MemoryOffset RAY_10 = new MemoryOffset(RAY_09.Start + 16, RAY_09.End + 16);
        public static readonly MemoryOffset RAY_11 = new MemoryOffset(RAY_10.Start + 16, RAY_10.End + 16);
        public static readonly MemoryOffset RAY_12 = new MemoryOffset(RAY_11.Start + 16, RAY_11.End + 16);
        public static readonly MemoryOffset RAY_13 = new MemoryOffset(RAY_12.Start + 16, RAY_12.End + 16);
        public static readonly MemoryOffset RAY_14 = new MemoryOffset(RAY_13.Start + 16, RAY_13.End + 16);
        public static readonly MemoryOffset RAY_15 = new MemoryOffset(RAY_14.Start + 16, RAY_14.End + 16);
        public static readonly MemoryOffset RAY_16 = new MemoryOffset(RAY_15.Start + 16, RAY_15.End + 16);
        public static readonly MemoryOffset RAY_17 = new MemoryOffset(RAY_16.Start + 16, RAY_16.End + 16);
        public static readonly MemoryOffset RAY_18 = new MemoryOffset(RAY_17.Start + 16, RAY_17.End + 16);
        public static readonly MemoryOffset RAY_19 = new MemoryOffset(RAY_18.Start + 16, RAY_18.End + 16);
        public static readonly MemoryOffset RAY_20 = new MemoryOffset(RAY_19.Start + 16, RAY_19.End + 16);
        public static readonly MemoryOffset RAY_21 = new MemoryOffset(RAY_20.Start + 16, RAY_20.End + 16);
        public static readonly MemoryOffset RAY_22 = new MemoryOffset(RAY_21.Start + 16, RAY_21.End + 16);
        public static readonly MemoryOffset RAY_23 = new MemoryOffset(RAY_22.Start + 16, RAY_22.End + 16);
        public static readonly MemoryOffset RAY_24 = new MemoryOffset(RAY_23.Start + 16, RAY_23.End + 16);
        public static readonly MemoryOffset RAY_25 = new MemoryOffset(RAY_24.Start + 16, RAY_24.End + 16);
        #endregion

        #region Calculated From SolidusName
        public static readonly MemoryOffset SOLIDUS_HP_TEXT = new MemoryOffset(61, 67); //TODO: prove this is valid
        #endregion

        #region Calculated From EmmaO2
        public static readonly MemoryOffset EMMA_O2_TEXT = new MemoryOffset(25, 31); //TODO: prove this is valid
        public static readonly MemoryOffset RAIDEN_O2_TEXT = new MemoryOffset(137, 138); //TODO: prove this is valid
        #endregion

        #region Calculated From FatmanName
        public static readonly MemoryOffset FATMAN_HP_TEXT = new MemoryOffset(89, 94); //TODO: validate
        #endregion

        #region Calculated From OlgaName
        public static readonly MemoryOffset OLGA_HP_TEXT = new MemoryOffset(293, 296); //TODO: validate
                                                                                       //there is also a meryl string right next to OLGA... but idk what it is used for so i'm not bothering to add it atm
        #endregion

        #region Calculated From HarrierName
        public static readonly MemoryOffset HARRIER_HP_TEXT = new MemoryOffset(-109, -103);
        #endregion

        #region Calculated From KasatkaName
        public static readonly MemoryOffset KASATKA_HP_TEXT = new MemoryOffset(-8, -2);
        #endregion

        #region Calculated From FortuneName
        public static readonly MemoryOffset FORTUNE_HP_TEXT = new MemoryOffset(1189, 1195); //TODO: validate
        #endregion
        #endregion

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
