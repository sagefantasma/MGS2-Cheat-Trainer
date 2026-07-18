using System.Collections.Generic;

namespace MGS2_CheatTrainer_V2
{
    //REWRITE STATUS: Done?
    public static class Mgs2Strings
    {
        /// <summary>
        /// An object representing a string value in MGS2.
        /// MemoryOffset describes how it is stored in memory.
        /// Tag provides the object with a constant, human-readable name.
        /// </summary>
        public class Mgs2String
        {
            public required MemoryOffset MemoryOffset { get; init; }
            public required string FinderAoB { get; init; }
            public required string Tag { get; init; }
            public required string CurrentText { get; set; }
        }

        #region String collection
        public static readonly List<Mgs2String> Mgs2StringList =
        [
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.LifeText, FinderAoB = Mgs2AoB.LifeAndGripNames, Tag = "Life Bar",
                CurrentText = "LIFE"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.GripLv1Text, FinderAoB = Mgs2AoB.LifeAndGripNames, Tag = "Grip Lv1",
                CurrentText = "GRIP Lv1"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.GripLv2Text, FinderAoB = Mgs2AoB.LifeAndGripNames, Tag = "Grip Lv2",
                CurrentText = "GRIP Lv2"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.GripLv3Text, FinderAoB = Mgs2AoB.LifeAndGripNames, Tag = "Grip Lv3",
                CurrentText = "GRIP Lv3"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.OlgaHpText, FinderAoB = Mgs2AoB.OlgaName, Tag = "Olga Life Bar",
                CurrentText = "OLGA"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.RaidenO2Text, FinderAoB = Mgs2AoB.EmmaO2, Tag = "O2 Bar", CurrentText = "O2"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.FatmanHpText, FinderAoB = Mgs2AoB.FatmanName, Tag = "Fatman Life Bar",
                CurrentText = "FATMAN"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.FortuneHpText, FinderAoB = Mgs2AoB.FortuneName, Tag = "Fortune Life Bar",
                CurrentText = "FORTUNE"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.VampHpText, FinderAoB = Mgs2AoB.FortuneName, Tag = "Vamp Life Bar",
                CurrentText = "VAMP"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Vamp02Text, FinderAoB = Mgs2AoB.Vamp02, Tag = "Vamp O2 Bar",
                CurrentText = "VAMP O2"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.HarrierHpText, FinderAoB = Mgs2AoB.HarrierName, Tag = "Harrier Life Bar",
                CurrentText = "HARRIER"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.EmmaHpText, FinderAoB = Mgs2AoB.EmmaName, Tag = "Emma Life Bar",
                CurrentText = "EMMA"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.KasatkaHpText, FinderAoB = Mgs2AoB.KasatkaName, Tag = "Kasatka Life Bar",
                CurrentText = "KASATKA"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.EmmaO2Text, FinderAoB = Mgs2AoB.EmmaO2, Tag = "Emma O2 Bar",
                CurrentText = "EMMA O2"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray01, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #01 Life Bar",
                CurrentText = "RAY-A01E"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray02, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #02 Life Bar",
                CurrentText = "RAY-A02E"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray03, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #03 Life Bar",
                CurrentText = "RAY-A03E"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray04, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #04 Life Bar",
                CurrentText = "RAY-A04E"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray05, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #05 Life Bar",
                CurrentText = "RAY-A05E"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray06, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #06 Life Bar",
                CurrentText = "RAY-B01F"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray07, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #07 Life Bar",
                CurrentText = "RAY-B02F"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray08, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #08 Life Bar",
                CurrentText = "RAY-B03F"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray09, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #09 Life Bar",
                CurrentText = "RAY-B04F"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray10, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #10 Life Bar",
                CurrentText = "RAY-B05F"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray11, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #11 Life Bar",
                CurrentText = "RAY-C01H"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray12, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #12 Life Bar",
                CurrentText = "RAY-C02H"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray13, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #13 Life Bar",
                CurrentText = "RAY-C03H"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray14, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #14 Life Bar",
                CurrentText = "RAY-C04H"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray15, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #15 Life Bar",
                CurrentText = "RAY-C05H"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray16, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #16 Life Bar",
                CurrentText = "RAY-D01G"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray17, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #17 Life Bar",
                CurrentText = "RAY-D02G"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray18, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #18 Life Bar",
                CurrentText = "RAY-D03G"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray19, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #19 Life Bar",
                CurrentText = "RAY-D04G"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray20, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #20 Life Bar",
                CurrentText = "RAY-D05G"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray21, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #21 Life Bar",
                CurrentText = "RAY-E01L"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray22, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #22 Life Bar",
                CurrentText = "RAY-E02L"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray23, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #23 Life Bar",
                CurrentText = "RAY-E03L"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray24, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #24 Life Bar",
                CurrentText = "RAY-E04L"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.Ray25, FinderAoB = Mgs2AoB.RayNames, Tag = "Ray #25 Life Bar",
                CurrentText = "RAY-E05L"
            },
            new Mgs2String
            {
                MemoryOffset = Mgs2Offset.SolidusHpText, FinderAoB = Mgs2AoB.SolidusName, Tag = "Solidus Life Bar",
                CurrentText = "SOLIDUS"
            }
        ];
        #endregion
    }
}
