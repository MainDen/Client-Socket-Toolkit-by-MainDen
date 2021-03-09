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
    public partial class HttpPresenter : Form
    {
        public HttpPresenter(string httpMessage)
        {
            InitializeComponent();
            string[] parts = System.Text.RegularExpressions.Regex.Split(httpMessage, @"\r\n\r\n|\r\r|\n\n");
            int length = parts.Length;
            if (length > 1)
            {
                rtbHTTPHeaders.Text = parts[0];
                rtbContent.Text = string.Join("\n\n", parts, 1, length - 1);
            }
            else
            {
                lHTTPHeaders.Visible = false;
                rtbHTTPHeaders.Visible = false;
                rtbContent.Text = httpMessage;
            }
        }
    }
}
