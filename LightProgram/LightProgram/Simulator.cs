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

        public Simulator(SimulatorComms simulatorComms)
        {
            this.simulatorComms = simulatorComms;
            InitializeComponent();
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
                        this.light.BackColor = Color.FromArgb(c.red, c.green, c.blue);
                        break;
                    case CommandType.CommandSetDisplay:
                        if (c.value < 0x10)
                        {
                            this.display.Text = String.Format("{0:X01}", c.value);
                        }
                        else
                        {
                            this.display.Text = "";
                        }
                        break;
                    case CommandType.CommandExit:
                        this.Hide();
                        break;
                    case CommandType.CommandRunProgram:
                        break;
                }
            }
        }

        private void light_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
