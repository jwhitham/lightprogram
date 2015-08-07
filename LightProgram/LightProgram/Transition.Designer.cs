namespace LightProgram
{
    partial class Transition
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.redBar = new System.Windows.Forms.TrackBar();
            this.redLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.greenBar = new System.Windows.Forms.TrackBar();
            this.greenLabel = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.blueBar = new System.Windows.Forms.TrackBar();
            this.blueLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timeBar = new System.Windows.Forms.TrackBar();
            this.timeLabel = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15, 7, 7, 7);
            this.groupBox1.Size = new System.Drawing.Size(408, 151);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transition to Colour";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.redBar);
            this.flowLayoutPanel1.Controls.Add(this.redLabel);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.greenBar);
            this.flowLayoutPanel1.Controls.Add(this.greenLabel);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.blueBar);
            this.flowLayoutPanel1.Controls.Add(this.blueLabel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(386, 124);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Red;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(35, 35);
            this.panel3.TabIndex = 0;
            // 
            // redBar
            // 
            this.redBar.AccessibleDescription = "Red";
            this.redBar.AccessibleName = "Red";
            this.redBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.redBar.BackColor = System.Drawing.SystemColors.Control;
            this.redBar.LargeChange = 16;
            this.redBar.Location = new System.Drawing.Point(44, 3);
            this.redBar.Maximum = 255;
            this.redBar.Name = "redBar";
            this.redBar.Size = new System.Drawing.Size(298, 35);
            this.redBar.TabIndex = 0;
            this.redBar.Scroll += new System.EventHandler(this.redChanged);
            // 
            // redLabel
            // 
            this.redLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.redLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redLabel.Location = new System.Drawing.Point(348, 0);
            this.redLabel.Name = "redLabel";
            this.redLabel.Size = new System.Drawing.Size(35, 41);
            this.redLabel.TabIndex = 5;
            this.redLabel.Text = "FF";
            this.redLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel4.Location = new System.Drawing.Point(3, 44);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(35, 35);
            this.panel4.TabIndex = 3;
            // 
            // greenBar
            // 
            this.greenBar.AccessibleDescription = "Green";
            this.greenBar.AccessibleName = "Green";
            this.greenBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.greenBar.BackColor = System.Drawing.SystemColors.Control;
            this.greenBar.LargeChange = 16;
            this.greenBar.Location = new System.Drawing.Point(44, 44);
            this.greenBar.Maximum = 255;
            this.greenBar.Name = "greenBar";
            this.greenBar.Size = new System.Drawing.Size(298, 35);
            this.greenBar.TabIndex = 1;
            this.greenBar.Scroll += new System.EventHandler(this.greenChanged);
            // 
            // greenLabel
            // 
            this.greenLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.greenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.greenLabel.Location = new System.Drawing.Point(348, 41);
            this.greenLabel.Name = "greenLabel";
            this.greenLabel.Size = new System.Drawing.Size(35, 41);
            this.greenLabel.TabIndex = 6;
            this.greenLabel.Text = "FF";
            this.greenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Blue;
            this.panel5.Location = new System.Drawing.Point(3, 85);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(35, 35);
            this.panel5.TabIndex = 4;
            // 
            // blueBar
            // 
            this.blueBar.AccessibleDescription = "Blue";
            this.blueBar.AccessibleName = "Blue";
            this.blueBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blueBar.BackColor = System.Drawing.SystemColors.Control;
            this.blueBar.LargeChange = 16;
            this.blueBar.Location = new System.Drawing.Point(44, 85);
            this.blueBar.Maximum = 255;
            this.blueBar.Name = "blueBar";
            this.blueBar.Size = new System.Drawing.Size(298, 35);
            this.blueBar.TabIndex = 2;
            this.blueBar.Scroll += new System.EventHandler(this.blueChanged);
            // 
            // blueLabel
            // 
            this.blueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blueLabel.Location = new System.Drawing.Point(348, 82);
            this.blueLabel.Name = "blueLabel";
            this.blueLabel.Size = new System.Drawing.Size(35, 41);
            this.blueLabel.TabIndex = 7;
            this.blueLabel.Text = "FF";
            this.blueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanel2);
            this.groupBox2.Location = new System.Drawing.Point(12, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(15, 7, 7, 7);
            this.groupBox2.Size = new System.Drawing.Size(408, 71);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transition Time";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.panel1);
            this.flowLayoutPanel2.Controls.Add(this.timeBar);
            this.flowLayoutPanel2.Controls.Add(this.timeLabel);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(15, 20);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(386, 44);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 35);
            this.panel1.TabIndex = 0;
            // 
            // timeBar
            // 
            this.timeBar.AccessibleDescription = "Red";
            this.timeBar.AccessibleName = "Red";
            this.timeBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeBar.BackColor = System.Drawing.SystemColors.Control;
            this.timeBar.LargeChange = 10;
            this.timeBar.Location = new System.Drawing.Point(14, 3);
            this.timeBar.Maximum = 999;
            this.timeBar.Name = "timeBar";
            this.timeBar.Size = new System.Drawing.Size(273, 35);
            this.timeBar.TabIndex = 0;
            this.timeBar.Scroll += new System.EventHandler(this.timeChanged);
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.Location = new System.Drawing.Point(293, 0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(90, 41);
            this.timeLabel.TabIndex = 5;
            this.timeLabel.Text = "65.535s";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.timeLabel.Click += new System.EventHandler(this.timeLabel_Click);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(264, 246);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 6;
            this.ok.Text = "Ok";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.okButton);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(345, 246);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.cancelButton);
            // 
            // Transition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 280);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(447, 316);
            this.MinimumSize = new System.Drawing.Size(447, 316);
            this.Name = "Transition";
            this.Text = "Light Program - Transition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeButton);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TrackBar redBar;
        private System.Windows.Forms.Label redLabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TrackBar greenBar;
        private System.Windows.Forms.Label greenLabel;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TrackBar blueBar;
        private System.Windows.Forms.Label blueLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar timeBar;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button button2;
    }
}