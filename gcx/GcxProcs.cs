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
        public string BigEndianRepresentation { get; set; }
        public byte[] LittleEndianRepresentation { get; set; }
        public string CommonName { get; set; }
        public RawProc[] ProcDependencies { get; set; }
    }

    internal class KnownProc
    {

        #region MGS2 procs
        #region Proc Dependencies
        public static readonly RawProc InventoryCheck = new RawProc
        {
            CommonName = "TankerLogicCheck",
            BigEndianRepresentation = "D5B259",
            LittleEndianRepresentation = new byte[] { 0x59, 0xB2, 0xD5 },
            ProcDependencies = null
        };

        public static readonly RawProc SomeBooleanResponse = new RawProc
        {
            CommonName = "SomeBooleanResponse",
            BigEndianRepresentation = "B35BC1",
            LittleEndianRepresentation = new byte[] { 0xC1, 0x5B, 0xB3 },
            ProcDependencies = null
        };

        public static readonly RawProc Necessity = new RawProc
        {
            CommonName = "CodeNecessity",
            BigEndianRepresentation = "DC748A",
            LittleEndianRepresentation = new byte[] { 0x8A, 0x74, 0xDC },
            ProcDependencies = new RawProc[] { SomeBooleanResponse }
        };

        public static readonly RawProc GunCheck = new RawProc
        {
            CommonName = "GunCheck",
            BigEndianRepresentation = "57208",
            LittleEndianRepresentation = new byte[] { 0x08, 0x72, 0x05 },
            ProcDependencies = new RawProc[] { AwardM9Ammo, AwardUspAmmo, AwardSocomAmmo, AwardM4Ammo,
            AwardPsg1Ammo, AwardPsg1tAmmo, AwardRgbAmmo, AwardNikitaAmmo, AwardStingerAmmo, AwardAksAmmo,
            AwardC4, AwardChaffG, AwardStunG, AwardGrenade, AwardBook, AwardRation, AwardBandages, AwardPentazemin}
        };

        public static readonly RawProc UnknownCheck = new RawProc
        {
            CommonName = "Unknown",
            BigEndianRepresentation = "261E2F",
            LittleEndianRepresentation = new byte[] { 0x2F, 0x1E, 0x26 },
            ProcDependencies = new RawProc[] { SomeBooleanResponse }
        };

        public static readonly RawProc UnknownCheck2 = new RawProc
        {
            CommonName = "Unknown",
            BigEndianRepresentation = "92DCE",
            LittleEndianRepresentation = new byte[] { 0xCE, 0x2D, 0x09 },
            ProcDependencies = new RawProc[] { SomeBooleanResponse }
        };

        public static readonly RawProc UnknownCheck3 = new RawProc
        {
            CommonName = "Unknown",
            BigEndianRepresentation = "6428F0",
            LittleEndianRepresentation = new byte[] { 0xF0, 0x28, 0x64 },
            ProcDependencies = new RawProc[] { SomeBooleanResponse }
        };

        public static readonly RawProc DifficultyCheck = new RawProc
        {
            CommonName = "DifficultyCheck",
            BigEndianRepresentation = "4B0A61",
            LittleEndianRepresentation = new byte[] { 0x61, 0x0A, 0x4B },
            ProcDependencies = null
        };
        #endregion

        #region Placed objects
        #region Items
        public static readonly RawProc AwardAksSuppressor = new RawProc
        {
            CommonName = "AK Suppressor",
            BigEndianRepresentation = "A10594",
            LittleEndianRepresentation = new byte[] { 0x94, 0x05, 0xA1 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardBandages = new RawProc
        {
            CommonName = "Bandages",
            BigEndianRepresentation = "D2A222",
            LittleEndianRepresentation = new byte[] { 0x22, 0xA2, 0xD2 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardBodyArmor = new RawProc
        {
            CommonName = "Body Armor",
            BigEndianRepresentation = "E6E562",
            LittleEndianRepresentation = new byte[] { 0x62, 0xE5, 0xE6 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardBox1 = new RawProc
        {
            CommonName = "Box 1",
            BigEndianRepresentation = "97D665",
            LittleEndianRepresentation = new byte[] { 0x65, 0xD6, 0x97 },
            ProcDependencies = null
        }; //w01d says this is box2, not box1(maybe error on bp side?)
        public static readonly RawProc AwardBox2 = new RawProc
        {
            CommonName = "Box 2",
            BigEndianRepresentation = "3E97CF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x3E },
            ProcDependencies = null
        };
        public static readonly RawProc AwardBox3 = new RawProc
        {
            CommonName = "Box 3",
            BigEndianRepresentation = "5E97CF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x5E},
            ProcDependencies = null
        };
        public static readonly RawProc AwardBox4 = new RawProc
        {
            CommonName = "Box 4",
            BigEndianRepresentation = "7E97CF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x7E },
            ProcDependencies = null
        };
        public static readonly RawProc AwardBox5 = new RawProc
        {
            CommonName = "Box 5",
            BigEndianRepresentation = "9E97CF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x97, 0x9E },
            ProcDependencies = null
        };
        public static readonly RawProc AwardWetBox = new RawProc
        {
            CommonName = "Wet Box",
            BigEndianRepresentation = "CAF11B",
            LittleEndianRepresentation = new byte[] { 0x1B, 0xF1, 0xCA },
            ProcDependencies = null
        };
        public static readonly RawProc AwardRation = new RawProc
        {
            CommonName = "Ration",
            BigEndianRepresentation = "53A62E",
            LittleEndianRepresentation = new byte[] { 0x2E, 0xA6, 0x53 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        }; //technically there are multiple versions of this with different dependencies. need to be careful about this
        public static readonly RawProc AwardColdMeds = new RawProc
        {
            CommonName = "Cold Meds",
            BigEndianRepresentation = "AD455F",
            LittleEndianRepresentation = new byte[] { 0x5F, 0x45, 0xAD },
            ProcDependencies = new RawProc[] { DifficultyCheck, AwardRation }
        };
        public static readonly RawProc AwardColdMeds2 = new RawProc
        {
            CommonName = "Cold Meds 2",
            BigEndianRepresentation = "42E8F1",
            LittleEndianRepresentation = new byte[] { 0xF1, 0xE8, 0x42 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCoolantSpray = new RawProc
        {
            CommonName = "Coolant Spray",
            BigEndianRepresentation = "7F5DC3",
            LittleEndianRepresentation = new byte[] { 0xC3, 0x5D, 0x7F },
            ProcDependencies = new RawProc[] { UnknownCheck3 }
        };
        public static readonly RawProc AwardDigitalCamera = new RawProc
        {
            CommonName = "Digital Camera",
            BigEndianRepresentation = "640C81",
            LittleEndianRepresentation = new byte[] { 0x81, 0x0C, 0x64 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardMineDetector = new RawProc
        {
            CommonName = "Mine Detector",
            BigEndianRepresentation = "C60769",
            LittleEndianRepresentation = new byte[] { 0x69, 0x07, 0xC6 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardNvg = new RawProc
        {
            CommonName = "Night Vision Goggles",
            BigEndianRepresentation = "3579CF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x79, 0x35 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardPentazemin = new RawProc
        {
            CommonName = "Pentazemin",
            BigEndianRepresentation = "F71E5B",
            LittleEndianRepresentation = new byte[] { 0x5B, 0x1E, 0xF7 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        
        public static readonly RawProc AwardSensorB = new RawProc
        {
            CommonName = "Sensor B",
            BigEndianRepresentation = "293658",
            LittleEndianRepresentation = new byte[] { 0x58, 0x36, 0x29 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardShaver = new RawProc
        {
            CommonName = "Shaver",
            BigEndianRepresentation = "616ECF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0x6E, 0x61 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardSocomSuppressor = new RawProc
        {
            CommonName = "Socom Supressor",
            BigEndianRepresentation = "96A6A",
            LittleEndianRepresentation = new byte[] { 0x6A, 0x6A, 0x9 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardThermalG = new RawProc
        {
            CommonName = "Thermal Goggles",
            BigEndianRepresentation = "2F31D6",
            LittleEndianRepresentation = new byte[] { 0xD6, 0x31, 0x2F },
            ProcDependencies = null
        };
        public static readonly RawProc AwardUspSuppressor = new RawProc
        {
            CommonName = "USP Suppressor",
            BigEndianRepresentation = "C92A40",
            LittleEndianRepresentation = new byte[] { 0x40, 0x2A, 0xC9 },
            ProcDependencies = null
        };
        public static readonly RawProc AwardCigarettes = new RawProc
        {
            CommonName = "Cigarettes",
            BigEndianRepresentation = "FFDAC8",
            LittleEndianRepresentation = new byte[] { 0xC8, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardSensorA = new RawProc
        {
            CommonName = "Sensor A",
            BigEndianRepresentation = "FFDAC9",
            LittleEndianRepresentation = new byte[] { 0xC9, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardAPSensor = new RawProc
        {
            CommonName = "AP Sensor",
            BigEndianRepresentation = "FFDACA",
            LittleEndianRepresentation = new byte[] { 0xCA, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardPhone = new RawProc
        {
            CommonName = "Phone",
            BigEndianRepresentation = "FFDACB",
            LittleEndianRepresentation = new byte[] { 0xCB, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardScope = new RawProc
        {
            CommonName = "Scope",
            BigEndianRepresentation = "FFDACC",
            LittleEndianRepresentation = new byte[] { 0xCC, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardMODisk = new RawProc
        {
            CommonName = "MO Disk",
            BigEndianRepresentation = "FFDACD",
            LittleEndianRepresentation = new byte[] { 0xCD, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCamera = new RawProc
        {
            CommonName = "Camera",
            BigEndianRepresentation = "FFDACE",
            LittleEndianRepresentation = new byte[] { 0xCE, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardBDU = new RawProc
        {
            CommonName = "BDU",
            BigEndianRepresentation = "FFDACF",
            LittleEndianRepresentation = new byte[] { 0xCF, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCard1 = new RawProc
        {
            CommonName = "Card 1",
            BigEndianRepresentation = "FFDAD0",
            LittleEndianRepresentation = new byte[] { 0xD0, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCard2 = new RawProc
        {
            CommonName = "Card 2",
            BigEndianRepresentation = "FFDAD1",
            LittleEndianRepresentation = new byte[] { 0xD1, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCard3 = new RawProc
        {
            CommonName = "Card 3",
            BigEndianRepresentation = "FFDAD2",
            LittleEndianRepresentation = new byte[] { 0xD2, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCard4 = new RawProc
        {
            CommonName = "Card 4",
            BigEndianRepresentation = "FFDAD3",
            LittleEndianRepresentation = new byte[] { 0xD3, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardCard5 = new RawProc
        {
            CommonName = "Card 5",
            BigEndianRepresentation = "FFDAD4",
            LittleEndianRepresentation = new byte[] { 0xD4, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        #endregion
        #region Weapons
        public static readonly RawProc AwardAksAmmo = new RawProc
        {
            CommonName = "AKS Ammo",
            BigEndianRepresentation = "E7C89B",
            LittleEndianRepresentation = new byte[] { 0x9B, 0xC8, 0xE7 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardAksGun = new RawProc
        {
            CommonName = "AKS Weapon",
            BigEndianRepresentation = "35513F",
            LittleEndianRepresentation = new byte[] { 0x3F, 0x51, 0x35 },
            ProcDependencies = new RawProc[] { InventoryCheck, Necessity }
        };
        public static readonly RawProc AwardBook = new RawProc
        {
            CommonName = "Book",
            BigEndianRepresentation = "74BFF8",
            LittleEndianRepresentation = new byte[] { 0xF8, 0xBF, 0x74 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardC4 = new RawProc
        {
            CommonName = "C4",
            BigEndianRepresentation = "134D11",
            LittleEndianRepresentation = new byte[] { 0x11, 0x4D, 0x13 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardChaffG = new RawProc
        {
            CommonName = "Chaff Grenade",
            BigEndianRepresentation = "F57D47",
            LittleEndianRepresentation = new byte[] { 0x47, 0x7D, 0xF5 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardClaymore = new RawProc
        {
            CommonName = "Claymore",
            BigEndianRepresentation = "60924",
            LittleEndianRepresentation = new byte[] { 0x24, 0x09, 0x6 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardDirectionalMic = new RawProc
        {
            CommonName = "Directional Mic",
            BigEndianRepresentation = "6F5DC3",
            LittleEndianRepresentation = new byte[] { 0xC3, 0x5D, 0x6F },
            ProcDependencies = new RawProc[] { UnknownCheck3 }
        };
        public static readonly RawProc AwardGrenade = new RawProc
        {
            CommonName = "Grenade",
            BigEndianRepresentation = "37D629",
            LittleEndianRepresentation = new byte[] { 0x29, 0xD6, 0x37 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardM4Ammo = new RawProc
        {
            CommonName = "M4 Ammo",
            BigEndianRepresentation = "EA7EAE",
            LittleEndianRepresentation = new byte[] { 0xAE, 0x7E, 0xEA },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardM4Gun = new RawProc
        {
            CommonName = "M4 Weapon",
            BigEndianRepresentation = "135211",
            LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0x13 },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck }
        };
        public static readonly RawProc AwardM9Ammo = new RawProc
        {
            CommonName = "M9 Ammo",
            BigEndianRepresentation = "8A7EAF",
            LittleEndianRepresentation = new byte[] { 0xAF, 0x7E, 0x8A },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardM9Gun = new RawProc
        {
            CommonName = "M9 Weapon",
            BigEndianRepresentation = "B35211",
            LittleEndianRepresentation = new byte[] { 0x11, 0x52, 0xB3 },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck }
        };
        public static readonly RawProc AwardNikitaAmmo = new RawProc
        {
            CommonName = "Nikita Ammo",
            BigEndianRepresentation = "EB47E6",
            LittleEndianRepresentation = new byte[] { 0xE6, 0x47, 0xEB },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardNikitaGun = new RawProc
        {
            CommonName = "Nikita Weapon",
            BigEndianRepresentation = "DD51D",
            LittleEndianRepresentation = new byte[] { 0x1D, 0xD5, 0xD },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck, UnknownCheck2 }
        };
        public static readonly RawProc AwardPsg1Ammo = new RawProc
        {
            CommonName = "PSG1 Ammo",
            BigEndianRepresentation = "F76B84",
            LittleEndianRepresentation = new byte[] { 0x84, 0x6B, 0xF7 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardPsg1Gun = new RawProc
        {
            CommonName = "PSG1 Weapon",
            BigEndianRepresentation = "19F8BB",
            LittleEndianRepresentation = new byte[] { 0xBB, 0xF8, 0x19 },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck, UnknownCheck }
        };
        public static readonly RawProc AwardPsg1tAmmo = new RawProc
        {
            CommonName = "PSG1-T Ammo",
            BigEndianRepresentation = "7A253E",
            LittleEndianRepresentation = new byte[] { 0x3E, 0x25, 0x7A },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardPsg1tGun = new RawProc
        {
            CommonName = "PSG1-T Weapon",
            BigEndianRepresentation = "DC4E11",
            LittleEndianRepresentation = new byte[] { 0x11, 0x4E, 0xDC },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck }
        };
        public static readonly RawProc AwardRgbAmmo = new RawProc
        {
            CommonName = "RGB-6 Ammo",
            BigEndianRepresentation = "7F6915",
            LittleEndianRepresentation = new byte[] { 0x15, 0x69, 0x7F },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardRgbGun = new RawProc
        {
            CommonName = "RGB-6 Weapon",
            BigEndianRepresentation = "A1F64B",
            LittleEndianRepresentation = new byte[] { 0x4B, 0xF6, 0xA1 },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck }
        };
        public static readonly RawProc AwardSocomAmmo = new RawProc
        {
            CommonName = "SOCOM Ammo",
            BigEndianRepresentation = "FFDCA5",
            LittleEndianRepresentation = new byte[] { 0xA5, 0xDC, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardStingerAmmo = new RawProc
        {
            CommonName = "Stinger Ammo",
            BigEndianRepresentation = "4ADA9",
            LittleEndianRepresentation = new byte[] { 0xA9, 0xAD, 0x4 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardStingerGun = new RawProc
        {
            CommonName = "Stinger Weapon",
            BigEndianRepresentation = "66D67C",
            LittleEndianRepresentation = new byte[] { 0x7C, 0xD6, 0x66 },
            ProcDependencies = new RawProc[] { InventoryCheck, GunCheck }
        };
        public static readonly RawProc AwardStunG = new RawProc
        {
            CommonName = "Stun Grenade",
            BigEndianRepresentation = "A56B4B",
            LittleEndianRepresentation = new byte[] { 0x4B, 0x6B, 0xA5 },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardUspAmmo = new RawProc
        {
            CommonName = "USP Ammo",
            BigEndianRepresentation = "AFCC9B",
            LittleEndianRepresentation = new byte[] { 0x9B, 0xCC, 0xAF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardUsp = new RawProc
        {
            CommonName = "USP",
            BigEndianRepresentation = "FFDAC6",
            LittleEndianRepresentation = new byte[] { 0xC6, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        public static readonly RawProc AwardSocom = new RawProc
        {
            CommonName = "Socom",
            BigEndianRepresentation = "FFDAC7",
            LittleEndianRepresentation = new byte[] { 0xC7, 0xDA, 0xFF },
            ProcDependencies = new RawProc[] { InventoryCheck }
        };
        #endregion
        #endregion

        #region Dropped objects
        public static readonly RawProc DropRation = new RawProc
        {
            BigEndianRepresentation = "40DFF",
            LittleEndianRepresentation = new byte[] { 0xFF, 0x0D, 0x4 }
        }; //4B7E2E <--- stinky ration??
        public static readonly RawProc DropDogTag = new RawProc
        {
            BigEndianRepresentation = "DD10F7",
            LittleEndianRepresentation = new byte[] { 0xF7, 0x10, 0xDD }
        };
        public static readonly RawProc DropBandage = new RawProc
        {
            BigEndianRepresentation = "F3CD46",
            LittleEndianRepresentation = new byte[] { 0x46, 0xCD, 0xF3 }
        };
        public static readonly RawProc DropStunG = new RawProc
        {
            BigEndianRepresentation = "205FD4",
            LittleEndianRepresentation = new byte[] { 0xD4, 0x5F, 0x20 }
        };
        public static readonly RawProc DropChaffG = new RawProc
        {
            BigEndianRepresentation = "217F99",
            LittleEndianRepresentation = new byte[] { 0x99, 0x7F, 0x21 }
        };
        public static readonly RawProc DropM9 = new RawProc
        {
            BigEndianRepresentation = "9A9E1D",
            LittleEndianRepresentation = new byte[] { 0x1D, 0x9E, 0x9A }
        }; //719612 seems to be a complicated version of this?
        public static readonly RawProc DropUsp = new RawProc
        {
            BigEndianRepresentation = "C674D4",
            LittleEndianRepresentation = new byte[] { 0xD4, 0x74, 0xC6 }
        };
        public static readonly RawProc DropPentazemin = new RawProc
        {
            BigEndianRepresentation = "3B90D9",
            LittleEndianRepresentation = new byte[] { 0xD9, 0x90, 0x3B }
        };
        public static readonly RawProc DropGrenade = new RawProc
        {
            BigEndianRepresentation = "470DAD",
            LittleEndianRepresentation = new byte[] { 0xAD, 0x0D, 0x47 }
        };
        public static readonly RawProc DropM4 = new RawProc
        {
            BigEndianRepresentation = "719608",
            LittleEndianRepresentation = new byte[] { 0x08, 0x96, 0x71 }
        };
        public static readonly RawProc DropSocom = new RawProc
        {
            BigEndianRepresentation = "C77579",
            LittleEndianRepresentation = new byte[] { 0x79, 0x75, 0xC7 }
        };
        public static readonly RawProc DropStinger = new RawProc
        {
            BigEndianRepresentation = "1485AA",
            LittleEndianRepresentation = new byte[] { 0xAA, 0x85, 0x14 }
        };
        public static readonly RawProc DropPsg1tAmmo = new RawProc
        {
            BigEndianRepresentation = "6BFF01",
            LittleEndianRepresentation = new byte[] { 0x01, 0xFF, 0x6B }
        };
        public static readonly RawProc DropNikitaAmmo = new RawProc
        {
            BigEndianRepresentation = "7E2988",
            LittleEndianRepresentation = new byte[] { 0x88, 0x29, 0x7E }
        };

        #endregion

        #region Spawner Functions
        public static readonly RawProc w00aSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w00bSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w00cSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01aSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01bSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01cSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01dSpawner1 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01dSpawner2 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01eSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01fSpawner1 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01fSpawner2 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01fSpawner3 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w01fSpawner4 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w02aSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w03aSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w03bSpawner = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w04aSpawner1 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        public static readonly RawProc w04aSpawner2 = new RawProc
        {
            CommonName = "w00a spawner",
            BigEndianRepresentation = "0x98B5A",
            LittleEndianRepresentation = new byte[] { 0x5A, 0x8B, 0x09 }
        };
        #endregion

        #region Other Functions
        public static readonly RawProc EnableBlade = new RawProc
        {
            CommonName = "EnableBlade",
            BigEndianRepresentation = "40ADFE",
            LittleEndianRepresentation = new byte[] { 0xFE, 0xAD, 0x40 },
            ProcDependencies = null
        };
        #endregion

        #endregion

        public static readonly List<RawProc> SpawnProcs = new List<RawProc>
        {
            AwardAksAmmo, AwardAksGun, AwardAksSuppressor, AwardBandages, AwardBodyArmor,
            AwardBook, AwardBox1, AwardBox2, AwardBox3, AwardBox4, 
            AwardBox5, AwardWetBox, AwardC4, AwardChaffG, AwardClaymore, 
            AwardColdMeds, AwardColdMeds2, AwardCoolantSpray, AwardDigitalCamera, AwardDirectionalMic, AwardGrenade, 
            AwardM4Ammo, AwardM4Gun, AwardMineDetector, AwardNvg, AwardM9Ammo, 
            AwardM9Gun, AwardNikitaAmmo, AwardNikitaGun, AwardPsg1Ammo, AwardPsg1Gun, 
            AwardPsg1tAmmo, AwardPsg1tGun, AwardRgbAmmo, AwardRgbGun, AwardSocomAmmo,
            AwardSocomSuppressor, AwardStingerAmmo, AwardStingerGun, AwardStunG, AwardUspAmmo, AwardPentazemin,
            AwardRation, AwardSensorB, AwardShaver, AwardUspSuppressor, AwardThermalG,
            AwardCigarettes, AwardSensorA, AwardAPSensor, AwardUsp, AwardSocom, 
            AwardPhone, AwardScope, AwardMODisk, AwardCamera, AwardBDU, 
            AwardCard1, AwardCard2, AwardCard3, AwardCard4, AwardCard5, EnableBlade
        };
    }
}
