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
    public partial class GCX_Explorer : Form
    {
        private Dictionary<string, string> contentTree = new Dictionary<string, string>();
        private Dictionary<string, string> contentTreeCarbonCopy = new Dictionary<string, string>();
        private Dictionary<string, string> hexFunctionChanges = new Dictionary<string, string>();
        private gcx_editor gcx_Editor;
        readonly Regex hexRegex = new Regex("^[0-9A-F]+$");

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
            functionContentsTextbox.Text = functionContentsTextbox.Text.Replace("}", "}\r\n");
            string hexContents = BitConverter.ToString(gcx_Editor.Procedures.Find(proc => proc.Name == selectedNode).RawContents);
            hexCodeTextbox.MaxLength = hexContents.Length;
            hexCodeTextbox.Text = hexContents;
        }

        private void saveFunctionChangesBtn_Click(object sender, EventArgs e)
        {
            //ForceProperHexFormat(hexCodeTextbox);
            if (hexCodeTextbox.TextLength != hexCodeTextbox.MaxLength)
            {
                //TODO: insert a bunch of zeroes at the end of the function until the max length is met
                if (hexCodeTextbox.Text[hexCodeTextbox.Text.Length - 1] == '-')
                {
                    hexCodeTextbox.Text.Remove(hexCodeTextbox.TextLength -1, 1);
                }
                while(hexCodeTextbox.TextLength != hexCodeTextbox.MaxLength)
                {
                    hexCodeTextbox.Text += "-00";
                }
            }
            string selectedFunction = contentsTreeView.SelectedNode.Text;
            if (!hexFunctionChanges.ContainsKey(selectedFunction))
                hexFunctionChanges.Add(selectedFunction, hexCodeTextbox.Text);
            else
                hexFunctionChanges[selectedFunction] = hexCodeTextbox.Text;
        }

        private void ForceProperHexFormat(TextBox textbox)
        {
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
        }

        private void hexCodeTextbox_TextChanged(object sender, EventArgs e)
        {
            //TODO: should we do anything here?            
        }
    }
}
