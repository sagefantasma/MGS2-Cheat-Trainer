namespace gcx
{
    partial class GcxExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.guiControlsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.selectGcxFile = new System.Windows.Forms.Button();
            this.contentsTreeView = new System.Windows.Forms.TreeView();
            this.nodeContentsGroupBox = new System.Windows.Forms.GroupBox();
            this.functionContentsRichTextbox = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.debugTextbox = new System.Windows.Forms.TextBox();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.hexCodeRichTextbox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.guiControlsLayoutPanel.SuspendLayout();
            this.nodeContentsGroupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guiControlsLayoutPanel
            // 
            this.guiControlsLayoutPanel.ColumnCount = 3;
            this.guiControlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.guiControlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.guiControlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.guiControlsLayoutPanel.Controls.Add(this.selectGcxFile, 0, 0);
            this.guiControlsLayoutPanel.Controls.Add(this.contentsTreeView, 1, 0);
            this.guiControlsLayoutPanel.Controls.Add(this.nodeContentsGroupBox, 0, 1);
            this.guiControlsLayoutPanel.Controls.Add(this.flowLayoutPanel1, 2, 1);
            this.guiControlsLayoutPanel.Controls.Add(this.hexCodeRichTextbox, 2, 0);
            this.guiControlsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guiControlsLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.guiControlsLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiControlsLayoutPanel.Name = "guiControlsLayoutPanel";
            this.guiControlsLayoutPanel.RowCount = 2;
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.guiControlsLayoutPanel.Size = new System.Drawing.Size(1200, 692);
            this.guiControlsLayoutPanel.TabIndex = 0;
            // 
            // selectGcxFile
            // 
            this.selectGcxFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectGcxFile.Location = new System.Drawing.Point(4, 5);
            this.selectGcxFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectGcxFile.Name = "selectGcxFile";
            this.selectGcxFile.Size = new System.Drawing.Size(391, 336);
            this.selectGcxFile.TabIndex = 0;
            this.selectGcxFile.Text = "Choose a GCX file to edit";
            this.selectGcxFile.UseVisualStyleBackColor = true;
            this.selectGcxFile.Click += new System.EventHandler(this.selectGcxFile_Click);
            // 
            // contentsTreeView
            // 
            this.contentsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentsTreeView.Location = new System.Drawing.Point(403, 5);
            this.contentsTreeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.contentsTreeView.Name = "contentsTreeView";
            this.contentsTreeView.Size = new System.Drawing.Size(391, 336);
            this.contentsTreeView.TabIndex = 1;
            this.contentsTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.contentsTreeView_NodeMouseClick);
            // 
            // nodeContentsGroupBox
            // 
            this.guiControlsLayoutPanel.SetColumnSpan(this.nodeContentsGroupBox, 2);
            this.nodeContentsGroupBox.Controls.Add(this.functionContentsRichTextbox);
            this.nodeContentsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeContentsGroupBox.Location = new System.Drawing.Point(4, 351);
            this.nodeContentsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nodeContentsGroupBox.Name = "nodeContentsGroupBox";
            this.nodeContentsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nodeContentsGroupBox.Size = new System.Drawing.Size(790, 336);
            this.nodeContentsGroupBox.TabIndex = 2;
            this.nodeContentsGroupBox.TabStop = false;
            this.nodeContentsGroupBox.Text = "groupBox1";
            // 
            // functionContentsRichTextbox
            // 
            this.functionContentsRichTextbox.DetectUrls = false;
            this.functionContentsRichTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionContentsRichTextbox.Location = new System.Drawing.Point(4, 24);
            this.functionContentsRichTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.functionContentsRichTextbox.Name = "functionContentsRichTextbox";
            this.functionContentsRichTextbox.ReadOnly = true;
            this.functionContentsRichTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.functionContentsRichTextbox.Size = new System.Drawing.Size(782, 307);
            this.functionContentsRichTextbox.TabIndex = 0;
            this.functionContentsRichTextbox.Text = "";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.debugTextbox);
            this.flowLayoutPanel1.Controls.Add(this.saveFileButton);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(802, 351);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(394, 336);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // debugTextbox
            // 
            this.debugTextbox.Location = new System.Drawing.Point(4, 5);
            this.debugTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.debugTextbox.Multiline = true;
            this.debugTextbox.Name = "debugTextbox";
            this.debugTextbox.ReadOnly = true;
            this.debugTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextbox.Size = new System.Drawing.Size(368, 115);
            this.debugTextbox.TabIndex = 6;
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(4, 130);
            this.saveFileButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(170, 102);
            this.saveFileButton.TabIndex = 4;
            this.saveFileButton.Text = "Save GCX File";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // hexCodeRichTextbox
            // 
            this.hexCodeRichTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexCodeRichTextbox.Location = new System.Drawing.Point(802, 5);
            this.hexCodeRichTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hexCodeRichTextbox.Name = "hexCodeRichTextbox";
            this.hexCodeRichTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.hexCodeRichTextbox.Size = new System.Drawing.Size(394, 336);
            this.hexCodeRichTextbox.TabIndex = 6;
            this.hexCodeRichTextbox.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 130);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 102);
            this.button1.TabIndex = 7;
            this.button1.Text = "Insert Proc";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // GcxExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.guiControlsLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GcxExplorer";
            this.Text = "GCX_Editor";
            this.guiControlsLayoutPanel.ResumeLayout(false);
            this.nodeContentsGroupBox.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel guiControlsLayoutPanel;
        private System.Windows.Forms.Button selectGcxFile;
        private System.Windows.Forms.TreeView contentsTreeView;
        private System.Windows.Forms.GroupBox nodeContentsGroupBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.TextBox debugTextbox;
        private System.Windows.Forms.RichTextBox hexCodeRichTextbox;
        private System.Windows.Forms.RichTextBox functionContentsRichTextbox;
        private System.Windows.Forms.Button button1;
    }
}