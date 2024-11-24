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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace gcx
{
    public partial class ProcSelector : Form
    {
        public List<DecodedProc> ProcsToAdd = new List<DecodedProc>();
        public ProcSelector()
        {
            InitializeComponent();
            SecondInitializer();
        }

        private void SecondInitializer()
        {
            procListBox.DataSource = KnownProc.SpawnProcs;
            procListBox.DisplayMember = "CommonName";
            InitializeProcDependencies();
        }

        private void InitializeProcDependencies()
        {
            KnownProc.GunCheck.ProcDependencies = new RawProc[] { KnownProc.AwardM9Ammo, KnownProc.AwardUspAmmo,
                KnownProc.AwardSocomAmmo, KnownProc.AwardM4Ammo, KnownProc.AwardPsg1Ammo, KnownProc.AwardPsg1tAmmo,
                KnownProc.AwardRgbAmmo, KnownProc.AwardNikitaAmmo, KnownProc.AwardStingerAmmo, KnownProc.AwardAksAmmo,
                KnownProc.AwardC4, KnownProc.AwardChaffG, KnownProc.AwardStunG, KnownProc.AwardGrenade,
                KnownProc.AwardBook, KnownProc.AwardRation, KnownProc.AwardBandages, KnownProc.AwardPentazemin };
        }

        public static DecodedProc ConvertRawProcToDecodedProc(RawProc rawProc)
        {
            FileInfo procFile = new FileInfo($"MGS2 Known Procs/proc_0x{rawProc.BigEndianRepresentation}.proc");
            string procName = procFile.Name.Replace("proc_0x", "").Replace(".proc", "");
            uint order = Convert.ToUInt32(procName.Trim(), 16);
            return new DecodedProc(procName, order, File.ReadAllBytes(procFile.FullName), null, 0, 0);
        }

        private void addProcsButton_Click(object sender, EventArgs e)
        {
            foreach(var proc in procListBox.CheckedItems)
            {
                RawProc rawProc = proc as RawProc;
                DecodedProc procedure = ConvertRawProcToDecodedProc(rawProc);
                ProcsToAdd.Add(procedure);
                AddDependencies(rawProc);
            }
            //MessageBox.Show($"Adding {ProcsToAdd.Count} procs!");
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void AddDependencies(RawProc proc)
        {
            if (proc.ProcDependencies != null)
            {
                foreach (RawProc dependency in proc.ProcDependencies)
                {
                    AddDependencies(dependency);
                    if(!ProcsToAdd.Any(alreadyQueuedProc => alreadyQueuedProc.Name == dependency.BigEndianRepresentation))
                        ProcsToAdd.Add(ConvertRawProcToDecodedProc(dependency));
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
