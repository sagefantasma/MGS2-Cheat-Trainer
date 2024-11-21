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
        }

        private void addProcsButton_Click(object sender, EventArgs e)
        {
            foreach(var proc in procListBox.CheckedItems)
            {
                RawProc rawProc = proc as RawProc;
                FileInfo procFile = new FileInfo($"MGS2 Known Procs/proc_0x{rawProc.BigEndianRepresentation}.proc");
                string procName = procFile.Name.Replace("proc_0x", "").Replace(".proc", "");
                uint order = Convert.ToUInt32(procName.Trim(), 16);
                DecodedProc procedure = new DecodedProc(procName, order, File.ReadAllBytes(procFile.FullName), null, 0, 0);
                ProcsToAdd.Add(procedure);
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
