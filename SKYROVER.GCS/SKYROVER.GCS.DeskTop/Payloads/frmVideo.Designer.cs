namespace SKYROVER.GCS.DeskTop.Payloads
{
    partial class frmVideo
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
            this.myBtnPlay = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.myVlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.clickPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.myVlcControl)).BeginInit();
            this.VideoPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.clickPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // myBtnPlay
            // 
            this.myBtnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.myBtnPlay.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.myBtnPlay.Location = new System.Drawing.Point(474, 386);
            this.myBtnPlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.myBtnPlay.Name = "myBtnPlay";
            this.myBtnPlay.Size = new System.Drawing.Size(69, 23);
            this.myBtnPlay.TabIndex = 9;
            this.myBtnPlay.Text = "播放";
            this.myBtnPlay.UseVisualStyleBackColor = true;
            this.myBtnPlay.Click += new System.EventHandler(this.myBtnPlay_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUrl.Location = new System.Drawing.Point(3, 386);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(464, 25);
            this.txtUrl.TabIndex = 20;
            this.txtUrl.Text = "rtsp://127.0.0.1:8554/1";
            // 
            // myVlcControl
            // 
            this.myVlcControl.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.myVlcControl.BackgroundImage = global::SKYROVER.GCS.DeskTop.Properties.Resources.UAV01;
            this.myVlcControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.myVlcControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.myVlcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myVlcControl.Enabled = false;
            this.myVlcControl.Location = new System.Drawing.Point(0, 0);
            this.myVlcControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.myVlcControl.Name = "myVlcControl";
            this.myVlcControl.Size = new System.Drawing.Size(540, 375);
            this.myVlcControl.Spu = -1;
            this.myVlcControl.TabIndex = 1;
            this.myVlcControl.Text = "vlcRincewindControl1";
            this.myVlcControl.VlcLibDirectory = null;
            this.myVlcControl.VlcMediaplayerOptions = null;
            this.myVlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.myVlcControl_VlcLibDirectoryNeeded);
            // 
            // VideoPanel
            // 
            this.VideoPanel.Controls.Add(this.tableLayoutPanel1);
            this.VideoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoPanel.Location = new System.Drawing.Point(0, 0);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Size = new System.Drawing.Size(548, 415);
            this.VideoPanel.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.Controls.Add(this.myBtnPlay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.clickPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtUrl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 415);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // clickPanel
            // 
            this.clickPanel.BackColor = System.Drawing.Color.Transparent;
            this.clickPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.clickPanel, 2);
            this.clickPanel.Controls.Add(this.myVlcControl);
            this.clickPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clickPanel.Location = new System.Drawing.Point(3, 3);
            this.clickPanel.Name = "clickPanel";
            this.clickPanel.Size = new System.Drawing.Size(542, 377);
            this.clickPanel.TabIndex = 22;
            this.clickPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.clickPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.clickPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // frmVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(548, 415);
            this.Controls.Add(this.VideoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmVideo";
            this.Opacity = 0.75D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "视频";
            ((System.ComponentModel.ISupportInitialize)(this.myVlcControl)).EndInit();
            this.VideoPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.clickPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl myVlcControl;
        private System.Windows.Forms.Button myBtnPlay;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Panel VideoPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel clickPanel;
    }
}