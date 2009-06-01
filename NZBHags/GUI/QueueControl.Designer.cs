namespace NZBHags
{
    partial class QueueControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelTimeleft = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelMb = new System.Windows.Forms.Label();
            this.MBDone = new System.Windows.Forms.Label();
            this.buttonQueueup = new System.Windows.Forms.Button();
            this.buttonQueuedown = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(581, 60);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.labelName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelProgress, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTimeleft, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelSpeed, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 6, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(569, 51);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(32, 19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(125, 13);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "Name";
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(163, 19);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(31, 13);
            this.labelProgress.TabIndex = 7;
            this.labelProgress.Text = "%";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(200, 14);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 2;
            // 
            // labelTimeleft
            // 
            this.labelTimeleft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeleft.AutoSize = true;
            this.labelTimeleft.Location = new System.Drawing.Point(306, 19);
            this.labelTimeleft.Name = "labelTimeleft";
            this.labelTimeleft.Size = new System.Drawing.Size(56, 13);
            this.labelTimeleft.TabIndex = 4;
            this.labelTimeleft.Text = "TimeLeft";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonQueueup);
            this.panel1.Controls.Add(this.buttonQueuedown);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(23, 45);
            this.panel1.TabIndex = 8;
            // 
            // labelSpeed
            // 
            this.labelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(368, 19);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(53, 13);
            this.labelSpeed.TabIndex = 10;
            this.labelSpeed.Text = "0 Kb/s";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.MBDone);
            this.panel2.Controls.Add(this.labelMb);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(427, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(79, 45);
            this.panel2.TabIndex = 11;
            // 
            // labelMb
            // 
            this.labelMb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMb.AutoSize = true;
            this.labelMb.Location = new System.Drawing.Point(32, 26);
            this.labelMb.Name = "labelMb";
            this.labelMb.Size = new System.Drawing.Size(44, 13);
            this.labelMb.TabIndex = 7;
            this.labelMb.Text = "MB Left";
            // 
            // MBDone
            // 
            this.MBDone.AutoSize = true;
            this.MBDone.Location = new System.Drawing.Point(3, 4);
            this.MBDone.Name = "MBDone";
            this.MBDone.Size = new System.Drawing.Size(23, 13);
            this.MBDone.TabIndex = 8;
            this.MBDone.Text = "MB";
            // 
            // buttonQueueup
            // 
            this.buttonQueueup.Image = global::NZBHags.Properties.Resources.arrow_up;
            this.buttonQueueup.Location = new System.Drawing.Point(1, -1);
            this.buttonQueueup.Name = "buttonQueueup";
            this.buttonQueueup.Size = new System.Drawing.Size(22, 23);
            this.buttonQueueup.TabIndex = 0;
            this.buttonQueueup.UseVisualStyleBackColor = true;
            this.buttonQueueup.Click += new System.EventHandler(this.buttonQueueup_Click);
            // 
            // buttonQueuedown
            // 
            this.buttonQueuedown.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonQueuedown.FlatAppearance.BorderSize = 0;
            this.buttonQueuedown.Image = global::NZBHags.Properties.Resources.arrow_down;
            this.buttonQueuedown.Location = new System.Drawing.Point(1, 21);
            this.buttonQueuedown.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQueuedown.Name = "buttonQueuedown";
            this.buttonQueuedown.Size = new System.Drawing.Size(22, 23);
            this.buttonQueuedown.TabIndex = 1;
            this.buttonQueuedown.UseVisualStyleBackColor = true;
            this.buttonQueuedown.Click += new System.EventHandler(this.buttonQueuedown_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = global::NZBHags.Properties.Resources.remove;
            this.button1.Location = new System.Drawing.Point(542, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Image = global::NZBHags.Properties.Resources.pause;
            this.button2.Location = new System.Drawing.Point(512, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 24);
            this.button2.TabIndex = 9;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::NZBHags.Properties.Resources.Untitled_11;
            this.label1.Location = new System.Drawing.Point(-1, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "          ";
            // 
            // QueueControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(400, 0);
            this.Name = "QueueControl";
            this.Size = new System.Drawing.Size(584, 63);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonQueueup;
        private System.Windows.Forms.Button buttonQueuedown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelTimeleft;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label MBDone;
        private System.Windows.Forms.Label labelMb;
        private System.Windows.Forms.Label label1;
    }
}
