namespace SKYROVER.GCS.DeskTop.Controls
{
    partial class CtlHoverMAV
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblHoverValue = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDesc = new System.Windows.Forms.Button();
            this.btnAsc = new System.Windows.Forms.Button();
            this.HoverTrackBar = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.Controls.Add(this.lblHoverValue, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDelete, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDesc, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAsc, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.HoverTrackBar, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(432, 156);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblHoverValue
            // 
            this.lblHoverValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHoverValue.AutoSize = true;
            this.lblHoverValue.Font = new System.Drawing.Font("黑体", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHoverValue.ForeColor = System.Drawing.Color.White;
            this.lblHoverValue.Location = new System.Drawing.Point(57, 70);
            this.lblHoverValue.Name = "lblHoverValue";
            this.lblHoverValue.Size = new System.Drawing.Size(318, 24);
            this.lblHoverValue.TabIndex = 6;
            this.lblHoverValue.Text = "value";
            this.lblHoverValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Image = global::SKYROVER.GCS.DeskTop.Properties.Resources.btnDelete;
            this.btnDelete.Location = new System.Drawing.Point(381, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 48);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::SKYROVER.GCS.DeskTop.Properties.Resources.btnDetail;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 48);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnDesc
            // 
            this.btnDesc.BackColor = System.Drawing.Color.Transparent;
            this.btnDesc.BackgroundImage = global::SKYROVER.GCS.DeskTop.Properties.Resources.btnDesc;
            this.btnDesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDesc.Location = new System.Drawing.Point(3, 97);
            this.btnDesc.Name = "btnDesc";
            this.btnDesc.Size = new System.Drawing.Size(48, 48);
            this.btnDesc.TabIndex = 8;
            this.btnDesc.UseVisualStyleBackColor = false;
            this.btnDesc.Click += new System.EventHandler(this.btnDesc_Click);
            // 
            // btnAsc
            // 
            this.btnAsc.BackColor = System.Drawing.Color.Transparent;
            this.btnAsc.BackgroundImage = global::SKYROVER.GCS.DeskTop.Properties.Resources.btnAsc;
            this.btnAsc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAsc.Location = new System.Drawing.Point(381, 97);
            this.btnAsc.Name = "btnAsc";
            this.btnAsc.Size = new System.Drawing.Size(48, 48);
            this.btnAsc.TabIndex = 8;
            this.btnAsc.UseVisualStyleBackColor = false;
            this.btnAsc.Click += new System.EventHandler(this.btnAsc_Click);
            // 
            // HoverTrackBar
            // 
            this.HoverTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.HoverTrackBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.HoverTrackBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HoverTrackBar.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.HoverTrackBar.DisabledBorderColor = System.Drawing.Color.Empty;
            this.HoverTrackBar.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.HoverTrackBar.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.HoverTrackBar.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.HoverTrackBar.Location = new System.Drawing.Point(57, 113);
            this.HoverTrackBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HoverTrackBar.Maximum = 40;
            this.HoverTrackBar.Minimum = 1;
            this.HoverTrackBar.Name = "HoverTrackBar";
            this.HoverTrackBar.Size = new System.Drawing.Size(318, 16);
            this.HoverTrackBar.Style = MetroSet_UI.Design.Style.Light;
            this.HoverTrackBar.StyleManager = null;
            this.HoverTrackBar.TabIndex = 5;
            this.HoverTrackBar.Text = "metroSetTrackBar1";
            this.HoverTrackBar.ThemeAuthor = "Narwin";
            this.HoverTrackBar.ThemeName = "MetroLite";
            this.HoverTrackBar.Value = 4;
            this.HoverTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.HoverTrackBar.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.HoverTrackBar_Scroll);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 3);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(3, 151);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(426, 2);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // CtlHoverMAV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CtlHoverMAV";
            this.Size = new System.Drawing.Size(432, 156);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroSet_UI.Controls.MetroSetTrackBar HoverTrackBar;
        private System.Windows.Forms.Label lblHoverValue;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDesc;
        private System.Windows.Forms.Button btnAsc;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
