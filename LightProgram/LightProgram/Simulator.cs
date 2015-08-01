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
    public partial class Simulator : Form
    {
        private SimulatorComms simulatorComms = null;

        public Simulator(SimulatorComms simulatorComms)
        {
            this.simulatorComms = simulatorComms;
            InitializeComponent();
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
                            r.program_bytes = new byte[Comms.program_size];
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
