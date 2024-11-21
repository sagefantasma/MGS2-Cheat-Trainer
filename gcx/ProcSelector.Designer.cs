namespace gcx
{
    partial class ProcSelector
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
            this.addProcsLabel = new System.Windows.Forms.Label();
            this.procListBox = new System.Windows.Forms.CheckedListBox();
            this.addProcsButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addProcsLabel
            // 
            this.addProcsLabel.AutoSize = true;
            this.addProcsLabel.Location = new System.Drawing.Point(59, 21);
            this.addProcsLabel.Name = "addProcsLabel";
            this.addProcsLabel.Size = new System.Drawing.Size(114, 20);
            this.addProcsLabel.TabIndex = 1;
            this.addProcsLabel.Text = "Proc(s) To Add";
            // 
            // procListBox
            // 
            this.procListBox.FormattingEnabled = true;
            this.procListBox.Location = new System.Drawing.Point(63, 64);
            this.procListBox.Name = "procListBox";
            this.procListBox.Size = new System.Drawing.Size(642, 257);
            this.procListBox.TabIndex = 2;
            // 
            // addProcsButton
            // 
            this.addProcsButton.Location = new System.Drawing.Point(566, 355);
            this.addProcsButton.Name = "addProcsButton";
            this.addProcsButton.Size = new System.Drawing.Size(139, 63);
            this.addProcsButton.TabIndex = 3;
            this.addProcsButton.Text = "Add Proc(s)";
            this.addProcsButton.UseVisualStyleBackColor = true;
            this.addProcsButton.Click += new System.EventHandler(this.addProcsButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(63, 355);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(122, 63);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ProcSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addProcsButton);
            this.Controls.Add(this.procListBox);
            this.Controls.Add(this.addProcsLabel);
            this.Name = "ProcSelector";
            this.Text = "ProcSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addProcsLabel;
        private System.Windows.Forms.CheckedListBox procListBox;
        private System.Windows.Forms.Button addProcsButton;
        private System.Windows.Forms.Button cancelButton;
    }
}