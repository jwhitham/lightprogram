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
        private static int max_bar_value = 999;
        private static int min_nonzero_time_value = 9;
        private static int max_time_value = 0xffff;
        private int time_value = 0;
        private static double bar_constant =
            (max_bar_value - 1) / Math.Log(max_time_value - min_nonzero_time_value);

        public Transition(ProgramEditor programEditor)
        {
            this.programEditor = programEditor;
            this.comms = new Comms();
            InitializeComponent();
            this.timeBar.Minimum = 0;
            this.timeBar.Maximum = max_bar_value;
            this.Icon = Properties.Resources.rgb;
        }

        public void SetInstruction(Instruction inst)
        {
            this.redBar.Value = inst.r;
            this.greenBar.Value = inst.g;
            this.blueBar.Value = inst.b;

            if (inst.value < min_nonzero_time_value)
            {
                this.timeBar.Value = 0;
            }
            else
            {
                this.timeBar.Value = 1 + (int) Math.Floor(bar_constant * Math.Log(inst.value - min_nonzero_time_value));
            }

            this.time_value = inst.value;
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
            this.timeLabel.Text = String.Format("{0:D}.{1:D03}s", this.time_value / 1000, this.time_value % 1000);
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
            if (this.timeBar.Value <= 0)
            {
                this.time_value = 0;
            }
            else
            {
                this.time_value = min_nonzero_time_value + (int)Math.Floor(Math.Exp((this.timeBar.Value - 1) / bar_constant));
                if (this.time_value > max_time_value)
                {
                    this.time_value = max_time_value;
                }
            }
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
            this.inst.value = this.time_value;
            this.programEditor.refreshInstruction(this.inst);
            this.inst = null;
        }
    }
}
