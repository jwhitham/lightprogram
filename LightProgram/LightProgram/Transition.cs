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

        public Transition(ProgramEditor programEditor)
        {
            this.programEditor = programEditor;
            this.comms = new Comms();
            InitializeComponent();
        }

        public void SetInstruction(Instruction inst)
        {
            this.redBar.Value = inst.r;
            this.greenBar.Value = inst.g;
            this.blueBar.Value = inst.b;
            this.timeBar.Value = inst.value;
            this.inst = inst;
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
            e.Cancel = true;
        }
    }
}
