using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public class DecodedProc
    {
        public string Name { get; private set; }
        public uint Order { get; private set; }
        public string DecodedContents { get; private set; }
        public byte[] RawContents { get; set; }

        public readonly int ProcTablePosition;
        public readonly int ScriptLength;
        public int ScriptInitialPosition;

        public DecodedProc(string name, uint order, byte[] raw, string decoded, int procTablePosition, int scriptPosition)
        {
            Name = name;
            Order = order;
            DecodedContents = decoded;
            RawContents = raw;
            ScriptLength = raw.Length;
            ScriptInitialPosition = scriptPosition;
            ProcTablePosition = procTablePosition;
        }
    }

    public class RawProc
    {
        public string BigEndianRepresentation;
        public byte[] LittleEndianRepresentation;
    }

    internal class KnownProc
    {
        
        #region MGS2 procs
        #region Placed objects
        #region Items
        //box 2, 3 and 5 are fucking things up because they're 4 bytes long instead of just 3.
        //for now, i'm just going to leave these out of logic, but we should find a way of bringing them back later
        public static readonly RawProc AwardAksSuppressor = new RawProc { BigEndianRepresentation = "A10594", LittleEndianRepresentation = new byte[] { 0x94, 0x05, 0xA1 } };
        public static readonly RawProc AwardBandages = new RawProc { BigEndianRepresentation = "D2A222", LittleEndianRepresentation = new byte[] { 0x22, 0xA2, 0xD2 } };
        public static readonly RawProc AwardBodyArmor = new RawProc { BigEndianRepresentation = "E6E562", LittleEndianRepresentation = new byte[] { 0x62, 0xE5, 0xE6 } };
        public static readonly RawProc AwardBox1 = new RawProc { BigEndianRepresentation = "97D665", LittleEndianRepresentation = new byte[] { 0x65, 0xD6, 0x97 } };
        public static readonly RawProc AwardBox2 = new RawProc { BigEndianRepresentation = "3E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x3 } }; //none of these seem to exist in the list of procs?
        public static readonly RawProc AwardBox3 = new RawProc { BigEndianRepresentation = "9E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x9 } }; //none of these seem to exist in the list of procs?
        public static readonly RawProc AwardBox4 = new RawProc { BigEndianRepresentation = "7E97CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x7E } };
        public static readonly RawProc AwardBox5 = new RawProc { BigEndianRepresentation = "9E997CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0xE9, 0x9 } }; //none of these seem to exist in the list of procs?
        public static readonly RawProc AwardWetBox = new RawProc { BigEndianRepresentation = "CAF11B", LittleEndianRepresentation = new byte[] { 0x1B, 0xF1, 0xCA } };
        public static readonly RawProc AwardColdMeds = new RawProc { BigEndianRepresentation = "AD455F", LittleEndianRepresentation = new byte[] { 0x5F, 0x45, 0xAD } };
        public static readonly RawProc AwardColdMeds2 = new RawProc { BigEndianRepresentation = "42E8F1", LittleEndianRepresentation = new byte[] { 0xF1, 0xE8, 0x42 } };
        public static readonly RawProc AwardDigitalCamera = new RawProc { BigEndianRepresentation = "640C81", LittleEndianRepresentation = new byte[] { 0x81, 0x0C, 0x64 } };
        public static readonly RawProc AwardMineDetector = new RawProc { BigEndianRepresentation = "C60769", LittleEndianRepresentation = new byte[] { 0x69, 0x07, 0xC6 } };
        public static readonly RawProc AwardNvg = new RawProc { BigEndianRepresentation = "3579CF", LittleEndianRepresentation = new byte[] { 0xCF, 0x79, 0x35 } };
        public static readonly RawProc AwardPentazemin = new RawProc { BigEndianRepresentation = "F71E5B", LittleEndianRepresentation = new byte[] { 0x5B, 0x1E, 0xF7 } };
        public static readonly RawProc AwardRation = new RawProc { BigEndianRepresentation = "53A62E", LittleEndianRepresentation = new byte[] { 0x2E, 0xA6, 0x53 } };
        public static readonly RawProc AwardSensorB = new RawProc { BigEndianRepresentation = "293658", LittleEndianRepresentation = new byte[] { 0x58, 0x36, 0x29 } };
        public static readonly RawProc AwardShaver = new RawProc { BigEndianRepresentation = "616ECF", LittleEndianRepresentation = new byte[] { 0xCF, 0x6E, 0x61 } };
        public static readonly RawProc AwardSocomSuppressor = new RawProc { BigEndianRepresentation = "96A6A", LittleEndianRepresentation = new byte[] { 0x6A, 0x6A, 0x9 } };
        public static readonly RawProc AwardThermalG = new RawProc { BigEndianRepresentation = "2F31D6", LittleEndianRepresentation = new byte[] { 0xD6, 0x31, 0x2F } };
        public static readonly RawProc AwardUspSuppressor = new RawProc { BigEndianRepresentation = "C92A40", LittleEndianRepresentation = new byte[] { 0x40, 0x2A, 0xC9 } };
        #endregion
        #region Weapons
        public static readonly RawProc AwardAksAmmo = new RawProc { BigEndianRepresentation = "E7C89B", LittleEndianRepresentation = new byte[] { 0x9B, 0xC8, 0xE7 } };
        public static readonly RawProc AwardAksGun = new RawProc { BigEndianRepresentation = "35513F", LittleEndianRepresentation = new byte[] { 0x3F, 0x51, 0x35 } };
        public static readonly RawProc AwardBook = new RawProc { BigEndianRepresentation = "74BFF8", LittleEndianRepresentation = new byte[] { 0xF8, 0xBF, 0x74 } };
        public static readonly RawProc AwardC4 = new RawProc { BigEndianRepresentation = "134D11", LittleEndianRepresentation = new byte[] { 0x11, 0x4D, 0x13 } };
        public static readonly RawProc AwardChaffG = new RawProc { BigEndianRepresentation = "F57D47", LittleEndianRepresentation = new byte[] { 0x47, 0x7D, 0xF5 } };
        public static readonly RawProc AwardClaymore = new RawProc { BigEndianRepresentation = "60924", LittleEndianRepresentation = new byte[] { 0x24, 0x09, 0x6 } };
        public static readonly RawProc AwardDirectionalMic = new RawProc { BigEndianRepresentation = "6F5DC3", LittleEndianRepresentation = new byte[] { 0xC3, 0x5D, 0x6F } };
        public static readonly RawProc AwardGrenade = new RawProc { BigEndianRepresentation = "37D629", LittleEndianRepresentation = new byte[] { 0x29, 0xD6, 0x37 } };
        public static readonly RawProc AwardM4Ammo = new RawProc { BigEndianRepresentation = "EA7EAE", LittleEndianRepresentation = new byte[] { 0xAE, 0x7E, 0xEA } };
        public static readonly RawProc AwardM4Gun = new RawProc { BigEndianRepresentation = "135211", LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0x13 } };
        public static readonly RawProc AwardM9Ammo =         new RawProc { BigEndianRepresentation = "8A7EAF", LittleEndianRepresentation = new byte[] { 0xAF, 0x7E, 0x8A } };
        public static readonly RawProc AwardM9Gun =          new RawProc { BigEndianRepresentation = "B35211", LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0xB3 } };
        public static readonly RawProc AwardNikitaAmmo = new RawProc { BigEndianRepresentation = "EB47E6", LittleEndianRepresentation = new byte[] { 0xE6, 0x47, 0xEB } };
        public static readonly RawProc AwardNikitaGun = new RawProc { BigEndianRepresentation = "DD51D", LittleEndianRepresentation = new byte[] { 0x1D, 0xD5, 0xD } };
        public static readonly RawProc AwardPsg1Ammo = new RawProc { BigEndianRepresentation = "F76B84", LittleEndianRepresentation = new byte[] { 0x84, 0x6B, 0xF7 } };
        public static readonly RawProc AwardPsg1Gun = new RawProc { BigEndianRepresentation = "19F8BB", LittleEndianRepresentation = new byte[] { 0xBB, 0xF8, 0x19 } };
        public static readonly RawProc AwardPsg1tAmmo = new RawProc { BigEndianRepresentation = "7A253E", LittleEndianRepresentation = new byte[] { 0x3E, 0x25, 0x7A } };
        public static readonly RawProc AwardPsg1tGun = new RawProc { BigEndianRepresentation = "DC4E11", LittleEndianRepresentation = new byte[] { 0x11, 0x4E, 0xDC } };
        public static readonly RawProc AwardRgbAmmo = new RawProc { BigEndianRepresentation = "7F6915", LittleEndianRepresentation = new byte[] { 0x15, 0x69, 0x7F } };
        public static readonly RawProc AwardRgbGun = new RawProc { BigEndianRepresentation = "A1F64B", LittleEndianRepresentation = new byte[] { 0x4B, 0xF6, 0xA1 } };
        public static readonly RawProc AwardSocomAmmo = new RawProc { BigEndianRepresentation = "FFDCA5", LittleEndianRepresentation = new byte[] { 0xA5, 0xDC, 0xFF } };
        public static readonly RawProc AwardStingerAmmo = new RawProc { BigEndianRepresentation = "4ADA9", LittleEndianRepresentation = new byte[] { 0xA9, 0xAD, 0x4 } };
        public static readonly RawProc AwardStingerGun = new RawProc { BigEndianRepresentation = "66D67C", LittleEndianRepresentation = new byte[] { 0x7C, 0xD6, 0x66 } };
        public static readonly RawProc AwardStunG = new RawProc { BigEndianRepresentation = "A56B4B", LittleEndianRepresentation = new byte[] { 0x4B, 0x6B, 0xA5 } };
        public static readonly RawProc AwardUspAmmo =        new RawProc { BigEndianRepresentation = "AFCC9B", LittleEndianRepresentation = new byte[] { 0x9B, 0xCC, 0xAF } };
        #endregion
        #endregion

        #region Dropped objects
        public static readonly RawProc DropRation = new RawProc { BigEndianRepresentation = "40DFF", LittleEndianRepresentation = new byte[] { 0xFF, 0x0D, 0x4 } }; //4B7E2E <--- stinky ration??
        public static readonly RawProc DropDogTag = new RawProc { BigEndianRepresentation = "DD10F7", LittleEndianRepresentation = new byte[] { 0xF7, 0x10, 0xDD } };
        public static readonly RawProc DropBandage = new RawProc { BigEndianRepresentation = "F3CD46", LittleEndianRepresentation = new byte[] { 0x46, 0xCD, 0xF3 } };
        public static readonly RawProc DropStunG = new RawProc { BigEndianRepresentation = "205FD4", LittleEndianRepresentation = new byte[] { 0xD4, 0x5F, 0x20 } }; 
        public static readonly RawProc DropChaffG = new RawProc { BigEndianRepresentation = "217F99", LittleEndianRepresentation = new byte[] { 0x99, 0x7F, 0x21 } };
        public static readonly RawProc DropM9 = new RawProc { BigEndianRepresentation = "9A9E1D", LittleEndianRepresentation = new byte[] { 0x1D, 0x9E, 0x9A } }; //719612 seems to be a complicated version of this?
        public static readonly RawProc DropUsp = new RawProc { BigEndianRepresentation = "C674D4", LittleEndianRepresentation = new byte[] { 0xD4, 0x74, 0xC6 } };
        public static readonly RawProc DropPentazemin = new RawProc { BigEndianRepresentation = "3B90D9", LittleEndianRepresentation = new byte[] { 0xD9, 0x90, 0x3B } };
        public static readonly RawProc DropGrenade = new RawProc { BigEndianRepresentation = "470DAD", LittleEndianRepresentation = new byte[] { 0xAD, 0x0D, 0x47 } };
        public static readonly RawProc DropM4 = new RawProc { BigEndianRepresentation = "719608", LittleEndianRepresentation = new byte[] { 0x08, 0x96, 0x71 } };
        public static readonly RawProc DropSocom = new RawProc { BigEndianRepresentation = "C77579", LittleEndianRepresentation = new byte[] { 0x79, 0x75, 0xC7 } };
        public static readonly RawProc DropStinger = new RawProc { BigEndianRepresentation = "1485AA", LittleEndianRepresentation = new byte[] { 0xAA, 0x85, 0x14 } };
        public static readonly RawProc DropPsg1tAmmo = new RawProc { BigEndianRepresentation = "6BFF01", LittleEndianRepresentation = new byte[] { 0x01, 0xFF, 0x6B } };
        public static readonly RawProc DropNikitaAmmo = new RawProc { BigEndianRepresentation = "7E2988", LittleEndianRepresentation = new byte[] { 0x88, 0x29, 0x7E } };

        #endregion
        #endregion

        public static readonly List<RawProc> SpawnProcs = new List<RawProc>
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
