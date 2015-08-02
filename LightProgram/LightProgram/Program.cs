using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.ComponentModel;

namespace LightProgram
{
    // Command and Reply data structures
    // These are used to send and receive messages from the Comms thread
    public enum CommandType
    {
        CommandNone, CommandConnect, CommandSetColour, CommandSetDisplay, CommandExit,
        CommandRunEEPROMProgram, CommandRunTemporaryProgram, CommandSaveEEPROMProgram,
        CommandGetColour
    }
    public struct Command
    {
        public CommandType t;
        public int red, green, blue;
        public int value;
        public string portName;
        public byte[] program_bytes;
        public int program_number;
    }
    public enum ReplyType
    {
        ReplyNone, ReplyConnected, ReplyError, ReplyMsg, ReplyProgram, ReplyGotColour
    }
    public struct Reply
    {
        public ReplyType t;
        public string errorCode;
        public int red, green, blue;
        public byte[] program_bytes;
        public int program_number;
    }

    // Communications components that are independent of the target
    public class Comms
    {
        // These constants are also found in the Arduino code
        public static int num_programs = 8;
        public static int program_size = 128;
        public static int program_name_size = 8;
        public static int max_program_size = program_size - program_name_size;


        public virtual Reply GetReply()
        {
            Reply r = new Reply();
            r.t = ReplyType.ReplyNone;
            return r;
        }

        public virtual void Disconnect()
        {
        }

        public virtual void SendCommand(Command c)
        {
        }

        public void Connect(String portName)
        {
            Command c = new Command();
            c.t = CommandType.CommandConnect;
            c.portName = portName;
            SendCommand(c);
        }

        public void RunProgram(int program_number)
        {
            Command c = new Command();
            c.t = CommandType.CommandRunEEPROMProgram;
            c.value = program_number;
            SendCommand(c);
        }

        public void SetColour(int r, int g, int b)
        {
            Command c = new Command();
            c.t = CommandType.CommandSetColour;
            c.red = r;
            c.green = g;
            c.blue = b;
            SendCommand(c);
        }

        public void SetDisplay(int value)
        {
            Command c = new Command();
            c.t = CommandType.CommandSetDisplay;
            c.value = value;
            SendCommand(c);
        }
    }

    // Communication from programming GUI to simulator GUI
    public class SimulatorComms : Comms
    {
        private LinkedList<Command> commands;
        private LinkedList<Reply> replies;

        public SimulatorComms()
        {
            this.commands = new LinkedList<Command>();
            this.replies = new LinkedList<Reply>();
        }

        public override Reply GetReply()
        {
            Reply r;
            if (this.replies.Count != 0)
            {
                r = this.replies.First();
                this.replies.RemoveFirst();
            }
            else
            {
                r = new Reply();
                r.t = ReplyType.ReplyNone;
            }
            return r;
        }

        public override void Disconnect()
        {
        }

        public override void SendCommand(Command c)
        {
            this.commands.AddLast(c);
        }

        public Command SimulatorGetCommand()
        {
            Command c;
            if (this.commands.Count != 0)
            {
                c = this.commands.First();
                this.commands.RemoveFirst();
            }
            else
            {
                c = new Command();
                c.t = CommandType.CommandNone;
            }
            return c;
        }

        public void SimulatorSendReply(Reply r)
        {
            this.replies.AddLast(r);
        }
    }

    // Communication via serial
    public class SerialComms : Comms
    {
        private LinkedList<Command> commands;
        private LinkedList<Reply> replies;
        private Thread threadHandle;
        private SerialPort serialHandle;
        public static int timeout = 250;

        public SerialComms()
        {
            this.commands = new LinkedList<Command>();
            this.replies = new LinkedList<Reply>();
            this.threadHandle = new Thread(new ThreadStart(Worker));
            this.threadHandle.IsBackground = true;
            this.threadHandle.Start();
            while (!this.threadHandle.IsAlive) ;
        }

        public override void SendCommand (Command c)
        {
            lock (this)
            {
                this.commands.AddLast (c);
                Monitor.Pulse (this);
            }
        }

