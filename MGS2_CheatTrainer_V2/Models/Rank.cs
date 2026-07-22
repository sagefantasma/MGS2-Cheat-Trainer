using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MGS2_CheatTrainer_V2.Models
{
    //REWRITE STATUS: Not needed to update?
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
        public static Rank? CurrentlyProjectedRank(GameStats currentStats, Difficulty currentDifficulty, GameType gameType)
        {
            Rank? projectedRank = null;
            if (gameType != GameType.TankerPlant) //for now i'm only worrying about TankerPlant shared ranks on different difficulties
                return projectedRank;
            
            switch(currentDifficulty)
            {
                case Difficulty.EuropeanExtreme:
                case Difficulty.Extreme:
                    projectedRank = Mgs2ExtremeRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats));
                    break;
                case Difficulty.Hard:
                    projectedRank = Mgs2HardRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats));
                    break;
                case Difficulty.Normal:
                    projectedRank = Mgs2NormalRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats));
                    break;
                case Difficulty.Easy:
                case Difficulty.VeryEasy:
                    projectedRank = Mgs2EasyRanks.FirstOrDefault(rank => rank.AreStatsWithinRankRequirements(currentStats));
                    break;
            }

            return projectedRank;
        }

        private bool AreStatsWithinRankRequirements(GameStats stats)
        {
            foreach(FieldInfo member in typeof(GameStats).GetFields())
            {
                if(member.FieldType == typeof(short))
                {
                    if ((short)member.GetValue(stats)! < (short)member.GetValue(MinimumStats)!)
                        return false;
                    if ((short)member.GetValue(stats)! > (short)member.GetValue(MaximumStats)!)
                        return false;
                }
                else if(member.FieldType == typeof(int))
                {
                    if ((int)member.GetValue(stats)! < (int)member.GetValue(MinimumStats)!)
                        return false;
                    if ((int)member.GetValue(stats)! > (int)member.GetValue(MaximumStats)!)
                        return false;
                }
                
            }

            return true;
        }

        public string? Name { get; set; }
        public required GameStats MinimumStats { get; set; }
        public required GameStats MaximumStats { get; set; }

        //taken from: https://metalgear.fandom.com/wiki/Codename_(gameplay)#Requirements -- not the best source, but it'll do.
        private static readonly List<Rank> Mgs2ExtremeRanks =
        [
            RankRequirements.BigBoss, RankRequirements.FoxExtreme, RankRequirements.DobermanExtreme,
            RankRequirements.HoundExtreme
        ];
        private static readonly List<Rank> Mgs2HardRanks =
            [RankRequirements.FoxHard, RankRequirements.DobermanHard, RankRequirements.HoundHard];

        private static readonly List<Rank> Mgs2NormalRanks =
            [RankRequirements.DobermanNormal, RankRequirements.HoundNormal];
        private static readonly List<Rank> Mgs2EasyRanks = [RankRequirements.HoundEasy];
        private static readonly List<Rank> Mgs2DifficultyAgnosticRanks = []; //in case we ever decide to implement more
    }

    struct RankRequirements
    {
        

        #region Extreme Ranks
        public static Rank BigBoss = new Rank
        {
            Name = "Big Boss",
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
            MinimumStats = new GameStats
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
            MaximumStats = new GameStats
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
