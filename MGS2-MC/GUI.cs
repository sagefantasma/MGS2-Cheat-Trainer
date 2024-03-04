using MGS2_MC.Controllers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MGS2_MC
{
    public partial class GUI : Form
    {
        public static bool CanNavigateWithController = false;
        private static readonly List<GuiObject> itemGuiObjectList =  new List<GuiObject> ();
        private static readonly List<GuiObject> weaponGuiObjectList = new List<GuiObject> ();
        private static object CurrentlySelectedObject = null;
        private static bool? CurrentCountSelected = true;
        private static int CurrentTab = 0;
        private static int LastSelectedItemIndex = -1;
        private static int LastSelectedWeaponIndex = -1;
        private static object CurrentlySelectedOpposingObject = null;
        public static GUI StaticGuiReference = null;
        public static bool? GuiLoaded = false;
        private readonly ILogger _logger;

        internal static void ShowGui()
        {
            StaticGuiReference.Invoke(new Action(() => 
            {
                StaticGuiReference.WindowState = FormWindowState.Normal;
                StaticGuiReference.FindForm().Activate();                
            }
            ));
        }

        internal static void HideGui()
        {
            StaticGuiReference.Invoke(new Action(() => StaticGuiReference.WindowState = FormWindowState.Minimized));
        }

        internal static void NavigateViaController(ControllerInterpreter.PressedButton pressedButton, 
            ControllerInterpreter.PressedButton modifierButton = ControllerInterpreter.PressedButton.None)
        {
            if (CanNavigateWithController)
            {
                StaticGuiReference.Invoke(new Action(() =>
                {
                    if (CurrentlySelectedObject == null)
                    {
                        //we are navigating via controller for the first time or just switched tabs
                        switch (CurrentTab)
                        {
                            case 0:
                                //Item tab
                                CurrentlySelectedObject = StaticGuiReference.itemListBox.Name;
                                StaticGuiReference.itemListBox.BackColor = Color.LightCyan;
                                StaticGuiReference.itemListBox.SelectedIndex = LastSelectedItemIndex == -1 ? 0 : LastSelectedItemIndex;
                                break;
                            case 1:
                                //Weapon tab
                                CurrentlySelectedObject = StaticGuiReference.weaponListBox.Name;
                                StaticGuiReference.weaponListBox.BackColor = Color.LightCyan;
                                StaticGuiReference.weaponListBox.SelectedIndex = LastSelectedWeaponIndex == -1 ? 0 : LastSelectedWeaponIndex;
                                break;
                            default:
                                MessageBox.Show("This tab isn't yet have controller support, please use mouse & keyboard for this tab :)");
                                CurrentlySelectedObject = StaticGuiReference.stringsListBox.Name;
                                break;
                        }
                        NavigateViaController(ControllerInterpreter.PressedButton.Cross);
                        return;
                    }
                    switch (pressedButton)
                    {
                        case ControllerInterpreter.PressedButton.Cross:
                            //click on element, set value if deep enough

                            //first check to see if we're in a TableLayoutPanel(as deep as we can go)
                            if (CurrentlySelectedObject is TableLayoutPanel)
                            {
                                if (CurrentCountSelected != null)
                                {
                                    foreach (Control subControl in (CurrentlySelectedObject as TableLayoutPanel).Controls)
                                    {
                                        if (subControl is Button sendButton)
                                        {
                                            Color previousColor = sendButton.BackColor;
                                            sendButton.BackColor = Color.Green;
                                            sendButton.PerformClick();
                                            sendButton.BackColor = previousColor;
                                        }
                                    }
                                }
                                else
                                {
                                    //do nothing if we're on a button that can only be enabled or disabled and the cross button was pressed
                                    break;
                                }
                            }

                            if (CurrentlySelectedObject.ToString() == StaticGuiReference.itemListBox.Name) //TODO: determine if working as expected now
                            {
                                if (LastSelectedItemIndex == -1)
                                {
                                    //first time going into this tab, so auto-select first item
                                    CurrentlySelectedObject = StaticGuiReference.itemListBox.Items[0];
                                    StaticGuiReference.itemListBox.SelectedIndex = 0;
                                    break;
                                }
                                else
                                {
                                    //otherwise, auto-select the last chosen item
                                    CurrentlySelectedObject = StaticGuiReference.itemListBox.Items[LastSelectedItemIndex];
                                    StaticGuiReference.itemListBox.SelectedIndex = LastSelectedItemIndex;
                                    break;
                                }
                            }
                            else if (StaticGuiReference.itemListBox.Items.Contains(CurrentlySelectedObject))
                            {
                                //looking to select an item of the itemListBox
                                SelectObjectFromList(ref LastSelectedItemIndex, StaticGuiReference.itemListBox, itemGuiObjectList, ItemListBox_Click);
                                //now that item's groupBox is selected and the currentCount, if applicable, is "selected"
                            }

                            if (CurrentlySelectedObject.ToString() == StaticGuiReference.weaponListBox.Name)
                            {
                                if (LastSelectedWeaponIndex == -1)
                                {
                                    CurrentlySelectedObject = StaticGuiReference.weaponListBox.Items[0];
                                    StaticGuiReference.weaponListBox.SelectedIndex = 0;
                                    break;
                                }
                                else
                                {
                                    CurrentlySelectedObject = StaticGuiReference.weaponListBox.Items[LastSelectedWeaponIndex];
                                    StaticGuiReference.itemListBox.SelectedIndex = LastSelectedWeaponIndex;
                                    break;
                                }
                            }
                            else if (StaticGuiReference.weaponListBox.Items.Contains(CurrentlySelectedObject))
                            {
                                //looking to select an item of the weaponListBox
                                if (CurrentlySelectedObject == StaticGuiReference.hfBladeGroupBox)
                                {
                                    //the HF blade needs to have it's own logic, when/if it gets done properly
                                }
                                else
                                {
                                    SelectObjectFromList(ref LastSelectedWeaponIndex, StaticGuiReference.weaponListBox, weaponGuiObjectList, WeaponListBox_Click);
                                }
                                //now that weapons's groupBox is selected and the currentCount, if applicable, is "selected"
                            }
                            break;

                        case ControllerInterpreter.PressedButton.Circle:
                            //go back to list
                            if (CurrentlySelectedObject is TableLayoutPanel || CurrentlySelectedObject is GroupBox)
                            {
                                (CurrentlySelectedObject as Control).BackColor = Color.Transparent;
                                switch (CurrentTab)
                                {
                                    case 0:
                                        CurrentlySelectedObject = itemGuiObjectList[LastSelectedItemIndex];
                                        StaticGuiReference.itemListBox.BackColor = Color.LightCyan;
                                        break;
                                    case 1:
                                        CurrentlySelectedObject = weaponGuiObjectList[LastSelectedWeaponIndex];
                                        StaticGuiReference.weaponListBox.BackColor = Color.LightCyan;
                                        break;
                                }
                                //(CurrentlySelectedObject as Control).BackColor = Color.LightCyan;
                            }
                            break;

                        case ControllerInterpreter.PressedButton.Triangle:
                            //enable/disable(no need to go into item i think)
                            /*if (CurrentlySelectedObject != null)
                            {
                                if (CurrentlySelectedObject is GroupBox currentGroupBox)
                                {
                                    foreach (Control subControl in currentGroupBox.Controls[0].Controls)
                                    {
                                        if (subControl is CheckBox enableItemCheckBox)
                                        {
                                            enableItemCheckBox.Checked = !enableItemCheckBox.Checked;
                                        }
                                    }
                                }
                                else if(CurrentlySelectedObject is TableLayoutPanel activeLayoutPanel)
                                {
                                    foreach (Control subControl in activeLayoutPanel.Controls)
                                    {
                                        if (subControl is CheckBox enableItemCheckBox)
                                        {
                                            enableItemCheckBox.Checked = !enableItemCheckBox.Checked;
                                        }
                                    }
                                }
                            }*/
                            switch (CurrentTab)
                            {
                                case 0:
                                    foreach(Control subControl in itemGuiObjectList[StaticGuiReference.itemListBox.SelectedIndex].AssociatedControl.Controls[0].Controls)
                                    {
                                        if(subControl is CheckBox enableItemCheckBox)
                                        {
                                            enableItemCheckBox.Checked = !enableItemCheckBox.Checked;
                                        }
                                    }
                                    break;
                                case 1:
                                    foreach(Control subControl in weaponGuiObjectList[StaticGuiReference.weaponListBox.SelectedIndex].AssociatedControl.Controls[0].Controls)
                                    {
                                        if (subControl is CheckBox enableWeaponCheckBox)
                                        {
                                            enableWeaponCheckBox.Checked = !enableWeaponCheckBox.Checked;
                                        }
                                    }
                                    break;
                            }
                            break;

                        case ControllerInterpreter.PressedButton.Square:
                            //change from current/max(if applicable)
                            if (CurrentlySelectedOpposingObject != null)
                            {
                                object tempObject = CurrentlySelectedObject;
                                CurrentlySelectedObject = CurrentlySelectedOpposingObject;
                                (CurrentlySelectedObject as Control).BackColor = Color.LightCyan;
                                CurrentlySelectedOpposingObject = tempObject;
                                (CurrentlySelectedOpposingObject as Control).BackColor = Color.Transparent;
                            }
                            break;

                        case ControllerInterpreter.PressedButton.L1:
                            // move to tab on left
                            if (StaticGuiReference.mgs2TabControl.TabIndex == 0)
                                StaticGuiReference.mgs2TabControl.TabIndex = StaticGuiReference.mgs2TabControl.TabCount - 1;
                            else
                                StaticGuiReference.mgs2TabControl.TabIndex--;
                            StaticGuiReference.mgs2TabControl.SelectTab(StaticGuiReference.mgs2TabControl.TabIndex);
                            StaticGuiReference.Mgs2TabControl_SelectedIndexChanged(null, null);
                            break;

                        case ControllerInterpreter.PressedButton.L2:
                            //??
                            break;

                        case ControllerInterpreter.PressedButton.L3:
                            //minimize value
                            break;

                        case ControllerInterpreter.PressedButton.R1:
                            // move to tab on right
                            if (StaticGuiReference.mgs2TabControl.TabIndex == StaticGuiReference.mgs2TabControl.TabCount - 1)
                                StaticGuiReference.mgs2TabControl.TabIndex = 0;
                            else
                                StaticGuiReference.mgs2TabControl.TabIndex++;
                            StaticGuiReference.mgs2TabControl.SelectTab(StaticGuiReference.mgs2TabControl.TabIndex);
                            StaticGuiReference.Mgs2TabControl_SelectedIndexChanged(null, null);
                            break;

                        case ControllerInterpreter.PressedButton.R2:
                            //??
                            break;

                        case ControllerInterpreter.PressedButton.R3:
                            //maximize value
                            break;

                        case ControllerInterpreter.PressedButton.Select:

                            break;

                        case ControllerInterpreter.PressedButton.Start:

                            break;

                        case ControllerInterpreter.PressedButton.UpDirectional:
                            if (CurrentlySelectedObject is GuiObject)
                            {
                                switch (CurrentTab)
                                {
                                    case 0:
                                        int currentItemIndex = StaticGuiReference.itemListBox.SelectedIndex;
                                        if (currentItemIndex == 0)
                                            StaticGuiReference.itemListBox.SelectedIndex = StaticGuiReference.itemListBox.Items.Count - 1;
                                        else
                                            StaticGuiReference.itemListBox.SelectedIndex--;
                                        CurrentlySelectedObject = StaticGuiReference.itemListBox.SelectedItem;
                                        PreviewObjectFromList(ItemListBox_Click);
                                        break;
                                    case 1:
                                        int currentWeaponIndex = StaticGuiReference.weaponListBox.SelectedIndex;
                                        if (currentWeaponIndex == 0)
                                            StaticGuiReference.weaponListBox.SelectedIndex = StaticGuiReference.weaponListBox.Items.Count - 1;
                                        else
                                            StaticGuiReference.weaponListBox.SelectedIndex--;
                                        CurrentlySelectedObject = StaticGuiReference.weaponListBox.SelectedItem;
                                        PreviewObjectFromList(WeaponListBox_Click);
                                        break;
                                }
                                break;
                            }

                            if (modifierButton == ControllerInterpreter.PressedButton.RightDirectional)
                            {

                            }
                            else if (modifierButton == ControllerInterpreter.PressedButton.LeftDirectional)
                            {

                            }
                            else
                            {
                                //increase value by 10
                                if (CurrentlySelectedObject is TableLayoutPanel upTableLayoutPanel)
                                {
                                    foreach (Control control in upTableLayoutPanel.Controls)
                                    {
                                        if (control is NumericUpDown numericUpDown)
                                        {
                                            try
                                            {
                                                numericUpDown.Value += 10;
                                            }
                                            catch
                                            {
                                                numericUpDown.Value = numericUpDown.Maximum;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case ControllerInterpreter.PressedButton.RightDirectional:
                            //increase value by 1
                            if (CurrentlySelectedObject is TableLayoutPanel rightTableLayoutPanel)
                            {
                                foreach (Control control in rightTableLayoutPanel.Controls)
                                {
                                    if (control is NumericUpDown numericUpDown)
                                    {
                                        try
                                        {
                                            numericUpDown.Value++;
                                        }
                                        catch
                                        {
                                            numericUpDown.Value = numericUpDown.Maximum;
                                        }
                                    }
                                }
                            }
                            break;

                        case ControllerInterpreter.PressedButton.LeftDirectional:
                            //decrease value by 1
                            if (CurrentlySelectedObject is TableLayoutPanel leftTableLayoutPanel)
                            {
                                foreach (Control control in leftTableLayoutPanel.Controls)
                                {
                                    if (control is NumericUpDown numericUpDown)
                                    {
                                        try
                                        {
                                            numericUpDown.Value--;
                                        }
                                        catch
                                        {
                                            numericUpDown.Value = numericUpDown.Minimum;
                                        }
                                    }
                                }
                            }
                            break;

                        case ControllerInterpreter.PressedButton.DownDirectional:
                            if (CurrentlySelectedObject is GuiObject)
                            {
                                switch (CurrentTab)
                                {
                                    case 0:
                                        int currentItemIndex = StaticGuiReference.itemListBox.SelectedIndex;
                                        if (currentItemIndex == StaticGuiReference.itemListBox.Items.Count - 1)
                                            StaticGuiReference.itemListBox.SelectedIndex = 0;
                                        else
                                            StaticGuiReference.itemListBox.SelectedIndex++;
                                        CurrentlySelectedObject = StaticGuiReference.itemListBox.SelectedItem;
                                        PreviewObjectFromList(ItemListBox_Click);
                                        break;
                                    case 1:
                                        int currentWeaponIndex = StaticGuiReference.weaponListBox.SelectedIndex;
                                        if (currentWeaponIndex == StaticGuiReference.weaponListBox.Items.Count - 1)
                                            StaticGuiReference.weaponListBox.SelectedIndex = 0;
                                        else
                                            StaticGuiReference.weaponListBox.SelectedIndex++;
                                        CurrentlySelectedObject = StaticGuiReference.weaponListBox.SelectedItem;
                                        PreviewObjectFromList(WeaponListBox_Click);
                                        break;
                                }
                                break;
                            }

                            if (modifierButton == ControllerInterpreter.PressedButton.RightDirectional)
                            {

                            }
                            else if (modifierButton == ControllerInterpreter.PressedButton.LeftDirectional)
                            {

                            }
                            else
                            {
                                //decrease value by 10
                                if (CurrentlySelectedObject is TableLayoutPanel downTableLayoutPanel)
                                {
                                    foreach (Control control in downTableLayoutPanel.Controls)
                                    {
                                        if (control is NumericUpDown numericUpDown)
                                        {
                                            try
                                            {
                                                numericUpDown.Value -= 10;
                                            }
                                            catch
                                            {
                                                numericUpDown.Value = numericUpDown.Minimum;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }));
            }
        }

        private static void PreviewObjectFromList(Action StaticFunction)
        {
            StaticFunction.Invoke();
        }

        private static void SelectObjectFromList(ref int lastSelectedIndex, ListBox desiredListBox, List<GuiObject> desiredObjectList, Action StaticFunction)
        {
            lastSelectedIndex = desiredListBox.SelectedIndex;
            (CurrentlySelectedObject as GuiObject).AssociatedControl.BackColor = Color.Transparent;
            desiredListBox.BackColor = Color.White;
            CurrentlySelectedObject = desiredObjectList[desiredListBox.SelectedIndex].AssociatedControl; //currently selecting a groupbox
            StaticFunction.Invoke();
            (CurrentlySelectedObject as Control).BackColor = Color.LightCyan;
            
            if ((CurrentlySelectedObject as GroupBox).Controls[0] //this is the layoutpanel within the groupbox
            .Controls.Count > 2)
            {
                CurrentCountSelected = true; //always start at CurrentCount for anything with current count
                foreach (Control subControl in (CurrentlySelectedObject as GroupBox).Controls[0].Controls)
                {
                    if (subControl.Name.Contains("CurrentLayoutPanel"))
                    {
                        (CurrentlySelectedObject as Control).BackColor = Color.Transparent;
                        CurrentlySelectedObject = subControl;
                        subControl.BackColor = Color.LightCyan;
                    }
                    if (subControl.Name.Contains("MaxLayoutPanel"))
                    {
                        CurrentlySelectedOpposingObject = subControl; //for simplified switching later on
                    }
                }
            }
            else
            {
                CurrentCountSelected = null; //null the boolean if we don't have counts
            }
        }

        private void BuildGuiObjectLists()
        {
            itemGuiObjectList.Add(new GuiObject("AK Suppressor", akSupGroupBox));
            itemGuiObjectList.Add(new GuiObject("AP Sensor", apSensorGroupBox));
            itemGuiObjectList.Add(new GuiObject("Bandana", bandanaGroupBox));
            itemGuiObjectList.Add(new GuiObject("Bandage", bandageGroupBox));
            itemGuiObjectList.Add(new GuiObject("BDU", bduGroupBox));
            itemGuiObjectList.Add(new GuiObject("Blue Wig", blueWigGroupBox));
            itemGuiObjectList.Add(new GuiObject("Body Armor", bodyArmorGroupBox));
            itemGuiObjectList.Add(new GuiObject("Box 1", box1GroupBox));
            itemGuiObjectList.Add(new GuiObject("Box 2", box2GroupBox));
            itemGuiObjectList.Add(new GuiObject("Box 3", box3GroupBox));
            itemGuiObjectList.Add(new GuiObject("Box 4", box4GroupBox));
            itemGuiObjectList.Add(new GuiObject("Box 5", box5GroupBox));
            itemGuiObjectList.Add(new GuiObject("Camera", camera1GroupBox));
            itemGuiObjectList.Add(new GuiObject("Cigarettes", cigarettesGroupBox));
            itemGuiObjectList.Add(new GuiObject("Cold Medicine", coldMedsGroupBox));
            itemGuiObjectList.Add(new GuiObject("Digital Camera", digitalCameraGroupBox));
            itemGuiObjectList.Add(new GuiObject("Infinity Wig", infinityWigGroupBox));
            itemGuiObjectList.Add(new GuiObject("MO Disc", moDiscGroupBox));
            itemGuiObjectList.Add(new GuiObject("Mine Detector", mineDetectorGroupBox));
            itemGuiObjectList.Add(new GuiObject("Night Vision Goggles", nvgGroupBox));
            itemGuiObjectList.Add(new GuiObject("Orange Wig", orangeWigGroupBox));
            itemGuiObjectList.Add(new GuiObject("Pentazemin", pentazeminGroupBox));
            itemGuiObjectList.Add(new GuiObject("Phone", phoneGroupBox));
            itemGuiObjectList.Add(new GuiObject("Ration", rationGroupBox));
            itemGuiObjectList.Add(new GuiObject("Scope", scope1GroupBox));
            itemGuiObjectList.Add(new GuiObject("Security Card", cardGroupBox));
            itemGuiObjectList.Add(new GuiObject("Sensor A", sensorAGroupBox));
            itemGuiObjectList.Add(new GuiObject("Sensor B", sensorBGroupBox));
            itemGuiObjectList.Add(new GuiObject("Shaver", shaverGroupBox));
            itemGuiObjectList.Add(new GuiObject("Socom Suppressor", socomGroupBox));
            itemGuiObjectList.Add(new GuiObject("Stealth", stealthGroupBox));
            itemGuiObjectList.Add(new GuiObject("Thermal Goggles", thermalGroupBox));
            itemGuiObjectList.Add(new GuiObject("USP Suppressor", uspSupGroupBox));
            itemGuiObjectList.Add(new GuiObject("Wet Box", wetBoxGroupBox));

            weaponGuiObjectList.Add(new GuiObject("AKS-74u", akGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Book", bookGroupBox));
            weaponGuiObjectList.Add(new GuiObject("C4", c4GroupBox));
            weaponGuiObjectList.Add(new GuiObject("Chaff Grenade", chaffGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Claymore", claymoreGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Coolant", coolantGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Directional Mic", dmic1GroupBox));
            weaponGuiObjectList.Add(new GuiObject("Directional Mic(Zoomed)", dmic2GroupBox));
            weaponGuiObjectList.Add(new GuiObject("Grenade", grenadeGroupBox));
            weaponGuiObjectList.Add(new GuiObject("High Frequency Blade", hfBladeGroupBox));
            weaponGuiObjectList.Add(new GuiObject("M4", m4GroupBox));
            weaponGuiObjectList.Add(new GuiObject("M9", m9GroupBox));
            weaponGuiObjectList.Add(new GuiObject("Magazine", magazineGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Nikita", nikitaGroupBox));
            weaponGuiObjectList.Add(new GuiObject("PSG1", psg1GroupBox));
            weaponGuiObjectList.Add(new GuiObject("PSG1-T", psg1TGroupBox));
            weaponGuiObjectList.Add(new GuiObject("RGB6", rgb6GroupBox));
            weaponGuiObjectList.Add(new GuiObject("SOCOM", socomGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Stinger", stingerGroupBox));
            weaponGuiObjectList.Add(new GuiObject("Stun Grenade", stunGroupBox));
            weaponGuiObjectList.Add(new GuiObject("USP", uspGroupBox));
        }

        private void BuildGroupBoxForObject(MGS2Object objectToEdit)
        {
            //TODO: i think this is all set to be removed
            //envisioning a listbox on the left pane to choose item to edit
            //then the item you select determines what shows up in the right pane
            //eventually, this should dovetail well into the controller support

            //all items need the enable checkbox & picture
            switch(objectToEdit)
            {
                case AmmoWeapon ammoWeapon:
                    //need current ammo updown & set and max ammo updown & set
                    break;
                case SpecialWeapon specialWeapon:
                    //need lethal/stun buttons
                    break;
                case BasicWeapon basicWeapon:
                    //don't need anything else
                    break;
                case StackableItem stackableItem:
                    //need current count updown & set
                    break;
                case DurabilityItem durabilityItem:
                    //need durability updown & set
                    break;
                case LevelableItem levelableItem:
                    //need level updown & set
                    break;
                case BasicItem basicItem:
                    //don't need anything else
                    break;
            }
        }

        public GUI(ILogger logger)
        {
            _logger = logger;
            StaticGuiReference = this;
            InitializeComponent();
            BuildGuiObjectLists();
            itemListBox.DataSource = itemGuiObjectList;
            itemListBox.DisplayMember = "Name";
            itemListBox.SelectedIndex = -1;
            weaponListBox.DataSource = weaponGuiObjectList;
            weaponListBox.DisplayMember = "Name";
            weaponListBox.SelectedIndex = -1;
            stringsListBox.DataSource = MGS2Strings.MGS2_STRINGS;
            stringsListBox.DisplayMember = "Tag";
            stringsListBox.SelectedIndex = -1;
            mgs2TabControl.TabPages.RemoveByKey(tabPageStats.Name);
            GuiLoaded = true;
        }

        #region GUI getters
        private short CurrentRationValue()
        {
            return (short)rationCurrentUpDown.Value;
        }

        private short MaxRationValue()
        {
            return (short)rationMaxUpDown.Value;
        }

        private short CurrentBandageValue()
        {
            return (short)bandageCurrentUpDown.Value;
        }

        private short MaxBandageValue()
        {
            return (short)bandageMaxUpDown.Value;
        }

        private short CurrentPentazeminCount()
        {
            return (short)pentazeminCurrentUpDown.Value;
        }

        private short MaxPentazeminCount()
        {
            return (short)pentazeminMaxUpDown.Value;
        }

        private short CurrentDogTagCount()
        {
            return (short)dogTagsCurrentUpDown.Value;
        }

        private short MaxDogTagCount()
        {
            return (short)dogTagsMaxUpDown.Value;
        }

        private short CardSecurityLevel()
        {
            return (short)cardUpDown.Value;
        }

        private short M9CurrentAmmoCount()
        {
            return (short)m9CurrentUpDown.Value;
        }

        private short M9MaxAmmoCount()
        {
            return (short)m9MaxUpDown.Value;
        }

        private short USPCurrentAmmoCount()
        {
            return (short)uspCurrentUpDown.Value;
        }

        private short USPMaxAmmoCount()
        {
            return (short)uspMaxUpDown.Value;
        }

        private short SOCOMCurrentAmmoCount()
        {
            return (short)socomCurrentUpDown.Value;
        }

        private short SOCOMMaxAmmoCount()
        {
            return (short)socomMaxUpDown.Value;
        }

        private short PSG1CurrentAmmoCount()
        {
            return (short)psg1CurrentUpDown.Value;
        }

        private short PSG1MaxAmmoCount()
        {
            return (short)psg1MaxUpDown.Value;
        }

        private short RGB6CurrentAmmoCount()
        {
            return (short)rgb6CurrentUpDown.Value;
        }

        private short RGB6MaxAmmoCount()
        {
            return (short)rgb6MaxUpDown.Value;
        }

        private short NikitaCurrentAmmoCount()
        {
            return (short)nikitaCurrentUpDown.Value;
        }

        private short NikitaMaxAmmoCount()
        {
            return (short)nikitaMaxUpDown.Value;
        }

        private short StingerCurrentAmmoCount()
        {
            return (short)stingerCurrentUpDown.Value;
        }

        private short StingerMaxAmmoCount()
        {
            return (short)stingerMaxUpDown.Value;
        }

        private short C4CurrentAmmoCount()
        {
            return (short)c4CurrentUpDown.Value;
        }

        private short C4MaxAmmoCount()
        {
            return (short)c4MaxUpDown.Value;
        }

        private short AKCurrentAmmoCount()
        {
            return (short)akCurrentUpDown.Value;
        }

        private short AKMaxAmmoCount()
        {
            return (short)akMaxUpDown.Value;
        }

        private short M4CurrentAmmoCount()
        {
            return (short)m4CurrentUpDown.Value;
        }

        private short M4MaxAmmoCount()
        {
            return (short)m4MaxUpDown.Value;
        }

        private short PSG1TCurrentAmmoCount()
        {
            return (short)psg1TCurrentUpDown.Value;
        }

        private short PSG1TMaxAmmoCount()
        {
            return (short)psg1TMaxUpDown.Value;
        }

        private short ChaffCurrentAmmoCount()
        {
            return (short)chaffCurrentUpDown.Value;
        }

        private short ChaffMaxAmmoCount()
        {
            return (short)chaffMaxUpDown.Value;
        }

        private short StunCurrentAmmoCount()
        {
            return (short)stunCurrentUpDown.Value;
        }

        private short StunMaxAmmoCount()
        {
            return (short)stunMaxUpDown.Value;
        }

        private short GrenadeCurrentAmmoCount()
        {
            return (short)grenadeCurrentUpDown.Value;
        }

        private short GrenadeMaxAmmoCount()
        {
            return (short)grenadeMaxUpDown.Value;
        }

        private short BookCurrentAmmoCount()
        {
            return (short)bookCurrentUpDown.Value;
        }

        private short BookMaxAmmoCount()
        {
            return (short)bookMaxUpDown.Value;
        }

        private short MagazineCurrentAmmoCount()
        {
            return (short)magazineCurrentUpDown.Value;
        }

        private short MagazineMaxAmmoCount()
        {
            return (short)magazineMaxUpDown.Value;
        }

        private short ClaymoreCurrentAmmoCount()
        {
            return (short)claymoreCurrentUpDown.Value;
        }

        private short ClaymoreMaxAmmoCount()
        {
            return (short)claymoreMaxUpDown.Value;
        }

        private bool DMic1Enabled()
        {
            return dmic1CheckBox.Checked;
        }

        private bool DMic2Enabled()
        {
            return dmic2CheckBox.Checked;
        }

        private bool CoolantEnabled()
        {
            return coolantCheckBox.Checked;
        }

        private bool HfBladeEnabled()
        {
            return hfBladeCheckBox.Checked;
        }
        #endregion

        #region Items Button Functions
        private void RationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.ToggleItem(rationCheckBox.Checked);
        }

        private void RationCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateCurrentCount(CurrentRationValue());
        }

        private void RationMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateMaxCount(MaxRationValue());
        }

        private void BandageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.ToggleItem(bandageCheckBox.Checked);
        }

        private void BandageCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateCurrentCount(CurrentBandageValue());
        }

        private void BandageMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateMaxCount(MaxBandageValue());
        }

        private void PentazeminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.ToggleItem(pentazeminCheckBox.Checked);
        }

        private void PentazeminCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateCurrentCount(CurrentPentazeminCount());
        }

        private void PentazeminMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateMaxCount(MaxPentazeminCount());
        }

        private void CardBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.SetLevel(CardSecurityLevel());
        }

        private void CardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.ToggleItem(cardCheckBox.Checked);
        }

        private void Binos1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SnakeScope.ToggleItem(scope1CheckBox.Checked);
        }

        private void Binos2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RaidenScope.ToggleItem(scope2CheckBox.Checked);
        }

        private void Camera1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Camera1.ToggleItem(camera1CheckBox.Checked);
        }

        private void DigitalCameraCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DigitalCamera.ToggleItem(digitalCameraCheckBox.Checked);
        }

        private void NvgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.NightVisionGoggles.ToggleItem(nvgCheckBox.Checked);
        }

        private void ThermalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ThermalGoggles.ToggleItem(thermalCheckBox.Checked);
        }

        private void BodyArmorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BodyArmor.ToggleItem(bodyArmorCheckBox.Checked);
        }

        private void MineDetectorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MineDetector.ToggleItem(mineDetectorCheckBox.Checked);
        }

        private void ApSensorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.APSensor.ToggleItem(apSensorCheckBox.Checked);
        }

        private void SensorACheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorA.ToggleItem(sensorACheckBox.Checked);
        }

        private void SensorBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorB.ToggleItem(sensorBCheckBox.Checked);
        }

        private void PhoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Phone.ToggleItem(phoneCheckBox.Checked);
        }

        private void ColdMedsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ColdMedicine.ToggleItem(coldMedsCheckBox.Checked);
        }

        private void CigarettesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Cigarettes.ToggleItem(cigarettesCheckBox.Checked);
        }

        private void MoDiscCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MODisc.ToggleItem(moDiscCheckBox.Checked);
        }

        private void SocomSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SocomSuppressor.ToggleItem(socomSupCheckBox.Checked);
        }

        private void UspSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USPSuppressor.ToggleItem(uspSupCheckBox.Checked);
        }

        private void AkSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKSuppressor.ToggleItem(akSupCheckBox.Checked);
        }

        private void Box1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box1.ToggleItem(box1CheckBox.Checked);
        }

        private void Box2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.ToggleItem(box2CheckBox.Checked);
        }

        private void Box3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.ToggleItem(box3CheckBox.Checked);
        }

        private void Box4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.ToggleItem(box4CheckBox.Checked);
        }

        private void Box5CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.ToggleItem(box5CheckBox.Checked);
        }

        private void WetBoxCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.ToggleItem(wetBoxCheckBox.Checked);
        }

        private void BduCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BDU.ToggleItem(bduCheckBox.Checked);
        }

        private void BduMaskCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //MGS2UsableObjects.BDU.SetDurability(2);
        }

        private void BandanaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandana.ToggleItem(bandanaCheckBox.Checked);
        }

        private void InfinityWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.InfinityWig.ToggleItem(infinityWigCheckBox.Checked);
        }

        private void BlueWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BlueWig.ToggleItem(blueWigCheckBox.Checked);
        }

        private void OrangeWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.OrangeWig.ToggleItem(orangeWigCheckBox.Checked);
        }

        private void StealthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stealth.ToggleItem(stealthCheckBox.Checked);
        }

        private void DogTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DogTags.ToggleItem(dogTagsCheckBox.Checked);
        }

        private void ShaverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Shaver.ToggleItem(shaverCheckBox.Checked);
        }

        private void ColdMedsCurrentBtn_Click(object sender, EventArgs e)
        {
            //MGS2UsableObjects.ColdMedicine.UpdateCurrentCount(CurrentColdMedsCount());
            //TODO: remove this function
        }

        private void ColdMedsMaxBtn_Click(object sender, EventArgs e)
        {
            //MGS2UsableObjects.ColdMedicine.UpdateMaxCount(MaxColdMedsCount());
            //TODO: remove this function
        }

        private void box1Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box1.SetDurability((short)box1UpDown.Value);
        }

        private void box2Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.SetDurability((short)box2UpDown.Value);
        }

        private void box3Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.SetDurability((short)box3UpDown.Value);
        }

        private void box4Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.SetDurability((short)box4UpDown.Value);
        }

        private void box5Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.SetDurability((short)box5UpDown.Value);
        }

        private void wetBoxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.SetDurability((short)wetBoxUpDown.Value);
        }
        #endregion

        #region Weapons Button Functions
        private void AkMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateMaxAmmoCount(AKMaxAmmoCount());
        }

        private void M9CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateCurrentAmmoCount(M9CurrentAmmoCount());
        }

        private void M9MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateMaxAmmoCount(M9MaxAmmoCount());
        }

        private void SocomCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateCurrentAmmoCount(SOCOMCurrentAmmoCount());
        }

        private void SocomMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateMaxAmmoCount(SOCOMMaxAmmoCount());
        }

        private void UspCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateCurrentAmmoCount(USPCurrentAmmoCount());
        }

        private void UspMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateMaxAmmoCount(USPMaxAmmoCount());
        }

        private void ChaffCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateCurrentAmmoCount(ChaffCurrentAmmoCount());
        }

        private void ChaffMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateMaxAmmoCount(ChaffMaxAmmoCount());
        }

        private void StunCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateCurrentAmmoCount(StunCurrentAmmoCount());
        }

        private void StunMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateMaxAmmoCount(StunMaxAmmoCount());
        }

        private void GrenadeCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateCurrentAmmoCount(GrenadeCurrentAmmoCount());
        }

        private void GrenadeMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateMaxAmmoCount(GrenadeMaxAmmoCount());
        }

        private void MagazineCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateCurrentAmmoCount(MagazineCurrentAmmoCount());
        }

        private void MagazineMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateMaxAmmoCount(MagazineMaxAmmoCount());
        }

        private void AkCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateCurrentAmmoCount(AKCurrentAmmoCount());
        }

        private void M4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateCurrentAmmoCount(M4CurrentAmmoCount());
        }

        private void M4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateMaxAmmoCount(M4MaxAmmoCount());
        }

        private void Psg1CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateCurrentAmmoCount(PSG1CurrentAmmoCount());
        }

        private void Psg1MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateMaxAmmoCount(PSG1MaxAmmoCount());
        }

        private void Psg1TCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateCurrentAmmoCount(PSG1TCurrentAmmoCount());
        }

        private void Psg1TMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateMaxAmmoCount(PSG1TMaxAmmoCount());
        }

        private void Rgb6CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateCurrentAmmoCount(RGB6CurrentAmmoCount());
        }

        private void Rgb6MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateMaxAmmoCount(RGB6MaxAmmoCount());
        }

        private void NikitaCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateCurrentAmmoCount(NikitaCurrentAmmoCount());
        }

        private void NikitaMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateMaxAmmoCount(NikitaMaxAmmoCount());
        }

        private void StingerCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateCurrentAmmoCount(StingerCurrentAmmoCount());
        }

        private void StingerMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateMaxAmmoCount(StingerMaxAmmoCount());
        }

        private void ClaymoreCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateCurrentAmmoCount(ClaymoreCurrentAmmoCount());
        }

        private void ClaymoreMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateMaxAmmoCount(ClaymoreMaxAmmoCount());
        }

        private void C4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateCurrentAmmoCount(ClaymoreCurrentAmmoCount());
        }

        private void C4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateMaxAmmoCount(ClaymoreMaxAmmoCount());
        }

        private void BookCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateCurrentAmmoCount(BookCurrentAmmoCount());
        }

        private void BookMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateMaxAmmoCount(BookMaxAmmoCount());
        }

        private void HfBladeLethalBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToLethal();
        }

        private void HfBladeStunBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToStun();
        }

        private void M9CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.ToggleWeapon(m9CheckBox.Checked);
        }

        private void SocomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.ToggleWeapon(socomCheckBox.Checked);
        }

        private void UspCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.ToggleWeapon(uspCheckBox.Checked);
        }

        private void ChaffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.ToggleWeapon(chaffCheckBox.Checked);
        }

        private void StunCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.ToggleWeapon(stunCheckBox.Checked);
        }

        private void GrenadeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.ToggleWeapon(grenadeCheckBox.Checked);
        }

        private void MagazineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.ToggleWeapon(magazineCheckBox.Checked);
        }

        private void AkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.ToggleWeapon(akCheckBox.Checked);
        }

        private void M4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.ToggleWeapon(m4CheckBox.Checked);
        }

        private void Psg1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.ToggleWeapon(psg1CheckBox.Checked);
        }

        private void Psg1TCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.ToggleWeapon(psg1TCheckBox.Checked);
        }

        private void Rgb6CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.ToggleWeapon(rgb6CheckBox.Checked);
        }

        private void NikitaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.ToggleWeapon(nikitaCheckBox.Checked);
        }

        private void StingerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.ToggleWeapon(stingerCheckBox.Checked);
        }

        private void ClaymoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.ToggleWeapon(claymoreCheckBox.Checked);
        }

        private void C4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.ToggleWeapon(c4CheckBox.Checked);
        }

        private void BookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.ToggleWeapon(bookCheckBox.Checked);
        }

        private void CoolantCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Coolant.ToggleWeapon(coolantCheckBox.Checked);
        }

        private void Dmic1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic1.ToggleWeapon(dmic1CheckBox.Checked);
        }

        private void Dmic2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic2.ToggleWeapon(dmic2CheckBox.Checked);
        }

        private void HfBladeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.ToggleWeapon(hfBladeCheckBox.Checked);
        }
        #endregion

        private void StringsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GuiLoaded == true)
            {
                StaticGuiReference.Invoke(new Action(() =>
                {
                    MGS2Strings.MGS2String selectedString = stringsListBox.SelectedItem as MGS2Strings.MGS2String;
                    StaticGuiReference.basicNameTextBox.Text = selectedString.CurrentText;
                    StaticGuiReference.basicNameTextBox.MaxLength = selectedString.MemoryOffset.Length;
                    StaticGuiReference.characterLimitLabel.Text = selectedString.MemoryOffset.Length.ToString();
                    StaticGuiReference.basicNameGroupBox.Text = selectedString.Tag;
                }));
            }
        }

        private void setBasicNameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MGS2Strings.MGS2String selectedString = stringsListBox.SelectedItem as MGS2Strings.MGS2String;
                MGS2MemoryManager.UpdateGameString(selectedString, basicNameTextBox.Text);
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to update game string: {ex}");
                MessageBox.Show($"Failed to update game string");
            }
        }

        private void ItemListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ItemListBox_Click();
        }

        private static void ItemListBox_Click()
        {
            StaticGuiReference.Invoke(new Action(() =>
            {
                if (StaticGuiReference.itemGroupBox.Controls.Count > 0)
                    StaticGuiReference.itemGroupBox.Controls[0].Visible = false;
                StaticGuiReference.itemGroupBox.Controls.Clear();

                GuiObject itemObject = itemGuiObjectList.First(guiObject => guiObject.Name == (StaticGuiReference.itemListBox.SelectedItem as GuiObject).Name);

                StaticGuiReference.itemGroupBox.Controls.Add(itemObject.AssociatedControl);
                itemObject.AssociatedControl.Visible = true;
            }));
        }

        private void weaponListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WeaponListBox_Click();
        }

        private static void WeaponListBox_Click()
        {
            StaticGuiReference.Invoke(new Action(() =>
            {
                if (StaticGuiReference.weaponGroupBox.Controls.Count > 0)
                    StaticGuiReference.weaponGroupBox.Controls[0].Visible = false;
                StaticGuiReference.weaponGroupBox.Controls.Clear();

                GuiObject weaponObject = weaponGuiObjectList.First(guiObject => guiObject.Name == (StaticGuiReference.weaponListBox.SelectedItem as GuiObject).Name);

                StaticGuiReference.weaponGroupBox.Controls.Add(weaponObject.AssociatedControl);
                weaponObject.AssociatedControl.Visible = true;
            }));
        }

        private void Mgs2TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentTab = mgs2TabControl.SelectedIndex;
            CurrentlySelectedObject = null;
        }

        private void GithubMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sagefantasma/MGS2-Cheat-Trainer");
        }

        private void ViewLogsMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Logging.LogLocation);
        }

        private void ModifyConfigMenuItem_Click(object sender, EventArgs e)
        {
            ConfigEditorForm configEditorForm = new ConfigEditorForm(_logger);
            
            if(configEditorForm.ShowDialog() == DialogResult.OK)
            {
                MGS2Monitor.LoadConfig();
            }            
        }

        private void LaunchMgs2MenuItem_Click(object sender, EventArgs e)
        {
            if (MGS2Monitor.MGS2Process != null)
            {
                MessageBox.Show("MGS2 is already running, please exit MGS2 before attempting to launch it again.");
            }
            else if(!Program.MGS2Thread.IsAlive)
            {
                try
                {
                    Program.RestartMonitoringThread();
                }
                catch(Exception ex)
                {
                    _logger.Error($"Failed to start MGS2 thread: {ex}");
                }
            }
            else
            {
                Program.MGS2Thread.Abort(); 
                Program.RestartMonitoringThread();
                //Program.MGS2Thread.
            }
        }

        public static void ToggleLaunchMGS2Option()
        {
            StaticGuiReference.Invoke(new Action(() =>
            {
                StaticGuiReference.launchMgs2MenuItem.Enabled = !StaticGuiReference.launchMgs2MenuItem.Enabled;
            }));
        }
    }
}
