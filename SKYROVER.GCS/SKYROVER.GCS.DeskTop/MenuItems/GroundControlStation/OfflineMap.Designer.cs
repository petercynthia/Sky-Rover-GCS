namespace SKYROVER.GCS.DeskTop.MenuItems.GroundControlStation
{
    partial class OfflineMap
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.z = new System.Windows.Forms.TableLayoutPanel();
            this.btnDrawarea = new System.Windows.Forms.Button();
            this.noramlMap = new System.Windows.Forms.RadioButton();
            this.moreDetailMap = new System.Windows.Forms.RadioButton();
            this.btnCancle = new System.Windows.Forms.Button();
            this.prgDownload = new System.Windows.Forms.ProgressBar();
            this.textBoxMemory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.z.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.z, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.prgDownload, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBoxMemory, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 9);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(341, 529);
            this.tableLayoutPanel1.TabIndex = 42;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(24, 152);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 15);
            this.lblStatus.TabIndex = 40;
            this.lblStatus.Text = "当前状态";
            // 
            // z
            // 
            this.z.ColumnCount = 2;
            this.z.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.z.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.z.Controls.Add(this.btnDrawarea, 0, 1);
            this.z.Controls.Add(this.noramlMap, 0, 0);
            this.z.Controls.Add(this.moreDetailMap, 1, 0);
            this.z.Controls.Add(this.btnCancle, 1, 1);
            this.z.Dock = System.Windows.Forms.DockStyle.Fill;
            this.z.Location = new System.Drawing.Point(23, 3);
            this.z.Name = "z";
            this.z.RowCount = 2;
            this.z.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.z.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.z.Size = new System.Drawing.Size(295, 71);
            this.z.TabIndex = 51;
            // 
            // btnDrawarea
            // 
            this.btnDrawarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDrawarea.Location = new System.Drawing.Point(34, 39);
            this.btnDrawarea.Margin = new System.Windows.Forms.Padding(4);
            this.btnDrawarea.Name = "btnDrawarea";
            this.btnDrawarea.Size = new System.Drawing.Size(79, 28);
            this.btnDrawarea.TabIndex = 39;
            this.btnDrawarea.Text = "下载";
            this.btnDrawarea.UseVisualStyleBackColor = true;
            this.btnDrawarea.Click += new System.EventHandler(this.btnDrawarea_Click);
            // 
            // noramlMap
            // 
            this.noramlMap.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.noramlMap.AutoSize = true;
            this.noramlMap.Checked = true;
            this.noramlMap.Location = new System.Drawing.Point(29, 3);
            this.noramlMap.Name = "noramlMap";
            this.noramlMap.Size = new System.Drawing.Size(88, 19);
            this.noramlMap.TabIndex = 0;
            this.noramlMap.TabStop = true;
            this.noramlMap.Text = "一般精度";
            this.noramlMap.UseVisualStyleBackColor = true;
            // 
            // moreDetailMap
            // 
            this.moreDetailMap.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.moreDetailMap.AutoSize = true;
            this.moreDetailMap.Location = new System.Drawing.Point(184, 3);
            this.moreDetailMap.Name = "moreDetailMap";
            this.moreDetailMap.Size = new System.Drawing.Size(73, 19);
            this.moreDetailMap.TabIndex = 1;
            this.moreDetailMap.Text = "高精度";
            this.moreDetailMap.UseVisualStyleBackColor = true;
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnCancle.Location = new System.Drawing.Point(173, 38);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(95, 30);
            this.btnCancle.TabIndex = 40;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // prgDownload
            // 
            this.prgDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgDownload.Location = new System.Drawing.Point(23, 170);
            this.prgDownload.Name = "prgDownload";
            this.prgDownload.Size = new System.Drawing.Size(295, 24);
            this.prgDownload.TabIndex = 52;
            // 
            // textBoxMemory
            // 
            this.textBoxMemory.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMemory.Location = new System.Drawing.Point(24, 226);
            this.textBoxMemory.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxMemory.Name = "textBoxMemory";
            this.textBoxMemory.ReadOnly = true;
            this.textBoxMemory.Size = new System.Drawing.Size(293, 34);
            this.textBoxMemory.TabIndex = 39;
            this.textBoxMemory.Text = "...";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 207);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 40;
            this.label1.Text = "磁盘空间";
            // 
            // OfflineMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OfflineMap";
            this.Size = new System.Drawing.Size(341, 529);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.z.ResumeLayout(false);
            this.z.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxMemory;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar prgDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel z;
        private System.Windows.Forms.RadioButton noramlMap;
        private System.Windows.Forms.RadioButton moreDetailMap;
        private System.Windows.Forms.Button btnDrawarea;
        private System.Windows.Forms.Button btnCancle;
    }
}
