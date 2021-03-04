using Extension.Text;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MainDen.ClientSocketToolkit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Action<string> LogWrite = new Action<string>(message =>
            {
                rtbLog.Text += message;
            });
            Logger.CustomWrite += new Action<string>(message => rtbLog.Invoke(LogWrite, message));
            Client.Logger = Logger;
            Client.StatusChanged += OnStatusChangedAsync;
            Client.DataReceived += OnDataReceivedAsync;
        }

        Logger Logger = new Logger();

        Client Client = new Client();

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

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("App will be reset.", "New", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Client.Reset();
                Reset();
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

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("App will be closed.", "Close", MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
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
            rtbLog.Text += IncomingEncoding.GetString(data);
        }

        private void OnDataReceivedAsync(byte[] data)
        {
            Invoke(OnDataReceivedAction, data);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger?.Write("Application was loaded.");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger?.Write("Application was closed.");
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
                    Echo(tbMessage.Text);
                    break;
                case "Execute":
                    ExecuteCommand(tbMessage.Text);
                    break;
            }
        }

        private void Echo(string text)
        {
            rtbLog.Text += $"\n(Echo)\n{IncomingEncoding.GetString(OutcomingEncoding.GetBytes(text))}\n";
        }

        private void ExecuteCommand(string command)
        {
            Logger?.Write("Command execution is not yet supported.");
        }

        private void EditSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm(Logger, Client);
            settings.ShowDialog();
        }
    }
}
