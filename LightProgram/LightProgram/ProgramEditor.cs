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


    public partial class ProgramEditor : Form
    {
        private Comms comms = null;
        private LightChooser lightChooser = null;
        private int program_number = -1;
        private Transition transition = null;
        private SetDisplay set_display = null;
        private Wait wait = null;
        private Instruction last_added_wait = null;
        private Instruction last_added_transition = null;
        private static string save_filter = "Light Program files|*.lightprogram";
        private string save_filename = null;

        public class InstructionEditor
        {
            public Instruction inst = null;
 
            public InstructionEditor(Instruction inst)
            {
                this.inst = inst;
            }
            
            public override string ToString()
            {
                return inst.ToString();
            }
        };

        public ProgramEditor(LightChooser lightChooser)
        {
            this.comms = new Comms();
            this.lightChooser = lightChooser;
            InitializeComponent();
            this.transition = new Transition(this);
            this.set_display = new SetDisplay(this);
            this.wait = new Wait(this);
            this.Icon = Properties.Resources.rgb;
        }

        public void SetComms(Comms comms)
        {
            this.comms = comms;
            this.transition.SetComms(comms);
            this.set_display.SetComms(comms);
            this.wait.SetComms(comms);
        }

        public void SetProgram(InstructionList inst_list, int program_number)
        {
            this.transition.Hide();

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
            this.program_number = program_number;
            this.namebox.Text = copy_inst_list.name;
            this.properties.Text = copy_inst_list.getProperties();
            this.Text = "Edit Program " + program_number;
            buttonsUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        private void doneClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        public enum ModeType
        {
            RunMode, CheckMode, SaveMode, ExportMode
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

            switch (mode)
            {
                case ModeType.RunMode:
                    // Start running the program
                    if (err != "")
                    {
                        MessageBox.Show(err, "Run Program");
                    }
                    else
                    {
                        c.t = CommandType.CommandRunTemporaryProgram;
                        this.comms.SendCommand(c);
                        SetProgram(copy_inst_list, this.program_number);
                        buttonsUpdate();
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
                    if (err != "")
                    {
                        MessageBox.Show(err, "Save Program");
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Really store program " + this.program_number + " in memory?", "Save Program", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            c.t = CommandType.CommandSaveEEPROMProgram;
                            c.program_number = this.program_number;
                            this.comms.SendCommand(c);
                            this.Hide();
                            SetProgram(copy_inst_list, this.program_number);
                            buttonsUpdate();
                        }
                    }
                    break;
                case ModeType.ExportMode:
                    if (err != "")
                    {
                        MessageBox.Show(err, "Export Program");
                    }
                    else
                    {
                        SaveFileDialog theDialog = new SaveFileDialog();
                        theDialog.Title = "Export Light Program File";
                        theDialog.Filter = save_filter;
                        theDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        theDialog.FileName = this.save_filename;
                        if (theDialog.ShowDialog() == DialogResult.OK)
                        {
                            this.save_filename = theDialog.FileName;
                            ProgramIO.writeProgram(this.save_filename, c.program_bytes);
                        }
                    }
                    break;
            }
        }

        private void runClicked(object sender, EventArgs e)
        {
            checkProgramThenDoSomething(ModeType.RunMode);
        }

        private void revalidate()
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

        private void insertAndEdit(Instruction inst)
        {
            InstructionEditor inst_ed = new InstructionEditor(inst);
            if (this.instructions.SelectedItems.Count != 0)
            {
                int i = this.instructions.SelectedIndex;
                this.instructions.Items.Insert(i, inst_ed);
            }
            else
            {
                this.instructions.Items.Add(inst_ed);
            }
            editItem(inst_ed);
        }

        private void editItem(InstructionEditor inst_ed)
        {
            switch (inst_ed.inst.t)
            {
                case InstructionType.InstructionTransition:
                    this.transition.SetInstruction(inst_ed.inst);
                    this.transition.Show();
                    break;
                case InstructionType.InstructionSetDisplay:
                    this.set_display.SetInstruction(inst_ed.inst);
                    this.set_display.Show();
                    break;
                case InstructionType.InstructionWait:
                    this.wait.SetInstruction(inst_ed.inst);
                    this.wait.Show();
                    break;
                default:
                    break;
            }
            revalidate();
        }

        private void addTransitionClicked(object sender, EventArgs e)
        {
            Instruction inst = new Instruction();
            inst.t = InstructionType.InstructionTransition;
            if (last_added_transition != null)
            {
                inst.r = last_added_transition.r;
                inst.g = last_added_transition.g;
                inst.b = last_added_transition.b;
                inst.value = last_added_transition.value;
            }
            insertAndEdit(inst);
        }

        private void addDisplayButton(object sender, EventArgs e)
        {
            Instruction inst = new Instruction();
            inst.t = InstructionType.InstructionSetDisplay;
            insertAndEdit(inst);
        }

        private void addWaitButton_Click(object sender, EventArgs e)
        {
            Instruction inst = new Instruction();
            inst.t = InstructionType.InstructionWait;
            if (last_added_wait != null)
            {
                inst.value = last_added_wait.value;
            }
            insertAndEdit(inst);
        }

        private void moveUpButton(object sender, EventArgs e)
        {
            if (this.instructions.SelectedItems.Count != 0)
            {
                int i = this.instructions.SelectedIndex;
                InstructionEditor inst = (InstructionEditor)this.instructions.Items[i];
                this.instructions.Items.RemoveAt(i);
                i--;
                if (i < 0) i = 0;
                this.instructions.Items.Insert(i, inst);
                this.instructions.SetSelected(i, true);
            }
            revalidate();
        }

        private void moveDownButton(object sender, EventArgs e)
        {
            if (this.instructions.SelectedItems.Count != 0)
            {
                int i = this.instructions.SelectedIndex;
                InstructionEditor inst = (InstructionEditor)this.instructions.Items[i];
                this.instructions.Items.RemoveAt(i);
                i++;
                int j = this.instructions.Items.Count;
                if (i >= j)
                {
                    this.instructions.Items.Add(inst);
                    this.instructions.SetSelected(j, true);
                }
                else
                {
                    this.instructions.Items.Insert(i, inst);
                    this.instructions.SetSelected(i, true);
                }
            }
            revalidate();
        }

        private void deleteButton(object sender, EventArgs e)
        {
            if (this.instructions.SelectedItems.Count != 0)
            {
                this.instructions.Items.RemoveAt(this.instructions.SelectedIndex);
            }
            revalidate();
        }

        private void buttonsUpdate()
        {
            bool selection = (this.instructions.SelectedItems.Count != 0);
            this.move_up.Enabled = selection && (this.instructions.SelectedIndex > 0);
            this.move_down.Enabled = selection && (this.instructions.SelectedIndex < (this.instructions.Items.Count - 1));
            this.edit.Enabled = selection;
            this.delete.Enabled = selection;
        }

        public void refreshInstruction(Instruction inst)
        {
            if (inst.t == InstructionType.InstructionTransition)
            {
                last_added_transition = inst;
            }
            if (inst.t == InstructionType.InstructionWait)
            {
                last_added_wait = inst;
            }
            int i, j = this.instructions.Items.Count;
            for (i = 0; i < j; i++)
            {
                InstructionEditor inst_ed = (InstructionEditor) this.instructions.Items[i];
                if (inst_ed.inst == inst)
                {
                    this.instructions.Items.RemoveAt(i);
                    if ((i + 1) == j)
                    {
                        // Last member of list
                        this.instructions.Items.Add(inst_ed);
                        this.instructions.SetSelected(0, false);
                    }
                    else
                    {
                        this.instructions.Items.Insert(i, inst_ed);
                        if (i < j)
                        {
                            this.instructions.SetSelected(i + 1, true);
                        }
                    }
                    revalidate();
                    return;
                }
            }
        }

        private void editClicked(object sender, EventArgs e)
        {
            if (this.instructions.SelectedItems.Count != 0)
            {
                InstructionEditor inst_ed = (InstructionEditor)this.instructions.Items[this.instructions.SelectedIndex];
                editItem(inst_ed);
            }
        }

        private void instructions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void instructionsMouseDown(object sender, MouseEventArgs e)
        {
            int index = this.instructions.IndexFromPoint(new Point(e.X, e.Y));
            if ((index >= 0) && (index < this.instructions.Items.Count))
            {
                this.instructions.SetSelected(index, true);
                this.instructions.DoDragDrop(this.instructions.Items[index], DragDropEffects.Move);
            }
        }

        private void instructionsDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void instructionsDragDrop(object sender, DragEventArgs e)
        {
            Point point = this.instructions.PointToClient(new Point(e.X, e.Y));
            int index = this.instructions.IndexFromPoint(point);
            if ((index < 0) || (index >= this.instructions.Items.Count))
            {
                index = this.instructions.Items.Count - 1;
            }
            object data = e.Data.GetData(typeof(InstructionEditor));
            if (data != null)
            {
                int i = this.instructions.Items.IndexOf(data);
                if (i == index)
                {
                    this.instructions.SetSelected(index, true);
                    InstructionEditor inst_ed = (InstructionEditor)this.instructions.Items[index];
                    editItem(inst_ed);
                }
                else
                {
                    this.instructions.Items.Remove(data);
                    this.instructions.Items.Insert(index, data);
                    this.instructions.ClearSelected();
                }
                revalidate();
                buttonsUpdate();
            }
        }

        private void instructions_SelectedIndexChanged(object sender, EventArgs e)
        {
            revalidate();
            buttonsUpdate();
        }

        private void closeButton(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void namebox_TextChanged(object sender, EventArgs e)
        {

        }

        private void importButtonClicked(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Import Light Program File";
            theDialog.Filter = save_filter;
            theDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                this.save_filename = theDialog.FileName;
                byte[] new_program = ProgramIO.readProgram(this.save_filename);

                if ((new_program == null) || (new_program.Length != Comms.program_size))
                {
                    MessageBox.Show("This program file is not valid", "Import Program");
                }
                else
                {
                    SetProgram(new InstructionList(new_program), this.program_number);
                }
            }
        }

        private void exportButtonClicked(object sender, EventArgs e)
        {
            checkProgramThenDoSomething(ModeType.ExportMode);
        }
    }
}
