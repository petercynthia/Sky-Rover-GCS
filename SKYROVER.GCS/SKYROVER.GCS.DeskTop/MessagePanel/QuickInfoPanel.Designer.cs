namespace SKYROVER.GCS.DeskTop.MessagePanel
{
    partial class QuickInfoPanel
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
            this.tableLayoutPanelQuick = new System.Windows.Forms.TableLayoutPanel();
            this.alt_View = new MissionPlanner.Controls.QuickView();
            this.airspeed_View = new MissionPlanner.Controls.QuickView();
            this.altoffsethome_View = new MissionPlanner.Controls.QuickView();
            this.throttle_percent_View = new MissionPlanner.Controls.QuickView();
            this.satcount_View = new MissionPlanner.Controls.QuickView();
            this.DistToHome_View = new MissionPlanner.Controls.QuickView();
            this.wp_dist_View = new MissionPlanner.Controls.QuickView();
            this.nextWP_View = new MissionPlanner.Controls.QuickView();
            this.yaw_View = new MissionPlanner.Controls.QuickView();
            this.timeSinceArmInAir_View = new MissionPlanner.Controls.QuickView();
            this.groundspeed_View = new MissionPlanner.Controls.QuickView();
            this.battery_voltage_View = new MissionPlanner.Controls.QuickView();
            this.tableLayoutPanelQuick.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelQuick
            // 
            this.tableLayoutPanelQuick.AutoScroll = true;
            this.tableLayoutPanelQuick.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelQuick.ColumnCount = 3;
            this.tableLayoutPanelQuick.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanelQuick.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanelQuick.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.4F));
            this.tableLayoutPanelQuick.Controls.Add(this.alt_View, 0, 2);
            this.tableLayoutPanelQuick.Controls.Add(this.airspeed_View, 1, 1);
            this.tableLayoutPanelQuick.Controls.Add(this.altoffsethome_View, 0, 1);
            this.tableLayoutPanelQuick.Controls.Add(this.throttle_percent_View, 1, 0);
            this.tableLayoutPanelQuick.Controls.Add(this.satcount_View, 0, 0);
            this.tableLayoutPanelQuick.Controls.Add(this.DistToHome_View, 2, 3);
            this.tableLayoutPanelQuick.Controls.Add(this.wp_dist_View, 1, 3);
            this.tableLayoutPanelQuick.Controls.Add(this.nextWP_View, 0, 3);
            this.tableLayoutPanelQuick.Controls.Add(this.yaw_View, 1, 2);
            this.tableLayoutPanelQuick.Controls.Add(this.timeSinceArmInAir_View, 2, 2);
            this.tableLayoutPanelQuick.Controls.Add(this.groundspeed_View, 2, 1);
            this.tableLayoutPanelQuick.Controls.Add(this.battery_voltage_View, 2, 0);
            this.tableLayoutPanelQuick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelQuick.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelQuick.Name = "tableLayoutPanelQuick";
            this.tableLayoutPanelQuick.RowCount = 4;
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelQuick.Size = new System.Drawing.Size(442, 280);
            this.tableLayoutPanelQuick.TabIndex = 76;
            this.tableLayoutPanelQuick.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.tableLayoutPanelQuick.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.tableLayoutPanelQuick.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // alt_View
            // 
            this.alt_View.desc = "alt";
            this.alt_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alt_View.ForeColor = System.Drawing.Color.White;
            this.alt_View.Location = new System.Drawing.Point(3, 143);
            this.alt_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.alt_View.Name = "alt_View";
            this.alt_View.number = 0D;
            this.alt_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(86)))));
            this.alt_View.numberformat = "0.00";
            this.alt_View.Size = new System.Drawing.Size(141, 64);
            this.alt_View.TabIndex = 9;
            // 
            // airspeed_View
            // 
            this.airspeed_View.desc = "airspeed";
            this.airspeed_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.airspeed_View.ForeColor = System.Drawing.Color.White;
            this.airspeed_View.Location = new System.Drawing.Point(150, 73);
            this.airspeed_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.airspeed_View.Name = "airspeed_View";
            this.airspeed_View.number = 0D;
            this.airspeed_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(83)))));
            this.airspeed_View.numberformat = "0.00";
            this.airspeed_View.Size = new System.Drawing.Size(141, 64);
            this.airspeed_View.TabIndex = 8;
            // 
            // altoffsethome_View
            // 
            this.altoffsethome_View.desc = "altoffsethome";
            this.altoffsethome_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.altoffsethome_View.ForeColor = System.Drawing.Color.White;
            this.altoffsethome_View.Location = new System.Drawing.Point(3, 73);
            this.altoffsethome_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.altoffsethome_View.Name = "altoffsethome_View";
            this.altoffsethome_View.number = 0D;
            this.altoffsethome_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(96)))), ((int)(((byte)(91)))));
            this.altoffsethome_View.numberformat = "0.00";
            this.altoffsethome_View.Size = new System.Drawing.Size(141, 64);
            this.altoffsethome_View.TabIndex = 3;
            // 
            // throttle_percent_View
            // 
            this.throttle_percent_View.desc = "throttle_percent";
            this.throttle_percent_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.throttle_percent_View.ForeColor = System.Drawing.Color.White;
            this.throttle_percent_View.Location = new System.Drawing.Point(150, 3);
            this.throttle_percent_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.throttle_percent_View.Name = "throttle_percent_View";
            this.throttle_percent_View.number = 0D;
            this.throttle_percent_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(132)))), ((int)(((byte)(46)))));
            this.throttle_percent_View.numberformat = "0.00";
            this.throttle_percent_View.Size = new System.Drawing.Size(141, 64);
            this.throttle_percent_View.TabIndex = 1;
            // 
            // satcount_View
            // 
            this.satcount_View.BackColor = System.Drawing.Color.Transparent;
            this.satcount_View.desc = "satcount";
            this.satcount_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.satcount_View.ForeColor = System.Drawing.Color.White;
            this.satcount_View.Location = new System.Drawing.Point(3, 3);
            this.satcount_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.satcount_View.Name = "satcount_View";
            this.satcount_View.number = 0D;
            this.satcount_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(151)))), ((int)(((byte)(248)))));
            this.satcount_View.numberformat = "0";
            this.satcount_View.Size = new System.Drawing.Size(141, 64);
            this.satcount_View.TabIndex = 1;
            // 
            // DistToHome_View
            // 
            this.DistToHome_View.desc = "DistToHome";
            this.DistToHome_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DistToHome_View.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DistToHome_View.ForeColor = System.Drawing.Color.White;
            this.DistToHome_View.Location = new System.Drawing.Point(297, 213);
            this.DistToHome_View.MinimumSize = new System.Drawing.Size(100, 27);
            this.DistToHome_View.Name = "DistToHome_View";
            this.DistToHome_View.number = 0D;
            this.DistToHome_View.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DistToHome_View.numberformat = "0";
            this.DistToHome_View.Size = new System.Drawing.Size(142, 64);
            this.DistToHome_View.TabIndex = 10;
            // 
            // wp_dist_View
            // 
            this.wp_dist_View.desc = "wp_dist";
            this.wp_dist_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wp_dist_View.ForeColor = System.Drawing.Color.White;
            this.wp_dist_View.Location = new System.Drawing.Point(150, 213);
            this.wp_dist_View.Name = "wp_dist_View";
            this.wp_dist_View.number = 0D;
            this.wp_dist_View.numberColor = System.Drawing.Color.White;
            this.wp_dist_View.numberformat = "0";
            this.wp_dist_View.Size = new System.Drawing.Size(141, 64);
            this.wp_dist_View.TabIndex = 11;
            // 
            // nextWP_View
            // 
            this.nextWP_View.desc = "下一航点";
            this.nextWP_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextWP_View.ForeColor = System.Drawing.Color.White;
            this.nextWP_View.Location = new System.Drawing.Point(3, 213);
            this.nextWP_View.Name = "nextWP_View";
            this.nextWP_View.number = 0D;
            this.nextWP_View.numberColor = System.Drawing.Color.White;
            this.nextWP_View.numberformat = "0.0";
            this.nextWP_View.Size = new System.Drawing.Size(141, 64);
            this.nextWP_View.TabIndex = 11;
            // 
            // yaw_View
            // 
            this.yaw_View.desc = "yaw";
            this.yaw_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yaw_View.ForeColor = System.Drawing.Color.White;
            this.yaw_View.Location = new System.Drawing.Point(150, 143);
            this.yaw_View.Name = "yaw_View";
            this.yaw_View.number = 0D;
            this.yaw_View.numberColor = System.Drawing.Color.White;
            this.yaw_View.numberformat = "0.00";
            this.yaw_View.Size = new System.Drawing.Size(141, 64);
            this.yaw_View.TabIndex = 11;
            // 
            // timeSinceArmInAir_View
            // 
            this.timeSinceArmInAir_View.desc = "已飞时间";
            this.timeSinceArmInAir_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSinceArmInAir_View.ForeColor = System.Drawing.Color.White;
            this.timeSinceArmInAir_View.Location = new System.Drawing.Point(297, 143);
            this.timeSinceArmInAir_View.Name = "timeSinceArmInAir_View";
            this.timeSinceArmInAir_View.number = 0D;
            this.timeSinceArmInAir_View.numberColor = System.Drawing.Color.White;
            this.timeSinceArmInAir_View.numberformat = "0";
            this.timeSinceArmInAir_View.Size = new System.Drawing.Size(142, 64);
            this.timeSinceArmInAir_View.TabIndex = 11;
            // 
            // groundspeed_View
            // 
            this.groundspeed_View.desc = "groundspeed";
            this.groundspeed_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groundspeed_View.ForeColor = System.Drawing.Color.White;
            this.groundspeed_View.Location = new System.Drawing.Point(297, 73);
            this.groundspeed_View.Name = "groundspeed_View";
            this.groundspeed_View.number = 0D;
            this.groundspeed_View.numberColor = System.Drawing.Color.White;
            this.groundspeed_View.numberformat = "0.00";
            this.groundspeed_View.Size = new System.Drawing.Size(142, 64);
            this.groundspeed_View.TabIndex = 11;
            // 
            // battery_voltage_View
            // 
            this.battery_voltage_View.desc = "battery_voltage";
            this.battery_voltage_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.battery_voltage_View.ForeColor = System.Drawing.Color.White;
            this.battery_voltage_View.Location = new System.Drawing.Point(297, 3);
            this.battery_voltage_View.Name = "battery_voltage_View";
            this.battery_voltage_View.number = 0D;
            this.battery_voltage_View.numberColor = System.Drawing.Color.White;
            this.battery_voltage_View.numberformat = "0.0";
            this.battery_voltage_View.Size = new System.Drawing.Size(142, 64);
            this.battery_voltage_View.TabIndex = 11;
            // 
            // QuickInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(442, 280);
            this.Controls.Add(this.tableLayoutPanelQuick);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QuickInfoPanel";
            this.Opacity = 0.75D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "QuickInfoPanel";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            this.tableLayoutPanelQuick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelQuick;
        private MissionPlanner.Controls.QuickView DistToHome_View;
        private MissionPlanner.Controls.QuickView alt_View;
        private MissionPlanner.Controls.QuickView airspeed_View;
        private MissionPlanner.Controls.QuickView altoffsethome_View;
        private MissionPlanner.Controls.QuickView throttle_percent_View;
        private MissionPlanner.Controls.QuickView satcount_View;
        private MissionPlanner.Controls.QuickView wp_dist_View;
        private MissionPlanner.Controls.QuickView nextWP_View;
        private MissionPlanner.Controls.QuickView yaw_View;
        private MissionPlanner.Controls.QuickView timeSinceArmInAir_View;
        private MissionPlanner.Controls.QuickView groundspeed_View;
        private MissionPlanner.Controls.QuickView battery_voltage_View;
    }
}