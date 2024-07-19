using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class Location
    {
        public string GcxFile;
        public float PosX;
        public float PosY;
        public float PosZ;
        public float Rot;
        public byte[] SpawnId;
        internal List<byte> ParameterBytes;

        public Location()
        {
            BuildParameterBytes();
        }
        public Location(string gcxFile, byte[] spawnId, float posX, float posZ, float posY, float rot)
        {
            GcxFile = gcxFile;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            Rot = rot;
            SpawnId = spawnId;
            BuildParameterBytes();
        }

        private void BuildParameterBytes()
        {
            ParameterBytes = new List<byte>();
            ParameterBytes.AddRange(SpawnId);
            ParameterBytes.AddRange(BitConverter.GetBytes(PosX));
            ParameterBytes.AddRange(BitConverter.GetBytes(PosZ));
            ParameterBytes.AddRange(BitConverter.GetBytes(PosY));
            //is rot really sent as a 0, or as C1?
        }
    }

    internal class Item
    {
        public string Name;
        public byte Id;
        public RawProc ProcId;
    }

    internal static class MGS2Items
    {
        public static readonly Item Ration = new Item { Name = "Ration", Id = GcxTableMapping.Ration, ProcId = KnownProc.AwardRation };
        public static readonly Item Scope1 = new Item { Name = "Scope", Id = GcxTableMapping.Scope1};
        public static readonly Item ColdMeds = new Item { Name = "Cold Medicine", Id = GcxTableMapping.ColdMeds, ProcId = KnownProc.AwardColdMeds };
        public static readonly Item Bandage = new Item { Name = "Bandage", Id = GcxTableMapping.Bandage, ProcId = KnownProc.AwardBandages };
        public static readonly Item Pentazemin = new Item { Name = "Pentazemin", Id = GcxTableMapping.Pentazemin, ProcId = KnownProc.AwardPentazemin };
        public static readonly Item BDU = new Item { Name = "B.D.U.", Id = GcxTableMapping.BDU };
        public static readonly Item BodyArmor = new Item { Name = "Body Armor", Id = GcxTableMapping.BodyArmor, ProcId = KnownProc.AwardBodyArmor };
        public static readonly Item Stealth = new Item { Name = "Stealth", Id = GcxTableMapping.Stealth };
        public static readonly Item MineDetector = new Item { Name = "Mine Detector", Id = GcxTableMapping.MineDetector, ProcId = KnownProc.AwardMineDetector };
        public static readonly Item SensorA = new Item { Name = "Sensor A", Id = GcxTableMapping.SensorA };
        public static readonly Item SensorB = new Item { Name = "Sensor B", Id = GcxTableMapping.SensorB, ProcId = KnownProc.AwardSensorB };
        public static readonly Item NVG = new Item { Name = "Night Vision Goggles", Id = GcxTableMapping.NVG, ProcId = KnownProc.AwardNvg };
        public static readonly Item Thermals = new Item { Name = "Thermal Goggles", Id = GcxTableMapping.Thermals, ProcId = KnownProc.AwardThermalG };
        public static readonly Item Scope2 = new Item { Name = "Scope", Id = GcxTableMapping.Scope2 };
        public static readonly Item DigitalCamera = new Item { Name = "Digital Camera", Id = GcxTableMapping.DigitalCamera, ProcId = KnownProc.AwardDigitalCamera };
        public static readonly Item Box1 = new Item { Name = "Box1", Id = GcxTableMapping.Box1, ProcId = KnownProc.AwardBox1 };
        public static readonly Item Cigs = new Item { Name = "Cigarettes", Id = GcxTableMapping.Cigs };
        public static readonly Item Card = new Item { Name = "Card", Id = GcxTableMapping.Card };
        public static readonly Item Shaver = new Item { Name = "Shaver", Id = GcxTableMapping.Shaver, ProcId = KnownProc.AwardShaver };
        public static readonly Item Phone = new Item { Name = "Phone", Id = GcxTableMapping.Phone };
        public static readonly Item Camera1 = new Item { Name = "Camera", Id = GcxTableMapping.Camera1 };
        public static readonly Item Box2 = new Item { Name = "Box 2", Id = GcxTableMapping.Box2, ProcId = KnownProc.AwardBox2 };
        public static readonly Item Box3 = new Item { Name = "Box 3", Id = GcxTableMapping.Box3, ProcId = KnownProc.AwardBox3 };
        public static readonly Item WetBox = new Item { Name = "Wet Box", Id = GcxTableMapping.WetBox, ProcId = KnownProc.AwardWetBox };
        public static readonly Item APSensor = new Item { Name = "A.P. Sensor", Id = GcxTableMapping.APSensor };
        public static readonly Item Box4 = new Item { Name = "Box 4", Id = GcxTableMapping.Box4, ProcId = KnownProc.AwardBox4 };
        public static readonly Item Box5 = new Item { Name = "Box 5", Id = GcxTableMapping.Box5, ProcId = KnownProc.AwardBox5 };
        public static readonly Item SocomSupp = new Item { Name = "Socom Suppressor", Id = GcxTableMapping.SocomSupp, ProcId = KnownProc.AwardSocomSuppressor };
        public static readonly Item AkSupp = new Item { Name = "AK Suppressor", Id = GcxTableMapping.AkSupp, ProcId = KnownProc.AwardAksSuppressor };
        public static readonly Item Camera2 = new Item { Name = "Camera", Id = GcxTableMapping.Camera2 };
        public static readonly Item Bandana = new Item { Name = "Bandana", Id = GcxTableMapping.Bandana };
        public static readonly Item DogTags = new Item { Name = "Dog Tags", Id = GcxTableMapping.DogTags };
        public static readonly Item MoDisc = new Item { Name = "M.O. Disc", Id = GcxTableMapping.MoDisc };
        public static readonly Item UspSupp = new Item { Name = "USP Suppressor", Id = GcxTableMapping.UspSupp, ProcId = KnownProc.AwardUspSuppressor };
        public static readonly Item InfWig = new Item { Name = "Infinity Wig", Id = GcxTableMapping.InfWig };
        public static readonly Item BlueWig = new Item { Name = "Blue Wig", Id = GcxTableMapping.BlueWig };
        public static readonly Item OrangeWig = new Item { Name = "Orange Wig", Id = GcxTableMapping.OrangeWig };
    }

    internal static class MGS2Weapons
    {
        public static readonly Item M9 = new Item { Name = "M9", Id = GcxTableMapping.M9, ProcId = KnownProc.AwardM9Gun };
        public static readonly Item M9Ammo = new Item { Name = "M9 Ammo", Id = GcxTableMapping.M9, ProcId = KnownProc.AwardM9Gun };
        public static readonly Item Usp = new Item { Name = "USP", Id = GcxTableMapping.Usp };
        public static readonly Item UspAmmo = new Item { Name = "USP Ammo", Id = GcxTableMapping.Usp, ProcId = KnownProc.AwardUspAmmo };
        public static readonly Item Socom = new Item { Name = "SOCOM", Id = GcxTableMapping.Socom };
        public static readonly Item SocomAmmo = new Item { Name = "SOCOM Ammo", Id = GcxTableMapping.Socom, ProcId = KnownProc.AwardSocomAmmo };
        public static readonly Item Psg1 = new Item { Name = "PSG1", Id = GcxTableMapping.Psg1, ProcId = KnownProc.AwardPsg1Gun };
        public static readonly Item Psg1Ammo = new Item { Name = "PSG1 Ammo", Id = GcxTableMapping.Psg1, ProcId = KnownProc.AwardPsg1Ammo };
        public static readonly Item Rgb6 = new Item { Name = "RGB-6", Id = GcxTableMapping.Rgb6, ProcId = KnownProc.AwardRgbGun };
        public static readonly Item Rgb6Ammo = new Item { Name = "RGB-6 Ammo", Id = GcxTableMapping.Rgb6, ProcId = KnownProc.AwardRgbAmmo };
        public static readonly Item Nikita = new Item { Name = "Nikita", Id = GcxTableMapping.Nikita, ProcId = KnownProc.AwardNikitaGun };
        public static readonly Item NikitaAmmo = new Item { Name = "Nikita Ammo", Id = GcxTableMapping.Nikita, ProcId = KnownProc.AwardNikitaAmmo };
        public static readonly Item Stinger = new Item { Name = "Stinger", Id = GcxTableMapping.Stinger, ProcId = KnownProc.AwardStingerGun };
        public static readonly Item StingerAmmo = new Item { Name = "Stinger Ammo", Id = GcxTableMapping.Stinger, ProcId = KnownProc.AwardStingerAmmo };
        public static readonly Item Claymore = new Item { Name = "Claymore", Id = GcxTableMapping.Claymore };
        public static readonly Item C4 = new Item { Name = "C4", Id = GcxTableMapping.C4 };
        public static readonly Item Chaff = new Item { Name = "Chaff Grenade", Id = GcxTableMapping.Chaff };
        public static readonly Item Stun = new Item { Name = "Stun Grenade", Id = GcxTableMapping.Stun};
        public static readonly Item Dmic1 = new Item { Name = "Directional Microphone", Id = GcxTableMapping.Dmic1 };
        public static readonly Item HfBlade = new Item { Name = "H.F. Blade", Id = GcxTableMapping.HfBlade };
        public static readonly Item Coolant = new Item { Name = "Coolant", Id = GcxTableMapping.Coolant };
        public static readonly Item Aks74u = new Item { Name = "AKS-74u", Id = GcxTableMapping.Aks74u, ProcId = KnownProc.AwardAksGun };
        public static readonly Item Aks74uAmmo = new Item { Name = "AKS-74u Ammo", Id = GcxTableMapping.Aks74u, ProcId = KnownProc.AwardAksAmmo };
        public static readonly Item Magazine = new Item { Name = "Empty Magazine", Id = GcxTableMapping.Magazine };
        public static readonly Item Grenade = new Item { Name = "Grenade", Id = GcxTableMapping.Grenade };
        public static readonly Item M4 = new Item { Name = "M4", Id = GcxTableMapping.M4, ProcId = KnownProc.AwardM4Gun };
        public static readonly Item M4Ammo = new Item { Name = "M4 Ammo", Id = GcxTableMapping.M4, ProcId = KnownProc.AwardM4Ammo };
        public static readonly Item Psg1t = new Item { Name = "PSG1-T", Id = GcxTableMapping.Psg1t, ProcId = KnownProc.AwardPsg1tGun };
        public static readonly Item Psg1tAmmo = new Item { Name = "PSG1-T Ammo", Id = GcxTableMapping.Psg1t, ProcId = KnownProc.AwardPsg1tAmmo };
        public static readonly Item Dmic2 = new Item { Name = " Directional Microphone", Id = GcxTableMapping.Dmic2 };
        public static readonly Item Book = new Item { Name = "Book", Id= GcxTableMapping.Book };
    }

    internal class ItemSet
    {
        public string Name { get; set; }
        public Dictionary<Location, Item> Entities = new Dictionary<Location, Item>();
        public List<Item> ItemsNeededToProgress = new List<Item>();
    }

    public static class LogicRequirements
    {
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
            TankerPart1.ItemsNeededToProgress.Add(MGS2Weapons.M9); //USP or M9

            TankerPart2.Name = "After Olga, Before Deck 2";
            TankerPart2.Entities = new Dictionary<Location, Item>();
            TankerPart2.ItemsNeededToProgress.AddRange(TankerPart1.ItemsNeededToProgress);
            TankerPart2.ItemsNeededToProgress.Add(MGS2Weapons.Usp);

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
            PlantSet4.ItemsNeededToProgress.AddRange(new Item[] {MGS2Items.Card, MGS2Items.BDU, MGS2Weapons.Aks74u }); //level 2 card;

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

        static VanillaItems() 
        {
            #region Tanker
            TankerPart1.Name = "Before Olga";
            TankerPart1.Entities = new Dictionary<Location, Item>();
            TankerPart1.ItemsNeededToProgress.Add(MGS2Weapons.M9);
            #region w00a
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0xB7BC, PosZ = 0, PosY = 0xBBA4, Rot = 1}, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x6F, 0x0E, 0xA8 }, PosX = 0x1388, PosZ = 0, PosY = 0x493E, Rot = 1 }, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x4E, 0x9E, 0x26 }, PosX = 0x3E80, PosZ = 0xBB8, PosY = 0xBD98, Rot = 1 }, MGS2Items.Bandage);
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x4F, 0x9E, 0x26 }, PosX = 0xE1BA, PosZ = 0, PosY = 0x1194, Rot = 1 }, MGS2Items.Bandage);
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0xDD, 0xC2, 0xAD }, PosX = 0x4844, PosZ = 0, PosY = 0xBBA4, Rot = 1 }, MGS2Items.Pentazemin);
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x8B, 0x42, 0xA9 }, PosX = 0x2710, PosZ = 0x7D0, PosY = 0xFE0C, Rot = 1 }, MGS2Weapons.Chaff);
            TankerPart1.Entities.Add(new Location { GcxFile = "w00a", SpawnId = new byte[] { 0x57, 0x6C}, PosX = 0x1770, PosZ = 0x157C, PosY = 0x0, Rot = 3 }, MGS2Items.ColdMeds); //not guaranteed spawn
            #endregion            
            #region w01a
            TankerPart1.Entities.Add(new Location { GcxFile = "w01a", SpawnId = new byte[] { 0x79, 0x7E, 0x24 }, PosX = 0x1194, PosZ = 0, PosY = 0xDDE, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01a", SpawnId = new byte[] { 0x57, 0x69, 0xFE }, PosX = 0xEE58, PosZ = 0, PosY = 0xDDE, Rot = 0 }, MGS2Items.Ration);
            #endregion
            #region w01b
            TankerPart1.Entities.Add(new Location { GcxFile = "w01b", SpawnId = new byte[] { 0x40, 0x14, 0x9B }, PosX = 0x2AF8, PosZ = 0xBB8, PosY = 0xC662, Rot = 0}, MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01b", SpawnId = new byte[] { 0xFE, 0x69, 0x57, 0x1 }, PosX = 0xDAC, PosZ = 0xBB8, PosY = 0x8CA, Rot = 0}, MGS2Items.Ration);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01b", SpawnId = new byte[] { 0x9C, 0x7F, 0xD1, 0x2 }, PosX = 0x3E8, PosZ = 0xBB8, PosY = 0x157C, Rot = 0}, MGS2Weapons.UspAmmo);
            #endregion
            #region w01c
            TankerPart1.Entities.Add(new Location { GcxFile = "w01c", SpawnId = new byte[] { 0xFE, 0x69, 0x57 }, PosX = 0x251C, PosZ = 0x1770, PosY = 0xC086, Rot = 0 }, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w01c", SpawnId = new byte[] { 0xCB, 0x22, 0x66 }, PosX = 0xFA24, PosZ = 0x1838, PosY = 0xB910, Rot = 0 }, MGS2Weapons.Chaff);
            #endregion
            #region w01d
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0x49, 0x35, 0xB8 }, PosX = 0xED72, PosZ = 0x2292, PosY = 0xCD38, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0xD8F0, PosZ = 0x2328, PosY = 0xB7BC, Rot = 0 }, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0xF3, 0xB9, 0x75 }, PosX = 0xDBDE, PosZ = 0x2292, PosY = 0xCD38, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0xF4, 0xB9, 0x75 }, PosX = 0xF63C, PosZ = 0x2328, PosY = 0xD19D, Rot = 0 }, MGS2Weapons.UspAmmo);
            //these two are part of a separate spawning function, maybe taking place after/before olga fight?(not that, at least)
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0x4A, 0x35, 0xB8 }, PosX = 0x1E14, PosZ = 0x2328, PosY = 0xD1B6, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01d", SpawnId = new byte[] { 0xF4, 0xF6, 0xAE }, PosX = 0x2710, PosZ = 0x2328, PosY = 0xD8F0, Rot = 0 }, MGS2Items.Box1);
            #endregion
            #region w01e
            TankerPart1.Entities.Add(new Location { GcxFile = "w01e", SpawnId = new byte[] { 0xFE, 0x69, 0x57 }, PosX = 0xEE6C, PosZ = 0x2EE0, PosY = 0xC27A, Rot = 0 }, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w01e", SpawnId = new byte[] { 0x9C, 0x7F, 0xD1 }, PosX = 0x12D1, PosZ = 0x2EE0, PosY = 0xB706, Rot = 0 }, MGS2Weapons.UspAmmo);
            #endregion
            #region w01f
            //possible thermal spawn?
            TankerPart1.Entities.Add(new Location { GcxFile = "w01f", SpawnId = new byte[] { 0x40, 0x14, 0x9B }, PosX = 0x2328, PosZ = 0, PosY = 0xB7BC, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart1.Entities.Add(new Location { GcxFile = "w01f", SpawnId = new byte[] { 0xFE, 0x69, 0x57 }, PosX = 0xDE68, PosZ = 0, PosY = 0xB8B6, Rot = 0 }, MGS2Items.Ration); //not guaranteed spawn
            TankerPart1.Entities.Add(new Location { GcxFile = "w01f", SpawnId = new byte[] { 0x9C, 0x7F, 0xD1 }, PosX = 0xCF2C, PosZ = 0xEC78, PosY = 0xCF2C, Rot = 0 }, MGS2Weapons.UspAmmo);
            #endregion

            TankerPart2.Name = "After Olga, Before Deck 2";
            TankerPart2.Entities = new Dictionary<Location, Item>();
            TankerPart1.Entities.ToList().ForEach(entity => TankerPart2.Entities.Add(entity.Key, entity.Value));
            TankerPart2.ItemsNeededToProgress.AddRange(TankerPart1.ItemsNeededToProgress);
            TankerPart2.ItemsNeededToProgress.Add(MGS2Weapons.Usp);
            #region w00c
            TankerPart2.Entities.Add(new Location { GcxFile = "w00c", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0xD314, PosZ = 0x2EE0, PosY = 0xB6C2, Rot = 1}, MGS2Items.Ration);
            TankerPart2.Entities.Add(new Location { GcxFile = "w00c", SpawnId = new byte[] { 0x6A, 0xC1, 0x11 }, PosX = 0x4650, PosZ = 0x31CE, PosY = 0xC568, Rot = 3 }, MGS2Items.WetBox);
            TankerPart2.Entities.Add(new Location { GcxFile = "w00c", SpawnId = new byte[] { 0x3D, 0xC7, 0x33 }, PosX = 0x2EE, PosZ = 0x6B6C, PosY = 0xCD38, Rot = 1 }, MGS2Items.Thermals);
            TankerPart2.Entities.Add(new Location { GcxFile = "w00c", SpawnId = new byte[] { 0x92, 0x8A, 0x3 }, PosX = 0xFD12, PosZ = 0x5DC0, PosY = 0xCC3E, Rot = 0 }, MGS2Items.UspSupp); //not guaranteed spawn
            #endregion
            #region w01f
            TankerPart2.Entities.Add(new Location { GcxFile = "w01f", SpawnId = new byte[] { 0xE8, 0x92, 0xE5 }, PosX = 0x30D4, PosZ = 0xEC78, PosY = 0xCF2C, Rot = 0 }, MGS2Weapons.Stun);
            #endregion
            #region w02a
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0xA2, 0xF9, 0xF9 }, PosX = 0x1B58, PosZ = 0xCD38, PosY = 0xB3D4, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0x8CA, PosZ = 0xC950, PosY = 0xB9B0, Rot = 0 }, MGS2Items.Ration);
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0x3A, 0xFB, 0x9E }, PosX = 0xEA84, PosZ = 0xCD38, PosY = 0xAC04, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0xF4, 0x6A, 0xA7 }, PosX = 0xD120, PosZ = 0xCD38, PosY = 0x3E8, Rot = 0 }, MGS2Weapons.Grenade); //not guaranteed spawn
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0x3B, 0xFB, 0x9E }, PosX = 0xE69C, PosZ = 0xDCD8, PosY = 0xD8F0, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0x3C, 0xFB, 0x9E }, PosX = 0xC180, PosZ = 0xF060, PosY = 0xA916, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart2.Entities.Add(new Location { GcxFile = "w02a", SpawnId = new byte[] { 0x3D, 0xFB, 0x9E }, PosX = 0x3F60, PosZ = 0xF060, PosY = 0xA7A6, Rot = 0 }, MGS2Weapons.UspAmmo);
            #endregion

            TankerPart3.Name = "Deck 2 & Beyond";
            TankerPart3.Entities = new Dictionary<Location, Item>();
            TankerPart2.Entities.ToList().ForEach(entity => TankerPart3.Entities.Add(entity.Key, entity.Value));
            TankerPart3.ItemsNeededToProgress.AddRange(TankerPart2.ItemsNeededToProgress);
            TankerPart3.ItemsNeededToProgress.Add(MGS2Items.Camera1);
            #region w03a
            TankerPart3.Entities.Add(new Location { GcxFile = "w03a", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0xD026, PosZ = 0xEC78, PosY = 0xFFFF5BF0, Rot = 0 }, MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location { GcxFile = "w03a", SpawnId = new byte[] { 0xF3, 0xB9, 0x75 }, PosX = 0xC950, PosZ = 0xEC78, PosY = 0xFFFEF46C, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location { GcxFile = "w03a", SpawnId = new byte[] { 0xF4, 0xB9, 0x75 }, PosX = 0xF060, PosZ = 0xEC78, PosY = 0xFFFE0C00, Rot = 0 }, MGS2Weapons.UspAmmo);
            #endregion
            #region w03b
            TankerPart3.Entities.Add(new Location { GcxFile = "w03b", SpawnId = new byte[] { 0xF3, 0xB9, 0x75 }, PosX = 0x3D86, PosZ = 0xEC78, PosY = 0xFFFE0A0C, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location { GcxFile = "w03b", SpawnId = new byte[] { 0xF4, 0xB9, 0x75 }, PosX = 0x31CE, PosZ = 0xEC78, PosY = 0xFFFE7866, Rot = 0 }, MGS2Weapons.UspAmmo);
            TankerPart3.Entities.Add(new Location { GcxFile = "w03b", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0x32C8, PosZ = 0xEC78, PosY = 0xFFFE841E, Rot = 0 }, MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location { GcxFile = "w03b", SpawnId = new byte[] { 0x49, 0x35, 0xB8 }, PosX = 0x300C, PosZ = 0xEC78, PosY = 0xFFFE0912, Rot = 0 }, MGS2Weapons.M9Ammo);
            #endregion
            #region w04a
            TankerPart3.Entities.Add(new Location { GcxFile = "w04a", SpawnId = new byte[] { 0x49, 0x35, 0xB8 }, PosX = 0xD508, PosZ = 0xDCD8, PosY = 0xFE0C, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart3.Entities.Add(new Location { GcxFile = "w04a", SpawnId = new byte[] { 0x4A, 0x35, 0xB8 }, PosX = 0x1B58, PosZ = 0xEC78, PosY = 0x6D6, Rot = 0 }, MGS2Weapons.M9Ammo);
            TankerPart3.Entities.Add(new Location { GcxFile = "w04a", SpawnId = new byte[] { 0x6E, 0x0E, 0xA8 }, PosX = 0x2710, PosZ = 0xEC78, PosY = 0xCB2, Rot = 0 }, MGS2Items.Ration);
            TankerPart3.Entities.Add(new Location { GcxFile = "w04a", SpawnId = new byte[] { 0x3D, 0xC7, 0x33 }, PosX = 0xD6FC, PosZ = 0xEC78, PosY = 0xF254, Rot = 0 }, MGS2Items.Thermals);
            #endregion
            #endregion

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
}
