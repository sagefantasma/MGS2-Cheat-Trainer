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
        private class ItemSpawn
        {
            public DecodedProc spawnerProc { get; set; }
            public int positionInSpawnerProc { get; set; }
            public RawProc itemProc { get; set; }
            public ComboBox uiComboBox { get; set; }

            public ItemSpawn(DecodedProc spawnerProc, int positionInSpawnerProc, RawProc itemProc, ComboBox comboBox)
            {
                this.spawnerProc = spawnerProc;
                this.positionInSpawnerProc = positionInSpawnerProc;
                this.itemProc = itemProc;
                this.uiComboBox = comboBox;
            }
        }

        List<ItemSpawn> spawnProcsCalled = new List<ItemSpawn>();
        public ProcEditor(List<DecodedProc> spawnerProcs)
        {
            InitializeComponent();
            foreach (DecodedProc procToEdit in spawnerProcs)
            {
                CreateUIElements(procToEdit);
            }
            int a = 2 + 2;
        }

        private void CreateUIElements(DecodedProc procToEdit)
        {
            foreach(RawProc knownProc in KnownProc.SpawnProcs)
            {
                List<int> positions = GcxEditor.FindAllSubArray(procToEdit.RawContents, knownProc.LittleEndianRepresentation);
                if(positions == null || positions.Count == 0)
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
                        spawnProcsCalled.Add(new ItemSpawn(procToEdit, position, knownProc, comboBox));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(ItemSpawn spawnProc in spawnProcsCalled)
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
    }
}
