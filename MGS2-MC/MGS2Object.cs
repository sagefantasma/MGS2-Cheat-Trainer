using System;
using System.Windows.Forms;

namespace MGS2_MC
{
    #region Internals

    internal class GameObject
    {
        internal string _name = "";
        internal IntPtr _nameOffset; //TODO: make this a MemoryOffset
    }

    interface IMGS2Object
    {
        void ToggleObject(bool shouldBeEnabled);
    }

    public abstract class MGS2Object : IMGS2Object
    {
        internal GameObject GameObject { get; set; } //replace with MGS2 string?
        public string Name { get { return GameObject._name; } }
        public IntPtr NameMemoryOffset { get { return GameObject._nameOffset; } } //TODO: make this a MemoryOffset
        public int InventoryOffset { get; set; } //TODO: make this a MemoryOffset

        public MGS2Object(string name, IntPtr nameMemoryOffset, int inventoryOffset)
        {
            GameObject = new GameObject { _name = name, _nameOffset = nameMemoryOffset };
            InventoryOffset = inventoryOffset;
        }

        public void ChangeName(string name)
        {
            //TODO: this should be leveraged in the string modifiers
            GameObject newGameObject = new GameObject { _name = name, _nameOffset = NameMemoryOffset };
            GameObject = newGameObject;
        }

