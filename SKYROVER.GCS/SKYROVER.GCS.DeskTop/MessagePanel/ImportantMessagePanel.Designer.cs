namespace SKYROVER.GCS.DeskTop.MessagePanel
{
    partial class ImportantMessagePanel
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
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelQuick = new System.Windows.Forms.TableLayoutPanel();
            this.quickView6 = new MissionPlanner.Controls.QuickView();
            this.quickView5 = new MissionPlanner.Controls.QuickView();
            this.quickView4 = new MissionPlanner.Controls.QuickView();
            this.quickView3 = new MissionPlanner.Controls.QuickView();
            this.quickView2 = new MissionPlanner.Controls.QuickView();
            this.quickView1 = new MissionPlanner.Controls.QuickView();
            this.tableLayoutPanelQuick.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelQuick
            // 
            this.tableLayoutPanelQuick.AutoScroll = true;
            this.tableLayoutPanelQuick.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelQuick.ColumnCount = 2;
            this.tableLayoutPanelQuick.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelQuick.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelQuick.Controls.Add(this.quickView6, 1, 2);
            this.tableLayoutPanelQuick.Controls.Add(this.quickView5, 0, 2);
            this.tableLayoutPanelQuick.Controls.Add(this.quickView4, 1, 1);
            this.tableLayoutPanelQuick.Controls.Add(this.quickView3, 0, 1);
            this.tableLayoutPanelQuick.Controls.Add(this.quickView2, 1, 0);
            this.tableLayoutPanelQuick.Controls.Add(this.quickView1, 0, 0);
            this.tableLayoutPanelQuick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelQuick.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelQuick.Name = "tableLayoutPanelQuick";
            this.tableLayoutPanelQuick.RowCount = 3;
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelQuick.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelQuick.Size = new System.Drawing.Size(340, 306);
            this.tableLayoutPanelQuick.TabIndex = 75;
            // 
            // quickView6
            // 
            this.quickView6.desc = "DistToMAV";
            this.quickView6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView6.Location = new System.Drawing.Point(173, 207);
            this.quickView6.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView6.Name = "quickView6";
            this.quickView6.number = 0D;
            this.quickView6.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.quickView6.numberformat = "0.00";
            this.quickView6.Size = new System.Drawing.Size(164, 96);
            this.quickView6.TabIndex = 10;
            // 
            // quickView5
            // 
            this.quickView5.desc = "verticalspeed";
            this.quickView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView5.Location = new System.Drawing.Point(3, 207);
            this.quickView5.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView5.Name = "quickView5";
            this.quickView5.number = 0D;
            this.quickView5.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(86)))));
            this.quickView5.numberformat = "0.00";
            this.quickView5.Size = new System.Drawing.Size(164, 96);
            this.quickView5.TabIndex = 9;
            // 
            // quickView4
            // 
            this.quickView4.desc = "yaw";
            this.quickView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView4.Location = new System.Drawing.Point(173, 105);
            this.quickView4.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView4.Name = "quickView4";
            this.quickView4.number = 0D;
            this.quickView4.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(83)))));
            this.quickView4.numberformat = "0.00";
            this.quickView4.Size = new System.Drawing.Size(164, 96);
            this.quickView4.TabIndex = 8;
            // 
            // quickView3
            // 
            this.quickView3.desc = "wp_dist";
            this.quickView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView3.Location = new System.Drawing.Point(3, 105);
            this.quickView3.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView3.Name = "quickView3";
            this.quickView3.number = 0D;
            this.quickView3.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(96)))), ((int)(((byte)(91)))));
            this.quickView3.numberformat = "0.00";
            this.quickView3.Size = new System.Drawing.Size(164, 96);
            this.quickView3.TabIndex = 3;
            // 
            // quickView2
            // 
            this.quickView2.desc = "groundspeed";
            this.quickView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView2.Location = new System.Drawing.Point(173, 3);
            this.quickView2.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView2.Name = "quickView2";
            this.quickView2.number = 0D;
            this.quickView2.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(132)))), ((int)(((byte)(46)))));
            this.quickView2.numberformat = "0.00";
            this.quickView2.Size = new System.Drawing.Size(164, 96);
            this.quickView2.TabIndex = 1;
            // 
            // quickView1
            // 
            this.quickView1.desc = "alt";
            this.quickView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickView1.Location = new System.Drawing.Point(3, 3);
            this.quickView1.MinimumSize = new System.Drawing.Size(100, 27);
            this.quickView1.Name = "quickView1";
            this.quickView1.number = 0D;
            this.quickView1.numberColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(151)))), ((int)(((byte)(248)))));
            this.quickView1.numberformat = "0.00";
            this.quickView1.Size = new System.Drawing.Size(164, 96);
            this.quickView1.TabIndex = 1;
            this.quickView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.quickView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.quickView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // ImportantMessagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanelQuick);
            this.Name = "ImportantMessagePanel";
            this.Size = new System.Drawing.Size(340, 306);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            this.tableLayoutPanelQuick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelQuick;
        private MissionPlanner.Controls.QuickView quickView1;
        private MissionPlanner.Controls.QuickView quickView6;
        private MissionPlanner.Controls.QuickView quickView5;
        private MissionPlanner.Controls.QuickView quickView4;
        private MissionPlanner.Controls.QuickView quickView3;
        private MissionPlanner.Controls.QuickView quickView2;
    }
}
