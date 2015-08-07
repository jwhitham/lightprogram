using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LightProgram
{
    public partial class SetDisplay : Form
    {
        private ProgramEditor programEditor = null;
        private Comms comms = null;
        private Instruction inst = null;

        public SetDisplay(ProgramEditor programEditor)
        {
            this.programEditor = programEditor;
            this.comms = new Comms();
            InitializeComponent();
            this.Icon = Properties.Resources.rgb;
        }

        public void SetInstruction(Instruction inst)
        {
            if ((0 <= inst.value) && (inst.value <= 16))
            {
                this.valueBar.Value = inst.value;
            }
            else
            {
                this.valueBar.Value = 16;
            }
            this.inst = inst;
            barChanged();
        }

        public void SetComms(Comms comms)
        {
            this.comms = comms;
        }

        private void barChanged()
        {
            this.comms.SetDisplay(this.valueBar.Value);
            if (this.valueBar.Value >= 0x10)
            {
                this.display.Text = " ";
            }
            else
            {
                this.display.Text = String.Format("{0:X01}", this.valueBar.Value);
            }
        }


        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.inst = null;
        }

        private void closeButton(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.inst = null;
            e.Cancel = true;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (this.inst == null)
            {
                return;
            }
            this.inst.value = this.valueBar.Value;
            this.programEditor.refreshInstruction(this.inst);
            this.inst = null;
        }

        private void valueBar_Scroll(object sender, EventArgs e)
        {
            barChanged();
        }

        private void SetDisplay_Load(object sender, EventArgs e)
        {

        }
    }
}
