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


    public partial class ProgramEditor : Form
    {
        private Comms comms = null;
        private LightChooser lightChooser = null;

        public class InstructionEditor
        {
            public Instruction inst = null;
 
            public InstructionEditor(Instruction inst)
            {
                this.inst = inst;
            }
            
            public string Text { get; set; }

            public override string ToString()
            {
                return inst.ToString();
            }
        };

        public ProgramEditor(Comms comms, LightChooser lightChooser)
        {
            this.comms = comms;
            this.lightChooser = lightChooser;
            InitializeComponent();
        }

        public void SetProgram(InstructionList inst_list)
        {
            // make a copy of the program
            byte[] tmp = new byte[Comms.program_size];
            inst_list.encode(tmp);
            InstructionList copy_inst_list = new InstructionList(tmp);

            // update the GUI with the details
            this.instructions.Items.Clear();
            foreach (Instruction inst in copy_inst_list.contents)
            {
                this.instructions.Items.Add(new InstructionEditor (inst));
            }
            this.instructions.Refresh();
            this.namebox.Text = copy_inst_list.name;
            this.properties.Text = copy_inst_list.getProperties();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        private void closeClicked(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        private void doneClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        public enum ModeType
        {
            RunMode, CheckMode, SaveMode
        }

        private void checkProgramThenDoSomething(ModeType mode)
        {
            InstructionList copy_inst_list = new InstructionList();
            foreach (InstructionEditor inst_ed in this.instructions.Items)
            {
                copy_inst_list.contents.Add(inst_ed.inst);
            }
            copy_inst_list.name = this.namebox.Text;

            // create a programming command
            Command c = new Command();
            c.program_bytes = new byte[Comms.program_size];

            // translate the program to bytes
            int rc = copy_inst_list.encode(c.program_bytes);
            copy_inst_list.updateProperties();
            string err = "";

            if (rc == InstructionList.program_too_big)
            {
                err = "This program is too large: there are too many instructions.";
            }
            else if (rc <= 0)
            {
                err = "This program contains no instructions: add at least one.";
            }
            else if (copy_inst_list.end_time <= 0)
            {
                err = "This program has no running time. It must contain some delays.";
            }

            SetProgram(copy_inst_list);
            switch (mode)
            {
                case ModeType.RunMode:
                    // Start running the program
                    if (err != "")
                    {
                        MessageBox.Show(err);
                    }
                    else
                    {
                        c.t = CommandType.CommandRunTemporaryProgram;
                        this.comms.SendCommand(c);
                    }
                    break;
                case ModeType.CheckMode:
                    // Just checking, no other action
                    if (err != "")
                    {
                        this.properties.Text = err;
                    }
                    break;
                case ModeType.SaveMode:
                    // Are you sure you want to save?
                    break;
            }
        }

        private void runClicked(object sender, EventArgs e)
        {
            checkProgramThenDoSomething(ModeType.RunMode);
        }

        private void updateClicked(object sender, EventArgs e)
        {
            checkProgramThenDoSomething(ModeType.CheckMode);
        }

        private void ProgramEditor_Load(object sender, EventArgs e)
        {

        }

        private void saveClicked(object sender, EventArgs e)
        {
            checkProgramThenDoSomething(ModeType.SaveMode);

        }
    }
}
