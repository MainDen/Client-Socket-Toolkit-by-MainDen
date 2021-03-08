
namespace MainDen.ClientSocketToolkit
{
    partial class HttpPresenter
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lHTTPHeaders = new System.Windows.Forms.Label();
            this.rtbHTTPHeaders = new System.Windows.Forms.RichTextBox();
            this.lContent = new System.Windows.Forms.Label();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lHTTPHeaders, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbHTTPHeaders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lContent, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rtbContent, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lHTTPHeaders
            // 
            this.lHTTPHeaders.AutoSize = true;
            this.lHTTPHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lHTTPHeaders.Location = new System.Drawing.Point(3, 3);
            this.lHTTPHeaders.Margin = new System.Windows.Forms.Padding(3);
            this.lHTTPHeaders.Name = "lHTTPHeaders";
            this.lHTTPHeaders.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lHTTPHeaders.Size = new System.Drawing.Size(478, 13);
            this.lHTTPHeaders.TabIndex = 0;
            this.lHTTPHeaders.Text = "HTTP Response:";
            // 
            // rtbHTTPHeaders
            // 
            this.rtbHTTPHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHTTPHeaders.Location = new System.Drawing.Point(3, 22);
            this.rtbHTTPHeaders.Name = "rtbHTTPHeaders";
            this.rtbHTTPHeaders.Size = new System.Drawing.Size(478, 60);
            this.rtbHTTPHeaders.TabIndex = 1;
            this.rtbHTTPHeaders.Text = "";
            // 
            // lContent
            // 
            this.lContent.AutoSize = true;
            this.lContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lContent.Location = new System.Drawing.Point(3, 88);
            this.lContent.Margin = new System.Windows.Forms.Padding(3);
            this.lContent.Name = "lContent";
            this.lContent.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lContent.Size = new System.Drawing.Size(478, 13);
            this.lContent.TabIndex = 2;
            this.lContent.Text = "Content:";
            // 
            // rtbContent
            // 
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Location = new System.Drawing.Point(3, 107);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(478, 351);
            this.rtbContent.TabIndex = 3;
            this.rtbContent.Text = "";
            // 
            // HttpPresenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "HttpPresenter";
            this.Text = "HttpHtmlPresenter";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lHTTPHeaders;
        private System.Windows.Forms.Label lContent;
        public System.Windows.Forms.RichTextBox rtbHTTPHeaders;
        public System.Windows.Forms.RichTextBox rtbContent;
    }
}