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
        //20 natively spawnable items
        public static readonly byte Ration = 0xC1; //can be spawned
        public static readonly byte Scope1 = 0xC2;
        public static readonly byte ColdMeds = 0xC3; //can be spawned
        public static readonly byte Bandage = 0xC4; //can be spawned
        public static readonly byte Pentazemin = 0xC5; //can be spawned
        public static readonly byte BDU = 0xC6; 
        public static readonly byte BodyArmor = 0xC7; //can be spawned
        public static readonly byte Stealth = 0xC8;
        public static readonly byte MineDetector = 0xC9; //can be spawned
        public static readonly byte SensorA = 0xCA;
        public static readonly byte SensorB = 0xCB; //can be spawned
        public static readonly byte NVG = 0xCC; //can be spawned
        public static readonly byte Thermals = 0xCD; //can be spawned
        public static readonly byte Scope2 = 0xCE;
        public static readonly byte DigitalCamera = 0xCF; //can be spawned
        public static readonly byte Box1 = 0xD0; //can be spawned
        public static readonly byte Cigs = 0xD1;
        public static readonly byte Card = 0xD2;
        public static readonly byte Shaver = 0xD3; //can be spawned
        public static readonly byte Phone = 0xD4;
        public static readonly byte Camera1 = 0xD5;
        public static readonly byte Box2 = 0xD6; //can be spawned
        public static readonly byte Box3 = 0xD7; //can be spawned
        public static readonly byte WetBox = 0xD8; //can be spawned
        public static readonly byte APSensor = 0xD9;
        public static readonly byte Box4 = 0xDA; //can be spawned
        public static readonly byte Box5 = 0xDB; //can be spawned
        public static readonly byte Unknown = 0xDC;
        public static readonly byte SocomSupp = 0xDD; //can be spawned
        public static readonly byte AkSupp = 0xDE; //can be spawned
        public static readonly byte Camera2 = 0xDF;
        public static readonly byte Bandana = 0xE0;
        public static readonly byte DogTags = 0xE1; //can be spawned
        public static readonly byte MoDisc = 0xE2;
        public static readonly byte UspSupp = 0xE3; //can be spawned
        public static readonly byte InfWig = 0xE4;
        public static readonly byte BlueWig = 0xE5;
        public static readonly byte OrangeWig = 0xE6;
        public static readonly byte ColorWig1 = 0xE7;
        public static readonly byte ColorWig2 = 0xE8;
        #endregion

        #region MGS2 Weapons
        //17 natively spawnable ammo boxes
        //8 natively spawnable guns
        public static readonly byte M9 = 0xC1; //can be spawned(gun & ammo)
        public static readonly byte Usp = 0xC2; //can be spawned(ammo only)
        public static readonly byte Socom = 0xC3; //can be spawned(ammo only)
        public static readonly byte Psg1 = 0xC4; //can be spawned(gun & ammo)
        public static readonly byte Rgb6 = 0xC5; //can be spawned(gun & ammo)
        public static readonly byte Nikita = 0xC6; //can be spawned(gun & ammo)
        public static readonly byte Stinger = 0xC7; //can be spawned(gun & ammo)
        public static readonly byte Claymore = 0xC8; //can be spawned
        public static readonly byte C4 = 0xC9; //can be spawned
        public static readonly byte Chaff = 0xCA; //can be spawned
        public static readonly byte Stun = 0xCB; //can be spawned
        public static readonly byte Dmic1 = 0xCC; //can be spawned
        public static readonly byte HfBlade = 0xCD;
        public static readonly byte Coolant = 0xCE;
        public static readonly byte Aks74u = 0xCF; //can be spawned(gun & ammo)
        public static readonly byte Magazine = 0xD0;
        public static readonly byte Grenade = 0xD1; //can be spawned
        public static readonly byte M4 = 0xD2; //can be spawned(gun & ammo)
        public static readonly byte Psg1t = 0xD3; //can be spawned(gun & ammo)
        public static readonly byte Dmic2 = 0xD4;
        public static readonly byte Book = 0xD5; //can be spawned
        #endregion
    }

    internal class Procedure
    {
        public string BigEndianRepresentation;
        public byte[] LittleEndianRepresentation;
    }

    internal class ProcIds
    {
        
        #region MGS2 procs
        #region Placed items
        #region Items
        public static readonly Procedure AwardAksSuppressor = new Procedure { BigEndianRepresentation = "A10594", LittleEndianRepresentation = new byte[] { 0x94, 0x05, 0xA1 } };
        public static readonly Procedure AwardBandages = new Procedure { BigEndianRepresentation = "D2A222", LittleEndianRepresentation = new byte[] { 0x22, 0xA2, 0xD2 } };
        public static readonly Procedure AwardBodyArmor = new Procedure { BigEndianRepresentation = "E6E562", LittleEndianRepresentation = new byte[] { 0x62, 0xE5, 0xE6 } };
        public static readonly Procedure AwardBox1 = new Procedure { BigEndianRepresentation = "97D665", LittleEndianRepresentation = new byte[] { 0x65, 0xD6, 0x97 } };
        public static readonly Procedure AwardBox2 = new Procedure { BigEndianRepresentation = "3E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x3 } };
        public static readonly Procedure AwardBox3 = new Procedure { BigEndianRepresentation = "9E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x9 } }; //check
        public static readonly Procedure AwardBox4 = new Procedure { BigEndianRepresentation = "7E97CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x7E } };
        public static readonly Procedure AwardBox5 = new Procedure { BigEndianRepresentation = "9E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x9 } }; //check... its valid????
        public static readonly Procedure AwardWetBox = new Procedure { BigEndianRepresentation = "CAF11B", LittleEndianRepresentation = new byte[] { 0x1B, 0xF1, 0xCA } };
        public static readonly Procedure AwardColdMeds = new Procedure { BigEndianRepresentation = "AD455F", LittleEndianRepresentation = new byte[] { 0x5F, 0x45, 0xAD } };
        public static readonly Procedure AwardColdMeds2 = new Procedure { BigEndianRepresentation = "42E8F1", LittleEndianRepresentation = new byte[] { 0xF1, 0xE8, 0x42 } };
        public static readonly Procedure AwardDigitalCamera = new Procedure { BigEndianRepresentation = "640C81", LittleEndianRepresentation = new byte[] { 0x81, 0x0C, 0x64 } };
        public static readonly Procedure AwardMineDetector = new Procedure { BigEndianRepresentation = "C60769", LittleEndianRepresentation = new byte[] { 0x69, 0x07, 0xC6 } };
        public static readonly Procedure AwardNvg = new Procedure { BigEndianRepresentation = "3579CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x79, 0x35 } };
        public static readonly Procedure AwardPentazemin = new Procedure { BigEndianRepresentation = "F71E5B", LittleEndianRepresentation = new byte[] { 0x5B, 0x1E, 0xF7 } };
        public static readonly Procedure AwardRation = new Procedure { BigEndianRepresentation = "53A62E", LittleEndianRepresentation = new byte[] { 0x2E, 0xA6, 0x53 } };
        public static readonly Procedure AwardSensorB = new Procedure { BigEndianRepresentation = "293658", LittleEndianRepresentation = new byte[] { 0x58, 0x36, 0x29 } };
        public static readonly Procedure AwardShaver = new Procedure { BigEndianRepresentation = "616ECF", LittleEndianRepresentation = new byte[] { 0xCF, 0x6E, 0x61 } };
        public static readonly Procedure AwardSocomSuppressor = new Procedure { BigEndianRepresentation = "96A6A", LittleEndianRepresentation = new byte[] { 0x6A, 0x6A, 0x9 } };
        public static readonly Procedure AwardThermalG = new Procedure { BigEndianRepresentation = "2F31D6", LittleEndianRepresentation = new byte[] { 0xD6, 0x31, 0x2F } };
        //missing USP suppressor?
        #endregion
        #region Weapons
        public static readonly Procedure AwardAksAmmo = new Procedure { BigEndianRepresentation = "E7C89B", LittleEndianRepresentation = new byte[] { 0x9B, 0xC8, 0xE7 } };
        public static readonly Procedure AwardAksGun = new Procedure { BigEndianRepresentation = "35513F", LittleEndianRepresentation = new byte[] { 0x3F, 0x51, 0x35 } };
        public static readonly Procedure AwardBook = new Procedure { BigEndianRepresentation = "74BFF8", LittleEndianRepresentation = new byte[] { 0xF8, 0xBF, 0x74 } };
        public static readonly Procedure AwardC4 = new Procedure { BigEndianRepresentation = "134D11", LittleEndianRepresentation = new byte[] { 0x11, 0x4D, 0x13 } };
        public static readonly Procedure AwardChaffG = new Procedure { BigEndianRepresentation = "F57D47", LittleEndianRepresentation = new byte[] { 0x47, 0x7D, 0xF5 } };
        public static readonly Procedure AwardClaymore = new Procedure { BigEndianRepresentation = "60924", LittleEndianRepresentation = new byte[] { 0x24, 0x09, 0x6 } };
        public static readonly Procedure AwardDirectionalMic = new Procedure { BigEndianRepresentation = "6F5DC3", LittleEndianRepresentation = new byte[] { 0xC3, 0x5D, 0x6F } };
        public static readonly Procedure AwardGrenade = new Procedure { BigEndianRepresentation = "37D629", LittleEndianRepresentation = new byte[] { 0x29, 0xD6, 0x37 } };
        public static readonly Procedure AwardM4Ammo = new Procedure { BigEndianRepresentation = "EA7EAE", LittleEndianRepresentation = new byte[] { 0xAE, 0x7E, 0xEA } };
        public static readonly Procedure AwardM4Gun = new Procedure { BigEndianRepresentation = "135211", LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0x13 } };
        public static readonly Procedure AwardM9Ammo =         new Procedure { BigEndianRepresentation = "8A7EAF", LittleEndianRepresentation = new byte[] { 0xAF, 0x7E, 0x8A } };
        public static readonly Procedure AwardM9Gun =          new Procedure { BigEndianRepresentation = "B35211", LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0xB3 } };
        public static readonly Procedure AwardNikitaAmmo = new Procedure { BigEndianRepresentation = "EB47E6", LittleEndianRepresentation = new byte[] { 0xE6, 0x47, 0xEB } };
        public static readonly Procedure AwardNikitaGun = new Procedure { BigEndianRepresentation = "DD51D", LittleEndianRepresentation = new byte[] { 0x1D, 0xD5, 0xD } };
        public static readonly Procedure AwardPsg1Ammo = new Procedure { BigEndianRepresentation = "F76B84", LittleEndianRepresentation = new byte[] { 0x84, 0x6B, 0xF7 } };
        public static readonly Procedure AwardPsg1Gun = new Procedure { BigEndianRepresentation = "19F8BB", LittleEndianRepresentation = new byte[] { 0xBB, 0xF8, 0x19 } };
        public static readonly Procedure AwardPsg1tAmmo = new Procedure { BigEndianRepresentation = "7A253E", LittleEndianRepresentation = new byte[] { 0x3E, 0x25, 0x7A } };
        public static readonly Procedure AwardPsg1tGun = new Procedure { BigEndianRepresentation = "DC4E11", LittleEndianRepresentation = new byte[] { 0x11, 0x4E, 0xDC } };
        public static readonly Procedure AwardRgbAmmo = new Procedure { BigEndianRepresentation = "7F6915", LittleEndianRepresentation = new byte[] { 0x15, 0x69, 0x7F } };
        public static readonly Procedure AwardRgbGun = new Procedure { BigEndianRepresentation = "A1F64B", LittleEndianRepresentation = new byte[] { 0x4B, 0xF6, 0xA1 } };
        public static readonly Procedure AwardSocomAmmo = new Procedure { BigEndianRepresentation = "FFDCA5", LittleEndianRepresentation = new byte[] { 0xA5, 0xDC, 0xFF } };
        public static readonly Procedure AwardStingerAmmo = new Procedure { BigEndianRepresentation = "4ADA9", LittleEndianRepresentation = new byte[] { 0xA9, 0xAD, 0x4 } };
        public static readonly Procedure AwardStingerGun = new Procedure { BigEndianRepresentation = "66D67C", LittleEndianRepresentation = new byte[] { 0x7C, 0xD6, 0x66 } };
        public static readonly Procedure AwardStunG = new Procedure { BigEndianRepresentation = "A56B4B", LittleEndianRepresentation = new byte[] { 0x4B, 0x6B, 0xA5 } };
        public static readonly Procedure AwardUspAmmo =        new Procedure { BigEndianRepresentation = "AFCC9B", LittleEndianRepresentation = new byte[] { 0x9B, 0xCC, 0xAF } };
        #endregion

        #region Dropped items
        public static readonly Procedure DropRation = new Procedure { BigEndianRepresentation = "40DFF", LittleEndianRepresentation = new byte[] { 0xFF, 0x0D, 0x4 } }; //4B7E2E <--- stinky ration??
        public static readonly Procedure DropDogTag = new Procedure { BigEndianRepresentation = "DD10F7", LittleEndianRepresentation = new byte[] { 0xF7, 0x10, 0xDD } };
        public static readonly Procedure DropBandage = new Procedure { BigEndianRepresentation = "F3CD46", LittleEndianRepresentation = new byte[] { 0x46, 0xCD, 0xF3 } };
        public static readonly Procedure DropStunG = new Procedure { BigEndianRepresentation = "205FD4", LittleEndianRepresentation = new byte[] { 0xD4, 0x5F, 0x20 } }; 
        public static readonly Procedure DropChaffG = new Procedure { BigEndianRepresentation = "217F99", LittleEndianRepresentation = new byte[] { 0x99, 0x7F, 0x21 } };
        public static readonly Procedure DropM9 = new Procedure { BigEndianRepresentation = "9A9E1D", LittleEndianRepresentation = new byte[] { 0x1D, 0x9E, 0x9A } }; //719612 seems to be a complicated version of this?
        public static readonly Procedure DropUsp = new Procedure { BigEndianRepresentation = "C674D4", LittleEndianRepresentation = new byte[] { 0xD4, 0x74, 0xC6 } };
        public static readonly Procedure DropPentazemin = new Procedure { BigEndianRepresentation = "3B90D9", LittleEndianRepresentation = new byte[] { 0xD9, 0x90, 0x3B } };
        public static readonly Procedure DropGrenade = new Procedure { BigEndianRepresentation = "470DAD", LittleEndianRepresentation = new byte[] { 0xAD, 0x0D, 0x47 } };
        public static readonly Procedure DropM4 = new Procedure { BigEndianRepresentation = "719608", LittleEndianRepresentation = new byte[] { 0x08, 0x96, 0x71 } };
        public static readonly Procedure DropSocom = new Procedure { BigEndianRepresentation = "C77579", LittleEndianRepresentation = new byte[] { 0x79, 0x75, 0xC7 } };
        public static readonly Procedure DropStinger = new Procedure { BigEndianRepresentation = "1485AA", LittleEndianRepresentation = new byte[] { 0xAA, 0x85, 0x14 } };
        public static readonly Procedure DropPsg1tAmmo = new Procedure { BigEndianRepresentation = "6BFF01", LittleEndianRepresentation = new byte[] { 0x01, 0xFF, 0x6B } };
        public static readonly Procedure DropNikitaAmmo = new Procedure { BigEndianRepresentation = "7E2988", LittleEndianRepresentation = new byte[] { 0x88, 0x29, 0x7E } };

        #endregion
        #endregion
        #endregion

        public static readonly List<Procedure> SpawnProcs = new List<Procedure>
        {
            AwardAksAmmo, AwardAksGun, AwardAksSuppressor, AwardBandages, AwardBodyArmor,
            AwardBook, AwardBox1, AwardBox2, AwardBox3, AwardBox4, AwardBox5, AwardWetBox,
            AwardC4, AwardChaffG, AwardClaymore, AwardColdMeds, AwardColdMeds2, AwardDigitalCamera,
            AwardDirectionalMic, AwardGrenade, AwardM4Ammo, AwardM4Gun, AwardMineDetector,
            AwardNvg, AwardM9Ammo, AwardM9Gun, AwardNikitaAmmo, AwardNikitaGun, AwardPsg1Ammo,
            AwardPsg1Gun, AwardPsg1tAmmo, AwardPsg1tGun, AwardRgbAmmo, AwardRgbGun, AwardSocomAmmo,
            AwardSocomSuppressor, AwardStingerAmmo, AwardStingerGun, AwardUspAmmo, AwardPentazemin,
            AwardRation, AwardSensorB, AwardShaver, AwardSocomSuppressor, AwardThermalG
        };
    }
}
