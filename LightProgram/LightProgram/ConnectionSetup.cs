using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace LightProgram
{
    public partial class ConnectionSetup : Form
    {
        private Comms comms = null;
        private System.Windows.Forms.Timer timer = null;
        private List<string> ports = null;
        private LightChooser lightChooser = null;
        private Simulator simulator = null;
        public static string simulation = "Simulation";

        public ConnectionSetup()
        {
            InitializeComponent();

            this.ports = new List<string> ();
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Tick += new EventHandler(TimerTick);
            this.timer.Interval = 100;
            this.timer.Enabled = true;
            this.timer.Start();
            RefreshPorts();
            if (this.ports.Count != 0)
            {
                this.serialPortList.SelectedIndex = this.ports.Count - 1;
            }
            this.connectionInfo.ReadOnly = true;
            this.connectionInfo.Clear();
            this.AppendText("Light Program");
            this.simulator = new Simulator(this);
            this.lightChooser = new LightChooser(this);
            this.comms = new Comms();
            this.lightChooser.SetComms(this.comms);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!this.lightChooser.Visible)
            {
                RefreshPorts();
            }
            if (this.comms is SimulatorComms)
            {
                this.simulator.RefreshSimulation((SimulatorComms)this.comms);
            }
            RefreshText();
        }

        private void RefreshPorts()
        {
            // Get port names
            string[] origNewPorts = SerialPort.GetPortNames();
            string[] newPorts = new string[origNewPorts.Length + 1];

            newPorts[0] = simulation;
            for (int i = 0; i < origNewPorts.Length; i++)
            {
                newPorts[i + 1] = origNewPorts[i];
            }

            // remove old items
            int oldIndex = this.serialPortList.SelectedIndex;
            bool reselect = false;

            for (int i = this.ports.Count - 1; i >= 0; i--)
            {
                // test to see if this item still exists
                bool keep = false;
                string port = this.ports[i];
                for (int j = 0; j < newPorts.Length; j++)
                {
                    if (port == newPorts[j])
                    {
                        keep = true;
                        newPorts[j] = null;
                        break;
                    }
                }
                if (!keep)
                {
                    // remove from list control
                    this.serialPortList.Items.RemoveAt(i);
                    this.ports.RemoveAt(i);
                    if (oldIndex == i)
                    {
                        reselect = true;
                    }
                }
            }
            // add new items
            for (int j = 0; j < newPorts.Length; j++)
            {
                if (newPorts[j] != null)
                {
                    this.ports.Add(newPorts[j]);
                    this.serialPortList.Items.Add(newPorts[j]);
                }
            }
            if (reselect)
            {
                this.serialPortList.SelectedIndex = 0;
            }
        }
        private void RefreshText()
        {
            // update the text window
            while (this.comms != null)
            {
                Reply r = this.comms.GetReply();
                switch (r.t)
                {
                    case ReplyType.ReplyNone:
                        return;
                    case ReplyType.ReplyConnected:
                        // Connected
                        this.AppendText("Connected");
                        this.lightChooser.SetColour(r.red, r.green, r.blue);
                        this.lightChooser.Show();
                        break;
                    case ReplyType.ReplyGotColour:
                        // Colour update from Arduino
                        this.lightChooser.SetColour(r.red, r.green, r.blue);
                        break;
                    case ReplyType.ReplyError:
                        // Disconnected
                        this.AppendText(r.errorCode);
                        this.connectButton.Enabled = true;
                        this.lightChooser.Hide();
                        break;
                    case ReplyType.ReplyMsg:
                        // Some other message
                        this.AppendText(r.errorCode);
                        break;
                    case ReplyType.ReplyProgram:
                        // Program contents, downloaded from Arduino
                        if (this.lightChooser != null)
                        {
                            this.lightChooser.SetProgram(r.program_number, r.program_bytes);
                        }
                        break;
                }
            }
        }

        private void AppendText(string text)
        {
            if (this.connectionInfo.Text.Length != 0)
            {
                this.connectionInfo.AppendText("\n");
            }
            this.connectionInfo.AppendText(text);
            this.connectionInfo.SelectionStart = this.connectionInfo.Text.Length;
            this.connectionInfo.ScrollToCaret();
        }

        private void ConnectClick(object sender, EventArgs e)
        {
            Disconnect();
            int index = this.serialPortList.SelectedIndex;
            if ((index < 0) || (index >= this.ports.Count))
            {
                this.AppendText("No port selected");
                return;
            }
            string port = this.ports[index];
            this.AppendText("Connecting to " + port + "...");
            this.connectButton.Enabled = false;

            if (port == simulation)
            {
                this.comms = new SimulatorComms();
                this.simulator.Show();
            }
            else
            {
                this.comms = new SerialComms();
            }
            this.comms.Connect(port);
            this.lightChooser.SetComms(this.comms);

        }

        public void Disconnect(object sender = null, FormClosedEventArgs e = null)
        {
            this.lightChooser.Hide();
            this.connectButton.Enabled = true;
            this.simulator.Hide();
            this.comms = new Comms();
            this.lightChooser.SetComms(this.comms);
        }

        private void ExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void connectionInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
