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
            new MGS2String { MemoryOffset = MGS2Offsets.LIFE_TEXT, FinderAoB = MGS2Offsets.LifeAndGripNamesAoB, Tag = "Life Bar", CurrentText = "LIFE" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv1_TEXT, FinderAoB = MGS2Offsets.LifeAndGripNamesAoB, Tag = "Grip Lv1", CurrentText = "GRIP Lv1" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv2_TEXT, FinderAoB = MGS2Offsets.LifeAndGripNamesAoB, Tag = "Grip Lv2", CurrentText = "GRIP Lv2" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv3_TEXT, FinderAoB = MGS2Offsets.LifeAndGripNamesAoB, Tag = "Grip Lv3", CurrentText = "GRIP Lv3" },
            new MGS2String { MemoryOffset = MGS2Offsets.OLGA_HP_TEXT, FinderAoB = MGS2Offsets.OlgaNameAoB, Tag = "Olga Life Bar", CurrentText = "OLGA" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAIDEN_O2_TEXT, FinderAoB = MGS2Offsets.EmmaO2AoB, Tag = "O2 Bar", CurrentText = "O2" },
            new MGS2String { MemoryOffset = MGS2Offsets.FATMAN_HP_TEXT, FinderAoB = MGS2Offsets.FatmanNameAoB, Tag = "Fatman Life Bar", CurrentText = "FATMAN" },
            new MGS2String { MemoryOffset = MGS2Offsets.FORTUNE_HP_TEXT, FinderAoB = MGS2Offsets.FortuneNameAoB, Tag = "Fortune Life Bar", CurrentText = "FORTUNE" },
            new MGS2String { MemoryOffset = MGS2Offsets.HARRIER_HP_TEXT, FinderAoB = MGS2Offsets.HarrierNameAoB, Tag = "Harrier Life Bar", CurrentText = "HARRIER" },
            new MGS2String { MemoryOffset = MGS2Offsets.KASATKA_HP_TEXT, FinderAoB = MGS2Offsets.KasatkaNameAoB, Tag = "Kasatka Life Bar", CurrentText = "KASATKA" },
            new MGS2String { MemoryOffset = MGS2Offsets.EMMA_O2_TEXT, FinderAoB = MGS2Offsets.EmmaO2AoB, Tag = "Emma's O2 Bar", CurrentText = "EMMA O2" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_01, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #01 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_02, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #02 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_03, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #03 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_04, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #04 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_05, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #05 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_06, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #06 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_07, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #07 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_08, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #08 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_09, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #09 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_10, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #10 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_11, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #11 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_12, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #12 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_13, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #13 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_14, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #14 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_15, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #15 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_16, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #16 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_17, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #17 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_18, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #18 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_19, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #19 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_20, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #20 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_21, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #21 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_22, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #22 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_23, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #23 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_24, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #24 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_25, FinderAoB = MGS2Offsets.RayNamesAoB, Tag = "Ray #25 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.SOLIDUS_HP_TEXT, FinderAoB = MGS2Offsets.SolidusNameAoB, Tag = "Solidus' Life Bar", CurrentText = "SOLIDUS" }
        };
        #endregion
    }
}
