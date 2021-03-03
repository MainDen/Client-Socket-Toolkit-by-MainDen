﻿using System;
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
            ReplaceRegex = new Regex(ReplacePattern);
            ReturnRegex = new Regex(ReturnPattern);
            Reset();
        }

        private Regex ReplaceRegex;

        private Regex ReturnRegex;

        private readonly string ReplacePattern = "(\\|\r\n|\r|\n)";

        private readonly string ReturnPattern = @"((\\\\)|(\\r\\n)|(\\r)|(\\n))";

        private string ReplaceNewLine(Match match)
        {
            switch (match.Value)
            {
                case @"\":
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

        private string ReturnNewLine(Match match)
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
            tbLFPF.Text = Logger.PathFormat;
            tbLMF.Text = ReplaceRegex.Replace(Logger.MessageFormat, ReplaceNewLine);
            tbLMDTF.Text = Logger.MessageDateTimeFormat;
            tbLMDF.Text = ReplaceRegex.Replace(Logger.DataFormat, ReplaceNewLine);
            nudCBS.Value = Client.BufferSize;
        }

        private void Apply()
        {
            if (Logger is null)
                return;
            if (Client is null)
                return;
            try
            {
                SampleLogWrite("Apply settings.", "OK");
                Logger.FileDateTimeFormat = tbLFDTF.Text;
                Logger.PathFormat = tbLFPF.Text;
                Logger.MessageFormat = ReturnRegex.Replace(tbLMF.Text, ReturnNewLine);
                Logger.MessageDateTimeFormat = tbLMDTF.Text;
                Logger.DataFormat = ReturnRegex.Replace(tbLMDF.Text, ReturnNewLine);
                Client.BufferSize = (int)nudCBS.Value;
            }
            catch
            {
                rtbEasyLog.Text += "\nInvalid format.";
            }
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
            try
            {
                int i = Random.Next(0, SampleMessages.Count - 1);
                SampleLogWrite(SampleMessages[i]);
                double j = Random.NextDouble();
                if (j > 0.7)
                    SampleLogWrite("It could have been a error. Be careful!", "Any data.", Logger.LoggerSender.Error);
            }
            catch
            {
                rtbEasyLog.Text += "\nInvalid format.";
            }
        }

        private void SampleLogWrite(string message, Logger.LoggerSender logSender = Logger.LoggerSender.Log)
        {
            DateTime now = DateTime.Now;
            string logMessage = string.Format(ReturnRegex.Replace(tbLMF.Text, ReturnNewLine),
                logSender, now.ToString(tbLMDTF.Text), message ?? "NULL");
            rtbEasyLog.Text += logMessage;
        }

        private void SampleLogWrite(string message, object data, Logger.LoggerSender logSender = Logger.LoggerSender.Log)
        {
            DateTime now = DateTime.Now;
            string logMessage = string.Format(ReturnRegex.Replace(tbLMF.Text, ReturnNewLine) +
                ReturnRegex.Replace(tbLMDF.Text, ReturnNewLine),
                logSender, now.ToString(tbLMDTF.Text), message ?? "NULL", data ?? "NULL");
            rtbEasyLog.Text += logMessage;
        }

        private void BReset_Click(object sender, EventArgs e) => Reset();

        private void BApply_Click(object sender, EventArgs e) => Apply();

        private void BAccept_Click(object sender, EventArgs e)
        {
            Apply();
            Close();
        }

        private void RTBEasyLog_TextChanged(object sender, EventArgs e)
        {
            rtbEasyLog.SelectionStart = rtbEasyLog.Text.Length;
            rtbEasyLog.ScrollToCaret();
        }
    }
}
