using MGS2_MC.Controllers;
using MGS2_MC.Helpers;
using Serilog;
using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
        private static bool UserHasBeenWarned = false;
        private PeriodicTask _reapplyFilterTask;
        private TreeNode CurrentBossNode = null;
        List<Task> BossTasks = new List<Task> ();
        Task CheatsTabTask;

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
                                MessageBox.Show(@"This tab doesn't yet have controller support, please use mouse & keyboard for this tab :)");
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
            itemGuiObjectList.Add(new GuiObject("SOCOM Suppressor", socomSupGroupBox));
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

            foreach(Cheat cheat in MGS2Cheat.CheatList)
            {
                cheatsCheckedListBox.Items.Add(cheat);
            }
            cheatsCheckedListBox.DisplayMember = "Name";
        }

        public GUI(ILogger logger)
        {
            _logger = logger;
            StaticGuiReference = this;
            InitializeComponent();
            this.Text += $" - v.{Program.AppVersion}";
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
            guardAnimationComboBox.DataSource = MGS2AoB.GuardAnimationList;
            guardAnimationComboBox.DisplayMember = "Name";
            GuiLoaded = true;
            //MessageBox.Show("As of MGS2 version 2.0, a lot of this trainer's functionalities have been heavily affected. We're fixing things up as quickly as we can, please be patient and bear with us as we continue to work. Thank you!\n\n Please note: This message box will stop appearing on trainer launch once all functionalities have been restored.", "Trainer under maintenance");
        }

        #region GUI getters
        private ushort CurrentRationValue()
        {
            return (ushort)rationCurrentUpDown.Value;
        }

        private ushort MaxRationValue()
        {
            return (ushort)rationMaxUpDown.Value;
        }

        private ushort CurrentBandageValue()
        {
            return (ushort)bandageCurrentUpDown.Value;
        }

        private ushort MaxBandageValue()
        {
            return (ushort)bandageMaxUpDown.Value;
        }

        private ushort CurrentPentazeminCount()
        {
            return (ushort)pentazeminCurrentUpDown.Value;
        }

        private ushort MaxPentazeminCount()
        {
            return (ushort)pentazeminMaxUpDown.Value;
        }

        private ushort CurrentDogTagCount()
        {
            return (ushort)dogTagsCurrentUpDown.Value;
        }

        private ushort MaxDogTagCount()
        {
            return (ushort)dogTagsMaxUpDown.Value;
        }

        private ushort CardSecurityLevel()
        {
            return (ushort)cardUpDown.Value;
        }

        private ushort M9CurrentAmmoCount()
        {
            return (ushort)m9CurrentUpDown.Value;
        }

        private ushort M9MaxAmmoCount()
        {
            return (ushort)m9MaxUpDown.Value;
        }

        private ushort USPCurrentAmmoCount()
        {
            return (ushort)uspCurrentUpDown.Value;
        }

        private ushort USPMaxAmmoCount()
        {
            return (ushort)uspMaxUpDown.Value;
        }

        private ushort SOCOMCurrentAmmoCount()
        {
            return (ushort)socomCurrentUpDown.Value;
        }

        private ushort SOCOMMaxAmmoCount()
        {
            return (ushort)socomMaxUpDown.Value;
        }

        private ushort PSG1CurrentAmmoCount()
        {
            return (ushort)psg1CurrentUpDown.Value;
        }

        private ushort PSG1MaxAmmoCount()
        {
            return (ushort)psg1MaxUpDown.Value;
        }

        private ushort RGB6CurrentAmmoCount()
        {
            return (ushort)rgb6CurrentUpDown.Value;
        }

        private ushort RGB6MaxAmmoCount()
        {
            return (ushort)rgb6MaxUpDown.Value;
        }

        private ushort NikitaCurrentAmmoCount()
        {
            return (ushort)nikitaCurrentUpDown.Value;
        }

        private ushort NikitaMaxAmmoCount()
        {
            return (ushort)nikitaMaxUpDown.Value;
        }

        private ushort StingerCurrentAmmoCount()
        {
            return (ushort)stingerCurrentUpDown.Value;
        }

        private ushort StingerMaxAmmoCount()
        {
            return (ushort)stingerMaxUpDown.Value;
        }

        private ushort C4CurrentAmmoCount()
        {
            return (ushort)c4CurrentUpDown.Value;
        }

        private ushort C4MaxAmmoCount()
        {
            return (ushort)c4MaxUpDown.Value;
        }

        private ushort AKCurrentAmmoCount()
        {
            return (ushort)akCurrentUpDown.Value;
        }

        private ushort AKMaxAmmoCount()
        {
            return (ushort)akMaxUpDown.Value;
        }

        private ushort M4CurrentAmmoCount()
        {
            return (ushort)m4CurrentUpDown.Value;
        }

        private ushort M4MaxAmmoCount()
        {
            return (ushort)m4MaxUpDown.Value;
        }

        private ushort PSG1TCurrentAmmoCount()
        {
            return (ushort)psg1TCurrentUpDown.Value;
        }

        private ushort PSG1TMaxAmmoCount()
        {
            return (ushort)psg1TMaxUpDown.Value;
        }

        private ushort ChaffCurrentAmmoCount()
        {
            return (ushort)chaffCurrentUpDown.Value;
        }

        private ushort ChaffMaxAmmoCount()
        {
            return (ushort)chaffMaxUpDown.Value;
        }

        private ushort StunCurrentAmmoCount()
        {
            return (ushort)stunCurrentUpDown.Value;
        }

        private ushort StunMaxAmmoCount()
        {
            return (ushort)stunMaxUpDown.Value;
        }

        private ushort GrenadeCurrentAmmoCount()
        {
            return (ushort)grenadeCurrentUpDown.Value;
        }

        private ushort GrenadeMaxAmmoCount()
        {
            return (ushort)grenadeMaxUpDown.Value;
        }

        private ushort BookCurrentAmmoCount()
        {
            return (ushort)bookCurrentUpDown.Value;
        }

        private ushort BookMaxAmmoCount()
        {
            return (ushort)bookMaxUpDown.Value;
        }

        private ushort MagazineCurrentAmmoCount()
        {
            return (ushort)magazineCurrentUpDown.Value;
        }

        private ushort MagazineMaxAmmoCount()
        {
            return (ushort)magazineMaxUpDown.Value;
        }

        private ushort ClaymoreCurrentAmmoCount()
        {
            return (ushort)claymoreCurrentUpDown.Value;
        }

        private ushort ClaymoreMaxAmmoCount()
        {
            return (ushort)claymoreMaxUpDown.Value;
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
            MGS2UsableObjects.Ration.ToggleItem(rationCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void RationCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateCurrentCount(CurrentRationValue(), _logger, toolStripStatusLabel);
        }

        private void RationMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateMaxCount(MaxRationValue(), _logger, toolStripStatusLabel);
        }

        private void BandageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.ToggleItem(bandageCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BandageCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateCurrentCount(CurrentBandageValue(), _logger, toolStripStatusLabel);
        }

        private void BandageMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateMaxCount(MaxBandageValue(), _logger, toolStripStatusLabel);
        }

        private void PentazeminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.ToggleItem(pentazeminCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void PentazeminCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateCurrentCount(CurrentPentazeminCount(), _logger, toolStripStatusLabel);
        }

        private void PentazeminMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateMaxCount(MaxPentazeminCount(), _logger, toolStripStatusLabel);
        }

        private void CardBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.SetLevel(CardSecurityLevel(), _logger, toolStripStatusLabel);
        }

        private void CardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.ToggleItem(cardCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Binos1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SnakeScope.ToggleItem(scope1CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Binos2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RaidenScope.ToggleItem(scope2CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Camera1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Camera1.ToggleItem(camera1CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void DigitalCameraCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DigitalCamera.ToggleItem(digitalCameraCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void NvgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.NightVisionGoggles.ToggleItem(nvgCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ThermalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ThermalGoggles.ToggleItem(thermalCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BodyArmorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BodyArmor.ToggleItem(bodyArmorCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void MineDetectorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MineDetector.ToggleItem(mineDetectorCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ApSensorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.APSensor.ToggleItem(apSensorCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void SensorACheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorA.ToggleItem(sensorACheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void SensorBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorB.ToggleItem(sensorBCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void PhoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Phone.ToggleItem(phoneCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ColdMedsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ColdMedicine.ToggleItem(coldMedsCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void CigarettesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Cigarettes.ToggleItem(cigarettesCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void MoDiscCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MODisc.ToggleItem(moDiscCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void SocomSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SocomSuppressor.ToggleItem(socomSupCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void UspSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USPSuppressor.ToggleItem(uspSupCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void AkSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKSuppressor.ToggleItem(akSupCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Box1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box1.ToggleItem(box1CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Box2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.ToggleItem(box2CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Box3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.ToggleItem(box3CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Box4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.ToggleItem(box4CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Box5CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.ToggleItem(box5CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void WetBoxCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.ToggleItem(wetBoxCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BduCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BDU.ToggleItem(bduCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BduMaskCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //MGS2UsableObjects.BDU.SetDurability(2);
        }

        private void BandanaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandana.ToggleItem(bandanaCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void InfinityWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.InfinityWig.ToggleItem(infinityWigCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BlueWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BlueWig.ToggleItem(blueWigCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void OrangeWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.OrangeWig.ToggleItem(orangeWigCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void StealthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stealth.ToggleItem(stealthCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void DogTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DogTags.ToggleItem(dogTagsCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ShaverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Shaver.ToggleItem(shaverCheckBox.Checked, _logger, toolStripStatusLabel);
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
            MGS2UsableObjects.Box1.SetDurability((ushort)box1UpDown.Value, _logger, toolStripStatusLabel);
        }

        private void box2Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.SetDurability((ushort)box2UpDown.Value, _logger, toolStripStatusLabel);
        }

        private void box3Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.SetDurability((ushort)box3UpDown.Value, _logger, toolStripStatusLabel);
        }

        private void box4Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.SetDurability((ushort)box4UpDown.Value, _logger, toolStripStatusLabel);
        }

        private void box5Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.SetDurability((ushort)box5UpDown.Value, _logger, toolStripStatusLabel);
        }

        private void wetBoxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.SetDurability((ushort)wetBoxUpDown.Value, _logger, toolStripStatusLabel);
        }
        #endregion

        #region Weapons Button Functions
        private void AkMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateMaxAmmoCount(AKMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void M9CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateCurrentAmmoCount(M9CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void M9MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateMaxAmmoCount(M9MaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void SocomCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateCurrentAmmoCount(SOCOMCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void SocomMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateMaxAmmoCount(SOCOMMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void UspCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateCurrentAmmoCount(USPCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void UspMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateMaxAmmoCount(USPMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void ChaffCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateCurrentAmmoCount(ChaffCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void ChaffMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateMaxAmmoCount(ChaffMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void StunCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateCurrentAmmoCount(StunCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void StunMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateMaxAmmoCount(StunMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void GrenadeCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateCurrentAmmoCount(GrenadeCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void GrenadeMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateMaxAmmoCount(GrenadeMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void MagazineCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateCurrentAmmoCount(MagazineCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void MagazineMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateMaxAmmoCount(MagazineMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void AkCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateCurrentAmmoCount(AKCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void M4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateCurrentAmmoCount(M4CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void M4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateMaxAmmoCount(M4MaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Psg1CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateCurrentAmmoCount(PSG1CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Psg1MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateMaxAmmoCount(PSG1MaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Psg1TCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateCurrentAmmoCount(PSG1TCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Psg1TMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateMaxAmmoCount(PSG1TMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Rgb6CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateCurrentAmmoCount(RGB6CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void Rgb6MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateMaxAmmoCount(RGB6MaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void NikitaCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateCurrentAmmoCount(NikitaCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void NikitaMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateMaxAmmoCount(NikitaMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void StingerCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateCurrentAmmoCount(StingerCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void StingerMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateMaxAmmoCount(StingerMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void ClaymoreCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateCurrentAmmoCount(ClaymoreCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void ClaymoreMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateMaxAmmoCount(ClaymoreMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void C4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateCurrentAmmoCount(C4CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void C4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateMaxAmmoCount(C4CurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void BookCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateCurrentAmmoCount(BookCurrentAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void BookMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateMaxAmmoCount(BookMaxAmmoCount(), _logger, toolStripStatusLabel);
        }

        private void HfBladeLethalBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToLethal(_logger);
        }

        private void HfBladeStunBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToStun(_logger);
        }

        private void M9CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.ToggleWeapon(m9CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void SocomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.ToggleWeapon(socomCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void UspCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.ToggleWeapon(uspCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ChaffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.ToggleWeapon(chaffCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void StunCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.ToggleWeapon(stunCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void GrenadeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.ToggleWeapon(grenadeCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void MagazineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.ToggleWeapon(magazineCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void AkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.ToggleWeapon(akCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void M4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.ToggleWeapon(m4CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Psg1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.ToggleWeapon(psg1CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Psg1TCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.ToggleWeapon(psg1TCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Rgb6CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.ToggleWeapon(rgb6CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void NikitaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.ToggleWeapon(nikitaCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void StingerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.ToggleWeapon(stingerCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void ClaymoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.ToggleWeapon(claymoreCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void C4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.ToggleWeapon(c4CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void BookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.ToggleWeapon(bookCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void CoolantCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Coolant.ToggleWeapon(coolantCheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Dmic1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic1.ToggleWeapon(dmic1CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void Dmic2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic2.ToggleWeapon(dmic2CheckBox.Checked, _logger, toolStripStatusLabel);
        }

        private void HfBladeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.ToggleWeapon(hfBladeCheckBox.Checked, _logger, toolStripStatusLabel);
        }
        #endregion

        #region Stats Tab Functions
        internal void UpdateGameStats(MGS2MemoryManager.GameStats gameStats, Difficulty difficulty)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdateGui(gameStats, difficulty)));
            }
            else
            {
                UpdateGui(gameStats, difficulty);
            }
        }

        private void UpdateGui(MGS2MemoryManager.GameStats currentGameStats, Difficulty difficulty)
        {
            alertCountLabel.Text = currentGameStats.Alerts.ToString();
            continueCountLabel.Text = currentGameStats.Continues.ToString();
            damageTakenLabel.Text = currentGameStats.DamageTaken.ToString();
            killCountLabel.Text = currentGameStats.Kills.ToString();
            mechsDestroyedLabel.Text = currentGameStats.MechsDestroyed.ToString();
            playTimeLabel.Text = ParsePlayTime(currentGameStats.PlayTime);
            rationsUsedLabel.Text = currentGameStats.Rations.ToString();
            saveCountLabel.Text = currentGameStats.Saves.ToString();
            shotsFiredLabel.Text = currentGameStats.Shots.ToString();
            CheckOffSpecialItemsUsed(currentGameStats.SpecialItems);

            Rank projectedRank = Rank.CurrentlyProjectedRank(currentGameStats, difficulty, GameType.TankerPlant); //TODO: fix this when we actually figure out gametype
            projectedRankLabel.Text = projectedRank?.Name;
        }

        private static string ParsePlayTime(int playTime)
        {
            //yes konami. why record game time at a normal rate, when you can record it at 60...
            //ARE THEY COUNTING FRAMES??
            TimeSpan parsedPlayTime = TimeSpan.FromSeconds(playTime / 60);
            return parsedPlayTime.ToString(@"hh\:mm\:ss");
        }

        private void CheckOffSpecialItemsUsed(short specialItems)
        {
            int indexOfInfWig = specialItemsCheckedListBox.FindString("Infinity Wig");
            int indexOfBlueWig = specialItemsCheckedListBox.FindString("Blue Wig");
            int indexOfOrangeWig = specialItemsCheckedListBox.FindString("Orange Wig");
            int indexOfStealth = specialItemsCheckedListBox.FindString("Stealth");
            int indexOfBandana = specialItemsCheckedListBox.FindString("Bandana");
            int indexOfRadar = specialItemsCheckedListBox.FindString("Radar");
            specialItemsCheckedListBox.BackColor = Color.FromName("Window");
            for (int i = 0; i < specialItemsCheckedListBox.Items.Count; i++)
                specialItemsCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);

            switch (specialItems)
            {
                //I really don't want to go through all 63 combinations.
                //Storing a list of ones I've discovered so far here: 
                //https://docs.google.com/spreadsheets/d/1Zzb6t_0igEPBxIIwQcKYzol3s4Tta1DafDkt3769WOA/edit?usp=sharing
                //Please add any new ones here, and there! :)
                default:
                    //just marking the box red because we know BB run is failed
                    specialItemsCheckedListBox.BackColor = Color.DarkRed;
                    break;
                case 0x3C20:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    break;
                case 0x3820:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    break;
                case 0x2620:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    break;
                case 0x0000:
                    break;
                case 0x0120:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBandana, CheckState.Checked);
                    break;
                case 0x0220:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    break;
                case 0x1020:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x2420:
                    //blue wig used
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    break;
                case 0x1220:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x2000:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    break;
                case 0x2120:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBandana, CheckState.Checked);
                    break;
                case 0x2220:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    break;
                case 0x2820:
                    //orange wig used
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    break;
                case 0x2A20:
                    //orange and infinity wigs used
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    break;
                case 0x3020:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x3104: //orange and blue wigs
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    break;
                case 0x3220:
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x3420: //blue wig and stealth
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x3616: //orange, blue, and infinity wigs
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    break;
                case 0x3620: //blue and infinity wigs AND stealth
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x3A20:
                    //orange, and infinity wigs AND stealth
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
                case 0x3E20: //orange, blue, and infinity wigs AND stealth
                    specialItemsCheckedListBox.SetItemCheckState(indexOfRadar, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfOrangeWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfInfWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfBlueWig, CheckState.Checked);
                    specialItemsCheckedListBox.SetItemCheckState(indexOfStealth, CheckState.Checked);
                    break;
            }
        }

        private void DisableStatsTrackingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _logger.Debug($"Toggling stat tracking");
            MGS2Monitor.EnableGameStats = !disableStatsTrackingCheckBox.Checked;
        }

        private void AdjustStat(string stat, Button statButton, TextBox statTextBox, MGS2MemoryManager.GameStats.ModifiableStats statType)
        {
            try
            {
                if (statButton.Text == $"Adjust {stat}")
                {
                    MGS2Monitor.EnableGameStats = false;
                    statButton.Text = $"Save {stat} Count";
                }
                else
                {
                    MGS2MemoryManager.ChangeGameStat(statType, short.Parse(statTextBox.Text));
                    MGS2Monitor.EnableGameStats = true;
                    statButton.Text = $"Adjust {stat}";
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to adjust {stat}: {ex}");
                toolStripStatusLabel.Text = $"Failed to adjust {stat}!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void alertCountButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Alerts", sender as Button, alertCountLabel, MGS2MemoryManager.GameStats.ModifiableStats.Alerts);
        }

        private void adjustKillCountButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Kills", sender as Button, killCountLabel, MGS2MemoryManager.GameStats.ModifiableStats.Kills);
        }

        private void rationsUsedButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Rations Used", sender as Button, rationsUsedLabel, MGS2MemoryManager.GameStats.ModifiableStats.Rations);
        }

        private void continueCountButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Continues", sender as Button, continueCountLabel, MGS2MemoryManager.GameStats.ModifiableStats.Continues);
        }

        private void saveCountButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Saves", sender as Button, saveCountLabel, MGS2MemoryManager.GameStats.ModifiableStats.Saves);
        }

        private void shotsFiredButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Shots Fired", sender as Button, shotsFiredLabel, MGS2MemoryManager.GameStats.ModifiableStats.Shots);
        }

        private void damageTakenButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Damage Taken", sender as Button, damageTakenLabel, MGS2MemoryManager.GameStats.ModifiableStats.DamageTaken);
        }

        private void mechsDestroyedButton_Click(object sender, EventArgs e)
        {
            AdjustStat("Mechs Destroyed", sender as Button, mechsDestroyedLabel, MGS2MemoryManager.GameStats.ModifiableStats.MechsDestroyed);
        }
        #endregion

        private void StringsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (GuiLoaded == true)
                {
                    StaticGuiReference?.Invoke(new Action(() =>
                    {
                        MGS2Strings.MGS2String selectedString = stringsListBox.SelectedItem as MGS2Strings.MGS2String;
                        StaticGuiReference.basicNameTextBox.Text = selectedString.CurrentText;
                        StaticGuiReference.basicNameTextBox.MaxLength = selectedString.MemoryOffset.Length;
                        StaticGuiReference.characterLimitLabel.Text = selectedString.MemoryOffset.Length.ToString();
                        StaticGuiReference.basicNameGroupBox.Text = selectedString.Tag;
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to select string from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to select string. If this error persists, please restart the application.");
            }
        }

        private void setBasicNameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MGS2Strings.MGS2String selectedString = stringsListBox.SelectedItem as MGS2Strings.MGS2String;
                _logger.Verbose($"Changing {selectedString.Tag} in memory...");
                toolStripStatusLabel.Text = $"Finding {selectedString.Tag} in memory...";
                selectedString.CurrentText = MGS2MemoryManager.ReadGameString(selectedString);
                MGS2MemoryManager.UpdateGameString(selectedString, basicNameTextBox.Text);
                toolStripStatusLabel.Text = $"Set {selectedString.Tag} from {selectedString.CurrentText} to {basicNameTextBox.Text} successfully!";
                selectedString.CurrentText = basicNameTextBox.Text;
                _logger.Verbose($"Successfully changed {selectedString.Tag} in memory");
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to update game string: {ex}");
                MessageBox.Show($@"Failed to update game string");
            }
        }

        private void ItemListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           ItemListBox_Click();            
        }

        private static void ItemListBox_Click()
        {
            try
            {
                StaticGuiReference.Invoke(new Action(() =>
                    {
                        if (StaticGuiReference.itemGroupBox.Controls.Count > 0)
                            StaticGuiReference.itemGroupBox.Controls[0].Visible = false;
                        StaticGuiReference.itemGroupBox.Controls.Clear();

                        GuiObject itemObject = itemGuiObjectList.First(guiObject => guiObject.Name == (StaticGuiReference.itemListBox.SelectedItem as GuiObject).Name);

                        StaticGuiReference._logger.Verbose($"Item {itemObject.Name} selected");
                        StaticGuiReference.itemGroupBox.Controls.Add(itemObject.AssociatedControl);
                        itemObject.AssociatedControl.Visible = true;
                    }));
            }
            catch (Exception ex)
            {
                StaticGuiReference._logger.Error($"Failed to select item from itemListBox: {ex}");
                MessageBox.Show(@"Failed to select item from itemListBox. If this error persists, please restart the application.");
            }
        }

        private void weaponListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WeaponListBox_Click();
        }

        private static void WeaponListBox_Click()
        {
            try
            {
                StaticGuiReference.Invoke(new Action(() =>
                {
                    if (StaticGuiReference.weaponGroupBox.Controls.Count > 0)
                        StaticGuiReference.weaponGroupBox.Controls[0].Visible = false;
                    StaticGuiReference.weaponGroupBox.Controls.Clear();

                    GuiObject weaponObject = weaponGuiObjectList.First(guiObject => guiObject.Name == (StaticGuiReference.weaponListBox.SelectedItem as GuiObject).Name);

                    StaticGuiReference._logger.Verbose($"Weapon {weaponObject.Name} selected");
                    StaticGuiReference.weaponGroupBox.Controls.Add(weaponObject.AssociatedControl);
                    weaponObject.AssociatedControl.Visible = true;
                }));
            }
            catch (Exception ex)
            {
                StaticGuiReference._logger.Error($"Failed to select weapon from weaponListBox: {ex}");
                MessageBox.Show(@"Failed to select weapon from weaponListBox. If this error persists, please restart the application.");
            }
        }

        private void Mgs2TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _logger.Verbose($"Switching to tab #{mgs2TabControl.SelectedIndex}...");
                CurrentTab = mgs2TabControl.SelectedIndex;
                #if DEBUG
                UserHasBeenWarned = true;
                #endif
                if (CurrentTab == mgs2TabControl.TabPages.IndexOfKey("tabPageCheats"))
                {
                    if (!UserHasBeenWarned)
                    {
                        MessageBox.Show("WARNING! Use the contents of this tab at your own risk. USE OF THESE CHEATS MAY CRASH YOUR GAME! All of these have worked at some point or another, but may not always. Results not guaranteed.");
                        //MessageBox.Show("As of MGS2 version 2.0, the cheats tab is very hit-or-miss. We're working on a fix, please be patient!", "Warning");
                        UserHasBeenWarned = true;
                    }
                    try
                    {
                        if (MGS2Monitor.EnableGameStats)
                        {
                            Constants.PlayableCharacter currentCharacter = MGS2MemoryManager.DetermineActiveCharacter();
                            if(currentCharacter == Constants.PlayableCharacter.Snake)
                            {
                                gripTrackBar.Maximum = 1800; //1800//3600//5400
                            }
                            else
                            {
                                gripTrackBar.Maximum = 3600;
                            }
                            playerMaxHpUpDown.Value = MGS2MemoryManager.GetCurrentMaxHP();
                            playerCurrentHpTrackBar.Maximum = (int)playerMaxHpUpDown.Value;
                            playerCurrentHpTrackBar.Value = MGS2MemoryManager.GetCurrentHP();
                            gripTrackBar.Value = MGS2MemoryManager.GetCurrentGripGauge();
                            CheatsTabTask = Task.Factory.StartNew(LiveUpdateHp);
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.Error($"Something went wrong with the live HP stats: {ex}");
                        //playerHealthGroupBox.Enabled = false;
                    }
                }
                else
                { 
                    CheatsTabTask?.Dispose();
                }

                if(CurrentTab != mgs2TabControl.TabPages.IndexOfKey("tabPageBosses"))
                {
                    foreach(Task bossTask in BossTasks)
                    {
                        bossTask.Dispose();
                    }
                    BossTasks.Clear();
                }
                /* Turns out we don't have any cheats that require administrator, but I'm leaving this reference here in case we do.
                if(!Program.IsRunAsAdministrator())
                {
                    Program.RestartInAdminMode();
                }
                */
                CurrentlySelectedObject = null;
            }
            catch(Exception ex) 
            {
                _logger.Error($"Failed to change tabs from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to change tabs. If this error persists, please restart the application.");
            }
        }

        private void LiveUpdateHp()
        {
            try
            {
                //only keep this task alive while the cheats tab is open to save system resources
                while (CurrentTab == mgs2TabControl.TabPages.IndexOfKey("tabPageCheats"))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            playerCurrentHpTrackBar.Maximum = MGS2MemoryManager.GetCurrentMaxHP();
                            playerCurrentHpTrackBar.Value = MGS2MemoryManager.GetCurrentHP();
                            ushort currentGripStamina = MGS2MemoryManager.GetCurrentGripGauge();
                            if (currentGripStamina > gripTrackBar.Maximum)
                                gripTrackBar.Maximum = currentGripStamina;
                            gripTrackBar.Value = currentGripStamina;
                        }));
                    }
                    else
                    {
                        playerCurrentHpTrackBar.Maximum = MGS2MemoryManager.GetCurrentMaxHP();
                        playerCurrentHpTrackBar.Value = MGS2MemoryManager.GetCurrentHP();
                        ushort currentGripStamina = MGS2MemoryManager.GetCurrentGripGauge();
                        if (currentGripStamina > gripTrackBar.Maximum)
                            gripTrackBar.Maximum = currentGripStamina;
                        gripTrackBar.Value = currentGripStamina;
                    }
                    Thread.Sleep(333);
                }
            }
            catch(Exception e)
            {
                _logger.Error($"Something went wrong when getting current HP or grip stamina. Going to wait 5 seconds before retrying.\n\nError information: {e}");
                Thread.Sleep(5000);
                LiveUpdateHp();
            }
        }

        private void GithubMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Verbose("Opening github link in web-browser");
                Process.Start("https://github.com/sagefantasma/MGS2-Cheat-Trainer");
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to launch Github page from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to launch Github page. If this error persists, please restart the application.");
            }
        }

        private void ViewLogsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Logging.LogLocation);
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to open logs from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to open log location. If this error persists, please restart the application.");
            }
        }

        private void ModifyConfigMenuItem_Click(object sender, EventArgs e)
        {
            _logger.Verbose("Opening modify config form...");
            ConfigEditorForm configEditorForm = new ConfigEditorForm(_logger);
            
            if(configEditorForm.ShowDialog() == DialogResult.OK)
            {
                _logger.Verbose("Finished modifying config, loading new config");
                try
                {
                    MGS2Monitor.LoadConfig();
                    _logger.Verbose($"Config loaded");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to load config: {ex}");
                    toolStripStatusLabel.Text = $"Failed to load config!";
                    MessageBox.Show(toolStripStatusLabel.Text);
                }
            }            
        }

        private void LaunchMgs2MenuItem_Click(object sender, EventArgs e)
        {
            if (MGS2Monitor.MGS2Process != null)
            {
                MessageBox.Show(@"MGS2 is already running, please exit MGS2 before attempting to launch it again.");
            }
            else if(!Program.MGS2Thread.IsAlive)
            {
                try
                {
                    _logger.Verbose("MGS2 thread is dead, restarting MGS2 thread...");
                    Program.RestartMonitoringThread();
                    _logger.Verbose("MGS2 thread restarted successfully!");
                }
                catch(Exception ex)
                {
                    _logger.Error($"Failed to start MGS2 thread: {ex}");
                }
            }
            else
            {
                _logger.Verbose("MGS2 thread isn't dead, but we don't see the MGS2 process. Killing MGS2 thread and restarting");
                Program.MGS2Thread.Abort(); 
                Program.RestartMonitoringThread();
                _logger.Verbose("MGS2 thread killed and restarted successfully!");                
            }
        }

        public static void EnableLaunchMGS2Option(bool enable)
        {
            try
            {
                StaticGuiReference.Invoke(new Action(() =>
                {
                    StaticGuiReference._logger.Verbose($"Setting LaunchMgs2MenuItem.Enabled to {enable}");
                    StaticGuiReference.launchMgs2MenuItem.Enabled = enable;
                }));
            }
            catch(Exception e)
            {
                StaticGuiReference._logger.Error($"Failed to set Launch MGS2 Option to {enable}: {e}");
                //this isn't a user-facing error, so dont bother them with a message box here
            }
        }

        private void JoinOurDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Verbose("Opening discord link in web-browser");
                Process.Start("https://discord.gg/XUh58VfqDu");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to launch Discord page from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to launch Discord page. If this error persists, please restart the application.");
            }
        }

        private void CheatsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox cheatsBox = sender as CheckedListBox;

            try
            {
                if (cheatsBox?.SelectedItem != null)
                {
                    Cheat selectedCheat = (Cheat)cheatsBox.SelectedItem;
                    _logger.Debug($"Trying to toggle cheat: {selectedCheat.Name}");

                    if (!cheatsBox.CheckedItems.Contains(cheatsBox.SelectedItem))
                    {
                        //cheat is enabled already, so now disable it
                        toolStripStatusLabel.Text = $"Attempting to disable {selectedCheat.Name}, this may take a long time...";
                        Application.DoEvents();
                        selectedCheat.CheatAction(false);
                        toolStripStatusLabel.Text = $"Finished trying to disable {selectedCheat.Name}. Did it work?!?";
                    }
                    else
                    {
                        //cheat is not yet enabled, so enable it
                        toolStripStatusLabel.Text = $"Attempting to enable {selectedCheat.Name}, this may take a long time...";
                        Application.DoEvents();
                        selectedCheat.CheatAction(true);
                        toolStripStatusLabel.Text = $"Finished trying to enable {selectedCheat.Name}. Did it work?!?";
                    }
                    _logger.Debug($"Finished trying to toggle cheat: {selectedCheat.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to toggle selected cheat: {ex}");
                toolStripStatusLabel.Text = $"Failed to toggle selected cheat!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void LowerGripButton_Click(object sender, EventArgs e)
        {
            _logger.Information("User clicked -100 pull-ups button");
            toolStripStatusLabel.Text = $"Attempting to reduce pull-up count by 100 pull-ups...";
            try
            {
                ushort currentPushups = MGS2MemoryManager.ModifyGripLevel(false);
                toolStripStatusLabel.Text = $"Pushup count set to {currentPushups} for this character. This may not actually change anything.";
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to remove 100 pull-ups from the pull-up count: {ex}");
                toolStripStatusLabel.Text = $"Failed to decrease pull-up count for the current character!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void RaiseGripButton_Click(object sender, EventArgs e)
        {
            _logger.Information("User clicked +100 pull-ups button");
            toolStripStatusLabel.Text = $"Attempting to increase pull-up count by 100 pull-ups...";
            try
            {
                ushort currentPushups = MGS2MemoryManager.ModifyGripLevel(true);
                toolStripStatusLabel.Text = $"Pushup count set to {currentPushups} for this character. Perform one manual pull-up to set grip level.";
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to add 100 pull-ups to the pull-up count: {ex}");
                toolStripStatusLabel.Text = $"Failed to increase pull-up count for the current character!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void PlayerCurrentHpTrackBar_Scroll(object sender, EventArgs e)
        {
            //lock the trackbar so modifying the hp feels more natural(by preventing auto-updates)
            try
            {
                lock (playerCurrentHpTrackBar)
                {
                    //modify hp
                    _logger.Verbose($"Setting current HP to: {playerCurrentHpTrackBar.Value}");
                    MGS2MemoryManager.ModifyCurrentHp((ushort)playerCurrentHpTrackBar.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to modify player current HP: {ex}");
                toolStripStatusLabel.Text = $"Failed to modify character's current HP!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void GripTrackBar_Scroll(object sender, EventArgs e)
        {
            //lock the trackbar so modifying the grip feels more natural(by preventing auto-updates)
            try
            {
                lock (gripTrackBar)
                {
                    //modify grip
                    //can use current grip stamina
                    _logger.Verbose($"Setting current grip stamina to: {gripTrackBar.Value}");
                    MGS2MemoryManager.ModifyCurrentGripGauge((ushort)gripTrackBar.Value);
                }
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to modify current grip stamina: {ex}");
                toolStripStatusLabel.Text = $"Failed to modify current grip stamina!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void PlayerMaxHpUpDown_ValueChanged(object sender, EventArgs e)
        {
            //TODO: implement
        }

        private void forceSleepButton_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Information("User clicked on 'force guards to sleep' button");
                toolStripStatusLabel.Text = $"Attempting to force all guards to sleep...";
                //force undo of wake(if done)
                byte[] currentWake = Cheat.CheatActions.ReadMemory(MGS2AoB.ForceGuardsToWake, MGS2Offset.FORCE_WAKE);

                if (currentWake != null && currentWake.SequenceEqual(MGS2AoB.ForceGuardsToWakeBytes))
                {
                    _logger.Information("Guards are currently forced awake, attempting to disable that");
                    Cheat.CheatActions.ReplaceWithSpecificCode(MGS2AoB.ForceGuardsToWake, MGS2AoB.StandardGuardWakeBytes, MGS2Offset.FORCE_WAKE);
                    _logger.Information("Guards are no longer forced awake");
                }

                Cheat.CheatActions.ReplaceWithSpecificCode(MGS2AoB.StandardGuardSleep, MGS2AoB.ForceGuardsToSleepBytes, MGS2Offset.FORCE_SLEEP);

                toolStripStatusLabel.Text = $"All guards suddenly feel asleep!";
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to force guard sleep: {ex}");
                toolStripStatusLabel.Text = $"Failed to force all guards to sleep!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void forceWakeButton_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Information("User clicked on 'force guards to wake' button");
                toolStripStatusLabel.Text = $"Attempting to force all guards to wake...";
                //force undo of sleep(if done)
                byte[] currentSleep = Cheat.CheatActions.ReadMemory(MGS2AoB.ForceGuardsToSleep, MGS2Offset.FORCE_SLEEP);


                if (currentSleep != null && currentSleep.SequenceEqual(MGS2AoB.ForceGuardsToSleepBytes))
                {
                    _logger.Information("Guards are currently forced asleep, attempting to disable that");
                    Cheat.CheatActions.ReplaceWithSpecificCode(MGS2AoB.ForceGuardsToSleep, MGS2AoB.StandardGuardSleepBytes, MGS2Offset.FORCE_SLEEP);
                    _logger.Information("Guards are no longer forced asleep");
                }

                Cheat.CheatActions.ReplaceWithSpecificCode(MGS2AoB.StandardGuardWake, MGS2AoB.ForceGuardsToWakeBytes, MGS2Offset.FORCE_WAKE);

                toolStripStatusLabel.Text = $"All guards have awoken!";
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to force guard wake: {ex}");
                toolStripStatusLabel.Text = $"Failed to force all guards to wake!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void startAnimationButton_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Information($"User clicked on 'Start animation' button with {guardAnimationComboBox.SelectedText} animation selected");
                toolStripStatusLabel.Text = $"Attempting to set all guards animation to :{guardAnimationComboBox.SelectedText}";

                Cheat.CheatActions.ReplaceWithSpecificCode(MGS2AoB.GuardAnimations, (guardAnimationComboBox.SelectedItem as MGS2AoB.GuardAnimation).Bytes, MGS2Offset.GUARD_ANIMATIONS);

                toolStripStatusLabel.Text = $"All guards' animations have been set to {guardAnimationComboBox.SelectedText}~!";
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to start guard animation: {ex}");
                toolStripStatusLabel.Text = $"Failed to force guard animation!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private async void filterColorButton_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog dialog = new ColorDialog();
                dialog.AnyColor = false;
                dialog.FullOpen = true;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    filterColorPictureBox.BackColor = dialog.Color;
                    if (enableCustomFilterColorCheckBox.Checked == false)
                    {
                        Cheat.CheatActions.EnableCustomFilter(true);
                        enableCustomFilterColorCheckBox.Checked = true;
                    }
                    await Cheat.CheatActions.ApplyColorFilter(filterColorPictureBox.BackColor);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to set custom filter: {ex}");
                toolStripStatusLabel.Text = $"Failed to set custom filter!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void enableCustomFilterColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cheat.CheatActions.EnableCustomFilter(enableCustomFilterColorCheckBox.Checked);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to enable custom filter: {ex}");
                toolStripStatusLabel.Text = $"Failed to enable custom filter!";
                MessageBox.Show(toolStripStatusLabel.Text);
            }
        }

        private void bossTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Text == "Harrier" || e.Node.Text == "Solidus")
            {
                MessageBox.Show("Sorry, modifying this boss' vitals with the trainer currently crashes the game. We're trying to find a " +
                    "solution to this issue and hope to enable modification of this boss' vitals in a future update! Thank you " +
                    "for your understanding :)", $"{e.Node.Text} Vitals Modification Not Supported");
                return;
            }
            if (e.Node.Text != "RAY Battle")
            {
                try
                {
                    bossHealthStaminaLayoutPanel.Visible = true;
                    CurrentBossNode = e.Node;
                    bossGroupBox.Text = e.Node.Text;

                    BossTasks.Add(Task.Factory.StartNew(() => LiveUpdateBossVitals(BossParser.ParseNode(e.Node.Text))));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to start live boss vitals tracking: {ex}");
                    toolStripStatusLabel.Text = $"Failed to start live boss vitals tracking!";
                    MessageBox.Show(toolStripStatusLabel.Text);
                }
            }
        }

        private void UpdateBossVitals(Constants.Boss boss)
        {
            try
            {
                BossVitals vitals = MGS2MemoryManager.GetBossVitals(boss);


                if (bossHpTrackbar.Maximum < vitals.Health)
                    bossHpTrackbar.Maximum = vitals.Health;
                bossHpTrackbar.Value = vitals.Health;

                if (vitals.HasStamina)
                {
                    bossStaminaGroupBox.Enabled = true;
                    if (bossStaminaTrackbar.Maximum < vitals.Stamina)
                        bossStaminaTrackbar.Maximum = vitals.Stamina;
                    bossStaminaTrackbar.Value = vitals.Stamina;
                }
                else
                {
                    bossStaminaGroupBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update boss vitals: {ex}");
            }
        }

        private void LiveUpdateBossVitals(Constants.Boss boss)
        {
            try
            {
                //only keep this task alive while the boss tab is open to save system resources
                while (true)
                {
                    if (BossParser.ParseNode(CurrentBossNode.Text) != boss)
                        return;
                        
                    if (CurrentTab == mgs2TabControl.TabPages.IndexOfKey("tabPageBosses"))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                UpdateBossVitals(boss);
                            }));
                        }
                        else
                        {
                            UpdateBossVitals(boss);
                        }
                    }
                    Thread.Sleep(333);
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Something went wrong when getting boss vitals. Going to wait 5 seconds before retrying.\n\nError information: {e}");
                Thread.Sleep(5000);
                LiveUpdateBossVitals(boss);
            }
        }

        private void bossHpTrackbar_Scroll(object sender, EventArgs e)
        {
            //lock the trackbar so modifying the hp feels more natural(by preventing auto-updates)
            if (bossTreeView.SelectedNode != null)
            {
                try
                {
                    lock (bossHpTrackbar)
                    {
                        BossVitals bossVitals = BossVitals.ParseBossVitals(BossParser.ParseNode(bossTreeView.SelectedNode.Text));
                        bossVitals.Health = bossHpTrackbar.Value;
                        MGS2MemoryManager.SetBossVitals(bossVitals);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to modify boss HP: {ex}");
                    toolStripStatusLabel.Text = $"Failed to modify boss' HP!";
                    MessageBox.Show(toolStripStatusLabel.Text);
                }
            }
        }

        private void bossStaminaTrackbar_Scroll(object sender, EventArgs e)
        {
            //lock the trackbar so modifying the stamina feels more natural(by preventing auto-updates)
            if (bossTreeView.SelectedNode != null)
            {
                try
                {
                    lock (bossStaminaTrackbar)
                    {
                        BossVitals bossVitals = BossVitals.ParseBossVitals(BossParser.ParseNode(bossTreeView.SelectedNode.Text));
                        bossVitals.Stamina = bossStaminaTrackbar.Value;
                        MGS2MemoryManager.SetBossVitals(bossVitals);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to modify boss stamina: {ex}");
                    toolStripStatusLabel.Text = $"Failed to modify boss stamina!";
                    MessageBox.Show(toolStripStatusLabel.Text);
                }
            }
        }

        private void randomizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logger.Verbose("Opening randomization tool...");

            DialogResult deprecatedNotice = MessageBox.Show("This version of the MGS2 Randomizer is officially deprecated. If you'd like to enjoy the latest version of the randomizer, please download the standalone mod! Would you like to go to Nexus Mods now to download it?", "Thanks for playing!", MessageBoxButtons.YesNo);
            if (deprecatedNotice == DialogResult.Yes)
            {
                Process.Start("https://www.nexusmods.com/metalgearsolid2mc/mods/92");
                return;
            }
            
            MGS2RandomizationTool randomizationTool = new MGS2RandomizationTool();
            try
            {
                if (randomizationTool.ShowDialog() == DialogResult.OK)
                {
                    _logger.Verbose("Finished with randomization tool");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Something went wrong with the randomizer!: {ex.Message}");
                _logger.Error($"RANDOMIZER UNHANDLED ERROR: {ex.Message}");
            }
        }

        private void openInstallLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new FileInfo(Application.ExecutablePath).Directory.FullName);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to open install location from sender {JsonSerializer.Serialize(sender)} and args {JsonSerializer.Serialize(e)}: {ex}");
                MessageBox.Show(@"Failed to open install location. If this error persists, please report the bug to our Discord.");
            }
        }
    }
}
