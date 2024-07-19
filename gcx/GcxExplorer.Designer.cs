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
            this.saveFunctionChangesBtn = new System.Windows.Forms.Button();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.hexCodeRichTextbox = new System.Windows.Forms.RichTextBox();
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
            this.guiControlsLayoutPanel.Name = "guiControlsLayoutPanel";
            this.guiControlsLayoutPanel.RowCount = 2;
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.guiControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.guiControlsLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.guiControlsLayoutPanel.TabIndex = 0;
            // 
            // selectGcxFile
            // 
            this.selectGcxFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectGcxFile.Location = new System.Drawing.Point(3, 3);
            this.selectGcxFile.Name = "selectGcxFile";
            this.selectGcxFile.Size = new System.Drawing.Size(260, 219);
            this.selectGcxFile.TabIndex = 0;
            this.selectGcxFile.Text = "Choose a GCX file to edit";
            this.selectGcxFile.UseVisualStyleBackColor = true;
            this.selectGcxFile.Click += new System.EventHandler(this.selectGcxFile_Click);
            // 
            // contentsTreeView
            // 
            this.contentsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentsTreeView.Location = new System.Drawing.Point(269, 3);
            this.contentsTreeView.Name = "contentsTreeView";
            this.contentsTreeView.Size = new System.Drawing.Size(260, 219);
            this.contentsTreeView.TabIndex = 1;
            this.contentsTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.contentsTreeView_NodeMouseClick);
            // 
            // nodeContentsGroupBox
            // 
            this.guiControlsLayoutPanel.SetColumnSpan(this.nodeContentsGroupBox, 2);
            this.nodeContentsGroupBox.Controls.Add(this.functionContentsRichTextbox);
            this.nodeContentsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeContentsGroupBox.Location = new System.Drawing.Point(3, 228);
            this.nodeContentsGroupBox.Name = "nodeContentsGroupBox";
            this.nodeContentsGroupBox.Size = new System.Drawing.Size(526, 219);
            this.nodeContentsGroupBox.TabIndex = 2;
            this.nodeContentsGroupBox.TabStop = false;
            this.nodeContentsGroupBox.Text = "groupBox1";
            // 
            // functionContentsRichTextbox
            // 
            this.functionContentsRichTextbox.DetectUrls = false;
            this.functionContentsRichTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionContentsRichTextbox.Location = new System.Drawing.Point(3, 16);
            this.functionContentsRichTextbox.Name = "functionContentsRichTextbox";
            this.functionContentsRichTextbox.ReadOnly = true;
            this.functionContentsRichTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.functionContentsRichTextbox.Size = new System.Drawing.Size(520, 200);
            this.functionContentsRichTextbox.TabIndex = 0;
            this.functionContentsRichTextbox.Text = "";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.debugTextbox);
            this.flowLayoutPanel1.Controls.Add(this.saveFunctionChangesBtn);
            this.flowLayoutPanel1.Controls.Add(this.saveFileButton);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(535, 228);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(262, 219);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // debugTextbox
            // 
            this.debugTextbox.Enabled = false;
            this.debugTextbox.Location = new System.Drawing.Point(3, 3);
            this.debugTextbox.Multiline = true;
            this.debugTextbox.Name = "debugTextbox";
            this.debugTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextbox.Size = new System.Drawing.Size(247, 76);
            this.debugTextbox.TabIndex = 6;
            // 
            // saveFunctionChangesBtn
            // 
            this.saveFunctionChangesBtn.Location = new System.Drawing.Point(3, 85);
            this.saveFunctionChangesBtn.Name = "saveFunctionChangesBtn";
            this.saveFunctionChangesBtn.Size = new System.Drawing.Size(128, 65);
            this.saveFunctionChangesBtn.TabIndex = 5;
            this.saveFunctionChangesBtn.Text = "Save Changes to Function";
            this.saveFunctionChangesBtn.UseVisualStyleBackColor = true;
            this.saveFunctionChangesBtn.Click += new System.EventHandler(this.saveFunctionChangesBtn_Click);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(137, 85);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(113, 66);
            this.saveFileButton.TabIndex = 4;
            this.saveFileButton.Text = "Save GCX File";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 62);
            this.button1.TabIndex = 7;
            this.button1.Text = "Insert Blade Function Into This GCX File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(137, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 59);
            this.button2.TabIndex = 8;
            this.button2.Text = "Enumerate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // hexCodeRichTextbox
            // 
            this.hexCodeRichTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexCodeRichTextbox.Location = new System.Drawing.Point(535, 3);
            this.hexCodeRichTextbox.Name = "hexCodeRichTextbox";
            this.hexCodeRichTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.hexCodeRichTextbox.Size = new System.Drawing.Size(262, 219);
            this.hexCodeRichTextbox.TabIndex = 6;
            this.hexCodeRichTextbox.Text = "";
            // 
            // GCX_Explorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guiControlsLayoutPanel);
            this.Name = "GCX_Explorer";
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
        private System.Windows.Forms.Button saveFunctionChangesBtn;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.TextBox debugTextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox hexCodeRichTextbox;
        private System.Windows.Forms.RichTextBox functionContentsRichTextbox;
    }
}