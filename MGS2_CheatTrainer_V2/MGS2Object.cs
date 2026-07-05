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
                    return Mgs2UsableObjects.Olga;
                case Constants.Boss.Fortune:
                    return Mgs2UsableObjects.Fortune;
                case Constants.Boss.Fatman:
                    return Mgs2UsableObjects.Fatman;
                case Constants.Boss.Harrier:
                    return Mgs2UsableObjects.Harrier;
                case Constants.Boss.Vamp:
                    return Mgs2UsableObjects.Vamp;
                case Constants.Boss.VampSnipe:
                    return Mgs2UsableObjects.VampSniping;
                case Constants.Boss.Solidus:
                    return Mgs2UsableObjects.Solidus;
                case Constants.Boss.Ray1:
                    return Mgs2UsableObjects.Ray1;
                case Constants.Boss.Ray2:
                    return Mgs2UsableObjects.Ray2;
                case Constants.Boss.Ray3:
                    return Mgs2UsableObjects.Ray3;
                case Constants.Boss.Ray4:
                    return Mgs2UsableObjects.Ray4;
                case Constants.Boss.Ray5:
                    return Mgs2UsableObjects.Ray5;
                case Constants.Boss.Ray6:
                    return Mgs2UsableObjects.Ray6;
                case Constants.Boss.Ray7:
                    return Mgs2UsableObjects.Ray7;
                case Constants.Boss.Ray8:
                    return Mgs2UsableObjects.Ray8;
                case Constants.Boss.Ray9:
                    return Mgs2UsableObjects.Ray9;
                case Constants.Boss.Ray10:
                    return Mgs2UsableObjects.Ray10;
                case Constants.Boss.Ray11:
                    return Mgs2UsableObjects.Ray11;
                case Constants.Boss.Ray12:
                    return Mgs2UsableObjects.Ray12;
                case Constants.Boss.Ray13:
                    return Mgs2UsableObjects.Ray13;
                case Constants.Boss.Ray14:
                    return Mgs2UsableObjects.Ray14;
                case Constants.Boss.Ray15:
                    return Mgs2UsableObjects.Ray15;
                case Constants.Boss.Ray16:
                    return Mgs2UsableObjects.Ray16;
                case Constants.Boss.Ray17:
                    return Mgs2UsableObjects.Ray17;
                case Constants.Boss.Ray18:
                    return Mgs2UsableObjects.Ray18;
                case Constants.Boss.Ray19:
                    return Mgs2UsableObjects.Ray19;
                case Constants.Boss.Ray20:
                    return Mgs2UsableObjects.Ray20;
                case Constants.Boss.Ray21:
                    return Mgs2UsableObjects.Ray21;
                case Constants.Boss.Ray22:
                    return Mgs2UsableObjects.Ray22;
                case Constants.Boss.Ray23:
                    return Mgs2UsableObjects.Ray23;
                case Constants.Boss.Ray24:
                    return Mgs2UsableObjects.Ray24;
                case Constants.Boss.Ray25:
                    return Mgs2UsableObjects.Ray25;
                default:
                    throw new InvalidEnumArgumentException("Boss not recognized.");
            }
        }
    }

    internal class GameObject
    {
        internal string Name = "";
        internal IntPtr NameOffset; //TODO: make this a MemoryOffset
    }

    interface IOldMgs2Object
    {
        void OldToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel);
    }

    public abstract class OldMgs2Object : IOldMgs2Object
    {
        internal GameObject GameObject { get; set; } //replace with MGS2 string?
        public string Name { get { return GameObject.Name; } }
        public IntPtr NameMemoryOffset { get { return GameObject.NameOffset; } } //TODO: make this a MemoryOffset
        public int InventoryOffset { get; set; } //TODO: make this a MemoryOffset

        public OldMgs2Object(string name, IntPtr nameMemoryOffset, int inventoryOffset)
        {
            GameObject = new GameObject { Name = name, NameOffset = nameMemoryOffset };
            InventoryOffset = inventoryOffset;
        }

        public void ChangeName(string name)
        {
            //TODO: this should be leveraged in the string modifiers
            GameObject newGameObject = new GameObject { Name = name, NameOffset = NameMemoryOffset };
            GameObject = newGameObject;
        }

        public void OldToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            /*
            logger.Verbose($"Toggling {Name}...");
            Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
            statusLabel.Text = $"Finding {Name} in memory...";
            ushort currentObjectValue = BitConverter.ToUInt16(Mgs2MemoryManager.GetPlayerInfoBasedValue(InventoryOffset, sizeof(short), currentPc), 0);
            bool isCurrentlyEnabled;
            if (this is OldBasicItem)
                isCurrentlyEnabled = currentObjectValue == 0 ? false : true;
            else        
                isCurrentlyEnabled = currentObjectValue == ushort.MaxValue ? false : true;
            //Toggle the object if it is currently disabled and needs enabling, or if it is currently enabled and needs disabling.
            if (isCurrentlyEnabled != shouldBeEnabled)
            {
                //Mgs2MemoryManager.ToggleObject(this, currentPc, shouldBeEnabled);
            }
            statusLabel.Text = $"Toggled {Name}!";
            logger.Verbose($"Toggle was successful");
            */
        }
    }
    #endregion

    #region Item Classes
    /*
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
                //MessageBox.Show($"Failed to toggle {Name}: {e}");
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
                Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, level, currentPc);
                statusLabel.Text = $"{Name} level updated to {level}";
                logger.Verbose($"Level set");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set card level: {e}");
                //MessageBox.Show($"Failed to set card level: {e}");
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
                Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, value, currentPc);
                statusLabel.Text = $"{Name} durability updated to {value}";
                logger.Verbose($"Durability set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to modify durability for {Name}: {e}");
                //MessageBox.Show($"Failed to modify durability for {Name}: {e}");
            }
        }

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
            short currentDurability = BitConverter.ToInt16(Mgs2MemoryManager.GetPlayerInfoBasedValue(DurabilityOffset, sizeof(short), currentPc), 0);
            
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

        const int MinMaxCountDiff = 96;
        private ushort _lastKnownCurrentCount = 1;

        public OldStackableItem(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxCountOffset = inventoryOffset + MinMaxCountDiff;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
            //ushort currentCount = BitConverter.ToUInt16(Mgs2MemoryManager.GetPlayerInfoBasedValue(CurrentCountOffset, sizeof(short), currentPc), 0);
            if (currentCount == 0 && shouldBeEnabled)
            {
                if (_lastKnownCurrentCount != 0)
                    UpdateCurrentCount(_lastKnownCurrentCount, logger, statusLabel);
                else
                    UpdateCurrentCount(1, logger, statusLabel);
            }
            else if(!shouldBeEnabled)
            {
                _lastKnownCurrentCount = currentCount;
                UpdateCurrentCount(0, logger, statusLabel); 
            }
        }

        public void UpdateCurrentCount(ushort count, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                logger.Verbose($"Setting current count to {count} for {Name}...");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, count, currentPc);
                statusLabel.Text = $"Current count for {Name} updated to {count}";
                logger.Verbose($"Current count set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update current count of {Name}: {e}");
                //MessageBox.Show($"Failed to update current count of {Name}: {e}");
            }
        }

        public void UpdateMaxCount(ushort count, ILogger logger, TextBlock statusLabel)
        {
            try
            {
                logger.Verbose($"Setting max count to {count} for {Name}...");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                //Mgs2MemoryManager.UpdateObjectMaxValue(this, count, currentPc);
                statusLabel.Text = $"Max count for {Name} updated to {count}";
                logger.Verbose($"Max count set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update max count of {Name}: {e}");
                //MessageBox.Show($"Failed to update max count of {Name}: {e}");
            }
        }
    }*/
    #endregion

    #region Weapon Classes
    /*
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
                //MessageBox.Show($"Failed to toggle {Name}: {e}");
            }
        }
    }

    public class OldAmmoWeapon : OldBasicWeapon
    {
        #region Internals & Constructor
        public int CurrentAmmoOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        public int MaxAmmoOffset { get; set; } //TODO: make this a MemoryOffset

        const int MinMaxCountDiff = 72;
        private short _lastKnownCurrentAmmo = 1;
        public OldAmmoWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
            MaxAmmoOffset = inventoryOffset + MinMaxCountDiff;
        }
        #endregion

        internal new void ToggleObject(bool shouldBeEnabled, ILogger logger, TextBlock statusLabel)
        {
            //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
            short currentAmmo = BitConverter.ToInt16(Mgs2MemoryManager.GetPlayerInfoBasedValue(CurrentAmmoOffset, sizeof(short), currentPc), 0);
            //TODO: it would be cool to duplicate the "NO USE" functionality the Stinger gets when prone when disabled!
            //can't seem to easily find the bytes that control that though :(
            if (currentAmmo <= 0 && shouldBeEnabled)
            {
                if (_lastKnownCurrentAmmo != 0)
                    UpdateCurrentAmmoCount(_lastKnownCurrentAmmo, logger, statusLabel);
                else
                    UpdateCurrentAmmoCount(1, logger, statusLabel);
            }
            else if(!shouldBeEnabled)
            {
                _lastKnownCurrentAmmo = currentAmmo;
                UpdateCurrentAmmoCount(-1, logger, statusLabel);
            }
        }

        public void UpdateCurrentAmmoCount(int count, ILogger logger, TextBlock statusLabel)
        {
            ushort shortCount = (ushort)count;
            try
            {
                logger?.Verbose($"Setting current ammo to {count} for {Name}...");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, shortCount, currentPc);
                statusLabel.Text = $"Current ammo count for {Name} updated to {count}";
                logger.Verbose($"Current ammo set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update current ammo count for {Name}: {e}");
                //MessageBox.Show($"Failed to update current ammo count for {Name}: {e}");
            }
        }

        public void UpdateMaxAmmoCount(int count, ILogger logger, TextBlock statusLabel)
        {
            ushort shortCount = (ushort)count;
            try
            {
                logger.Verbose($"Setting max ammo to {count} for {Name}...");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                statusLabel.Text = $"Finding {Name} in memory...";
                Mgs2MemoryManager.UpdateObjectMaxValue(this, shortCount, currentPc);
                statusLabel.Text = $"Max ammo count for {Name} updated to {count}";
                logger.Verbose($"Max ammo set successfully");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to update max ammo count for {Name}: {e}");
                //MessageBox.Show($"Failed to update max ammo count for {Name}: {e}");
            }
        }
    }

    public class OldSpecialWeapon : OldBasicWeapon
    {
        #region Internals & Constructor
        public int SpecialOffset { get { return InventoryOffset; } set { InventoryOffset = value; } } //TODO: make this a MemoryOffset
        ushort _count = 0;
        public OldSpecialWeapon(string name, IntPtr nameMemoryOffset, int inventoryOffset) : base(name, nameMemoryOffset, inventoryOffset)
        {
        }
        #endregion

        public void SetToLethal(ILogger logger)
        {
            try
            {
                logger.Verbose($"Setting HF blade to lethal");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, _count += 1, currentPc); //TODO: determine real values
                logger.Verbose($"HF blade set to lethal successfully!");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set HF blade to lethal: {e}");
                //MessageBox.Show($"Failed to set HF blade to lethal: {e}");
            }
        }

        public void SetToStun(ILogger logger)
        {
            try
            {
                logger.Verbose($"Setting HF blade to stun");
                //Constants.PlayableCharacter currentPc = Mgs2MemoryManager.CheckIfUsable(this);
                //Mgs2MemoryManager.UpdateObjectBaseValue(this, _count -= 1, currentPc); //TODO: determine real values
                logger.Verbose($"HF blade set to lethal successfully!");
            }
            catch(Exception e)
            {
                logger.Error($"Failed to set HF blade to stun: {e}");
                //MessageBox.Show($"Failed to set HF blade to stun: {e}");
            }
        }
    }
    */
    #endregion

    public class Mgs2UsableObjects
    {
        #region Bosses
        public static readonly BossVitals Olga = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.OlgaNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.OlgaNestedPointers, HealthOffset = 0x46A, StaminaOffset = 0x46C,
            Boss = Constants.Boss.Olga
        };
        public static readonly BossVitals Fortune = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.FortuneNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.FortuneNestedPointers,HealthOffset = Mgs2Offset.FortuneHpValue.Start, 
            StaminaOffset = Mgs2Offset.FortuneStaminaValue.Start,
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
