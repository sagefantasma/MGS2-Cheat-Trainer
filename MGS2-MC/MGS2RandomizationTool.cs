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

            this.helpProvider1.SetShowHelp(this.randomizeBombLocations, true);
            this.helpProvider1.SetHelpString(this.randomizeBombLocations, "Randomize where all bombs during the bomb defusal segment spawn.");

            this.helpProvider1.SetShowHelp(this.randomizeEFConnectingBridgeClaymores, true);
            this.helpProvider1.SetHelpString(this.randomizeEFConnectingBridgeClaymores, "Randomize where the claymores spawn on the EF Connecting Bridge.");

            this.helpProvider1.SetShowHelp(this.restoreBaseGameButton, true);
            this.helpProvider1.SetHelpString(this.restoreBaseGameButton, "Restores the game's files to their 'vanilla' state. If this does not work properly, use Steam to 'Verify integrity of game files' to accomplish the same result.");
        }

        private void EnableCustomSeed_CheckedChanged(object sender, EventArgs e)
        {
            seedUpDown.Enabled = customSeedCheckbox.Checked;
        }

        private void restoreBaseGameButton_Click(object sender, EventArgs e)
        {
            MGS2Randomizer randomizer = new MGS2Randomizer(_installLocation);
            randomizer.DerandomizeItemSpawns();
            randomizer.SaveRandomizationToDisk(false);
        }

        private async void randomizeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Randomizing MGS2's game files to your specifications, this may take some time...", "Heads up!");
            randomizeButton.Enabled = false;
            Application.DoEvents();
            await Task.Run(() =>
            {
                MGS2Randomizer randomizer = new MGS2Randomizer(_installLocation, seedUpDown.Enabled ? (int)seedUpDown.Value : 0);
                MGS2Randomizer.RandomizationOptions randomizationOptions = new MGS2Randomizer.RandomizationOptions
                {
                    NoHardLogicLocks = seedAlwaysBeatableCheckbox.Checked,
                    NoSoftLogicLocks = restrictNikitaCheckbox.Checked
                };
                int seed = 0;
                while (seed == 0)
                {
                    try
                    {
                        seed = randomizer.RandomizeItemSpawns(randomizationOptions);
                        randomizer.SaveRandomizationToDisk(true, false);
                    }
                    catch (OutOfMemoryException oome)
                    {

                    }
                    catch (MGS2Randomizer.RandomizerException ee)
                    {
                        randomizer.Seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond).Next();
                        randomizer.Randomizer = new Random(randomizer.Seed);
                    }
                    catch (Exception ee)
                    {

                    }
                }
            });
            MessageBox.Show("Finished! Spoiler file available in your Documents folder.", "Randomization Complete!");
            randomizeButton.Enabled = true;
        }
    }
}
