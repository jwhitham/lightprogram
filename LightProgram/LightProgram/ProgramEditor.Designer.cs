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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.edit = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.move_down = new System.Windows.Forms.Button();
            this.move_up = new System.Windows.Forms.Button();
            this.add_display = new System.Windows.Forms.Button();
            this.add_transition = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.instructions);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 348);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instructions";
            // 
            // instructions
            // 
            this.instructions.AllowDrop = true;
            this.instructions.FormattingEnabled = true;
            this.instructions.Location = new System.Drawing.Point(7, 19);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(286, 316);
            this.instructions.TabIndex = 0;
            this.instructions.SelectedIndexChanged += new System.EventHandler(this.instructions_SelectedIndexChanged);
            this.instructions.DragDrop += new System.Windows.Forms.DragEventHandler(this.instructionsDragDrop);
            this.instructions.DragOver += new System.Windows.Forms.DragEventHandler(this.instructionsDragOver);
            this.instructions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.instructions_MouseDoubleClick);
            this.instructions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.instructionsMouseDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 418);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test Program";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.runClicked);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(247, 418);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Save to Device";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.saveClicked);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(364, 418);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.doneClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namebox);
            this.groupBox2.Location = new System.Drawing.Point(317, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(122, 49);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program Name";
            // 
            // namebox
            // 
            this.namebox.Location = new System.Drawing.Point(6, 20);
            this.namebox.Name = "namebox";
            this.namebox.Size = new System.Drawing.Size(85, 20);
            this.namebox.TabIndex = 0;
            this.namebox.TextChanged += new System.EventHandler(this.namebox_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.properties);
            this.groupBox3.Location = new System.Drawing.Point(12, 366);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(427, 46);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.edit);
            this.groupBox4.Controls.Add(this.delete);
            this.groupBox4.Controls.Add(this.move_down);
            this.groupBox4.Controls.Add(this.move_up);
            this.groupBox4.Controls.Add(this.add_display);
            this.groupBox4.Controls.Add(this.add_transition);
            this.groupBox4.Location = new System.Drawing.Point(317, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(122, 202);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Edit Controls";
            // 
            // edit
            // 
            this.edit.Location = new System.Drawing.Point(6, 139);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(107, 23);
            this.edit.TabIndex = 5;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.editClicked);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(6, 168);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(107, 23);
            this.delete.TabIndex = 4;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.deleteButton);
            // 
            // move_down
            // 
            this.move_down.Location = new System.Drawing.Point(6, 109);
            this.move_down.Name = "move_down";
            this.move_down.Size = new System.Drawing.Size(107, 23);
            this.move_down.TabIndex = 3;
            this.move_down.Text = "Move Down";
            this.move_down.UseVisualStyleBackColor = true;
            this.move_down.Click += new System.EventHandler(this.moveDownButton);
            // 
            // move_up
            // 
            this.move_up.Location = new System.Drawing.Point(6, 79);
            this.move_up.Name = "move_up";
            this.move_up.Size = new System.Drawing.Size(107, 23);
            this.move_up.TabIndex = 2;
            this.move_up.Text = "Move Up";
            this.move_up.UseVisualStyleBackColor = true;
            this.move_up.Click += new System.EventHandler(this.moveUpButton);
            // 
            // add_display
            // 
            this.add_display.Location = new System.Drawing.Point(6, 49);
            this.add_display.Name = "add_display";
            this.add_display.Size = new System.Drawing.Size(107, 23);
            this.add_display.TabIndex = 1;
            this.add_display.Text = "Add Set Display";
            this.add_display.UseVisualStyleBackColor = true;
            this.add_display.Click += new System.EventHandler(this.addDisplayButton);
            // 
            // add_transition
            // 
            this.add_transition.Location = new System.Drawing.Point(6, 19);
            this.add_transition.Name = "add_transition";
            this.add_transition.Size = new System.Drawing.Size(107, 23);
            this.add_transition.TabIndex = 0;
            this.add_transition.Text = "Add Transition";
            this.add_transition.UseVisualStyleBackColor = true;
            this.add_transition.Click += new System.EventHandler(this.addTransitionClicked);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.exportButton);
            this.groupBox5.Controls.Add(this.importButton);
            this.groupBox5.Location = new System.Drawing.Point(317, 276);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(122, 84);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Backup";
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(6, 49);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(107, 23);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "Export to File";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButtonClicked);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(6, 20);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(107, 23);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "Import from File";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButtonClicked);
            // 
            // ProgramEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 452);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(465, 560);
            this.MinimumSize = new System.Drawing.Size(465, 460);
            this.Name = "ProgramEditor";
            this.Text = "Light Program - Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeButton);
            this.Load += new System.EventHandler(this.ProgramEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox namebox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label properties;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button move_down;
        private System.Windows.Forms.Button move_up;
        private System.Windows.Forms.Button add_display;
        private System.Windows.Forms.Button add_transition;
        private System.Windows.Forms.ListBox instructions;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button importButton;
    }
}