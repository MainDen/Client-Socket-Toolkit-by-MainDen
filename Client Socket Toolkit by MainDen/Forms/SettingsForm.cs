using MainDen.ClientSocketToolkit.Modules;
using MainDen.Modules.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MainDen.ClientSocketToolkit
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(Client client)
        {
            InitializeComponent();
            if (client is null)
                throw new ArgumentNullException(nameof(client));
            Client = client;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Reset();
        }

        private bool Apply()
        {
            bool applied = true;
            DateTime now = DateTime.Now;
            try
            {
                string path = Log.GetFilePath(tbLFilePathF.Text, now);
                Log.FilePathFormat = tbLFilePathF.Text;
                tbLFilePathF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbLFilePathF.BackColor = Color.Red;
            }
            try
            {
                Log.GetLogMessage(TextConverter.ToMultiLine(tbLMessageF.Text), Log.Sender.Log, now, "Test message.");
                Log.MessageFormat = TextConverter.ToMultiLine(tbLMessageF.Text);
                tbLMessageF.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                tbLMessageF.BackColor = Color.Red;
            }
            try
            {
                Log.WriteToFile = cbLWTFile.Checked;
                cbLWTFile.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                cbLWTFile.BackColor = Color.Red;
            }
            try
            {
                Log.WriteToCustom = cbLWTScreen.Checked;
                cbLWTScreen.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                cbLWTScreen.BackColor = Color.Red;
            }
            try
            {
                Echo.GetEchoMessage(TextConverter.ToMultiLine(tbEMessageF.Text), "Test message.");
                Echo.MessageFormat = TextConverter.ToMultiLine(tbEMessageF.Text);
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
            try
            {
                Client.ReceiveTimeout = (int)nudCReceiveTimeout.Value;
                nudCReceiveTimeout.BackColor = Color.Lime;
            }
            catch
            {
                applied = false;
                nudCReceiveTimeout.BackColor = Color.Red;
            }
            return applied;
        }

        private void Reset()
        {
            tbLFilePathF.Text = Log.FilePathFormat;
            tbLFilePathF.BackColor = Color.Lime;
            tbLMessageF.Text = Log.MessageFormat;
            tbLMessageF.BackColor = Color.Lime;
            cbLWTFile.Checked = Log.WriteToFile;
            cbLWTFile.BackColor = Color.Lime;
            cbLWTScreen.Checked = Log.WriteToCustom;
            cbLWTScreen.BackColor = Color.Lime;
            tbEMessageF.Text = Echo.MessageFormat;
            tbEMessageF.BackColor = Color.Lime;
            nudCBufferSize.Value = Client.BufferSize;
            nudCBufferSize.BackColor = Color.Lime;
            nudCReceiveTimeout.Value = Client.ReceiveTimeout;
            nudCReceiveTimeout.BackColor = Color.Lime;
        }

        private Client Client;
        private Echo Echo = Echo.Default;
        private Log Log = Log.Default;
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
            rtbSampleOutput.Text += Log.GetLogMessage(TextConverter.ToMultiLine(messageFormat), (Log.Sender)Random.Next(0, 3), DateTime.Now, SampleMessages[Random.Next(0, SampleMessages.Count)]);
        }

        private void EchoWriteSampleMessage(string messageFormat)
        {
            rtbSampleOutput.Text += Echo.GetEchoMessage(messageFormat, SampleMessages[Random.Next(0, SampleMessages.Count)]);
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
            if (control.Text == Log.FilePathFormat)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
            try
            {
                tbLFilePathFR.Text = Log.GetFilePath(control.Text, now);
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
            if (control.Text == Log.MessageFormat)
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

        private void CBLWTFile_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (control is null)
                return;
            if (control.Checked == Log.WriteToFile)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
        }

        private void CBLWTScreen_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (control is null)
                return;
            if (control.Checked == Log.WriteToCustom)
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

        private void NUDCReceiveTimeout_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control is null)
                return;
            if (control.Value == Client.ReceiveTimeout)
                control.BackColor = Color.Lime;
            else
                control.BackColor = DefaultBackColor;
        }
    }
}
