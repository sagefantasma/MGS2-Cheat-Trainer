using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MGS2_MC.Helpers
{
    public enum GameType
    {
        Tanker,
        Plant,
        TankerPlant
    }

    public enum Difficulty
    {
        EuropeanExtreme = 60,
        Extreme = 50,
        Hard = 40,
        Normal = 30,
        Easy = 20,
        VeryEasy = 10
    }

    internal class Rank
    {
        public static Rank CurrentlyProjectedRank(MGS2MemoryManager.GameStats currentStats, Difficulty currentDifficulty, GameType gameType)
        {
            Rank projectedRank = null;
            if (gameType != GameType.TankerPlant) //for now i'm only worrying about TankerPlant shared ranks on different difficulties
                return projectedRank;

            projectedRank = new Rank();
            switch(currentDifficulty)
            {
                case Difficulty.EuropeanExtreme:
                case Difficulty.Extreme:
                    projectedRank = MGS2ExtremeRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats) == true);
                    break;
                case Difficulty.Hard:
                    projectedRank = MGS2HardRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats) == true);
                    break;
                case Difficulty.Normal:
                    projectedRank = MGS2NormalRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats) == true);
                    break;
                case Difficulty.Easy:
                case Difficulty.VeryEasy:
                    projectedRank = MGS2EasyRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats) == true);
                    break;
            }

            return projectedRank;
        }

        private bool AreStatsWithinRankRequirements(MGS2MemoryManager.GameStats stats)
        {
            foreach(FieldInfo member in typeof(MGS2MemoryManager.GameStats).GetFields())
            {
                var test = member.FieldType;
                if(member.FieldType == typeof(short))
                {
                    if ((short)member.GetValue(stats) < (short)member.GetValue(MinimumStats))
                        return false;
                    if ((short)member.GetValue(stats) > (short)member.GetValue(MaximumStats))
                        return false;
                }
                else if(member.FieldType == typeof(int))
                {
                    if ((int)member.GetValue(stats) < (int)member.GetValue(MinimumStats))
                        return false;
                    if ((int)member.GetValue(stats) > (int)member.GetValue(MaximumStats))
                        return false;
                }
                
            }

            return true;
        }

        public string Name { get; set; }
        public MGS2MemoryManager.GameStats MinimumStats { get; set; }
        public MGS2MemoryManager.GameStats MaximumStats { get; set; }

        //taken from: https://metalgear.fandom.com/wiki/Codename_(gameplay)#Requirements -- not the best source, but it'll do.
        public static List<Rank> MGS2ExtremeRanks = new List<Rank> { RankRequirements.BigBoss, RankRequirements.FoxExtreme, RankRequirements.DobermanExtreme, RankRequirements.HoundExtreme };
        public static List<Rank> MGS2HardRanks = new List<Rank> { RankRequirements.FoxHard, RankRequirements.DobermanHard, RankRequirements.HoundHard };
        public static List<Rank> MGS2NormalRanks = new List<Rank> { RankRequirements.DobermanNormal, RankRequirements.HoundNormal };
        public static List<Rank> MGS2EasyRanks = new List<Rank> { RankRequirements.HoundEasy };
        public static List<Rank> MGS2DifficultyAgnosticRanks = new List<Rank>(); //in case we ever decide to implement more
    }

    struct RankRequirements
    {
        

        #region Extreme Ranks
        public static Rank BigBoss = new Rank
        {
            Name = "Big Boss",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = 500,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 8,
                Shots = 700,
                SpecialItems = 0
            }
        };

        public static readonly Rank FoxExtreme = new Rank
        {
            Name = "Fox",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 500,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 8,
                Shots = 700,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 16,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //can use radar
            }
        };

        public static readonly Rank DobermanExtreme = new Rank
        {
            Name = "Doberman",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 4,
                Continues = 0,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = 3,
                MechsDestroyed = 60,
                PlayTime = 648900,
                Saves = short.MaxValue,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //can use radar
            }
        };

        public static readonly Rank HoundExtreme = new Rank
        {
            Name = "Hound",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 5,
                Continues = short.MaxValue,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = short.MaxValue,
                MechsDestroyed = 60,
                PlayTime = 649800,
                Saves = short.MaxValue,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //radar can be used
            }
        };
        #endregion

        #region Hard Ranks
        public static readonly Rank FoxHard = new Rank
        {
            Name = "Fox",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = 750,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 8,
                Shots = 700,
                SpecialItems = 0
            }
        };

        public static readonly Rank DobermanHard = new Rank
        {
            Name = "Doberman",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 8,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 16,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //radar can be used
            }
        };

        public static readonly Rank HoundHard = new Rank
        {
            Name = "Hound",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 4,
                Continues = 0,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = 3,
                MechsDestroyed = 60,
                PlayTime = 648900,
                Saves = short.MaxValue,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //radar can be used
            }
        };
        #endregion

        #region Normal Ranks
        public static readonly Rank DobermanNormal = new Rank
        {
            Name = "Doberman",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = 1000,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 8,
                Shots = 700,
                SpecialItems = 0
            }
        };

        public static readonly Rank HoundNormal = new Rank
        {
            Name = "Hound",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = short.MaxValue,
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 16,
                Shots = short.MaxValue,
                SpecialItems = 0x2000 //radar can be used
            }
        };
        #endregion



        #region Easy Ranks
        public static readonly Rank HoundEasy = new Rank
        {
            Name = "Hound",
            MinimumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 0,
                Continues = 0,
                DamageTaken = 0,
                Kills = 0,
                MechsDestroyed = 0,
                PlayTime = 0,
                Rations = 0,
                Saves = 0,
                Shots = 0,
                SpecialItems = 0
            },
            MaximumStats = new MGS2MemoryManager.GameStats
            {
                Alerts = 3,
                Continues = 0,
                DamageTaken = 1500, //TODO: confirm
                Kills = 0,
                Rations = 0,
                MechsDestroyed = 60,
                PlayTime = 648000,
                Saves = 8,
                Shots = 700,
                SpecialItems = 0
            }
        };
        #endregion
    }

}
