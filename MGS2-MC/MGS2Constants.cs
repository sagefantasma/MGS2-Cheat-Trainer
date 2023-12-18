using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal class MGS2Constants
    {
        /*useful information from @ANTIBigBoss irt GoG port CT:
         * 
         * Misc notes:
         * You  can open menus bij clicking their active boxes left to them
______________________________
Values to make some cheats work
----------------------------------------------------
Regular cheats
  - Walk through walls: 39 = on, 36=off
  - Enable Stealth: 64 = invisible :not from cameras ='(]
 
Weapon values
 - If you want to disable weapons: set value to -1 or 65535
 - this also means for items.
 - Whitesnoop ~
Alert mode = Put 1
Evasion = just lock for infinite evation
Caution = 5000 or whatever number to get Caution
------
Knockout (normal) takes 9 punches
         * 
         * !!!!!!!!!!! Unfortunately, it seems like the offsets in the GoG version are NOT translating to the MC version :( !!!!!!!
         * (Statics we can manipulate) = (type) -- (original offset in hex == original offset in decimal)
         * 
         * ------------
         * "Main" stuff
         * ------------
         * MaxLife = 2 bytes
         * AmmoInClip = 4 bytes -- 618B2C == 6392620
         * FPV State = array bytes -- 9C18C == 639372
         * FPV = byte -- 618B03 == 6392579
         * CurrentItem = 2 bytes -- D8AEC6 == 14200518
         * CurrentLevel = 7 char string -- D8ADEC == 14200300
         * CurrentWeapon = 2 bytes -- D8AEC4 == 14200516
         * Life = 2 bytes -- 618BD0 == 6392784
         * LifeText = 15 char string
         * End if found = 1 byte
         * Walk through walls = 1 byte
         * Walk through walls(soft) = 1 byte
         * Set character at load = 7 char string -- D8C374 == 14205812
         * VR end(34)?? = 4 bytes 
         * Set start point ROT = 2 bytes -- D8FE28 == 14220840
         * Set start point X = 2 bytes -- D8FE30 == 14220848
         * Set start point Y = 2 bytes -- D8FE34 == 14220852
         * Set start point Z = 2 bytes -- D8FE38 == 14220856
         * Load level = 16 char string -- D8C384 == 14205828
         * Previous level = 7 char string -- 664FC4 == 6705092
         * Difficulty = 1 byte -- D8C368 == 14205800
         * Difficulty(RO) = 1 byte -- D8ADD0 == 14200272
         * Screen visible(codec eg?) = 1 byte -- 9B7044 == 10186820
         * X(?) = float -- 6164E0 == 6382816
         * Y(?) = float -- 6164E4 == 6382820
         * Z(?) = float -- 6164E8 == 6382824
         * LocationX(?) = byte? -- 4A9910 == 4888848
         * LocationY(?) = 2 bytes -- 4A990B == 4888843
         * 
         * ----------------
         * VR mission stuff
         * ----------------
         * Bomb disposal = 2 bytes -- B60CFC == 11930876
         * Enemies = 2 bytes -- B6DE20 == 11984416
         * Score = 4 bytes -- B60C20 == 11930656
         * Targets = 2 bytes -- B60C04 == 11930628
         * Time = 2 bytes -- B60CF8 == 11930872
         * 
         * ----------------
         * Alert mode stuff
         * ----------------
         * Caution = 4 bytes -- 6160C8 == 6381768
         * Current Mode = 1 byte -- D8AEDA == 14200538
         * 
         * ------------
         * Random stuff
         * ------------
         * Character luminance = float -- 5FE990 == 6285712
         * Flip screen = float -- 5FE2F4 == 6284020
         * Hud horizontal placement = float -- 5FE610 == 6284816
         * Hud vertical placement = float -- 5FE614 == 6284820
         * Function starter? = 4 bytes -- B60A0C == 11930124
         * FOV = float -- 5FE1C8 == 6283720
         * Horizontal Stretch = float -- 5FE1B4 == 6283700
         * Vertical stretch = float -- 5FE1A0 == 6283680
         * Screen Y pos = float -- 5FD704 == 6280964
         */


        /*
         * 12/10: Interesting CE learnings -
         * 
         * Certain game stats are tracked GLOBALLY and reset on launch(kill count, shot count, holdups, choke outs, prolly more)
         * It looks like the memory accesses these counts AT LEAST once on game load, once on screen load and twice on gameplay
         */

        public const string PROCESS_NAME = "METAL GEAR SOLID2";
        //these commented offsets are old possible anchors i found that weren't consistent/usable
        //public static byte[] PlayerOffsetBytes = new byte[6] { 104, 01, 100, 00, 100, 00 }; // the CURRENT player offset will be the second to last one in memory.
        //public static IntPtr PlayerOffsetPtr = (IntPtr)0x680164006400;
        //public static byte[] PlayerOffsetBytes = new byte[] { 82, 82, 74, 41, 37, 148, 82, 74, 41, 145, 66, 170, 148, 82, 74, 41, 1, 132, 18, 80, 74, 165, 144, 145, 145, 145, 82, 162, 164, 148, 145, 82, 74, 41, 165, 148, 82, 74, 41, 73, 72, 33 };
        //public static IntPtr PlayerOffsetPtr = (IntPtr)0x52524A292594524A299142AA94524A29018412504AA59092929252A2A49492524A29A594524A29494821;
        public static byte[] OldPlayerOffsetBytes = new byte[] { 00, 00, 00, 00, 01, 00, 46, 00 };
        public static byte[] PlayerOffsetBytes = new byte[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00 };
        //00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 
        //it changes(if changing zones) THEN moves

        public const int BASE_WEAPON_OFFSET = -42; //if a "new" "anchor" is chosen, only need to update this value and all others will update.
        public const int BASE_ITEM_OFFSET = BASE_WEAPON_OFFSET + 144;
        public const int TIMES_FOUND_GAME_LAUNCH_OFFSET = 0x17B786C;
        public const int HOLD_UPS_GAME_LAUNCH_OFFSET = 0x1673DB0;
        public const int CHOKE_OUTS_GAME_LAUNCH_OFFSET = 0x1673DBC;
        public const int SHOT_COUNT_PLAYER_OFFSET = -96; //TODO: need to redetermine
        public const int HOLD_UP_PLAYER_OFFSET = 5108; //TODO: need to redetermine
        //TODO: add more of the game stats here


        //These values are PRESENTLY unknown
        //internal const int HealthPointerOffset = 0x00AE49D8; 
        //internal const int CurrentHealthOffset = 0x684;
        //internal const int MaxHealthOffset = 0x686;
        //internal const int StaminaOffset = 0xA4A;
        //internal static IntPtr HudOffset = (IntPtr)0xADB40F;
        //internal static IntPtr CamOffset = (IntPtr)0xAE3B37;
        //internal static IntPtr AlertStatusOffset = (IntPtr)0x1D9C3D8;

        #region Item Table
        public const int RationOffset = 0;
        public const int SnakeScopeOffset = 2;
        public const int ColdMedicineOffset = 4;
        public const int BandageOffset = 6;
        public const int PentazeminOffset = 8;
        public const int BDUOffset = 10;
        public const int BodyArmorOffset = 12;
        public const int StealthOffset = 14;
        public const int MineDetectorOffset = 16;
        public const int SensorAOffset = 18;
        public const int SensorBOffset = 20;
        public const int NightVisionGogglesOffset = 22;
        public const int ThermalGogglesOffset = 24;
        public const int RaidenScopeOffset = 26;
        public const int DigitalCameraOffset = 28;
        public const int Box1Offset = 30;
        public const int CigarettesOffset = 32;
        public const int CardOffset = 34;
        public const int ShaverOffset = 36;
        public const int PhoneOffset = 38;
        public const int Camera1Offset = 40;
        public const int Box2Offset = 42;
        public const int Box3Offset = 44;
        public const int WetBoxOffset = 46;
        public const int APSensorOffset = 48;
        public const int Box4Offset = 50;
        public const int Box5Offset = 52;
        public const int UnknownItemOffset = 54; //razor?
        public const int SocomSuppressorOffset = 56;
        public const int AKSuppressorOffset = 58;
        public const int Camera2Offset = 60;
        public const int BandanaOffset = 62;
        public const int DogTagsOffset = 64;
        public const int MODiscOffset = 66;
        public const int USPSuppressorOffset = 68;
        public const int InfinityWigOffset = 70;
        public const int BlueWigOffset = 72;
        public const int OrangeWigOffset = 74;
        public const int ColorWigOffset = 76;
        public const int ColorWig2Offset = 78;
        #endregion

        #region Weapon Table
        public const int M9Offset = 0;
        public const int USPOffset = 2;
        public const int SOCOMOffset = 4;
        public const int PSG1Offset = 6;
        public const int RGB6Offset = 8;
        public const int NikitaOffset = 10;
        public const int StingerOffset = 12;
        public const int ClaymoreOffset = 14;
        public const int C4Offset = 16;
        public const int ChaffGrenadeOffset = 18;
        public const int StunGrenadeOffset = 20;
        public const int DMicOffset = 22;
        public const int HighFrequencyBladeOffset = 24;
        public const int CoolantOffset = 26;
        public const int AKS74uOffset = 28;
        public const int MagazineOffset = 30;
        public const int GrenadeOffset = 32;
        public const int M4Offset = 34;
        public const int PSG1TOffset = 36;
        public const int DMic2Offset = 38;
        public const int BookOffset = 40;
        #endregion

        public static List<MGS2Object> SnakeUsableObjects = new List<MGS2Object>
        {
            MGS2UsableObjects.AKSuppressor,
            MGS2UsableObjects.APSensor,
            MGS2UsableObjects.Bandage,
            MGS2UsableObjects.Bandana,
            MGS2UsableObjects.Box1,
            MGS2UsableObjects.Box2, //TODO: verify if this is true, and if there are more
            MGS2UsableObjects.Camera1,
            MGS2UsableObjects.Camera2,
            MGS2UsableObjects.Card,
            MGS2UsableObjects.ChaffGrenade,
            MGS2UsableObjects.Cigarettes,
            MGS2UsableObjects.ColdMedicine,
            MGS2UsableObjects.DigitalCamera, //TODO: verify
            MGS2UsableObjects.DogTags,
            MGS2UsableObjects.Grenade,
            MGS2UsableObjects.Magazine,
            MGS2UsableObjects.MineDetector,
            MGS2UsableObjects.MODisc,
            MGS2UsableObjects.M9,
            MGS2UsableObjects.Pentazemin,
            MGS2UsableObjects.Phone,
            MGS2UsableObjects.RaidenScope,
            MGS2UsableObjects.Ration,
            MGS2UsableObjects.SensorA,
            MGS2UsableObjects.SensorB,
            MGS2UsableObjects.Shaver,
            MGS2UsableObjects.SnakeScope,
            MGS2UsableObjects.SocomSuppressor,
            MGS2UsableObjects.Stealth,
            MGS2UsableObjects.StunGrenade,
            MGS2UsableObjects.ThermalGoggles,
            MGS2UsableObjects.USP,
            MGS2UsableObjects.USPSuppressor,
            MGS2UsableObjects.WetBox
        };

        public static List<MGS2Object> RaidenUsableObjects = new List<MGS2Object>
        {
            MGS2UsableObjects.AKS74u,
            MGS2UsableObjects.AKSuppressor,
            MGS2UsableObjects.APSensor,
            MGS2UsableObjects.Bandage,
            MGS2UsableObjects.BDU,
            MGS2UsableObjects.BlueWig,
            MGS2UsableObjects.BodyArmor,
            MGS2UsableObjects.Book,
            MGS2UsableObjects.Box1,
            MGS2UsableObjects.Box2,
            MGS2UsableObjects.Box3,
            MGS2UsableObjects.Box4,
            MGS2UsableObjects.Box5,
            MGS2UsableObjects.C4,
            MGS2UsableObjects.Camera1,
            MGS2UsableObjects.Camera2,
            MGS2UsableObjects.Card,
            MGS2UsableObjects.ChaffGrenade,
            MGS2UsableObjects.Cigarettes,
            MGS2UsableObjects.Claymore,
            MGS2UsableObjects.ColdMedicine,
            MGS2UsableObjects.ColorWig,
            MGS2UsableObjects.ColorWig2,
            MGS2UsableObjects.Coolant,
            MGS2UsableObjects.DigitalCamera,
            MGS2UsableObjects.DMic1,
            MGS2UsableObjects.DMic2,
            MGS2UsableObjects.DogTags,
            MGS2UsableObjects.Grenade,
            MGS2UsableObjects.HighFrequencyBlade,
            MGS2UsableObjects.InfinityWig,
            MGS2UsableObjects.M4,
            MGS2UsableObjects.M9,
            MGS2UsableObjects.Magazine,
            MGS2UsableObjects.MineDetector,
            MGS2UsableObjects.MODisc,
            MGS2UsableObjects.NightVisionGoggles,
            MGS2UsableObjects.Nikita,
            MGS2UsableObjects.OrangeWig,
            MGS2UsableObjects.Pentazemin,
            MGS2UsableObjects.Phone,
            MGS2UsableObjects.PSG1,
            MGS2UsableObjects.PSG1T,
            MGS2UsableObjects.RaidenScope,
            MGS2UsableObjects.Ration,
            MGS2UsableObjects.RGB6,
            MGS2UsableObjects.SensorA,
            MGS2UsableObjects.SensorB,
            MGS2UsableObjects.Shaver,
            MGS2UsableObjects.SnakeScope,
            MGS2UsableObjects.SOCOM,
            MGS2UsableObjects.SocomSuppressor,
            MGS2UsableObjects.Stealth,
            MGS2UsableObjects.StunGrenade,
            MGS2UsableObjects.ThermalGoggles,
            MGS2UsableObjects.USPSuppressor,
            MGS2UsableObjects.WetBox
        };
    };
    }
}
