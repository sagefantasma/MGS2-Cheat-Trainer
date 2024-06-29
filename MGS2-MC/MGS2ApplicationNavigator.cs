using System;
using System.Windows.Forms;

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

    internal struct MGS2Pointer
    {
        internal static int WalkThroughWalls = 0x01534E68;
        internal static int ModifiableHP = 0x017C1A20;
        internal static int CurrentGrip = 0x016CBDA0;
    }

    internal struct MGS2AoB
    {
        //if the region is dynamic(i.e. PlayerOffsetAoB), it will change on area load. The others will only (possibly[hopefully]) change with game updates
        #region Dynamic AoBs
        internal static byte[] PlayerInfoFinderRaiden = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x01, 0x00, 0x10, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x14, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0F, 0x27, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 };
        internal static byte[] PlayerInfoFinderSnake = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x14, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0F, 0x27, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 };
        internal static byte[] PlayerInfoFinderGeneric = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x19, 0x00, 0x01, 0x00, 0x19, 0x00, 0x19, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0F, 0x27, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 };
        internal static string PlayerInfoFinderString = "01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 19 00 01 00 ?? 00 01 00 01 00 01 00 19 00 19 00 ?? 00 01 00 19 00 19 00 01 00 01 00 01 00 01 00 01 00 0F 27 01 00 01 00 01 00 01 00 01 00 01 00 01";

        internal static byte[] MenuNamesFinder = new byte[] { 0x6D, 0x61, 0x70, 0x2E, 0x63 }; //TODO: prove this is valid
        internal static byte[] DifficultyAndAreaNames = new byte[] { 0x2F, 0x44, 0x2A }; //TODO: prove this is valid, also this is _concerningly_ short.
        internal static string LifeAndGripNames = "72 61 69 64 65 6E 2E 63";
        internal static string RayNames = "6D 69 6E 69 5F 73 63 6E 2E 63";
        //weapon & item descriptions dispersed through out. seems to start around +00613CCB or so in the memory print?
        internal static byte[] RationMedsBandagePentazeminDescriptions = new byte[] { 0xA4, 0xE3, 0x81, 0xAF, 0xE3, 0x81, 0x9A, 0xE3, 0x81, 0xA0, 0xEE, 0x80, 0x80, 0xE3, 0x80, 0x82, 0x0A }; //TODO: prove this is valid
        internal static string SolidusName = "69 6E 69 74 5F 73 6F 6C 2E 63";
        internal static string EmmaName = "62 72 6B 5F 67 6C 73 5F 69 6E 69 2E 63";
        internal static string EmmaO2 = "65 6D 61 5F 72 61 69 5F 6F 6E 62 75 5F 65 6E 64";
        internal static string FatmanName = "77 61 74 65 72 6C 69 6E 65 66 61 6C 6C 2E 63"; 
        internal static string OlgaName = "6F 72 67 61 5F 6C 6E 7A 2E 63";
        internal static string HarrierName = "68 61 72 5F 76 75 6C 63 2E 63";
        internal static string KasatkaName = "6B 63 6B 5F 70 6C 61 6E 74 5F 6D 74";
        internal static string FortuneName = "66 6F 72 74 5F 6F 62 6A 5F 69 6E 69 2E 63";
        internal static string Vamp02 = "76 61 6D 70 2E 63";
        //00 00 00 78 00 08 00 <-- possibly an AoB for HP/Magazine modificaitons? might have to key off of LIFE?(or at least whatever it is called at that moment, within the games memory block)
        internal static byte[] HealthMod = new byte[] { 0x00, 0x00, 0x00, 0x78, 0x00, 0x08, 0x00 }; //TODO: prove this is valid
        //clipcurrentCount == -114 from the above AoB, 4bytes long. HP mod is DIRECTLY after, it seems?
        internal static byte[] StageInfo = new byte[] { 0x10, 0x0E, 0x18, 0x15, 0x00, 0x00 };
        internal static string StageInfoString = "10 0E 18 15 00 00";
        //current "stage" == -267 from the above AoB, 5 bytes long. If it has r_tnk, it is Snake. if it is r_plt, it is raiden :)
        internal static byte[] OriginalAmmoBytes = new byte[] { 0x66, 0x89, 0x0C, 0x1A, 0x48, 0x8B, 0x5C, 0x24, 0x30, 0x0F, 0xBF, 0xC1, 0x48, 0x83, 0xC4, 0x20 };
        internal static string RestoreAmmo = "90 90 90 90 48 8B 5C 24 30 0F BF C1 48 83 C4 20";
        internal static string InfiniteAmmo = "66 89 0C 1A 48 8B 5C 24 30 0F BF C1 48 83 C4 20";
        internal static byte[] OriginalReloadBytes = { 0xFF, 0xC8, 0x89, 0x05, 0xF2, 0xBB, 0x18, 0x01 };
        internal static string RestoreReload = "90 90 89 05 F2 BB 18 01";
        internal static string NeverReload = "FF C8 89 05 F2 BB 18 01";
        internal static byte[] OriginalLifeBytes = { 0x66, 0x29, 0x43, 0x12, 0xE9, 0x67, 0xFF, 0xFF, 0xFF, 0x41, 0x0F, 0x28, 0xF9, 0x41, 0x0F, 0x28 };
        internal static string RestoreLife = "90 90 90 90 E9 67 FF FF FF 41 0F 28 F9 41 0F 28";
        internal static string InfiniteLife = "66 29 43 12 E9 67 FF FF FF 41 0F 28 F9 41 0F 28";
        internal static byte[] OriginalO2Bytes = { 0x66, 0x89, 0x47, 0x78, 0xF3, 0x0F, 0x11, 0x87, 0xE0, 0x00, 0x00, 0x00, 0x44, 0x0F, 0xBF, 0x47 };
        internal static string RestoreO2 = "90 90 90 90 F3 0F 11 87 E0 00 00 00 44 0F BF 47";
        internal static string InfiniteO2 = "66 89 47 78 F3 0F 11 87 E0 00 00 00 44 0F BF 47";
        internal static byte[] OriginalBleedDamageBytes = { 0x66, 0x89, 0x8B, 0xD2, 0x08, 0x00, 0x00, 0xB9, 0x00, 0x00, 0x01, 0x00, 0xE8, 0x5C, 0x9E, 0x03 };
        internal static string RestoreBleedDamage = "90 90 90 90 90 90 90 B9 00 00 01 00 E8 5C 9E 03";
        internal static string NoBleedDamage = "66 89 8B D2 08 00 00 B9 00 00 01 00 E8 5C 9E 03";
        internal static byte[] OriginalBurnDamageBytes = { 0x66, 0x89, 0x83, 0xD2, 0x08, 0x00, 0x00, 0x8B, 0x0D, 0x96, 0xBA, 0x19, 0x01, 0x0F, 0xBF, 0xD0 };
        internal static string RestoreBurnDamage = "90 90 90 90 90 90 90 8B 0D 96 BA 19 01 0F BF D0";
        internal static string NoBurnDamage = "66 89 83 D2 08 00 00 8B 0D 96 BA 19 01 0F BF D0";
        internal static byte[] OriginalClippingBytes = { };
        internal static string RaidenClipping = "01 ?? 00 00 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string NinjaClipping = "01 ?? F4 01 24 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string NakedRaidenClipping = "01 ?? 01 24 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string SnakeClipping = "01 ?? 00 00 00 00 00 00 00 00 00 00 00 E6 DC 28";
        internal static string VRClipping = "DA 01 F4 01 ?? 00 00 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string PliskinClipping = "DA 01 F4 01 ?? 00 00 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string TuxedoSnakeClipping = "DA 01 F4 01 ?? 00 00 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static string MGS1SnakeClipping = "DA 01 F4 01 ?? 00 00 00 00 00 00 00 00 00 00 00 27 15 9B";
        internal static byte[] OriginalCameraBytes = { 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x17, 0x42, 0x53, 0x17, 0x06, 0x53, 0x42, 0x44 };
        internal static string Camera = "45 00 00 00 00 00 00 00 06 17 42 53 17 06 53 42 44";
        internal static string DecrementGripGauge = "66 89 BB 18 04 00 00";
        internal static byte[] OriginalGripDamageBytes = { 0x66, 0x89, 0xBB, 0x18, 0x04, 0x00, 0x00 };
        internal static string IncrementGripGauge = "90 90 BB 18 04 00 00";
        internal static string InGamePause = "A9 00 07 00 00 0F 85 C4";
        internal static byte[] OriginalPauseButtonBytes = { 0xA9, 0x00, 0x07, 0x00, 0x00 };
        internal static string ItemMenuPause = "09 35 B1 EF 5D 01";
        internal static byte[] OriginalItemMenuPauseBytes = { 0x09, 0x35, 0xB1, 0xEF, 0x5D, 0x01 };
        internal static string WeaponMenuPause = "09 35 14 D5 5D 01";
        internal static byte[] OriginalWeaponMenuPauseBytes = { 0x09, 0x35, 0x14, 0xD5, 0x5D, 0x01 };



        /*new offsets and shit found 4/17/24:
        * Offsets from AoBs:
        * MaxLife is -140 bytes from PlayerInfoFinder (2bytes)
        * Current(unmodifiable) health is -142 bytes from PlayerInfoFinder (2bytes)
        * Player o2 is -138 bytes from PlayerInfoFinder (2 bytes)
        * Snake grip level is -91~(0A) bytes from PlayerInfoFinder (2 bytes)
        * Raiden grip level is -89~ bytes from PlayerInfoFinder (2 bytes)
        * 
        * New AoBs:
        * ED 26 61 F7 7F 00 00 <--- current health + grip AoB? (this is also outside of the MGS2.exe, so idk if it will work)
        *               - ED 26 61 F7 7F 00 00 E0 C5 53 6D 01 02 00 00 E6 DC 28 for NG Hard as Snake at start of game
        *               - ED 26 61 F7 7F 00 00 00 29 36 6D 01 02 00 00 E6 DC 28 for NG Hard as Snake in Hold 2
        *               - ED 26 61 F7 7F 00 00 F0 29 36 6D 01 02 00 00 E6 DC 28 for NG Hard as Snake in Hold 2 after reentering once
        *               - ED 26 61 F7 7F 00 00 80 A0 5E 6D 01 02 00 00 27 15 9B for NG++ Normal as Raiden at Shell 1-2 Connecting Bridge
        *               - ED 26 61 F7 7F 00 00 70 C8 64 6D 01 02 00 00 27 15 9B for NG Easy as Raiden at Shell A Roof(Plant only)
        *               
        *      current health is either + 41 or 42 bytes from the above AoB
        *      current grip value is either -1169 or -1168 above the above aob
        */
        #endregion
    }

    internal struct MGS2Offset
    {
        #region Offsets
        #region String offsets
        #region Calculated From LifeAndGripNames
        public static readonly MemoryOffset LIFE_TEXT = new MemoryOffset(9, 12); 
        public static readonly MemoryOffset GRIP_Lv1_TEXT = new MemoryOffset(21, 28);
        public static readonly MemoryOffset GRIP_Lv2_TEXT = new MemoryOffset(-156, -149);
        public static readonly MemoryOffset GRIP_Lv3_TEXT = new MemoryOffset(-172, -165);
        #endregion

        #region Calculated From RationMedsBandagePentazeminDescriptions
        //TODO: fill these out
        #endregion

        #region Calculated From RayNames
        public static readonly MemoryOffset RAY_01 = new MemoryOffset(70, 77);
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
        public static readonly MemoryOffset SOLIDUS_HP_TEXT = new MemoryOffset(60, 66);
        #endregion

        #region Calculated From EmmaO2
        public static readonly MemoryOffset EMMA_O2_TEXT = new MemoryOffset(24, 30);
        public static readonly MemoryOffset RAIDEN_O2_TEXT = new MemoryOffset(136, 137);
        #endregion

        #region Calculated From EmmaName
        public static readonly MemoryOffset EMMA_HP_TEXT = new MemoryOffset(390, 393);
        #endregion

        #region Calculated From FatmanName
        public static readonly MemoryOffset FATMAN_HP_TEXT = new MemoryOffset(88, 93);
        #endregion

        #region Calculated From OlgaName
        public static readonly MemoryOffset OLGA_HP_TEXT = new MemoryOffset(292, 295);
                                                                                       //there is also a meryl string right next to OLGA... but idk what it is used for so i'm not bothering to add it atm
                                                                                       //guessing the meryl^^ string is related to the OLGA boss fight!
        #endregion

        #region Calculated From HarrierName
        public static readonly MemoryOffset HARRIER_HP_TEXT = new MemoryOffset(-109, -103);
        #endregion

        #region Calculated From KasatkaName
        public static readonly MemoryOffset KASATKA_HP_TEXT = new MemoryOffset(-8, -2);
        #endregion

        #region Calculated From FortuneName
        public static readonly MemoryOffset FORTUNE_HP_TEXT = new MemoryOffset(1188, 1194);
        public static readonly MemoryOffset VAMP_HP_TEXT = new MemoryOffset(1908, 1911);
        #endregion

        #region Calculated From Vamp02
        public static readonly MemoryOffset VAMP_02_TEXT = new MemoryOffset(8, 14);
        #endregion
        #endregion

        #region Value offsets
        #region Calculated From PlayerInfo
        public static readonly MemoryOffset BASE_WEAPON = new MemoryOffset(-250, 0); //if a "new" playerOffsetBytes is chosen, only need to update this value and the item offset will update.
        public static readonly MemoryOffset BASE_ITEM = new MemoryOffset(BASE_WEAPON.Start + 144, BASE_WEAPON.Start + 144 + 80);
        public static readonly MemoryOffset GRIP_LEVEL_SNAKE = new MemoryOffset(-634, -633);
        public static readonly MemoryOffset GRIP_LEVEL_RAIDEN = new MemoryOffset(-632, -631);
        #endregion

        #region Calculated From HealthMod
        public static readonly MemoryOffset MODIFY_PLAYER_HP = new MemoryOffset(-110, -107); //TODO: prove this is valid
        public static readonly MemoryOffset MODIFY_CLIP_SIZE = new MemoryOffset(-114, -111); //TODO: prove this is valid
        #endregion

        #region Calculated From StageInfo
        public static readonly MemoryOffset CURRENT_CHARACTER = new MemoryOffset(-260, -255);
        public static readonly MemoryOffset CURRENT_MAX_HP = new MemoryOffset(-36, -35);
        public static readonly MemoryOffset CURRENT_HP = new MemoryOffset(-38, -37);
        public static readonly MemoryOffset CURRENT_STAGE = new MemoryOffset(-244, -238);
        public static readonly MemoryOffset CURRENT_DIFFICULTY = new MemoryOffset(-272);
        public static readonly MemoryOffset NGPLUS_COUNT = new MemoryOffset(-271, -272); //TODO: prove this is valid
        public static readonly MemoryOffset CURRENT_GAMETYPE = new MemoryOffset(-281); //TODO: prove this is valid
        public static readonly MemoryOffset CURRENT_EQUIPPED_ITEM = new MemoryOffset(-26); //TODO: prove this is valid
        public static readonly MemoryOffset CURRENT_EQUIPPED_WEAPON = new MemoryOffset(-28); //TODO: prove this is valid
        public static readonly MemoryOffset GAME_STATS_BLOCK = new MemoryOffset(14, 57);
        public static readonly MemoryOffset DAMAGE_TAKEN = new MemoryOffset(38);
        public static readonly MemoryOffset KILL_COUNT = new MemoryOffset(36);
        public static readonly MemoryOffset ALERT_COUNT = new MemoryOffset(34);
        public static readonly MemoryOffset SHOT_COUNT = new MemoryOffset(32);
        public static readonly MemoryOffset PLAY_TIME = new MemoryOffset(26, 29);
        public static readonly MemoryOffset SAVE_COUNT = new MemoryOffset(22);
        public static readonly MemoryOffset CONTINUE_COUNT = new MemoryOffset(18);
        public static readonly MemoryOffset MECHS_DESTROYED = new MemoryOffset(56);
        public static readonly MemoryOffset PULL_UP_COUNT = new MemoryOffset(14);
        public static readonly MemoryOffset SPECIAL_ITEMS_USED = new MemoryOffset(5238, 5239);
        public static readonly MemoryOffset RATIONS_USED = new MemoryOffset(5232, 5233);
        #endregion

        #region Cheats offsets
        public static readonly MemoryOffset INFINITE_AMMO = new MemoryOffset(0, 16);
        public static readonly MemoryOffset NEVER_RELOAD = new MemoryOffset(0, 16);
        public static readonly MemoryOffset INFINITE_LIFE = new MemoryOffset(0, 16);
        public static readonly MemoryOffset INFINITE_O2 = new MemoryOffset(0, 16);
        public static readonly MemoryOffset NO_BLEED_DMG = new MemoryOffset(0, 16);
        public static readonly MemoryOffset NO_BURN_DMG = new MemoryOffset(0, 16);
        public static readonly MemoryOffset NO_CLIP = new MemoryOffset(0x40, 0x53);
        public static readonly MemoryOffset LETTERBOX = new MemoryOffset(-187);
        public static readonly MemoryOffset ZOOM = new MemoryOffset(-189); //todo: verify
        public static readonly MemoryOffset BLACK_SCREEN = new MemoryOffset(-228);
        public static readonly MemoryOffset NO_GRIP_DMG = new MemoryOffset(0, 6);
        public static readonly MemoryOffset NO_PAUSE_BTN = new MemoryOffset(0, 5);
        public static readonly MemoryOffset NO_ITEM_PAUSE = new MemoryOffset(0, 6);
        public static readonly MemoryOffset NO_WEAPON_PAUSE = new MemoryOffset(0, 6);
        #endregion

        #region Calculated From CurrentGripGauge
        public static readonly MemoryOffset CURRENT_GRIP_GAUGE = new MemoryOffset(136, 137);
        #endregion

        #region Calculated From ModifiableHP
        public static readonly MemoryOffset MODIFIABLE_HP = new MemoryOffset(2258, 2259);
        #endregion

        #region Calculated from Unknown Finder AoBs    
        public static readonly MemoryOffset PLAYER_COLD = new MemoryOffset(-128); //TODO: prove this is valid, if even useful        
        public static readonly MemoryOffset PLAYER_STANCE = new MemoryOffset(-134); //TODO: prove this is valid, if even useful
        public static readonly MemoryOffset PLAYER_SNEEZING = new MemoryOffset(-108, -107); //TODO: prove this is valid, if even useful

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
