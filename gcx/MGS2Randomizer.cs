using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class MGS2Randomizer
    {
        public static string GcxDirectory { get; private set; }
        public static string ResourceDirectory { get; private set; }
        private static DirectoryInfo ResourceSuperDirectory { get; set; }
        private static List<string> filesInDirectory;
        static GcxEditor gcxEditor = new GcxEditor();

        private static MGS2ItemSet _vanillaItems;
        private static MGS2ItemSet _randomizedItems;
        public static Random Randomizer { get; set; }
        private static int _seed;

        public MGS2Randomizer(string mgs2Directory, int seed = 0)
        {
            DirectoryInfo mgs2DirectoryInfo = new DirectoryInfo(mgs2Directory);
            DirectoryInfo gcxDirectory = new DirectoryInfo(mgs2DirectoryInfo.FullName + "\\assets\\gcx\\eu\\_bp");
            filesInDirectory = Directory.EnumerateFiles(gcxDirectory.FullName).ToList();
            ResourceSuperDirectory = new DirectoryInfo(mgs2DirectoryInfo.FullName + "\\eu\\stage");

            if (seed == 0)
            {
                _seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond).Next();
            }
            else
            {
                _seed = seed;
            }

            Randomizer = new Random(_seed);
            VanillaItems.BuildVanillaItems();
            BuildVanillaItemSet();
        }

        [Obsolete]
        public MGS2Randomizer(string gcxDirectory, string resourceDirectory, int seed = 0)
        {
            GcxDirectory = gcxDirectory;
            ResourceDirectory = resourceDirectory;
            ResourceSuperDirectory = new DirectoryInfo(ResourceDirectory);
            filesInDirectory = Directory.EnumerateFiles(GcxDirectory).ToList();
            if (seed == 0)
            {
                _seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond).Next();
            }
            else
            {
                _seed = seed;
            }

            Randomizer = new Random(_seed);
            VanillaItems.BuildVanillaItems();
            BuildVanillaItemSet();
        }

        private void BuildVanillaItemSet()
        {
            _vanillaItems = new MGS2ItemSet
            {
                TankerPart1 = new ItemSet(VanillaItems.TankerPart1),
                TankerPart2 = new ItemSet(VanillaItems.TankerPart2),
                TankerPart3 = new ItemSet(VanillaItems.TankerPart3),

                PlantSet1 = new ItemSet(VanillaItems.PlantSet1),
                PlantSet2 = new ItemSet(VanillaItems.PlantSet2),
                PlantSet3 = new ItemSet(VanillaItems.PlantSet3),
                PlantSet4 = new ItemSet(VanillaItems.PlantSet4),
                PlantSet5 = new ItemSet(VanillaItems.PlantSet5),
                PlantSet6 = new ItemSet(VanillaItems.PlantSet6),
                PlantSet7 = new ItemSet(VanillaItems.PlantSet7),
                PlantSet8 = new ItemSet(VanillaItems.PlantSet8),
                PlantSet9 = new ItemSet(VanillaItems.PlantSet9)
            };
        }

        public int RandomizeItemSpawns()
        {
            _randomizedItems = new MGS2ItemSet();

            //TODO: randomize it, yo
            List<KeyValuePair<Location, Item>> TankerSpawnsLeft = new List<KeyValuePair<Location, Item>>();
            foreach (var kvp in VanillaItems.TankerPart3.Entities)
            {
                TankerSpawnsLeft.Add(kvp);
            }

            int itemsAssigned = 0;
            while(TankerSpawnsLeft.Count> 0)
            {
                int randomNum = Randomizer.Next();
                int modValue = randomNum % TankerSpawnsLeft.Count;
                var randomChoice = TankerSpawnsLeft[modValue];
                
                //TODO: verify if this is correct... it feels very much not so
                if (itemsAssigned < VanillaItems.TankerPart1.Entities.Count)
                {
                    if(VanillaItems.TankerPart1.ItemsNeededToProgress.Contains(randomChoice.Value) && !VanillaItems.TankerPart1.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    {
                        //if the spawn being modified isn't a mandatory spawn and we're currently trying to assign an item needed to progress, skip
                        //these are entirely inconsequential at the moment, as the mandatory items are automatically given by the game
                        continue;
                    }
                    _randomizedItems.TankerPart1.Entities.Add(VanillaItems.TankerPart1.Entities.ElementAt(itemsAssigned).Key, randomChoice.Value);                    
                }
                else if (itemsAssigned < VanillaItems.TankerPart2.Entities.Count)
                {
                    if (VanillaItems.TankerPart2.ItemsNeededToProgress.Contains(randomChoice.Value) && !VanillaItems.TankerPart2.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    {
                        //if the spawn being modified isn't a mandatory spawn and we're currently trying to assign an item needed to progress, skip
                        //these are entirely inconsequential at the moment, as the mandatory items are automatically given by the game
                        continue;
                    }
                    _randomizedItems.TankerPart2.Entities.Add(VanillaItems.TankerPart2.Entities.ElementAt(itemsAssigned).Key, randomChoice.Value);
                }
                else
                {
                    if (VanillaItems.TankerPart3.ItemsNeededToProgress.Contains(randomChoice.Value) && !VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    {
                        //if the spawn being modified isn't a mandatory spawn and we're currently trying to assign an item needed to progress, skip
                        //these are entirely inconsequential at the moment, as the mandatory items are automatically given by the game
                        continue;
                    }
                    _randomizedItems.TankerPart3.Entities.Add(VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice.Value);
                }

                TankerSpawnsLeft.Remove(randomChoice);
                itemsAssigned++;
            }

            foreach(var entity in _randomizedItems.TankerPart1.Entities)
            {
                _randomizedItems.TankerPart2.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.TankerPart2.Entities)
            {
                _randomizedItems.TankerPart3.Entities.Add(entity.Key, entity.Value);
            }

            List<KeyValuePair<Location, Item>> PlantSpawns = new List<KeyValuePair<Location, Item>>();
            foreach(var kvp in VanillaItems.PlantSet9.Entities)
            {
                PlantSpawns.Add(kvp);
            }
            

            //if the itemset isn't logically sound, re-randomize.
            if(!VerifyItemSetLogicValidity(_randomizedItems))
            {
                Randomizer = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond);
                RandomizeItemSpawns();
            }

            return _seed;
        }

        private bool VerifyItemSetLogicValidity(MGS2ItemSet setToCheck)
        {
            return true;
            //TODO: check it, yo
            foreach (Item item in VanillaItems.TankerPart1.ItemsNeededToProgress) 
            {
                if (!setToCheck.TankerPart1.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.TankerPart2.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart2.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.TankerPart3.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart3.Entities.ContainsValue(item))
                    return false;
            }
            /*
            foreach (Item item in VanillaItems.PlantSet1.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet1.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet2.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet2.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet3.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet3.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet4.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet4.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet5.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet5.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet6.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet6.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet7.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet7.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet8.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet8.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in VanillaItems.PlantSet9.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet9.Entities.ContainsValue(item))
                    return false;
            }
            */
            return true;
        }

        public bool SaveRandomizationToDisk(List<DecodedProc> spawnerProcs, string gcxFile)
        {
            //TODO: clean this up and polish it off

            GcxEditor gcx_Editor = new GcxEditor();
            ProcEditor.InitializeEditor(spawnerProcs);
            //Technically, adding all procs is TOTAL overkill, but it's literally just 5.4KB. Unless something doesn't load as a result
            //I think this is the easiest and most straight-forward solution.
            AddAllProcs(gcx_Editor);
            foreach (KeyValuePair<Location, Item> spawn in _randomizedItems.TankerPart3.Entities)
            {
                ProcEditor.ModifySpawnProc(spawn.Key.SpawnId, spawn.Value.ProcId);
            }
            ProcEditor.SaveAutomatedChanges();
            byte[] newGcxBytes = gcx_Editor.BuildGcxFile();
            string date = $"{gcxFile}_custom.gcx";
            File.WriteAllBytes(date, newGcxBytes);
            return true;
        }

        private void AddAllProcs(GcxEditor gcx_Editor)
        {
            ProcSelector.GetAllProcs();

            foreach (DecodedProc proc in ProcSelector.ProcsToAdd)
            {
                gcx_Editor.InsertNewProcedureToFile(proc);
            }
        }
    }
}
