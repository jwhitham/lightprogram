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
    
    public partial class LightChooser : Form
    {
        private Comms comms = null;
        private ProgramEditor programEditor = null;

        public class ProgramListItem
        {
            public int program_number = 0;
            public InstructionList inst_list = null;

            public ProgramListItem(int program_number, byte[] program_bytes)
            {
                this.program_number = program_number;
                updateProgram(program_bytes);
            }
            
            public void updateProgram(byte[] program_bytes)
            {
                this.inst_list = new InstructionList(program_bytes);
            }

            public string Text { get; set; }

            public override string ToString()
            {
                if (this.inst_list.contents.Count == 0)
                {
                    return "Program " + program_number + " is empty";
                }
                else
                {
                    return "Program " + program_number + ": '" + this.inst_list.name
                       + "' (" + this.inst_list.getProperties() + ")";
                }
            }
        };

        private Dictionary<int, ProgramListItem> programs = null;
        private ConnectionSetup previous = null;

        public LightChooser(Comms comms, ConnectionSetup previous)
        {
            this.comms = comms;
            this.previous = previous;
            this.programs = new Dictionary<int, ProgramListItem> ();
            this.programEditor = new ProgramEditor(this.comms, this);
            InitializeComponent();
            this.programList.FormattingEnabled = false;
        }

        private void barChanged()
        {
            this.comms.SetColour(this.redBar.Value, this.greenBar.Value, this.blueBar.Value);
            this.redLabel.Text = String.Format("{0:X02}", this.redBar.Value);
            this.greenLabel.Text = String.Format("{0:X02}", this.greenBar.Value);
            this.blueLabel.Text = String.Format("{0:X02}", this.blueBar.Value);
        }

        public void SetColour(int red, int green, int blue)
        {
            this.redBar.Value = red;
            this.greenBar.Value = green;
            this.blueBar.Value = blue;
            barChanged();
        }

        public void SetProgram(int program_number, byte[] program_bytes)
        {
            ProgramListItem p;

            if (this.programs.ContainsKey(program_number))
            {
                // updating a program
                p = this.programs[program_number];
                p.updateProgram (program_bytes);
                this.programList.Items.RemoveAt(program_number);
                this.programList.Items.Insert(program_number, p);
            }
            else
            {
                // adding a new program
                p = new ProgramListItem(program_number, program_bytes);
                this.programList.Items.Add(p);
                this.programs.Add(program_number, p);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void closeClicked(object sender, FormClosedEventArgs e)
        {
            this.previous.Disconnect();
        }

        private void disconnectClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void runProgram(object sender, EventArgs e)
        {
            ProgramListItem p = (ProgramListItem) this.programList.SelectedItem;
            if (p == null)
            {
                return;
            }
            this.comms.RunProgram(p.program_number);
        }

        private void editProgram(object sender, EventArgs e)
        {
            ProgramListItem p = (ProgramListItem)this.programList.SelectedItem;
            if (p == null)
            {
                return;
            }
            this.programEditor.SetProgram(p.inst_list, p.program_number);
            this.programEditor.Show();
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

        private void applyClicked(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void redLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
