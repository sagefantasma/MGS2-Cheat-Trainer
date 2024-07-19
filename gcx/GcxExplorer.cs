using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gcx
{
    public partial class GcxExplorer : Form
    {
        private List<DecodedProc> contentTree = new List<DecodedProc>();
        private List<DecodedProc> contentTreeCarbonCopy = new List<DecodedProc>();
        private Dictionary<string, string> hexFunctionChanges = new Dictionary<string, string>();
        private GcxEditor gcx_Editor;
        readonly Regex hexRegex = new Regex("^[0-9A-F]+$");
        private Dictionary<RawProc, Color> quickIdProcs = new Dictionary<RawProc, Color>();

        public GcxExplorer()
        {
            InitializeComponent();
            BuildQuickIdProcs();
        }

        private void BuildQuickIdProcs()
        {
            quickIdProcs.Add(KnownProc.AwardAksAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardAksGun, Color.Orange); //progression blocker
            quickIdProcs.Add(KnownProc.AwardAksSuppressor, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBandages, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBodyArmor, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardBook, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardBox1, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBox2, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBox3, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBox4, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardBox5, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardC4, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardChaffG, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardClaymore, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardColdMeds, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardColdMeds2, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardDigitalCamera, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardDirectionalMic, Color.Orange); //progression blocker
            quickIdProcs.Add(KnownProc.AwardGrenade, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardM4Ammo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardM4Gun, Color.Orange);
            quickIdProcs.Add(KnownProc.AwardM9Ammo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardM9Gun, Color.Orange);
            quickIdProcs.Add(KnownProc.AwardMineDetector, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardNikitaAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardNikitaGun, Color.Orange); //progression blocker
            quickIdProcs.Add(KnownProc.AwardNvg, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardPentazemin, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardPsg1Ammo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardPsg1Gun, Color.Orange); //progression blocker
            quickIdProcs.Add(KnownProc.AwardPsg1tAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardPsg1tGun, Color.Orange);
            quickIdProcs.Add(KnownProc.AwardRation, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardRgbAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardRgbGun, Color.Orange);
            quickIdProcs.Add(KnownProc.AwardSensorB, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardShaver, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardSocomAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardSocomSuppressor, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardStingerAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardStingerGun, Color.Orange);
            quickIdProcs.Add(KnownProc.AwardStunG, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardThermalG, Color.LightCoral);
            quickIdProcs.Add(KnownProc.AwardUspAmmo, Color.LightGreen);
            quickIdProcs.Add(KnownProc.AwardWetBox, Color.LightCoral);
        }

        private void selectGcxFile_Click(object sender, EventArgs e)
        {
            contentTree = new List<DecodedProc>();
            contentTreeCarbonCopy = new List<DecodedProc>();
            contentsTreeView.Nodes.Clear();
            hexFunctionChanges = new Dictionary<string, string>();
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog(this);
            gcx_Editor = new GcxEditor();

            if (result == DialogResult.OK)
            {
                debugTextbox.Text = gcx_Editor.CallDecompiler(ofd.FileName);
                this.Text = ofd.SafeFileName;
                contentTree = gcx_Editor.BuildContentTree();
                /* this will show you all the (known) functions within the gcx file responsible for item/weapon spawns.
                 * this can give you a good idea of _what_ each level has in it as possible spawns.
                foreach(KeyValuePair<string, string> entry in contentTree)
                {
                    if(ProcIds.SpawnProcs.Any(knownProc => entry.Key.Contains(knownProc.BigEndianRepresentation))) //this works fine to do what it is designed to
                        contentTreeCarbonCopy.Add(entry.Key, entry.Value);
                }
                */

                //filter out any procs that don't call any of our known, desired procs
                foreach(DecodedProc entry in contentTree)
                {
                    if (KnownProc.SpawnProcs.Any(knownProc => entry.Name.Contains(knownProc.BigEndianRepresentation)))
                        contentTreeCarbonCopy.Add(entry);
                }

                List<TreeNode> treeNodes = new List<TreeNode>();
                foreach(DecodedProc item in contentTreeCarbonCopy)
                {
                    treeNodes.Add(new TreeNode(item.Name));
                }
                contentsTreeView.Nodes.AddRange(treeNodes.ToArray());
            }
        }

        private void contentsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string selectedNode = e.Node.Text;

            nodeContentsGroupBox.Text = selectedNode;
            functionContentsRichTextbox.Text = contentTree.Find(proc => proc.Name == selectedNode).Name;
            functionContentsRichTextbox.Text = functionContentsRichTextbox.Text.Replace("\r\n", "");
            functionContentsRichTextbox.Text = functionContentsRichTextbox.Text.Replace("\t", "");
            functionContentsRichTextbox.Text = functionContentsRichTextbox.Text.Replace("}", "}\r\n");

            
            foreach(KeyValuePair<RawProc, Color> kvp in quickIdProcs)
            {
                //this goes through and highlights all proc calls in the decoded text
                List<int> positionsFound = new List<int>();
                do
                {
                    positionsFound.Add(functionContentsRichTextbox.Find(kvp.Key.BigEndianRepresentation, positionsFound.Count > 0 ? positionsFound.Last() + 1 : 0, RichTextBoxFinds.None));
                } while (!positionsFound.Any(position => position == -1));
                foreach (int position in positionsFound)
                {
                    if (position == -1)
                        break;
                    functionContentsRichTextbox.Select(position, kvp.Key.BigEndianRepresentation.Length);
                    functionContentsRichTextbox.SelectionBackColor = kvp.Value;
                }
            }

            string hexContents = BitConverter.ToString(gcx_Editor.Procedures.Find(proc => proc.Name == selectedNode).RawContents);
            hexCodeRichTextbox.MaxLength = hexContents.Length;
            hexCodeRichTextbox.Text = hexContents;

            //TODO: highlight the procs in the hexCodeRichTextBox as well
        }

        private void saveFunctionChangesBtn_Click(object sender, EventArgs e)
        {
            if (!ForceProperHexFormat(hexCodeRichTextbox))
            {
                return;
            } 
            if (hexCodeRichTextbox.TextLength != hexCodeRichTextbox.MaxLength)
            {
                //TODO: verify this works
                if (hexCodeRichTextbox.Text[hexCodeRichTextbox.Text.Length - 1] == '-')
                {
                    hexCodeRichTextbox.Text.Remove(hexCodeRichTextbox.TextLength -1, 1);
                }
                while(hexCodeRichTextbox.TextLength != hexCodeRichTextbox.MaxLength)
                {
                    hexCodeRichTextbox.Text += "-00";
                }
            }
            string selectedFunction = contentsTreeView.SelectedNode.Text;
            if (!hexFunctionChanges.ContainsKey(selectedFunction))
                hexFunctionChanges.Add(selectedFunction, hexCodeRichTextbox.Text);
            else
                hexFunctionChanges[selectedFunction] = hexCodeRichTextbox.Text;

            MessageBox.Show("Function changes saved!");
        }

        private bool ForceProperHexFormat(RichTextBox textbox)
        {
            //TODO: fix this, cuz it isn't working correctly
            return true;
            bool validFormatting = true;
            
            for(int i = 0; i < textbox.MaxLength; i++)
            {
                if (i != 0 && i % 3 == 0)
                {
                    //must be a hyphen
                    if (textbox.Text[i] != '-')
                    {
                        validFormatting = false;
                    }
                }
                else
                {
                    //must not be a hyphen
                    if (!hexRegex.IsMatch(textbox.Text[i].ToString()))
                    {
                        validFormatting = false;
                    }
                }
            }

            if (!validFormatting)
            {
                MessageBox.Show("Invalid hex code format - values can only be 0-9, A-F. Every third character must be a hyphen('-').");
            }
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, byte[]> formattedChanges = new Dictionary<string, byte[]>();
            foreach(KeyValuePair<string, string> unformattedChange in hexFunctionChanges)
            {
                List<byte> derivedBytes = new List<byte>();
                foreach(string substring in unformattedChange.Value.Split('-'))
                {
                    derivedBytes.Add(byte.Parse(substring, NumberStyles.HexNumber));
                }
                formattedChanges.Add(unformattedChange.Key, derivedBytes.ToArray());
            }

            gcx_Editor.SaveGcxFile(formattedChanges);
            MessageBox.Show("GCX file saved!");
        }

        private void hexCodeTextbox_TextChanged(object sender, EventArgs e)
        {
            //TODO: should we do anything here?            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] contents = new byte[] { 0x8D, 0x12, 0x6D, 0x0F, 0xC9, 0x2B, 0x08, 0x04, 0x06, 0xCB, 0xB5, 0x56, 0x55, 0x6D, 0x06, 0xE9, 0xB3, 0xCC };
            DecodedProc procedure = new DecodedProc("40ADFE", contents, "doesnt matter", 0, 0);

            gcx_Editor.InsertNewProcedureToFile(procedure);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
