namespace MGS2_MC
{
    partial class ConfigEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigEditorForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.closeMgs2CheckBox = new System.Windows.Forms.CheckBox();
            this.closeTrainerCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.launchMgs2CheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.mgs2ExeTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.10962F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.89038F));
            this.tableLayoutPanel1.Controls.Add(this.closeMgs2CheckBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.closeTrainerCheckBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.saveButton, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.launchMgs2CheckBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(447, 275);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // closeMgs2CheckBox
            // 
            this.closeMgs2CheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.closeMgs2CheckBox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.closeMgs2CheckBox, 2);
            this.closeMgs2CheckBox.Location = new System.Drawing.Point(127, 74);
            this.closeMgs2CheckBox.Name = "closeMgs2CheckBox";
            this.closeMgs2CheckBox.Size = new System.Drawing.Size(193, 17);
            this.closeMgs2CheckBox.TabIndex = 1;
            this.closeMgs2CheckBox.Text = "Close MGS2 When Trainer Closes?";
            this.closeMgs2CheckBox.UseVisualStyleBackColor = true;
            this.closeMgs2CheckBox.MouseHover += new System.EventHandler(this.CloseMgs2CheckBox_MouseHover);
            // 
            // closeTrainerCheckBox
            // 
            this.closeTrainerCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.closeTrainerCheckBox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.closeTrainerCheckBox, 2);
            this.closeTrainerCheckBox.Location = new System.Drawing.Point(127, 129);
            this.closeTrainerCheckBox.Name = "closeTrainerCheckBox";
            this.closeTrainerCheckBox.Size = new System.Drawing.Size(193, 17);
            this.closeTrainerCheckBox.TabIndex = 2;
            this.closeTrainerCheckBox.Text = "Close Trainer When MGS2 Closes?";
            this.closeTrainerCheckBox.UseVisualStyleBackColor = true;
            this.closeTrainerCheckBox.MouseHover += new System.EventHandler(this.CloseTrainerCheckBox_MouseHover);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 99;
            this.label1.Text = "MGS2 Executable Location:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(24, 232);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel Changes";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.saveButton.Location = new System.Drawing.Point(247, 232);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 30);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // launchMgs2CheckBox
            // 
            this.launchMgs2CheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.launchMgs2CheckBox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.launchMgs2CheckBox, 2);
            this.launchMgs2CheckBox.Location = new System.Drawing.Point(128, 19);
            this.launchMgs2CheckBox.Name = "launchMgs2CheckBox";
            this.launchMgs2CheckBox.Size = new System.Drawing.Size(191, 17);
            this.launchMgs2CheckBox.TabIndex = 0;
            this.launchMgs2CheckBox.Text = "Launch MGS2 on Trainer Launch?";
            this.launchMgs2CheckBox.UseVisualStyleBackColor = true;
            this.launchMgs2CheckBox.MouseHover += new System.EventHandler(this.LaunchMgs2CheckBox_MouseHover);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.64846F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.35154F));
            this.tableLayoutPanel2.Controls.Add(this.mgs2ExeTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.browseButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(151, 168);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(293, 49);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // mgs2ExeTextBox
            // 
            this.mgs2ExeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mgs2ExeTextBox.Enabled = false;
            this.mgs2ExeTextBox.Location = new System.Drawing.Point(3, 3);
            this.mgs2ExeTextBox.Multiline = true;
            this.mgs2ExeTextBox.Name = "mgs2ExeTextBox";
            this.mgs2ExeTextBox.Size = new System.Drawing.Size(200, 43);
            this.mgs2ExeTextBox.TabIndex = 0;
            this.mgs2ExeTextBox.TabStop = false;
            this.mgs2ExeTextBox.MouseHover += new System.EventHandler(this.Mgs2ExeTextBox_MouseHover);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseButton.Location = new System.Drawing.Point(212, 13);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 3;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // ConfigEditorForm
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(447, 275);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Editor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.ConfigEditorForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox closeMgs2CheckBox;
        private System.Windows.Forms.CheckBox closeTrainerCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox launchMgs2CheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox mgs2ExeTextBox;
        private System.Windows.Forms.Button browseButton;
    }
}