using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
            Logger.CustomLogging += new Action<string>(message => rtbLog.Invoke(LogWrite, message));
            Client.Logger = Logger;
            Client.StatusChanged += OnClientStausChanged;
        }

        Logger Logger = new Logger();

        Client Client = new Client();

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.Write("Application was loaded.");
        }

        private void BConnect_Click(object sender, EventArgs e)
        {
            switch (Client.Status)
            {
                case Client.ClientStatus.Available:
                    Client.ConnectAsync(tbServer.Text, tbPort.Text);
                    break;
                case Client.ClientStatus.Connected:
                    Client.DisconnectAsync();
                    break;
            }
        }

        private void OnClientStausChanged(Client.ClientStatus status)
        {
            Invoke(new Action(() =>
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
                        bConnect.Enabled = true;
                        break;
                    case Client.ClientStatus.Connecting:
                        bConnect.Text = "Connecting..";
                        bConnect.Enabled = false;
                        break;
                    case Client.ClientStatus.Connected:
                        bConnect.Text = "Disconnect";
                        bConnect.Enabled = true;
                        break;
                    case Client.ClientStatus.Disconnecting:
                        bConnect.Text = "Disconnecting..";
                        bConnect.Enabled = false;
                        break;
                }
            }));
        }
    }
}
