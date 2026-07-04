using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;

namespace MGS2_CheatTrainer_V2
{
    #region Internals
    public class BossVitals
    {
        public List<int> NestedHealthPointers;
        public List<int> NestedStaminaPointers;
        public int HealthOffset;
        public int Health;
        public bool HasStamina;
        public int StaminaOffset;
        public int Stamina;
        public Constants.Boss Boss;

        public static BossVitals ParseBossVitals(Constants.Boss boss)
        {
            switch(boss)
            {
                case Constants.Boss.Olga:
                    return MGS2UsableObjects.Olga;
                case Constants.Boss.Fortune:
                    return MGS2UsableObjects.Fortune;
                case Constants.Boss.Fatman:
                    return MGS2UsableObjects.Fatman;
                case Constants.Boss.Harrier:
                    return MGS2UsableObjects.Harrier;
                case Constants.Boss.Vamp:
                    return MGS2UsableObjects.Vamp;
                case Constants.Boss.VampSnipe:
                    return MGS2UsableObjects.VampSniping;
                case Constants.Boss.Solidus:
                    return MGS2UsableObjects.Solidus;
                case Constants.Boss.Ray1:
                    return MGS2UsableObjects.Ray1;
                case Constants.Boss.Ray2:
                    return MGS2UsableObjects.Ray2;
                case Constants.Boss.Ray3:
                    return MGS2UsableObjects.Ray3;
                case Constants.Boss.Ray4:
                    return MGS2UsableObjects.Ray4;
                case Constants.Boss.Ray5:
                    return MGS2UsableObjects.Ray5;
                case Constants.Boss.Ray6:
                    return MGS2UsableObjects.Ray6;
                case Constants.Boss.Ray7:
                    return MGS2UsableObjects.Ray7;
                case Constants.Boss.Ray8:
                    return MGS2UsableObjects.Ray8;
                case Constants.Boss.Ray9:
                    return MGS2UsableObjects.Ray9;
                case Constants.Boss.Ray10:
                    return MGS2UsableObjects.Ray10;
                case Constants.Boss.Ray11:
                    return MGS2UsableObjects.Ray11;
                case Constants.Boss.Ray12:
                    return MGS2UsableObjects.Ray12;
                case Constants.Boss.Ray13:
                    return MGS2UsableObjects.Ray13;
                case Constants.Boss.Ray14:
                    return MGS2UsableObjects.Ray14;
                case Constants.Boss.Ray15:
                    return MGS2UsableObjects.Ray15;
                case Constants.Boss.Ray16:
                    return MGS2UsableObjects.Ray16;
                case Constants.Boss.Ray17:
                    return MGS2UsableObjects.Ray17;
                case Constants.Boss.Ray18:
                    return MGS2UsableObjects.Ray18;
                case Constants.Boss.Ray19:
                    return MGS2UsableObjects.Ray19;
                case Constants.Boss.Ray20:
                    return MGS2UsableObjects.Ray20;
                case Constants.Boss.Ray21:
                    return MGS2UsableObjects.Ray21;
                case Constants.Boss.Ray22:
                    return MGS2UsableObjects.Ray22;
                case Constants.Boss.Ray23:
                    return MGS2UsableObjects.Ray23;
                case Constants.Boss.Ray24:
                    return MGS2UsableObjects.Ray24;
                case Constants.Boss.Ray25:
                    return MGS2UsableObjects.Ray25;
                default:
                    throw new InvalidEnumArgumentException("Boss not recognized.");
            }
        }
    }

    internal class GameObject
    {
        internal string _name = "";
        internal IntPtr _nameOffset; //TODO: make this a MemoryOffset
    }

