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

        public void SetProgram(InstructionList programList)
        {
            this.instructions.Items.Clear();
            foreach (Instruction inst in programList.contents)
            {
                this.instructions.Items.Add(new InstructionEditor (inst));
            }
            this.instructions.Refresh();
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

        private void runClicked(object sender, EventArgs e)
        {
            InstructionList instruction_list = new InstructionList();
            foreach (InstructionEditor inst_ed in this.instructions.Items)
            {
                instruction_list.contents.Add(inst_ed.inst);
            }
            Command c = new Command();
            c.t = CommandType.CommandRunTemporaryProgram;
            c.program_bytes = new byte[Comms.program_size];
            int rc = instruction_list.encode(c.program_bytes);
            instruction_list.updateTimings();
            if (rc == InstructionList.program_too_big)
            {
                MessageBox.Show("This program is too large: there are too many instructions.");
            }
            else if (rc <= 0)
            {
                MessageBox.Show("This program contains no instructions: add at least one.");
            }
            else if (instruction_list.end_time <= 0)
            {
                MessageBox.Show("This program has no running time. It must contain some delays.");
            }
            else
            {
                this.comms.SendCommand(c);
            }
        }

        private void ProgramEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
