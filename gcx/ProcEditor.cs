using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gcx
{
    public partial class ProcEditor : Form
    {
        public class ItemSpawn
        {
            public DecodedProc spawnerProc { get; set; }
            public int positionInSpawnerProc { get; set; }
            public RawProc itemProc { get; set; }
            public ComboBox uiComboBox { get; set; }

            public ItemSpawn(DecodedProc spawnerProc, int positionInSpawnerProc, RawProc itemProc)
            {
                this.spawnerProc = spawnerProc;
                this.positionInSpawnerProc = positionInSpawnerProc;
                this.itemProc = itemProc;
            }

            public ItemSpawn(DecodedProc spawnerProc, int positionInSpawnerProc, RawProc itemProc, ComboBox comboBox)
            {
                this.spawnerProc = spawnerProc;
                this.positionInSpawnerProc = positionInSpawnerProc;
                this.itemProc = itemProc;
                this.uiComboBox = comboBox;
            }
        }

        #region Manual Modification
        public ProcEditor(List<DecodedProc> spawnerProcs)
        {
            InitializeComponent();
            foreach (DecodedProc procToEdit in spawnerProcs)
            {
                CreateUIElements(procToEdit);
            }
        }

        private void CreateUIElements(DecodedProc procToEdit)
        {
            foreach (RawProc knownProc in KnownProc.SpawnProcs)
            {
                List<int> positions = GcxEditor.FindAllSubArray(procToEdit.RawContents, knownProc.LittleEndianRepresentation);
                if (positions == null || positions.Count == 0)
                {
                    continue;
                }
                foreach (int position in positions)
                {
                    if (procToEdit.RawContents[position - 2] == 0x7D)
                    {
                        int index = KnownProc.SpawnProcs.IndexOf(knownProc);
                        BindingSource source = new BindingSource();
                        source.DataSource = KnownProc.SpawnProcs;
                        ComboBox comboBox = new ComboBox
                        {
                            DisplayMember = "CommonName",
                            DataSource = source
                        };
                        flowLayoutPanel1.Controls.Add(comboBox);
                        source.Position = index;
                        _spawnProcsCalled.Add(new ItemSpawn(procToEdit, position, knownProc, comboBox));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveManualChanges();
            Close();
        }

        private void SaveManualChanges()
        {
            foreach (ItemSpawn spawnProc in _spawnProcsCalled)
            {
                RawProc selectedProc = (spawnProc.uiComboBox.SelectedItem as RawProc);
                if (selectedProc != spawnProc.itemProc)
                {
                    //the proc has been updated
                    Array.Copy(selectedProc.LittleEndianRepresentation, 0, spawnProc.spawnerProc.RawContents, spawnProc.positionInSpawnerProc, selectedProc.LittleEndianRepresentation.Length);
                }
            }
            Close();
        }
        #endregion

        private static List<ItemSpawn> _spawnProcsCalled = new List<ItemSpawn>();
        public static List<ItemSpawn> SpawningProcs {  get { return _spawnProcsCalled; } }


        #region Automated Modification
        public static void InitializeEditor(List<DecodedProc> spawnerProcs)
        {
            foreach(DecodedProc procToEdit in spawnerProcs)
            {
                EnumerateSpawnProcs(procToEdit);
            }
        }

        private static void EnumerateSpawnProcs(DecodedProc procToEdit)
        {
            foreach (RawProc knownProc in KnownProc.SpawnProcs)
            {
                List<int> positions = GcxEditor.FindAllSubArray(procToEdit.RawContents, knownProc.LittleEndianRepresentation);
                if (positions == null || positions.Count == 0)
                {
                    continue;
                }
                foreach (int position in positions)
                {
                    if (procToEdit.RawContents[position - 2] == 0x7D)
                    {
                        _spawnProcsCalled.Add(new ItemSpawn(procToEdit, position, knownProc));
                    }
                }
            }
        }

        public static void ModifySpawnProc(ItemSpawn spawnToEdit, RawProc replacementProc)
        {
            ItemSpawn localSpawn = _spawnProcsCalled.Find(x => x.spawnerProc == spawnToEdit.spawnerProc 
                                                          && x.itemProc == spawnToEdit.itemProc 
                                                          && x.positionInSpawnerProc == spawnToEdit.positionInSpawnerProc);
            localSpawn.itemProc = replacementProc;
        }

        public static void SaveAutomatedChanges()
        {
            foreach (ItemSpawn spawnProc in _spawnProcsCalled)
            {
                RawProc selectedProc = spawnProc.itemProc;
                Array.Copy(selectedProc.LittleEndianRepresentation, 0, spawnProc.spawnerProc.RawContents, spawnProc.positionInSpawnerProc, selectedProc.LittleEndianRepresentation.Length);
            }
        }

        #endregion
    }
}
