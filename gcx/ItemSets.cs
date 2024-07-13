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
    }

    internal class Item
    {
        public string Name;
        public byte Id;
    }

    internal static class MGS2Items
    {
        public static readonly Item Ration = new Item { Name = "Ration", Id = ObjectIds.Ration };
        public static readonly Item Scope1 = new Item { Name = "Scope", Id = ObjectIds.Scope1};
        public static readonly Item ColdMeds = new Item { Name = "Cold Medicine", Id = ObjectIds.ColdMeds };
        public static readonly Item Bandage = new Item { Name = "Bandage", Id = ObjectIds.Bandage };
        public static readonly Item Pentazemin = new Item { Name = "Pentazemin", Id = ObjectIds.Pentazemin };
        public static readonly Item BDU = new Item { Name = "B.D.U.", Id = ObjectIds.BDU };
        public static readonly Item BodyArmor = new Item { Name = "Body Armor", Id = ObjectIds.BodyArmor };
        public static readonly Item Stealth = new Item { Name = "Stealth", Id = ObjectIds.Stealth };
        public static readonly Item MineDetector = new Item { Name = "Mine Detector", Id = ObjectIds.MineDetector };
        public static readonly Item SensorA = new Item { Name = "Sensor A", Id = ObjectIds.SensorA };
        public static readonly Item SensorB = new Item { Name = "Sensor B", Id = ObjectIds.SensorB };
        public static readonly Item NVG = new Item { Name = "Night Vision Goggles", Id = ObjectIds.NVG };
        public static readonly Item Thermals = new Item { Name = "Thermal Goggles", Id = ObjectIds.Thermals };
        public static readonly Item Scope2 = new Item { Name = "Scope", Id = ObjectIds.Scope2 };
        public static readonly Item DigitalCamera = new Item { Name = "Digital Camera", Id = ObjectIds.DigitalCamera };
        public static readonly Item Box1 = new Item { Name = "Box1", Id = ObjectIds.Box1 };
        public static readonly Item Cigs = new Item { Name = "Cigarettes", Id = ObjectIds.Cigs };
        public static readonly Item Card = new Item { Name = "Card", Id = ObjectIds.Card };
        public static readonly Item Shaver = new Item { Name = "Shaver", Id = ObjectIds.Shaver };
        public static readonly Item Phone = new Item { Name = "Phone", Id = ObjectIds.Phone };
        public static readonly Item Camera1 = new Item { Name = "Camera", Id = ObjectIds.Camera1 };
        public static readonly Item Box2 = new Item { Name = "Box 2", Id = ObjectIds.Box2 };
        public static readonly Item Box3 = new Item { Name = "Box 3", Id = ObjectIds.Box3 };
        public static readonly Item WetBox = new Item { Name = "Wet Box", Id = ObjectIds.WetBox };
        public static readonly Item APSensor = new Item { Name = "A.P. Sensor", Id = ObjectIds.APSensor };
        public static readonly Item Box4 = new Item { Name = "Box 4", Id = ObjectIds.Box4 };
        public static readonly Item Box5 = new Item { Name = "Box 5", Id = ObjectIds.Box5 };
        public static readonly Item SocomSupp = new Item { Name = "Socom Suppressor", Id = ObjectIds.SocomSupp };
        public static readonly Item AkSupp = new Item { Name = "AK Suppressor", Id = ObjectIds.AkSupp };
        public static readonly Item Camera2 = new Item { Name = "Camera", Id = ObjectIds.Camera2 };
        public static readonly Item Bandana = new Item { Name = "Bandana", Id = ObjectIds.Bandana };
        public static readonly Item DogTags = new Item { Name = "Dog Tags", Id = ObjectIds.DogTags };
        public static readonly Item MoDisc = new Item { Name = "M.O. Disc", Id = ObjectIds.MoDisc };
        public static readonly Item UspSupp = new Item { Name = "USP Suppressor", Id = ObjectIds.UspSupp };
        public static readonly Item InfWig = new Item { Name = "Infinity Wig", Id = ObjectIds.InfWig };
        public static readonly Item BlueWig = new Item { Name = "Blue Wig", Id = ObjectIds.BlueWig };
        public static readonly Item OrangeWig = new Item { Name = "Orange Wig", Id = ObjectIds.OrangeWig };
    }

    internal static class MGS2Weapons
    {
        public static readonly Item M9 = new Item { Name = "M9", Id = ObjectIds.M9 };
        public static readonly Item Usp = new Item { Name = "USP", Id = ObjectIds.Usp };
        public static readonly Item Socom = new Item { Name = "SOCOM", Id = ObjectIds.Socom };
        public static readonly Item Psg1 = new Item { Name = "PSG1", Id = ObjectIds.Psg1 };
        public static readonly Item Rgb6 = new Item { Name = "RGB-6", Id = ObjectIds.Rgb6 };
        public static readonly Item Nikita = new Item { Name = "Nikita", Id = ObjectIds.Nikita };
        public static readonly Item Stinger = new Item { Name = "Stinger", Id = ObjectIds.Stinger };
        public static readonly Item Claymore = new Item { Name = "Claymore", Id = ObjectIds.Claymore };
        public static readonly Item C4 = new Item { Name = "C4", Id = ObjectIds.C4 };
        public static readonly Item Chaff = new Item { Name = "Chaff Grenade", Id = ObjectIds.Chaff };
        public static readonly Item Stun = new Item { Name = "Stun Grenade", Id = ObjectIds.Stun};
        public static readonly Item Dmic1 = new Item { Name = "Directional Microphone", Id = ObjectIds.Dmic1 };
        public static readonly Item HfBlade = new Item { Name = "H.F. Blade", Id = ObjectIds.HfBlade };
        public static readonly Item Coolant = new Item { Name = "Coolant", Id = ObjectIds.Coolant };
        public static readonly Item Aks74u = new Item { Name = "AKS-74u", Id = ObjectIds.Aks74u };
        public static readonly Item Magazine = new Item { Name = "Empty Magazine", Id = ObjectIds.Magazine };
        public static readonly Item Grenade = new Item { Name = "Grenade", Id = ObjectIds.Grenade };
        public static readonly Item M4 = new Item { Name = "M4", Id = ObjectIds.M4 };
        public static readonly Item Psg1t = new Item { Name = "PSG1-T", Id = ObjectIds.Psg1t };
        public static readonly Item Dmic2 = new Item { Name = " Directional Microphone", Id = ObjectIds.Dmic2 };
        public static readonly Item Book = new Item { Name = "Book", Id= ObjectIds.Book };
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

        static readonly ItemSet PlantSet1 = new ItemSet();
        static readonly ItemSet PlantSet2 = new ItemSet();
        static readonly ItemSet PlantSet3 = new ItemSet();
        static readonly ItemSet PlantSet4 = new ItemSet();
        static readonly ItemSet PlantSet5 = new ItemSet();
        static readonly ItemSet PlantSet6 = new ItemSet();
    }
}