    interface IOldMGS2Object
    {
        void OldToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel);
    }

    public abstract class OldMgs2Object : IOldMGS2Object
    {
        internal GameObject GameObject { get; set; } //replace with MGS2 string?
        public string Name { get { return GameObject._name; } }
        public IntPtr NameMemoryOffset { get { return GameObject._nameOffset; } } //TODO: make this a MemoryOffset
        public int InventoryOffset { get; set; } //TODO: make this a MemoryOffset

        public OldMgs2Object(string name, IntPtr nameMemoryOffset, int inventoryOffset)
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

        public void OldToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            logger.Verbose($"Toggling {Name}...");
            Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
            statusLabel.Text = $"Finding {Name} in memory...";
            ushort currentObjectValue = BitConverter.ToUInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(InventoryOffset, sizeof(short), currentPC), 0);
            bool isCurrentlyEnabled;
            if (this is OldBasicItem)
                isCurrentlyEnabled = currentObjectValue == 0 ? false : true;
            else        
                isCurrentlyEnabled = currentObjectValue == ushort.MaxValue ? false : true;
            //Toggle the object if it is currently disabled and needs enabling, or if it is currently enabled and needs disabling.
            if (isCurrentlyEnabled != shouldBeEnabled)
            {
                MGS2MemoryManager.ToggleObject(this, currentPC, shouldBeEnabled);
            }
            statusLabel.Text = $"Toggled {Name}!";
            logger.Verbose($"Toggle was successful");
        }
    }
    #endregion

    #region Item Classes
    public class OldBasicItem : OldMgs2Object
    {
        #region Internals & Constructor
        public OldBasicItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void ToggleItem(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                OldToggleObject(shouldBeEnabled, logger, statusLabel);
            }
            catch(Exception e)
            {
                logger.Error($"Failed to toggle {Name}: {e}");
                MessageBox.Show($"Failed to toggle {Name}: {e}");
            }
        }
    }

    public class OldLevelableItem : OldBasicItem
    {
        internal int LevelOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        //TODO: remember last known level?

        public OldLevelableItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }

        public void SetLevel(ushort level, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                logger.Verbose($"Setting {Name} to {level}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectBaseValue(this, level, currentPC);
                statusLabel.Text = $"{Name} level updated to {level}";
                logger.Verbose($"Level set");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set card level: {e}");
                MessageBox.Show($"Failed to set card level: {e}");
            }
        }
    }

    public class OldDurabilityItem : OldBasicItem
    {
        #region Internals & Constructor
        internal int DurabilityOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset

        public OldDurabilityItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void SetDurability(ushort value, ILogger logger, TextBlock statusLabel)
        {
            //Boxes have a durability of 21(perfect condition) -> 1(nearly destroyed)
            try
            {
                logger.Verbose($"Setting durability {value} for {Name}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectBaseValue(this, value, currentPC);
                statusLabel.Text = $"{Name} durability updated to {value}";
                logger.Verbose($"Durability set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to modify durability for {Name}: {e}");
                MessageBox.Show($"Failed to modify durability for {Name}: {e}");
            }
        }

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
            short currentDurability = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(DurabilityOffset, sizeof(short), currentPC), 0);
            
            if (currentDurability == 0 && shouldBeEnabled)
            {
                //if the box is destroyed/disabled and should be enabled, set to "max" durability
                SetDurability(21, logger, statusLabel);
            }
            else if(currentDurability != 0 && !shouldBeEnabled)
            {
                //if the box is in-tact/enabled and should be disabled, set to 0 durability
                SetDurability(0, logger, statusLabel);
            }
        }
    }

    public class OldStackableItem : OldBasicItem
    {
        #region Internals & Constructor
        internal int CurrentCountOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        internal int MaxCountOffset { get; set; } //TODO: make this a MemoryOffset

        const int MIN_MAX_COUNT_DIFF = 96;
        private ushort LastKnownCurrentCount = 1;

        public OldStackableItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxCountOffset = inventoryOffset + MIN_MAX_COUNT_DIFF;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
            ushort currentCount = BitConverter.ToUInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(CurrentCountOffset, sizeof(short), currentPC), 0);
            if (currentCount == 0 && shouldBeEnabled)
            {
                if (LastKnownCurrentCount != 0)
                    UpdateCurrentCount(LastKnownCurrentCount, logger, statusLabel);
                else
                    UpdateCurrentCount(1, logger, statusLabel);
            }
            else if(!shouldBeEnabled)
            {
                LastKnownCurrentCount = currentCount;
                UpdateCurrentCount(0, logger, statusLabel); 
            }
        }

        public void UpdateCurrentCount(ushort count, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                logger.Verbose($"Setting current count to {count} for {Name}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectBaseValue(this, count, currentPC);
                statusLabel.Text = $"Current count for {Name} updated to {count}";
                logger.Verbose($"Current count set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update current count of {Name}: {e}");
                MessageBox.Show($"Failed to update current count of {Name}: {e}");
            }
        }

        public void UpdateMaxCount(ushort count, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                logger.Verbose($"Setting max count to {count} for {Name}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectMaxValue(this, count, currentPC);
                statusLabel.Text = $"Max count for {Name} updated to {count}";
                logger.Verbose($"Max count set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update max count of {Name}: {e}");
                MessageBox.Show($"Failed to update max count of {Name}: {e}");
            }
        }
    }
    #endregion

    #region Weapon Classes
    public class OldBasicWeapon : OldMgs2Object
    {
        #region Internals & Constructor
        public OldBasicWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void ToggleWeapon(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                OldToggleObject(shouldBeEnabled, logger, statusLabel);
            }
            catch(Exception e)
            {
                logger.Error($"Failed to toggle {Name}: {e}");
                MessageBox.Show($"Failed to toggle {Name}: {e}");
            }
        }
    }

    public class OldAmmoWeapon : OldBasicWeapon
    {
        #region Internals & Constructor
        public int CurrentAmmoOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        public int MaxAmmoOffset { get; set; } //TODO: make this a MemoryOffset

        const int MIN_MAX_COUNT_DIFF = 72;
        private short LastKnownCurrentAmmo = 1;
        public OldAmmoWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxAmmoOffset = inventoryOffset + MIN_MAX_COUNT_DIFF;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
            short currentAmmo = BitConverter.ToInt16(MGS2MemoryManager.GetPlayerInfoBasedValue(CurrentAmmoOffset, sizeof(short), currentPC), 0);
            //TODO: it would be cool to duplicate the "NO USE" functionality the Stinger gets when prone when disabled!
            //can't seem to easily find the bytes that control that though :(
            if (currentAmmo <= 0 && shouldBeEnabled)
            {
                if (LastKnownCurrentAmmo != 0)
                    UpdateCurrentAmmoCount(LastKnownCurrentAmmo, logger, statusLabel);
                else
                    UpdateCurrentAmmoCount(1, logger, statusLabel);
            }
            else if(!shouldBeEnabled)
            {
                LastKnownCurrentAmmo = currentAmmo;
                UpdateCurrentAmmoCount(-1, logger, statusLabel);
            }
        }

        public void UpdateCurrentAmmoCount(int count, ILogger logger, TextBlock statusLabel)
        {
            ushort shortCount = (ushort)count;
            try
            {
                logger?.Verbose($"Setting current ammo to {count} for {Name}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectBaseValue(this, shortCount, currentPC);
                statusLabel.Text = $"Current ammo count for {Name} updated to {count}";
                logger.Verbose($"Current ammo set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update current ammo count for {Name}: {e}");
                MessageBox.Show($"Failed to update current ammo count for {Name}: {e}");
            }
        }

        public void UpdateMaxAmmoCount(int count, ILogger logger, TextBlock statusLabel)
        {
            ushort shortCount = (ushort)count;
            try
            {
                logger.Verbose($"Setting max ammo to {count} for {Name}...");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                MGS2MemoryManager.UpdateObjectMaxValue(this, shortCount, currentPC);
                statusLabel.Text = $"Max ammo count for {Name} updated to {count}";
                logger.Verbose($"Max ammo set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update max ammo count for {Name}: {e}");
                MessageBox.Show($"Failed to update max ammo count for {Name}: {e}");
            }
        }
    }

    public class OldSpecialWeapon : OldBasicWeapon
    {
        #region Internals & Constructor
        public int SpecialOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        ushort count = 0;
        public OldSpecialWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void SetToLethal(ILogger logger)
        {
            try
            {
                logger.Verbose($"Setting HF blade to lethal");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                MGS2MemoryManager.UpdateObjectBaseValue(this, count += 1, currentPC); //TODO: determine real values
                logger.Verbose($"HF blade set to lethal successfully!");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set HF blade to lethal: {e}");
                MessageBox.Show($"Failed to set HF blade to lethal: {e}");
            }
        }

        public void SetToStun(ILogger logger)
        {
            try
            {
                logger.Verbose($"Setting HF blade to stun");
                Constants.PlayableCharacter currentPC = MGS2MemoryManager.CheckIfUsable(this);
                MGS2MemoryManager.UpdateObjectBaseValue(this, count -= 1, currentPC); //TODO: determine real values
                logger.Verbose($"HF blade set to lethal successfully!");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set HF blade to stun: {e}");
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
        public static readonly OldBasicWeapon DMic1 = new OldBasicWeapon("Directional Microphone", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.D_MIC);
        public static readonly OldBasicWeapon DMic2 = new OldBasicWeapon("Directional Microphone", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.D_MIC_ZOOMED);
        public static readonly OldBasicWeapon Coolant = new OldBasicWeapon("Coolant Spray", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.COOLANT);
        #endregion
        #region Ammo Weapons
        public static readonly OldAmmoWeapon M9 = new OldAmmoWeapon("M9", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.M9);
        public static readonly OldAmmoWeapon USP = new OldAmmoWeapon("USP", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.USP);
        public static readonly OldAmmoWeapon SOCOM = new OldAmmoWeapon("SOCOM", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.SOCOM);
        public static readonly OldAmmoWeapon PSG1 = new OldAmmoWeapon("PSG1", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.PSG1);
        public static readonly OldAmmoWeapon RGB6 = new OldAmmoWeapon("RGB6", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.RGB6);
        public static readonly OldAmmoWeapon Nikita = new OldAmmoWeapon("Nikita", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.NIKITA);
        public static readonly OldAmmoWeapon Stinger = new OldAmmoWeapon("Stinger", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.STINGER);
        public static readonly OldAmmoWeapon Claymore = new OldAmmoWeapon("Claymore", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.CLAYMORE);
        public static readonly OldAmmoWeapon C4 = new OldAmmoWeapon("C4", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.C4);
        public static readonly OldAmmoWeapon ChaffGrenade = new OldAmmoWeapon("Chaff Grenade", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.CHAFF_GRENADE);
        public static readonly OldAmmoWeapon StunGrenade = new OldAmmoWeapon("Stun Grenade", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.STUN_GRENADE);
        public static readonly OldAmmoWeapon AKS74u = new OldAmmoWeapon("AKS74u", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.AKS74U);
        public static readonly OldAmmoWeapon Magazine = new OldAmmoWeapon("Magazine", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.MAGAZINE);
        public static readonly OldAmmoWeapon Grenade = new OldAmmoWeapon("Grenade", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.GRENADE);
        public static readonly OldAmmoWeapon M4 = new OldAmmoWeapon("M4", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.M4);
        public static readonly OldAmmoWeapon PSG1T = new OldAmmoWeapon("PGS1-T", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.PSG1T);
        public static readonly OldAmmoWeapon Book = new OldAmmoWeapon("Book", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.BOOK);
        #endregion
        #region Special Weapons
        public static readonly OldSpecialWeapon HighFrequencyBlade = new OldSpecialWeapon("HF Blade", IntPtr.Zero, Mgs2Offset.BASE_WEAPON.Start + Constants.HIGH_FREQUENCY_BLADE);
        #endregion
        #endregion

        #region Items
        #region Basic Items
        public static readonly OldBasicItem SnakeScope = new OldBasicItem("Binoculars", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BROKEN_SCOPE);
        public static readonly OldBasicItem BodyArmor = new OldBasicItem("Body Armor", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BODY_ARMOR);
        public static readonly OldBasicItem Stealth = new OldBasicItem("Stealth", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.STEALTH);
        public static readonly OldBasicItem MineDetector = new OldBasicItem("Mine Detector", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.MINE_DETECTOR);
        public static readonly OldBasicItem SensorA = new OldBasicItem("Sensor A", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.SENSOR_A);
        public static readonly OldBasicItem SensorB = new OldBasicItem("Sensor B", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.SENSOR_B);
        public static readonly OldBasicItem NightVisionGoggles = new OldBasicItem("NVG", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.NVG);
        public static readonly OldBasicItem ThermalGoggles = new OldBasicItem("ThermalG", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.THERMAL_GOGGLES);
        public static readonly OldBasicItem RaidenScope = new OldBasicItem("Binoculars", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start   + Constants.SCOPE);
        public static readonly OldBasicItem DigitalCamera = new OldBasicItem("Digital Camera", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.DIGITAL_CAMERA);
        public static readonly OldBasicItem Cigarettes = new OldBasicItem("Cigs", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.CIGARETTES);
        public static readonly OldBasicItem Shaver = new OldBasicItem("Shaver", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.SHAVER);
        public static readonly OldBasicItem Phone = new OldBasicItem("Phone", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.PHONE);
        public static readonly OldBasicItem Camera1 = new OldBasicItem("Camera", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.CAMERA);
        public static readonly OldBasicItem APSensor = new OldBasicItem("AP Sensor", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.AP_SENSOR);
        public static readonly OldBasicItem UnknownItem = new OldBasicItem("Unknown Item", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.UNKNOWN_ITEM); //TODO: unused? need to confirm
        public static readonly OldBasicItem SocomSuppressor = new OldBasicItem("SOCOM Suppressor", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.SOCOM_SUPPRESSOR);
        public static readonly OldBasicItem AKSuppressor = new OldBasicItem("AK Suppressor", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.AK_SUPPRESSOR);
        public static readonly OldBasicItem Camera2 = new OldBasicItem("Camera", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BROKEN_CAMERA);
        public static readonly OldBasicItem Bandana = new OldBasicItem("Bandana", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BANDANA);
        public static readonly OldBasicItem MODisc = new OldBasicItem("MODisc", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.MO_DISC);
        public static readonly OldBasicItem USPSuppressor = new OldBasicItem("USP Suppressor", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.USP_SUPPRESSOR);
        public static readonly OldBasicItem InfinityWig = new OldBasicItem("Infinity Wig", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.INFINITY_WIG);
        public static readonly OldBasicItem BlueWig = new OldBasicItem("Blue Wig", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BLUE_WIG);
        public static readonly OldBasicItem OrangeWig = new OldBasicItem("Orange Wig", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.ORANGE_WIG);
        public static readonly OldBasicItem ColorWig = new OldBasicItem("Color Wig", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.COLOR_WIG_1); //unused
        public static readonly OldBasicItem ColorWig2 = new OldBasicItem("Color Wig 2", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.COLOR_WIG_2); //unused
        public static readonly OldBasicItem ColdMedicine = new OldBasicItem("Cold Medicine", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.COLD_MEDICINE);
        #endregion
        #region Durability Items
        public static readonly OldDurabilityItem Box1 = new OldDurabilityItem("Box1", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BOX_1);
        public static readonly OldDurabilityItem Box2 = new OldDurabilityItem("Box2", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BOX_2);
        public static readonly OldDurabilityItem Box3 = new OldDurabilityItem("Box3", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BOX_3);
        public static readonly OldDurabilityItem WetBox = new OldDurabilityItem("WetBox", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.WET_BOX);
        public static readonly OldDurabilityItem Box4 = new OldDurabilityItem("Box4", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BOX_4);
        public static readonly OldDurabilityItem Box5 = new OldDurabilityItem("Box5", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BOX_5);
        #endregion
        #region Enumerable Items
        public static readonly OldStackableItem Ration = new OldStackableItem("Ration", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.RATION);
        public static readonly OldStackableItem Bandage = new OldStackableItem("Bandage", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BANDAGE);
        public static readonly OldStackableItem Pentazemin = new OldStackableItem("Pentazemin", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.PENTAZEMIN);
        public static readonly OldStackableItem DogTags = new OldStackableItem("DogTags", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.DOG_TAGS);
        #endregion
        #region Levelable Items
        public static readonly OldLevelableItem Card = new OldLevelableItem("Card", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.CARD);
        #endregion
        #region Unknown Items
        public static readonly OldBasicItem BDU = new OldDurabilityItem("BDU", IntPtr.Zero, Mgs2Offset.BASE_ITEM.Start + Constants.BDU);
        #endregion
        #endregion

        #region Bosses
        public static readonly BossVitals Olga = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.OlgaNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.OlgaNestedPointers, HealthOffset = 0x46A, StaminaOffset = 0x46C,
            Boss = Constants.Boss.Olga
        };
        public static readonly BossVitals Fortune = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.FortuneNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.FortuneNestedPointers,HealthOffset = Mgs2Offset.FORTUNE_HP_VALUE.Start, 
            StaminaOffset = Mgs2Offset.FORTUNE_STAMINA_VALUE.Start,
            Boss = Constants.Boss.Fortune
        };
        public static readonly BossVitals Fatman = new BossVitals { HasStamina = true, 
            NestedHealthPointers = Mgs2Pointer.FatmanNestedPointers, NestedStaminaPointers = Mgs2Pointer.FatmanNestedPointers,
            HealthOffset = 0x41E,
            StaminaOffset = 0x2330,
            Boss = Constants.Boss.Fatman
        };
        public static readonly BossVitals Harrier = new BossVitals { HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.HarrierNestedPointers, //it works for tracking, but crashes the game when we modify it KEKW
            HealthOffset = 0x78,
            Boss = Constants.Boss.Harrier
        };
        public static readonly BossVitals Vamp = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.VampNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.VampNestedPointers, 
            HealthOffset = 0xFD0,
            StaminaOffset = 0xFD2,
            Boss = Constants.Boss.Vamp
        };
        public static readonly BossVitals VampSniping = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.VampSnipingNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.VampSnipingNestedPointers,
            HealthOffset = 0x22A,
            StaminaOffset = 0x119A,
            Boss = Constants.Boss.VampSnipe
        };
        #region Rays
        public static readonly BossVitals Ray1 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray1NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray1
        };
        public static readonly BossVitals Ray2 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray2NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray2
        };
        public static readonly BossVitals Ray3 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray3NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray3
        };
        public static readonly BossVitals Ray4 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray4NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray4
        };
        public static readonly BossVitals Ray5 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray5NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray5
        };
        public static readonly BossVitals Ray6 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray6NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray6
        };
        public static readonly BossVitals Ray7 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray7NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray7
        };
        public static readonly BossVitals Ray8 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray8NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray8
        };
        public static readonly BossVitals Ray9 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray9NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray9
        };
        public static readonly BossVitals Ray10 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray10NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray10
        };
        public static readonly BossVitals Ray11 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray11NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray11
        };
        public static readonly BossVitals Ray12 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray12NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray12
        };
        public static readonly BossVitals Ray13 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray13NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray13
        };
        public static readonly BossVitals Ray14 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray14NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray14
        };
        public static readonly BossVitals Ray15 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray15NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray15
        };
        public static readonly BossVitals Ray16 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray16NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray16
        };
        public static readonly BossVitals Ray17 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray17NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray17
        };
        public static readonly BossVitals Ray18 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray18NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray18
        };
        public static readonly BossVitals Ray19 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray19NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray19
        };
        public static readonly BossVitals Ray20 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray20NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray20
        };
        public static readonly BossVitals Ray21 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray21NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray21
        };
        public static readonly BossVitals Ray22 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray22NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray22
        };
        public static readonly BossVitals Ray23 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray23NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray23
        };
        public static readonly BossVitals Ray24 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray24NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray24
        };
        public static readonly BossVitals Ray25 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray25NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray25
        };
        #endregion
        public static readonly BossVitals Solidus = new BossVitals
        {
            HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.SolidusNestedPointers, //these crash the game, sadge
            NestedStaminaPointers = Mgs2Pointer.SolidusNestedPointers, //these crash the game, sadge
            HealthOffset = 0x348,
            StaminaOffset = 0x358,
            Boss = Constants.Boss.Solidus
        };
        #endregion
    }
}
