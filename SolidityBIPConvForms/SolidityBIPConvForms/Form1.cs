using SolidityBIPConv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;


namespace SolidityBIPConvForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectFileBtn_Click(object sender, EventArgs e)
        {
            ClearBTN_Click(sender,e); // call function Clean button to clear text.
            string text;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    text = File.ReadAllText(file);
                    InputBox.Text = (text);
                    Main NewMAin = new Main();
                    NewMAin.MainMethod(text, OutputBox);
                }
                catch (IOException)
                {
                    MessageBox.Show("ERROR UPLOADING FILE", "ERROR UPLOAD FILE",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            InputBox.ScrollBars = ScrollBars.Both;
        }
        private void OutputBox_TextChanged(object sender, EventArgs e)
        {
            OutputBox.ScrollBars = ScrollBars.Both;
        }

        private void ClearBTN_Click(object sender, EventArgs e)
        {
            InputBox.Clear();
            OutputBox.Clear();
        }

        private void SaveBTN_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "bip files (*.bip)|*.bip|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                }
            }
            string Output = OutputBox.Text;
            string path = @Path.GetFullPath(saveFileDialog1.FileName);
            File.WriteAllText(path, Output); // Gets text from output box and saves to a new file
        }

    }
}