using System;
using System.Collections.Generic;
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
        internal static int WalkThroughWalls = 0x01551078;
        internal static int ModifiableHP = 0x017DE780; 
        internal static int CurrentGrip = 0x016E8B20;
        internal static int CurrentAmmo = 0x0153FC10;
        internal static List<int> OlgaNestedPointers = new List<int> { 0x01534CF8, 0xF0, 0x8, 0x590 };
        internal static List<int> FortuneNestedPointers = null;
        internal static List<int> FatmanNestedHealthPointers = new List<int> { 0x1795F88 };
        internal static List<int> FatmanNestedStaminaPointers = new List<int> { 0x1534CF0 };
        internal static List<int> HarrierNestedPointers = new List<int> { 0x17972A0, 0x70 };
        internal static List<int> VampNestedPointers = new List<int> { 0x1534740 };
        internal static List<int> VampSnipingHealthNestedPointers = new List<int> { 0x1795F98 };
        internal static List<int> VampSnipingStaminaNestedPointers = new List<int> { 0x1534740, 0x68 };
        internal static List<int> Ray1NestedPointers = new List<int> { 0x16CC378, 0x170 };
        internal static List<int> Ray2NestedPointers = new List<int> { 0x16CC378, 0x190 };
        internal static List<int> Ray3NestedPointers = new List<int> { 0x16CC378, 0x1B0 };
        internal static List<int> Ray4NestedPointers = new List<int> { 0x16CC378, 0x1D0 };
        internal static List<int> Ray5NestedPointers = new List<int> { 0x16CC378, 0x1F0 };
        internal static List<int> Ray6NestedPointers = new List<int> { 0x16CC378, 0x210 };
        internal static List<int> Ray7NestedPointers = new List<int> { 0x16CC378, 0x230 };
        internal static List<int> Ray8NestedPointers = new List<int> { 0x16CC378, 0x250 };
        internal static List<int> Ray9NestedPointers = new List<int> { 0x16CC378, 0x270 };
        internal static List<int> Ray10NestedPointers = new List<int> { 0x16CC378, 0x290 };
        internal static List<int> Ray11NestedPointers = new List<int> { 0x16CC378, 0x2B0 };
        internal static List<int> Ray12NestedPointers = new List<int> { 0x16CC378, 0x2D0 };
        internal static List<int> Ray13NestedPointers = new List<int> { 0x16CC378, 0x2F0 };
        internal static List<int> Ray14NestedPointers = new List<int> { 0x16CC378, 0x310 };
        internal static List<int> Ray15NestedPointers = new List<int> { 0x16CC378, 0x330 };
        internal static List<int> Ray16NestedPointers = new List<int> { 0x16CC378, 0x350 };
        internal static List<int> Ray17NestedPointers = new List<int> { 0x16CC378, 0x370 };
        internal static List<int> Ray18NestedPointers = new List<int> { 0x16CC378, 0x390 };
        internal static List<int> Ray19NestedPointers = new List<int> { 0x16CC378, 0x3B0 };
        internal static List<int> Ray20NestedPointers = new List<int> { 0x16CC378, 0x3D0 };
        internal static List<int> Ray21NestedPointers = new List<int> { 0x16CC378, 0x3F0 };
        internal static List<int> Ray22NestedPointers = new List<int> { 0x16CC378, 0x410 };
        internal static List<int> Ray23NestedPointers = new List<int> { 0x16CC378, 0x430 };
        internal static List<int> Ray24NestedPointers = new List<int> { 0x16CC378, 0x450 };
        internal static List<int> Ray25NestedPointers = new List<int> { 0x16CC378, 0x470 };
        internal static List<int> SolidusNestedPointers = new List<int> { 0x17970B8, 0x70 };
        //internal static List<int> SolidusNestedStaminaPointers = new List<>
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
        internal static string StandardGuardSleep = "66 44 29 82 52 13 00 00";
        internal static byte[] StandardGuardSleepBytes = { 0x66, 0x44, 0x29, 0x82, 0x52, 0x13, 0x00, 0x00 };
        internal static string ForceGuardsToSleep = "83 AA 52 13 00 00 35 90";
        internal static byte[] ForceGuardsToSleepBytes = { 0x83, 0xAA, 0x52, 0x13, 0x00, 0x00, 0x35, 0x90 };
        internal static string StandardGuardWake = "66 83 AB 5A 13 00 00 01 79 07";
        internal static byte[] StandardGuardWakeBytes = { 0x66, 0x83, 0xAB, 0x5A, 0x13, 0x00, 0x00, 0x01, 0x79, 0x07 };
        internal static string ForceGuardsToWake = "66 81 AB 5A 13 00 00 00 10 90";
        internal static byte[] ForceGuardsToWakeBytes = { 0x66, 0x81, 0xAB, 0x5A, 0x13, 0x00, 0x00, 0x00, 0x10, 0x90 };
        internal static string InfiniteItemUse = "66 41 89 09 0F BF C1";
        internal static byte[] OriginalItemUseBytes = { 0x66, 0x41, 0x89, 0x09, 0x0F, 0xBF, 0xC1 };
        internal static string MaxCountOnPickup = "44 8B 47 60 8B 57 5C";
        internal static byte[] OriginalCountOnPickup = { 0x44, 0x8B, 0x47, 0x60, 0x8B, 0x57, 0x5C };
        internal static string KnockoutDuration = "66 83 AB 5A 13 00 00 01";
        internal static byte[] OriginalKnockoutDuration = { 0x66, 0x83, 0xAB, 0x5A, 0x13, 0x00, 0x00, 0x01 };
        internal static string RemovePlantFilter = "F6 05 0F BF 4C 01 01";
        internal static byte[] OriginalRemovePlantFilterBytes = { 0xF6, 0x05, 0x0F, 0xBF, 0x4C, 0x01, 0x01 };
        internal static string GuardAnimations = "?? ?? ?? ?? ?? ?? ?? 48 8B 41 20 89 90 40 14 00 00 48 8B 49 20 8B 81 40 14";
        internal static byte[] OriginalGuardAnimationsBytes = { 0x0F, 0xBF, 0x90, 0x00, 0x0C, 0x00, 0x00, 0x48, 0x8B, 0x41, 0x20, 0x89, 0x90, 0x40, 0x14, 0x00, 0x00, 0x48, 0x8B, 0x49, 0x20, 0x8B, 0x81, 0x40, 0x14 };
        internal static byte[] StaticGuardAnimationBytes = { 0x48, 0x8B, 0x41, 0x20, 0x89, 0x90, 0x40, 0x14, 0x00, 0x00, 0x48, 0x8B, 0x49, 0x20, 0x8B, 0x81, 0x40, 0x14 };
        internal static string RemovePlantFog = "B9 1C 00 00 00 89 18 8B 96 20 05";
        internal static byte[] OriginalPlantFogBytes = { 0xB9, 0x1C, 0x00, 0x00, 0x00, 0x89, 0x18, 0x8B, 0x96, 0x20, 0x05 };
        internal static string RemoveTankerFilter = "E7 F8 FF 66 41 0F 6E 8F 2C 03";
        internal static byte[] OriginalRemoveTankerFilterBytes = { 0xE7, 0xF8, 0xFF, 0x66, 0x41, 0x0F, 0x6E, 0x8F, 0x2C, 0x03 };
        internal static string NightTime = "FF 00 00 00 4C 6B C8 70 0F B6 83 C8 01";
        internal static byte[] OriginalNightTimeBytes = { 0xFF, 0x00, 0x00, 0x00, 0x4C, 0x6B, 0xC8, 0x70, 0x0F, 0xB6, 0x83, 0xC8, 0x01 };
        internal static string EnableCustomFiltering = "89 05 F5 BE 4C 01";
        internal static byte[] OriginalCustomFilteringBytes = { 0x89, 0x05, 0xF5, 0xBE, 0x4C, 0x01 };
        internal static string CustomFilteringAoB = "00 00 80 3F 00 00 00 00 E0 0B 00 00 00 00 00 00";
        internal static string PauseVRAoB = "7E 51 FF 0D ?? ?? ?? 01";
        internal static byte[] OriginalPauseVRBytes = { 0x7E, 0x51, 0xFF, 0x0D, 0x00, 0x00, 0x00, 0x01 };
        internal static string VRObjectiveAoB = "0F 8C 9F 00 00 00 48 8B CB";
        internal static byte[] OriginalVRObjectiveBytes = { 0x0F, 0x8C, 0x9F, 0x00, 0x00, 0x00, 0x48, 0x8B, 0xCB };
        internal static string VREnemiesAoB = "7C 36 8B 83 98 00 00 00";
        internal static byte[] OriginalVREnemiesBytes = { 0x7C, 0x36, 0x8B, 0x83, 0x98, 0x00, 0x00, 0x00 };
        internal static string VRNoHitDamageAoB = "66 29 43 12 E9 67 FF FF FF";
        internal static byte[] OriginalVRNoHitDamageBytes = { 0x66, 0x29, 0x43, 0x12, 0xE9, 0x67, 0xFF, 0xFF, 0xFF };
        internal static string VRNoFallDamageAoB = "66 01 97 D2 08 00 00";
        internal static byte[] OriginalVRNoFallDamageBytes = { 0x66, 0x01, 0x97, 0xD2, 0x08, 0x00, 0x00 };
        internal static string VRInfiniteStrAoB = "66 89 BB 18 04 00 00";
        internal static byte[] OriginalVRInfiniteStrBytes = { 0x66, 0x89, 0xBB, 0x18, 0x04, 0x00, 0x00 };
        internal static string VRGripDamageAoB = "66 01 87 18 04 00 00 41";
        internal static byte[] OriginalVRGripDamageBytes = { 0x66, 0x01, 0x87, 0x18, 0x04, 0x00, 0x00, 0x41 };
        internal static string VRAimStabilityAoB = "0F 8F 90 01 00 00 48 8B";
        internal static byte[] OriginalVRAimStabilityBytes = { 0x0F, 0x8F, 0x90, 0x01, 0x00, 0x00, 0x48, 0x8B };
        internal static string VRInfiniteAmmoAoB = "66 2B CF 79 02";
        internal static byte[] OriginalVRInfiniteAmmoBytes = { 0x66, 0x2B, 0xCF, 0x79, 0x02 };
        internal static string VRInfiniteItemAoB = "66 41 89 09 0F BF C1";
        internal static byte[] OriginalVRInfiniteItemBytes = { 0x66, 0x41, 0x89, 0x09, 0x0F, 0xBF, 0xC1 };
        internal static string VRNoReloadAoB = "FF C8 89 05 ?? ?? ?? 01 C3 CC 48";
        internal static byte[] OriginalVRNoReloadBytes = { 0xFF, 0xC8, 0x89, 0x05, 0x00, 0x00, 0x00, 0x01, 0xC3, 0xCC, 0x48 };

        #region Guard animations
        internal static GuardAnimation WaitAnimation = new GuardAnimation { Name = "Wait", Bytes = new byte[] { 0xBA, 0x00, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation PatrolAnimation = new GuardAnimation { Name = "Patrol", Bytes = new byte[] { 0xBA, 0x02, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation MoveForwardAnimation = new GuardAnimation { Name = "Move Forward", Bytes = new byte[] { 0xBA, 0x03, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation YawnAnimation = new GuardAnimation { Name = "Yawn", Bytes = new byte[] { 0xBA, 0x04, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation StretchAnimation = new GuardAnimation { Name = "Stretch", Bytes = new byte[] { 0xBA, 0x05, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation DozeAnimation = new GuardAnimation { Name = "Doze", Bytes = new byte[] { 0xBA, 0x06, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation AttentionAnimation = new GuardAnimation { Name = "Attention", Bytes = new byte[] { 0xBA, 0x07, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation BoogieAnimation = new GuardAnimation { Name = "Boogie", Bytes = new byte[] { 0xBA, 0x09, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation LDOverwatchAnimation = new GuardAnimation { Name = "Long Distance Overwatch", Bytes = new byte[] { 0xBA, 0x0A, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation RemoveGogglesAnimation = new GuardAnimation { Name = "Remove Goggles", Bytes = new byte[] { 0xBA, 0x0B, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation DazedAnimation = new GuardAnimation { Name = "Dazed", Bytes = new byte[] { 0xBA, 0x0C, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation SDOverwatchAnimation = new GuardAnimation { Name = "Short Distance Overwatch", Bytes = new byte[] { 0xBA, 0x0D, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation MoveBackwardAnimation = new GuardAnimation { Name = "Move Backward", Bytes = new byte[] { 0xBA, 0x0F, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation LookMagazineAnimation = new GuardAnimation { Name = "Look at Magazine", Bytes = new byte[] { 0xBA, 0x15, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation DeactivateAIAnimation = new GuardAnimation { Name = "Deactivate AI", Bytes = new byte[] { 0xBA, 0x16, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation CommandSoldiersAnimation = new GuardAnimation { Name = "Command Soldiers", Bytes = new byte[] { 0xBA, 0x18, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation RadioCheckinAnimation = new GuardAnimation { Name = "Radio Check-in", Bytes = new byte[] { 0xBA, 0x19, 0x00, 0x00, 0x00, 0x90, 0x90 }};
        internal static GuardAnimation PeeAnimation = new GuardAnimation { Name = "Pee", Bytes = new byte[] { 0xBA, 0x1A, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation PeeWiggleAnimation = new GuardAnimation { Name = "Pee Wiggle", Bytes = new byte[] { 0xBA, 0x1B, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation LeanRightAnimation = new GuardAnimation { Name = "Lean Right", Bytes = new byte[] { 0xBA, 0x1C, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation LeanLeftAnimation = new GuardAnimation { Name = "Lean Left", Bytes = new byte[] { 0xBA, 0x1D, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation RollRightAnimation = new GuardAnimation { Name = "Roll Right", Bytes = new byte[] { 0xBA, 0x1E, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static GuardAnimation RollLeftAnimation = new GuardAnimation { Name = "Roll Left", Bytes = new byte[] { 0xBA, 0x1F, 0x00, 0x00, 0x00, 0x90, 0x90 } };
        internal static List<GuardAnimation> GuardAnimationList = new List<GuardAnimation>
        { 
            WaitAnimation, PatrolAnimation, MoveForwardAnimation, YawnAnimation, StretchAnimation, DozeAnimation, AttentionAnimation, BoogieAnimation,
            LDOverwatchAnimation, RemoveGogglesAnimation, DazedAnimation, SDOverwatchAnimation, MoveBackwardAnimation, LookMagazineAnimation,
            DeactivateAIAnimation, CommandSoldiersAnimation, RadioCheckinAnimation, PeeAnimation, PeeWiggleAnimation, LeanRightAnimation,
            LeanLeftAnimation, RollRightAnimation, RollLeftAnimation
        };

        internal class GuardAnimation
        {
            public string Name { get; set; }
            public byte[] Bytes { get; set; }

            public GuardAnimation() { }
        }
        #endregion


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
        public static readonly MemoryOffset LIFE_TEXT = new MemoryOffset(10, 13); 
        public static readonly MemoryOffset GRIP_Lv1_TEXT = new MemoryOffset(18, 25);
        public static readonly MemoryOffset GRIP_Lv2_TEXT = new MemoryOffset(-150, -143);
        public static readonly MemoryOffset GRIP_Lv3_TEXT = new MemoryOffset(-166, -159);
        #endregion

        #region Calculated From RationMedsBandagePentazeminDescriptions
        //TODO: fill these out
        #endregion

        #region Calculated From RayNames
        public static readonly MemoryOffset RAY_01 = new MemoryOffset(75, 82);
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
        public static readonly MemoryOffset SOLIDUS_HP_TEXT = new MemoryOffset(65, 71);
        #endregion

        #region Calculated From EmmaO2
        public static readonly MemoryOffset EMMA_O2_TEXT = new MemoryOffset(24, 30);
        public static readonly MemoryOffset RAIDEN_O2_TEXT = new MemoryOffset(136, 137);
        #endregion

        #region Calculated From EmmaName
        public static readonly MemoryOffset EMMA_HP_TEXT = new MemoryOffset(395, 398);
        #endregion

        #region Calculated From FatmanName
        public static readonly MemoryOffset FATMAN_HP_TEXT = new MemoryOffset(93, 98);
        #endregion

        #region Calculated From OlgaName
        public static readonly MemoryOffset OLGA_HP_TEXT = new MemoryOffset(297, 300);
                                                                                       //there is also a meryl string right next to OLGA... but idk what it is used for so i'm not bothering to add it atm
                                                                                       //guessing the meryl^^ string is related to the OLGA boss fight! OLGAMERYL
        #endregion

        #region Calculated From HarrierName
        public static readonly MemoryOffset HARRIER_HP_TEXT = new MemoryOffset(-104, -98);
        #endregion

        #region Calculated From KasatkaName
        public static readonly MemoryOffset KASATKA_HP_TEXT = new MemoryOffset(-8, -2);
        #endregion

        #region Calculated From FortuneName
        public static readonly MemoryOffset FORTUNE_HP_TEXT = new MemoryOffset(1193, 1199);
        public static readonly MemoryOffset FORTUNE_HP_VALUE = new MemoryOffset(1217, 1218);
        public static readonly MemoryOffset FORTUNE_STAMINA_VALUE = new MemoryOffset(1233, 1234);
        public static readonly MemoryOffset VAMP_HP_TEXT = new MemoryOffset(1913, 1916);
        #endregion

        #region Calculated From Vamp02
        public static readonly MemoryOffset VAMP_02_TEXT = new MemoryOffset(13, 19);
        #endregion
        #endregion

        #region Value offsets
        #region Calculated From PlayerInfo
        public static readonly MemoryOffset BASE_WEAPON = new MemoryOffset(2, 116); //if a "new" playerOffsetBytes is chosen, only need to update this value and the item offset will update.
        public static readonly MemoryOffset BASE_ITEM = new MemoryOffset(BASE_WEAPON.Start + 144, BASE_WEAPON.Start + 144 + 80);
        public static readonly MemoryOffset GRIP_LEVEL_SNAKE = new MemoryOffset(-46, -45); //will break
        public static readonly MemoryOffset GRIP_LEVEL_RAIDEN = new MemoryOffset(-44, -43); //will break
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
        public static readonly MemoryOffset FORCE_SLEEP = new MemoryOffset(0, 7);
        public static readonly MemoryOffset FORCE_WAKE = new MemoryOffset(0, 9);
        public static readonly MemoryOffset INFINITE_ITEMS = new MemoryOffset(0, 6);
        public static readonly MemoryOffset MAX_ON_PICKUP = new MemoryOffset(0, 6);
        public static readonly MemoryOffset KNOCKOUT_DURATION = new MemoryOffset(0, 7);
        public static readonly MemoryOffset REMOVE_PLANT_FILTER = new MemoryOffset(0, 9);
        public static readonly MemoryOffset GUARD_ANIMATIONS = new MemoryOffset(0, 6);
        public static readonly MemoryOffset REMOVE_PLANT_FOG = new MemoryOffset(0);
        public static readonly MemoryOffset REMOVE_TANKER_FILTER = new MemoryOffset(9);
        public static readonly MemoryOffset NIGHT_TIME = new MemoryOffset(0);
        public static readonly MemoryOffset ENABLE_CUSTOM_FILTER = new MemoryOffset(0, 18);
        public static readonly MemoryOffset CUSTOM_FILTERING = new MemoryOffset(-204, -202);
        public static readonly MemoryOffset PAUSE_VR_TIMER = new MemoryOffset(0, 7);
        public static readonly MemoryOffset VR_AUTO_COMPLETE_OBJECTIVES = new MemoryOffset(0, 7);
        public static readonly MemoryOffset VR_AUTO_COMPLETE_ENEMIES = new MemoryOffset(0, 7);
        public static readonly MemoryOffset VR_NO_HIT_DMG = new MemoryOffset(0, 8);
        public static readonly MemoryOffset VR_NO_FALL_DMG = new MemoryOffset(0, 6);
        public static readonly MemoryOffset VR_INF_STR = new MemoryOffset(0, 6);
        public static readonly MemoryOffset VR_TAKE_GRIP_DMG = new MemoryOffset(0, 7);
        public static readonly MemoryOffset VR_AIM_STAB = new MemoryOffset(0, 7);
        public static readonly MemoryOffset VR_INF_AMMO = new MemoryOffset(0, 4);
        public static readonly MemoryOffset VR_INF_ITEM = new MemoryOffset(0, 6);
        public static readonly MemoryOffset VR_NO_RELOAD = new MemoryOffset(0, 10);
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
