﻿using Extension.Text;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MainDen.ClientSocketToolkit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TextWrite = new Action<string>(message =>
            {
                rtbLog.Text += message;
            });
            TextWriteAsync += new Action<string>(message => rtbLog.Invoke(TextWrite, message));
            Echo.CustomWrite += TextWriteAsync;
            Logger.CustomWrite += TextWriteAsync;
            Client.Logger = Logger;
            Client.StatusChanged += OnStatusChangedAsync;
            Client.DataReceived += OnDataReceivedAsync;
        }

        const string defaultSettingsFileName = @".\settings.xml";

        Action<string> TextWrite;

        Action<string> TextWriteAsync;

        Client Client = new Client();

        Echo Echo = new Echo();

        Logger Logger = new Logger();

        Encoding IncomingEncoding = Encoding.Default;

        Encoding OutcomingEncoding = Encoding.Default;

        private Action<Client.ClientStatus> onStatusChangedAction;

        private Action<Client.ClientStatus> OnStatusChangedAction
        {
            get
            {
                return onStatusChangedAction ??
                    (onStatusChangedAction = new Action<Client.ClientStatus>(OnStatusChanged));
            }
        }

        private Action<byte[]> onDataReceivedAction;

        private Action<byte[]> OnDataReceivedAction
        {
            get
            {
                return onDataReceivedAction ??
                    (onDataReceivedAction = new Action<byte[]>(OnDataReceived));
            }
        }

        private void OnStatusChanged(Client.ClientStatus status)
        {
            if (status == Client.ClientStatus.Available)
            {
                bProtocolType.Enabled = true;
                bAddressFamily.Enabled = true;
                tbServer.Enabled = true;
                tbPort.Enabled = true;
            }
            else
            {
                bProtocolType.Enabled = false;
                bAddressFamily.Enabled = false;
                tbServer.Enabled = false;
                tbPort.Enabled = false;
            }
            switch (status)
            {
                case Client.ClientStatus.Available:
                    bConnect.Text = "Connect";
                    if (bSend.Text != "Execute")
                        bSend.Text = "Echo";
                    break;
                case Client.ClientStatus.Connecting:
                    bConnect.Text = "Connecting..";
                    if (bSend.Text != "Execute")
                        bSend.Text = "Echo";
                    break;
                case Client.ClientStatus.Connected:
                    bConnect.Text = "Disconnect";
                    if (bSend.Text != "Execute")
                        bSend.Text = "Send";
                    break;
                case Client.ClientStatus.Disconnecting:
                    bConnect.Text = "Disconnecting..";
                    if (bSend.Text != "Execute")
                        bSend.Text = "Echo";
                    break;
            }
        }

        private void OnStatusChangedAsync(Client.ClientStatus status)
        {
            Invoke(OnStatusChangedAction, status);
        }

        private void OnDataReceived(byte[] data)
        {
            Encoding InEnc = IncomingEncoding;
            string text = InEnc.GetString(data);
            if (InEnc == Hexadecimal.HASCII)
                new ContentPresenter(text, "HASCII", System.Drawing.FontFamily.GenericMonospace).Show();
            else if (InEnc == Hexadecimal.Hex)
                new ContentPresenter(text, "HEX", System.Drawing.FontFamily.GenericMonospace).Show();
            else if (text.StartsWith("HTTP/"))
                new HttpPresenter(text).Show();
            else
                new ContentPresenter(text).Show();
        }

        private void OnDataReceivedAsync(byte[] data)
        {
            Invoke(OnDataReceivedAction, data);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlAsSettings(defaultSettingsFileName);
                SettingsAsXml().Save(defaultSettingsFileName);
            } catch { }
            Logger?.Write("Application was loaded.");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger?.Write("Application was closed.");
            Client.Dispose();
        }

        private void RTBLog_TextChanged(object sender, EventArgs e)
        {
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
        }

        private void BProtocolType_Click(object sender, EventArgs e)
        {
            switch (bProtocolType.Text)
            {
                case "TCP":
                    Client.ProtocolType = ProtocolType.Udp;
                    Client.SocketType = SocketType.Dgram;
                    bProtocolType.Text = "UDP";
                    break;
                case "UDP":
                    Client.ProtocolType = ProtocolType.IP;
                    Client.SocketType = SocketType.Stream;
                    bProtocolType.Text = "IP";
                    break;
                default:
                    Client.ProtocolType = ProtocolType.Tcp;
                    Client.SocketType = SocketType.Stream;
                    bProtocolType.Text = "TCP";
                    break;
            }
        }

        private void BAddressFamily_Click(object sender, EventArgs e)
        {
            switch (bAddressFamily.Text)
            {
                case "IPv4":
                    Client.AddressFamily = AddressFamily.InterNetworkV6;
                    bAddressFamily.Text = "IPv6";
                    break;
                default:
                    Client.AddressFamily = AddressFamily.InterNetwork;
                    bAddressFamily.Text = "IPv4";
                    break;
            }
        }

        private void BIncomingEncoding_Click(object sender, EventArgs e)
        {
            switch (bIncomingEncoding.Text)
            {
                case "Default":
                    IncomingEncoding = Encoding.ASCII;
                    bIncomingEncoding.Text = "ASCII";
                    break;
                case "ASCII":
                    IncomingEncoding = Encoding.UTF8;
                    bIncomingEncoding.Text = "UTF-8";
                    break;
                case "UTF-8":
                    IncomingEncoding = Encoding.Unicode;
                    bIncomingEncoding.Text = "Unicode";
                    break;
                case "Unicode":
                    IncomingEncoding = Hexadecimal.Hex;
                    bIncomingEncoding.Text = "HEX";
                    break;
                case "HEX":
                    IncomingEncoding = Hexadecimal.HASCII;
                    bIncomingEncoding.Text = "HASCII";
                    break;
                default:
                    IncomingEncoding = Encoding.Default;
                    bIncomingEncoding.Text = "Default";
                    break;
            }
        }

        private void BOutcomingEncoding_Click(object sender, EventArgs e)
        {
            switch (bOutcomingEncoding.Text)
            {
                case "Default":
                    OutcomingEncoding = Encoding.ASCII;
                    bOutcomingEncoding.Text = "ASCII";
                    break;
                case "ASCII":
                    OutcomingEncoding = Encoding.UTF8;
                    bOutcomingEncoding.Text = "UTF-8";
                    break;
                case "UTF-8":
                    OutcomingEncoding = Encoding.Unicode;
                    bOutcomingEncoding.Text = "Unicode";
                    break;
                case "Unicode":
                    OutcomingEncoding = Hexadecimal.Hex;
                    bOutcomingEncoding.Text = "HEX";
                    break;
                case "HEX":
                    OutcomingEncoding = Hexadecimal.HASCII;
                    bOutcomingEncoding.Text = "HASCII";
                    break;
                case "HASCII":
                    bOutcomingEncoding.Text = "Command";
                    bSend.Text = "Execute";
                    break;
                default:
                    OutcomingEncoding = Encoding.Default;
                    bOutcomingEncoding.Text = "Default";
                    if (Client.Status == Client.ClientStatus.Connected)
                        bSend.Text = "Send";
                    else
                        bSend.Text = "Echo";
                    break;
            }
        }

        private void BConnect_Click(object sender, EventArgs e)
        {
            switch (Client.Status)
            {
                case Client.ClientStatus.Available:
                    Client.ConnectAsync(tbServer.Text, tbPort.Text);
                    break;
                case Client.ClientStatus.Connected:
                case Client.ClientStatus.Connecting:
                    Client.DisconnectAsync();
                    break;
            }
        }

        private void BSend_Click(object sender, EventArgs e)
        {
            switch (bSend.Text)
            {
                case "Send":
                    Client.SendAsync(OutcomingEncoding.GetBytes(tbMessage.Text));
                    break;
                case "Echo":
                    Echo.Write(IncomingEncoding.GetString(OutcomingEncoding.GetBytes(tbMessage.Text)));
                    break;
                case "Execute":
                    ExecuteCommand(tbMessage.Text);
                    break;
            }
        }

        private void ExecuteCommand(string command)
        {
            Logger?.Write("Command execution is not yet supported.");
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("App will be reset.", "New", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Client.Reset();
                Reset();
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("App will be closed.", "Close", MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }

        private void ImportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdSettings.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                XmlAsSettings(ofdSettings.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc?.Message ?? "Unexpected error.", "Error");
            }
            try
            {
                SettingsAsXml().Save("settings.xml");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc?.Message ?? "Unexpected error.", "Error");
            }
        }

        private void ExportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSettings.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                SettingsAsXml().Save(sfdSettings.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc?.Message ?? "Unexpected error.", "Error");
            }
        }

        private void EditSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm(Client, Echo, Logger);
            settings.ShowDialog();
            try
            {
                SettingsAsXml().Save("settings.xml");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc?.Message ?? "Unexpected error.", "Error");
            }
        }

        private void Reset()
        {
            tbServer.Text = "";
            tbPort.Text = "";
            bConnect.Text = "Connect";
            bProtocolType.Text = "Protocol Type";
            bAddressFamily.Text = "Address Family";
            bIncomingEncoding.Text = "Incoming Encoding";
            tbMessage.Text = "";
            bSend.Text = "Echo";
            bOutcomingEncoding.Text = "Outcoming Encoding";
            rtbLog.Text = "";
            IncomingEncoding = Encoding.Default;
            OutcomingEncoding = Encoding.Default;
        }

        private void XmlAsSettings(string filename)
        {
            XmlPorter porter = new XmlPorter();
            porter.Document.Load(filename);
            porter.Set("/Settings/Client", Client,
                nameof(Client.BufferSize),
                nameof(Client.ReceiveTimeout));
            porter.Set("/Settings/Echo", Echo,
                nameof(Echo.WriteToCustom),
                nameof(Echo.WriteToConsole),
                nameof(Echo.MessageFormat));
            porter.Set("/Settings/Logger", Logger,
                nameof(Logger.WriteToCustom),
                nameof(Logger.WriteToConsole),
                nameof(Logger.WriteToFile),
                nameof(Logger.FilePathFormat),
                nameof(Logger.MessageFormat),
                nameof(Logger.MessageDetailsFormat));
        }

        private XmlDocument SettingsAsXml()
        {
            XmlPorter porter = new XmlPorter();
            XmlNode root = porter.Document.AppendChild(porter.Add("Settings"));

            root.AppendChild(porter.Add("Client", Client,
                nameof(Client.BufferSize),
                nameof(Client.ReceiveTimeout)));

            root.AppendChild(porter.Add("Echo", Echo,
                nameof(Echo.WriteToCustom),
                nameof(Echo.WriteToConsole),
                nameof(Echo.MessageFormat)));

            root.AppendChild(porter.Add("Logger", Logger,
                nameof(Logger.WriteToCustom),
                nameof(Logger.WriteToConsole),
                nameof(Logger.WriteToFile),
                nameof(Logger.FilePathFormat),
                nameof(Logger.MessageFormat),
                nameof(Logger.MessageDetailsFormat)));

            return porter.Document;
        }
    }
}
