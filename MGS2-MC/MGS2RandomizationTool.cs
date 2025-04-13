using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC
{
    public partial class MGS2RandomizationTool : Form
    {
        private string _installLocation { get; set; }
        public MGS2RandomizationTool()
        {
            InitializeComponent();
            FileInfo fileInfo = new FileInfo(MGS2Monitor.TrainerConfig.Mgs2ExePath);
            _installLocation = fileInfo.Directory.FullName;
            this.helpProvider1.SetShowHelp(this.seedAlwaysBeatableCheckbox, true);
            this.helpProvider1.SetHelpString(this.seedAlwaysBeatableCheckbox, "This option will make sure progressive weapons/items never spawn in an area you do not have access to.");

            this.helpProvider1.SetShowHelp(this.restrictNikitaCheckbox, true);
            this.helpProvider1.SetHelpString(this.restrictNikitaCheckbox, "This option will make sure the Nikita always spawns somewhere in Shell 2 before the Purification Chamber, so you don't get soft-locked if you missed it and 'Seed Always Beatable' is enabled.");

            this.helpProvider1.SetShowHelp(this.allWeaponsWillSpawnCheckbox, true);
            this.helpProvider1.SetHelpString(this.allWeaponsWillSpawnCheckbox, "This option will make sure weapons will not spawn in optional spawns, so you will have access to all of them throughout the game.");

            this.helpProvider1.SetShowHelp(this.randomizeRationsCheckbox, true);
            this.helpProvider1.SetHelpString(this.randomizeRationsCheckbox, "This will add Rations to the randomization pool.");

            this.helpProvider1.SetShowHelp(this.randomizeStartingItemsCheckbox, true);
            this.helpProvider1.SetHelpString(this.randomizeStartingItemsCheckbox, "You will no longer be guaranteed M9, Camera, AP Sensor and Cigs on Tanker; nor AP Sensor and Binoculars on Plant.");

            this.helpProvider1.SetShowHelp(this.randomizeAutomaticRewardsCheckbox, true);
            this.helpProvider1.SetHelpString(this.randomizeAutomaticRewardsCheckbox, "Automatic rewards will be randomized into the pool. This includes: USP on Tanker; SOCOM, Coolant, Sensor A, Card Keys, BDU, Phone, and MO Disc on Plant.");

            this.helpProvider1.SetShowHelp(this.addCardsCheckbox, true);
            this.helpProvider1.SetHelpString(this.addCardsCheckbox, "If automatic rewards are enabled, you can enable this option to add cards to the randomization pool.");

            this.helpProvider1.SetShowHelp(this.randomizeBombLocations, true);
            this.helpProvider1.SetHelpString(this.randomizeBombLocations, "Randomize where all bombs during the bomb defusal segment spawn.");

            this.helpProvider1.SetShowHelp(this.randomizeEFConnectingBridgeClaymores, true);
            this.helpProvider1.SetHelpString(this.randomizeEFConnectingBridgeClaymores, "Randomize where the claymores spawn on the EF Connecting Bridge.");

            this.helpProvider1.SetShowHelp(this.randomizeTankerControlUnitLocations, true);
            this.helpProvider1.SetHelpString(this.randomizeTankerControlUnitLocations, "Randomize where control units spawn in the engine room on the Tanker.");

            this.helpProvider1.SetShowHelp(this.restoreBaseGameButton, true);
            this.helpProvider1.SetHelpString(this.restoreBaseGameButton, "Restores the game's files to their 'vanilla' state. If this does not work properly, use Steam to 'Verify integrity of game files' to accomplish the same result.");
        }

        private void EnableCustomSeed_CheckedChanged(object sender, EventArgs e)
        {
            //Ideally, going forward, the seed will automagically set your settings for you, but for now, just warn the user
            /*ToggleControls(!customSeedCheckbox.Checked);
            randomizeButton.Enabled = true;
            restoreBaseGameButton.Enabled = true;
            customSeedCheckbox.Enabled = true;*/
            if (customSeedCheckbox.Checked)
                MessageBox.Show("Heads up: be sure to check your settings to get accurate seed results. If you have different settings than the seed's original settings, you will get different output.");
            seedUpDown.Enabled = customSeedCheckbox.Checked;
        }

        private void ToggleControls(bool enable)
        {
            randomizeButton.Enabled = enable;
            restoreBaseGameButton.Enabled = enable;
            seedAlwaysBeatableCheckbox.Enabled = enable;
            restrictNikitaCheckbox.Enabled = enable;
            allWeaponsWillSpawnCheckbox.Enabled = enable;
            randomizeRationsCheckbox.Enabled = enable;
            randomizeStartingItemsCheckbox.Enabled = enable;
            randomizeAutomaticRewardsCheckbox.Enabled = enable;
            randomizeBombLocations.Enabled = enable;
            randomizeEFConnectingBridgeClaymores.Enabled = enable;
            randomizeTankerControlUnitLocations.Enabled = enable;
            if (!enable && randomizeAutomaticRewardsCheckbox.Checked)
            {
                addCardsCheckbox.Enabled = enable;
            }
            if(!enable && addCardsCheckbox.Checked)
            {
                keepVanillaCardLevelsCheckbox.Enabled = enable;
            }
            if (!enable && customSeedCheckbox.Checked)
            {
                seedUpDown.Enabled = enable;
            }
            else
                seedUpDown.Value = 0;
            customSeedCheckbox.Enabled = enable;
        }

        private void restoreBaseGameButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Restoring MGS2's base game files, this will take but a moment...");
            ToggleControls(false);
            MGS2Randomizer randomizer = new MGS2Randomizer(_installLocation);
            randomizer.Derandomize();
            ToggleControls(true);
            MessageBox.Show("MGS2's base game files are restored! Enjoy vanilla MGS2!");
        }

        private async void randomizeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Randomizing MGS2's game files to your specifications, this may take some time...", "Heads up!");
            ToggleControls(false);
            Application.DoEvents();
            await Task.Run(() =>
            {
                MGS2Randomizer randomizer = new MGS2Randomizer(_installLocation, (int) seedUpDown.Value);
                MGS2Randomizer.RandomizationOptions randomizationOptions = new MGS2Randomizer.RandomizationOptions
                {
                    NoHardLogicLocks = seedAlwaysBeatableCheckbox.Checked,
                    NikitaShell2 = restrictNikitaCheckbox.Checked,
                    AllWeaponsSpawnable = allWeaponsWillSpawnCheckbox.Checked,
                    IncludeRations = randomizeRationsCheckbox.Checked,
                    RandomizeStartingItems = randomizeStartingItemsCheckbox.Checked,
                    RandomizeAutomaticRewards = randomizeAutomaticRewardsCheckbox.Checked,
                    RandomizeC4 = randomizeBombLocations.Checked,
                    RandomizeClaymores = randomizeEFConnectingBridgeClaymores.Checked,
                    RandomizeCards = addCardsCheckbox.Checked,
                    KeepVanillaCardAccess = keepVanillaCardLevelsCheckbox.Checked,
                    RandomizeTankerControlUnits = randomizeTankerControlUnitLocations.Checked
                };
                int seed = 0;
                if(randomizer.Seed == 0)
                    randomizer.Randomizer = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond);
                while (seed == 0)
                {
                    try
                    {
                        seed = randomizer.RandomizeItemSpawns(randomizationOptions);
                        randomizer.SaveRandomizationToDisk(true, false);
                    }
                    catch (OutOfMemoryException oome)
                    {
                        throw oome; //rethrow to help debug
                    }
                    catch (MGS2Randomizer.RandomizerException ee)
                    {
                        //randomizer.Seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond);
                        //randomizer.Randomizer = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond);
                        randomizer.Seed = randomizer.Randomizer.Next();
                        randomizer.Randomizer = new Random(randomizer.Seed);
                    }
                    catch (Exception ee)
                    {
                        throw ee; //rethrow to help debug
                    }
                }
            });
            MessageBox.Show("Finished! Spoiler file available in your Documents folder.", "Randomization Complete!");
            ToggleControls(true);
        }

        private void restrictNikitaCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!restrictNikitaCheckbox.Checked)
            {
                MessageBox.Show("This can cause logic-locks if the Nikita spawns on Shell 1 and you do not pick it up before fighting the Harrier.", "WARNING");
            }
        }

        private void randomizeAutomaticRewardsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            addCardsCheckbox.Enabled = randomizeAutomaticRewardsCheckbox.Checked;
            if (!randomizeAutomaticRewardsCheckbox.Checked)
            {
                addCardsCheckbox.Checked = false;
            }
        }

        private void addCardsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            keepVanillaCardLevelsCheckbox.Enabled = addCardsCheckbox.Checked;
            if (!addCardsCheckbox.Checked)
            {
                keepVanillaCardLevelsCheckbox.Checked = false;
            }
        }
    }
}
