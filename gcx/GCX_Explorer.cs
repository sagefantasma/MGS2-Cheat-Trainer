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
        public GCX_Explorer()
        {
            InitializeComponent();
        }

        private void selectGcxFile_Click(object sender, EventArgs e)
        {
            contentTree = new Dictionary<string, string>();
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog(this);
            gcx_editor editor = new gcx_editor();

            if (result == DialogResult.OK)
            {
                debugTextbox.Text = editor.CallDecompiler(ofd.FileName);
                contentTree = editor.BuildContentTree();
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
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("{", "{\r\n\t");
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("}", "}\r\n");
        }

        private void saveFunctionChangesBtn_Click(object sender, EventArgs e)
        {
            contentTree[nodeContentsGroupBox.Text] = functionContentsTextbox.Text;
        }
    }
}
