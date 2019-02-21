namespace SKYROVER.GCS.DeskTop.Controls
{
    partial class MapTools
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
            this.setHomeTool = new System.Windows.Forms.Button();
            this.ZoomInTool = new System.Windows.Forms.Button();
            this.ZoomOutTool = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.setHomeTool, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ZoomInTool, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ZoomOutTool, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(53, 153);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // setHomeTool
            // 
            this.setHomeTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setHomeTool.Location = new System.Drawing.Point(3, 3);
            this.setHomeTool.Name = "setHomeTool";
            this.setHomeTool.Size = new System.Drawing.Size(47, 44);
            this.setHomeTool.TabIndex = 0;
            this.setHomeTool.Text = "H";
            this.setHomeTool.UseVisualStyleBackColor = true;
            this.setHomeTool.Click += new System.EventHandler(this.setHomeTool_Click);
            // 
            // ZoomInTool
            // 
            this.ZoomInTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomInTool.Location = new System.Drawing.Point(3, 53);
            this.ZoomInTool.Name = "ZoomInTool";
            this.ZoomInTool.Size = new System.Drawing.Size(47, 46);
            this.ZoomInTool.TabIndex = 1;
            this.ZoomInTool.Text = "+";
            this.ZoomInTool.UseVisualStyleBackColor = true;
            this.ZoomInTool.Click += new System.EventHandler(this.ZoomInTool_Click);
            // 
            // ZoomOutTool
            // 
            this.ZoomOutTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomOutTool.Location = new System.Drawing.Point(3, 105);
            this.ZoomOutTool.Name = "ZoomOutTool";
            this.ZoomOutTool.Size = new System.Drawing.Size(47, 45);
            this.ZoomOutTool.TabIndex = 2;
            this.ZoomOutTool.Text = "-";
            this.ZoomOutTool.UseVisualStyleBackColor = true;
            this.ZoomOutTool.Click += new System.EventHandler(this.ZoomOutTool_Click);
            // 
            // MapTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MapTools";
            this.Size = new System.Drawing.Size(53, 153);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button setHomeTool;
        private System.Windows.Forms.Button ZoomInTool;
        private System.Windows.Forms.Button ZoomOutTool;
    }
}
