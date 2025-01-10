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
        private string SpoilerContents = "";
        private int PlantSet2CardsRequired = 1;
        private int PlantSet3CardsRequired = 2;
        private int PlantSet6CardsRequired = 3;
        private int PlantSet7CardsRequired = 4;
        private int PlantSet9CardsRequired = 5;


        private static List<RandomizedItem> MasterRaidenItemAwardOptions = new List<RandomizedItem> {
            new RandomizedItem{Index = 1+0xC1, Count = 2+0xC1,Name = "Ration" }, new RandomizedItem{Index = 3+0xC1, Count = 1+0xC1, Name = "Cold Medicine" },
            new RandomizedItem{Index = 4+0xC1, Count = 5+0xC1, Name = "Bandages" },new RandomizedItem{Index = 5+0xC1, Count = 5+0xC1, Name = "Pentazemin" },
            new RandomizedItem{Index = 6+0xC1, Count = 1+0xC1, Name = "B.D.U" },new RandomizedItem{Index = 7 + 0xC1,Count = 1 + 0xC1, Name = "Body Armor" },
            new RandomizedItem{Index = 8+0xC1, Count = 1 + 0xC1, Name = "Stealth" },new RandomizedItem{Index = 9 + 0xC1, Count = 1 + 0xC1, Name = "Mine Detector" },
            new RandomizedItem{Index = 10+0xC1, Count = 1 + 0xC1, Name = "Sensor A" }, new RandomizedItem{Index = 11 + 0xC1, Count = 1 + 0xC1, Name = "Sensor B" },
            new RandomizedItem{Index = 12+0xC1, Count = 1 + 0xC1, Name = "N.V.G." }, new RandomizedItem{Index = 13 + 0xC1, Count = 1 + 0xC1, Name = "Thermal Goggles" },
            new RandomizedItem{Index = 14+0xC1, Count = 1 + 0xC1, Name = "Scope" }, new RandomizedItem{Index = 15 + 0xC1, Count = 1 + 0xC1, Name = "Digital Camera" },
            new RandomizedItem{Index = 16+0xC1, Count = 21 + 0xC1, Name = "Box 1" }, new RandomizedItem{Index = 17 + 0xC1, Count = 1 + 0xC1, Name = "Cigarettes" },
            /*new RandomizedItem{Index = 18+0xC1, Count = 1 + 0xC1, Name = "Card 1" },*/ new RandomizedItem{Index = 19 + 0xC1, Count = 1 + 0xC1, Name = "Shaver" },
            new RandomizedItem{Index = 20+0xC1, Count = 1 + 0xC1, Name = "Phone" }, new RandomizedItem{Index = 22 + 0xC1, Count = 21 + 0xC1, Name = "Box 2" },
            new RandomizedItem{Index = 23+0xC1, Count = 21 + 0xC1, Name = "Box 3" },new RandomizedItem{Index = 25 + 0xC1, Count = 1 + 0xC1, Name = "A.P. Sensor" },
            new RandomizedItem{Index = 26+0xC1, Count = 21 + 0xC1, Name = "Box 4" }, new RandomizedItem{Index = 27 + 0xC1, Count = 21 + 0xC1, Name = "Box 5" },
            new RandomizedItem{Index = 29+0xC1, Count = 1 + 0xC1, Name = "SOCOM Suppressor" }, new RandomizedItem{Index = 30 + 0xC1, Count = 1 + 0xC1, Name = "AK Suppressor" },
            new RandomizedItem{Index = 34+0xC1, Count = 1 + 0xC1, Name = "M.O. Disc" }, new RandomizedItem{Index = 36 + 0xC1, Count = 1 + 0xC1, Name = "Infinity Wig" },
            new RandomizedItem{Index = 37+0xC1, Count = 1 + 0xC1, Name = "Blue Wig" }, new RandomizedItem{Index = 38 + 0xC1, Count = 1 + 0xC1, Name = "Orange Wig" },
            /*new RandomizedItem{Index = 18+0xC1, Count = 2 + 0xC1, Name = "Card 2" }, new RandomizedItem{Index = 18 + 0xC1, Count = 3 + 0xC1, Name = "Card 3" },
            new RandomizedItem{Index = 18+0xC1, Count = 4+0xC1, Name = "Card 4" }, new RandomizedItem{Index = 18 + 0xC1, Count = 5 + 0xC1, Name = "Card 5" }*/ };
        private List<RandomizedItem> RaidenItemAwardOptions;
                            
        private static List<RandomizedItem> MasterRaidenWeaponAwardOptions = new List<RandomizedItem> {
            new RandomizedItem{Index = 3 + 0xC1, Count = 12 + 0xC1, Name= "SOCOM" }, new RandomizedItem{Index = 5 + 0xC1, Count = 10 + 0xC1, Name= "RGB6" },
            new RandomizedItem{Index = 7 + 0xC1,Count = 10 + 0xC1, Name = "Stinger" }, new RandomizedItem{Index = 14 + 0xC1,Count = 1 + 0xC1,Name = "Coolant" },
            new RandomizedItem{Index = 18 + 0xC1,Count = 60 + 0xC1,Name= "M4" }, new RandomizedItem{Index = 19 + 0xC1,Count = 20 + 0xC1,Name= "PSG1T" },
            new RandomizedItem{Index = 21 + 0xC1,Count = 5 + 0xC1, Name= "Book" }, new RandomizedItem{Index = 6 + 0xC1, Count = 10 + 0xC1, Name = "Nikita" },
            new RandomizedItem{Index = 1 + 0xC1,Count = 15 + 0xC1, Name= "M9" }, new RandomizedItem{Index = 4 + 0xC1, Count = 10 + 0xC1, Name = "PSG1" },
            new RandomizedItem{Index = 8 + 0xC1,Count = 5 + 0xC1, Name= "Claymore" }, new RandomizedItem{Index = 8 + 0xC1, Count = 5 + 0xC1, Name = "C4" },
            new RandomizedItem{Index = 10 + 0xC1,Count = 2 + 0xC1, Name= "Chaff Grenade" }, new RandomizedItem{Index = 11 + 0xC1, Count = 2 + 0xC1, Name = "Stun Grenade" },
            new RandomizedItem{Index = 12 + 0xC1,Count = 1 + 0xC1, Name= "Directional Microphone" }, new RandomizedItem{Index = 15 + 0xC1, Count = 60 + 0xC1, Name = "AKS-74u" },
            new RandomizedItem{Index = 16 + 0xC1,Count = 5 + 0xC1, Name= "Magazine" }, new RandomizedItem{Index = 17 + 0xC1, Count = 2 + 0xC1, Name = "Grenade" },
        };
        private List<RandomizedItem> RaidenWeaponAwardOptions;

        private static List<RandomizedItem> MasterSnakeItemAwardOptions = new List<RandomizedItem> { new RandomizedItem { Index = 1, Count = 2, Name = "Ration" },
            new RandomizedItem{Index = 3 + 0xC1, Count = 1 + 0xC1, Name = "Cold Medicine" },new RandomizedItem{Index = 4 + 0xC1, Count = 5 + 0xC1, Name = "Bandage" },
            new RandomizedItem{Index = 5 + 0xC1, Count = 5 + 0xC1, Name = "Pentazemin" }, new RandomizedItem{Index = 8 + 0xC1, Count = 1 + 0xC1, Name = "Stealth" },
            new RandomizedItem{Index = 9 + 0xC1, Count = 1 + 0xC1, Name = "Mine Detector" }, new RandomizedItem{Index = 13 + 0xC1, Count = 1 + 0xC1, Name = "Thermals" },
            new RandomizedItem{Index = 21 + 0xC1, Count = 1 + 0xC1, Name = "Camera" }, new RandomizedItem{Index = 15 + 0xC1, Count = 1 + 0xC1, Name = "Digital Camera" },
            new RandomizedItem{Index = 16 + 0xC1, Count = 21 + 0xC1, Name = "Box 1" }, new RandomizedItem{Index = 17 + 0xC1, Count = 1 + 0xC1, Name = "Cigarettes" },
            new RandomizedItem{Index = 19 + 0xC1, Count = 1 + 0xC1, Name = "Shaver" }, new RandomizedItem{Index = 25 + 0xC1, Count = 1 + 0xC1, Name = "A.P. Sensor" },
            new RandomizedItem{Index = 35 + 0xC1, Count = 1 + 0xC1, Name = "USP Suppressor" }, new RandomizedItem{Index = 32 + 0xC1, Count = 1 + 0xC1, Name = "Bandana" } };
        private List<RandomizedItem> SnakeItemAwardOptions;

        private static List<RandomizedItem> MasterSnakeWeaponAwardOptions = new List<RandomizedItem> {
            new RandomizedItem{Index = 2 + 0xC1, Count = 12 + 0xC1, Name= "USP" }, new RandomizedItem{Index = 1 + 0xC1, Count = 15 + 0xC1, Name= "M9" },
            new RandomizedItem{Index = 11 + 0xC1,Count = 4 + 0xC1, Name = "Stun Grenade" }, new RandomizedItem{Index = 10 + 0xC1,Count = 4 + 0xC1,Name = "Chaff Grenade" },
            new RandomizedItem{Index = 17 + 0xC1, Count = 4 + 0xC1, Name = "Grenade" }, new RandomizedItem{Index = 16 + 0xC1, Count = 10 + 0xC1, Name = "Magazine" } };
        private List<RandomizedItem> SnakeWeaponAwardOptions;

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
            public bool NikitaShell2 { get; set; }
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
            public string Name;
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
            //TODO: test and confirm
            RandomizedItem randomizedItem;

            if (isPlant)
            {
                if (isWeapon)
                {
                    int randomChoice = Randomizer.Next(RaidenWeaponAwardOptions.Count);
                    randomizedItem = RaidenWeaponAwardOptions[randomChoice];
                    RaidenWeaponAwardOptions.Remove(randomizedItem);
                }
                else
                {
                    int randomChoice = Randomizer.Next(RaidenItemAwardOptions.Count);
                    randomizedItem = RaidenItemAwardOptions[randomChoice];
                    RaidenItemAwardOptions.Remove(randomizedItem);
                }
                
            }
            else
            {
                if (isWeapon)
                {
                    int randomChoice = Randomizer.Next(SnakeWeaponAwardOptions.Count);
                    randomizedItem = SnakeWeaponAwardOptions[randomChoice];
                    SnakeWeaponAwardOptions.Remove(randomizedItem);
                }
                else
                {
                    int randomChoice = Randomizer.Next(SnakeItemAwardOptions.Count);
                    randomizedItem = SnakeItemAwardOptions[randomChoice];
                    SnakeItemAwardOptions.Remove(randomizedItem);
                }
            }

            return randomizedItem;
        }

        private void AddAutomaticRewardsToPools()
        {
            Location socomLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w12b" && spawn.Key.Name == "Locker1").Key;
            VanillaItems.PlantSet10.Entities[socomLocation] = MGS2Weapons.Socom;
            
            Location cigsLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w12a" && spawn.Key.Name == "RightCage").Key;
            VanillaItems.PlantSet10.Entities[cigsLocation] = MGS2Items.Cigs;
            
            Location sensorALocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w16a" && spawn.Key.Name == "LadiesRoom2").Key;
            VanillaItems.PlantSet10.Entities[sensorALocation] = MGS2Items.SensorA;
            
            Location coolantSprayLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w16a" && spawn.Key.Name == "MensRoom").Key;
            VanillaItems.PlantSet10.Entities[coolantSprayLocation] = MGS2Weapons.Coolant;
            
            Location bduLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w18a" && spawn.Key.Name == "UnderStairs").Key;
            VanillaItems.PlantSet10.Entities[bduLocation] = MGS2Items.BDU;
            
            Location phoneLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w20a" && spawn.Key.Name == "UnderConveyerBelt").Key;
            VanillaItems.PlantSet10.Entities[phoneLocation] = MGS2Items.Phone;

            Location moDiskLocation = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w31d" && spawn.Key.Name == "ElectricalRoom2").Key;
            VanillaItems.PlantSet10.Entities[moDiskLocation] = MGS2Items.MoDisc;

            /*Location card1Location = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w14a" && spawn.Key.Name == "Locker1").Key;
            VanillaItems.PlantSet10.Entities[card1Location] = MGS2Items.Card;
            
            Location card2Location = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w22a" && spawn.Key.Name == "LockerNearNode1").Key;
            VanillaItems.PlantSet10.Entities[card2Location] = MGS2Items.Card;
            
            Location card3Location = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w22a" && spawn.Key.Name == "C4Room2").Key;
            VanillaItems.PlantSet10.Entities[card3Location] = MGS2Items.Card;
            
            Location card4Location = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w31b" && spawn.Key.Name == "MiddleHallwayAlcove").Key;
            VanillaItems.PlantSet10.Entities[card4Location] = MGS2Items.Card;

            Location card5Location = VanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w31d" && spawn.Key.Name == "LeftsideAlcove").Key;
            VanillaItems.PlantSet10.Entities[card5Location] = MGS2Items.Card;*/
        }

        private void CheckAndRemoveFromRequirements(RandomizedItem item, ItemSet itemSetAdjusted)
        {
            Item itemToRemove;
            switch (itemSetAdjusted.Name)
            {
                //Pliskin cutscene affects all item sets
                //Stillman cutscene affects all item sets
                //Ninja cutscene affects BeforeShells, BeforeJohnson, Before Emma, Before Strut L
                //President cutscene affects Before Emma, Before Strut L
                //Emma affects before strut L
                case "Before Pliskin":
                case "Before Stillman":
                case "Before Fatman":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if(itemToRemove != null)
                    {
                        VanillaItems.PlantSet3.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet4.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Shell 1 Elevator":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet4.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Ames":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Shells Connecting Bridge":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Johnson":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Emma":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Strut L":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "After Strut L":
                    itemToRemove = VanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        VanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
            }
        }

        private string RandomizeAutomaticRewards()
        {
            /*
             * It would be very interesting to add Cards to the randomization pool, but unfortunately I just
             * don't have the structuring built for it. The way I built my pools relies on cutscenes not being by-passed
             * which means cards being awarded at them as well. I absolutely want to implement this at some point in 
             * the future, but for now I am commenting out everything related to card key randomization.
             * Randomizing card keys WILL require a restructuring of item sets.
             */
            //Insert automatic rewards into the spawning pools
            AddAutomaticRewardsToPools();
            #region Olga USP
            string spoiler = "";
            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w00c"));
            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            List<int> snakeWeaponAward = GcxEditor.FindAllSubArray(gcxContents, TankerWeaponArray);

            RandomizedItem randomizedReward = GetRandomItem(true, false);
            spoiler += $"Olga will give you {randomizedReward.Name} after defeating her.\n";
            gcxContents[snakeWeaponAward[0]+ItemIndexOffset] = randomizedReward.Index;
            gcxContents[snakeWeaponAward[0]+ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Pliskin SOCOM & Cigs
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w14a"));
            gcxContents = File.ReadAllBytes(gcxFile);

            List<int> raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);
            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet1);
            spoiler += $"Pliskin will give you {randomizedReward.Name} on Strut B.\n"; //working
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            List<int> raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);
            randomizedReward = GetRandomItem(true);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet1);
            spoiler += $"Pliskin will give you {randomizedReward.Name} on Strut B.\n"; //working
            gcxContents[raidenWeaponAward[2] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[2] + ItemCountOffset] = randomizedReward.Count;
            gcxContents[raidenWeaponAward[3] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[3] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //the weapon ALSO gets assigned during the cutscene, so update that while we're at it
            //5:24PM - 1/7/25 test: for some reason got 1 AK bullet? lmao
            //5:43PM - 1/7/25 test: got RGB6 and magazines?? waduheck something weird is happening here 
            //5:51 - 1 AK bullet again... need to figure this out
            //and now after changing nothing, it worked correctly??
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_d010p01"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);
            gcxContents[raidenWeaponAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[0] + ItemCountOffset] = randomizedReward.Count;
            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Stillman Card 1, Sensor A & Coolant Spray
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);
            randomizedReward = GetRandomItem(true);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet2);
            spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
            gcxContents[raidenWeaponAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[0] + ItemCountOffset] = randomizedReward.Count;

            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet2);
            spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            /*randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet2);
            spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            //looks like Card 1 gets actually set in w16b, so we will set it here as well
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16b"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;*/

            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Ninja Card 2, BDU & Phone
            //not working, gave everything normal on latest test
            //looks like d021p01 gives the phone[20,48]
            //still none of it working? huh?
            //d036p03, d036p05, wmovie(probably useless), d11t - not seeing anything in any of these
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20d")); //now all working :)
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            /*randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet4);
            spoiler += $"Cyborg Ninja will give you {randomizedReward.Name} on Strut E.\n";
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;*/

            RandomizedItem randomizedReward2 = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet4);
            spoiler += $"Cyborg Ninja will give you {randomizedReward2.Name} on Strut E.\n";
            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward2.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward2.Count;

            RandomizedItem randomizedReward3 = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet4);
            spoiler += $"Cyborg Ninja will give you {randomizedReward3.Name} on Strut E.\n";
            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);


            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20b"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);
            
            /*gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;*/

            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward2.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward2.Count;

            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20c"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            /*gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;*/

            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward2.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward2.Count;

            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);


            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_d021p01"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Ames Card 3
            //d036p03, w24e
            /*gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w24b"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet6);
            spoiler += $"Ames will give you {randomizedReward.Name} in the Hostage Room.\n"; //worked
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);*/
            #endregion

            #region President Card 4 & MO Disk
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w31a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            /*randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet8);
            spoiler += $"President Johnson will give you {randomizedReward.Name} on Shell 2.\n"; //worked
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;*/

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet8);
            spoiler += $"President Johnson will give you {randomizedReward.Name} on Shell 2.\n"; //worked
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Emma Card 5 
            //d036p01
            /*gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w25d"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, VanillaItems.PlantSet10);
            spoiler += $"Emma will give you {randomizedReward.Name} on the KL Connecting Bridge.\n"; //worked
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);*/
            #endregion

            //Snake HF Blade
            //TODO: implement

            return spoiler;
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

            SpoilerContents = "";
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
            RaidenItemAwardOptions = new List<RandomizedItem>();
            RaidenItemAwardOptions.AddRange(MasterRaidenItemAwardOptions);
            RaidenWeaponAwardOptions = new List<RandomizedItem>();
            RaidenWeaponAwardOptions.AddRange(MasterRaidenWeaponAwardOptions);
            SnakeItemAwardOptions = new List<RandomizedItem>();
            SnakeItemAwardOptions.AddRange(MasterSnakeItemAwardOptions);
            SnakeWeaponAwardOptions = new List<RandomizedItem>();
            SnakeWeaponAwardOptions.AddRange(MasterSnakeWeaponAwardOptions);
            _randomizedItems = new MGS2ItemSet();

            if (options.RandomizeStartingItems)
            {
                RandomizeStartingItems();
            }
            if (options.RandomizeAutomaticRewards)
            {
                SpoilerContents = RandomizeAutomaticRewards();
            }
            if (options.RandomizeClaymores)
            {
                RandomizeClaymores();
            }
            if (options.RandomizeC4)
            {
                RandomizeC4Locations();
            }

            try
            {
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
                while (TankerSpawnsLeft.Count > 0)
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

                foreach (var entity in _randomizedItems.TankerPart1.Entities)
                {
                    _randomizedItems.TankerPart2.Entities.Add(entity.Key, entity.Value);
                }
                foreach (var entity in _randomizedItems.TankerPart2.Entities)
                {
                    _randomizedItems.TankerPart3.Entities.Add(entity.Key, entity.Value);
                }
            }
            catch (Exception ex)
            {

            }


            try
            {
                List<Item> PlantSpawns = new List<Item>();
                foreach (var kvp in VanillaItems.PlantSet10.Entities)
                {
                    if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                        continue;
                    else
                        PlantSpawns.Add(kvp.Value);
                }

                int itemsAssigned = 0;
                int retries = 1000;
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

                    if(options.RandomizeAutomaticRewards 
                        && LogicRequirements.AutoAwardedProgressionItems.Contains(randomChoice.Name) &&
                        !VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    {
                        retries--;
                        if (retries == 0)
                            break;
                        continue;
                    }

                    if (randomChoice.Name == "Nikita" && options.NikitaShell2)
                    {
                        //currently, only the Nikita can cause a soft logic lock if the spawn is not in Shell 2
                        if (!(new[] { "w31a", "w31b" }.Contains(VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.GcxFile)))
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }
                    }

                    if (new[] { "M9", "RGB-6", "M4", "PSG1-T" }.Contains(randomChoice.Name) && options.AllWeaponsSpawnable && VanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn == false)
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
            }
            catch (Exception ex)
            {

            }

            /* Removed from code execution as we are not currently adding card spawns to the item pool.
            if (options.RandomizeAutomaticRewards && !VerifyCardSpawns(_randomizedItems))
            {
                throw new RandomizerException("bad randomization seed");
            }
            

            //TODO: verify this
            if (options.RandomizeAutomaticRewards && options.NoHardLogicLocks)
            {
                List<KeyValuePair<Location, Item>> spawnsOnShell2BeforeEmma = _randomizedItems.PlantSet10.Entities.Where(stages => new[] { "w31a", "w31b" }.Contains(stages.Key.GcxFile)).ToList();
                List<KeyValuePair<Location, Item>> spawnsOnShell2BeforeCard5Door = _randomizedItems.PlantSet10.Entities.Where(stages => new[] { "w31a", "w31b", "w31c", "w25d", "w31d" }.Contains(stages.Key.GcxFile)).ToList();

                if(!spawnsOnShell2BeforeEmma.Any(spawn => spawn.Value == MGS2Items.Card) || spawnsOnShell2BeforeCard5Door.Count(spawn => spawn.Value == MGS2Items.Card) != 2)
                {
                    throw new RandomizerException("bad randomization seed");
                }
            }*/

            //if the itemset isn't logically sound, re-randomize.
            if (!VerifyItemSetLogicValidity(_randomizedItems))
            {
                throw new RandomizerException("bad randomization seed");
            }
            
            return Seed;
        }

        private bool VerifyCardSpawns(MGS2ItemSet setToCheck)
        {
            if (setToCheck.PlantSet3.Entities.Count(spawn => spawn.Value == MGS2Items.Card) < PlantSet2CardsRequired)
            {
                //only need 1
                return false;
            }

            if (setToCheck.PlantSet3.Entities.Count(spawn => spawn.Value == MGS2Items.Card) < PlantSet3CardsRequired)
            {
                //need 2
                return false;
            }

            if (setToCheck.PlantSet6.Entities.Count(spawn => spawn.Value == MGS2Items.Card) < PlantSet6CardsRequired)
            {
                //need 3
                return false;
            }

            if (setToCheck.PlantSet7.Entities.Count(spawn => spawn.Value == MGS2Items.Card) < PlantSet7CardsRequired)
            {
                //need 4
                return false;
            }

            if (setToCheck.PlantSet9.Entities.Count(spawn => spawn.Value == MGS2Items.Card) < PlantSet9CardsRequired)
            {
                //need 5
                return false;
            }

            return true;
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
            string cheatSheet = SpoilerContents;
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
