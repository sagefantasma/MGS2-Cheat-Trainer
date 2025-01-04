using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gcx
{
    public class MGS2Randomizer
    {
        public class RandomizerException : Exception
        {
            public RandomizerException(string message) : base(message)
            {
            }
        }

        private static DirectoryInfo ResourceSuperDirectory { get; set; }
        private static List<string> GcxFileDirectory { get; set; }
        static GcxEditor gcxEditor = new GcxEditor();

        private static MGS2ItemSet _vanillaItems;
        private static MGS2ItemSet _randomizedItems;
        public Random Randomizer { get; set; }
        public int Seed { get; set; }

        public MGS2Randomizer(string mgs2Directory, int seed = 0)
        {
            if (Directory.Exists(mgs2Directory))
            {
                DirectoryInfo gcxDirectory = new DirectoryInfo(mgs2Directory + "\\assets\\gcx\\eu\\_bp");
                GcxFileDirectory = Directory.EnumerateFiles(gcxDirectory.FullName).ToList();
                ResourceSuperDirectory = new DirectoryInfo(mgs2Directory + "\\eu\\stage");

                if (seed == 0)
                {
                    Seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond).Next();
                }
                else
                {
                    Seed = seed;
                }

                Randomizer = new Random(Seed);
                BuildVanillaItemSet();
            }
            else
            {
                throw new DirectoryNotFoundException("Invalid directory provided, please provide the full path to your MGS2 install location.");
            }
        }

        private void BuildVanillaItemSet()
        {
            VanillaItems.BuildVanillaItems();

            _vanillaItems = new MGS2ItemSet
            {
                //0x30 spawns in tanker
                TankerPart1 = new ItemSet(VanillaItems.TankerPart1),
                TankerPart2 = new ItemSet(VanillaItems.TankerPart2),
                TankerPart3 = new ItemSet(VanillaItems.TankerPart3),
                 
                //0xd3 spawns in plant
                PlantSet1 = new ItemSet(VanillaItems.PlantSet1),
                PlantSet2 = new ItemSet(VanillaItems.PlantSet2),
                PlantSet3 = new ItemSet(VanillaItems.PlantSet3),
                PlantSet4 = new ItemSet(VanillaItems.PlantSet4),
                PlantSet5 = new ItemSet(VanillaItems.PlantSet5),
                PlantSet6 = new ItemSet(VanillaItems.PlantSet6),
                PlantSet7 = new ItemSet(VanillaItems.PlantSet7),
                PlantSet8 = new ItemSet(VanillaItems.PlantSet8),
                PlantSet9 = new ItemSet(VanillaItems.PlantSet9),
                PlantSet10 = new ItemSet(VanillaItems.PlantSet10)
            };
        }

        public class RandomizationOptions
        {
            public bool NoHardLogicLocks { get; set; }
            public bool NoSoftLogicLocks { get; set; }
        }

        public void DerandomizeItemSpawns()
        {

        }

        public int RandomizeItemSpawns(RandomizationOptions options)
        {
            //TODO: in the future, we should have something to randomize the "auto-awarded" items, and
            //also to have an option to not randomize optional spawns
            _randomizedItems = new MGS2ItemSet();

            //Create a list of all spawns on the tanker chapter
            List<Item> TankerSpawnsLeft = new List<Item>();
            foreach (var kvp in VanillaItems.TankerPart3.Entities)
            {
                TankerSpawnsLeft.Add(kvp.Value);
            }

            //assign each spawn on the tanker a random item from the list of available spawns
            int itemsAssigned = 0;
            while(TankerSpawnsLeft.Count> 0)
            {
                int randomNum = Randomizer.Next();
                int modValue = randomNum % TankerSpawnsLeft.Count;
                Item randomChoice = TankerSpawnsLeft[modValue];

                if (options.NoHardLogicLocks && 
                    LogicRequirements.ProgressionItems.Contains(randomChoice.Name) && 
                    !VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    continue;

                //iteratively go through spawns in "sequential" order, setting random items to each
                if (itemsAssigned < VanillaItems.TankerPart1.Entities.Count)
                {
                    _randomizedItems.TankerPart1.Entities.Add(VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);                    
                }
                else if (itemsAssigned < VanillaItems.TankerPart2.Entities.Count)
                {
                    _randomizedItems.TankerPart2.Entities.Add(VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else
                {
                    _randomizedItems.TankerPart3.Entities.Add(VanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);
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


            List<Item> PlantSpawns = new List<Item>();
            foreach(var kvp in VanillaItems.PlantSet10.Entities)
            {
                PlantSpawns.Add(kvp.Value);
            }

            itemsAssigned = 0;
            int retries = 10;
            while (PlantSpawns.Count > 0)
            {
                int randomNum = Randomizer.Next();
                int modValue = randomNum % PlantSpawns.Count;
                Item randomChoice = PlantSpawns[modValue];

                if (options.NoHardLogicLocks &&
                    LogicRequirements.ProgressionItems.Contains(randomChoice.Name) &&
                    !VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                {
                    retries--;
                    if (retries == 0)
                        break;
                    continue;
                }

                if(randomChoice.Name == "Nikita" && options.NoSoftLogicLocks)
                {
                    //currently, only the Nikita can cause a soft logic lock if the spawn is not in Shell 2
                    if(!(new[] { "w31a", "w31b" }.Contains(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.GcxFile)))
                    {
                        retries--;
                        if (retries == 0)
                            break;
                        continue;
                    }
                }

                //iteratively go through spawns in "sequential" order, setting random items to each
                if (itemsAssigned < VanillaItems.PlantSet1.Entities.Count)
                {
                    /*if (randomChoice.Name == "Sensor B" && options.NoHardLogicLocks)
                    {
                        //if people report this issue, I'll create a new option for "reduce crash risk" and include this and others reported
                        //getting the Sensor B before meeting Stillman crashes the game(sometimes?)
                        continue;
                    }*/
                    _randomizedItems.PlantSet1.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet2.Entities.Count)
                {
                    /*if (randomChoice.Name == "Sensor B" && options.NoHardLogicLocks)
                    {
                        //if people report this issue, I'll create a new option for "reduce crash risk" and include this and others reported
                        //getting the Sensor B before meeting Stillman crashes the game(sometimes?)
                        continue;
                    }*/
                    _randomizedItems.PlantSet2.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet3.Entities.Count)
                {
                    _randomizedItems.PlantSet3.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet4.Entities.Count)
                {
                    _randomizedItems.PlantSet4.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet5.Entities.Count)
                {
                    _randomizedItems.PlantSet5.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet6.Entities.Count)
                {
                    _randomizedItems.PlantSet6.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet7.Entities.Count)
                {
                    _randomizedItems.PlantSet7.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet8.Entities.Count)
                {
                    _randomizedItems.PlantSet8.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else if (itemsAssigned < VanillaItems.PlantSet9.Entities.Count)
                {
                    _randomizedItems.PlantSet9.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }
                else
                {
                    _randomizedItems.PlantSet10.Entities.Add(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                }

                PlantSpawns.Remove(randomChoice);
                itemsAssigned++;
            }

            foreach (var entity in _randomizedItems.PlantSet1.Entities)
            {
                _randomizedItems.PlantSet2.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet2.Entities)
            {
                _randomizedItems.PlantSet3.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet3.Entities)
            {
                _randomizedItems.PlantSet4.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet4.Entities)
            {
                _randomizedItems.PlantSet5.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet5.Entities)
            {
                _randomizedItems.PlantSet6.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet6.Entities)
            {
                _randomizedItems.PlantSet7.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet7.Entities)
            {
                _randomizedItems.PlantSet8.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet8.Entities)
            {
                _randomizedItems.PlantSet9.Entities.Add(entity.Key, entity.Value);
            }
            foreach (var entity in _randomizedItems.PlantSet9.Entities)
            {
                _randomizedItems.PlantSet10.Entities.Add(entity.Key, entity.Value);
            }

            //if the itemset isn't logically sound, re-randomize.
            if (!VerifyItemSetLogicValidity(_randomizedItems))
            {
                throw new RandomizerException("bad randomization seed");
            }

            return Seed;
        }

        private bool VerifyItemSetLogicValidity(MGS2ItemSet setToCheck)
        {
            /* For now, the Tanker is always completeable. When we figure out auto-given items, this will be needed
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
            */

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
            return true;
        }

        class OpenedFileData
        {
            public GcxEditor GcxEditor { get; set; }
            public List<DecodedProc> DecodedProcs { get; set; }
            public ProcEditor ProcEditor { get; set; }
        }

        public static bool ContainsSpawningFunctions(DecodedProc func)
        {
            List<string> spawningFunctions = new List<string>();
            foreach (RawProc spawningFunc in KnownProc.SpawnProcs)
            {
                spawningFunctions.Add(spawningFunc.BigEndianRepresentation);
            }
            return spawningFunctions.Any(function => func.DecodedContents.Contains(function));
        }

        public bool SaveRandomizationToDisk(bool makeSpoilerFile = true, bool customDirectory = true)
        {
            AddAllResources();
             
            //since some levels are part of multiple different logic sets,
            //we should instead go spawn by spawn rather than file by file
            Dictionary<string, OpenedFileData> openedFiles = new Dictionary<string, OpenedFileData>();
            string cheatSheet = "";
            foreach(KeyValuePair<Location, Item> spawnToEdit in _randomizedItems.TankerPart3.Entities)
            {
                string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_{spawnToEdit.Key.GcxFile}"));
                GcxEditor gcx_Editor;
                List<DecodedProc> spawns;
                ProcEditor procEditor;
                if (!openedFiles.ContainsKey(spawnToEdit.Key.GcxFile))
                {
                    gcx_Editor = new GcxEditor();
                    gcx_Editor.CallDecompiler(gcxFile);
                    List<DecodedProc> allFileFunctions = gcx_Editor.BuildContentTree();
                    spawns = new List<DecodedProc>();
                    foreach (DecodedProc entry in allFileFunctions)
                    {
                        if (ContainsSpawningFunctions(entry))
                            spawns.Add(entry);
                    }
                    AddAllProcs(gcx_Editor);
                    procEditor = new ProcEditor(spawns, true);
                    openedFiles.Add(spawnToEdit.Key.GcxFile, new OpenedFileData { GcxEditor = gcx_Editor, DecodedProcs = spawns, ProcEditor = procEditor });
                }
                else
                {
                    OpenedFileData openedFileData = openedFiles[spawnToEdit.Key.GcxFile];
                    gcx_Editor = openedFileData.GcxEditor;
                    spawns = openedFileData.DecodedProcs;
                    procEditor = openedFileData.ProcEditor;
                }
                
                cheatSheet += $"{spawnToEdit.Key.GcxFile}({MGS2Levels.MainGameStages.PlayableStageList.FirstOrDefault(x=>x.AreaCode == spawnToEdit.Key.GcxFile).Name}): {spawnToEdit.Key.Name} now has a {spawnToEdit.Value.Name}\n";
                procEditor.ModifySpawnProc(spawnToEdit.Key.SpawnId, spawnToEdit.Value.ProcId);
                procEditor.SaveAutomatedChanges();
                if(spawnToEdit.Key.SisterSpawn != null)
                {
                    //TODO: implement sister spawn duplication better
                    gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_{spawnToEdit.Key.SisterSpawn}"));
                    if (!openedFiles.ContainsKey(spawnToEdit.Key.SisterSpawn))
                    {
                        gcx_Editor = new GcxEditor();
                        gcx_Editor.CallDecompiler(gcxFile);
                        List<DecodedProc> allFileFunctions = gcx_Editor.BuildContentTree();
                        spawns = new List<DecodedProc>();
                        foreach (DecodedProc entry in allFileFunctions)
                        {
                            if (ContainsSpawningFunctions(entry))
                                spawns.Add(entry);
                        }
                        AddAllProcs(gcx_Editor);
                        procEditor = new ProcEditor(spawns, true);
                        openedFiles.Add(spawnToEdit.Key.SisterSpawn, new OpenedFileData { GcxEditor = gcx_Editor, DecodedProcs = spawns, ProcEditor = procEditor });
                    }
                    else
                    {
                        OpenedFileData openedFileData = openedFiles[spawnToEdit.Key.SisterSpawn];
                        gcx_Editor = openedFileData.GcxEditor;
                        spawns = openedFileData.DecodedProcs;
                        procEditor = openedFileData.ProcEditor;
                    }

                    cheatSheet += $"{spawnToEdit.Key.SisterSpawn}({MGS2Levels.MainGameStages.PlayableStageList.FirstOrDefault(x => x.AreaCode == spawnToEdit.Key.GcxFile).Name}): {spawnToEdit.Key.Name} now has a  {spawnToEdit.Value.Name}\n";
                    procEditor.ModifySpawnProc(spawnToEdit.Key.SpawnId, spawnToEdit.Value.ProcId);
                    procEditor.SaveAutomatedChanges();
                }
            }

            foreach (KeyValuePair<Location, Item> spawnToEdit in _randomizedItems.PlantSet10.Entities)
            {
                string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_{spawnToEdit.Key.GcxFile}"));
                GcxEditor gcx_Editor;
                List<DecodedProc> spawns;
                ProcEditor procEditor;
                if (!openedFiles.ContainsKey(spawnToEdit.Key.GcxFile))
                {
                    gcx_Editor = new GcxEditor();
                    gcx_Editor.CallDecompiler(gcxFile);
                    List<DecodedProc> allFileFunctions = gcx_Editor.BuildContentTree();
                    spawns = new List<DecodedProc>();
                    foreach (DecodedProc entry in allFileFunctions)
                    {
                        if (ContainsSpawningFunctions(entry))
                            spawns.Add(entry);
                    }
                    AddAllProcs(gcx_Editor);
                    procEditor = new ProcEditor(spawns, true);
                    openedFiles.Add(spawnToEdit.Key.GcxFile, new OpenedFileData { GcxEditor = gcx_Editor, DecodedProcs = spawns, ProcEditor = procEditor });
                }
                else
                {
                    OpenedFileData openedFileData = openedFiles[spawnToEdit.Key.GcxFile];
                    gcx_Editor = openedFileData.GcxEditor;
                    spawns = openedFileData.DecodedProcs;
                    procEditor = openedFileData.ProcEditor;
                }

                cheatSheet += $"{spawnToEdit.Key.GcxFile}({MGS2Levels.MainGameStages.PlayableStageList.FirstOrDefault(x => x.AreaCode == spawnToEdit.Key.GcxFile).Name}): {spawnToEdit.Key.Name} now has a {spawnToEdit.Value.Name}\n";
                procEditor.ModifySpawnProc(spawnToEdit.Key.SpawnId, spawnToEdit.Value.ProcId);
                procEditor.SaveAutomatedChanges();
                if (spawnToEdit.Key.SisterSpawn != null)
                {
                    //TODO: implement sister spawn duplication better
                    gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_{spawnToEdit.Key.SisterSpawn}"));
                    if (!openedFiles.ContainsKey(spawnToEdit.Key.SisterSpawn))
                    {
                        gcx_Editor = new GcxEditor();
                        gcx_Editor.CallDecompiler(gcxFile);
                        List<DecodedProc> allFileFunctions = gcx_Editor.BuildContentTree();
                        spawns = new List<DecodedProc>();
                        foreach (DecodedProc entry in allFileFunctions)
                        {
                            if (ContainsSpawningFunctions(entry))
                                spawns.Add(entry);
                        }
                        AddAllProcs(gcx_Editor);
                        procEditor = new ProcEditor(spawns, true);
                        openedFiles.Add(spawnToEdit.Key.SisterSpawn, new OpenedFileData { GcxEditor = gcx_Editor, DecodedProcs = spawns, ProcEditor = procEditor });
                    }
                    else
                    {
                        OpenedFileData openedFileData = openedFiles[spawnToEdit.Key.SisterSpawn];
                        gcx_Editor = openedFileData.GcxEditor;
                        spawns = openedFileData.DecodedProcs;
                        procEditor = openedFileData.ProcEditor;
                    }

                    cheatSheet += $"{spawnToEdit.Key.SisterSpawn}({MGS2Levels.MainGameStages.PlayableStageList.FirstOrDefault(x => x.AreaCode == spawnToEdit.Key.GcxFile).Name}): {spawnToEdit.Key.Name} now has a {spawnToEdit.Value.Name}\n";
                    procEditor.ModifySpawnProc(spawnToEdit.Key.SpawnId, spawnToEdit.Value.ProcId);
                    procEditor.SaveAutomatedChanges();
                }
            }

            DirectoryInfo createdDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            if (customDirectory)
                createdDirectory = Directory.CreateDirectory($"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_randomizedGcxFiles");
            foreach (KeyValuePair<string, OpenedFileData> kvp in openedFiles)
            {
                OpenedFileData openedFileData = kvp.Value;
                byte[] newGcxBytes = openedFileData.GcxEditor.BuildGcxFile();
                string date = $"{createdDirectory.Name}/scenerio_stage_{kvp.Key}.gcx";
                if (customDirectory)
                    File.WriteAllBytes(date, newGcxBytes);
                else
                    File.WriteAllBytes(GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_{kvp.Key}")), newGcxBytes);
            }
            if(makeSpoilerFile)
                File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/spoiler_seed-{Seed}.txt", cheatSheet);
            return true;
        }

        private void AddAllResources()
        {
            List<string> strings = new List<string>();
            foreach (Resource value in Resource.ResourceList)
            {
                strings.Add(value.CommonName);
            }
            List<string> stages = new List<string> { "w00a", "w00b", "w00c", "w01a", "w01b", "w01c", "w01d", "w01e", "w01f",
            "w02a", "w03a", "w03b", "w04a", "w04b", "w04c", "w11a", "w11b", "w11c", "w12a", "w12b", "w12c", "w13a", "w13b",
            "w14a", "w15a", "w15b", "w16a", "w16b", "w17a", "w18a", "w19a", "w20a", "w20b", "w20c", "w20d", "w21a", "w21b",
            "w22a", "w23a", "w23b", "w24a", "w24b", "w24c", "w24d", "w24e", "w25a", "w25b", "w25c", "w25d", "w28a", "w31a",
            "w31b", "w31c", "w31d", "w31f", "w32a", "w32b", "w41a", "w42a", "w43a", "w44a", "w45a", "w46a", "w51a", "w61a"};
            foreach (string stage in stages)
                ResourceEditor.AddResources(stage, ResourceSuperDirectory.FullName, strings);
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
