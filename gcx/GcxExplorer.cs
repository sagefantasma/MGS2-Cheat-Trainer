﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
            quickIdProcs.Add(KnownProc.AwardUspSuppressor, Color.LightCoral);
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
                    //if (KnownProc.SpawnProcs.Any(knownProc => entry.Name.Contains(knownProc.BigEndianRepresentation)))
                    if(ContainsSpawningFunctions(entry))
                        contentTreeCarbonCopy.Add(entry);
                }

                List<TreeNode> treeNodes = new List<TreeNode>();
                foreach(DecodedProc item in contentTreeCarbonCopy)
                {
                    treeNodes.Add(new TreeNode(item.Name));
                    if(!item.Name.Contains("main"))
                        File.WriteAllBytes($"{item.Name.Trim()}.proc", contentTree.Find(proc => proc.Name == item.Name).RawContents);
                }
                contentsTreeView.Nodes.AddRange(treeNodes.ToArray());
            }
        }

        private bool ContainsSpawningFunctions(DecodedProc func)
        {
            List<string> spawningFunctions = new List<string>();
            foreach(RawProc spawningFunc in KnownProc.SpawnProcs)
            {
                spawningFunctions.Add(spawningFunc.BigEndianRepresentation);
            }
            return spawningFunctions.Any(function => func.DecodedContents.Contains(function));
        }

        private void contentsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string selectedNode = e.Node.Text;

            nodeContentsGroupBox.Text = selectedNode;
            functionContentsRichTextbox.Text = contentTree.Find(proc => proc.Name == selectedNode).DecodedContents;
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

            string hexContents = BitConverter.ToString(contentTree.Find(proc => proc.Name == selectedNode).RawContents);
            hexCodeRichTextbox.MaxLength = hexContents.Length;
            hexCodeRichTextbox.Text = hexContents;

            //TODO: highlight the procs in the hexCodeRichTextBox as well
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {
            byte[] newGcxBytes = gcx_Editor.BuildGcxFile();
            string date = $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_custom.gcx"; 
            File.WriteAllBytes(date, newGcxBytes);
            MessageBox.Show("GCX file saved!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<DecodedProc> procsToAdd = SelectProcsToAdd();
            if (procsToAdd.Count == 0)
                return;
            foreach (DecodedProc proc in procsToAdd)
            {
                gcx_Editor.InsertNewProcedureToFile(proc);
            }
        }

        private List<DecodedProc> SelectProcsToAdd()
        {
            using (ProcSelector procSelector = new ProcSelector())
                if (procSelector.ShowDialog() == DialogResult.OK)
                {
                    return ProcSelector.ProcsToAdd;
                }
            return null;
        }

        private void AddAllProcs()
        {
            ProcSelector.GetAllProcs();
            
            foreach(DecodedProc proc in ProcSelector.ProcsToAdd)
            {
                gcx_Editor.InsertNewProcedureToFile(proc);
            }
        }

        private void ExecuteRandomization(object sender, EventArgs e)
        {
            string mgs2Directory = @"C:\Users\yonan\Documents\Pinned Folders\C Drive Steam Games\MGS2\";
            MGS2Randomizer mgs2Randomizer = new MGS2Randomizer(mgs2Directory, 0);

            mgs2Randomizer.RandomizeItemSpawns(new MGS2Randomizer.RandomizationOptions { NoHardLogicLocks = true });
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void InsertAllKnownProcs(object sender, EventArgs e)
        {
            AddAllProcs();
            saveFileButton_Click(null, null);
        }

        /*private void OopsAllShavers(object sender, EventArgs e)
        {
            List<DecodedProc> spawnerProcsInFile = new List<DecodedProc>();
            foreach (DecodedProc spawnerProc in contentTreeCarbonCopy)
            {
                spawnerProcsInFile.Add(spawnerProc);
            }
            ProcEditor.InitializeEditor(spawnerProcsInFile);
            //Technically, adding all procs is TOTAL overkill, but it's literally just 5.4KB. Unless something doesn't load as a result
            //I think this is the easiest and most straight-forward solution.
            AddAllProcs();
            List<ProcEditor.ItemSpawn> list = ProcEditor.SpawningProcs;
            foreach (ProcEditor.ItemSpawn spawn in list)
            {
                ProcEditor.ModifySpawnProc(spawn.Id, KnownProc.AwardShaver);
            }
            ProcEditor.SaveAutomatedChanges();
            saveFileButton_Click(null, null);
        }*/

        private void ManuallyLoadProc()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".proc";
            ofd.Multiselect = true;
            ofd.Filter = "Proc files (*.proc)|*.proc";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] filesSelected = ofd.FileNames;
                foreach (string fileSelected in filesSelected)
                {
                    FileInfo fileInfo = new FileInfo(fileSelected);
                    string procName = fileInfo.Name.Replace("proc_0x", "").Replace(".proc", "");
                    uint order = Convert.ToUInt32(procName.Trim(), 16);
                    DecodedProc procedure = new DecodedProc(procName, order, File.ReadAllBytes(fileSelected), null, 0, 0);
                    gcx_Editor.InsertNewProcedureToFile(procedure);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<DecodedProc> spawnerProcsInFile = new List<DecodedProc>();
            foreach(DecodedProc spawnerProc in contentTreeCarbonCopy)
            {
                spawnerProcsInFile.Add(spawnerProc);
            }
            ProcEditor procEditor = new ProcEditor(spawnerProcsInFile);
            procEditor.ShowDialog();
        }
    }
}
