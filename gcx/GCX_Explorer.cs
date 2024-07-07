using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gcx
{
    public partial class GCX_Explorer : Form
    {
        private Dictionary<string, string> contentTree = new Dictionary<string, string>();
        private Dictionary<string, string> contentTreeCarbonCopy = new Dictionary<string, string>();
        private gcx_editor gcx_Editor;
        public GCX_Explorer()
        {
            InitializeComponent();
        }

        private void selectGcxFile_Click(object sender, EventArgs e)
        {
            contentTree = new Dictionary<string, string>();
            contentTreeCarbonCopy = new Dictionary<string, string>();
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog(this);
            gcx_Editor = new gcx_editor();

            if (result == DialogResult.OK)
            {
                debugTextbox.Text = gcx_Editor.CallDecompiler(ofd.FileName);
                contentTree = gcx_Editor.BuildContentTree();
                foreach(KeyValuePair<string, string> entry in contentTree)
                {
                    contentTreeCarbonCopy.Add(entry.Key, entry.Value);
                }

                List<TreeNode> treeNodes = new List<TreeNode>();
                foreach(KeyValuePair<string, string> item in contentTree)
                {
                    treeNodes.Add(new TreeNode(item.Key));
                }
                contentsTreeView.Nodes.AddRange(treeNodes.ToArray());
            }
        }

        private void contentsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string selectedNode = e.Node.Text;

            nodeContentsGroupBox.Text = selectedNode;
            functionContentsTextbox.Text = contentTree[selectedNode];
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("\r\n", "");
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("\t", "");
            //functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("{", "{\r\n\t");
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("}", "}\r\n");
            hexCodeTextbox.Text = BitConverter.ToString(gcx_Editor.Procedures.Find(proc => proc.Name == selectedNode).RawContents);
        }

        private void saveFunctionChangesBtn_Click(object sender, EventArgs e)
        {
            //TODO: copy the data from the hex panel into an object to be saved into the GCX later
            // don't strip out the hypens and shit yet, because that will make things more confusing later
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {

        }
    }
}