        public override Reply GetReply()
        {
            Reply r;
            lock (this)
            {
                if (this.replies.Count != 0)
                {
                    r = this.replies.First();
                    this.replies.RemoveFirst();
                }
                else
                {
                    r = new Reply();
                    r.t = ReplyType.ReplyNone;
                }
            }
            return r;
        }

        public override void Disconnect()
        {
            if (this.threadHandle == null)
            {
                return;
            }
            Command c = new Command();
            c.t = CommandType.CommandExit;
            SendCommand(c);
            this.threadHandle.Join();
            this.threadHandle = null;
        }

        private void Worker()
        {
            Action<Reply> SendReply = (Reply r) =>
            {
                lock (this)
                {
                    this.replies.AddLast(r);
                }
            };
            Action CloseSerial = () =>
            {
                try
                {
                    if ((this.serialHandle != null) && this.serialHandle.IsOpen)
                    {
                        this.serialHandle.Close();
                    }
                }
                catch (Exception)
                {
                }
                this.serialHandle = null;
            };
            Action<string> ConnectionError = (string s) =>
            {
                CloseSerial();
                Reply r = new Reply();
                r.t = ReplyType.ReplyError;
                r.errorCode = s;
                SendReply(r);
            };
            Action<byte[], int> SendSerialCommand = (byte[] data, int size) =>
            {
                if (this.serialHandle == null)
                {
                    return;
                }
                try
                {
                    this.serialHandle.Write(data, 0, size);
                    int ok = this.serialHandle.ReadByte();
                    if (ok != 'K')
                    {
                        ConnectionError("Protocol error when sending command " + data[0] + ", got " + ok);
                    }
                }
                catch (System.TimeoutException)
                {
                    ConnectionError("Timeout error when sending command " + data[0]);
                }
                catch (Exception e)
                {
                    ConnectionError("Communications error when sending command " + data[0] + ": " + e.Message);
                }
            };
            Action<byte[], int> ReadSerialBytes = (byte[] data, int size) =>
            {
                if (this.serialHandle == null)
                {
                    return;
                }
                try
                {
                    int count = 0, offset = 0;

                    while (count < size)
                    {
                        int now = this.serialHandle.Read(data, offset, size - count);
                        if (now <= 0)
                        {
                            ConnectionError("Protocol error when reading " + size + " bytes: got " + count + " and " + now);
                        }
                        count += now;
                        offset += now;
                    }
                }
                catch (System.TimeoutException)
                {
                    ConnectionError("Timeout error when reading " + size + " bytes");
                }
                catch (Exception e)
                {
                    ConnectionError("Communications error when reading " + size + " bytes: " + e.Message);
                }
            };
            Action<int> SendDisplay = (int d) =>
            {
                byte[] b = new byte[2];
                b[0] = (byte)'D';
                b[1] = (byte)d;
                SendSerialCommand(b, 2);
            };
            Action PostConnection = () =>
            {
                byte[] bytesOut = new byte[16];
                byte[] bytesIn = new byte[16];

                // Activities carried out when a connection is established
                // set display to 0xc
                SendDisplay(0x10);
                SendDisplay(0xc);

                // request current colour
                Reply r = new Reply();
                bytesOut[0] = (byte)'c'; // request current colour
                SendSerialCommand(bytesOut, 1);
                if (this.serialHandle == null) return;
                ReadSerialBytes(bytesIn, 3);
                if (this.serialHandle == null) return;
                // Send "connected" message as connection seems ok
                r.red = (int)bytesIn[0];
                r.green = (int)bytesIn[1];
                r.blue = (int)bytesIn[2];
                r.t = ReplyType.ReplyConnected;
                SendReply(r);

                // Download programs
                for (int i = 0; i < num_programs; i++)
                {
                    bytesOut[0] = (byte)'L'; // load program from EEPROM
                    bytesOut[1] = (byte)i;
                    SendSerialCommand(bytesOut, 2);
                    if (this.serialHandle == null) return;
                    bytesOut[0] = (byte)'m'; // download program via serial line
                    SendSerialCommand(bytesOut, 1);
                    if (this.serialHandle == null) return;
                    r.program_bytes = new byte[program_size];
                    r.program_number = i;
                    ReadSerialBytes(r.program_bytes, program_size);
                    if (this.serialHandle == null) return;
                    r.t = ReplyType.ReplyProgram;
                    SendReply(r);
                }
            };

            Command colour = new Command();
            LinkedList<Command> sequential = new LinkedList<Command>();
            bool get_colour = false;
            while (true)
            {
                colour.t = CommandType.CommandNone;
                get_colour = false;
                sequential.Clear();

                // Await messages
                lock (this)
                {
                    if (commands.Count == 0)
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (Exception) { }
                    }
                    while (commands.Count != 0)
                    {
                        Command cmd = commands.First();
                        commands.RemoveFirst();
                        switch (cmd.t)
                        {
                            case CommandType.CommandSetColour:
                                // Only keep the most recent colour command
                                colour = cmd;
                                break;
                            case CommandType.CommandGetColour:
                                // Done after SetColour
                                get_colour = true;
                                break;
                            case CommandType.CommandExit:
                                // exit takes priority over everything else: immediate exit
                                CloseSerial();
                                return;
                            default:
                                // All other commands are processed in sequence
                                sequential.AddLast (cmd);
                                break;
                        }
                    }
                }
                // Process commands apart from colour
                while (sequential.Count != 0)
                {
                    Command cmd = sequential.First();
                    sequential.RemoveFirst();

                    switch (cmd.t)
                    {
                        case CommandType.CommandConnect:
                            CloseSerial();
                            try
                            {
                                this.serialHandle = new SerialPort(cmd.portName, 19200);
                                this.serialHandle.ReadTimeout = timeout;
                                this.serialHandle.WriteTimeout = timeout;
                                this.serialHandle.Open();
                            }
                            catch (Exception e)
                            {
                                ConnectionError("Connection to " + cmd.portName + " failed with error: " + e.Message);
                            }
                            if (this.serialHandle != null)
                            {
                                // send some '\0' bytes before starting it
                                const int leadin_size = 6;
                                byte[] initBytesOut = new byte[leadin_size];
                                initBytesOut[leadin_size - 1] = (byte)'Q';

                                int attempts = 3;
                                while (attempts > 0)
                                {
                                    try
                                    {
                                        this.serialHandle.Write(initBytesOut, 0, leadin_size);
                                    }
                                    catch (Exception)
                                    {
                                        ConnectionError("Write error during setup");
                                        break;
                                    }
                                    try
                                    {
                                        this.serialHandle.ReadTo("K");
                                        try
                                        {
                                            // This read is expected to timeout.
                                            // Just flushes extra stuff on the input
                                            this.serialHandle.ReadTo("Z");
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;
                                    }
                                    catch (TimeoutException)
                                    {
                                        // try again unless final attempt
                                        attempts--;
                                        if (attempts <= 0)
                                        {
                                            ConnectionError("Timeout during connection: no valid response");
                                            break;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ConnectionError("Read error during setup");
                                        break;
                                    }
                                }

                                PostConnection();
                            }
                            else
                            {
                                CloseSerial();
                            }
                            break;
                        case CommandType.CommandSetDisplay:
                            {
                                Reply r = new Reply();
                                r.t = ReplyType.ReplyMsg;
                                r.errorCode = "Setting display " + cmd.value;
                                SendReply(r);
                                SendDisplay(cmd.value);
                            }
                            break;
                        case CommandType.CommandRunEEPROMProgram:
                            {
                                byte[] bytesOut = new byte[2];
                                bytesOut[0] = (byte)'L'; // load from EEPROM
                                bytesOut[1] = (byte)cmd.value; // program number
                                SendSerialCommand(bytesOut, 2);
                                bytesOut[0] = (byte)'R'; // run
                                SendSerialCommand(bytesOut, 1);
                                Reply r = new Reply();
                                r.t = ReplyType.ReplyMsg;
                                r.errorCode = "Running program " + cmd.value;
                                SendReply(r);
                            }
                            break;
                        case CommandType.CommandRunTemporaryProgram:
                            {
                                byte[] bytesOut = new byte[Comms.program_size + 1];
                                bytesOut[0] = (byte)'m'; // receive program from PC
                                for (int i = 0; i < Comms.program_size; i++)
                                {
                                    bytesOut[i + 1] = cmd.program_bytes[i];
                                }
                                SendSerialCommand(bytesOut, Comms.program_size + 1);
                                bytesOut[0] = (byte)'R'; // run
                                SendSerialCommand(bytesOut, 1);
                                Reply r = new Reply();
                                r.t = ReplyType.ReplyMsg;
                                r.errorCode = "Running program from editor";
                                SendReply(r);
                            }
                            break;
                        case CommandType.CommandSaveEEPROMProgram:
                            {
                                Reply r = new Reply();
                                r.t = ReplyType.ReplyMsg;
                                r.errorCode = "Writing program " + cmd.program_number + " to EEPROM";
                                SendReply(r);

                                byte[] bytesOut = new byte[Comms.program_size + 1];
                                bytesOut[0] = (byte)'M'; // upload program via serial line (PC to Arduino)
                                for (int i = 0; i < Comms.program_size; i++)
                                {
                                    bytesOut[i + 1] = cmd.program_bytes[i];
                                }
                                SendSerialCommand(bytesOut, Comms.program_size + 1);
                                bytesOut[0] = (byte)'S'; // save to EEPROM
                                bytesOut[1] = (byte)cmd.program_number;
                                SendSerialCommand(bytesOut, 2);
                                bytesOut[0] = (byte)'L'; // load program from EEPROM (readback)
                                bytesOut[1] = (byte)cmd.program_number;
                                SendSerialCommand(bytesOut, 2);
                                bytesOut[0] = (byte)'m'; // download program via serial line (readback)
                                SendSerialCommand(bytesOut, 1);
                                if (this.serialHandle != null)
                                {
                                    r = new Reply();
                                    r.program_bytes = new byte[Comms.program_size];
                                    r.program_number = cmd.program_number;
                                    ReadSerialBytes(r.program_bytes, Comms.program_size);
                                    r.t = ReplyType.ReplyProgram;
                                    SendReply(r);
                                }
                                if (this.serialHandle != null)
                                {
                                    r = new Reply();
                                    r.t = ReplyType.ReplyMsg;
                                    r.errorCode = "Done";
                                    SendReply(r);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                // Process colour command
                if (colour.t != CommandType.CommandNone)
                {
                    byte[] b = new byte[4];
                    b[0] = (byte) 'C';
                    b[1] = (byte) colour.red;
                    b[2] = (byte) colour.green;
                    b[3] = (byte) colour.blue;
                    SendSerialCommand (b, 4);
                }
                if (get_colour)
                {
                    get_colour = false;
                    byte[] bytesOut = new byte[1];
                    bytesOut[0] = (byte)'c'; // request current colour
                    SendSerialCommand(bytesOut, 1);
                    byte[] bytesIn = new byte[3];
                    ReadSerialBytes(bytesIn, 3);
                    Reply r = new Reply();
                    r.t = ReplyType.ReplyProgram;
                    r.red = (int)bytesIn[0];
                    r.green = (int)bytesIn[1];
                    r.blue = (int)bytesIn[2];
                    r.t = ReplyType.ReplyGotColour;
                    SendReply(r);
                }
            }
        }
    }

    // These classes represent instructions and programs and can be used to
    // serialise/deserialise instructions into byte form
    public enum InstructionType
    {
        InstructionRestart, InstructionSetDisplay,
        InstructionTransition,
        InstructionIllegal
    };
    public class Instruction
    {
        public InstructionType t = InstructionType.InstructionIllegal;
        public int r = 0;
        public int g = 0;
        public int b = 0;
        public int value = 0;
        public int start_time = 0;
        public static int max_inst_length = 8;

        public Instruction(InstructionType t = InstructionType.InstructionIllegal)
        {
            this.t = t;
        }

        public int encode(byte[] b)
        {
            switch (this.t)
            {
                // 1 byte
                case InstructionType.InstructionRestart:
                    b[0] = (byte)'R';
                    return 1;
                // 2 byte
                case InstructionType.InstructionSetDisplay:
                    b[0] = (byte)'D';
                    b[1] = (byte)(this.value & 0x1f);
                    return 2;
                // 1-6 byte
                case InstructionType.InstructionTransition:
                    if (this.value <= 0)
                    {
                        if (this.r == 0 && this.g == 0 && this.b == 0)
                        {
                            // encode as blackout ('B')
                            b[0] = (byte)'B';
                            return 1;
                        }
                        else
                        {
                            // encode as setcolour 'C'
                            b[0] = (byte)'C';
                            b[1] = (byte)(this.r & 0xff);
                            b[2] = (byte)(this.g & 0xff);
                            b[3] = (byte)(this.b & 0xff);
                            return 4;
                        }
                    }
                    else if (this.value <= 255)
                    {
                        // encode as fast transition 'T'
                        b[0] = (byte)'T';
                        b[1] = (byte)(this.r & 0xff);
                        b[2] = (byte)(this.g & 0xff);
                        b[3] = (byte)(this.b & 0xff);
                        b[4] = (byte)(this.value & 0xff);
                        return 5;
                    }
                    else
                    {
                        // encode as slow transition 't'
                        b[0] = (byte)'t';
                        b[1] = (byte)(this.r & 0xff);
                        b[2] = (byte)(this.g & 0xff);
                        b[3] = (byte)(this.b & 0xff);
                        b[4] = (byte)((this.value & 0xff00) >> 8);
                        b[5] = (byte)(this.value & 0xff);
                        return 6;
                    }
                case InstructionType.InstructionIllegal:
                    return 0;
                default:
                    return 0;
            }
        }

        public void decode(byte[] b)
        {
            this.t = InstructionType.InstructionIllegal;
            if (b.Length < 1) return;
            switch ((char)b[0])
            {
                case 'R':
                    this.t = InstructionType.InstructionRestart;
                    return;
                case 'B':
                    this.t = InstructionType.InstructionTransition;
                    this.r = this.g = this.b = 0;
                    this.value = 0;
                    return;
                case 'D':
                    if (b.Length < 2) return;
                    this.t = InstructionType.InstructionSetDisplay;
                    this.value = (int)b[1];
                    return;
                case 'C':
                    if (b.Length < 4) return;
                    this.t = InstructionType.InstructionTransition;
                    this.r = (int)b[1];
                    this.g = (int)b[2];
                    this.b = (int)b[3];
                    this.value = 0;
                    return;
                case 'T':
                    if (b.Length < 5) return;
                    this.t = InstructionType.InstructionTransition;
                    this.r = (int)b[1];
                    this.g = (int)b[2];
                    this.b = (int)b[3];
                    this.value = (int)b[4];
                    return;
                case 't':
                    if (b.Length < 6) return;
                    this.t = InstructionType.InstructionTransition;
                    this.r = (int)b[1];
                    this.g = (int)b[2];
                    this.b = (int)b[3];
                    this.value = ((int)b[4]) << 8;
                    this.value |= (int)b[5];
                    return;
                default:
                    return;
            }
        }

        public int getSize()
        {
            byte[] temporary = new byte[max_inst_length];
            return encode(temporary);
        }

        public string Text { get; set; }

        public string getColour()
        {
            return String.Format("{0:X02} {1:X02} {2:X02}", this.r, this.g, this.b);
        }

        public override string ToString()
        {
            switch (this.t)
            {
                // 1 byte
                case InstructionType.InstructionRestart:
                    return "Restart";
                // 2 byte
                case InstructionType.InstructionSetDisplay:
                    if (this.value >= 0x10)
                    {
                        return "Set Display to Blank";
                    }
                    else
                    {
                        return "Set Display to " + String.Format("{0:X01}", this.value);
                    }
                // 1-6 byte
                case InstructionType.InstructionTransition:
                    return "Transition to " + getColour() + " in " + this.value + " milliseconds";
                case InstructionType.InstructionIllegal:
                    return "Illegal";
                default:
                    return "Unknown";
            }
        }

        public int getTime()
        {
            switch (this.t)
            {
                // 1 byte
                case InstructionType.InstructionRestart:
                    return 0;
                // 2 byte
                case InstructionType.InstructionSetDisplay:
                    return 0;
                // 1-6 byte
                case InstructionType.InstructionTransition:
                    return this.value;
                case InstructionType.InstructionIllegal:
                    return 0;
                default:
                    return 0;
            }
        }
    }

    public class InstructionList
    {
        public List<Instruction> contents = null;
        public static int program_too_big = -1;
        public int end_time = 0;
        public string name = null;
        public int num_bytes = 0;

        public InstructionList(byte[] program_bytes = null)
        {
            this.contents = new List<Instruction>();
            if (program_bytes != null)
            {
                decode(program_bytes);
            }
        }

        public void updateProperties()
        {
            int time = 0;
            this.num_bytes = 0;
            this.contents.Add(new Instruction(InstructionType.InstructionRestart));
            foreach (Instruction inst in this.contents)
            {
                inst.start_time = time;
                time += inst.getTime();
                this.num_bytes += inst.getSize();
            }
            this.contents.RemoveAt(this.contents.Count() - 1);
            this.end_time = time;
        }

        public void decode(byte[] program_bytes)
        {
            byte[] slice = new byte[Instruction.max_inst_length];
            byte[] check = new byte[Instruction.max_inst_length];

            // decode each instruction in turn
            this.contents.Clear();
            for (int a = 0; (a < program_bytes.Length) && (a < Comms.max_program_size); )
            {
                Instruction inst = new Instruction();
                int i;
                for (i = 0; (i < Instruction.max_inst_length) && ((a + i) < program_bytes.Length); i++)
                {
                    slice[i] = program_bytes[a + i];
                }
                for (; i < Instruction.max_inst_length; i++)
                {
                    slice[i] = (byte)0;
                }
                inst.decode(slice);

                // detect illegal instruction
                if (inst.t == InstructionType.InstructionIllegal) break;

                int sz = inst.getSize();
                a += sz;
                inst.encode(check);
                for (i = 0; i < sz; i++)
                {
                    if (check[i] != slice[i])
                    {
                        // decoder/encoder error
                        throw new Exception("decoded instruction does not match original (1)");
                    }
                }

                // detect end of program (don't add to list)
                if (inst.t == InstructionType.InstructionRestart) break;

                // all other instructions go into the list
                this.contents.Add(inst);
            }

            // decode program name (if present)
            this.name = "";
            bool valid_name = false;
            for (int i = Comms.program_size - Comms.program_name_size; (i < program_bytes.Length) && (i < Comms.program_size); i++)
            {
                int ch = (int)program_bytes[i];
                if ((ch < 32) || (ch > 126))
                {
                    ch = (int)'?';
                } else if (ch > 32)
                {
                    valid_name = true;
                }
                this.name += (char)((byte)ch);
            }
            if (!valid_name)
            {
                this.name = "";
            }
            updateProperties();
        }

        public string getProperties()
        {
            return this.end_time + " milliseconds, " +
                this.contents.Count + " instructions, " + this.num_bytes + " bytes";
        }

        public int encode(byte[] program_bytes)
        {
            byte[] slice = new byte[Instruction.max_inst_length];
            int a = 0, i = 0;

            // restart instruction at end of list
            this.contents.Add(new Instruction(InstructionType.InstructionRestart));
            // encode each instruction in turn
            foreach (Instruction inst in this.contents)
            {
                if (inst.t == InstructionType.InstructionIllegal)
                {
                    continue;
                }
                int sz = inst.encode(slice);

                for (i = 0; i < sz; i++)
                {
                    if ((a >= program_bytes.Length)
                        || (a >= Comms.max_program_size))
                    {
                        return program_too_big;
                    }
                    program_bytes[a] = slice[i];
                    a++;
                }
            }
            // remove restart instruction
            this.contents.RemoveAt(this.contents.Count() - 1);

            // encode name
            for (i = 0; (i < Comms.program_name_size) && (i < name.Length); i++)
            {
                byte b;
                try
                {
                    b = (byte)name[i];
                }
                catch (Exception)
                {
                    b = (byte)'?';
                }

                program_bytes[i + Comms.max_program_size] = b;
            }
            for (; i < Comms.program_name_size; i++)
            {
                program_bytes[i + Comms.max_program_size] = (byte)' ';
            }

            return a;
        }
    }

    // Entry point
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ConnectionSetup connectionSetup = new ConnectionSetup();
            Application.Run(connectionSetup);
        }
    }
}
