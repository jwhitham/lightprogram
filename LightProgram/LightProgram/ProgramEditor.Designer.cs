namespace LightProgram
{
    partial class ProgramEditor
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
            this.instructions = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.namebox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.Label();
            this.verify = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.instructions);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 321);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instructions";
            // 
            // instructions
            // 
            this.instructions.FormattingEnabled = true;
            this.instructions.Location = new System.Drawing.Point(6, 19);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(405, 290);
            this.instructions.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Run...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.runClicked);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 395);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Save...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.saveClicked);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(355, 395);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Done";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.doneClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namebox);
            this.groupBox2.Location = new System.Drawing.Point(18, 340);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(102, 49);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Name";
            // 
            // namebox
            // 
            this.namebox.Location = new System.Drawing.Point(7, 20);
            this.namebox.Name = "namebox";
            this.namebox.Size = new System.Drawing.Size(84, 20);
            this.namebox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.verify);
            this.groupBox3.Controls.Add(this.properties);
            this.groupBox3.Location = new System.Drawing.Point(126, 340);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 49);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Properties";
            // 
            // properties
            // 
            this.properties.AutoSize = true;
            this.properties.Location = new System.Drawing.Point(7, 20);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(53, 13);
            this.properties.TabIndex = 0;
            this.properties.Text = "properties";
            // 
            // verify
            // 
            this.verify.Location = new System.Drawing.Point(246, 15);
            this.verify.Name = "verify";
            this.verify.Size = new System.Drawing.Size(51, 23);
            this.verify.TabIndex = 7;
            this.verify.Text = "Update";
            this.verify.UseVisualStyleBackColor = true;
            this.verify.Click += new System.EventHandler(this.updateClicked);
            // 
            // ProgramEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 427);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProgramEditor";
            this.Text = "Program Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.closeClicked);
            this.Load += new System.EventHandler(this.ProgramEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox instructions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox namebox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label properties;
        private System.Windows.Forms.Button verify;
    }
}