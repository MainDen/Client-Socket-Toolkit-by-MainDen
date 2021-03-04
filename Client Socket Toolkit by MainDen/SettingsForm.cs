using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MainDen.ClientSocketToolkit
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(Logger logger, Client client)
        {
            InitializeComponent();
            Logger = logger;
            Client = client;
            MultiLineRegex = new Regex(MultiLinePattern);
            SingleLineRegex = new Regex(SingleLinePattern);
            Reset();
        }

        private readonly Regex MultiLineRegex;

        private readonly Regex SingleLineRegex;

        private readonly string MultiLinePattern = @"\\|\r\n|\r|\n";

        private readonly string SingleLinePattern = @"\\\\|\\r\\n|\\r|\\n";

        private string ToSingleLine(Match match)
        {
            switch (match.Value)
            {
                case "\\":
                    return @"\\";
                case "\r\n":
                    return @"\r\n";
                case "\r":
                    return @"\r";
                case "\n":
                    return @"\n";
                default:
                    return match.Value;
            }
        }

        private string ToMultiLine(Match match)
        {
            switch (match.Value)
            {
                case @"\\":
                    return "\\";
                case @"\r\n":
                    return "\r\n";
                case @"\r":
                    return "\r";
                case @"\n":
                    return "\n";
                default:
                    return match.Value;
            }
        }

        private void Reset()
        {
            if (Logger is null)
                return;
            if (Client is null)
                return;
            tbLFDTF.Text = Logger.FileDateTimeFormat;
            tbLFPF.Text = Logger.FilePathFormat;
            tbLMF.Text = MultiLineRegex.Replace(Logger.MessageFormat, ToSingleLine);
            tbLMDTF.Text = Logger.MessageDateTimeFormat;
            tbLMDF.Text = MultiLineRegex.Replace(Logger.MessageDetailsFormat, ToSingleLine);
            nudCBS.Value = Client.BufferSize;
        }

        private bool Apply()
        {
            bool applied = false;
            if (Logger is null)
                return applied;
            if (Client is null)
                return applied;
            try
            {
                try
                {
                    tbLFPFResult.Text = string.Format(tbLFPF.Text, tbLFDTFResult.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid log file path format.", "Error");
                    throw;
                }
                try
                {
                    SampleLogWrite("Apply try 1.");
                }
                catch
                {
                    MessageBox.Show("Invalid log message format.", "Error");
                    throw;
                }
                try
                {
                    SampleLogWrite("Apply try 2.", "OK");
                }
                catch
                {
                    MessageBox.Show("Invalid log message data format.", "Error");
                    throw;
                }
                Logger.FileDateTimeFormat = tbLFDTF.Text;
                Logger.FilePathFormat = tbLFPF.Text;
                Logger.MessageFormat = SingleLineRegex.Replace(tbLMF.Text, ToMultiLine);
                Logger.MessageDateTimeFormat = tbLMDTF.Text;
                Logger.MessageDetailsFormat = SingleLineRegex.Replace(tbLMDF.Text, ToMultiLine);
                Client.BufferSize = (int)nudCBS.Value;
                applied = true;
            }
            catch { }
            return applied;
        }

        private Logger Logger;
        private Client Client;
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

        private void TSLF_Tick(object sender, EventArgs e)
        {
            tbLFDTFResult.Text = DateTime.Now.ToString(tbLFDTF.Text);
            try
            {
                tbLFPFResult.Text = string.Format(tbLFPF.Text, tbLFDTFResult.Text);
            }
            catch
            {
                tbLFPFResult.Text = "Invalid format.";
            }
            try
            {
                int i = Random.Next(0, SampleMessages.Count - 1);
                SampleLogWrite(SampleMessages[i]);
                double j = Random.NextDouble();
                if (j > 0.7)
                    SampleLogWrite("It could have been a error. Be careful!", "Any data.", Logger.Sender.Error);
            }
            catch
            {
                rtbEasyLog.Text += "\nInvalid format.";
            }
        }

        private void SampleLogWrite(string message, Logger.Sender logSender = Logger.Sender.Log)
        {
            DateTime now = DateTime.Now;
            string logMessage = string.Format(SingleLineRegex.Replace(tbLMF.Text, ToMultiLine),
                logSender, now.ToString(tbLMDTF.Text), message ?? "NULL");
            rtbEasyLog.Text += logMessage;
        }

        private void SampleLogWrite(string message, object data, Logger.Sender logSender = Logger.Sender.Log)
        {
            DateTime now = DateTime.Now;
            string logMessage = string.Format(SingleLineRegex.Replace(tbLMF.Text, ToMultiLine) +
                SingleLineRegex.Replace(tbLMDF.Text, ToMultiLine),
                logSender, now.ToString(tbLMDTF.Text), message ?? "NULL", data ?? "NULL");
            rtbEasyLog.Text += logMessage;
        }

        private void BReset_Click(object sender, EventArgs e) => Reset();

        private void BApply_Click(object sender, EventArgs e) => Apply();

        private void BAccept_Click(object sender, EventArgs e)
        {
            if (Apply())
                Close();
        }

        private void RTBEasyLog_TextChanged(object sender, EventArgs e)
        {
            rtbEasyLog.SelectionStart = rtbEasyLog.Text.Length;
            rtbEasyLog.ScrollToCaret();
        }

        private void TBLFDTF_TextChanged(object sender, EventArgs e)
        {
            tbLFDTFResult.Text = DateTime.Now.ToString(tbLFDTF.Text);
            try
            {
                tbLFPFResult.Text = string.Format(tbLFPF.Text, tbLFDTFResult.Text);
            }
            catch
            {
                tbLFPFResult.Text = "Invalid format.";
            }
        }

        private void TBLFPF_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tbLFPFResult.Text = string.Format(tbLFPF.Text, tbLFDTFResult.Text);
            }
            catch
            {
                tbLFPFResult.Text = "Invalid format.";
            }
        }
    }
}
