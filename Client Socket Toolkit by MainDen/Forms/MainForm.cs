using MainDen.ClientSocketToolkit.Modules;
using MainDen.Modules.IO;
using MainDen.Modules.Text;
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
            Echo.Default.Custom += CustomWriteAsync;
            Log.Default.Custom += CustomWriteAsync;
            Client.StatusChanged += OnStatusChangedAsync;
            Client.DataReceived += OnDataReceivedAsync;
        }

        const string defaultSettingsFileName = @".\settings.xml";

        Client Client = new Client();

        Encoding IncomingEncoding = Encoding.Default;

        Encoding OutcomingEncoding = Encoding.Default;

        private readonly object lSettings = new object();

        private Delegate onStatusChangedDelegate;

        private Delegate OnStatusChangedDelegate
        {
            get
            {
                lock (lSettings)
                    return onStatusChangedDelegate ?? (onStatusChangedDelegate = new Action<Client.ClientStatus>(OnStatusChanged));
            }
        }

        private Delegate onDataReceivedDelegate;

        private Delegate OnDataReceivedDelegate
        {
            get
            {
                lock (lSettings)
                    return onDataReceivedDelegate ?? (onDataReceivedDelegate = new Action<byte[]>(OnDataReceived));
            }
        }

        private Delegate customWriteDelegate;

        private Delegate CustomWriteDelegate
        {
            get
            {
                lock (lSettings)
                    return customWriteDelegate ?? (customWriteDelegate = new Action<string>(CustomWrite));
            }
        }

        private void CustomWrite(string message)
        {
            rtbLog.Text += message;
        }

        private void CustomWriteAsync(string message)
        {
            rtbLog.Invoke(CustomWriteDelegate, message);
        }

        private void OnStatusChanged(Client.ClientStatus status)
        {
            bool available = status == Client.ClientStatus.Available;
            bProtocolType.Enabled = available;
            bAddressFamily.Enabled = available;
            tbServer.Enabled = available;
            tbPort.Enabled = available;
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
            Invoke(OnStatusChangedDelegate, status);
        }

        private void OnDataReceived(byte[] data)
        {
            Encoding InEnc = IncomingEncoding;
            string text = InEnc.GetString(data);
            if (InEnc == HexadecimalEncoding.HASCII)
                new ContentPresenter(text, "HASCII", System.Drawing.FontFamily.GenericMonospace).Show();
            else if (InEnc == HexadecimalEncoding.Hex)
                new ContentPresenter(text, "HEX", System.Drawing.FontFamily.GenericMonospace).Show();
            else if (text.StartsWith("HTTP/"))
                new HttpPresenter(text).Show();
            else
                new ContentPresenter(text).Show();
        }

        private void OnDataReceivedAsync(byte[] data)
        {
            Invoke(OnDataReceivedDelegate, data);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlAsSettings(defaultSettingsFileName);
                SettingsAsXml().Save(defaultSettingsFileName);
            } catch { }
            Log.Default.Write("Application was loaded.");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Default.Write("Application was closed.");
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
                    IncomingEncoding = HexadecimalEncoding.Hex;
                    bIncomingEncoding.Text = "HEX";
                    break;
                case "HEX":
                    IncomingEncoding = HexadecimalEncoding.HASCII;
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
                    OutcomingEncoding = HexadecimalEncoding.Hex;
                    bOutcomingEncoding.Text = "HEX";
                    break;
                case "HEX":
                    OutcomingEncoding = HexadecimalEncoding.HASCII;
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
                    Echo.Default.Write(IncomingEncoding.GetString(OutcomingEncoding.GetBytes(tbMessage.Text)));
                    break;
                case "Execute":
                    ExecuteCommand(tbMessage.Text);
                    break;
            }
        }

        private void ExecuteCommand(string command)
        {
            Echo.Default.Write("Command execution is not yet supported.");
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
            SettingsForm settings = new SettingsForm(Client);
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
            porter.Initialize(Client, "/Settings/Client",
                nameof(Client.BufferSize),
                nameof(Client.ReceiveTimeout));
            porter.Initialize(Echo.Default, "/Settings/Echo",
                nameof(Echo.Default.WriteToCustom),
                nameof(Echo.Default.WriteToConsole),
                nameof(Echo.Default.MessageFormat));
            porter.Initialize(Log.Default, "/Settings/Log",
                nameof(Log.Default.WriteToCustom),
                nameof(Log.Default.WriteToConsole),
                nameof(Log.Default.WriteToFile),
                nameof(Log.Default.FilePathFormat),
                nameof(Log.Default.MessageFormat));
        }

        private XmlDocument SettingsAsXml()
        {
            XmlPorter porter = new XmlPorter();
            XmlNode root = porter.Document.AppendChild(porter.Add("Settings"));

            root.AppendChild(porter.Add("Client", Client,
                nameof(Client.BufferSize),
                nameof(Client.ReceiveTimeout)));

            root.AppendChild(porter.Add("Echo", Echo.Default,
                nameof(Echo.Default.WriteToCustom),
                nameof(Echo.Default.WriteToConsole),
                nameof(Echo.Default.MessageFormat)));

            root.AppendChild(porter.Add("Log", Log.Default,
                nameof(Log.Default.WriteToCustom),
                nameof(Log.Default.WriteToConsole),
                nameof(Log.Default.WriteToFile),
                nameof(Log.Default.FilePathFormat),
                nameof(Log.Default.MessageFormat)));

            return porter.Document;
        }
    }
}
