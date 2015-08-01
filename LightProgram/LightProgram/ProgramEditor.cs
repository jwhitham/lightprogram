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

    }
}
