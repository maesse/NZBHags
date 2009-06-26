namespace NZBHags.GUI
{
    partial class GeneralSettingsGUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCacheSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxMultiCore = new System.Windows.Forms.CheckBox();
            this.checkBoxPostCleanup = new System.Windows.Forms.CheckBox();
            this.checkBoxPar2Enabled = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxRarEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxCacheSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 205);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Settings";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(229, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "*Doesn\'t take effect before program is restarted";
            // 
            // textBoxCacheSize
            // 
            this.textBoxCacheSize.Location = new System.Drawing.Point(94, 20);
            this.textBoxCacheSize.Name = "textBoxCacheSize";
            this.textBoxCacheSize.Size = new System.Drawing.Size(43, 20);
            this.textBoxCacheSize.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "MB*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max cache size:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxMultiCore);
            this.groupBox2.Controls.Add(this.checkBoxPostCleanup);
            this.groupBox2.Controls.Add(this.checkBoxPar2Enabled);
            this.groupBox2.Location = new System.Drawing.Point(272, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(129, 109);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PAR2 Checking";
            // 
            // checkBoxMultiCore
            // 
            this.checkBoxMultiCore.AutoSize = true;
            this.checkBoxMultiCore.Location = new System.Drawing.Point(6, 65);
            this.checkBoxMultiCore.Name = "checkBoxMultiCore";
            this.checkBoxMultiCore.Size = new System.Drawing.Size(100, 17);
            this.checkBoxMultiCore.TabIndex = 3;
            this.checkBoxMultiCore.Text = "Multicore PAR2";
            this.checkBoxMultiCore.UseVisualStyleBackColor = true;
            // 
            // checkBoxPostCleanup
            // 
            this.checkBoxPostCleanup.AutoSize = true;
            this.checkBoxPostCleanup.Location = new System.Drawing.Point(6, 42);
            this.checkBoxPostCleanup.Name = "checkBoxPostCleanup";
            this.checkBoxPostCleanup.Size = new System.Drawing.Size(97, 17);
            this.checkBoxPostCleanup.TabIndex = 2;
            this.checkBoxPostCleanup.Text = "POST-Cleanup";
            this.checkBoxPostCleanup.UseVisualStyleBackColor = true;
            // 
            // checkBoxPar2Enabled
            // 
            this.checkBoxPar2Enabled.AutoSize = true;
            this.checkBoxPar2Enabled.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPar2Enabled.Name = "checkBoxPar2Enabled";
            this.checkBoxPar2Enabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxPar2Enabled.TabIndex = 1;
            this.checkBoxPar2Enabled.Text = "Enabled";
            this.checkBoxPar2Enabled.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkBoxRarEnabled);
            this.groupBox3.Location = new System.Drawing.Point(272, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(129, 90);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RAR Extraction";
            // 
            // checkBoxRarEnabled
            // 
            this.checkBoxRarEnabled.AutoSize = true;
            this.checkBoxRarEnabled.Location = new System.Drawing.Point(6, 19);
            this.checkBoxRarEnabled.Name = "checkBoxRarEnabled";
            this.checkBoxRarEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxRarEnabled.TabIndex = 0;
            this.checkBoxRarEnabled.Text = "Enabled";
            this.checkBoxRarEnabled.UseVisualStyleBackColor = true;
            // 
            // GeneralSettingsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GeneralSettingsGUI";
            this.Size = new System.Drawing.Size(404, 211);
            this.Load += new System.EventHandler(this.UpdateUI);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxPar2Enabled;
        private System.Windows.Forms.CheckBox checkBoxRarEnabled;
        private System.Windows.Forms.TextBox textBoxCacheSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxPostCleanup;
        private System.Windows.Forms.CheckBox checkBoxMultiCore;
        private System.Windows.Forms.Label label3;
    }
}
