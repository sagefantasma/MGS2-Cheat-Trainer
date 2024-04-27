using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC.Helpers
{
    internal class Rank
    {
        string Name;
        MGS2MemoryManager.GameStats MinimumStats;
        MGS2MemoryManager.GameStats MaximumStats;

        List<Rank> MGS2ExtremeRanks = new List<Rank>();
        List<Rank> MGS2HardRanks = new List<Rank>();
        List<Rank> MGS2NormalRanks = new List<Rank>();
        List<Rank> MGS2EasyRanks = new List<Rank>();
        List<Rank> MGS2DifficultyAgnosticRanks = new List<Rank>();

        #region Extreme Ranks
        private static readonly Rank BigBoss = new Rank
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

        private static readonly Rank FoxExtreme = new Rank
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
                SpecialItems = 0 //can use radar
            }
        };

        private static readonly Rank DobermanExtreme = new Rank
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
                SpecialItems = 0 //can use radar
            }
        };

        private static readonly Rank HoundExtreme = new Rank
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
                SpecialItems = 0 //radar can be used
            }
        };
        #endregion


        private static readonly Rank FoxHard = new Rank
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

        

        private static readonly Rank DobermanNormal = new Rank
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

        private static readonly Rank DobermanHard = new Rank
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
                SpecialItems = 0 //radar can be used
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
                SpecialItems = 0
            }
        };

        

        private static readonly Rank HoundEasy = new Rank
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

        private static readonly Rank HoundNormal = new Rank
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
                SpecialItems = 0 //radar can be used
            }
        };

        private static readonly Rank HoundHard = new Rank
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
                SpecialItems = 0 //radar can be used
            }
        };

        
    }

}
