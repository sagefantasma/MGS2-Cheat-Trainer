using MathNet.Numerics.Random;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
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
            new RandomizedItem{Index = 6+0xC1, Count = 1+0xC1, Name = "B.D.U." },new RandomizedItem{Index = 7 + 0xC1,Count = 1 + 0xC1, Name = "Body Armor" },
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

        private static List<RandomizedItem> MasterSnakeItemAwardOptions = new List<RandomizedItem> { new RandomizedItem { Index = 1 + 0xC1, Count = 2, Name = "Ration" },
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
                VanillaItems.BuildVanillaItems();
            }
            else
            {
                throw new DirectoryNotFoundException("Invalid directory provided, please provide the full path to your MGS2 install location.");
            }
        }

        private void BuildVanillaItemSet()
        {
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
                PlantSet10 = new ItemSet(VanillaItems.PlantSet10),

                PlantCard0Set = new ItemSet(VanillaItems.PlantCard0Set),
                PlantCard1Set = new ItemSet(VanillaItems.PlantCard1Set),
                PlantCard2Set = new ItemSet(VanillaItems.PlantCard2Set),
                PlantCard3Set = new ItemSet(VanillaItems.PlantCard3Set),
                PlantCard4Set = new ItemSet(VanillaItems.PlantCard4Set),
                PlantCard5Set = new ItemSet(VanillaItems.PlantCard5Set),

                CardRandomizationFirstProgressionItems = new List<Item>(LogicRequirements.CardRandomizationFirstProgressionItems),
                CardRandomizationSecondProgressionItems = new List<Item>(LogicRequirements.CardRandomizationSecondProgressionItems),
                CardRandomizationThirdProgressionItems = new List<Item>(LogicRequirements.CardRandomizationThirdProgressionItems)
            };
        }

        public class RandomizationOptions
        {
            public bool NoHardLogicLocks { get; set; }
            public bool NikitaShell2 { get; set; }
            public bool RandomizeStartingItems { get; set; }
            public bool RandomizeAutomaticRewards { get; set; }
            public bool RandomizeClaymores { get; set; }
            public bool RandomizeTankerControlUnits { get; set; }
            public bool RandomizeC4 { get; set; }
            public bool IncludeRations { get; set; }
            public bool AllWeaponsSpawnable { get; set; }
            public bool RandomizeCards { get; set; }
            public bool KeepVanillaCardAccess { get; set; }

            public override string ToString()
            {
                return $"NoHardLogicLocks = {NoHardLogicLocks};\n" +
                    $"NikitaShell2 = {NikitaShell2};\n" +
                    $"AllWeaponsSpawnable = {AllWeaponsSpawnable};\n" +
                    $"IncludeRations = {IncludeRations};\n" +
                    $"RandomizeStartingItems = {RandomizeStartingItems};\n" +
                    $"RandomizeAutomaticRewards = {RandomizeAutomaticRewards};\n" +
                    $"RandomizeCards = {RandomizeCards};\n" +
                    $"KeepVanillaCardAccess = {KeepVanillaCardAccess};\n" +
                    $"RandomizeC4 = {RandomizeC4};\n" +
                    $"RandomizeClaymores = {RandomizeClaymores};\n" +
                    $"RandomizeTankerControlUnits = {RandomizeTankerControlUnits};\n\n\n\n\n\n";
            }
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
            int leftWall = 0xBF68;
            int rightSideLowerCatwalk = 0xABB0;
            uint topWall = 0xFFFF0218;
            int rightWall = 0xD6D8;
            uint bottomWall = 0xFFFF2928;

            List<PointF> walkableArea = new List<PointF>
            {

                new PointF(0xED4F, 0xFFFEDC3D),
                new PointF(0xCF29, 0xFFFEDCFD),
                new PointF(0xCE0F, 0xFFFEDE6E),
                new PointF(0xCF47, 0xFFFEDFE9),
                new PointF(0xD16F, 0xFFFEE0A9),
                new PointF(0xD16F, 0xFFFEE48B),
                new PointF(0xCEC0, 0xFFFEE53E),
                new PointF(0xCD88, 0xFFFEE67B),
                new PointF(0xCD88, 0xFFFEFA69),
                new PointF(0xCED7, 0xFFFEFBCA),
                new PointF(0xD696, 0xFFFEFBCA),
                new PointF(0xD74A, 0xFFFEFC7E),
                new PointF(0xD74C, 0xFFFF2EC2),
                new PointF(0xD696, 0xFFFF2F76),
                new PointF(0xCF19, 0xFFFF2F77),
                new PointF(0xCD87, 0xFFFF30FB),
                new PointF(0xCD86, 0xFFFF44DE),
                new PointF(0xCEA2, 0xFFFF4601),
                new PointF(0xD0BA, 0xFFFF4601),
                new PointF(0xD16E, 0xFFFF46B6),
                new PointF(0xD170, 0xFFFF4A98),
                new PointF(0xD0BA, 0xFFFF4B57),
                new PointF(0xCF63, 0xFFFF4B57),
                new PointF(0xCE0F, 0xFFFF4C67),
                new PointF(0xCF62, 0xFFFF4E43),
                new PointF(0xE4BF, 0xFFFF4E43),
                new PointF(0xE57F, 0xFFFF4F03),
                new PointF(0xE57F, 0xFFFF5361),
                new PointF(0xE4BF, 0xFFFF5421),
                new PointF(0xC508, 0xFFFF5421),
                new PointF(0xC449, 0xFFFF5362),
                new PointF(0xC449, 0xFFFF4F03),
                new PointF(0xC509, 0xFFFF4E43),
                new PointF(0xC719, 0xFFFF4E43),
                new PointF(0xC831, 0xFFFF4D22),
                new PointF(0xC705, 0xFFFF4B57),
                new PointF(0xC585, 0xFFFF4B57),
                new PointF(0xC4D0, 0xFFFF4A98),
                new PointF(0xC4D2, 0xFFFF46B6),
                new PointF(0xC585, 0xFFFF4601),
                new PointF(0xC7AF, 0xFFFF4602),
                new PointF(0xC8B8, 0xFFFF448C),
                new PointF(0xC8B9, 0xFFFF3102),
                new PointF(0xC73A, 0xFFFF2F76),
                new PointF(0xBFA9, 0xFFFF2F77),
                new PointF(0xBEF4, 0xFFFF2EC3),
                new PointF(0xBEF6, 0xFFFF1955),
                new PointF(0xBDB1, 0xFFFF1808),
                new PointF(0x5D6D, 0xFFFF1808),
                new PointF(0x5C40, 0xFFFF1A14),
                new PointF(0x5C3E, 0xFFFF1B3B),
                new PointF(0x5B8A, 0xFFFF1BF0),
                new PointF(0x57A8, 0xFFFF1BF0),
                new PointF(0x56E9, 0xFFFF1B3B),
                new PointF(0x56E9, 0xFFFF19CD),
                new PointF(0x552E, 0xFFFF188F),
                new PointF(0x533D, 0xFFFF2D11),
                new PointF(0x4CF5, 0xFFFF2D11),
                new PointF(0x4CEB, 0xFFFF0EC9),
                new PointF(0x533D, 0xFFFF0EC9),
                new PointF(0x5537, 0xFFFF12B1),
                new PointF(0x56E9, 0xFFFF101D),
                new PointF(0x5C40, 0xFFFF1006),
                new PointF(0x5EFC, 0xFFFF133A),
                new PointF(0xBC39, 0xFFFF133A),
                new PointF(0xBEF6, 0xFFFF1078),
                new PointF(0xBFA9, 0xFFFEFBC8),
                new PointF(0xC676, 0xFFFEFBC9),
                new PointF(0xC8B8, 0xFFFEFA4C),
                new PointF(0xC8B9, 0xFFFEE7B7),
                new PointF(0xC5CE, 0xFFFEE53E),
                new PointF(0xC59A, 0xFFFEDFE9),
                new PointF(0xC831, 0xFFFEDE34),
                new PointF(0xC508, 0xFFFEDCFD),
                new PointF(0xC508, 0xFFFED71F),
                new PointF(0xED4F, 0xFFFED7A3)
            };

            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w21a"));

            //make claymore functions bigger
            GcxEditor w21a = new GcxEditor();
            w21a.CallDecompiler(gcxFile);
            List<DecodedProc> contentTree = w21a.BuildContentTree();
            DecodedProc claymoreSpawningFunction = contentTree.Find(x => x.Name == "proc_0x223D85 ");
            byte[] customClaymoreFunctionContents = File.ReadAllBytes("gcx\\w21a_custom_claymores.proc");
            claymoreSpawningFunction.RawContents = customClaymoreFunctionContents;
            byte[] modifiedGcxContents = w21a.BuildGcxFile();
            File.WriteAllBytes(gcxFile, modifiedGcxContents);

            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            List<int> claymores = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x85, 0xD6, 0x78 });

            PointF randomPoint;

            foreach (int claymore in claymores)
            {
                randomPoint = GetRandomPointInPolygon(walkableArea, Randomizer);
                // Rerandomize any rolls that are on the stairs leading to lower catwalk
                while (randomPoint.X < leftWall && randomPoint.X > rightSideLowerCatwalk)
                {
                    randomPoint = GetRandomPointInPolygon(walkableArea, Randomizer);
                }
                int xPos = (int) randomPoint.X;
                uint yPos = (uint)randomPoint.Y;

                Array.Copy(BitConverter.GetBytes(xPos), 0, gcxContents, claymore + 0xB, 2);
                
                // Claymores on the lower catwalk need to be, well, lowered.
                if(xPos < rightSideLowerCatwalk)
                {
                    Array.Copy(BitConverter.GetBytes(0xFFFFFA20), 0, gcxContents, claymore + 0x10, 4);
                }

                Array.Copy(BitConverter.GetBytes(yPos), 0, gcxContents, claymore + 0x15, 4); //the FFFF should be untouched with this and still work
            }

            File.WriteAllBytes(gcxFile, gcxContents);
        }

        #region ChatGPT polygon interior randomization magic
        public static bool IsPointInPolygon(PointF p, List<PointF> polygon)
        {
            bool inside = false;
            int n = polygon.Count;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y)) &&
                    (p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        // Get the bounding box of the polygon
        public static RectangleF GetBoundingBox(List<PointF> polygon)
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

            foreach (var point in polygon)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        // Generate a random point inside the polygon
        public static PointF GetRandomPointInPolygon(List<PointF> polygon, Random rand)
        {
            var boundingBox = GetBoundingBox(polygon);

            PointF randomPoint;
            do
            {
                // Generate random point within the bounding box
                float x = (float)(rand.NextDouble() * boundingBox.Width + boundingBox.Left);
                float y = (float)(rand.NextDouble() * boundingBox.Height + boundingBox.Top);
                randomPoint = new PointF(x, y);
            }
            while (!IsPointInPolygon(randomPoint, polygon));  // Check if it's inside the polygon

            return randomPoint;
        }
        #endregion

        private void AddTankerStartingItemsToPool()
        {
            //Add M9, Camera, Cigs and AP Sensor to randomization pool
            KeyValuePair<Location, Item> newSpawn1 = _vanillaItems.TankerPart3.Entities.First(spawn => spawn.Key.Name == "RightsideLifeboats" && spawn.Key.GcxFile == "w00a");
            _vanillaItems.TankerPart3.Entities[newSpawn1.Key] = MGS2Weapons.M9; 

            KeyValuePair<Location, Item> newSpawn2 = _vanillaItems.TankerPart3.Entities.First(spawn => spawn.Key.Name == "UnderLeftsideStairs" && spawn.Key.GcxFile == "w00a");
            _vanillaItems.TankerPart3.Entities[newSpawn2.Key] = MGS2Items.Camera1;

            KeyValuePair<Location, Item> newSpawn3 = _vanillaItems.TankerPart3.Entities.First(spawn => spawn.Key.Name == "UnderRightsideStairs" && spawn.Key.GcxFile == "w01b");
            _vanillaItems.TankerPart3.Entities[newSpawn3.Key] = MGS2Items.Cigs;

            KeyValuePair<Location, Item> newSpawn4 = _vanillaItems.TankerPart1.Entities.First(spawn => spawn.Key.Name == "Bar" && spawn.Key.GcxFile == "w01f");
            _vanillaItems.TankerPart3.Entities[newSpawn4.Key] = MGS2Items.APSensor;

            if (!_vanillaItems.TankerPart1.ItemsNeededToProgress.Contains(MGS2Weapons.M9))
                _vanillaItems.TankerPart1.ItemsNeededToProgress.Add(MGS2Weapons.M9);
            if (!_vanillaItems.TankerPart3.ItemsNeededToProgress.Contains(MGS2Items.Camera1))
                _vanillaItems.TankerPart3.ItemsNeededToProgress.Add(MGS2Items.Camera1);
        }

        private void AddPlantStartingItemsToPool()
        {
            //Add AP Sensor and Scope to randomization pool
            KeyValuePair<Location, Item> newSpawn1 = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.Name == "BottomFloorMiddleCrates" && spawn.Key.GcxFile == "w22a");
            _vanillaItems.PlantSet10.Entities[newSpawn1.Key] = MGS2Items.APSensor;

            KeyValuePair<Location, Item> newSpawn2 = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.Name == "BottomFloorParkourBoxes" && spawn.Key.GcxFile == "w22a");
            _vanillaItems.PlantSet10.Entities[newSpawn2.Key] = MGS2Items.Scope1;
        }

        private void RandomizeStartingItems()
        {
            string gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_n_title"));
            byte[] gcxContents = File.ReadAllBytes(gcxFile);
            byte[] snakeStartingAmmoBytes = new byte[] { 0x11, 0x00, 0x0A, 0x5C };
            byte[] emptyInitializeWeaponsArray = new byte[] { 0xC2, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0 };
            byte[] startingItemCountBytes = new byte[] { 0x14, 0x06, 0x02, 0x7D };
            byte[] emptyInitializeItemsArray = new byte[] { 0xC2, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1, 0xC1 };

            #region Tanker
            //Snake starts with M9, so randomize that
            List<int> snakeWeaponAward = GcxEditor.FindAllSubArray(gcxContents, TankerInitializeWeaponsArray);
            RandomizedItem randomTankerStartingWeapon = GetRandomItem(true, false);
            int indexToModify = randomTankerStartingWeapon.Index - 0xC1;
            byte[] newInitializeWeaponsArray = new byte[emptyInitializeWeaponsArray.Length + 3];
            Array.Copy(emptyInitializeWeaponsArray, newInitializeWeaponsArray, indexToModify);
            Array.Copy(snakeStartingAmmoBytes, 0, newInitializeWeaponsArray, indexToModify, snakeStartingAmmoBytes.Length);
            Array.Copy(emptyInitializeWeaponsArray, indexToModify + 1, newInitializeWeaponsArray, indexToModify + 4, emptyInitializeWeaponsArray.Length - indexToModify - 1);
            foreach (int location in snakeWeaponAward)
            {
                Array.Copy(newInitializeWeaponsArray, 0, gcxContents, location + 6, newInitializeWeaponsArray.Length);
            }
            //^this works, but oh my lord is this over-engineered. I can just... insert C0s until I get where I need to be, then insert the ammo bytes, then fill out with C0s. christ.


            //Snake starts with Camera, cigs, and (possibly) AP Sensor.
            List<int> snakeItemAward = GcxEditor.FindAllSubArray(gcxContents, TankerInitializeItemsArray);
            while (snakeItemAward.Count > 5)
            {
                snakeItemAward.RemoveAt(5);
            }
            List<RandomizedItem> randomTankerStartingItems = new List<RandomizedItem>();
            
            for (int i = 0; i< 3; i++)
            {
                RandomizedItem randomItem = GetRandomItem(false, false);
                if(randomTankerStartingItems.Contains(randomItem))
                {
                    i--;
                }
                else
                {
                    randomTankerStartingItems.Add(randomItem);
                }
            }

            //if not starting with Camera, modify w00a to not automatically award the Camera
            //use only result from `39218001ECF1D6C2` and set the ending C2 to C1
            if (!randomTankerStartingItems.Any(x=>x.Name == "Camera"))
            {
                string w00aFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w00a"));
                byte[] w00aByteContents = File.ReadAllBytes(w00aFile);

                int cameraIndex = GcxEditor.FindSubArray(w00aByteContents, new byte[] { 0x39, 0x21, 0x80, 0x01, 0xEC, 0xF1, 0xD6, 0xC2 }) + 7;

                w00aByteContents[cameraIndex] = 0xC1;
                File.WriteAllBytes(w00aFile, w00aByteContents);
            }

            List<int> selectedRandomItemIndices = new List<int>();
            foreach(RandomizedItem item in randomTankerStartingItems)
            {
                selectedRandomItemIndices.Add(item.Index - 0xC1);
            }
            selectedRandomItemIndices.Sort();

            byte[] newInitializeTankerItemsArray = new byte[emptyInitializeItemsArray.Length + 3];
            newInitializeTankerItemsArray[0] = 0xC2;
            for(int i = 1; i<newInitializeTankerItemsArray.Length; i++)
            {
                if (selectedRandomItemIndices.Contains(i))
                {
                    if(selectedRandomItemIndices.Count > 1)
                    {
                        newInitializeTankerItemsArray[i] = 0xC2;
                        selectedRandomItemIndices.Remove(i);
                    }
                    else
                    {
                        Array.Copy(startingItemCountBytes, 0, newInitializeTankerItemsArray, i, startingItemCountBytes.Length);
                        i += 3;
                    }
                }
                else
                {
                    newInitializeTankerItemsArray[i] = 0xC1;
                }
            }
            foreach (int location in snakeItemAward)
            {
                Array.Copy(newInitializeTankerItemsArray, 0, gcxContents, location + 6, newInitializeTankerItemsArray.Length);
            }
            AddTankerStartingItemsToPool();

            //Fixing the inventory bug that occurs if you watch the M9 pad demo on w00a
            string w00a = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w00a"));
            byte[] w00aContents = File.ReadAllBytes(w00a);

            List<int> autoMenuInitId = GcxEditor.FindAllSubArray(w00aContents, new byte[] { 0x86, 0x3A, 0xA2 });

            w00aContents[autoMenuInitId[0] + 8] = 0xC1;
            w00aContents[autoMenuInitId[0] + 9] = 0xC1;

            w00aContents[autoMenuInitId[1] + 14] = 0xC1;
            w00aContents[autoMenuInitId[1] + 18] = 0xC1;
            File.WriteAllBytes(w00a, w00aContents);
            #endregion

            #region Plant
            //Raiden only starts with the AP sensor and Scope, so randomize those
            List<int> raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantInitializeItemsArray);
            while(raidenItemAward.Count > 6)
            {
                raidenItemAward.RemoveAt(6);
            }
            raidenItemAward.RemoveAt(0);
            List<RandomizedItem> randomPlantStartingItems = new List<RandomizedItem>();
            for (int i = 0; i < 2; i++)
            {
                RandomizedItem randomItem = GetRandomItem(false, true);
                if (randomPlantStartingItems.Contains(randomItem))
                {
                    i--;
                }
                else
                {
                    randomPlantStartingItems.Add(randomItem);
                }
            }

            selectedRandomItemIndices = new List<int>();
            foreach (RandomizedItem item in randomPlantStartingItems)
            {
                selectedRandomItemIndices.Add(item.Index - 0xC1);
            }

            byte[] newInitializePlantItemsArray = new byte[emptyInitializeItemsArray.Length + 3];
            newInitializePlantItemsArray[0] = 0xC2;
            for (int i = 1; i < newInitializePlantItemsArray.Length; i++)
            {
                if (selectedRandomItemIndices.Contains(i))
                {
                    if (selectedRandomItemIndices.Count > 1)
                    {
                        newInitializePlantItemsArray[i] = 0xC2;
                        selectedRandomItemIndices.Remove(i);
                    }
                    else
                    {
                        Array.Copy(startingItemCountBytes, 0, newInitializePlantItemsArray, i, startingItemCountBytes.Length);
                        i += 3;
                    }
                }
                else
                {
                    newInitializePlantItemsArray[i] = 0xC1;
                }
            }
            foreach (int location in raidenItemAward)
            {
                Array.Copy(newInitializePlantItemsArray, 0, gcxContents, location + 6, newInitializePlantItemsArray.Length);
            }

            string d13tGcx = GcxFileDirectory.Find(file => file.Contains("scenerio_stage_d13t"));
            byte[] d13tContents = File.ReadAllBytes(d13tGcx);
            List<int> d13tRaidenItemAward = GcxEditor.FindAllSubArray(d13tContents, PlantInitializeItemsArray);
            while(d13tRaidenItemAward.Count > 5)
            {
                d13tRaidenItemAward.RemoveAt(5);
            }
            foreach(int location in d13tRaidenItemAward)
            {
                Array.Copy(newInitializePlantItemsArray, 0, d13tContents, location + 6, newInitializePlantItemsArray.Length);
            }
            AddPlantStartingItemsToPool();
            #endregion

            File.WriteAllBytes(gcxFile, gcxContents);
            File.WriteAllBytes(d13tGcx, d13tContents);
        }

        class RandomizedItem
        {
            public byte Index;
            public byte Count;
            public string Name;
        }

        private RandomizedItem GetRandomItem(bool isWeapon = false, bool isPlant = true)
        {
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
            Location uspLocation = _vanillaItems.TankerPart3.Entities.First(spawn => spawn.Key.GcxFile == "w01f" && spawn.Key.Name == "StinkyRationMan").Key;
            _vanillaItems.TankerPart3.Entities[uspLocation] = MGS2Weapons.Usp;
            if(!_vanillaItems.TankerPart2.ItemsNeededToProgress.Contains(MGS2Weapons.Usp))
                _vanillaItems.TankerPart2.ItemsNeededToProgress.Add(MGS2Weapons.Usp);

            Location socomLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w12b" && spawn.Key.Name == "Locker1").Key;
            _vanillaItems.PlantSet10.Entities[socomLocation] = MGS2Weapons.Socom;
            _vanillaItems.PlantCard5Set.Entities[socomLocation] = MGS2Weapons.Socom;
            
            Location cigsLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w12a" && spawn.Key.Name == "RightCage").Key;
            _vanillaItems.PlantSet10.Entities[cigsLocation] = MGS2Items.Cigs;
            _vanillaItems.PlantCard5Set.Entities[cigsLocation] = MGS2Items.Cigs;

            Location sensorALocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w16a" && spawn.Key.Name == "LadiesRoom2").Key;
            _vanillaItems.PlantSet10.Entities[sensorALocation] = MGS2Items.SensorA;
            _vanillaItems.PlantCard5Set.Entities[sensorALocation] = MGS2Items.SensorA;

            Location coolantSprayLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w16a" && spawn.Key.Name == "MensRoom").Key;
            _vanillaItems.PlantSet10.Entities[coolantSprayLocation] = MGS2Weapons.Coolant;
            _vanillaItems.PlantCard5Set.Entities[coolantSprayLocation] = MGS2Weapons.Coolant;

            Location bduLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w18a" && spawn.Key.Name == "UnderStairs").Key;
            _vanillaItems.PlantSet10.Entities[bduLocation] = MGS2Items.BDU;
            _vanillaItems.PlantCard5Set.Entities[bduLocation] = MGS2Items.BDU;

            Location phoneLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w20a" && spawn.Key.Name == "UnderConveyerBelt").Key;
            _vanillaItems.PlantSet10.Entities[phoneLocation] = MGS2Items.Phone;
            _vanillaItems.PlantCard5Set.Entities[phoneLocation] = MGS2Items.Phone;

            Location moDiskLocation = _vanillaItems.PlantSet10.Entities.First(spawn => spawn.Key.GcxFile == "w31d" && spawn.Key.Name == "ElectricalRoom2").Key;
            _vanillaItems.PlantSet10.Entities[moDiskLocation] = MGS2Items.MoDisc;
            _vanillaItems.PlantCard5Set.Entities[moDiskLocation] = MGS2Items.MoDisc;
        }

        private void AddCardsToPools()
        {
            Location card1Location = _vanillaItems.PlantCard5Set.Entities.First(spawn => spawn.Key.GcxFile == "w14a" && spawn.Key.Name == "Locker1").Key;
            _vanillaItems.PlantCard5Set.Entities[card1Location] = MGS2Items.Card;

            Location card2Location = _vanillaItems.PlantCard5Set.Entities.First(spawn => spawn.Key.GcxFile == "w22a" && spawn.Key.Name == "LockerNearNode1").Key;
            _vanillaItems.PlantCard5Set.Entities[card2Location] = MGS2Items.Card;

            Location card3Location = _vanillaItems.PlantCard5Set.Entities.First(spawn => spawn.Key.GcxFile == "w22a" && spawn.Key.Name == "C4Room2").Key;
            _vanillaItems.PlantCard5Set.Entities[card3Location] = MGS2Items.Card;

            Location card4Location = _vanillaItems.PlantCard5Set.Entities.First(spawn => spawn.Key.GcxFile == "w31b" && spawn.Key.Name == "MiddleHallwayAlcove").Key;
            _vanillaItems.PlantCard5Set.Entities[card4Location] = MGS2Items.Card;

            Location card5Location = _vanillaItems.PlantCard5Set.Entities.First(spawn => spawn.Key.GcxFile == "w31d" && spawn.Key.Name == "LeftsideAlcove").Key;
            _vanillaItems.PlantCard5Set.Entities[card5Location] = MGS2Items.Card;
        }

        private void CheckAndRemoveFromRequirements(RandomizedItem item, ItemSet itemSetAdjusted)
        {
            Item itemToRemove;
            if(itemSetAdjusted.Name == "Card0Set" && LogicRequirements.CardRandomizationFirstProgressionItems.Any(progressiveItem => progressiveItem.Name == item.Name))
            {
                itemToRemove = _vanillaItems.CardRandomizationFirstProgressionItems.Find(x => x.Name == item.Name);
                _vanillaItems.CardRandomizationFirstProgressionItems.Remove(itemToRemove);
            }
            if (itemSetAdjusted.Name == "Card1Set" && LogicRequirements.CardRandomizationSecondProgressionItems.Any(progressiveItem => progressiveItem.Name == item.Name))
            {
                itemToRemove = _vanillaItems.CardRandomizationSecondProgressionItems.Find(x => x.Name == item.Name);
                _vanillaItems.CardRandomizationSecondProgressionItems.Remove(itemToRemove);
            }
            if (itemSetAdjusted.Name == "Card2Set" && LogicRequirements.CardRandomizationThirdProgressionItems.Any(progressiveItem => progressiveItem.Name == item.Name))
            {
                itemToRemove = _vanillaItems.CardRandomizationThirdProgressionItems.Find(x => x.Name == item.Name);
                _vanillaItems.CardRandomizationThirdProgressionItems.Remove(itemToRemove);
            }
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
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if(itemToRemove != null)
                    {
                        _vanillaItems.PlantSet3.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet4.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Shell 1 Elevator":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet4.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Ames":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Shells Connecting Bridge":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Johnson":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Emma":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "Before Strut L":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(itemToRemove);
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
                case "After Strut L":
                    itemToRemove = _vanillaItems.PlantSet3.ItemsNeededToProgress.Find(x => x.Name == item.Name);
                    if (itemToRemove != null)
                    {
                        _vanillaItems.PlantSet10.ItemsNeededToProgress.Remove(itemToRemove);
                    }
                    break;
            }
        }

        private string RandomizeAutomaticRewards(bool randomizeCards)
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
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard0Set : _vanillaItems.PlantSet1);
            spoiler += $"Pliskin will give you {randomizedReward.Name} on Strut B.\n"; //working
            gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            List<int> raidenWeaponAward = GcxEditor.FindAllSubArray(gcxContents, PlantWeaponArray);
            randomizedReward = GetRandomItem(true);
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard0Set : _vanillaItems.PlantSet1);
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
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard0Set : _vanillaItems.PlantSet2);
            spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
            gcxContents[raidenWeaponAward[0] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenWeaponAward[0] + ItemCountOffset] = randomizedReward.Count;

            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard0Set : _vanillaItems.PlantSet2);
            spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            if (randomizeCards)
            {
                randomizedReward = GetRandomItem(false);
                CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantCard0Set);
                spoiler += $"Stillman will give you {randomizedReward.Name} on Strut C.\n"; //working
                gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward.Count;

                File.WriteAllBytes(gcxFile, gcxContents);

                //looks like Card 1 gets actually set in w16b, so we will set it here as well
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16b"));
                gcxContents = File.ReadAllBytes(gcxFile);
                raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);
                gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;
            }

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

            if (randomizeCards)
            {
                randomizedReward = GetRandomItem(false);
                CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantCard1Set);
                spoiler += $"Cyborg Ninja will give you {randomizedReward.Name} on Strut E.\n";
                gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;
            }

            RandomizedItem randomizedReward2 = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard1Set : _vanillaItems.PlantSet4);
            spoiler += $"Cyborg Ninja will give you {randomizedReward2.Name} on Strut E.\n";
            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward2.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward2.Count;

            RandomizedItem randomizedReward3 = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, randomizeCards ? _vanillaItems.PlantCard1Set : _vanillaItems.PlantSet4);
            spoiler += $"Cyborg Ninja will give you {randomizedReward3.Name} on Strut E.\n";
            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);


            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20b"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            if (randomizeCards)
            {
                gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;
            }

            gcxContents[raidenItemAward[2] + ItemIndexOffset] = randomizedReward2.Index;
            gcxContents[raidenItemAward[2] + ItemCountOffset] = randomizedReward2.Count;

            gcxContents[raidenItemAward[3] + ItemIndexOffset] = randomizedReward3.Index;
            gcxContents[raidenItemAward[3] + ItemCountOffset] = randomizedReward3.Count;

            File.WriteAllBytes(gcxFile, gcxContents);

            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20c"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            if (randomizeCards)
            {
                gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;
            }

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
            if (randomizeCards)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w24b"));
                gcxContents = File.ReadAllBytes(gcxFile);
                raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

                randomizedReward = GetRandomItem(false);
                CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantCard2Set);
                spoiler += $"Ames will give you {randomizedReward.Name} in the Hostage Room.\n"; //worked
                gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region President Card 4 & MO Disk
            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w31a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

            if (randomizeCards)
            {
                randomizedReward = GetRandomItem(false);
                CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantSet8);
                spoiler += $"President Johnson will give you {randomizedReward.Name} on Shell 2.\n"; //worked
                gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;
            }

            randomizedReward = GetRandomItem(false);
            CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantSet8);
            spoiler += $"President Johnson will give you {randomizedReward.Name} on Shell 2.\n"; //worked
            gcxContents[raidenItemAward[1] + ItemIndexOffset] = randomizedReward.Index;
            gcxContents[raidenItemAward[1] + ItemCountOffset] = randomizedReward.Count;

            File.WriteAllBytes(gcxFile, gcxContents);
            #endregion

            #region Emma Card 5 
            //d036p01
            if (randomizeCards)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w25d"));
                gcxContents = File.ReadAllBytes(gcxFile);
                raidenItemAward = GcxEditor.FindAllSubArray(gcxContents, PlantItemArray);

                randomizedReward = GetRandomItem(false);
                CheckAndRemoveFromRequirements(randomizedReward, _vanillaItems.PlantSet10);
                spoiler += $"Emma will give you {randomizedReward.Name} on the KL Connecting Bridge.\n"; //worked
                gcxContents[raidenItemAward[0] + ItemIndexOffset] = randomizedReward.Index;
                gcxContents[raidenItemAward[0] + ItemCountOffset] = randomizedReward.Count;

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            //Snake HF Blade
            //TODO: implement

            return spoiler;
        }

        private void RandomizeTankerSemtexControlUnitLocations()
        {
            byte[] controlUnit1Bytes = { 0x6C, 0x55, 0xF5 };
            byte[] brokenControlUnit1Bytes = { 0xAD, 0x55, 0xD5 };
            byte[] controlUnit2Bytes = { 0x6B, 0x55, 0xF5 };
            byte[] brokenControlUnit2Bytes = { 0xAC, 0x55, 0xD5 };
            byte[] controlUnit3Bytes = { 0x6A, 0x55, 0xF5 };
            byte[] brokenControlUnit3Bytes = { 0xAB, 0x55, 0xD5 };
            string gcxFile;
            byte[] gcxContents;

            int c1Choice = Randomizer.Next(4);
            int c2Choice = Randomizer.Next(5);
            int c3Choice = c1Choice;
            while(c3Choice == c1Choice)
            {
                c3Choice = Randomizer.Next(4);
            }

            ByteLocation c1Location = SouthFacingControlUnit(c1Choice);
            ByteLocation c2Location = new ByteLocation();
            ByteLocation c3Location = SouthFacingControlUnit(c3Choice);
            switch (c2Choice)
            {
                case 0:
                    c2Location = null;
                    break;
                case 1:
                    c2Location.X = new byte[] { 0xE4, 0xC1 };
                    c2Location.Y = new byte[] { 0x78, 0xEC };
                    c2Location.Z = new byte[] { 0x30, 0x87 };
                    break;
                case 2:
                    c2Location.X = new byte[] { 0xE4, 0xC1 };
                    c2Location.Y = new byte[] { 0x78, 0xEC };
                    c2Location.Z = new byte[] { 0x08, 0x96 };
                    break;
                case 3:
                    c2Location.X = new byte[] { 0xB0, 0xDE };
                    c2Location.Y = new byte[] { 0x00, 0xFA };
                    c2Location.Z = new byte[] { 0x37, 0xD7 };
                    break;
                case 4:
                    c2Location.X = new byte[] { 0xB0, 0xDE };
                    c2Location.Y = new byte[] { 0x00, 0xFA };
                    c2Location.Z = new byte[] { 0x32, 0xA0 };
                    break;
                case 5:
                    c2Location.X = new byte[] { 0x0F, 0xCF };
                    c2Location.Y = new byte[] { 0x40, 0xF6 };
                    c2Location.Z = new byte[] { 0x90, 0xB2 };
                    break;
            }

            gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w02a"));
            gcxContents = File.ReadAllBytes(gcxFile);
            if (c1Location != null)
            {
                int controlUnit1Offset = GcxEditor.FindAllSubArray(gcxContents, controlUnit1Bytes).FirstOrDefault();
                int brokenControlUnit1Offset = GcxEditor.FindAllSubArray(gcxContents, brokenControlUnit1Bytes).LastOrDefault();

                Array.Copy(c1Location.X, 0, gcxContents, controlUnit1Offset + 0x7, 2);
                Array.Copy(c1Location.Y, 0, gcxContents, controlUnit1Offset + 0xA, 2);
                Array.Copy(c1Location.Z, 0, gcxContents, controlUnit1Offset + 0xD, 2);

                Array.Copy(c1Location.X, 0, gcxContents, brokenControlUnit1Offset + 0x11, 2);
                Array.Copy(c1Location.Y, 0, gcxContents, brokenControlUnit1Offset + 0x14, 2);
                Array.Copy(c1Location.Z, 0, gcxContents, brokenControlUnit1Offset + 0x17, 2);
            }
            if(c2Location != null)
            {
                int controlUnit2Offset = GcxEditor.FindAllSubArray(gcxContents, controlUnit2Bytes).FirstOrDefault();
                int brokenControlUnit2Offset = GcxEditor.FindAllSubArray(gcxContents, brokenControlUnit2Bytes).LastOrDefault();

                Array.Copy(c2Location.X, 0, gcxContents, controlUnit2Offset + 0x7, 2);
                Array.Copy(c2Location.Y, 0, gcxContents, controlUnit2Offset + 0xA, 2);
                Array.Copy(c2Location.Z, 0, gcxContents, controlUnit2Offset + 0xD, 2);

                Array.Copy(c2Location.X, 0, gcxContents, brokenControlUnit2Offset + 0x11, 2);
                Array.Copy(c2Location.Y, 0, gcxContents, brokenControlUnit2Offset + 0x14, 2);
                Array.Copy(c2Location.Z, 0, gcxContents, brokenControlUnit2Offset + 0x17, 2);
            }
            if(c3Location != null)
            {
                int controlUnit3Offset = GcxEditor.FindAllSubArray(gcxContents, controlUnit3Bytes).FirstOrDefault();
                int brokenControlUnit3Offset = GcxEditor.FindAllSubArray(gcxContents, brokenControlUnit3Bytes).LastOrDefault();

                Array.Copy(c3Location.X, 0, gcxContents, controlUnit3Offset + 0x7, 2);
                Array.Copy(c3Location.Y, 0, gcxContents, controlUnit3Offset + 0xA, 2);
                Array.Copy(c3Location.Z, 0, gcxContents, controlUnit3Offset + 0xD, 2);

                Array.Copy(c3Location.X, 0, gcxContents, brokenControlUnit3Offset + 0x11, 2);
                Array.Copy(c3Location.Y, 0, gcxContents, brokenControlUnit3Offset + 0x14, 2);
                Array.Copy(c3Location.Z, 0, gcxContents, brokenControlUnit3Offset + 0x17, 2);
            }

            File.WriteAllBytes(gcxFile, gcxContents);
        }

        private class ByteLocation
        {
            public byte[] X;
            public byte[] Y;
            public byte[] Z;
        }

        private ByteLocation SouthFacingControlUnit(int choice)
        {
            ByteLocation byteLocation = new ByteLocation();
            switch(choice)
            {
                case 0:
                    byteLocation = null;
                    break;
                case 1:
                    byteLocation.X = new byte[] { 0xBB, 0xB7 };
                    byteLocation.Y = new byte[] { 0x80, 0xF1 };
                    byteLocation.Z = new byte[] { 0x00, 0xA3 };
                    break;
                case 2:
                    byteLocation.X = new byte[] { 0xBB, 0xB7 };
                    byteLocation.Y = new byte[] { 0x40, 0xF5 };
                    byteLocation.Z = new byte[] { 0xB5, 0x9E };
                    break;
                case 3:
                    byteLocation.X = new byte[] { 0xB5, 0xC8 };
                    byteLocation.Y = new byte[] { 0x40, 0xF6 };
                    byteLocation.Z = new byte[] { 0xB5, 0x9E };
                    break;
                case 4:
                    byteLocation.X = new byte[] { 0xB7, 0xE4 };
                    byteLocation.Y = new byte[] { 0x00, 0xF3 };
                    byteLocation.Z = new byte[] { 0xBE, 0xA6 };
                    break;
            }

            return byteLocation;
        }

        private void RandomizeC4Locations()
        {
            byte[] bulC4InitBytes = { 0x11, 0xBB, 0xDB, 0x06 };
            string gcxFile;
            byte[] gcxContents;
            List<int> c4Locations;

            #region Strut A Roof
            int roofChoice = Randomizer.Next(3);
            byte[] roofXLocation = new byte[2];
            byte[] roofYLocation = new byte[2];
            byte[] roofZLocation = new byte[2];
            switch (roofChoice)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    roofXLocation = new byte[] { 0xCD, 0x0B };
                    roofYLocation = new byte[] { 0x6A, 0x14 };
                    roofZLocation = new byte[] { 0x00, 0x00};
                    break;
                case 2:
                    roofXLocation = new byte[] { 0x52, 0xE4 };
                    roofYLocation = new byte[] { 0x6A, 0x18 };
                    roofZLocation = new byte[] { 0x57, 0x09};
                    break;
            }

            if (roofChoice != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w12a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, bulC4InitBytes);

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(roofXLocation, 0, gcxContents, c4Location + 0xC, 2);
                    Array.Copy(roofYLocation, 0, gcxContents, c4Location + 0xF, 2);
                    Array.Copy(roofZLocation, 0, gcxContents, c4Location + 0x12, 2);
                }

                File.WriteAllBytes(gcxFile, gcxContents);

                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w12c"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, bulC4InitBytes);

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(roofXLocation, 0, gcxContents, c4Location + 0xC, 2);
                    Array.Copy(roofYLocation, 0, gcxContents, c4Location + 0xF, 2);
                    Array.Copy(roofZLocation, 0, gcxContents, c4Location + 0x12, 2);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Pump Room
            int pumpRoom = Randomizer.Next(6);
            byte[] pumpRoomXLocation = new byte[1];
            byte[] pumpRoomYLocation = new byte[1];
            byte[] pumpRoomZLocation = new byte[2];
            switch (pumpRoom)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    pumpRoomXLocation = new byte[] { 0x6F };
                    pumpRoomYLocation = new byte[] { 0xFF };
                    pumpRoomZLocation = new byte[] { 0x00, 0x01 };
                    break;
                case 2:
                    pumpRoomXLocation = new byte[] { 0x70 };
                    pumpRoomYLocation = new byte[] { 0xFF};
                    pumpRoomZLocation = new byte[] { 0x80, 0x31 };
                    break;
                case 3:
                    pumpRoomXLocation = new byte[] { 0x00 };
                    pumpRoomYLocation = new byte[] { 0xFF };
                    pumpRoomZLocation = new byte[] { 0x38, 0x20 };
                    break;
                case 4:
                    pumpRoomXLocation = new byte[] { 0x00 };
                    pumpRoomYLocation = new byte[] { 0xFF };
                    pumpRoomZLocation = new byte[] { 0x40, 0x1A };
                    break;
                case 5:
                    pumpRoomXLocation = new byte[] { 0x00 };
                    pumpRoomYLocation = new byte[] { 0xFF };
                    pumpRoomZLocation = new byte[] { 0x50, 0x10 };
                    break;
            }

            if (pumpRoom != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w12b"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x16, 0x99, 0x61, 0x59 });

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(pumpRoomXLocation, 0, gcxContents, c4Location + 0xD, 1);
                    Array.Copy(pumpRoomYLocation, 0, gcxContents, c4Location + 0xF, 1);
                    Array.Copy(pumpRoomZLocation, 0, gcxContents, c4Location + 0x11, 2);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Transformer Room
            int transformerRoom = Randomizer.Next(6);
            byte[] transformerRoomXLocation = new byte[4];
            byte[] transformerRoomYLocation = new byte[2];
            byte[] transformerRoomZLocation = new byte[4];
            switch (transformerRoom)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    transformerRoomXLocation = new byte[] { 0x56, 0x43, 0xFF, 0xFF };
                    transformerRoomYLocation = new byte[] { 0x4C, 0x04 };
                    transformerRoomZLocation = new byte[] { 0x90, 0x75, 0xFF, 0xFF };
                    break;
                case 2:
                    transformerRoomXLocation = new byte[] { 0x56, 0x43, 0xFF, 0xFF };
                    transformerRoomYLocation = new byte[] { 0x7A, 0x00 };
                    transformerRoomZLocation = new byte[] { 0x10, 0x89, 0xFF, 0xFF };
                    break;
                case 3:
                    transformerRoomXLocation = new byte[] { 0x00, 0x20, 0xFF, 0xFF };
                    transformerRoomYLocation = new byte[] { 0xE3, 0x01 };
                    transformerRoomZLocation = new byte[] { 0x90, 0x93, 0xFF, 0xFF };
                    break;
                case 4:
                    transformerRoomXLocation = new byte[] { 0xC6, 0x2C, 0xFF, 0xFF };
                    transformerRoomYLocation = new byte[] { 0x40, 0x0D };
                    transformerRoomZLocation = new byte[] { 0xAC, 0x67, 0xFF, 0xFF };
                    break;
                case 5:
                    transformerRoomXLocation = new byte[] { 0x61, 0x53, 0xFF, 0xFF };
                    transformerRoomYLocation = new byte[] { 0x7A, 0x00 };
                    transformerRoomZLocation = new byte[] { 0xA1, 0x67, 0xFF, 0xFF };
                    break;
            }

            if (transformerRoom != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w14a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, bulC4InitBytes);

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(transformerRoomXLocation, 0, gcxContents, c4Location + 0xC, 4);
                    Array.Copy(transformerRoomYLocation, 0, gcxContents, c4Location + 0x11, 2);
                    Array.Copy(transformerRoomZLocation, 0, gcxContents, c4Location + 0x14, 4);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Mess Hall
            int diningHall = Randomizer.Next(8);
            byte[] diningHallXLocation = new byte[4];
            byte[] diningHallYLocation = new byte[2];
            byte[] diningHallZLocation = new byte[4];
            switch (diningHall)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    diningHallXLocation = new byte[] { 0x41, 0x44, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0xB0, 0x05 };
                    diningHallZLocation = new byte[] { 0xC3, 0xBD, 0xFE, 0xFF };
                    break;
                case 2:
                    diningHallXLocation = new byte[] { 0x2E, 0x52, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0x32, 0x0D };
                    diningHallZLocation = new byte[] { 0xC3, 0xBD, 0xFE, 0xFF };
                    break;
                case 3:
                    diningHallXLocation = new byte[] { 0x34, 0x28, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0x32, 0x0B };
                    diningHallZLocation = new byte[] { 0xAB, 0xC1, 0xFE, 0xFF };
                    break;
                case 4:
                    diningHallXLocation = new byte[] { 0x61, 0x0F, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0xE2, 0x0C };
                    diningHallZLocation = new byte[] { 0x28, 0xAC, 0xFE, 0xFF };
                    break;
                case 5:
                    diningHallXLocation = new byte[] { 0x11, 0x25, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0x00, 0x0C };
                    diningHallZLocation = new byte[] { 0x33, 0xD5, 0xFE, 0xFF };
                    break;
                case 6:
                    diningHallXLocation = new byte[] { 0xA9, 0x4D, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0x60, 0x09 };
                    diningHallZLocation = new byte[] { 0xD8, 0x76, 0xFE, 0xFF };
                    break;
                case 7:
                    diningHallXLocation = new byte[] { 0x2E, 0x11, 0xFF, 0xFF };
                    diningHallYLocation = new byte[] { 0x70, 0x01 };
                    diningHallZLocation = new byte[] { 0x9D, 0x90, 0xFE, 0xFF };
                    break;
            }

            if (diningHall != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] {0x20, 0x99, 0x61, 0x59} );

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(diningHallXLocation, 0, gcxContents, c4Location + 0xD, 4);
                    Array.Copy(diningHallYLocation, 0, gcxContents, c4Location + 0x12, 2);
                    Array.Copy(diningHallZLocation, 0, gcxContents, c4Location + 0x15, 4);
                }

                File.WriteAllBytes(gcxFile, gcxContents);

                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w16b"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x20, 0x99, 0x61, 0x59 });

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(diningHallXLocation, 0, gcxContents, c4Location + 0xD, 4);
                    Array.Copy(diningHallYLocation, 0, gcxContents, c4Location + 0x12, 2);
                    Array.Copy(diningHallZLocation, 0, gcxContents, c4Location + 0x15, 4);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Sediment Pool
            int sedimentPool1 = Randomizer.Next(5);
            byte[] sedimentPool1XLocation = new byte[2];
            byte[] sedimentPool1YLocation = new byte[2];
            byte[] sedimentPool1ZLocation = new byte[4];
            switch (sedimentPool1)
            {
                default:
                    sedimentPool1 = 0;
                    break;
                case 0:
                    //change nothing
                    break;
                case 1:
                    //other liftable & sprayable hatch
                    sedimentPool1XLocation = new byte[] { 0x0B, 0xDB };
                    sedimentPool1YLocation = new byte[] { 0x66, 0xEF };
                    sedimentPool1ZLocation = new byte[] { 0x41, 0x0C, 0xFE, 0xFF };
                    break;
                case 2:
                    //behind fence
                    sedimentPool1XLocation = new byte[] { 0x45, 0xE4 };
                    sedimentPool1YLocation = new byte[] { 0x60, 0xF0 };
                    sedimentPool1ZLocation = new byte[] { 0x56, 0x5F, 0xFE, 0xFF };
                    break;
                case 3:
                    //left-side scaffold 
                    sedimentPool1XLocation = new byte[] { 0x9F, 0xFC };
                    sedimentPool1YLocation = new byte[] { 0x75, 0xED };
                    sedimentPool1ZLocation = new byte[] { 0xA3, 0x06, 0xFE, 0xFF };
                    break;
                    
                case 4: 
                    //under stairs 
                    sedimentPool1XLocation = new byte[] { 0x99, 0xEA };
                    sedimentPool1YLocation = new byte[] { 0x60, 0xF0 };
                    sedimentPool1ZLocation = new byte[] { 0x62, 0xF7, 0xFD, 0xFF };
                    break;
                    
            }

            int sedimentPool2 = Randomizer.Next(4);
            byte[] sedimentPool2XLocation = new byte[2];
            byte[] sedimentPool2YLocation = new byte[2];
            byte[] sedimentPool2ZLocation = new byte[4];
            switch (sedimentPool2)
            {
                default:
                    sedimentPool2 = 0;
                    break;
                case 0:
                    //change nothing
                    break;
                case 1:
                    //center cage
                    sedimentPool2XLocation = new byte[] { 0x77, 0x00 };
                    sedimentPool2YLocation = new byte[] { 0x30, 0xFC };
                    sedimentPool2ZLocation = new byte[] { 0x6F, 0x24, 0xFE, 0xFF };
                    break;
                case 2:
                    //behind fence
                    sedimentPool2XLocation = new byte[] { 0x5C, 0x1C};
                    sedimentPool2YLocation = new byte[] { 0x60, 0xF0 };
                    sedimentPool2ZLocation = new byte[] { 0x56, 0x5F, 0xFE, 0xFF };
                    break;
                case 3:
                    //right-side scaffold
                    sedimentPool2XLocation = new byte[] { 0x1C, 0x03 };
                    sedimentPool2YLocation = new byte[] { 0x75, 0xED };
                    sedimentPool2ZLocation = new byte[] { 0xB3, 0x06, 0xFE, 0xFF };
                    break;
            }

            int sedimentPool3 = Randomizer.Next(5);
            byte[] sedimentPool3XLocation = new byte[2];
            byte[] sedimentPool3YLocation = new byte[2];
            byte[] sedimentPool3ZLocation = new byte[4];
            switch (sedimentPool3)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    sedimentPool3XLocation = new byte[] { 0xBF, 0x00 };
                    sedimentPool3YLocation = new byte[] { 0xC2, 0x01 };
                    sedimentPool3ZLocation = new byte[] { 0x6A, 0xF6, 0xFD, 0xFF };
                    break;
                case 2:
                    sedimentPool3XLocation = new byte[] { 0xB6, 0x26 };
                    sedimentPool3YLocation = new byte[] { 0xCE, 0xFF };
                    sedimentPool3ZLocation = new byte[] { 0x54, 0x49, 0xFE, 0xFF };
                    break;
                case 3:
                    sedimentPool3XLocation = new byte[] { 0xB0, 0x01 };
                    sedimentPool3YLocation = new byte[] { 0xB8, 0xFA };
                    sedimentPool3ZLocation = new byte[] { 0x66, 0x02, 0xFE, 0xFF };
                    break;
                case 4:
                    sedimentPool3XLocation = new byte[] { 0xBD, 0x1F };
                    sedimentPool3YLocation = new byte[] { 0xB8, 0xFA };
                    sedimentPool3ZLocation = new byte[] { 0xF6, 0x3B, 0xFE, 0xFF };
                    break;
            }

            if (sedimentPool1 != 0 || sedimentPool2 != 0 || sedimentPool3 != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w18a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                List<int> c41Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x06, 0x25, 0x6F, 0x3A, 0x06, 0x4D, 0x25, 0xB2 });
                List<int> c42Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x06, 0x25, 0x6F, 0x3A, 0x06, 0x4E, 0x25, 0xB2 });
                List<int> c43Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x06, 0x25, 0x6F, 0x3A, 0x06, 0x4F, 0x25, 0xB2 });

                if (sedimentPool1 != 0)
                {
                    //need to grab third hit of 4D25B2 + 7 and change from C1 -> C2
                    //otherwise, only default spawn will work
                    List<int> bomb1References = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x4D, 0x25, 0xB2 });
                    byte originalByte = 0;
                    int i = 0;
                    while(originalByte != 0xC1)
                    {
                        originalByte = gcxContents[bomb1References[i] + 7];
                        if (originalByte != 0xC1)
                        {
                            i++;
                        }
                    }
                    
                    gcxContents[bomb1References[i] + 7] = 0xC2;
                    foreach (int c4Location in c41Locations)
                    {
                        Array.Copy(sedimentPool1XLocation, 0, gcxContents, c4Location + 0xB, 2);
                        Array.Copy(sedimentPool1YLocation, 0, gcxContents, c4Location + 0xE, 2);
                        Array.Copy(sedimentPool1ZLocation, 0, gcxContents, c4Location + 0x11, 4);
                    }
                }
                if (sedimentPool2 != 0)
                {
                    //need to grab second hit of 4E25B2 + 7 and change from C1 -> C2
                    //otherwise, only default spawn will work
                    List<int> bomb2References = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x4E, 0x25, 0xB2 });
                    byte originalByte = 0;
                    int i = 0;
                    while (originalByte != 0xC1)
                    {
                        originalByte = gcxContents[bomb2References[i] + 7];
                        if (originalByte != 0xC1)
                        {
                            i++;
                        }
                    }

                    gcxContents[bomb2References[i] + 7] = 0xC2;
                    foreach (int c4Location in c42Locations)
                    {
                        Array.Copy(sedimentPool2XLocation, 0, gcxContents, c4Location + 0xB, 2);
                        Array.Copy(sedimentPool2YLocation, 0, gcxContents, c4Location + 0xE, 2);
                        Array.Copy(sedimentPool2ZLocation, 0, gcxContents, c4Location + 0x11, 4);
                    }
                }
                if (sedimentPool3 != 0)
                {
                    foreach (int c4Location in c43Locations)
                    {
                        Array.Copy(sedimentPool3XLocation, 0, gcxContents, c4Location + 0xB, 2);
                        Array.Copy(sedimentPool3YLocation, 0, gcxContents, c4Location + 0xE, 2);
                        Array.Copy(sedimentPool3ZLocation, 0, gcxContents, c4Location + 0x11, 4);
                    }
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Parcel Room
            int parcelRoom = Randomizer.Next(4);
            byte[] parcelRoomXLocation = new byte[4];
            byte[] parcelRoomYLocation = new byte[2];
            byte[] parcelRoomZLocation = new byte[4];
            switch (parcelRoom)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    parcelRoomXLocation = new byte[] { 0x27, 0xCC, 0x00, 0x00 };
                    parcelRoomYLocation = new byte[] { 0x2A, 0x09 };
                    parcelRoomZLocation = new byte[] { 0x41, 0x7A, 0xFE, 0xFF };
                    break;
                case 2:
                    parcelRoomXLocation = new byte[] { 0xDC, 0xDC, 0x00, 0x00 };
                    parcelRoomYLocation = new byte[] { 0x2A, 0x10 };
                    parcelRoomZLocation = new byte[] { 0x10, 0x89, 0xFE, 0xFF };
                    break;
                case 3:
                    parcelRoomXLocation = new byte[] { 0x42, 0xE5, 0x00, 0x00 };
                    parcelRoomYLocation = new byte[] { 0x4B, 0x06 };
                    parcelRoomZLocation = new byte[] { 0xC1, 0xC2, 0xFE, 0xFF };
                    break;
            }

            if (parcelRoom != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x06, 0x44, 0x31, 0x41, 0x0D, 0xEA, 0x7D, 0x5C, 0x99 });

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(parcelRoomXLocation, 0, gcxContents, c4Location + 0x19, 4);
                    Array.Copy(parcelRoomYLocation, 0, gcxContents, c4Location + 0x1E, 2);
                    Array.Copy(parcelRoomZLocation, 0, gcxContents, c4Location + 0x21, 4);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Helipad
            int helipad = Randomizer.Next(5);
            byte[] helipadXLocation = new byte[4];
            byte[] helipadYLocation = new byte[2];
            byte[] helipadZLocation = new byte[4];
            switch (helipad)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    helipadXLocation = new byte[] { 0x44, 0xBB, 0x00, 0x00 };
                    helipadYLocation = new byte[] { 0x00, 0x31 };
                    helipadZLocation = new byte[] { 0x3B, 0xA8, 0xFE, 0xFF };
                    break;
                case 2:
                    helipadXLocation = new byte[] { 0xBF, 0xB1, 0x00, 0x00 };
                    helipadYLocation = new byte[] { 0x00, 0x2E };
                    helipadZLocation = new byte[] { 0x3B, 0xA8, 0xFE, 0xFF };
                    break;
                case 3:
                    helipadXLocation = new byte[] { 0x75, 0xCD, 0x00, 0x00 };
                    helipadYLocation = new byte[] { 0x0D, 0x27 };
                    helipadZLocation = new byte[] { 0xDD, 0xC2, 0xFE, 0xFF };
                    break;
                case 4:
                    helipadXLocation = new byte[] { 0x91, 0x9E, 0x00, 0x00 };
                    helipadYLocation = new byte[] { 0x80, 0x11 };
                    helipadZLocation = new byte[] { 0x88, 0xA1, 0xFE, 0xFF };
                    break;
            }

            if (helipad != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w20b"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, bulC4InitBytes);

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(helipadXLocation, 0, gcxContents, c4Location + 0xC, 4);
                    Array.Copy(helipadYLocation, 0, gcxContents, c4Location + 0x11, 2);
                    Array.Copy(helipadZLocation, 0, gcxContents, c4Location + 0x14, 4);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion

            #region Armory
            int armory = Randomizer.Next(9);
            byte[] armoryXLocation = new byte[4];
            byte[] armoryYLocation = new byte[2];
            byte[] armoryZLocation = new byte[2];
            switch (armory)
            {
                case 0:
                    //change nothing
                    break;
                case 1:
                    armoryXLocation = new byte[] { 0x30, 0xC5, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0x60, 0xFB };
                    armoryZLocation = new byte[] { 0xDC, 0xB6};
                    break;
                case 2:
                    armoryXLocation = new byte[] { 0xA9, 0xB3, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0x60, 0xF0 };
                    armoryZLocation = new byte[] { 0x9F, 0x81};
                    break;
                case 3:
                    armoryXLocation = new byte[] { 0x45, 0x9A, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0xE3, 0x01 };
                    armoryZLocation = new byte[] { 0x53, 0x95};
                    break;
                case 4:
                    armoryXLocation = new byte[] { 0x81, 0xAF, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0x9B, 0x05 };
                    armoryZLocation = new byte[] { 0xB5, 0xB8};
                    break;
                case 5:
                    armoryXLocation = new byte[] { 0xFE, 0xF3, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0xB2, 0x00 };
                    armoryZLocation = new byte[] { 0x74, 0xAA};
                    break;
                case 6:
                    armoryXLocation = new byte[] { 0x3D, 0xC5, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0xE2, 0x01 };
                    armoryZLocation = new byte[] { 0x64, 0xA6};
                    break;
                case 7:
                    armoryXLocation = new byte[] { 0x53, 0xC4, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0x1B, 0x04 };
                    armoryZLocation = new byte[] { 0x00, 0x8E};
                    break;
                case 8:
                    armoryXLocation = new byte[] { 0xB9, 0xDA, 0x00, 0x00 };
                    armoryYLocation = new byte[] { 0x4B, 0x04 };
                    armoryZLocation = new byte[] { 0xAE, 0x90};
                    break;
            }

            if (armory != 0)
            {
                gcxFile = GcxFileDirectory.Find(file => file.Contains($"scenerio_stage_w22a"));
                gcxContents = File.ReadAllBytes(gcxFile);
                c4Locations = GcxEditor.FindAllSubArray(gcxContents, new byte[] { 0x06, 0x25, 0x6F, 0x3A, 0x06, 0x25, 0x6F, 0x3A });

                foreach (int c4Location in c4Locations)
                {
                    Array.Copy(armoryXLocation, 0, gcxContents, c4Location + 0xB, 4);
                    Array.Copy(armoryYLocation, 0, gcxContents, c4Location + 0x10, 2);
                    Array.Copy(armoryZLocation, 0, gcxContents, c4Location + 0x13, 2);
                    Array.Copy(new byte[] { 0x00, 0x04 }, 0, gcxContents, c4Location + 0x19, 2);
                }

                File.WriteAllBytes(gcxFile, gcxContents);
            }
            #endregion
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
            BuildVanillaItemSet();
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
            SpoilerContents = options.ToString();

            if (options.RandomizeStartingItems)
            {
                RandomizeStartingItems();
            }
            if (options.RandomizeAutomaticRewards)
            {
                SpoilerContents += RandomizeAutomaticRewards(options.RandomizeCards); 
            }
            else
            {
                //need to remove the automatic rewards from the logic checker if we aren't randomizing automatic rewards
                _vanillaItems.PlantSet3.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet3.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet4.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet4.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet4.ItemsNeededToProgress.Remove(MGS2Items.BDU);
                _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet5.ItemsNeededToProgress.Remove(MGS2Items.BDU);
                _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet6.ItemsNeededToProgress.Remove(MGS2Items.BDU);
                _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet7.ItemsNeededToProgress.Remove(MGS2Items.BDU);
                _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet8.ItemsNeededToProgress.Remove(MGS2Items.BDU);
                _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(MGS2Weapons.Socom);
                _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(MGS2Weapons.Coolant);
                _vanillaItems.PlantSet9.ItemsNeededToProgress.Remove(MGS2Items.BDU);
            }

            #region Tanker Randomization
            try
            {
                //Create a list of all spawns on the tanker chapter
                List<Item> TankerSpawnsLeft = new List<Item>();
                foreach (var kvp in _vanillaItems.TankerPart3.Entities)
                {
                    if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                        continue;
                    else
                        TankerSpawnsLeft.Add(kvp.Value);
                }

                //assign each spawn on the tanker a random item from the list of available spawns
                int itemsAssigned = 0;
                int retries = 1000;
                while (TankerSpawnsLeft.Count > 0)
                {
                    int randomNum = Randomizer.Next();
                    int modValue = randomNum % TankerSpawnsLeft.Count;
                    Item randomChoice = TankerSpawnsLeft[modValue];

                    if (options.NoHardLogicLocks &&
                        LogicRequirements.ProgressionItems.Contains(randomChoice.Name) &&
                        !_vanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                    {
                        retries--;
                        if (retries == 0)
                            break; //maybe throw and rethrow instead of break?
                        continue;
                    }

                    //iteratively go through spawns in "sequential" order, setting random items to each
                    if (itemsAssigned < _vanillaItems.TankerPart1.Entities.Count)
                    {
                        _randomizedItems.TankerPart1.Entities.Add(_vanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                    }
                    else if (itemsAssigned < _vanillaItems.TankerPart2.Entities.Count)
                    {
                        _randomizedItems.TankerPart2.Entities.Add(_vanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                    }
                    else
                    {
                        _randomizedItems.TankerPart3.Entities.Add(_vanillaItems.TankerPart3.Entities.ElementAt(itemsAssigned).Key, randomChoice);
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
            #endregion

            #region Plant Randomization
            try
            {
                List<Item> PlantSpawns = new List<Item>();

                int itemsAssigned = 0;
                int retries = 1000;

                if (!options.RandomizeCards)
                {
                    foreach (var kvp in _vanillaItems.PlantSet10.Entities)
                    {
                        if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                            continue;
                        else
                            PlantSpawns.Add(kvp.Value);
                    }

                    while (PlantSpawns.Count > 0)
                    {
                        int randomNum = Randomizer.Next();
                        int modValue = randomNum % PlantSpawns.Count;
                        Item randomChoice = PlantSpawns[modValue];

                        //isolate rations to only non-mandatory spawns
                        if (randomChoice.Name == "Ration" &&
                            _vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (options.NoHardLogicLocks &&
                            LogicRequirements.ProgressionItems.Contains(randomChoice.Name) &&
                            !_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (options.RandomizeAutomaticRewards
                            && LogicRequirements.AutoAwardedProgressionItems.Contains(randomChoice.Name) &&
                            !_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (randomChoice.Name == "Nikita" && options.NikitaShell2)
                        {
                            //currently, only the Nikita can cause a soft logic lock if the spawn is not in Shell 2
                            if (!(new[] { "w31a", "w31b" }.Contains(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.GcxFile)))
                            {
                                retries--;
                                if (retries == 0)
                                    break;
                                continue;
                            }
                        }

                        if (new[] { "M9", "RGB-6", "M4", "PSG1-T" }.Contains(randomChoice.Name) && options.AllWeaponsSpawnable && _vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn == false)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        //iteratively go through spawns in "sequential" order, setting random items to each
                        if (itemsAssigned < _vanillaItems.PlantSet1.Entities.Count)
                        {
                            /*if (randomChoice.Name == "Sensor B" && options.NoHardLogicLocks)
                            {
                                //if people report this issue, I'll create a new option for "reduce crash risk" and include this and others reported
                                //getting the Sensor B before meeting Stillman crashes the game(sometimes?)
                                continue;
                            }*/
                            _randomizedItems.PlantSet1.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet2.Entities.Count)
                        {
                            /*if (randomChoice.Name == "Sensor B" && options.NoHardLogicLocks)
                            {
                                //if people report this issue, I'll create a new option for "reduce crash risk" and include this and others reported
                                //getting the Sensor B before meeting Stillman crashes the game(sometimes?)
                                continue;
                            }*/
                            _randomizedItems.PlantSet2.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet3.Entities.Count)
                        {
                            _randomizedItems.PlantSet3.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet4.Entities.Count)
                        {
                            _randomizedItems.PlantSet4.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet5.Entities.Count)
                        {
                            _randomizedItems.PlantSet5.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet6.Entities.Count)
                        {
                            _randomizedItems.PlantSet6.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet7.Entities.Count)
                        {
                            _randomizedItems.PlantSet7.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet8.Entities.Count)
                        {
                            _randomizedItems.PlantSet8.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantSet9.Entities.Count)
                        {
                            _randomizedItems.PlantSet9.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else
                        {
                            _randomizedItems.PlantSet10.Entities.Add(_vanillaItems.PlantSet10.Entities.ElementAt(itemsAssigned).Key, randomChoice);
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
                }
                else
                {
                    AddCardsToPools();

                    foreach (var kvp in _vanillaItems.PlantCard5Set.Entities)
                    {
                        if (!options.IncludeRations && kvp.Value == MGS2Items.Ration)
                            continue;
                        else
                            PlantSpawns.Add(kvp.Value);
                    }

                    while (PlantSpawns.Count > 0)
                    {
                        int randomNum = Randomizer.Next();
                        int modValue = randomNum % PlantSpawns.Count;
                        Item randomChoice = PlantSpawns[modValue];

                        //isolate rations to only non-mandatory spawns
                        if(randomChoice.Name == "Ration" &&
                            _vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (options.NoHardLogicLocks &&
                            LogicRequirements.ProgressionItems.Contains(randomChoice.Name) &&
                            !_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (options.RandomizeAutomaticRewards
                            && LogicRequirements.AutoAwardedProgressionItems.Contains(randomChoice.Name) &&
                            !_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (options.NoHardLogicLocks &&
                            _vanillaItems.CardRandomizationFirstProgressionItems.Any(progressionItem => progressionItem.Name == randomChoice.Name) &&
                            Location.FirstProgressionAreas.Contains(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.GcxFile) &&
                            !_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn
                            )
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }
                        if (options.NoHardLogicLocks &&
                            _vanillaItems.CardRandomizationSecondProgressionItems.Any(progressionItem => progressionItem.Name == randomChoice.Name) &&
                            Location.SecondProgressionAreas.Contains(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.GcxFile) &&
                            !_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn
                            )
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }
                        if (options.NoHardLogicLocks &&
                            _vanillaItems.CardRandomizationThirdProgressionItems.Any(progressionItem => progressionItem.Name == randomChoice.Name) &&
                            Location.ThirdProgressionAreas.Contains(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.GcxFile) &&
                            !_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn
                            )
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }

                        if (randomChoice.Name == "Nikita" && options.NikitaShell2)
                        {
                            //currently, only the Nikita can cause a soft logic lock if the spawn is not in Shell 2
                            if (!Location.FourthProgressionAreas.Contains(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.GcxFile))
                            {
                                retries--;
                                if (retries == 0)
                                    break;
                                continue;
                            }
                        }
                        else if(randomChoice.Name == "Nikita")
                        {
                            if (Location.FifthProgressionAreas.Contains(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.GcxFile))
                            {
                                retries--;
                                if (retries == 0)
                                    break;
                                continue;
                            }
                        }

                        if (new[] { "M9", "RGB-6", "M4", "PSG1-T" }.Contains(randomChoice.Name) 
                            && options.AllWeaponsSpawnable 
                            && _vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key.MandatorySpawn == false)
                        {
                            retries--;
                            if (retries == 0)
                                break;
                            continue;
                        }
                        
                        //iteratively go through spawns in "sequential" order, setting random items to each
                        if (itemsAssigned < _vanillaItems.PlantCard0Set.Entities.Count)
                        {
                            _randomizedItems.PlantCard0Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantCard1Set.Entities.Count)
                        {
                            _randomizedItems.PlantCard1Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantCard2Set.Entities.Count)
                        {
                            _randomizedItems.PlantCard2Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantCard3Set.Entities.Count)
                        {
                            _randomizedItems.PlantCard3Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else if (itemsAssigned < _vanillaItems.PlantCard4Set.Entities.Count)
                        {
                            _randomizedItems.PlantCard4Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }
                        else
                        {
                            _randomizedItems.PlantCard5Set.Entities.Add(_vanillaItems.PlantCard5Set.Entities.ElementAt(itemsAssigned).Key, randomChoice);
                        }

                        PlantSpawns.Remove(randomChoice);
                        itemsAssigned++;
                    }
                    if(retries == 0)
                    {
                        throw new RandomizerException("bad randomization seed");
                    }

                    foreach (var entity in _randomizedItems.PlantCard0Set.Entities)
                    {
                        _randomizedItems.PlantCard1Set.Entities.Add(entity.Key, entity.Value);
                    }
                    foreach (var entity in _randomizedItems.PlantCard1Set.Entities)
                    {
                        _randomizedItems.PlantCard2Set.Entities.Add(entity.Key, entity.Value);
                    }
                    foreach (var entity in _randomizedItems.PlantCard2Set.Entities)
                    {
                        _randomizedItems.PlantCard3Set.Entities.Add(entity.Key, entity.Value);
                    }
                    foreach (var entity in _randomizedItems.PlantCard3Set.Entities)
                    {
                        _randomizedItems.PlantCard4Set.Entities.Add(entity.Key, entity.Value);
                    }
                    foreach (var entity in _randomizedItems.PlantCard4Set.Entities)
                    {
                        _randomizedItems.PlantCard5Set.Entities.Add(entity.Key, entity.Value);
                    }

                    if (!VerifyCardSetLogicValidity(_randomizedItems, options.KeepVanillaCardAccess))
                    {
                        throw new RandomizerException("bad randomization seed");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is RandomizerException)
                    throw ex;
                else
                {
                    throw new RandomizerException("bad randomization seed");
                }
            }

            foreach(var entity in _randomizedItems.PlantCard5Set.Entities)
            {
                _randomizedItems.PlantSet10.Entities[entity.Key] = entity.Value;
            }
            #endregion

            if (options.RandomizeClaymores)
            {
                RandomizeClaymores();
            }

            if (options.RandomizeC4)
            {
                RandomizeC4Locations();
            }

            if (options.RandomizeTankerControlUnits)
            {
                RandomizeTankerSemtexControlUnitLocations();
            }

            return Seed;
        }

        private bool VerifyItemSetLogicValidity(MGS2ItemSet setToCheck)
        {
            foreach (Item item in _vanillaItems.TankerPart1.ItemsNeededToProgress) 
            {
                if (!setToCheck.TankerPart1.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.TankerPart2.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart2.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.TankerPart3.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart3.Entities.ContainsValue(item))
                    return false;
            }

            foreach (Item item in _vanillaItems.PlantSet2.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet2.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet3.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet3.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet4.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet4.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet5.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet5.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet6.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet6.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet7.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet7.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet8.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet8.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.PlantSet9.ItemsNeededToProgress)
            {
                if (!setToCheck.PlantSet9.Entities.ContainsValue(item))
                    return false;
            }
            return true;
        }

        private bool VerifyCardSetLogicValidity(MGS2ItemSet setToCheck, bool keepCardAccessLevels = false)
        {
            #region Tanker Checks
            foreach (Item item in _vanillaItems.TankerPart1.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart1.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.TankerPart2.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart2.Entities.ContainsValue(item))
                    return false;
            }
            foreach (Item item in _vanillaItems.TankerPart3.ItemsNeededToProgress)
            {
                if (!setToCheck.TankerPart3.Entities.ContainsValue(item))
                    return false;
            }
            #endregion

            #region Plant Checks
            if (!setToCheck.PlantCard0Set.Entities.ContainsValue(MGS2Items.Card))
            {
                return false;
            }
            if (setToCheck.PlantCard1Set.Entities.Count(spawns => spawns.Value == MGS2Items.Card) <= 1)
            {
                return false;
            }
            if (setToCheck.PlantCard2Set.Entities.Count(spawns => spawns.Value == MGS2Items.Card) <= 2)
            {
                return false;
            }
            if (setToCheck.PlantCard3Set.Entities.Count(spawns => spawns.Value == MGS2Items.Card) <= 3)
            {
                return false;
            }
            if (setToCheck.PlantCard4Set.Entities.Count(spawns => spawns.Value == MGS2Items.Card) <= 4)
            {
                return false;
            }

            if(setToCheck.PlantCard5Set.Entities.Where(spawn => spawn.Key.CardNeededToAccess == 0).Count(spawns => spawns.Value == MGS2Items.Card) < 1)
            {
                return false;
            }

            if (keepCardAccessLevels)
            {
                foreach (KeyValuePair<Item, int> uniqueItemSpawn in VanillaItems.ItemAccessLevels)
                {
                    KeyValuePair<Location, Item> randomizedUniqueSpawnToSwap = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value == uniqueItemSpawn.Key);
                    if (randomizedUniqueSpawnToSwap.Key != null && randomizedUniqueSpawnToSwap.Key.CardNeededToAccess != uniqueItemSpawn.Value)
                    {
                        List<KeyValuePair<Location, Item>> acceptableLevelSpawns = setToCheck.PlantCard5Set.Entities.Where(spawn => spawn.Key.CardNeededToAccess == uniqueItemSpawn.Value 
                        && spawn.Key.MandatorySpawn
                        && !VanillaItems.ItemAccessLevels.ContainsKey(spawn.Value)
                        && spawn.Value.Name != "Card").ToList();
                        KeyValuePair<Location, Item> spawnToSwap = acceptableLevelSpawns[Randomizer.Next(0, acceptableLevelSpawns.Count - 1)];

                        setToCheck.PlantCard5Set.Entities[randomizedUniqueSpawnToSwap.Key] = spawnToSwap.Value;
                        setToCheck.PlantCard5Set.Entities[spawnToSwap.Key] = randomizedUniqueSpawnToSwap.Value;
                    }
                }
            }

            List<KeyValuePair<Location,Item>> firstProgressionSpawns = setToCheck.PlantCard5Set.Entities.Where(spawns => Location.FirstProgressionAreas.Contains(spawns.Key.GcxFile)).ToList();
            foreach (Item item in _vanillaItems.CardRandomizationFirstProgressionItems) 
            {
                if (!firstProgressionSpawns.Any(spawn => spawn.Value.Name == item.Name))
                {
                    if(item.Name == "SOCOM")
                    {
                        KeyValuePair<Location, Item> socomSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "SOCOM");
                        List<KeyValuePair<Location, Item>> part1SocomAmmoSpawns = firstProgressionSpawns.Where(spawn => spawn.Value.Name == "SOCOM Ammo" 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Weapons.Socom] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part1SocomAmmoSpawns[Randomizer.Next(0, part1SocomAmmoSpawns.Count -1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = socomSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[socomSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                    if(item.Name == "Coolant")
                    {
                        KeyValuePair<Location, Item> coolantSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "Coolant");
                        List<KeyValuePair<Location, Item>> part1M9AmmoSpawns = firstProgressionSpawns.Where(spawn => spawn.Value.Name == "M9 Ammo" 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Weapons.Coolant] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part1M9AmmoSpawns[Randomizer.Next(0, part1M9AmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = coolantSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[coolantSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                    if(item.Name == "Sensor B")
                    {
                        KeyValuePair<Location, Item> sensorBSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "Sensor B");
                        List<KeyValuePair<Location, Item>> part1M4AmmoSpawns = firstProgressionSpawns.Where(spawn => spawn.Value.Name == "M4 Ammo" && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Items.SensorB] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part1M4AmmoSpawns[Randomizer.Next(0, part1M4AmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = sensorBSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[sensorBSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                }
            }
            List<KeyValuePair<Location, Item>> secondProgressionSpawns = setToCheck.PlantCard5Set.Entities.Where(spawns => Location.SecondProgressionAreas.Contains(spawns.Key.GcxFile)
            && new string[] { "FrontDoor1", "FrontDoor2" }.Contains(spawns.Key.Name) == false).ToList();
            foreach (Item item in _vanillaItems.CardRandomizationSecondProgressionItems)
            {
                if (!secondProgressionSpawns.Any(spawn => spawn.Value.Name == item.Name))
                {
                    if (item.Name == "B.D.U.")
                    {
                        KeyValuePair<Location, Item> bduSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "B.D.U.");
                        List<KeyValuePair<Location, Item>> part2Rgb6AmmoSpawns = secondProgressionSpawns.Where(spawn => spawn.Value.Name == "RGB6 Ammo" 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Items.BDU] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part2Rgb6AmmoSpawns[Randomizer.Next(0, part2Rgb6AmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = bduSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[bduSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                    if (item.Name == "AKS-74u")
                    {
                        KeyValuePair<Location, Item> aks74uSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "AKS-74u");
                        List<KeyValuePair<Location, Item>> part2Aks74uAmmoSpawns = secondProgressionSpawns.Where(spawn => spawn.Value.Name == "AKS-74u Ammo" 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Weapons.Aks74u] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part2Aks74uAmmoSpawns[Randomizer.Next(0, part2Aks74uAmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = aks74uSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[aks74uSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                }
            }

            List<KeyValuePair<Location, Item>> thirdProgressionSpawns = setToCheck.PlantCard5Set.Entities.Where(spawns => Location.ThirdProgressionAreas.Contains(spawns.Key.GcxFile)
            && new string[] { "FrontDoor1", "FrontDoor2" }.Contains(spawns.Key.Name) == false).ToList();
            foreach (Item item in _vanillaItems.CardRandomizationThirdProgressionItems)
            {
                if (!thirdProgressionSpawns.Any(spawn => spawn.Value.Name == item.Name))
                {
                    if (item.Name == "Directional Microphone")
                    {
                        KeyValuePair<Location, Item> dmicSpawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "Directional Microphone");
                        List<KeyValuePair<Location, Item>> part3AnyAmmoSpawns = thirdProgressionSpawns.Where(spawn => spawn.Value.Name.Contains("Ammo") 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Weapons.Dmic1] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part3AnyAmmoSpawns[Randomizer.Next(0, part3AnyAmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = dmicSpawn.Value;
                        setToCheck.PlantCard5Set.Entities[dmicSpawn.Key] = ammoSpawnToSwap.Value;
                    }
                    if (item.Name == "PSG1")
                    {
                        KeyValuePair<Location, Item> psg1Spawn = setToCheck.PlantCard5Set.Entities.FirstOrDefault(spawn => spawn.Value.Name == "PSG1");
                        List<KeyValuePair<Location, Item>> part3Psg1AmmoSpawns = thirdProgressionSpawns.Where(spawn => spawn.Value.Name == "PSG1 Ammo" 
                        && spawn.Key.MandatorySpawn
                        && (keepCardAccessLevels ? spawn.Key.CardNeededToAccess == VanillaItems.ItemAccessLevels[MGS2Weapons.Psg1] : true)).ToList();
                        KeyValuePair<Location, Item> ammoSpawnToSwap = part3Psg1AmmoSpawns[Randomizer.Next(0, part3Psg1AmmoSpawns.Count-1)];

                        setToCheck.PlantCard5Set.Entities[ammoSpawnToSwap.Key] = psg1Spawn.Value;
                        setToCheck.PlantCard5Set.Entities[psg1Spawn.Key] = ammoSpawnToSwap.Value;
                    }
                }
            }
            #endregion

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
                if (kvp.Key.Contains("w0"))
                {
                    //this is here to allow any custom weapon spawns made for Raiden to work for Snake on Tanker levels.
                    List<int> plantWeaponReferences = GcxEditor.FindAllSubArray(newGcxBytes, new byte[] { 0x39, 0x21, 0x80, 0x02, 0xAC });
                    foreach(int index in plantWeaponReferences)
                    {
                        Array.Copy(new byte[] { 0x39, 0x21, 0x80, 0x01, 0x5C }, 0, newGcxBytes, index, 5);
                    }
                    //this is here to allow any custom item spawns made for Raiden to work for Snake on Tanker levels.
                    List<int> plantItemReferences = GcxEditor.FindAllSubArray(newGcxBytes, new byte[] { 0x39, 0x21, 0x80, 0x03, 0x3C });
                    foreach (int index in plantItemReferences)
                    {
                        Array.Copy(new byte[] { 0x39, 0x21, 0x80, 0x01, 0xEC }, 0, newGcxBytes, index, 5);
                    }
                }
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
