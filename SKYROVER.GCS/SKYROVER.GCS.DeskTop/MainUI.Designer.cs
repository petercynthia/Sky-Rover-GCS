using SKYROVER.GCS.DeskTop.Controls;

namespace SKYROVER.GCS.DeskTop
{
    partial class MainUI
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            MainUI.comPort.logreadmode = false;
            if (polygons != null)
                polygons.Dispose();
            if (routes != null)
                routes.Dispose();
            if (route != null)
                route.Dispose();
            if (aviwriter != null)
                aviwriter.Dispose();

            if (prop != null)
                prop.Stop();


        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.topPanel = new System.Windows.Forms.Panel();
            this.menuPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnAccount = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnConfigPlane = new System.Windows.Forms.Button();
            this.btnVideo = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.bottom_LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.msgInfoPanel = new System.Windows.Forms.RichTextBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.bindingSourceQuickTab = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceHud = new System.Windows.Forms.BindingSource(this.components);
            this.connectionControl1 = new SKYROVER.GCS.DeskTop.Controls.ConnectionControl();
            this.topPanel.SuspendLayout();
            this.menuPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.bottom_LayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQuickTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceHud)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.menuPanel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1308, 55);
            this.topPanel.TabIndex = 0;
            // 
            // menuPanel
            // 
            this.menuPanel.ColumnCount = 11;
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.758F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.242F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 237F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.menuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.menuPanel.Controls.Add(this.btnAccount, 10, 0);
            this.menuPanel.Controls.Add(this.btnSetting, 9, 0);
            this.menuPanel.Controls.Add(this.btnConfigPlane, 7, 0);
            this.menuPanel.Controls.Add(this.btnVideo, 8, 0);
            this.menuPanel.Controls.Add(this.button2, 6, 0);
            this.menuPanel.Controls.Add(this.button1, 5, 0);
            this.menuPanel.Controls.Add(this.connectionControl1, 4, 0);
            this.menuPanel.Controls.Add(this.btnConnect, 3, 0);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.RowCount = 1;
            this.menuPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.menuPanel.Size = new System.Drawing.Size(1308, 55);
            this.menuPanel.TabIndex = 0;
            // 
            // btnAccount
            // 
            this.btnAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAccount.ForeColor = System.Drawing.Color.Blue;
            this.btnAccount.Location = new System.Drawing.Point(1201, 3);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(104, 49);
            this.btnAccount.TabIndex = 0;
            this.btnAccount.Text = "用户";
            this.btnAccount.UseVisualStyleBackColor = true;
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Image = global::SKYROVER.GCS.DeskTop.Properties.Resources.light_connect_icon;
            this.btnConnect.Location = new System.Drawing.Point(567, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnConnect.Size = new System.Drawing.Size(91, 49);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Tag = "disconnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetting.Location = new System.Drawing.Point(1141, 3);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(54, 49);
            this.btnSetting.TabIndex = 2;
            this.btnSetting.Text = "设置";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnConfigPlane
            // 
            this.btnConfigPlane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfigPlane.Location = new System.Drawing.Point(1021, 3);
            this.btnConfigPlane.Name = "btnConfigPlane";
            this.btnConfigPlane.Size = new System.Drawing.Size(54, 49);
            this.btnConfigPlane.TabIndex = 7;
            this.btnConfigPlane.Text = "VTOL";
            this.btnConfigPlane.UseVisualStyleBackColor = true;
            this.btnConfigPlane.Click += new System.EventHandler(this.btnConfigPlane_Click);
            // 
            // btnVideo
            // 
            this.btnVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVideo.Location = new System.Drawing.Point(1081, 3);
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(54, 49);
            this.btnVideo.TabIndex = 8;
            this.btnVideo.Text = "视频";
            this.btnVideo.UseVisualStyleBackColor = true;
            this.btnVideo.Click += new System.EventHandler(this.btnVideo_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(961, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 49);
            this.button2.TabIndex = 4;
            this.button2.Text = "航线";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(901, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 49);
            this.button1.TabIndex = 3;
            this.button1.Text = "测区";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.bottom_LayoutPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 592);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1308, 45);
            this.bottomPanel.TabIndex = 1;
            // 
            // bottom_LayoutPanel
            // 
            this.bottom_LayoutPanel.ColumnCount = 4;
            this.bottom_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 529F));
            this.bottom_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 415F));
            this.bottom_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.bottom_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottom_LayoutPanel.Controls.Add(this.msgInfoPanel, 0, 0);
            this.bottom_LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottom_LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.bottom_LayoutPanel.Name = "bottom_LayoutPanel";
            this.bottom_LayoutPanel.RowCount = 1;
            this.bottom_LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottom_LayoutPanel.Size = new System.Drawing.Size(1308, 45);
            this.bottom_LayoutPanel.TabIndex = 0;
            // 
            // msgInfoPanel
            // 
            this.msgInfoPanel.BackColor = System.Drawing.Color.Black;
            this.msgInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.msgInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgInfoPanel.ForeColor = System.Drawing.Color.ForestGreen;
            this.msgInfoPanel.Location = new System.Drawing.Point(3, 3);
            this.msgInfoPanel.Name = "msgInfoPanel";
            this.msgInfoPanel.Size = new System.Drawing.Size(523, 39);
            this.msgInfoPanel.TabIndex = 0;
            this.msgInfoPanel.Text = "";
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 55);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1308, 537);
            this.mainPanel.TabIndex = 2;
            // 
            // bindingSourceQuickTab
            // 
            this.bindingSourceQuickTab.DataSource = typeof(MissionPlanner.CurrentState);
            // 
            // bindingSourceHud
            // 
            this.bindingSourceHud.DataSource = typeof(MissionPlanner.CurrentState);
            // 
            // connectionControl1
            // 
            this.connectionControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("connectionControl1.BackgroundImage")));
            this.connectionControl1.Location = new System.Drawing.Point(664, 3);
            this.connectionControl1.MinimumSize = new System.Drawing.Size(230, 54);
            this.connectionControl1.Name = "connectionControl1";
            this.connectionControl1.Size = new System.Drawing.Size(230, 54);
            this.connectionControl1.TabIndex = 9;
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1308, 637);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainUI";
            this.Text = "Sky Rover GCS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainUI_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainUI_KeyUp);
            this.topPanel.ResumeLayout(false);
            this.menuPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.bottom_LayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQuickTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceHud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.TableLayoutPanel menuPanel;
        private System.Windows.Forms.Button btnAccount;
        private System.Windows.Forms.Button btnConnect;
        public System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
       
        private System.Windows.Forms.BindingSource bindingSourceHud;
        private System.Windows.Forms.BindingSource bindingSourceQuickTab;
        private System.Windows.Forms.Button btnConfigPlane;
        public System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.RichTextBox msgInfoPanel;
        private System.Windows.Forms.Button btnVideo;
        private System.Windows.Forms.TableLayoutPanel bottom_LayoutPanel;
        private ConnectionControl connectionControl1;
    }
}

