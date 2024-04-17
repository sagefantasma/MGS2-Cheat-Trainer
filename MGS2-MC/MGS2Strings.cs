using System.Collections.Generic;

namespace MGS2_MC
{
    internal class MGS2Strings
    {
        /// <summary>
        /// An object representing a string value in MGS2.
        /// MemoryOffset describes how it is stored in memory.
        /// Tag provides the object with a constant, human-readable name.
        /// </summary>
        public class MGS2String
        {
            public MemoryOffset MemoryOffset { get; set; }
            internal byte[] FinderAoB { get; set; }
            public string Tag { get; set; }
            public string CurrentText { get; set; }
        }

        #region String collection
        public static readonly List<MGS2String> MGS2_STRINGS = new List<MGS2String>
        {
            new MGS2String { MemoryOffset = MGS2Offset.LIFE_TEXT, FinderAoB = MGS2AoB.LifeAndGripNames, Tag = "Life Bar", CurrentText = "LIFE" },
            new MGS2String { MemoryOffset = MGS2Offset.GRIP_Lv1_TEXT, FinderAoB = MGS2AoB.LifeAndGripNames, Tag = "Grip Lv1", CurrentText = "GRIP Lv1" },
            new MGS2String { MemoryOffset = MGS2Offset.GRIP_Lv2_TEXT, FinderAoB = MGS2AoB.LifeAndGripNames, Tag = "Grip Lv2", CurrentText = "GRIP Lv2" },
            new MGS2String { MemoryOffset = MGS2Offset.GRIP_Lv3_TEXT, FinderAoB = MGS2AoB.LifeAndGripNames, Tag = "Grip Lv3", CurrentText = "GRIP Lv3" },
            new MGS2String { MemoryOffset = MGS2Offset.OLGA_HP_TEXT, FinderAoB = MGS2AoB.OlgaName, Tag = "Olga Life Bar", CurrentText = "OLGA" },
            new MGS2String { MemoryOffset = MGS2Offset.RAIDEN_O2_TEXT, FinderAoB = MGS2AoB.EmmaO2, Tag = "O2 Bar", CurrentText = "O2" },
            new MGS2String { MemoryOffset = MGS2Offset.FATMAN_HP_TEXT, FinderAoB = MGS2AoB.FatmanName, Tag = "Fatman Life Bar", CurrentText = "FATMAN" },
            new MGS2String { MemoryOffset = MGS2Offset.FORTUNE_HP_TEXT, FinderAoB = MGS2AoB.FortuneName, Tag = "Fortune Life Bar", CurrentText = "FORTUNE" },
            new MGS2String { MemoryOffset = MGS2Offset.VAMP_HP_TEXT, FinderAoB = MGS2AoB.FortuneName, Tag = "Vamp Life Bar", CurrentText = "VAMP" },
            new MGS2String { MemoryOffset = MGS2Offset.VAMP_02_TEXT, FinderAoB = MGS2AoB.Vamp02, Tag = "Vamp O2 Bar", CurrentText = "O2" },
            new MGS2String { MemoryOffset = MGS2Offset.HARRIER_HP_TEXT, FinderAoB = MGS2AoB.HarrierName, Tag = "Harrier Life Bar", CurrentText = "HARRIER" },
            new MGS2String { MemoryOffset = MGS2Offset.EMMA_HP_TEXT, FinderAoB = MGS2AoB.EmmaName, Tag = "Emma Life Bar", CurrentText = "EMMA" },
            new MGS2String { MemoryOffset = MGS2Offset.KASATKA_HP_TEXT, FinderAoB = MGS2AoB.KasatkaName, Tag = "Kasatka Life Bar", CurrentText = "KASATKA" },
            new MGS2String { MemoryOffset = MGS2Offset.EMMA_O2_TEXT, FinderAoB = MGS2AoB.EmmaO2, Tag = "Emma O2 Bar", CurrentText = "EMMA O2" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_01, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #01 Life Bar", CurrentText = "RAY-A01E" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_02, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #02 Life Bar", CurrentText = "RAY-A02E" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_03, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #03 Life Bar", CurrentText = "RAY-A03E" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_04, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #04 Life Bar", CurrentText = "RAY-A04E" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_05, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #05 Life Bar", CurrentText = "RAY-A05E" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_06, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #06 Life Bar", CurrentText = "RAY-B01F" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_07, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #07 Life Bar", CurrentText = "RAY-B02F" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_08, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #08 Life Bar", CurrentText = "RAY-B03F" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_09, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #09 Life Bar", CurrentText = "RAY-B04F" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_10, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #10 Life Bar", CurrentText = "RAY-B05F" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_11, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #11 Life Bar", CurrentText = "RAY-C01H" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_12, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #12 Life Bar", CurrentText = "RAY-C02H" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_13, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #13 Life Bar", CurrentText = "RAY-C03H" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_14, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #14 Life Bar", CurrentText = "RAY-C04H" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_15, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #15 Life Bar", CurrentText = "RAY-C05H" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_16, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #16 Life Bar", CurrentText = "RAY-D01G" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_17, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #17 Life Bar", CurrentText = "RAY-D02G" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_18, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #18 Life Bar", CurrentText = "RAY-D03G" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_19, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #19 Life Bar", CurrentText = "RAY-D04G" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_20, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #20 Life Bar", CurrentText = "RAY-D05G" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_21, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #21 Life Bar", CurrentText = "RAY-E01L" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_22, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #22 Life Bar", CurrentText = "RAY-E02L" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_23, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #23 Life Bar", CurrentText = "RAY-E03L" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_24, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #24 Life Bar", CurrentText = "RAY-E04L" },
            new MGS2String { MemoryOffset = MGS2Offset.RAY_25, FinderAoB = MGS2AoB.RayNames, Tag = "Ray #25 Life Bar", CurrentText = "RAY-E05L" },
            new MGS2String { MemoryOffset = MGS2Offset.SOLIDUS_HP_TEXT, FinderAoB = MGS2AoB.SolidusName, Tag = "Solidus' Life Bar", CurrentText = "SOLIDUS" }
        };
        #endregion
    }
}
