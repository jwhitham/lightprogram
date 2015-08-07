namespace LightProgram
{
    partial class Simulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulator));
            this.display = new System.Windows.Forms.Label();
            this.light = new System.Windows.Forms.Panel();
            this.programState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.BackColor = System.Drawing.Color.Black;
            this.display.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display.ForeColor = System.Drawing.Color.Red;
            this.display.Location = new System.Drawing.Point(171, 9);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(35, 78);
            this.display.TabIndex = 6;
            this.display.Text = "F";
            this.display.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // light
            // 
            this.light.BackColor = System.Drawing.Color.Red;
            this.light.Location = new System.Drawing.Point(12, 9);
            this.light.Name = "light";
            this.light.Size = new System.Drawing.Size(153, 78);
            this.light.TabIndex = 7;
            this.light.Paint += new System.Windows.Forms.PaintEventHandler(this.light_Paint);
            // 
            // programState
            // 
            this.programState.AutoSize = true;
            this.programState.ForeColor = System.Drawing.Color.White;
            this.programState.Location = new System.Drawing.Point(13, 94);
            this.programState.Name = "programState";
            this.programState.Size = new System.Drawing.Size(35, 13);
            this.programState.TabIndex = 8;
            this.programState.Text = "label1";
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(218, 122);
            this.Controls.Add(this.programState);
            this.Controls.Add(this.light);
            this.Controls.Add(this.display);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(234, 158);
            this.MinimumSize = new System.Drawing.Size(234, 158);
            this.Name = "Simulator";
            this.Text = "Device Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeButton);
            this.Load += new System.EventHandler(this.Simulator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label display;
        private System.Windows.Forms.Panel light;
        private System.Windows.Forms.Label programState;
    }
}
