using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MGS2_MC.CommonObjects;

namespace MGS2_MC
{
    internal class Strings
    {
        #region String offsets
        #region Calculated From LifeAndGripNames
        public static readonly MemoryOffset LIFE_TEXT = new MemoryOffset(10, 13); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv1_TEXT = new MemoryOffset(22, 29); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv2_TEXT = new MemoryOffset(-156, -149); //TODO: prove this is valid
        public static readonly MemoryOffset GRIP_Lv3_TEXT = new MemoryOffset(-172, -165); //TODO: prove this is valid
        #endregion

        #region Calculated From RationMedsBandagePentazeminDescriptions
        //TODO: fill these out
        #endregion

        #region Calculated From RayNames
        public static readonly MemoryOffset RAY_01 = new MemoryOffset(78, 85); //TODO: prove these are valid (these values are using the endByte, cuz im a fool)
        //public static readonly MemoryBytes RAY_01 = new MemoryBytes(68, 75); //This should work if the uncommented one does not
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
        public static readonly MemoryOffset SOLIDUS_HP_TEXT = new MemoryOffset(61, 67); //TODO: prove this is valid
        #endregion

        #region Calculated From EmmaO2
        public static readonly MemoryOffset EMMA_O2_TEXT = new MemoryOffset(25, 31); //TODO: prove this is valid
        public static readonly MemoryOffset RAIDEN_O2_TEXT = new MemoryOffset(137, 138); //TODO: prove this is valid
        #endregion

        #region Calculated From FatmanName
        public static readonly MemoryOffset FATMAN_HP_TEXT = new MemoryOffset(89, 94); //TODO: validate
        #endregion

        #region Calculated From OlgaName
        public static readonly MemoryOffset OLGA_HP_TEXT = new MemoryOffset(293, 296); //TODO: validate
                                                                                       //there is also a meryl string right next to OLGA... but idk what it is used for so i'm not bothering to add it atm
        #endregion

        #region Calculated From HarrierName
        public static readonly MemoryOffset HARRIER_HP_TEXT = new MemoryOffset(-109, -103);
        #endregion

        #region Calculated From KasatkaName
        public static readonly MemoryOffset KASATKA_HP_TEXT = new MemoryOffset(-8, -2);
        #endregion

        #region Calculated From FortuneName
        public static readonly MemoryOffset FORTUNE_HP_TEXT = new MemoryOffset(1189, 1195); //TODO: validate
        #endregion
        #endregion

        #region String collection
        public static readonly List<MGS2String> MGS2_STRINGS = new List<MGS2String>
        {
            new MGS2String { MemoryOffset = LIFE_TEXT, Tag = "Life Bar" },
            new MGS2String { MemoryOffset = GRIP_Lv1_TEXT, Tag = "Grip Lv1" },
            new MGS2String { MemoryOffset = GRIP_Lv2_TEXT, Tag = "Grip Lv2" },
            new MGS2String { MemoryOffset = GRIP_Lv3_TEXT, Tag = "Grip Lv3" },
            new MGS2String { MemoryOffset = OLGA_HP_TEXT, Tag = "Olga Life Bar" },
            new MGS2String { MemoryOffset = RAIDEN_O2_TEXT, Tag = "O2 Bar" },
            new MGS2String { MemoryOffset = FATMAN_HP_TEXT, Tag = "Fatman Life Bar" },
            new MGS2String { MemoryOffset = FORTUNE_HP_TEXT, Tag = "Fortune Life Bar" },
            new MGS2String { MemoryOffset = HARRIER_HP_TEXT, Tag = "Harrier Life Bar" },
            new MGS2String { MemoryOffset = KASATKA_HP_TEXT, Tag = "Kasatka Life Bar" },
            new MGS2String { MemoryOffset = EMMA_O2_TEXT, Tag = "Emma's O2 Bar" },
            new MGS2String { MemoryOffset = RAY_01, Tag = "Ray #01 Life Bar" },
            new MGS2String { MemoryOffset = RAY_02, Tag = "Ray #02 Life Bar" },
            new MGS2String { MemoryOffset = RAY_03, Tag = "Ray #03 Life Bar" },
            new MGS2String { MemoryOffset = RAY_04, Tag = "Ray #04 Life Bar" },
            new MGS2String { MemoryOffset = RAY_05, Tag = "Ray #05 Life Bar" },
            new MGS2String { MemoryOffset = RAY_06, Tag = "Ray #06 Life Bar" },
            new MGS2String { MemoryOffset = RAY_07, Tag = "Ray #07 Life Bar" },
            new MGS2String { MemoryOffset = RAY_08, Tag = "Ray #08 Life Bar" },
            new MGS2String { MemoryOffset = RAY_09, Tag = "Ray #09 Life Bar" },
            new MGS2String { MemoryOffset = RAY_10, Tag = "Ray #10 Life Bar" },
            new MGS2String { MemoryOffset = RAY_11, Tag = "Ray #11 Life Bar" },
            new MGS2String { MemoryOffset = RAY_12, Tag = "Ray #12 Life Bar" },
            new MGS2String { MemoryOffset = RAY_13, Tag = "Ray #13 Life Bar" },
            new MGS2String { MemoryOffset = RAY_14, Tag = "Ray #14 Life Bar" },
            new MGS2String { MemoryOffset = RAY_15, Tag = "Ray #15 Life Bar" },
            new MGS2String { MemoryOffset = RAY_16, Tag = "Ray #16 Life Bar" },
            new MGS2String { MemoryOffset = RAY_17, Tag = "Ray #17 Life Bar" },
            new MGS2String { MemoryOffset = RAY_18, Tag = "Ray #18 Life Bar" },
            new MGS2String { MemoryOffset = RAY_19, Tag = "Ray #19 Life Bar" },
            new MGS2String { MemoryOffset = RAY_20, Tag = "Ray #20 Life Bar" },
            new MGS2String { MemoryOffset = RAY_21, Tag = "Ray #21 Life Bar" },
            new MGS2String { MemoryOffset = RAY_22, Tag = "Ray #22 Life Bar" },
            new MGS2String { MemoryOffset = RAY_23, Tag = "Ray #23 Life Bar" },
            new MGS2String { MemoryOffset = RAY_24, Tag = "Ray #24 Life Bar" },
            new MGS2String { MemoryOffset = RAY_25, Tag = "Ray #25 Life Bar" },
            new MGS2String { MemoryOffset = SOLIDUS_HP_TEXT, Tag = "Solidus' Life Bar" }
        };
        #endregion
    }
}
