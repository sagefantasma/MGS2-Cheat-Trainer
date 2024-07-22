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
        private static List<string> filesInDirectory;
        static GcxEditor gcxEditor = new GcxEditor();

        private static MGS2ItemSet _vanillaItems;
        private static MGS2ItemSet _randomizedItems;
        public static Random Randomizer { get; set; }
        private static int _seed;

        public MGS2Randomizer(string gcxDirectory, int seed = 0)
        {
            GcxDirectory = gcxDirectory;
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
                    _randomizedItems.TankerPart1.Entities.Add(VanillaItems.TankerPart1.Entities.ElementAt(itemsAssigned).Key, randomChoice.Value);                    
                }
                else if (itemsAssigned < VanillaItems.TankerPart2.Entities.Count)
                {
                    _randomizedItems.TankerPart2.Entities.Add(VanillaItems.TankerPart2.Entities.ElementAt(itemsAssigned).Key, randomChoice.Value);
                }
                else
                {
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

        private static bool FindAndReplaceCalledProcInGcx(Location locationToFind, Item replacingItem)
        {
            string desiredGcxFile = filesInDirectory.Find(file => file.EndsWith($"scenerio_stage_{locationToFind.GcxFile}.gcx"));
            byte[] gcxFileContents = File.ReadAllBytes(desiredGcxFile);

            for(int i = 0; i < gcxFileContents.Length - locationToFind.ParameterBytes.Count; i++)
            {
                for(int j = 0; j < locationToFind.ParameterBytes.Count; j++)
                {
                    if (gcxFileContents[i+j] != locationToFind.ParameterBytes[j])
                    {
                        break;
                    }
                    if(j > 2)
                    {
                        //okay, so we're only getting as far as 3 files BECAUSE: [w04a reference]
                        //calling m9 ammo drop(AF7E8A) the bytes are: 06 4935B8 01 08D5 01 D8DC 01 0CFE C1 000000 
                        //                                            06 F4B975 01 CE31 01 78EC 09 6678FEFF C1 000000
                        //we have MOST of what we're looking for in these, but there are some random bytes fucking shit up
                        //specifically, this 06, 01, 01, 01 C1 shit. the FUCK are these???? 
                        int a = 2 + 2;
                    }
                    if(j+1 ==  locationToFind.ParameterBytes.Count)
                    {
                        ReplaceProc(i - 3, replacingItem, desiredGcxFile);
                        return true;
                    }
                }
            }

            return false;
        }

        private static void ReplaceProc(int i, Item replacingItem, string gcxFileToModify)
        {
            using(BinaryWriter binWriter = new BinaryWriter(File.OpenWrite(gcxFileToModify)))
            {
                binWriter.Seek(i, SeekOrigin.Begin);
                binWriter.Write(replacingItem.ProcId.LittleEndianRepresentation);
                binWriter.Close();
            }
        }

        public bool SaveRandomizationToDisk()
        {
            foreach(var kvp in _randomizedItems.TankerPart3.Entities)
            {
                bool itWorked = FindAndReplaceCalledProcInGcx(kvp.Key, kvp.Value);
            }
            return true;
        }
    }
}
