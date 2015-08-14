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
    public partial class Wait : Form
    {
        private ProgramEditor programEditor = null;
        private Instruction inst = null;
        private Comms comms = null;
        private int time_value = 0;

        public Wait(ProgramEditor programEditor)
        {
            this.programEditor = programEditor;
            this.comms = new Comms();
            InitializeComponent();
            this.timeBar.Minimum = 0;
            this.timeBar.Maximum = 255;
            this.Icon = Properties.Resources.rgb;
        }

        public void SetInstruction(Instruction inst)
        {
            this.timeBar.Value = DelayTable.delayToIndex(inst.value);
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
            this.timeLabel.Text = String.Format("{0:D}.{1:D03}s", this.time_value / 1000, this.time_value % 1000);
        }

        private void timeChanged(object sender, EventArgs e)
        {
            this.time_value = DelayTable.indexToDelay((byte) this.timeBar.Value);
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
            this.inst.value = this.time_value;
            this.programEditor.refreshInstruction(this.inst);
            this.inst = null;
        }
    }
}
