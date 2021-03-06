using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MainDen.ClientSocketToolkit
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(Client client, Echo echo, Logger logger)
        {
            InitializeComponent();
            Client = client;
            Echo = echo;
            Logger = logger;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (Client is null)
            {
                MessageBox.Show("Client must not be null.");
                Close();
                return;
            }
            if (Echo is null)
            {
                MessageBox.Show("Echo must not be null.");
                Close();
                return;
            }
            if (Logger is null)
            {
                MessageBox.Show("Logger must not be null.");
                Close();
                return;
            }
            Reset();
        }

        private bool Apply()
        {
            bool applied = true;
            DateTime now = DateTime.Now;
            try
            {
                string path = Logger.GetFilePath(tbLFilePathF.Text, now);
                File.Create(path).Dispose();
                Logger.FilePathFormat = tbLFilePathF.Text;
                tbLFilePathF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbLFilePathF.BackColor = Color.Red;
            }
            try
            {
                Logger.GetMessage(tbLMessageF.Text, Logger.Sender.Log, now, "Test message.");
                Logger.MessageFormat = tbLMessageF.Text;
                tbLMessageF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbLMessageF.BackColor = Color.Red;
            }
            try
            {
                Logger.GetMessage(tbLMessageDetailsF.Text, Logger.Sender.Log, now, "Test message.", "Test details.");
                Logger.MessageDetailsFormat = tbLMessageDetailsF.Text;
                tbLMessageDetailsF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbLMessageDetailsF.BackColor = Color.Red;
            }
            try
            {
                Logger.WriteToFile = cbLWTFile.Checked;
                cbLWTFile.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                cbLWTFile.BackColor = Color.Red;
            }
            try
            {
                Logger.WriteToCustom = cbLWTScreen.Checked;
                cbLWTScreen.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                cbLWTScreen.BackColor = Color.Red;
            }
            try
            {
                Echo.GetMessage(tbEMessageF.Text, "Test message.");
                Echo.MessageFormat = tbEMessageF.Text;
                tbEMessageF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbEMessageF.BackColor = Color.Red;
            }
            try
            {
                Client.BufferSize = (int)nudCBufferSize.Value;
                nudCBufferSize.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                nudCBufferSize.BackColor = Color.Red;
            }
            return applied;
        }

        private void Reset()
        {
            tbLFilePathF.Text = Logger.FilePathFormat;
            tbLFilePathF.BackColor = Color.Lime;
            tbLMessageF.Text = Logger.MessageFormat;
            tbLMessageF.BackColor = Color.Lime;
            tbLMessageDetailsF.Text = Logger.MessageDetailsFormat;
            tbLMessageDetailsF.BackColor = Color.Lime;
            cbLWTFile.Checked = Logger.WriteToFile;
            cbLWTFile.BackColor = Color.Lime;
            cbLWTScreen.Checked = Logger.WriteToCustom;
            cbLWTScreen.BackColor = Color.Lime;
            tbEMessageF.Text = Echo.MessageFormat;
            tbEMessageF.BackColor = Color.Lime;
            nudCBufferSize.Value = Client.BufferSize;
            nudCBufferSize.BackColor = Color.Lime;
        }

        private Client Client;
        private Echo Echo;
        private Logger Logger;
        private Random Random = new Random();
        private readonly List<string> SampleMessages = new List<string>()
        {
            "Any log message.",
            "I'm a logged message!",
            "Do you like this log message format?",
            "Simple log message.",
            "*Made on Earth by humans*",
            "MainDen was here.",
            "Error.. Okay, this is a joke!",
            "I believe in you!",
            "I wanna pizza..",
            null,
            "Rumor has it that there is a NULL message here.",
            "Don't use this app to bother Google!",
        };

        private void LoggerWriteSampleMessage(string messageFormat)
        {
            rtbSampleOutput.Text += Logger.GetMessage(messageFormat, (Logger.Sender)Random.Next(0, 3), DateTime.Now, SampleMessages[Random.Next(0, SampleMessages.Count)]);
        }

        private void LoggerWriteMessageDetailsSample(string messageDetailsFormat)
        {
            rtbSampleOutput.Text += Logger.GetMessage(messageDetailsFormat, (Logger.Sender)Random.Next(0, 3), DateTime.Now, SampleMessages[Random.Next(0, SampleMessages.Count)], "Any details.");
        }

        private void EchoWriteSampleMessage(string messageFormat)
        {
            rtbSampleOutput.Text += Echo.GetMessage(messageFormat, SampleMessages[Random.Next(0, SampleMessages.Count)]);
        }

        private void BApply_Click(object sender, EventArgs e) => Apply();

        private void BReset_Click(object sender, EventArgs e) => Reset();

        private void BOK_Click(object sender, EventArgs e)
        {
            if (Apply())
                Close();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RTBSampleOutput_TextChanged(object sender, EventArgs e)
        {
            rtbSampleOutput.SelectionStart = rtbSampleOutput.Text.Length;
            rtbSampleOutput.ScrollToCaret();
        }

        private void TBLFilePathF_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control is null)
                return;
            DateTime now = DateTime.Now;
            if (control.Text == Logger.FilePathFormat)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
            try
            {
                tbLFilePathFR.Text = Logger.GetFilePath(control.Text, now);
            }
            catch
            {
                tbLFilePathFR.Text = "Invalid format.";
                control.BackColor = Color.Red;
            }
        }

        private void TBLMessageF_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control is null)
                return;
            DateTime now = DateTime.Now;
            if (control.Text == Logger.MessageFormat)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
            try
            {
                LoggerWriteSampleMessage(control.Text);
            }
            catch
            {
                control.BackColor = Color.Red;
            }
        }

        private void TBLMessageDetailsF_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control is null)
                return;
            DateTime now = DateTime.Now;
            if (control.Text == Logger.MessageDetailsFormat)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
            try
            {
                LoggerWriteMessageDetailsSample(control.Text);
            }
            catch
            {
                control.BackColor = Color.Red;
            }
        }

        private void CBLWTFile_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (control is null)
                return;
            if (control.Checked == Logger.WriteToFile)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
        }

        private void CBLWTScreen_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (control is null)
                return;
            if (control.Checked == Logger.WriteToCustom)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
        }

        private void TBEMessageF_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control is null)
                return;
            if (control.Text == Echo.MessageFormat)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
            try
            {
                EchoWriteSampleMessage(control.Text);
            }
            catch
            {
                control.BackColor = Color.Red;
            }
        }

        private void NUDCBufferSize_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control is null)
                return;
            if (control.Value == Client.BufferSize)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
        }
    }
}
