using Serilog;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_MC
{
    public partial class ConfigEditorForm : Form
    {
        readonly ToolTip hoverToolTip = new ToolTip();
        private readonly ILogger _logger;

        public ConfigEditorForm(ILogger logger)
        {
            _logger = logger;
            InitializeComponent();
            _logger.Verbose("Config editor opened...");
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            string executableLocation = mgs2ExeTextBox.Text;

            if (string.IsNullOrWhiteSpace(executableLocation) || !File.Exists(executableLocation))
            {
                executableLocation = Environment.CurrentDirectory;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Where is 'METAL GEAR SOLID2.exe' on your machine?",
                DefaultExt = ".exe",
                InitialDirectory = Path.GetDirectoryName(executableLocation)
            };
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mgs2ExeTextBox.Text = openFileDialog.FileName;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                TrainerConfig newConfig = new TrainerConfig
                {
                    AutoLaunchGame = launchMgs2CheckBox.Checked,
                    CloseGameWithTrainer = closeMgs2CheckBox.Checked,
                    CloseTrainerWithGame = closeTrainerCheckBox.Checked,
                    Mgs2ExePath = mgs2ExeTextBox.Text
                };
                File.WriteAllText(TrainerConfigFileLocation, JsonSerializer.Serialize(newConfig));
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save config: {ex}");
            }
        }

        private void ConfigEditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                TrainerConfig currentConfig = JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText(TrainerConfigFileLocation));
                launchMgs2CheckBox.Checked = currentConfig.AutoLaunchGame;
                closeMgs2CheckBox.Checked = currentConfig.CloseGameWithTrainer;
                closeTrainerCheckBox.Checked = currentConfig.CloseTrainerWithGame;
                mgs2ExeTextBox.Text = currentConfig.Mgs2ExePath;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load config: {ex}");
            }
        }

        private void LaunchMgs2CheckBox_MouseHover(object sender, EventArgs e)
        {
            hoverToolTip.Show("If checked, when you open the Cheat Trainer, MGS2.exe will automatically be launched.", launchMgs2CheckBox);
        }

        private void CloseMgs2CheckBox_MouseHover(object sender, EventArgs e)
        {
            hoverToolTip.Show("If checked, when you close this Cheat Trainer, MGS2 will automatically be closed.", closeMgs2CheckBox);
        }

        private void CloseTrainerCheckBox_MouseHover(object sender, EventArgs e)
        {
            hoverToolTip.Show("If checked, when you close MGS2, this Cheat Trainer will automatically be closed.", closeTrainerCheckBox);
        }

        private void Mgs2ExeTextBox_MouseHover(object sender, EventArgs e)
        {
            hoverToolTip.Show(mgs2ExeTextBox.Text, mgs2ExeTextBox);
        }

        private void ConfigEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Verbose($"Config editor closing. Cancelling?: {e.Cancel} -- CloseReason: {e.CloseReason}");
        }
    }
}
