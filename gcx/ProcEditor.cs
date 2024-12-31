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

namespace gcx
{
    public partial class ProcEditor : Form
    {
        public class Coordinates
        {
            public long X { get; set; }
            public long Z { get; set; }
            public long Y { get; set; }
            public long Rotation { get; set; }
        }

        public class ItemSpawn
        {
            public DecodedProc spawnerProc { get; set; }
            public int positionInSpawnerProc { get; set; }
            public RawProc itemProc { get; set; }
            public ComboBox uiComboBox { get; set; }
            public Coordinates Coordinates { get; set; }
            public byte[] Id { get; set; }

            public ItemSpawn(DecodedProc spawnerProc, int positionInSpawnerProc, RawProc itemProc, Coordinates coordinates = null, byte[] id = null)
            {
                this.spawnerProc = spawnerProc;
                this.positionInSpawnerProc = positionInSpawnerProc;
                this.itemProc = itemProc;
                Coordinates = coordinates;
                Id = id;
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

        private List<ItemSpawn> _spawnProcsCalled = new List<ItemSpawn>();
        public List<ItemSpawn> SpawningProcs {  get { return _spawnProcsCalled; } }


        #region Automated Modification
        public ProcEditor(List<DecodedProc> spawnerProcs, bool automated)
        {
            InitializeEditor(spawnerProcs);
        }

        public void InitializeEditor(List<DecodedProc> spawnerProcs)
        {
            _spawnProcsCalled = new List<ItemSpawn>();
            foreach(DecodedProc procToEdit in spawnerProcs)
            {
                EnumerateSpawnProcs(procToEdit);
            }
        }

        public void WriteOutSpawns(string gcxFile)
        {
            string fileContents = "";
            foreach(ItemSpawn spawnProc in _spawnProcsCalled)
            {
                //TankerPart1.Entities.Add(new Location (
                //gcxFile : "w00a", spawnId : new byte[] { 0x6E, 0x0E, 0xA8 },
                //posX : 0xB7BC, posZ : 0, posY : 0xBBA4, rot : 1), MGS2Items.Ration); //not guaranteed spawn

                string spawnInfo = $"Entities.Add(new Location(gcxFile: \"{gcxFile}\", spawnId: new byte[] {{";
                for(int i = 0; i<spawnProc.Id.Length; i++)
                {
                    spawnInfo += $"0x{spawnProc.Id[i]}";
                    if(i != spawnProc.Id.Length - 1)
                    {
                        spawnInfo += ", ";
                    }
                }
                spawnInfo += $"}}, posX: 0x0, posZ: 0x0, posY: 0x0, rot: 0), MGS2Items.{spawnProc.itemProc.CommonName}\n";
                fileContents += spawnInfo;
            }

            File.WriteAllText($"{gcxFile}spawns.txt", fileContents);
        }

        private void EnumerateSpawnProcs(DecodedProc procToEdit)
        {
            foreach (RawProc knownProc in KnownProc.SpawnProcs)
            {
                List<int> indices = GcxEditor.FindAllSubArray(procToEdit.RawContents, knownProc.LittleEndianRepresentation);
                if (indices == null || indices.Count == 0)
                {
                    continue;
                }
                foreach (int index in indices)
                {
                    if (procToEdit.RawContents[index - 2] == 0x7D)
                    {
                        Coordinates spawnCoordinates = GetCoordinates(procToEdit.RawContents, index);
                        byte[] spawnId = GetSpawnId(procToEdit.RawContents, index);
                        _spawnProcsCalled.Add(new ItemSpawn(procToEdit, index, knownProc, spawnCoordinates, spawnId));
                    }
                }
            }
        }

        private Coordinates GetCoordinates(byte[] contents, int index)
        {
            Coordinates coordinates = new Coordinates();

            //need to do some complex calculations to get these i think. leaving an analysis of just 2 spawns to illustrate and deal with this later
            /*
             * 7D-12- (call proc, 18 bytes long)

47-7D-F5 (proc to call)

-06-A9-42-8B (spawn ID)

-01-10-27- (X position)

01-D0-07- (Z position)

01-0C-FE- (Y position)

C2-00 (rotation, denotes a value of 1)




7D-10- (call proc, 16 bytes long)

22-A2-D2- (proc to call)

06-4F-9E-26- (spawn ID)

01-BA-E1- (X position)

C1- (Z position, denotes a value of 0)

01-94-11- (Y position)

C2-00 (rotation, denotes a value of 1)
             * 
             */

            return coordinates;
        }

        private byte[] GetSpawnId(byte[] contents, int index)
        {
            //TODO: validate
            byte[] spawnId;
            byte byteAfterSpawn = contents[index + 3];
            //i am seeing a possible pattern:
            //immediately after item declaration there is either a 06 or a 0D
            //from the three files i have checked thus far, 06 is before a 3 byte ID
            //and 0D is before a 4 byte ID. Please tell me this is real. IT LOOKS LIKE IT BOYS.
            if (byteAfterSpawn == 0x0D)
                spawnId = new byte[4];
            else if (byteAfterSpawn == 0x06)
                spawnId = new byte[3];
            else
            {
                throw new Exception("my idea is was bad and i feel bad");
            }

            Array.Copy(contents, index + 4, spawnId, 0, spawnId.Length); //i _believe_ this is correct all of the time

            return spawnId;
        }

        public void ModifySpawnProc(byte[] spawnId, RawProc replacementProc)
        {
            List<ItemSpawn> localSpawns = _spawnProcsCalled.FindAll(x => x.Id.SequenceEqual(spawnId));

            foreach(ItemSpawn localSpawn in localSpawns)
                localSpawn.itemProc = replacementProc;
        }

        public void SaveAutomatedChanges()
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
