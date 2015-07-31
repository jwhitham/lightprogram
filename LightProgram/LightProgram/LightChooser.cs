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
    
    public partial class LightChooser : Form
    {
        private SerialComms serialComms = null;
        private ProgramEditor programEditor = null;

        public class ProgramListItem
        {
            public int program_number = 0;
            public byte[] program_bytes = null;
            public InstructionList inst_list = null;
            public int num_insts = 0;
            public int num_bytes = 0;

            public ProgramListItem(int program_number, byte[] program_bytes)
            {
                this.program_number = program_number;
                updateProgram(program_bytes);
            }
            
            public void updateProgram(byte[] program_bytes)
            {
                this.program_bytes = program_bytes;
                this.inst_list = new InstructionList(program_bytes);
                byte[] check = new byte[SerialComms.program_size];
                this.num_bytes = this.inst_list.encode (check);
            }

            public string Text { get; set; }

            public override string ToString()
            {
                if (this.inst_list.contents.Count == 0)
                {
                    return "Program " + program_number + " is empty";
                }
                string last_eight = "";
                int j = program_bytes.Length - SerialComms.program_name_size;
                if (j > 1)
                {
                    for (int i = j; i < program_bytes.Length; i++)
                    {
                        int ch = (int)program_bytes[i];
                        if ((ch < 32) || (ch > 126))
                        {
                            ch = (int)'?';
                        }
                        last_eight += (char)((byte) ch);
                    }
                }
                return "Program " + program_number + ": '" + last_eight +
                    "' (" + this.inst_list.contents.Count + " instructions, " + this.num_bytes + " bytes)";
            }
        };

        private Dictionary<int, ProgramListItem> programs = null;

        public LightChooser(SerialComms serialComms)
        {
            this.serialComms = serialComms;
            this.programs = new Dictionary<int, ProgramListItem> ();
            this.programEditor = new ProgramEditor(serialComms, this);
            InitializeComponent();
            this.programList.FormattingEnabled = false;
        }

        private void barChanged()
        {
            this.serialComms.SetColour(this.redBar.Value, this.greenBar.Value, this.blueBar.Value);
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
                this.programList.Refresh();
            }
            else
            {
                // adding a new program
                p = new ProgramListItem(program_number, program_bytes);
                this.programList.Items.Add(p);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void closeClicked(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void disconnectClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void runProgram(object sender, EventArgs e)
        {
            ProgramListItem p = (ProgramListItem) this.programList.SelectedItem;
            if (p == null)
            {
                return;
            }
            this.serialComms.RunProgram(p.program_number);
        }

        private void editProgram(object sender, EventArgs e)
        {
            ProgramListItem p = (ProgramListItem)this.programList.SelectedItem;
            if (p == null)
            {
                return;
            }
            this.programEditor.SetProgram(p.inst_list);
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

    }
}
