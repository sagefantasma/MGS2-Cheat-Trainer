using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC
{
    public class MGS2Randomizer
    {
        public class RandomizerException : Exception
        {
            public RandomizerException(string message) : base(message)
            {
            }
        }

        private DirectoryInfo ResourceSuperDirectory { get; set; }
        private DirectoryInfo OriginalGcxFilesDirectory { get; set; }
        private List<string> GcxFileDirectory { get; set; }
        static GcxEditor gcxEditor = new GcxEditor();

        private static MGS2ItemSet _vanillaItems;
        private static MGS2ItemSet _randomizedItems;
        public Random Randomizer { get; set; }
        public int Seed { get; set; }
        private readonly byte[] TankerWeaponArray = new byte[] { 0x39, 0x21, 0x80, 0x01, 0x5C };
        private readonly byte[] TankerInitializeWeaponsArray = new byte[] { 0x21, 0x80, 0x01, 0x5C };
        private readonly byte[] TankerInitializeItemsArray = new byte[] { 0x21, 0x80, 0x01, 0xEC };
        private readonly byte[] PlantWeaponArray = new byte[] { 0x39, 0x21, 0x80, 0x02, 0xAC };
        private readonly byte[] PlantItemArray = new byte[] { 0x39, 0x21, 0x80, 0x03, 0x3C };
        private readonly byte[] PlantInitializeWeaponArray = new byte[] { 0x21, 0x80, 0x02, 0xAC };
        private readonly byte[] PlantInitializeItemsArray = new byte[] { 0x21, 0x80, 0x03, 0x3C };
        private readonly int ItemIndexOffset = 6;
        private readonly int ItemCountOffset = 7;
        private readonly byte WeaponIndexBase = 0xBB; 
        private readonly byte ItemIndexBase = 0xBD; 

        public MGS2Randomizer(string mgs2Directory, int seed = 0)
        {
            if (Directory.Exists(mgs2Directory))
            {
                DirectoryInfo gcxDirectory = new DirectoryInfo(mgs2Directory + "\\assets\\gcx\\eu\\_bp");
                SaveOldFiles(gcxDirectory);
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
            public bool RandomizeStartingItems { get; set; }
            public bool RandomizeAutomaticRewards { get; set; }
            public bool RandomizeClaymores { get; set; }
            public bool RandomizeC4 { get; set; }
            public bool IncludeRations { get; set; }
            public bool AllWeaponsSpawnable { get; set; }
        }

        private void FixOneOffItems()
        {
            var thermalGoggleSpawn = _randomizedItems.TankerPart3.Entities.FirstOrDefault(x => x.Key.Name == "LeftLadder2" && x.Key.GcxFile == "w04a");
            _randomizedItems.TankerPart3.Entities[thermalGoggleSpawn.Key] = MGS2Items.Thermals;

            var m9SpawnA = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "LeftCage" && x.Key.GcxFile == "w12a");
            _randomizedItems.PlantSet10.Entities[m9SpawnA.Key] = MGS2Weapons.M9;

            var m9SpawnC = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "M9Room1" && x.Key.GcxFile == "w22a");
            _randomizedItems.PlantSet10.Entities[m9SpawnC.Key] = MGS2Weapons.M9;

            var box5Spawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "RightsideFastTravel" && x.Key.GcxFile == "w20a");
            _randomizedItems.PlantSet10.Entities[box5Spawn.Key] = MGS2Items.Box5;

            var mineDetectorSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "RoomAcrossNode3.1" && x.Key.GcxFile == "w22a");
            _randomizedItems.PlantSet10.Entities[mineDetectorSpawn.Key] = MGS2Items.MineDetector;

            var aks74uSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "LockerRoom1" && x.Key.GcxFile == "w24a");
            _randomizedItems.PlantSet10.Entities[aks74uSpawn.Key] = MGS2Weapons.Aks74u;

            var socomSuppressorSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "LockerRoom8" && x.Key.GcxFile == "w24a");
            _randomizedItems.PlantSet10.Entities[socomSuppressorSpawn.Key] = MGS2Items.SocomSupp;

            var thermalGogglePlantSpawnA = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "Podium" && x.Key.GcxFile == "w24c");
            _randomizedItems.PlantSet10.Entities[thermalGogglePlantSpawnA.Key] = MGS2Items.Thermals;

            var akSuppSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "BehindFire" && x.Key.GcxFile == "w25b");
            _randomizedItems.PlantSet10.Entities[akSuppSpawn.Key] = MGS2Items.AkSupp;

            var m4Spawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "RightsideStairs" && x.Key.GcxFile == "w31a");
            _randomizedItems.PlantSet10.Entities[m4Spawn.Key] = MGS2Weapons.M4;

            var rgb6Spawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "RightsideAlcove2" && x.Key.GcxFile == "w31a");
            _randomizedItems.PlantSet10.Entities[rgb6Spawn.Key] = MGS2Weapons.Rgb6;

            var nikitaSpawnA = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "NikitaSpawn2" && x.Key.GcxFile == "w31b");
            _randomizedItems.PlantSet10.Entities[nikitaSpawnA.Key] = MGS2Weapons.Nikita;

            var nikitaSpawnB = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "NikitaSpawn3" && x.Key.GcxFile == "w31b");
            _randomizedItems.PlantSet10.Entities[nikitaSpawnB.Key] = MGS2Weapons.Nikita;

            var psg1tSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "CollapsedRoom1" && x.Key.GcxFile == "w31b");
            _randomizedItems.PlantSet10.Entities[psg1tSpawn.Key] = MGS2Weapons.Psg1t;

            var rgb6SpawnB = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "AirPocket2" && x.Key.GcxFile == "w31b");
            _randomizedItems.PlantSet10.Entities[rgb6SpawnB.Key] = MGS2Weapons.Rgb6;

            var thermalGogglePlantSpawnB = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "Locker4" && x.Key.GcxFile == "w31c");
            _randomizedItems.PlantSet10.Entities[thermalGogglePlantSpawnB.Key] = MGS2Items.Thermals;

            var box5SpawnB = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "LeftCatwalk" && x.Key.GcxFile == "w42a");
            _randomizedItems.PlantSet10.Entities[box5SpawnB.Key] = MGS2Items.Box5;

            var coldMedsSpawn = _randomizedItems.PlantSet10.Entities.FirstOrDefault(x => x.Key.Name == "BackLeftMainArea" && x.Key.GcxFile == "w42a");
            _randomizedItems.PlantSet10.Entities[coldMedsSpawn.Key] = MGS2Items.ColdMeds;
        }

        private void RandomizeClaymores()
        {
            //TODO: we should really make this a Polygon instead of just a box, so we can include more area :)
            //and also make sure the claymore positions somewhat make sense(maybe a north polygon and south polygon?)
            int leftWall = 0xBF68;
            //uint topWall = 0xFFFF0218;
            int topWall = 0x0218;
            int rightWall = 0xD6D8;
            //uint bottomWall = 0xFFFF2928;
            int bottomWall = 0x2928;

            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w21a"));
            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            List<int> claymores = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x85, 0xD6, 0x78 });

            foreach(int claymore in claymores)
            {
                int xPos = Randomizer.Next(leftWall, rightWall);
                int yPos = Randomizer.Next(topWall, bottomWall);

                Array.Copy(BitConverter.GetBytes(xPos), 0, gcxContents, claymore + 10, 2);
                Array.Copy(BitConverter.GetBytes(yPos), 0, gcxContents, claymore + 16, 2); //the FFFF should be untouched with this and still work
            }

            File.WriteAllBytes(gcxFile, gcxContents);
        }

        private void RandomizeStartingItems()
        {
            //TODO: finish implementation logic
            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_n_title"));
            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            
            //Snake starts with M9, so randomize that
            List<int> snakeWeaponAward = GcxEditor.FindAllSubArray(gcxContents, TankerInitializeWeaponsArray);
            RandomizedItem randomizedReward = GetRandomStartingItem(true);
            foreach(int weaponInitializiation in snakeWeaponAward)
            {
                gcxContents[weaponInitializiation + randomizedReward.Index - WeaponIndexBase] = randomizedReward.Count;
            }

            //Snake starts with AP Sensor, Camera, and cigs
            List<int> snakeItemAward = GcxEditor.FindAllSubArray(gcxContents, TankerInitializeItemsArray);
            while (snakeItemAward.Count > 5)
            {
                snakeItemAward.RemoveAt(snakeItemAward.Count - 1);
            }

            randomizedReward = GetRandomStartingItem(false);
            foreach (int initializationIndex in snakeItemAward)
            {
                gcxContents[initializationIndex + randomizedReward.Index - ItemIndexBase] = randomizedReward.Count;
            }
            randomizedReward = GetRandomStartingItem(false);
            foreach (int initializationIndex in snakeItemAward)
            {
                gcxContents[initializationIndex + randomizedReward.Index - ItemIndexBase] = randomizedReward.Count;
            }
            randomizedReward = GetRandomStartingItem(false);
            foreach (int initializationIndex in snakeItemAward)
            {
                gcxContents[initializationIndex + randomizedReward.Index - ItemIndexBase] = randomizedReward.Count;
            }

            //Raiden only starts with the AP sensor and Scope, so randomize those
            List<int> raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantInitializeItemsArray);
            while(raidenItemAward.Count > 6)
            {
                raidenItemAward.RemoveAt(raidenItemAward.Count - 1);
            }

            randomizedReward = GetRandomStartingItem(false);
            foreach (int initializationIndex in raidenItemAward)
            {
                gcxContents[initializationIndex + randomizedReward.Index - ItemIndexBase] = randomizedReward.Count;
            }
            randomizedReward = GetRandomStartingItem(false);
            foreach (int initializationIndex in raidenItemAward)
            {
                gcxContents[initializationIndex + randomizedReward.Index - ItemIndexBase] = randomizedReward.Count;
            }

            File.WriteAllBytes(gcxFile, gcxContents);
        }

        class RandomizedItem
        {
            public byte Index;
            public byte Count;
        }

        private RandomizedItem GetRandomStartingItem(bool isWeapon = false)
        {
            //TODO: finish implementation logic
            RandomizedItem randomizedItem = new RandomizedItem();

            if (isWeapon)
            {
                //just very basic now, start with either M9 or USP
                if(Randomizer.Next() % 2 == 0)
                { 
                    randomizedItem.Index = 0xC2;
                    randomizedItem.Count = 0x0E;
                }
                else
                {
                    randomizedItem.Index = 0xC3;
                    randomizedItem.Count = 0x0C;
                }
            }
            else
            {
                //for simplification at the moment, it needs to be things available to both Snake and Raiden(or at least wont crash the game)
                //Ration, Cold Meds, Bandage, Pentazemin, Thermals, Cigs, Box 1, Digital Camera, AP Sensor
                //USP Suppressor, SOCOM Suppressor, AK Suppressor, Stealth, Phone
                //these are also a bit more complicated than I was thinking at first. Going to once again need to account for the var references.
                switch (Randomizer.Next(0, 13))
                {
                    case 0:
                        //Ration
                        randomizedItem.Index = 0xC2;
                        break;
                    case 1:
                        //Cold meds
                        randomizedItem.Index = 0xC4;
                        break;
                    case 2:
                        //Bandage
                        randomizedItem.Index = 0xC5;
                        break;
                    case 3:
                        //Pentazemin
                        randomizedItem.Index = 0xC6;
                        break;
                    case 4:
                        //Thermals
                        randomizedItem.Index = 0xCE;
                        break;
                    case 5:
                        //Cigs
                        randomizedItem.Index = 0xD2;
                        break;
                    case 6:
                        //Box1
                        randomizedItem.Index = 0xD1;
                        break;
                    case 7:
                        //Digital Camera
                        randomizedItem.Index = 0xD0;
                        break;
                    case 8:
                        //AP Sensor
                        randomizedItem.Index = 0xDA;
                        break;
                    case 9:
                        //USP Suppressor
                        randomizedItem.Index = 0xE4;
                        break;
                    case 10:
                        //SOCOM suppressor
                        randomizedItem.Index = 0xDE;
                        break;
                    case 11:
                        //AK Suppressor
                        randomizedItem.Index = 0xDF;
                        break;
                    case 12:
                        //Stealth
                        randomizedItem.Index = 0xC9;
                        break;
                    case 13:
                        //Phone
                        randomizedItem.Index = 0xD5;
                        break;
                }
                randomizedItem.Count = 0x01;
            }

            return randomizedItem;
        }

        private RandomizedItem GetRandomItem(bool isWeapon = false, bool isPlant = true)
        {
            //TODO: the logic
            RandomizedItem randomizedItem = new RandomizedItem();

            if (isPlant)
            {

            }
            else
            {

            }

            return randomizedItem;
        }

        private void RandomizeAutomaticRewards()
        {
            //TODO: test and confirm
            //Olga USP
            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w00c"));
            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            List<int> snakeWeaponAward = GcxEditor.FindAllSubArray(gcxContents, TankerWeaponArray);

            RandomizedItem randomizedReward = GetRandomItem(true, false);
            gcxContents[snakeWeaponAward[0]+ItemIndexOffset] = randomizedReward.Index;
            gcxContents[snakeWeaponAward[0]+ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);


            //Pliskin USP & Cigs
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w14a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            List<int> raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);

            randomizedReward = GetRandomItem(true);
            gcxContents[raidenWeaponAward[2] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[2] + ItemCountOffset] = randomizedReward.Count;
            gcxContents[raidenWeaponAward[3] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[3] + ItemCountOffset] = randomizedReward.Count;

            List<int> raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //Stillman Card 1, Sensor A & Coolant Spray
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);

            randomizedReward = GetRandomItem(true);
            gcxContents[raidenWeaponAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[0] + ItemCountOffset] = randomizedReward.Count;

            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //Ninja Card 2, BDU & Phone
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20d"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward.Count;

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //Ames Card 3
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w24b"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //President Card 4 & MO Disk
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w31a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //Emma Card 5
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w25d"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //Snake HF Blade
            //TODO: implement
        }

        private void RandomizeC4Locations()
        {

        }

        public void Derandomize()
        {
            //So things are going to get EXTREMELY hairy if we try to "derandomize" things manually,
            //or once we have more options if we try to randomize on top of an already randomized gcx
            //To save myself a TON of work, instead we will copy the gcx files that will be modified into
            //a new subfolder, and push/pull from there to make this a simpler process.
            foreach(FileInfo file in OriginalGcxFilesDirectory.GetFiles())
            {
                file.CopyTo(Path.Combine(OriginalGcxFilesDirectory.Parent.FullName, file.Name), true);
            }
        }

        private void SaveOldFiles(DirectoryInfo gcxDirectory)
        {
            OriginalGcxFilesDirectory = gcxDirectory.CreateSubdirectory("originalGcxFiles");

            try
            {
                foreach (FileInfo file in gcxDirectory.GetFiles())
                {
                    file.CopyTo(Path.Combine(OriginalGcxFilesDirectory.FullName, file.Name));
                }
            }
            catch(IOException ioe)
            {
                if(ioe.Message.Contains("already exists"))
                {
                    //This error means we already have a back-up, so we're safe.
                    return;
                }
                MessageBox.Show("Something went wrong when trying to initialize the randomizer, please use Steam to Verify integrity of game files before trying again.");
            }
            catch
            {
                MessageBox.Show("Something went wrong when trying to initialize the randomizer, please use Steam to Verify integrity of game files before trying again.");
            }
        }

        public int RandomizeItemSpawns(RandomizationOptions options)
        {
            Derandomize(); //return to a "base" state to make our lives easier.
            _randomizedItems = new MGS2ItemSet();

            if (options.RandomizeStartingItems)
            {
                RandomizeStartingItems();
            }
            if (options.RandomizeAutomaticRewards)
            {
                RandomizeAutomaticRewards();
            }
            if (options.RandomizeClaymores)
            {
                RandomizeClaymores();
            }
            if (options.RandomizeC4)
            {
                RandomizeC4Locations();
            }

            //Create a list of all spawns on the tanker chapter
            List<Item> TankerSpawnsLeft = new List<Item>();
            foreach (var kvp in VanillaItems.TankerPart3.Entities)
            {
                if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                    continue;
                else
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
                if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                    continue;
                else
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

                if (new[] {"M9", "RGB-6", "M4", "PSG1-T"}.Contains(randomChoice.Name) && options.AllWeaponsSpawnable && VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn == false)
                {
                    retries--;
                    if (retries == 0)
                        break;
                    continue;
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
