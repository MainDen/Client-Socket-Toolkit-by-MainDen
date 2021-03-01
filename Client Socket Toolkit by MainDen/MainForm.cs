using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        Logger Logger = new Logger();

        Client Client = new Client();

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.Write("Application was loaded.");
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
        }
    }
}
