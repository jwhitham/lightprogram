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
    public partial class Transition : Form
    {
        private ProgramEditor programEditor = null;
        private Instruction inst = null;
        private Comms comms = null;
        private static string one_ms = "x 1ms";
        private static string ten_ms = "x 10ms";
        private static string hund_ms = "x 100ms";
        private static string thou_ms  = "x 1s";

        public Transition(ProgramEditor programEditor)
        {
            this.programEditor = programEditor;
            this.comms = new Comms();
            InitializeComponent();
            this.units.Items.Add(one_ms);
            this.units.Items.Add(ten_ms);
            this.units.Items.Add(hund_ms);
            this.units.Items.Add(thou_ms);
        }

        public void SetInstruction(Instruction inst)
        {
            this.redBar.Value = inst.r;
            this.greenBar.Value = inst.g;
            this.blueBar.Value = inst.b;

            if (inst.value < 100)
            {
                this.timeBar.Value = inst.value;
                this.units.Text = one_ms;
            }
            else if (inst.value < 1000)
            {
                this.timeBar.Value = inst.value / 10;
                this.units.Text = ten_ms;
            }
            else if (inst.value < 10000)
            {
                this.timeBar.Value = inst.value / 100;
                this.units.Text = hund_ms;
            }
            else
            {
                this.timeBar.Value = inst.value / 1000;
                this.units.Text = thou_ms;
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
            this.comms.SetColour(this.redBar.Value, this.greenBar.Value, this.blueBar.Value);
            Command c = new Command();
            c.t = CommandType.CommandGetColour;
            this.comms.SendCommand(c);
            this.redLabel.Text = String.Format("{0:X02}", this.redBar.Value);
            this.greenLabel.Text = String.Format("{0:X02}", this.greenBar.Value);
            this.blueLabel.Text = String.Format("{0:X02}", this.blueBar.Value);
            this.timeLabel.Text = "" + this.timeBar.Value;
        }

        private void redChanged(object sender, EventArgs e)
        {
            barChanged();
        }

        private void greenChanged(object sender, EventArgs e)
        {
            barChanged();
        }

        private void blueChanged(object sender, EventArgs e)
        {
            barChanged();
        }

        private void timeChanged(object sender, EventArgs e)
        {
            barChanged();
        }

        private void cancelButton(object sender, EventArgs e)
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

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }

        private void okButton(object sender, EventArgs e)
        {
            this.Hide();
            if (this.inst == null)
            {
                return;
            }
            this.inst.r = this.redBar.Value;
            this.inst.g = this.greenBar.Value;
            this.inst.b = this.blueBar.Value;
            this.inst.value = this.timeBar.Value;
            if (this.units.Text == one_ms)
            {
                this.inst.value *= 1;
            }
            else if (this.units.Text == ten_ms)
            {
                this.inst.value *= 10;
            }
            else if (this.units.Text == hund_ms)
            {
                this.inst.value *= 100;
            }
            else
            {
                this.inst.value *= 1000;
            }
            if (this.inst.value >= 0x10000)
            {
                this.inst.value = 0xffff;
            }
            this.programEditor.refreshInstruction(this.inst);
            this.inst = null;
        }
    }
}
