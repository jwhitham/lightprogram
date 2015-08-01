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
            this.display = new System.Windows.Forms.Label();
            this.light = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.BackColor = System.Drawing.Color.Black;
            this.display.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display.ForeColor = System.Drawing.Color.Red;
            this.display.Location = new System.Drawing.Point(96, 9);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(57, 56);
            this.display.TabIndex = 6;
            this.display.Text = "F";
            this.display.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // light
            // 
            this.light.BackColor = System.Drawing.Color.Red;
            this.light.Location = new System.Drawing.Point(12, 9);
            this.light.Name = "light";
            this.light.Size = new System.Drawing.Size(78, 78);
            this.light.TabIndex = 7;
            this.light.Paint += new System.Windows.Forms.PaintEventHandler(this.light_Paint);
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(165, 96);
            this.Controls.Add(this.light);
            this.Controls.Add(this.display);
            this.Name = "Simulator";
            this.Text = "Simulator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label display;
        private System.Windows.Forms.Panel light;
    }
}