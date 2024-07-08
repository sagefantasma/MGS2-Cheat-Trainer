using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class ObjectIds
    {
        #region MGS2 Items
        public static readonly byte Ration = 0xC1;
        public static readonly byte Scope1 = 0xC2;
        public static readonly byte ColdMeds = 0xC3;
        public static readonly byte Bandage = 0xC4;
        public static readonly byte Pentazemin = 0xC5;
        public static readonly byte BDU = 0xC6;
        public static readonly byte BodyArmor = 0xC7;
        public static readonly byte Stealth = 0xC8;
        public static readonly byte MineDetector = 0xC9;
        public static readonly byte SensorA = 0xCA;
        public static readonly byte SensorB = 0xCB;
        public static readonly byte NVG = 0xCC;
        public static readonly byte Thermals = 0xCD;
        public static readonly byte Scope2 = 0xCE;
        public static readonly byte DigitalCamera = 0xCF;
        public static readonly byte Box1 = 0xD0;
        public static readonly byte Cigs = 0xD1;
        public static readonly byte Card = 0xD2;
        public static readonly byte Shaver = 0xD3;
        public static readonly byte Phone = 0xD4;
        public static readonly byte Camera1 = 0xD5;
        public static readonly byte Box2 = 0xD6;
        public static readonly byte Box3 = 0xD7;
        public static readonly byte WetBox = 0xD8;
        public static readonly byte APSensor = 0xD9;
        public static readonly byte Box4 = 0xDA;
        public static readonly byte Box5 = 0xDB;
        public static readonly byte Unknown = 0xDC;
        public static readonly byte SocomSup = 0xDD;
        public static readonly byte AkSupp = 0xDE;
        public static readonly byte Camera2 = 0xDF;
        public static readonly byte Bandana = 0xE0;
        public static readonly byte DogTags = 0xE1;
        public static readonly byte MoDisc = 0xE2;
        public static readonly byte UspSup = 0xE3;
        public static readonly byte InfWig = 0xE4;
        public static readonly byte BlueWig = 0xE5;
        public static readonly byte OrangeWig = 0xE6;
        public static readonly byte ColorWig1 = 0xE7;
        public static readonly byte ColorWig2 = 0xE8;
        #endregion

        #region MGS2 Weapons
        public static readonly byte M9 = 0xC1;
        public static readonly byte Usp = 0xC2;
        public static readonly byte Socom = 0xC3;
        public static readonly byte Psg1 = 0xC4;
        public static readonly byte Rgb6 = 0xC5;
        public static readonly byte Nikita = 0xC6;
        public static readonly byte Stinger = 0xC7;
        public static readonly byte Claymore = 0xC8;
        public static readonly byte C4 = 0xC9;
        public static readonly byte Chaff = 0xCA;
        public static readonly byte Stun = 0xCB;
        public static readonly byte Dmic1 = 0xCC;
        public static readonly byte HfBlade = 0xCD;
        public static readonly byte Coolant = 0xCE;
        public static readonly byte Aks74u = 0xCF;
        public static readonly byte Magazine = 0xD0;
        public static readonly byte Grenade = 0xD1;
        public static readonly byte M4 = 0xD2;
        public static readonly byte Psg1t = 0xD3;
        public static readonly byte Dmic2 = 0xD4;
        public static readonly byte Book = 0xD5;
        #endregion
    }

    internal class ProcIds
    {
        #region MGS2 procs
        #region Placed items
        public static readonly byte[] AwardRation = { 0x2E, 0xA6, 0x53 }; 
        public static readonly byte[] AwardM9Ammo = { 0xAF, 0x7E, 0x8A }; 
        public static readonly byte[] AwardBandages = { 0x22, 0xA2, 0xD2 };
        public static readonly byte[] AwardChaff = { 0x47, 0x7D, 0xF5 };
        public static readonly byte[] AwardPentazemin = { 0x5B, 0x1E, 0xF7 };
        public static readonly byte[] AwardThermalG = { 0xD6, 0x31, 0x2F };
        public static readonly byte[] AwardWetBox = { 0x1B, 0xF1, 0xCA };
        public static readonly byte[] AwardUspAmmo = { 0x9B, 0xCC, 0xAF };
        public static readonly byte[] AwardBox1 = { 0x65, 0xD6, 0x97 };
        public static readonly byte[] AwardStunG = { 0x4B, 0x6B, 0xA5 };
        public static readonly byte[] AwardGrenade = { 0x29, 0xD6, 0x37 };
        public static readonly byte[] AwardStingerAmmo = { 0xA9, 0xAD, 0x4 };
        public static readonly byte[] AwardC4 = { 0x11, 0x4D, 0x13 };
        public static readonly byte[] AwardShaver = { 0xCF, 0x6E, 0x61 };
        public static readonly byte[] AwardBook = { 0xF8, 0xBF, 0x74 };
        public static readonly byte[] AwardPsg1tAmmo = { 0x3E, 0x25, 0x7A };
        public static readonly byte[] AwardRgbAmmo = { 0x15, 0x69, 0x7F };
        public static readonly byte[] AwardAksAmmo = { 0x9B, 0xC8, 0xE7 };
        public static readonly byte[] AwardM4Ammo = { 0xAE, 0x7E, 0xEA };
        public static readonly byte[] AwardNikitaAmmo = { 0xE6, 0x47, 0xEB };
        public static readonly byte[] AwardPsg1Ammo = { 0x84, 0x6B, 0xF7 };
        public static readonly byte[] AwardSocomAmmo = { 0xA5, 0xDC, 0xFF };
        public static readonly byte[] AwardSensorB = { 0x58, 0x36, 0x29 };
        public static readonly byte[] AwardDigitalCamera = { 0x81, 0x0C, 0x64 };
        public static readonly byte[] AwardBox5 = { 0xCF, 0x97, 0xE9, 0x9 }; //check... its valid????
        public static readonly byte[] AwardMineDetector = { 0x69, 0x07, 0xC6 };
        public static readonly byte[] AwardBox3 = { 0xCF, 0x97, 0xE9, 0x9 }; //check
        public static readonly byte[] AwardClaymore = { 0x24, 0x09, 0x6 };
        public static readonly byte[] AwardBox2 = { 0xCF, 0x97, 0xE9, 0x3 };
        public static readonly byte[] AwardRgbGun = { 0x4B, 0xF6, 0xA1 };
        public static readonly byte[] AwardDirectionalMic = { 0xC3, 0x5D, 0x6F };
        public static readonly byte[] AwardBox4 = { 0xCF, 0x97, 0x7E };
        public static readonly byte[] AwardStingerGun = { 0x7C, 0xD6, 0x66 };
        public static readonly byte[] AwardNikitaGun = { 0x1D, 0xD5, 0xD };
        public static readonly byte[] AwardNvg = { 0xCF, 0x79, 0x35 };
        public static readonly byte[] AwardBodyArmor = { 0x62, 0xE5, 0xE6 };
        public static readonly byte[] AwardColdMeds = { 0x5F, 0x45, 0xAD };
        public static readonly byte[] AwardColdMeds2 = { 0xF1, 0xE8, 0x42 };
        #endregion

        #region Dropped items
        public static readonly byte[] DropRation = { 0xFF, 0x0D, 0x4 }; //4B7E2E <--- stinky ration??
        public static readonly byte[] DropDogTag = { 0xF7, 0x10, 0xDD };
        public static readonly byte[] DropBandage = { 0x46, 0xCD, 0xF3 };
        public static readonly byte[] DropStunG = { 0xD4, 0x5F, 0x20 }; 
        public static readonly byte[] DropChaffG = { 0x99, 0x7F, 0x21 };
        public static readonly byte[] DropM9 = { 0x1D, 0x9E, 0x9A }; //719612 seems to be a complicated version of this?
        public static readonly byte[] DropUsp = { 0xD4, 0x74, 0xC6 };
        public static readonly byte[] DropPentazemin = { 0xD9, 0x90, 0x3B };
        public static readonly byte[] DropGrenade = { 0xAD, 0x0D, 0x47 };
        public static readonly byte[] DropM4 = { 0x08, 0x96, 0x71 };
        public static readonly byte[] DropSocom = { 0x79, 0x75, 0xC7 };
        public static readonly byte[] DropStinger = { 0xAA, 0x85, 0x14 };
        public static readonly byte[] DropPsg1tAmmo = { 0x01, 0xFF, 0x6B };
        public static readonly byte[] DropNikitaAmmo = { 0x88, 0x29, 0x7E };

        #endregion
        #endregion
    }
}
