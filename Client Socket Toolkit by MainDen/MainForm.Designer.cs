
namespace MainDen.ClientSocketToolkit
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bOutcomingEncoding = new System.Windows.Forms.Button();
            this.lServer = new System.Windows.Forms.Label();
            this.bProtocolType = new System.Windows.Forms.Button();
            this.bAddressFamily = new System.Windows.Forms.Button();
            this.bIncomingEncoding = new System.Windows.Forms.Button();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.lPort = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.bConnect = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.mMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.mMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.bOutcomingEncoding, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.lServer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bProtocolType, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bAddressFamily, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.bIncomingEncoding, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbServer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lPort, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbLog, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbPort, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbMessage, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.bConnect, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.bSend, 4, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(644, 237);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bOutcomingEncoding
            // 
            this.bOutcomingEncoding.AutoSize = true;
            this.bOutcomingEncoding.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bOutcomingEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bOutcomingEncoding.Location = new System.Drawing.Point(521, 207);
            this.bOutcomingEncoding.Name = "bOutcomingEncoding";
            this.bOutcomingEncoding.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bOutcomingEncoding.Size = new System.Drawing.Size(120, 27);
            this.bOutcomingEncoding.TabIndex = 23;
            this.bOutcomingEncoding.Text = "Outcoming Encoding";
            this.bOutcomingEncoding.UseVisualStyleBackColor = true;
            this.bOutcomingEncoding.Click += new System.EventHandler(this.BOutcomingEncoding_Click);
            // 
            // lServer
            // 
            this.lServer.AutoSize = true;
            this.lServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lServer.Location = new System.Drawing.Point(3, 3);
            this.lServer.Margin = new System.Windows.Forms.Padding(3);
            this.lServer.MinimumSize = new System.Drawing.Size(120, 0);
            this.lServer.Name = "lServer";
            this.lServer.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lServer.Size = new System.Drawing.Size(120, 27);
            this.lServer.TabIndex = 1;
            this.lServer.Text = "Server:";
            this.lServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bProtocolType
            // 
            this.bProtocolType.AutoSize = true;
            this.bProtocolType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bProtocolType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bProtocolType.Location = new System.Drawing.Point(3, 36);
            this.bProtocolType.Name = "bProtocolType";
            this.bProtocolType.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bProtocolType.Size = new System.Drawing.Size(120, 27);
            this.bProtocolType.TabIndex = 10;
            this.bProtocolType.Text = "Protocol Type";
            this.bProtocolType.UseVisualStyleBackColor = true;
            this.bProtocolType.Click += new System.EventHandler(this.BProtocolType_Click);
            // 
            // bAddressFamily
            // 
            this.bAddressFamily.AutoSize = true;
            this.bAddressFamily.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bAddressFamily.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bAddressFamily.Location = new System.Drawing.Point(3, 69);
            this.bAddressFamily.Name = "bAddressFamily";
            this.bAddressFamily.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bAddressFamily.Size = new System.Drawing.Size(120, 27);
            this.bAddressFamily.TabIndex = 11;
            this.bAddressFamily.Text = "Address Family";
            this.bAddressFamily.UseVisualStyleBackColor = true;
            this.bAddressFamily.Click += new System.EventHandler(this.BAddressFamily_Click);
            // 
            // bIncomingEncoding
            // 
            this.bIncomingEncoding.AutoSize = true;
            this.bIncomingEncoding.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bIncomingEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bIncomingEncoding.Location = new System.Drawing.Point(3, 102);
            this.bIncomingEncoding.Name = "bIncomingEncoding";
            this.bIncomingEncoding.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bIncomingEncoding.Size = new System.Drawing.Size(120, 27);
            this.bIncomingEncoding.TabIndex = 12;
            this.bIncomingEncoding.Text = "Incoming Encoding";
            this.bIncomingEncoding.UseVisualStyleBackColor = true;
            this.bIncomingEncoding.Click += new System.EventHandler(this.BIncomingEncoding_Click);
            // 
            // tbServer
            // 
            this.tbServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServer.Location = new System.Drawing.Point(129, 6);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(305, 20);
            this.tbServer.TabIndex = 2;
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPort.Location = new System.Drawing.Point(440, 3);
            this.lPort.Margin = new System.Windows.Forms.Padding(3);
            this.lPort.Name = "lPort";
            this.lPort.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lPort.Size = new System.Drawing.Size(29, 27);
            this.lPort.TabIndex = 3;
            this.lPort.Text = "Port:";
            this.lPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtbLog, 4);
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(129, 36);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.rtbLog, 4);
            this.rtbLog.Size = new System.Drawing.Size(512, 132);
            this.rtbLog.TabIndex = 24;
            this.rtbLog.Text = "";
            this.rtbLog.TextChanged += new System.EventHandler(this.RTBLog_TextChanged);
            // 
            // tbPort
            // 
            this.tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPort.Location = new System.Drawing.Point(475, 6);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(40, 20);
            this.tbPort.TabIndex = 4;
            // 
            // tbMessage
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbMessage, 4);
            this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessage.Location = new System.Drawing.Point(3, 174);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tableLayoutPanel1.SetRowSpan(this.tbMessage, 2);
            this.tbMessage.Size = new System.Drawing.Size(512, 60);
            this.tbMessage.TabIndex = 21;
            // 
            // bConnect
            // 
            this.bConnect.AutoSize = true;
            this.bConnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bConnect.Location = new System.Drawing.Point(521, 3);
            this.bConnect.MinimumSize = new System.Drawing.Size(120, 0);
            this.bConnect.Name = "bConnect";
            this.bConnect.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bConnect.Size = new System.Drawing.Size(120, 27);
            this.bConnect.TabIndex = 5;
            this.bConnect.Text = "Connect";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.BConnect_Click);
            // 
            // bSend
            // 
            this.bSend.AutoSize = true;
            this.bSend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSend.Location = new System.Drawing.Point(521, 174);
            this.bSend.Name = "bSend";
            this.bSend.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.bSend.Size = new System.Drawing.Size(120, 27);
            this.bSend.TabIndex = 22;
            this.bSend.Text = "Echo";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.BSend_Click);
            // 
            // mMainMenu
            // 
            this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.mMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mMainMenu.Name = "mMainMenu";
            this.mMainMenu.Size = new System.Drawing.Size(644, 24);
            this.mMainMenu.TabIndex = 1;
            this.mMainMenu.Text = "MainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importSettingsToolStripMenuItem,
            this.exportSettingsToolStripMenuItem,
            this.editSettingsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // importSettingsToolStripMenuItem
            // 
            this.importSettingsToolStripMenuItem.Name = "importSettingsToolStripMenuItem";
            this.importSettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importSettingsToolStripMenuItem.Text = "Import Settings";
            // 
            // exportSettingsToolStripMenuItem
            // 
            this.exportSettingsToolStripMenuItem.Name = "exportSettingsToolStripMenuItem";
            this.exportSettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportSettingsToolStripMenuItem.Text = "Export Settings";
            // 
            // editSettingsToolStripMenuItem
            // 
            this.editSettingsToolStripMenuItem.Name = "editSettingsToolStripMenuItem";
            this.editSettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editSettingsToolStripMenuItem.Text = "Edit Settings";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.mMainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mMainMenu;
            this.MinimumSize = new System.Drawing.Size(480, 240);
            this.Name = "MainForm";
            this.Text = "Client Socket Toolkit by MainDen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.mMainMenu.ResumeLayout(false);
            this.mMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lServer;
        private System.Windows.Forms.Button bProtocolType;
        private System.Windows.Forms.Button bAddressFamily;
        private System.Windows.Forms.Button bIncomingEncoding;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bOutcomingEncoding;
        private System.Windows.Forms.MenuStrip mMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSettingsToolStripMenuItem;
    }
}

