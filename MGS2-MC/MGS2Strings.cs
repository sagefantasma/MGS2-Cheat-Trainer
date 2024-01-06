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
            public string Tag { get; set; }
        }

        #region String collection
        public static readonly List<MGS2String> MGS2_STRINGS = new List<MGS2String>
        {
            new MGS2String { MemoryOffset = MGS2Offsets.LIFE_TEXT, Tag = "Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv1_TEXT, Tag = "Grip Lv1" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv2_TEXT, Tag = "Grip Lv2" },
            new MGS2String { MemoryOffset = MGS2Offsets.GRIP_Lv3_TEXT, Tag = "Grip Lv3" },
            new MGS2String { MemoryOffset = MGS2Offsets.OLGA_HP_TEXT, Tag = "Olga Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAIDEN_O2_TEXT, Tag = "O2 Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.FATMAN_HP_TEXT, Tag = "Fatman Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.FORTUNE_HP_TEXT, Tag = "Fortune Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.HARRIER_HP_TEXT, Tag = "Harrier Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.KASATKA_HP_TEXT, Tag = "Kasatka Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.EMMA_O2_TEXT, Tag = "Emma's O2 Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_01, Tag = "Ray #01 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_02, Tag = "Ray #02 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_03, Tag = "Ray #03 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_04, Tag = "Ray #04 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_05, Tag = "Ray #05 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_06, Tag = "Ray #06 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_07, Tag = "Ray #07 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_08, Tag = "Ray #08 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_09, Tag = "Ray #09 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_10, Tag = "Ray #10 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_11, Tag = "Ray #11 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_12, Tag = "Ray #12 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_13, Tag = "Ray #13 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_14, Tag = "Ray #14 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_15, Tag = "Ray #15 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_16, Tag = "Ray #16 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_17, Tag = "Ray #17 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_18, Tag = "Ray #18 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_19, Tag = "Ray #19 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_20, Tag = "Ray #20 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_21, Tag = "Ray #21 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_22, Tag = "Ray #22 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_23, Tag = "Ray #23 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_24, Tag = "Ray #24 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.RAY_25, Tag = "Ray #25 Life Bar" },
            new MGS2String { MemoryOffset = MGS2Offsets.SOLIDUS_HP_TEXT, Tag = "Solidus' Life Bar" }
        };
        #endregion
    }
}
