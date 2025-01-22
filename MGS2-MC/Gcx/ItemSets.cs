using MGS2_MC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    public class Location
    {
        public string GcxFile;
        public long PosX;
        public long PosY;
        public long PosZ;
        public long Rot;
        public byte[] SpawnId;
        public string Name;
        public bool MandatorySpawn;
        public string SisterSpawn;

        public Location()
        {
        }
        public Location(string gcxFile, byte[] spawnId, long posX, long posZ, long posY, long rot, bool mandatorySpawn = false, string sisterSpawn = null, string name = null)
        {
            GcxFile = gcxFile;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            Rot = rot;
            SpawnId = spawnId;
            MandatorySpawn = mandatorySpawn;
            SisterSpawn = sisterSpawn;
            Name = name;
        }
    }

    public class Item
    {
        public string Name;
        public byte Id;
        public RawProc ProcId;
    }

    public static class MGS2Items
    {
        public static readonly Item Ration = new Item { Name = "Ration", Id = GcxTableMapping.Ration, ProcId = KnownProc.AwardRation };
        public static readonly Item Scope1 = new Item { Name = "Scope", Id = GcxTableMapping.Scope1, ProcId = KnownProc.AwardScope };
        public static readonly Item ColdMeds = new Item { Name = "Cold Medicine", Id = GcxTableMapping.ColdMeds, ProcId = KnownProc.AwardColdMeds2 }; //TODO: VERIFY
        public static readonly Item Bandage = new Item { Name = "Bandage", Id = GcxTableMapping.Bandage, ProcId = KnownProc.AwardBandages };
        public static readonly Item Pentazemin = new Item { Name = "Pentazemin", Id = GcxTableMapping.Pentazemin, ProcId = KnownProc.AwardPentazemin };
        public static readonly Item BDU = new Item { Name = "B.D.U.", Id = GcxTableMapping.BDU, ProcId = KnownProc.AwardBDU };
        public static readonly Item BodyArmor = new Item { Name = "Body Armor", Id = GcxTableMapping.BodyArmor, ProcId = KnownProc.AwardBodyArmor };
        public static readonly Item Stealth = new Item { Name = "Stealth", Id = GcxTableMapping.Stealth };
        public static readonly Item MineDetector = new Item { Name = "Mine Detector", Id = GcxTableMapping.MineDetector, ProcId = KnownProc.AwardMineDetector };
        public static readonly Item SensorA = new Item { Name = "Sensor A", Id = GcxTableMapping.SensorA, ProcId = KnownProc.AwardSensorA };
        public static readonly Item SensorB = new Item { Name = "Sensor B", Id = GcxTableMapping.SensorB, ProcId = KnownProc.AwardSensorB };
        public static readonly Item NVG = new Item { Name = "Night Vision Goggles", Id = GcxTableMapping.NVG, ProcId = KnownProc.AwardNvg };
        public static readonly Item Thermals = new Item { Name = "Thermal Goggles", Id = GcxTableMapping.Thermals, ProcId = KnownProc.AwardThermalG };
        public static readonly Item Scope2 = new Item { Name = "Scope", Id = GcxTableMapping.Scope2 };
        public static readonly Item DigitalCamera = new Item { Name = "Digital Camera", Id = GcxTableMapping.DigitalCamera, ProcId = KnownProc.AwardDigitalCamera };
        public static readonly Item Box1 = new Item { Name = "Box1", Id = GcxTableMapping.Box1, ProcId = KnownProc.AwardBox1 };
        public static readonly Item Cigs = new Item { Name = "Cigarettes", Id = GcxTableMapping.Cigs, ProcId = KnownProc.AwardCigarettes };
        public static readonly Item Card = new Item { Name = "Card", Id = GcxTableMapping.Card, ProcId = KnownProc.AwardCard1 };
        public static readonly Item Shaver = new Item { Name = "Shaver", Id = GcxTableMapping.Shaver, ProcId = KnownProc.AwardShaver };
        public static readonly Item Phone = new Item { Name = "Phone", Id = GcxTableMapping.Phone, ProcId = KnownProc.AwardPhone };
        public static readonly Item Camera1 = new Item { Name = "Camera", Id = GcxTableMapping.Camera1, ProcId = KnownProc.AwardCamera };
        public static readonly Item Box2 = new Item { Name = "Box 2", Id = GcxTableMapping.Box2, ProcId = KnownProc.AwardBox2 };
        public static readonly Item Box3 = new Item { Name = "Box 3", Id = GcxTableMapping.Box3, ProcId = KnownProc.AwardBox3 };
        public static readonly Item WetBox = new Item { Name = "Wet Box", Id = GcxTableMapping.WetBox, ProcId = KnownProc.AwardWetBox };
        public static readonly Item APSensor = new Item { Name = "A.P. Sensor", Id = GcxTableMapping.APSensor, ProcId = KnownProc.AwardAPSensor };
        public static readonly Item Box4 = new Item { Name = "Box 4", Id = GcxTableMapping.Box4, ProcId = KnownProc.AwardBox4 };
        public static readonly Item Box5 = new Item { Name = "Box 5", Id = GcxTableMapping.Box5, ProcId = KnownProc.AwardBox5 };
        public static readonly Item SocomSupp = new Item { Name = "Socom Suppressor", Id = GcxTableMapping.SocomSupp, ProcId = KnownProc.AwardSocomSuppressor };
        public static readonly Item AkSupp = new Item { Name = "AK Suppressor", Id = GcxTableMapping.AkSupp, ProcId = KnownProc.AwardAksSuppressor };
        public static readonly Item Camera2 = new Item { Name = "Camera", Id = GcxTableMapping.Camera2 };
        public static readonly Item Bandana = new Item { Name = "Bandana", Id = GcxTableMapping.Bandana };
        public static readonly Item DogTags = new Item { Name = "Dog Tags", Id = GcxTableMapping.DogTags };
        public static readonly Item MoDisc = new Item { Name = "M.O. Disc", Id = GcxTableMapping.MoDisc, ProcId = KnownProc.AwardMODisk };
        public static readonly Item UspSupp = new Item { Name = "USP Suppressor", Id = GcxTableMapping.UspSupp, ProcId = KnownProc.AwardUspSuppressor };
        public static readonly Item InfWig = new Item { Name = "Infinity Wig", Id = GcxTableMapping.InfWig };
        public static readonly Item BlueWig = new Item { Name = "Blue Wig", Id = GcxTableMapping.BlueWig };
        public static readonly Item OrangeWig = new Item { Name = "Orange Wig", Id = GcxTableMapping.OrangeWig };
    }

    internal static class MGS2Weapons
    {
        public static readonly Item M9 = new Item { Name = "M9", Id = GcxTableMapping.M9, ProcId = KnownProc.AwardM9Gun };
        public static readonly Item M9Ammo = new Item { Name = "M9 Ammo", Id = GcxTableMapping.M9, ProcId = KnownProc.AwardM9Ammo };
        public static readonly Item Usp = new Item { Name = "USP", Id = GcxTableMapping.Usp, ProcId = KnownProc.AwardUsp };
        public static readonly Item UspAmmo = new Item { Name = "USP Ammo", Id = GcxTableMapping.Usp, ProcId = KnownProc.AwardUspAmmo };
        public static readonly Item Socom = new Item { Name = "SOCOM", Id = GcxTableMapping.Socom, ProcId = KnownProc.AwardSocom };
        public static readonly Item SocomAmmo = new Item { Name = "SOCOM Ammo", Id = GcxTableMapping.Socom, ProcId = KnownProc.AwardSocomAmmo };
        public static readonly Item Psg1 = new Item { Name = "PSG1", Id = GcxTableMapping.Psg1, ProcId = KnownProc.AwardPsg1Gun };
        public static readonly Item Psg1Ammo = new Item { Name = "PSG1 Ammo", Id = GcxTableMapping.Psg1, ProcId = KnownProc.AwardPsg1Ammo };
        public static readonly Item Rgb6 = new Item { Name = "RGB-6", Id = GcxTableMapping.Rgb6, ProcId = KnownProc.AwardRgbGun };
        public static readonly Item Rgb6Ammo = new Item { Name = "RGB-6 Ammo", Id = GcxTableMapping.Rgb6, ProcId = KnownProc.AwardRgbAmmo };
        public static readonly Item Nikita = new Item { Name = "Nikita", Id = GcxTableMapping.Nikita, ProcId = KnownProc.AwardNikitaGun };
        public static readonly Item NikitaAmmo = new Item { Name = "Nikita Ammo", Id = GcxTableMapping.Nikita, ProcId = KnownProc.AwardNikitaAmmo };
        public static readonly Item Stinger = new Item { Name = "Stinger", Id = GcxTableMapping.Stinger, ProcId = KnownProc.AwardStingerGun };
        public static readonly Item StingerAmmo = new Item { Name = "Stinger Ammo", Id = GcxTableMapping.Stinger, ProcId = KnownProc.AwardStingerAmmo };
        public static readonly Item Claymore = new Item { Name = "Claymore", Id = GcxTableMapping.Claymore, ProcId = KnownProc.AwardClaymore };
        public static readonly Item C4 = new Item { Name = "C4", Id = GcxTableMapping.C4, ProcId = KnownProc.AwardC4 };
        public static readonly Item Chaff = new Item { Name = "Chaff Grenade", Id = GcxTableMapping.Chaff, ProcId = KnownProc.AwardChaffG };
        public static readonly Item Stun = new Item { Name = "Stun Grenade", Id = GcxTableMapping.Stun, ProcId = KnownProc.AwardStunG };
        public static readonly Item Dmic1 = new Item { Name = "Directional Microphone", Id = GcxTableMapping.Dmic1, ProcId = KnownProc.AwardDirectionalMic };
        public static readonly Item HfBlade = new Item { Name = "H.F. Blade", Id = GcxTableMapping.HfBlade };
        public static readonly Item Coolant = new Item { Name = "Coolant", Id = GcxTableMapping.Coolant, ProcId = KnownProc.AwardCoolantSpray };
        public static readonly Item Aks74u = new Item { Name = "AKS-74u", Id = GcxTableMapping.Aks74u, ProcId = KnownProc.AwardAksGun };
        public static readonly Item Aks74uAmmo = new Item { Name = "AKS-74u Ammo", Id = GcxTableMapping.Aks74u, ProcId = KnownProc.AwardAksAmmo };
        public static readonly Item Magazine = new Item { Name = "Empty Magazine", Id = GcxTableMapping.Magazine };
        public static readonly Item Grenade = new Item { Name = "Grenade", Id = GcxTableMapping.Grenade, ProcId = KnownProc.AwardGrenade };
        public static readonly Item M4 = new Item { Name = "M4", Id = GcxTableMapping.M4, ProcId = KnownProc.AwardM4Gun };
        public static readonly Item M4Ammo = new Item { Name = "M4 Ammo", Id = GcxTableMapping.M4, ProcId = KnownProc.AwardM4Ammo };
        public static readonly Item Psg1t = new Item { Name = "PSG1-T", Id = GcxTableMapping.Psg1t, ProcId = KnownProc.AwardPsg1tGun };
        public static readonly Item Psg1tAmmo = new Item { Name = "PSG1-T Ammo", Id = GcxTableMapping.Psg1t, ProcId = KnownProc.AwardPsg1tAmmo };
        public static readonly Item Dmic2 = new Item { Name = " Directional Microphone", Id = GcxTableMapping.Dmic2 };
        public static readonly Item Book = new Item { Name = "Book", Id = GcxTableMapping.Book, ProcId = KnownProc.AwardBook };
    }

    internal static class MGS2Levels
    {
        public class Stage
        {
            public string Name;
            public string AreaCode;
        }

        public static class MainGameStages
        {
            public static readonly Stage AltDeck = new Stage { Name = "AltDeck", AreaCode = "w00a" };
            public static readonly Stage OlgaFight = new Stage { Name = "OlgaFight", AreaCode = "w00b" };
            public static readonly Stage NavigationalDeck = new Stage { Name = "NavigationalDeck", AreaCode = "w00c" };
            public static readonly Stage DeckACrewQuarters = new Stage { Name = "DeckACrewQuarters", AreaCode = "w01a" };
            public static readonly Stage DeckACrewQuartersStarboard = new Stage { Name = "DeckACrewQuartersStarboard", AreaCode = "w01b" };
            public static readonly Stage DeckCCrewQuarters = new Stage { Name = "DeckCCrewQuarters", AreaCode = "w01c" };
            public static readonly Stage DeckDCrewQuarters = new Stage { Name = "DeckDCrewQuarters", AreaCode = "w01d" };
            public static readonly Stage DeckEBridge = new Stage { Name = "DeckEBridge", AreaCode = "w01e" };
            public static readonly Stage DeckACrewLounge = new Stage { Name = "DeckACrewLounge", AreaCode = "w01f" };
            public static readonly Stage EngineRoom = new Stage { Name = "EngineRoom", AreaCode = "w02a" };
            public static readonly Stage Deck2Port = new Stage { Name = "Deck2Port", AreaCode = "w03a" };
            public static readonly Stage Deck2Starboard = new Stage { Name = "Deck2Starboard", AreaCode = "w03b" };
            public static readonly Stage Hold1 = new Stage { Name = "Hold1", AreaCode = "w04a" };
            public static readonly Stage Hold2 = new Stage { Name = "Hold2", AreaCode = "w04b" };
            public static readonly Stage Hold3 = new Stage { Name = "Hold3", AreaCode = "w04c" };
            public static readonly Stage SeaDock = new Stage { Name = "SeaDock", AreaCode = "w11a" }; //added
            public static readonly Stage SeaDockBombDisposal = new Stage { Name = "SeaDockBombDisposal", AreaCode = "w11b" }; //no unique spawns
            public static readonly Stage SeaDockFortune = new Stage { Name = "SeaDockFortune", AreaCode = "w11c" }; //will not add
            public static readonly Stage StrutARoof = new Stage { Name = "StrutARoof", AreaCode = "w12a" }; //added
            public static readonly Stage StrutARoofBomb = new Stage { Name = "StrutARoofBomb", AreaCode = "w12c" }; //handled by w12a sister spawn(s)
            public static readonly Stage StrutAPumpRoom = new Stage { Name = "StrutAPumpRoom", AreaCode = "w12b" }; //added
            public static readonly Stage ABConnectingBridge = new Stage { Name = "ABConnectingBridge", AreaCode = "w13a" }; //no spawns
            public static readonly Stage ABConnectingBridgeSensorB = new Stage { Name = "ABConnectingBridgeSensorB", AreaCode = "w13b" }; //no spawns
            public static readonly Stage TransformerRoom = new Stage { Name = "TransformerRoom", AreaCode = "w14a" }; //added
            public static readonly Stage BCConnectingBridge = new Stage { Name = "BCConnectingBridge", AreaCode = "w15a" }; //added
            public static readonly Stage BCConnectingBridgeAfterStillman = new Stage { Name = "BCConnectingBridgeAfterStillman", AreaCode = "w15b" }; //handled by w15a sister spawn(s)
            public static readonly Stage DiningHall = new Stage { Name = "DiningHall", AreaCode = "w16a" }; //added
            public static readonly Stage DiningHallAfterStillman = new Stage { Name = "DiningHallAfterStillman", AreaCode = "w16b" }; //handled by w16a sister spawn(s)
            public static readonly Stage CDConnectingBridge = new Stage { Name = "CDConnectingBridge", AreaCode = "w17a" }; //no spawns
            public static readonly Stage SedimentPool = new Stage { Name = "SedimentPool", AreaCode = "w18a" }; //added
            public static readonly Stage DEConnectingBridge = new Stage { Name = "DEConnectingBridge", AreaCode = "w19a" }; //added
            public static readonly Stage ParcelRoom = new Stage { Name = "ParcelRoom", AreaCode = "w20a" }; //added
            public static readonly Stage Heliport = new Stage { Name = "Heliport", AreaCode = "w20b" }; //added
            public static readonly Stage HeliportBomb = new Stage { Name = "HeliportBomb", AreaCode = "w20c" }; //will not add
            public static readonly Stage HeliportPostNinja = new Stage { Name = "HeliportPostNinja", AreaCode = "w20d" }; //handled by w20b sister spawn(s)
            public static readonly Stage EFConnectingBridge = new Stage { Name = "EFConnectingBridge", AreaCode = "w21a" }; //added
            public static readonly Stage EFConnectingBridge2 = new Stage { Name = "EFConnectingBridge", AreaCode = "w21b" }; //handled by w21a sister spawn(s)
            public static readonly Stage Warehouse = new Stage { Name = "Warehouse", AreaCode = "w22a" }; //added
            public static readonly Stage FAConnectingBridge = new Stage { Name = "FAConnectingBridge", AreaCode = "w23a" }; //added
            public static readonly Stage FAConnectingBridge2 = new Stage { Name = "FAConnectingBridgeAfterShell2", AreaCode = "w23b" }; //handled by w23a sister spawn(s)
            public static readonly Stage Shell1Core = new Stage { Name = "Shell1Core", AreaCode = "w24a" }; //added
            public static readonly Stage Shell1CoreB1 = new Stage { Name = "Shell1CoreB1", AreaCode = "w24b" }; //added
            public static readonly Stage Shell1CoreB2 = new Stage { Name = "Shell1CoreB2", AreaCode = "w24d" }; //added
            public static readonly Stage Shell1CoreHostageRoom = new Stage { Name = "Shell1CoreHostageRoom", AreaCode = "w24c" }; //added
            public static readonly Stage ShellsConnectingBridge = new Stage { Name = "ShellsConnectingBridge", AreaCode = "w25a" }; //will not add, is used for Harrier fight
            public static readonly Stage ShellsConnectingBridgeDestroyed = new Stage { Name = "ShellsConnectingBridgeDestroyed", AreaCode = "w25b" }; //added
            public static readonly Stage StrutLPerimeter = new Stage { Name = "StrutLPerimeter", AreaCode = "w25c" }; //added
            public static readonly Stage KLConnectingBridge = new Stage { Name = "KLConnectingBridge", AreaCode = "w25d" }; //added
            public static readonly Stage SewageTreatment = new Stage { Name = "SewageTreatment", AreaCode = "w28a" }; //added
            public static readonly Stage Shell2Core = new Stage { Name = "Shell2Core", AreaCode = "w31a" }; //added
            public static readonly Stage Shell2FiltrationChamber1 = new Stage { Name = "Shell2FiltrationChamber1", AreaCode = "w31b" }; //added
            public static readonly Stage Shell2FiltrationChamber2 = new Stage { Name = "Shell2FiltrationChamber2", AreaCode = "w31c" }; //added non-boss room spawns
            public static readonly Stage Shell2CoreWithEmma = new Stage { Name = "Shell2CoreWithEmma", AreaCode = "w31d" }; //added
            public static readonly Stage OilFence = new Stage { Name = "OilFence", AreaCode = "w32a" }; //will not add
            public static readonly Stage OilFenceVamp = new Stage { Name = "OilFenceVamp", AreaCode = "w32b" }; //will not add
            public static readonly Stage Stomach = new Stage { Name = "Stomach", AreaCode = "w41a" }; //added
            public static readonly Stage Jujenum = new Stage { Name = "Jujenum", AreaCode = "w42a" }; //added
            public static readonly Stage AscendingColon = new Stage { Name = "AscendingColon", AreaCode = "w43a" }; //added
            public static readonly Stage Ileum = new Stage { Name = "Ileum", AreaCode = "w44a" }; //no spawns
            public static readonly Stage SigmoidColon = new Stage { Name = "SigmoidColon", AreaCode = "w45a" }; //added
            public static readonly Stage Rectum = new Stage { Name = "Rectum", AreaCode = "w46a" }; //no spawns
            public static readonly Stage ArsenalGear = new Stage { Name = "ArsenalGear", AreaCode = "w51a" }; //will not add
            public static readonly Stage FederalHall = new Stage { Name = "FederalHall", AreaCode = "w61a" }; //will not add

            public static readonly List<Stage> PlayableStageList = new List<Stage> { AltDeck, OlgaFight, NavigationalDeck, DeckACrewQuarters, DeckACrewQuartersStarboard,
                DeckACrewLounge, DeckCCrewQuarters, DeckDCrewQuarters, DeckEBridge, EngineRoom, Deck2Port, Deck2Starboard, Hold1, Hold2, Hold3,SeaDock, SeaDockBombDisposal, SeaDockFortune, StrutARoof, StrutARoofBomb, StrutAPumpRoom,
                ABConnectingBridge, ABConnectingBridgeSensorB, TransformerRoom, BCConnectingBridge, BCConnectingBridgeAfterStillman, DiningHall, DiningHallAfterStillman,
                CDConnectingBridge, SedimentPool, DEConnectingBridge, ParcelRoom, Heliport, HeliportBomb, HeliportPostNinja, EFConnectingBridge, EFConnectingBridge2, Warehouse, FAConnectingBridge,
                Shell1Core, Shell1CoreB1, Shell1CoreB2, Shell1CoreHostageRoom, ShellsConnectingBridge, ShellsConnectingBridgeDestroyed, StrutLPerimeter, KLConnectingBridge,
                SewageTreatment, Shell2Core, Shell2FiltrationChamber1, Shell2FiltrationChamber2, Shell2CoreWithEmma, OilFence, OilFenceVamp, Stomach, Jujenum,
                AscendingColon, Ileum, SigmoidColon, Rectum, ArsenalGear, FederalHall, FAConnectingBridge2 };
        }
    }

    public class ItemSet
    {
        public string Name { get; set; }
        public Dictionary<Location, Item> Entities = new Dictionary<Location, Item>();
        public List<Item> ItemsNeededToProgress = new List<Item>();

        public ItemSet(ItemSet set)
        {
            Name = set.Name;
            foreach (KeyValuePair<Location, Item> item in set.Entities)
            {
                Entities.Add(item.Key, item.Value);
            }
            ItemsNeededToProgress.AddRange(set.ItemsNeededToProgress);
        }

        public ItemSet()
        {
            Name = string.Empty;
            Entities = new Dictionary<Location, Item>();
            ItemsNeededToProgress = new List<Item>();
        }
    }

    internal class MGS2ItemSet
    {
        public ItemSet TankerPart1 { get; set; } = new ItemSet();
        public ItemSet TankerPart2 { get; set; } = new ItemSet();
        public ItemSet TankerPart3 { get; set; } = new ItemSet();

        public ItemSet PlantSet1 { get; set; } = new ItemSet();
        public ItemSet PlantSet2 { get; set; } = new ItemSet();
        public ItemSet PlantSet3 { get; set; } = new ItemSet();
        public ItemSet PlantSet4 { get; set; } = new ItemSet();
        public ItemSet PlantSet5 { get; set; } = new ItemSet();
        public ItemSet PlantSet6 { get; set; } = new ItemSet();
        public ItemSet PlantSet7 { get; set; } = new ItemSet();
        public ItemSet PlantSet8 { get; set; } = new ItemSet();
        public ItemSet PlantSet9 { get; set; } = new ItemSet();
        public ItemSet PlantSet10 { get; set; } = new ItemSet();
    }

    public static class LogicRequirements
    {
        public static List<string> TankerProgressionItems = new List<string>
        {
            MGS2Items.Camera1.Name,
            MGS2Weapons.Usp.Name
        };

        public static List<string> ProgressionItems = new List<string>
        {
            MGS2Weapons.Psg1.Name,
            MGS2Weapons.Dmic1.Name,
            MGS2Weapons.Aks74u.Name,
            MGS2Weapons.Nikita.Name,
            MGS2Items.SensorB.Name,
            MGS2Weapons.Usp.Name,
            MGS2Items.Camera1.Name
        };

        public static List<string> AutoAwardedProgressionItems = new List<string>
        {
            MGS2Items.Card.Name,
            MGS2Weapons.Socom.Name,
            MGS2Weapons.Coolant.Name,
            MGS2Items.BDU.Name
        };

        static readonly ItemSet TankerPart1 = new ItemSet();
        static readonly ItemSet TankerPart2 = new ItemSet();
        static readonly ItemSet TankerPart3 = new ItemSet();

        static readonly ItemSet PlantSet1 = new ItemSet();
        static readonly ItemSet PlantSet2 = new ItemSet();
        static readonly ItemSet PlantSet3 = new ItemSet();
        static readonly ItemSet PlantSet4 = new ItemSet();
        static readonly ItemSet PlantSet5 = new ItemSet();
        static readonly ItemSet PlantSet6 = new ItemSet();
        static readonly ItemSet PlantSet7 = new ItemSet();
        static readonly ItemSet PlantSet8 = new ItemSet();
        static readonly ItemSet PlantSet9 = new ItemSet();

        static LogicRequirements()
        {
            TankerPart1.Name = "Before Olga";
            TankerPart1.Entities = new Dictionary<Location, Item>();
            TankerPart1.ItemsNeededToProgress.Add(MGS2Weapons.Usp); //USP or M9

            TankerPart2.Name = "After Olga, Before Deck 2";
            TankerPart2.Entities = new Dictionary<Location, Item>();
            TankerPart2.ItemsNeededToProgress.AddRange(TankerPart1.ItemsNeededToProgress);

            TankerPart3.Name = "Deck 2 & Beyond";
            TankerPart3.Entities = new Dictionary<Location, Item>();
            TankerPart3.ItemsNeededToProgress.AddRange(TankerPart2.ItemsNeededToProgress);
            TankerPart3.ItemsNeededToProgress.Add(MGS2Items.Camera1);

            PlantSet1.Name = "Before Pliskin";
            PlantSet1.Entities = new Dictionary<Location, Item>();
            //no items needed to progress beyond this

            PlantSet2.Name = "Before Stillman";
            PlantSet2.Entities = new Dictionary<Location, Item>();
            PlantSet2.ItemsNeededToProgress.AddRange(new Item[] { MGS2Weapons.Coolant, MGS2Items.Card }); //level 1 card;

            PlantSet3.Name = "Before Fatman";
            PlantSet3.Entities = new Dictionary<Location, Item>();
            PlantSet3.ItemsNeededToProgress.AddRange(PlantSet2.ItemsNeededToProgress);
            PlantSet3.ItemsNeededToProgress.Add(MGS2Weapons.Socom); //M9, socom, or Claymores, I think?

            PlantSet4.Name = "Before Shell 1 Elevator";
            PlantSet4.Entities = new Dictionary<Location, Item>();
            PlantSet4.ItemsNeededToProgress.AddRange(PlantSet3.ItemsNeededToProgress);
            PlantSet4.ItemsNeededToProgress.AddRange(new Item[] { MGS2Items.Card, MGS2Items.BDU, MGS2Weapons.Aks74u }); //level 2 card;

            PlantSet5.Name = "Before Ames";
            PlantSet5.Entities = new Dictionary<Location, Item>();
            PlantSet5.ItemsNeededToProgress.AddRange(PlantSet4.ItemsNeededToProgress);
            PlantSet5.ItemsNeededToProgress.Add(MGS2Weapons.Dmic1);

            PlantSet6.Name = "Before Shells Connecting Bridge";
            PlantSet6.Entities = new Dictionary<Location, Item>();
            PlantSet6.ItemsNeededToProgress.AddRange(PlantSet5.ItemsNeededToProgress);
            PlantSet6.ItemsNeededToProgress.AddRange(new Item[] { MGS2Weapons.Psg1, MGS2Items.Card }); //level 3 card

            PlantSet7.Name = "Before Johnson";
            PlantSet7.Entities = new Dictionary<Location, Item>();
            PlantSet7.ItemsNeededToProgress.AddRange(PlantSet6.ItemsNeededToProgress);
            PlantSet7.ItemsNeededToProgress.Add(MGS2Weapons.Nikita);

            PlantSet8.Name = "Before Emma";
            PlantSet8.Entities = new Dictionary<Location, Item>();
            PlantSet8.ItemsNeededToProgress.AddRange(PlantSet7.ItemsNeededToProgress);
            PlantSet8.ItemsNeededToProgress.Add(MGS2Items.Card); //level 4 card

            PlantSet9.Name = "Before Strut L";
            PlantSet9.Entities = new Dictionary<Location, Item>();
            PlantSet9.ItemsNeededToProgress.AddRange(PlantSet8.ItemsNeededToProgress);
            PlantSet9.ItemsNeededToProgress.Add(MGS2Items.Card); //level 5 card
        }
    }

    public static class VanillaItems
    {
        public static readonly ItemSet TankerPart1 = new ItemSet();
        public static readonly ItemSet TankerPart2 = new ItemSet();
        public static readonly ItemSet TankerPart3 = new ItemSet();

        public static readonly ItemSet PlantSet1 = new ItemSet();
        public static readonly ItemSet PlantSet2 = new ItemSet();
        public static readonly ItemSet PlantSet3 = new ItemSet();
        public static readonly ItemSet PlantSet4 = new ItemSet();
        public static readonly ItemSet PlantSet5 = new ItemSet();
        public static readonly ItemSet PlantSet6 = new ItemSet();
        public static readonly ItemSet PlantSet7 = new ItemSet();
        public static readonly ItemSet PlantSet8 = new ItemSet();
        public static readonly ItemSet PlantSet9 = new ItemSet();
        public static readonly ItemSet PlantSet10 = new ItemSet();

        public static void BuildVanillaItems()
        {
            #region Tanker
            TankerPart1.Name = "Before Olga";
            TankerPart1.Entities = new Dictionary<Location, Item>();
            #region w00a
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0xB7BC, posZ: 0, posY: 0xBBA4, rot: 1, name: "NearbyLeftsideBodyDump"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0x6F, 0x0E, 0xA8 }, posX: 0x1388, posZ: 0, posY: 0x493E, rot: 1, name: "NearSnakeSpawn"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0x4E, 0x9E, 0x26 }, posX: 0x3E80, posZ: 0xBB8, posY: 0xBD98, rot: 1, name: "RightsideLifeboats", mandatorySpawn: true), MGS2Items.Bandage);
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0x4F, 0x9E, 0x26 }, posX: 0xE1BA, posZ: 0, posY: 0x1194, rot: 1, name: "UnderLeftsideStairs", mandatorySpawn: true), MGS2Items.Bandage);
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0xDD, 0xC2, 0xAD }, posX: 0x4844, posZ: 0, posY: 0xBBA4, rot: 1, mandatorySpawn: true, name: "NearbyRightsideBodyDump"), MGS2Items.Pentazemin);
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0xA9, 0x42, 0x8B }, posX: 0x2710, posZ: 0x7D0, posY: 0xFE0C, rot: 1, name: "RightsideBehindCrates", mandatorySpawn: true), MGS2Weapons.Chaff);
            TankerPart1.Entities.Add(new Location(gcxFile: "w00a", spawnId: new byte[] { 0x57, 0x6C, 0x00 }, posX: 0x1770, posZ: 0x157C, posY: 0x0, rot: 3, name: "Overlook"), MGS2Items.ColdMeds); //not guaranteed spawn
            #endregion            
            #region w01a
            TankerPart1.Entities.Add(new Location(gcxFile: "w01a", spawnId: new byte[] { 0x79, 0x7E, 0x24 }, posX: 0x1194, posZ: 0, posY: 0xDDE, rot: 0, name: "Locker1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01a", spawnId: new byte[] { 0xFE, 0x69, 0x57 }, posX: 0xEE58, posZ: 0, posY: 0xDDE, rot: 0, name: "Locker2"), MGS2Items.Ration);
            #endregion
            #region w01b
            TankerPart1.Entities.Add(new Location(gcxFile: "w01b", spawnId: new byte[] { 0x40, 0x14, 0x9B }, posX: 0x2AF8, posZ: 0xBB8, posY: 0xC662, rot: 0, name: "UnderRightsideStairs", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01b", spawnId: new byte[] { 0xFE, 0x69, 0x57, 0x1 }, posX: 0xDAC, posZ: 0xBB8, posY: 0x8CA, rot: 0, name: "RightsideAlcove"), MGS2Items.Ration);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01b", spawnId: new byte[] { 0x9C, 0x7F, 0xD1, 0x2 }, posX: 0x3E8, posZ: 0xBB8, posY: 0x157C, rot: 0, name: "Locker", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            #endregion
            #region w01c
            TankerPart1.Entities.Add(new Location(gcxFile: "w01c", spawnId: new byte[] { 0xFE, 0x69, 0x57 }, posX: 0x251C, posZ: 0x1770, posY: 0xC086, rot: 0, name: "Crawlspace"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w01c", spawnId: new byte[] { 0xCB, 0x22, 0x66 }, posX: 0xFA24, posZ: 0x1838, posY: 0xB910, rot: 0, name: "Locker", mandatorySpawn: true), MGS2Weapons.Chaff);
            #endregion
            #region w01d
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0x49, 0x35, 0xB8 }, posX: 0xED72, posZ: 0x2292, posY: 0xCD38, rot: 0, name: "UnderTable1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0xD8F0, posZ: 0x2328, posY: 0xB7BC, rot: 0, name: "Stove"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0xF3, 0xB9, 0x75 }, posX: 0xDBDE, posZ: 0x2292, posY: 0xCD38, rot: 0, name: "UnderTable2"), MGS2Weapons.UspAmmo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0xF4, 0xB9, 0x75 }, posX: 0xF63C, posZ: 0x2328, posY: 0xD19D, rot: 0, name: "NextToCamera", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            //Box room spawns
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0x4A, 0x35, 0xB8 }, posX: 0x1E14, posZ: 0x2328, posY: 0xD1B6, rot: 0, name: "Pantry1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01d", spawnId: new byte[] { 0xF4, 0xF6, 0xAE }, posX: 0x2710, posZ: 0x2328, posY: 0xD8F0, rot: 0, name: "Pantry2"), MGS2Items.Box1);
            #endregion
            #region w01e
            TankerPart1.Entities.Add(new Location(gcxFile: "w01e", spawnId: new byte[] { 0xFE, 0x69, 0x57 }, posX: 0xEE6C, posZ: 0x2EE0, posY: 0xC27A, rot: 0, name: "NearLeftDoor"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w01e", spawnId: new byte[] { 0x9C, 0x7F, 0xD1 }, posX: 0x12D1, posZ: 0x2EE0, posY: 0xB706, rot: 0, name: "NearRightDoor", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            #endregion
            #region w01f
            //possible thermal spawn?
            TankerPart1.Entities.Add(new Location(gcxFile: "w01f", spawnId: new byte[] { 0x40, 0x14, 0x9B }, posX: 0x2328, posZ: 0, posY: 0xB7BC, rot: 0, name: "Bar", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location(gcxFile: "w01f", spawnId: new byte[] { 0xFE, 0x69, 0x57 }, posX: 0xDE68, posZ: 0, posY: 0xB8B6, rot: 0, name: "TV"), MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location(gcxFile: "w01f", spawnId: new byte[] { 0x9C, 0x7F, 0xD1 }, posX: 0xCF2C, posZ: 0xEC78, posY: 0xCF2C, rot: 0, name: "StinkyRationMan", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            #endregion

            TankerPart2.Name = "After Olga, Before Deck 2";
            TankerPart2.Entities = new Dictionary<Location, Item>();
            TankerPart1.Entities.ToList().ForEach(entity => TankerPart2.Entities.Add(entity.Key, entity.Value));
            TankerPart2.ItemsNeededToProgress.AddRange(TankerPart1.ItemsNeededToProgress);
            #region w00c
            TankerPart2.Entities.Add(new Location(gcxFile: "w00c", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0xD314, posZ: 0x2EE0, posY: 0xB6C2, rot: 1, name: "NearLeftsidePlatform"), MGS2Items.Ration);
            TankerPart2.Entities.Add(new Location(gcxFile: "w00c", spawnId: new byte[] { 0x6A, 0xC1, 0x11 }, posX: 0x4650, posZ: 0x31CE, posY: 0xC568, rot: 3, name: "RightsidePlatform"), MGS2Items.WetBox); //TODO: is this dependent on not having the item?
            TankerPart2.Entities.Add(new Location(gcxFile: "w00c", spawnId: new byte[] { 0x3D, 0xC7, 0x33 }, posX: 0x2EE, posZ: 0x6B6C, posY: 0xCD38, rot: 1, name: "CrowsNest1"), MGS2Items.Thermals); //TODO: is this dependent on not having the item?
            TankerPart2.Entities.Add(new Location(gcxFile: "w00c", spawnId: new byte[] { 0x92, 0x8A, 0x3 }, posX: 0xFD12, posZ: 0x5DC0, posY: 0xCC3E, rot: 0, name: "CrowsNest2"), MGS2Items.UspSupp); //not guaranteed spawn
            #endregion
            #region w01f
            TankerPart2.Entities.Add(new Location(gcxFile: "w01f", spawnId: new byte[] { 0xE8, 0x92, 0xE5 }, posX: 0x30D4, posZ: 0xEC78, posY: 0xCF2C, rot: 0, name: "EngineRoomAccess"), MGS2Weapons.Stun);
            #endregion
            #region w02a
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0xA2, 0xF9, 0xF9 }, posX: 0x1B58, posZ: 0xCD38, posY: 0xB3D4, rot: 0, name: "UnderRightsideStairs", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0x8CA, posZ: 0xC950, posY: 0xB9B0, rot: 0, name: "RightsideConnectingCatwalk"), MGS2Items.Ration);
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0x3A, 0xFB, 0x9E }, posX: 0xEA84, posZ: 0xCD38, posY: 0xAC04, rot: 0, name: "UnderLeftsideStairs", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0xF4, 0x6A, 0xA7 }, posX: 0xD120, posZ: 0xCD38, posY: 0x3E8, rot: 0, name: "LeftsideBottomFront"), MGS2Weapons.Grenade); //not guaranteed spawn
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0x3B, 0xFB, 0x9E }, posX: 0xE69C, posZ: 0xDCD8, posY: 0xD8F0, rot: 0, name: "LeftsideAlcove", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0x3C, 0xFB, 0x9E }, posX: 0xC180, posZ: 0xF060, posY: 0xA916, rot: 0, name: "SmallPortSideRoom", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location(gcxFile: "w02a", spawnId: new byte[] { 0x3D, 0xFB, 0x9E }, posX: 0x3F60, posZ: 0xF060, posY: 0xA7A6, rot: 0, name: "RavenCrates", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            #endregion

            TankerPart3.Name = "Deck 2 & Beyond";
            TankerPart3.Entities = new Dictionary<Location, Item>();
            TankerPart2.Entities.ToList().ForEach(entity => TankerPart3.Entities.Add(entity.Key, entity.Value));
            TankerPart3.ItemsNeededToProgress.AddRange(TankerPart2.ItemsNeededToProgress);
            #region w03a
            TankerPart3.Entities.Add(new Location(gcxFile: "w03a", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0xD026, posZ: 0xEC78, posY: 0xFFFF5BF0, rot: 0, name: "DeadguySideRoom"), MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location(gcxFile: "w03a", spawnId: new byte[] { 0xF3, 0xB9, 0x75 }, posX: 0xC950, posZ: 0xEC78, posY: 0xFFFEF46C, rot: 0, name: "CloseSideRoom", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location(gcxFile: "w03a", spawnId: new byte[] { 0xF4, 0xB9, 0x75 }, posX: 0xF060, posZ: 0xEC78, posY: 0xFFFE0C00, rot: 0, name: "MiddleShortHallway", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            //looks like we're missing 2 spawns?
            #endregion
            #region w03b
            TankerPart3.Entities.Add(new Location(gcxFile: "w03b", spawnId: new byte[] { 0xF3, 0xB9, 0x75 }, posX: 0x3D86, posZ: 0xEC78, posY: 0xFFFE0A0C, rot: 0, name: "StartOfHallway", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location(gcxFile: "w03b", spawnId: new byte[] { 0xF4, 0xB9, 0x75 }, posX: 0x31CE, posZ: 0xEC78, posY: 0xFFFE7866, rot: 0, name: "SideRoomPipes", mandatorySpawn: true), MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location(gcxFile: "w03b", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0x32C8, posZ: 0xEC78, posY: 0xFFFE841E, rot: 0, name: "Sideroom"), MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location(gcxFile: "w03b", spawnId: new byte[] { 0x49, 0x35, 0xB8 }, posX: 0x300C, posZ: 0xEC78, posY: 0xFFFE0912, rot: 0, name: "EntrywayPipes", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            #endregion
            #region w04a
            TankerPart3.Entities.Add(new Location(gcxFile: "w04a", spawnId: new byte[] { 0x49, 0x35, 0xB8 }, posX: 0xD508, posZ: 0xDCD8, posY: 0xFE0C, rot: 0, name: "LeftLadder1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart3.Entities.Add(new Location(gcxFile: "w04a", spawnId: new byte[] { 0x4A, 0x35, 0xB8 }, posX: 0x1B58, posZ: 0xEC78, posY: 0x6D6, rot: 0, name: "BehindHatch1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            TankerPart3.Entities.Add(new Location(gcxFile: "w04a", spawnId: new byte[] { 0x6E, 0x0E, 0xA8 }, posX: 0x2710, posZ: 0xEC78, posY: 0xCB2, rot: 0, name: "BehindHatch2"), MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location(gcxFile: "w04a", spawnId: new byte[] { 0x3D, 0xC7, 0x33 }, posX: 0xD6FC, posZ: 0xEC78, posY: 0xF254, rot: 0, name: "LeftLadder2"), MGS2Items.ColdMeds); //this is natively a thermal goggle

            #endregion
            #endregion

            #region Plant
            PlantSet1.Name = "Before Pliskin";
            PlantSet1.Entities = new Dictionary<Location, Item>();
            //no items needed to progress beyond this
            #region w11a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 121, 126, 36, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "WhiteCrates2", sisterSpawn: "w11b"), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 121, 126, 36, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "WhiteCrates1", sisterSpawn: "w11b"), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 159, 85, 254 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderWireRack", sisterSpawn: "w11b"), MGS2Weapons.M9);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker", sisterSpawn: "w11b"), MGS2Items.Ration);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 254, 105, 87, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightSideAlcove", sisterSpawn: "w11b"), MGS2Items.Ration);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 6, 175, 197 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "DivingCage"), MGS2Items.Shaver);
            PlantSet1.Entities.Add(new Location(gcxFile: "w11a", spawnId: new byte[] { 61, 199, 51 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Pool", sisterSpawn: "w11b"), MGS2Items.Thermals);
            #endregion
            #region w12a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet1.Entities.Add(new Location(gcxFile: "w12a", spawnId: new byte[] { 162, 73, 79, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Boxes", sisterSpawn: "w12c", mandatorySpawn: true), MGS2Items.Bandage);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12a", spawnId: new byte[] { 203, 34, 102, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightCage", sisterSpawn: "w12c", mandatorySpawn: true), MGS2Weapons.Chaff);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12a", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomLeft", sisterSpawn: "w12c", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12a", spawnId: new byte[] { 25, 201, 84, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftCage", sisterSpawn: "w12c"), MGS2Weapons.M9Ammo); //normally always M9, trying to reduce randomization issues
            #endregion
            #region w12b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet1.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderDeskNearNode", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 5, 255, 249, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "TopOfStairs", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 161, 8, 199, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker1", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker2"), MGS2Items.Ration);
            #endregion
            #region w23a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet1.Entities.Add(new Location(gcxFile: "w23a", spawnId: new byte[] { 203, 34, 102, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomBridgeLeftSide", sisterSpawn: "w23b", mandatorySpawn: true), MGS2Weapons.Chaff);
            #endregion
            #region w22a
            //top floor open area
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "OutsidePSG1Room", mandatorySpawn: true), MGS2Weapons.SocomAmmo); //outside psg1 room
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xFE, 0x69, 0x57, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "OutsideM9Room"), MGS2Items.Ration); //outside m9 room
            //bottom floor open area
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4F, 0x38, 0x60, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomFloorMiddleCrates", mandatorySpawn: true), MGS2Weapons.Book); //bottom floor, middle of room on top of boxes
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xE8, 0x92, 0xE5, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "OutsideAKRoom", mandatorySpawn: true), MGS2Weapons.Stun); //bottom floor, outside ak room
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x7E, 0x24, 0x04 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomFloorParkourBoxes", mandatorySpawn: true), MGS2Weapons.M9Ammo); //bottom floor, bottom right side crates
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x6 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomFloorRightCrates", mandatorySpawn: true), MGS2Weapons.SocomAmmo); //bottom floor, right side crates
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xCB, 0x22, 0x66, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "OutsideNodeRoom", mandatorySpawn: true), MGS2Weapons.Chaff); //bottom floor, outside node room
            //m9 room
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x9F, 0x55, 0xFE }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M9Room1"), MGS2Weapons.M9Ammo); //normally always M9, trying to reduce randomization issues
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x7E, 0x24, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M9Room2", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet1.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x7E, 0x24, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M9Room3", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            #endregion

            PlantSet2.Name = "Before Stillman";
            PlantSet2.Entities = new Dictionary<Location, Item>();
            PlantSet1.Entities.ToList().ForEach(entity => PlantSet2.Entities.Add(entity.Key, entity.Value));
            //PlantSet2.ItemsNeededToProgress.AddRange(new Item[] { MGS2Weapons.Coolant}); //level 1 card;
            #region w14a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet2.Entities.Add(new Location(gcxFile: "w14a", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Catwalk", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet2.Entities.Add(new Location(gcxFile: "w14a", spawnId: new byte[] { 161, 8, 199, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Node", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet2.Entities.Add(new Location(gcxFile: "w14a", spawnId: new byte[] { 161, 8, 199, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker1", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet2.Entities.Add(new Location(gcxFile: "w14a", spawnId: new byte[] { 106, 106, 9 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomSideElectrical"), MGS2Items.SocomSupp);
            PlantSet2.Entities.Add(new Location(gcxFile: "w14a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker2"), MGS2Items.Ration);
            #endregion
            #region w15a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet2.Entities.Add(new Location(gcxFile: "w15a", spawnId: new byte[] { 203, 34, 102, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BrokenBridge", sisterSpawn: "w15b", mandatorySpawn: true), MGS2Weapons.Chaff);
            #endregion

            PlantSet3.Name = "Before Fatman";
            PlantSet3.Entities = new Dictionary<Location, Item>();
            PlantSet2.Entities.ToList().ForEach(entity => PlantSet3.Entities.Add(entity.Key, entity.Value));
            PlantSet3.ItemsNeededToProgress.AddRange(PlantSet2.ItemsNeededToProgress);
            //PlantSet3.ItemsNeededToProgress.Add(MGS2Items.SensorB);
            PlantSet3.ItemsNeededToProgress.AddRange(new Item[] { MGS2Weapons.Socom, MGS2Weapons.Coolant });
            #region w12b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet3.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 254, 105, 87, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderPipes"), MGS2Items.Ration);
            PlantSet3.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 161, 8, 199, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderCamera", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 162, 73, 79, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UpperLeft", mandatorySpawn: true), MGS2Items.Bandage);
            //TODO: is this missing a spawn?
            PlantSet3.Entities.Add(new Location(gcxFile: "w12b", spawnId: new byte[] { 184, 235, 120, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerLeft"), MGS2Items.Box1); //TODO: is this dependent on not having the item?
            #endregion
            #region w16a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet3.Entities.Add(new Location(gcxFile: "w16a", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LadiesRoom1", sisterSpawn: "w16b", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w16a", spawnId: new byte[] { 161, 8, 199, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "MensRoom", sisterSpawn: "w16b", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w16a", spawnId: new byte[] { 107, 43, 115, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LadiesRoom2", sisterSpawn: "w16b", mandatorySpawn: true), MGS2Items.Pentazemin);
            PlantSet3.Entities.Add(new Location(gcxFile: "w16a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "DiningRoom", sisterSpawn: "w16b"), MGS2Items.Ration);
            PlantSet3.Entities.Add(new Location(gcxFile: "w16a", spawnId: new byte[] { 77, 235, 3, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "PantryRoom", sisterSpawn: "w16b"), MGS2Items.SensorB); //TODO: is this dependent on not having the item?
            #endregion
            #region w18a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet3.Entities.Add(new Location(gcxFile: "w18a", spawnId: new byte[] { 121, 126, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderStairs", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w18a", spawnId: new byte[] { 77, 171, 249 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideDoor", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w18a", spawnId: new byte[] { 187, 113, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideDoor", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w18a", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "CenterAlcove"), MGS2Items.Ration);
            #endregion
            #region w19a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet3.Entities.Add(new Location(gcxFile: "w19a", spawnId: new byte[] { 187, 113, 139, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerBridgeRightside1", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w19a", spawnId: new byte[] { 232, 146, 229, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerBridgeRightside2", mandatorySpawn: true), MGS2Weapons.Stun);
            #endregion
            #region w20a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 217, 115, 234 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.MineDetector); //TODO: is this dependent on not having the item?
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomFloorCrawlspace"), MGS2Items.Ration);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 156, 53, 76 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.Box5); //TODO: is this dependent on not having the item?
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 156, 53, 76 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideFastTravel"), MGS2Items.Bandage); //TODO: is this dependent on not having the item? - normally Box5, trying to cut down on randomization issues
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 121, 106, 36, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomsideMiddleBoxes", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 121, 126, 36, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideMiddleBoxes"), MGS2Weapons.M9Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 152, 131, 164, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.Psg1tAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 187, 113, 139, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderWireRackByEDBridge", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 187, 113, 139, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderWireRackByNode", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 233, 212, 177, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.StingerAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 232, 146, 229, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderConveyerBelt", mandatorySpawn: true), MGS2Weapons.Stun);
            #endregion
            #region w20b
            //TODO: go through and verify these aren't messed up(irt boss fight)
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 184, 235, 120, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Items.Box3); //TODO: is this dependent on not having the item?
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 171, 129, 234, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.Claymore);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.M9Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 5, 255, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.M9Ammo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 4 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 161, 8, 199, 3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.SocomAmmo);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 232, 146, 229, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Weapons.Stun);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Items.Ration);
            PlantSet3.Entities.Add(new Location(gcxFile: "w20b", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, sisterSpawn: "w20d"), MGS2Items.Ration);
            #endregion
            #region w21a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet3.Entities.Add(new Location(gcxFile: "w21a", spawnId: new byte[] { 120, 198, 100, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NearShell1Core", sisterSpawn: "w21b", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            #endregion
            #region w22a
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x4 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomAcrossNode1", mandatorySpawn: true), MGS2Weapons.SocomAmmo); //room across from node
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xD9, 0x73, 0xEA }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomAcrossNode3.1"), MGS2Items.Bandage); //room across from node - dependent on not having? -yes, normally mine detector
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x05 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomAcrossNode3.2"), MGS2Weapons.SocomAmmo); //room across from node(if suppressor has been picked up)
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x6A, 0x6A, 0x9 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomAcrossNode2"), MGS2Weapons.SocomAmmo); //room across from node - dependent on not having?
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 33, 142, 94 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomWithNode", mandatorySpawn: true), MGS2Items.Box2); //room with node
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 107, 43, 115 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerNearNode1", mandatorySpawn: true), MGS2Items.Pentazemin); //locker near node
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 254, 105, 87, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerNearNode2"), MGS2Items.Ration); //locker near node
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x7E, 0x24, 0x03 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RoomAcrossNode4", mandatorySpawn: true), MGS2Weapons.M9Ammo); //room across from node
            PlantSet3.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4F, 0x38, 0x60, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerNearNode3", mandatorySpawn: true), MGS2Weapons.Book); //locker near node
            #endregion

            PlantSet4.Name = "Before Shell 1 Elevator";
            PlantSet4.Entities = new Dictionary<Location, Item>();
            PlantSet3.Entities.ToList().ForEach(entity => PlantSet4.Entities.Add(entity.Key, entity.Value));
            PlantSet4.ItemsNeededToProgress.AddRange(PlantSet3.ItemsNeededToProgress);
            PlantSet4.ItemsNeededToProgress.AddRange(new Item[] { MGS2Items.BDU, MGS2Weapons.Aks74u }); //level 2 card;
            #region w22a
            //ak room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x30, 0x65, 0xC2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom1"), MGS2Items.AkSupp); //ak & m4 room, easy difficulty & other params i dont know
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xB9, 0x2E, 0xA, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom2", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo); //ak & m4 room (one of these is linked to AK spawn)
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xB9, 0x2E, 0xA, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom3", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo); //ak & m4 room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xB9, 0x2E, 0xA, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom4.2"), MGS2Weapons.Aks74uAmmo); //ak & m4 room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x36, 0x55, 0xBF }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom4.1"), MGS2Weapons.Aks74u); //ak & m4 room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom5", mandatorySpawn: true), MGS2Weapons.SocomAmmo); //ak & m4 room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x6A, 0x24, 0x5 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AKRoom6", mandatorySpawn: true), MGS2Weapons.M4Ammo); //ak & m4 room
            //c4 & claymore room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x14, 0xA1, 0x54, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "C4Room1", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x14, 0xA1, 0x54, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "C4Room2", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x14, 0xA1, 0x54, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "C4Room3", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xAB, 0x81, 0xEA, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "C4Room4", mandatorySpawn: true), MGS2Weapons.Claymore);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xAB, 0x81, 0xEA, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "C4Room5"), MGS2Weapons.Claymore); //conditional spawn
            //m4 room
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x6A, 0x24, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room1", mandatorySpawn: true), MGS2Weapons.M4Ammo); //one of these is linked to M4 spawn
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x6A, 0x24, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room2", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x6A, 0x24, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room3", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x79, 0x6A, 0x24, 0x4 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room4.2"), MGS2Weapons.M4Ammo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x9F, 0x55, 0xAE }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room4.1"), MGS2Weapons.M4);
            PlantSet4.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4D, 0xAB, 0xF9, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "M4Room5", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            #endregion
            #region w24a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 133, 194, 100 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom1"), MGS2Weapons.Aks74uAmmo); //normally aks74u
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 168, 139, 53 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom2", mandatorySpawn: true), MGS2Weapons.Book);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 20, 161, 84 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom3", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 203, 34, 102 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom4", mandatorySpawn: true), MGS2Weapons.Chaff);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 171, 129, 234 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom5", mandatorySpawn: true), MGS2Weapons.Claymore);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 121, 106, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom6", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 121, 126, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom7", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 187, 113, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BrokenDoor", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet4.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 242, 132, 47 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LockerRoom8"), MGS2Weapons.SocomAmmo); //normally socom suppressor
            #endregion

            PlantSet5.Name = "Before Ames";
            PlantSet5.Entities = new Dictionary<Location, Item>();
            PlantSet4.Entities.ToList().ForEach(entity => PlantSet5.Entities.Add(entity.Key, entity.Value));
            PlantSet5.ItemsNeededToProgress.AddRange(PlantSet4.ItemsNeededToProgress);
            PlantSet5.ItemsNeededToProgress.Add(MGS2Weapons.Dmic1);
            #region w24b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet5.Entities.Add(new Location(gcxFile: "w24b", spawnId: new byte[] { 73, 33, 184 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Hallway", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24b", spawnId: new byte[] { 245, 3, 119 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LoungeLocker1", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24b", spawnId: new byte[] { 167, 184, 75 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LoungeLocker2", mandatorySpawn: true), MGS2Weapons.Stun);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24b", spawnId: new byte[] { 110, 14, 168 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NodeLocker"), MGS2Items.Ration);
            #endregion
            #region w24c
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet5.Entities.Add(new Location(gcxFile: "w24c", spawnId: new byte[] { 78, 158, 38 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomLeft", mandatorySpawn: true), MGS2Items.Bandage);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24c", spawnId: new byte[] { 110, 14, 168 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BottomRight"), MGS2Items.Ration);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24c", spawnId: new byte[] { 61, 199, 51 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Podium"), MGS2Items.Pentazemin); //normally Thermals
            #endregion
            #region w24d
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 73, 125, 248 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Node1", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 162, 73, 79 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "UnderDesk", mandatorySpawn: true), MGS2Items.Bandage);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 7, 247, 174 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Node2"), MGS2Items.Box4); //is this dependent on not having the item?
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 167, 38, 53 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Parrot"), MGS2Weapons.Dmic1); //is this dependent on not having the item?
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 3, 46, 14 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker1", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 8, 46, 14 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker2", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 187, 29, 185 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker3", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet5.Entities.Add(new Location(gcxFile: "w24d", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightAlcove"), MGS2Items.Ration);
            #endregion

            PlantSet6.Name = "Before Shells Connecting Bridge";
            PlantSet6.Entities = new Dictionary<Location, Item>();
            PlantSet5.Entities.ToList().ForEach(entity => PlantSet6.Entities.Add(entity.Key, entity.Value));
            PlantSet6.ItemsNeededToProgress.AddRange(PlantSet5.ItemsNeededToProgress);
            PlantSet6.ItemsNeededToProgress.AddRange(new Item[] { MGS2Weapons.Psg1 }); //level 3 card
            #region w22a
            //RGB6 room
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x98, 0x83, 0xA4, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RgbRoom1", mandatorySpawn: true), MGS2Weapons.Psg1tAmmo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4D, 0xBC, 0xAB, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RgbRoom2", mandatorySpawn: true), MGS2Weapons.Rgb6Ammo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4D, 0xBC, 0xAB, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RgbRoom3.2"), MGS2Weapons.Rgb6Ammo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBC, 0xA7, 0xF5 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RgbRoom3.1"), MGS2Weapons.Rgb6);
            //PSG1 room
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4D, 0xAB, 0xF9, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Psg1Room1", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x4D, 0xAB, 0xF9, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Psg1Room2", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xF4, 0xA8, 0xB1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Psg1Room3"), MGS2Weapons.Psg1);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xA0, 0xD3, 0x12 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Psg1Room4"), MGS2Weapons.Psg1t);
            //Grenade room
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xD5, 0xE9, 0x78, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "GrenadeRoom1", mandatorySpawn: true), MGS2Weapons.Grenade);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xD5, 0xE9, 0x78, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "GrenadeRoom2", mandatorySpawn: true), MGS2Weapons.Grenade);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xD5, 0xE9, 0x78, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "GrenadeRoom3", mandatorySpawn: true), MGS2Weapons.Grenade);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0x98, 0x83, 0xA4, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "GrenadeRoom4", mandatorySpawn: true), MGS2Weapons.Psg1tAmmo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w22a", spawnId: new byte[] { 0xBB, 0x71, 0x8B, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "GrenadeRoom5", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            #endregion
            #region w24a
            PlantSet6.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 79, 43, 183 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "FrontDoor1", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet6.Entities.Add(new Location(gcxFile: "w24a", spawnId: new byte[] { 6, 204, 62 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "FrontDoor2", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            #endregion

            PlantSet7.Name = "Before Johnson";
            PlantSet7.Entities = new Dictionary<Location, Item>();
            PlantSet6.Entities.ToList().ForEach(entity => PlantSet7.Entities.Add(entity.Key, entity.Value));
            PlantSet7.ItemsNeededToProgress.AddRange(PlantSet6.ItemsNeededToProgress);
            PlantSet7.ItemsNeededToProgress.Add(MGS2Weapons.Nikita);
            #region w25b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 47, 220, 148 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Pipe"), MGS2Weapons.Aks74uAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 48, 101, 194 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BehindFire"), MGS2Weapons.Aks74uAmmo); //normally AkSupp
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 77, 171, 249 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BrokenStairs"), MGS2Weapons.Psg1Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 77, 171, 249 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BrokenCatwalk"), MGS2Weapons.Psg1Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 152, 131, 164 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "FrontFire"), MGS2Weapons.Psg1tAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.Ration);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25b", spawnId: new byte[] { 70, 8, 124 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.Ration);
            #endregion
            #region w25c
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet7.Entities.Add(new Location(gcxFile: "w25c", spawnId: new byte[] { 185, 46, 10 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerCatwalk", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25c", spawnId: new byte[] { 191, 66, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ConnectingBridge", mandatorySpawn: true), MGS2Weapons.Chaff);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25c", spawnId: new byte[] { 77, 171, 249 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ConnectingBridge", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25c", spawnId: new byte[] { 187, 113, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ConnectingBridge", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w25c", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerCatwalk"), MGS2Items.Ration);
            #endregion
            #region w31a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 79, 56, 96, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.Book);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 203, 34, 102, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "StrutHDoor1", mandatorySpawn: true), MGS2Weapons.Chaff);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 121, 106, 36, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideAlcove1", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 121, 106, 36, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideBoxes", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 159, 85, 174 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideStairs"), MGS2Weapons.M4Ammo); //normally M4
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 121, 126, 36, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideStairs", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 201, 233, 133, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: ""), MGS2Weapons.NikitaAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 201, 233, 133, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: ""), MGS2Weapons.NikitaAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 201, 233, 133, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "CameraRoom", mandatorySpawn: true), MGS2Weapons.NikitaAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 201, 233, 133, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "StrutHDoor2", mandatorySpawn: true), MGS2Weapons.NikitaAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 188, 167, 245 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideAlcove2"), MGS2Weapons.Rgb6Ammo); //normally Rgb6
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 187, 113, 139, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ElectricalRoom", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 187, 113, 139, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BreakRoom", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 233, 212, 177, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.StingerAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideLanding"), MGS2Items.Ration);
            #endregion
            #region w31b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 213, 233, 120 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.Grenade);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 121, 106, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "MiddleHallwayAlcove", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 121, 126, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.M4Ammo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 5, 7, 102 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "DeadendMiddleHallway"), MGS2Items.NVG); //is this dependent on not having the item?
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 102, 1, 22 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NikitaSpawn1"), MGS2Weapons.Nikita);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 102, 1, 22 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NikitaSpawn2"), MGS2Weapons.NikitaAmmo); //normally Nikita
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 102, 1, 22 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NikitaSpawn3"), MGS2Weapons.NikitaAmmo); //normally Nikita
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 26, 169, 156 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "CollapsedRoom1"), MGS2Weapons.Psg1tAmmo); //normally psg1t
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 232, 234, 201 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AirPocket2"), MGS2Weapons.Rgb6Ammo); //normally rgb6
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 233, 212, 177 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "CollapsedRoom2", mandatorySpawn: true), MGS2Weapons.StingerAmmo);
            PlantSet7.Entities.Add(new Location(gcxFile: "w31b", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "AirPocket3"), MGS2Items.Ration);
            #endregion

            PlantSet8.Name = "Before Emma";
            PlantSet8.Entities = new Dictionary<Location, Item>();
            PlantSet7.Entities.ToList().ForEach(entity => PlantSet8.Entities.Add(entity.Key, entity.Value));
            PlantSet8.ItemsNeededToProgress.AddRange(PlantSet7.ItemsNeededToProgress); //level 4 card
            #region w31c
            /*Vamp fight spawns:
             * M9 - 1247E79, C950, E796, FFFC49CE x3
             * RGB - 1ABBC4D, ED72, E796, FFFC44EC x3
             * SOCOM - 18B71BB, C950, E796, FFFC6A08 x3 
             * Ration - 15769FE, E890, E796, FFFC6A08 x4
             * AK - 1246A79, E890, E796, FFFC4CBC x4
             * M4 - 1246A79, E890, E796, FFFC4CBC x4
            */
            //TODO: give spawns names
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0xB9, 0x2E, 0x0A, 0x1 }, posX: 0xB9B0, posZ: 0xDAE4, posY: 0xFFFC25AC, rot: 0, name: "SideRoom1", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0xA2, 0x63, 0xF1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker1"), MGS2Items.BodyArmor); //is this dependent on not having the item?
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x4F, 0x38, 0x60, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker2", mandatorySpawn: true), MGS2Weapons.Book);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x14, 0xA1, 0x54, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker3", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x14, 0xA1, 0x54, 0x2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "DeadendHallway", mandatorySpawn: true), MGS2Weapons.C4);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x98, 0x83, 0xA4, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "SideRoom2", mandatorySpawn: true), MGS2Weapons.Psg1tAmmo);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x6B, 0x2B, 0x73, 0x1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NodeStairs", mandatorySpawn: true), MGS2Items.Pentazemin);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0xFE, 0x69, 0x57, 0x3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "OutsidePurificationChamber"), MGS2Items.Ration);
            PlantSet8.Entities.Add(new Location(gcxFile: "w31c", spawnId: new byte[] { 0x3D, 0xC7, 0x33 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Locker4"), MGS2Items.Pentazemin); //normally Thermals
            #endregion

            PlantSet9.Name = "Before Strut L";
            PlantSet9.Entities = new Dictionary<Location, Item>();
            PlantSet8.Entities.ToList().ForEach(entity => PlantSet9.Entities.Add(entity.Key, entity.Value));
            PlantSet9.ItemsNeededToProgress.AddRange(PlantSet8.ItemsNeededToProgress); //level 5 card
            #region w25d
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet9.Entities.Add(new Location(gcxFile: "w25d", spawnId: new byte[] { 203, 34, 102 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ShellEntrance", mandatorySpawn: true), MGS2Weapons.Chaff);
            PlantSet9.Entities.Add(new Location(gcxFile: "w25d", spawnId: new byte[] { 121, 106, 36 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerCatwalk", mandatorySpawn: true), MGS2Weapons.M4Ammo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w25d", spawnId: new byte[] { 77, 171, 249 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ShellEntrance", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w25d", spawnId: new byte[] { 187, 113, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LowerCatwalk", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w25d", spawnId: new byte[] { 254, 105, 87 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ShellEntrance"), MGS2Items.Ration);
            #endregion
            #region w31d
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 185, 46, 10, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "RightsideAlcove", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 185, 46, 10, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ElectricalRoom1", mandatorySpawn: true), MGS2Weapons.Aks74uAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 121, 126, 36, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ElectricalRoom2", mandatorySpawn: true), MGS2Weapons.M9Ammo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 77, 171, 249, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ElectricalRoom3", mandatorySpawn: true), MGS2Weapons.Psg1Ammo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 152, 131, 164, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.Psg1tAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 77, 188, 171, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Weapons.Rgb6Ammo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 187, 113, 139, 3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideAlcove", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 187, 113, 139, 4 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "StrutHDoor", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 107, 43, 115, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftsideLanding", mandatorySpawn: true), MGS2Items.Pentazemin);
            PlantSet9.Entities.Add(new Location(gcxFile: "w31d", spawnId: new byte[] { 254, 105, 87, 2 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "ElectricalRoom4"), MGS2Items.Ration);
            #endregion

            PlantSet10.Name = "After Strut L";
            PlantSet10.Entities = new Dictionary<Location, Item>();
            PlantSet9.Entities.ToList().ForEach(entity => PlantSet10.Entities.Add(entity.Key, entity.Value));
            PlantSet10.ItemsNeededToProgress.AddRange(PlantSet9.ItemsNeededToProgress);
            #region w20a
            PlantSet10.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 150, 156, 218 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "Level5DoorRoom"), MGS2Items.DigitalCamera);
            PlantSet10.Entities.Add(new Location(gcxFile: "w20a", spawnId: new byte[] { 187, 113, 139, 3 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "PerimeterAccessRoom", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            #endregion
            #region w21b
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet10.Entities.Add(new Location(gcxFile: "w21b", spawnId: new byte[] { 203, 34, 102, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "NearStrutF", mandatorySpawn: true), MGS2Weapons.Chaff);
            #endregion
            #region w28a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet10.Entities.Add(new Location(gcxFile: "w28a", spawnId: new byte[] { 187, 113, 139 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "SewageTreatment", mandatorySpawn: true), MGS2Weapons.SocomAmmo);
            #endregion
            #region w41a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet10.Entities.Add(new Location(gcxFile: "w41a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "TortureRoom", mandatorySpawn: true), MGS2Items.Ration);
            #endregion
            #region w42a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            //TODO: give spawns names
            PlantSet10.Entities.Add(new Location(gcxFile: "w42a", spawnId: new byte[] { 8, 247, 174, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "LeftCatwalk"), MGS2Items.Pentazemin); //normally box5
            PlantSet10.Entities.Add(new Location(gcxFile: "w42a", spawnId: new byte[] { 21, 128, 209, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BackLeftMainArea"), MGS2Items.Pentazemin); //normally coldmeds
            PlantSet10.Entities.Add(new Location(gcxFile: "w42a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.Ration);
            #endregion
            #region w43a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet10.Entities.Add(new Location(gcxFile: "w43a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "BackOfHallway"), MGS2Items.Ration);
            #endregion
            #region w45a
            //verified with new ID acquisiton(still not crazy about decimal instead of hex)
            PlantSet10.Entities.Add(new Location(gcxFile: "w45a", spawnId: new byte[] { 254, 105, 87, 1 }, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0, name: "EntryRoom"), MGS2Items.Ration);
            #endregion
            #endregion
        }
    }
}