        public void ToggleObject(bool shouldBeEnabled)
        {
            short currentObjectValue = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(InventoryOffset, sizeof(short)), 0);
            bool isCurrentlyEnabled = currentObjectValue != 0 ? true : false; //this feels more readable as a ternary than the shorthand
            //Toggle the object if it is currently disabled and needs enabling, or if it is currently enabled and needs disabling.
            if (isCurrentlyEnabled != shouldBeEnabled)
            {
                MGS2MemoryManager.ToggleObject(this);
            }
        }
    }
    #endregion

    #region Item Classes
    public class BasicItem : MGS2Object
    {
        #region Internals & Constructor
        public BasicItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void ToggleItem(bool shouldBeEnabled)
        {
            try
            {
                ToggleObject(shouldBeEnabled);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to toggle {Name}: {e}");
            }
        }
    }

    public class LevelableItem : BasicItem
    {
        internal int LevelOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        //TODO: remember last known level?

        public LevelableItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }

        public void SetLevel(short level)
        {
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, level);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to set card level: {e}");
            }
        }
    }

    public class DurabilityItem : BasicItem
    {
        #region Internals & Constructor
        internal int DurabilityOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset

        public DurabilityItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void SetDurability(short value)
        {
            //Boxes have a durability of 21(perfect condition) -> 1(nearly destroyed)
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, value);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to modify durability for {Name}: {e}");
            }
        }

        internal new void ToggleObject(bool shouldBeEnabled)
        {
            short currentDurability = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(DurabilityOffset, sizeof(short)), 0);
            
            if (currentDurability == 0 && shouldBeEnabled)
            {
                //if the box is destroyed/disabled and should be enabled, set to "max" durability
                SetDurability(21);
            }
            else if(currentDurability != 0 && !shouldBeEnabled)
            {
                //if the box is in-tact/enabled and should be disabled, set to 0 durability
                SetDurability(0);
            }
        }
    }

    public class StackableItem : BasicItem
    {
        #region Internals & Constructor
        internal int CurrentCountOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        internal int MaxCountOffset { get; set; } //TODO: make this a MemoryOffset

        const int MIN_MAX_COUNT_DIFF = 96;
        private short LastKnownCurrentCount = 1;

        public StackableItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxCountOffset = inventoryOffset + MIN_MAX_COUNT_DIFF;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled)
        {
            short currentCount = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(CurrentCountOffset, sizeof(short)), 0);
            if (currentCount == 0 && shouldBeEnabled)
            {
                if (LastKnownCurrentCount != 0)
                    UpdateCurrentCount(LastKnownCurrentCount);
                else
                    UpdateCurrentCount(1);
            }
            else if(!shouldBeEnabled)
            {
                LastKnownCurrentCount = currentCount;
                UpdateCurrentCount(0);
            }
        }

        public void UpdateCurrentCount(short count)
        {
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, count);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to update current count of {Name}: {e}");
            }
        }

        public void UpdateMaxCount(short count)
        {
            try
            {
                MGS2MemoryManager.UpdateObjectMaxValue(this, count);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to update max count of {Name}: {e}");
            }
        }
    }
    #endregion

    #region Weapon Classes
    public class BasicWeapon : MGS2Object
    {
        #region Internals & Constructor
        public BasicWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void ToggleWeapon(bool shouldBeEnabled)
        {
            try
            {
                ToggleObject(shouldBeEnabled);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to toggle {Name}: {e}");
            }
        }
    }

    public class AmmoWeapon : BasicWeapon
    {
        #region Internals & Constructor
        public int CurrentAmmoOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        public int MaxAmmoOffset { get; set; } //TODO: make this a MemoryOffset

        const int MIN_MAX_COUNT_DIFF = 72;
        private short LastKnownCurrentAmmo = 1;
        public AmmoWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxAmmoOffset = inventoryOffset + MIN_MAX_COUNT_DIFF;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled)
        {
            short currentAmmo = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(CurrentAmmoOffset, sizeof(short)), 0);
            //TODO: it would be cool to duplicate the "NO USE" functionality the Stinger gets when prone when disabled!
            //can't seem to easily find the bytes that control that though :(
            //TODO: for some reason this just... doesn't work as intended for the first set of toggling. O_o
            if (currentAmmo <= 0 && shouldBeEnabled)
            {
                if (LastKnownCurrentAmmo != 0)
                    UpdateCurrentAmmoCount(LastKnownCurrentAmmo);
                else
                    UpdateCurrentAmmoCount(1);
            }
            else if(!shouldBeEnabled)
            {
                LastKnownCurrentAmmo = currentAmmo;
                UpdateCurrentAmmoCount(-1);
            }
        }

        public void UpdateCurrentAmmoCount(int count)
        {
            short shortCount = (short)count;
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, shortCount);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to update current ammo count for {Name}: {e}");
            }
        }

        public void UpdateMaxAmmoCount(int count)
        {
            short shortCount = (short)count;
            try
            {
                MGS2MemoryManager.UpdateObjectMaxValue(this, shortCount);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to update max ammo count for {Name}: {e}");
            }
        }
    }

    public class SpecialWeapon : BasicWeapon
    {
        #region Internals & Constructor
        public int SpecialOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        short count = 0;
        public SpecialWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void SetToLethal()
        {
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, count += 1); //TODO: determine real values
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to set HF blade to lethal: {e}");
            }
        }

        public void SetToStun()
        {
            try
            {
                MGS2MemoryManager.UpdateObjectBaseValue(this, count -= 1); //TODO: determine real values
            }
            catch(Exception e)
            {
                MessageBox.Show($"Failed to set HF blade to stun: {e}");
            }
        }
    }
    #endregion

    public class MGS2UsableObjects
    {
        //TODO: update name pointers to, you know, real values :)
        #region Weapons
        #region Basic Weapons
        public static readonly BasicWeapon DMic1 = new BasicWeapon("Directional Microphone", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.D_MIC);
        public static readonly BasicWeapon DMic2 = new BasicWeapon("Directional Microphone", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.D_MIC_ZOOMED);
        public static readonly BasicWeapon Coolant = new BasicWeapon("Coolant Spray", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.COOLANT);
        #endregion
        #region Ammo Weapons
        public static readonly AmmoWeapon M9 = new AmmoWeapon("M9", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.M9);
        public static readonly AmmoWeapon USP = new AmmoWeapon("USP", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.USP);
        public static readonly AmmoWeapon SOCOM = new AmmoWeapon("SOCOM", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.SOCOM);
        public static readonly AmmoWeapon PSG1 = new AmmoWeapon("PSG1", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.PSG1);
        public static readonly AmmoWeapon RGB6 = new AmmoWeapon("RGB6", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.RGB6);
        public static readonly AmmoWeapon Nikita = new AmmoWeapon("Nikita", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.NIKITA);
        public static readonly AmmoWeapon Stinger = new AmmoWeapon("Stinger", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.STINGER);
        public static readonly AmmoWeapon Claymore = new AmmoWeapon("Claymore", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.CLAYMORE);
        public static readonly AmmoWeapon C4 = new AmmoWeapon("C4", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.C4);
        public static readonly AmmoWeapon ChaffGrenade = new AmmoWeapon("Chaff Grenade", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.CHAFF_GRENADE);
        public static readonly AmmoWeapon StunGrenade = new AmmoWeapon("Stun Grenade", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.STUN_GRENADE);
        public static readonly AmmoWeapon AKS74u = new AmmoWeapon("AKS74u", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.AKS74U);
        public static readonly AmmoWeapon Magazine = new AmmoWeapon("Magazine", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.MAGAZINE);
        public static readonly AmmoWeapon Grenade = new AmmoWeapon("Grenade", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.GRENADE);
        public static readonly AmmoWeapon M4 = new AmmoWeapon("M4", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.M4);
        public static readonly AmmoWeapon PSG1T = new AmmoWeapon("PGS1-T", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.PSG1T);
        public static readonly AmmoWeapon Book = new AmmoWeapon("Book", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.BOOK);
        #endregion
        #region Special Weapons
        public static readonly SpecialWeapon HighFrequencyBlade = new SpecialWeapon("HF Blade", IntPtr.Zero, MGS2Offset.BASE_WEAPON.Start + Constants.HIGH_FREQUENCY_BLADE);
        #endregion
        #endregion

        #region Items
        #region Basic Items
        public static readonly BasicItem SnakeScope = new BasicItem("Binoculars", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BROKEN_SCOPE);
        public static readonly BasicItem BodyArmor = new BasicItem("Body Armor", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BODY_ARMOR);
        public static readonly BasicItem Stealth = new BasicItem("Stealth", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.STEALTH);
        public static readonly BasicItem MineDetector = new BasicItem("Mine Detector", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.MINE_DETECTOR);
        public static readonly BasicItem SensorA = new BasicItem("Sensor A", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.SENSOR_A);
        public static readonly BasicItem SensorB = new BasicItem("Sensor B", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.SENSOR_B);
        public static readonly BasicItem NightVisionGoggles = new BasicItem("NVG", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.NVG);
        public static readonly BasicItem ThermalGoggles = new BasicItem("ThermalG", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.THERMAL_GOGGLES);
        public static readonly BasicItem RaidenScope = new BasicItem("Binoculars", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start   + Constants.SCOPE);
        public static readonly BasicItem DigitalCamera = new BasicItem("Digital Camera", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.DIGITAL_CAMERA);
        public static readonly BasicItem Cigarettes = new BasicItem("Cigs", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.CIGARETTES);
        public static readonly BasicItem Shaver = new BasicItem("Shaver", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.SHAVER);
        public static readonly BasicItem Phone = new BasicItem("Phone", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.PHONE);
        public static readonly BasicItem Camera1 = new BasicItem("Camera", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.CAMERA);
        public static readonly BasicItem APSensor = new BasicItem("AP Sensor", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.AP_SENSOR);
        public static readonly BasicItem UnknownItem = new BasicItem("Unknown Item", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.UNKNOWN_ITEM); //TODO: unused? need to confirm
        public static readonly BasicItem SocomSuppressor = new BasicItem("SOCOM Suppressor", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.SOCOM_SUPPRESSOR);
        public static readonly BasicItem AKSuppressor = new BasicItem("AK Suppressor", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.AK_SUPPRESSOR);
        public static readonly BasicItem Camera2 = new BasicItem("Camera", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BROKEN_CAMERA);
        public static readonly BasicItem Bandana = new BasicItem("Bandana", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BANDANA);
        public static readonly BasicItem MODisc = new BasicItem("MODisc", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.MO_DISC);
        public static readonly BasicItem USPSuppressor = new BasicItem("USP Suppressor", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.USP_SUPPRESSOR);
        public static readonly BasicItem InfinityWig = new BasicItem("Infinity Wig", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.INFINITY_WIG);
        public static readonly BasicItem BlueWig = new BasicItem("Blue Wig", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BLUE_WIG);
        public static readonly BasicItem OrangeWig = new BasicItem("Orange Wig", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.ORANGE_WIG);
        public static readonly BasicItem ColorWig = new BasicItem("Color Wig", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.COLOR_WIG_1); //unused
        public static readonly BasicItem ColorWig2 = new BasicItem("Color Wig 2", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.COLOR_WIG_2); //unused
        public static readonly BasicItem ColdMedicine = new BasicItem("Cold Medicine", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.COLD_MEDICINE);
        #endregion
        #region Durability Items
        public static readonly DurabilityItem Box1 = new DurabilityItem("Box1", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BOX_1);
        public static readonly DurabilityItem Box2 = new DurabilityItem("Box2", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BOX_2);
        public static readonly DurabilityItem Box3 = new DurabilityItem("Box3", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BOX_3);
        public static readonly DurabilityItem WetBox = new DurabilityItem("WetBox", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.WET_BOX);
        public static readonly DurabilityItem Box4 = new DurabilityItem("Box4", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BOX_4);
        public static readonly DurabilityItem Box5 = new DurabilityItem("Box5", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BOX_5);
        #endregion
        #region Enumerable Items
        public static readonly StackableItem Ration = new StackableItem("Ration", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.RATION);
        public static readonly StackableItem Bandage = new StackableItem("Bandage", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BANDAGE);
        public static readonly StackableItem Pentazemin = new StackableItem("Pentazemin", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.PENTAZEMIN);
        public static readonly StackableItem DogTags = new StackableItem("DogTags", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.DOG_TAGS);
        #endregion
        #region Levelable Items
        public static readonly LevelableItem Card = new LevelableItem("Card", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.CARD);
        #endregion
        #region Unknown Items
        public static readonly BasicItem BDU = new DurabilityItem("BDU", IntPtr.Zero, MGS2Offset.BASE_ITEM.Start + Constants.BDU);
        #endregion
        #endregion
    }
}
