using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LightProgram
{
    public partial class Simulator : Form
    {
        private SimulatorComms simulatorComms = null;
        public const string program_store = "simulated_eeprom_";
        private InstructionList simulated_program = null;
        private long start_time = 0;
        private int program_time = 0;
        private int program_counter = 0;
        private int r = 0;
        private int g = 0;
        private int b = 0;
        private ConnectionSetup previous = null;

        public Simulator(SimulatorComms simulatorComms, ConnectionSetup previous)
        {
            this.simulatorComms = simulatorComms;
            this.previous = previous;
            this.simulated_program = null;
            InitializeComponent();
            this.programState.Text = "";
        }

        private byte[] readProgram(int program_number)
        {
            string name = program_store + program_number;
            BinaryReader reader = null;
            byte[] output = null;

            try
            {
                reader = new BinaryReader(File.Open(name, FileMode.Open));
            }
            catch (Exception)
            { }
            try
            {
                output = reader.ReadBytes(Comms.program_size);
            }
            catch (Exception)
            { }
            try
            {
                reader.Close();
            }
            catch (Exception)
            { }
            if ((output == null) || (output.Length != Comms.program_size))
            {
                output = new byte[Comms.program_size];
                writeProgram(program_number, output);
            }
            return output;
        }

        private void writeProgram (int program_number, byte[] output)
        {
            string name = program_store + program_number;
            BinaryWriter writer = null;

            try
            {
                writer = new BinaryWriter(File.Open(name, FileMode.Create));
            }
            catch (Exception)
            { }
            try
            {
                writer.Write(output);
            }
            catch (Exception)
            { }
            try
            {
                writer.Close();
            }
            catch (Exception)
            { }
        }

        public void RefreshSimulation()
        {
            if (this.simulatorComms == null) return;

            if (this.simulated_program != null)
            {
                long ticks = DateTime.Now.Ticks - this.start_time;
                long milliseconds = ticks / TimeSpan.TicksPerMillisecond;
                int sim_time = (int)milliseconds;
                int size = this.simulated_program.contents.Count();

                if ((this.program_time + 120000) < sim_time)
                {
                    // clock skew - reset the program
                    this.program_time = 0;
                    this.start_time = DateTime.Now.Ticks;
                    this.program_counter = 0;
                }

                while (this.program_time < sim_time)
                {
                    // run forwards until the program time matches the simulator time
                    if (this.program_counter >= size)
                    {
                        this.program_counter = 0; // loop at end of program
                    }
                    Instruction inst = this.simulated_program.contents[this.program_counter];
                    int end_time = inst.getTime() + this.program_time;
                    bool commit = true;

                    switch (inst.t)
                    {
                        case InstructionType.InstructionSetDisplay:
                            this.setDisplay(inst.value);
                            break;
                        case InstructionType.InstructionTransition:
                            if (end_time < sim_time)
                            {
                                // Running behind. Apply this instruction immediately.
                                this.r = inst.r;
                                this.g = inst.g;
                                this.b = inst.b;
                                this.setColour(this.r, this.g, this.b);
                            }
                            else
                            {
                                // Transition
                                int rS = this.r, gS = this.g, bS = this.b;
                                int rT = inst.r, gT = inst.g, bT = inst.b;
                                int tD = (int)(sim_time - this.program_time);
                                int tI = (int)(end_time - this.program_time);
                                byte r = linear(rS, rT, tD, tI);
                                byte g = linear(gS, gT, tD, tI);
                                byte b = linear(bS, bT, tD, tI);
                                this.setColour(r, g, b);
                                commit = false;
                            }
                            break;
                        default:
                            break;
                    }
                    if (commit)
                    {
                        // Go to next instruction
                        this.program_counter++;
                        this.program_time = end_time;
                    }
                    else
                    {
                        // Need to wait for this instruction to complete
                        this.programState.Text = "Inst " + (this.program_counter + 1) + " of " + size + ": "
                            + inst.ToString();
                        break;
                    }
                }
            }

            while (true)
            {
                Command c = this.simulatorComms.SimulatorGetCommand();
                Reply r;
                switch (c.t)
                {
                    case CommandType.CommandNone:
                        return;
                    case CommandType.CommandConnect:
                        r = new Reply();
                        r.red = r.green = r.blue = 0x20;
                        r.t = ReplyType.ReplyConnected;
                        this.simulatorComms.SimulatorSendReply(r);
                        for (int i = 0; i < Comms.num_programs; i++)
                        {
                            r = new Reply();
                            r.program_number = i;
                            r.program_bytes = readProgram(i);

                            r.t = ReplyType.ReplyProgram;
                            this.simulatorComms.SimulatorSendReply(r);
                        }

                        r = new Reply();
                        r.errorCode = "Simulated mode";
                        r.t = ReplyType.ReplyMsg;
                        this.simulatorComms.SimulatorSendReply(r);
                        this.display.Text = "C";
                        this.Show();
                        break;
                    case CommandType.CommandSetColour:
                        this.simulated_program = null;
                        this.setColour(c.red, c.green, c.blue);
                        this.programState.Text = "";
                        break;
                    case CommandType.CommandSetDisplay:
                        this.simulated_program = null;
                        setDisplay(c.value);
                        this.programState.Text = "";
                        break;
                    case CommandType.CommandExit:
                        this.simulated_program = null;
                        break;
                    case CommandType.CommandRunEEPROMProgram:
                        this.simulated_program = new InstructionList(readProgram(c.value));
                        startProgram("" + c.value);
                        break;
                    case CommandType.CommandRunTemporaryProgram:
                        this.simulated_program = new InstructionList(c.program_bytes);
                        startProgram("(temporary)");
                        break;
                }
            }
        }

        private void setDisplay(int v)
        {
            if ((0 <= v) && (v < 0x10))
            {
                this.display.Text = String.Format("{0:X01}", v);
            }
            else
            {
                this.display.Text = "";
            }
        }

        private void setColour(int r, int g, int b)
        {
            this.light.BackColor = Color.FromArgb(r, g, b);
        }

        private void startProgram(string name)
        {
            Reply r;
            this.start_time = DateTime.Now.Ticks;
            this.program_time = 0;
            this.program_counter = 0;
            if (this.simulated_program.contents.Count() == 0)
            {
                r = new Reply();
                r.errorCode = "Program " + name + " contains no instructions.";
                r.t = ReplyType.ReplyMsg;
                this.simulatorComms.SimulatorSendReply(r);
                this.simulated_program = null;
            }
            else if (this.simulated_program.end_time <= 0)
            {
                r = new Reply();
                r.errorCode = "Program " + name + " has no running time.";
                r.t = ReplyType.ReplyMsg;
                this.simulatorComms.SimulatorSendReply(r);
                this.simulated_program = null;
            }
            else
            {
                r = new Reply();
                r.errorCode = "Program " + name + " simulating...";
                r.t = ReplyType.ReplyMsg;
                this.simulatorComms.SimulatorSendReply(r);
            }
            this.programState.Text = r.errorCode;
        }


        private byte linear(int s, int t, int p, int q)
        {
            long output = 0;
            if (q <= 0)
            {
                // For instant transitions
                output = t;
            } else
            {
                // clamped to 0 and 255
                output = s + (((t - s) * p) / q);
            }

            if (output >= 255)
            {
                return 255;
            }
            else if (output <= 0)
            {
                return 0;
            }
            else
            {
                return (byte) output;
            }
        }

        private void closeClicked(object sender, FormClosedEventArgs e)
        {
            this.previous.Disconnect();
        }

        private void light_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
