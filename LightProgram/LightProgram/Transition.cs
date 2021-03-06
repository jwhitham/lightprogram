﻿using System;
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
        private int time_value = 0;

        public Transition(ProgramEditor programEditor)
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
            this.redBar.Value = inst.r;
            this.greenBar.Value = inst.g;
            this.blueBar.Value = inst.b;
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
            this.inst.r = this.redBar.Value;
            this.inst.g = this.greenBar.Value;
            this.inst.b = this.blueBar.Value;
            this.inst.value = this.time_value;
            this.programEditor.refreshInstruction(this.inst);
            this.inst = null;
        }
    }
}
