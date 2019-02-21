namespace SKYROVER.GCS.DeskTop.MenuItems
{
    partial class frmWayPointCommands
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
            this.btnClose = new System.Windows.Forms.Button();
            this.commandContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHoverMAV = new SKYROVER.GCS.DeskTop.Controls.BtnCommand();
            this.btnTriggerCAM = new SKYROVER.GCS.DeskTop.Controls.BtnCommand();
            this.btnThrowMAV = new SKYROVER.GCS.DeskTop.Controls.BtnCommand();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.commandContainer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(410, 616);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClose.Image = global::SKYROVER.GCS.DeskTop.Properties.Resources.goback;
            this.btnClose.Location = new System.Drawing.Point(3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 34);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // commandContainer
            // 
            this.commandContainer.AutoScroll = true;
            this.commandContainer.AutoScrollMargin = new System.Drawing.Size(1, 0);
            this.commandContainer.AutoSize = true;
            this.commandContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandContainer.Location = new System.Drawing.Point(3, 43);
            this.commandContainer.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.commandContainer.Name = "commandContainer";
            this.commandContainer.Size = new System.Drawing.Size(407, 510);
            this.commandContainer.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel2.Controls.Add(this.btnHoverMAV, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnTriggerCAM, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnThrowMAV, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 559);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(404, 54);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnHoverMAV
            // 
            this.btnHoverMAV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHoverMAV.BackColor = System.Drawing.Color.DarkGray;
            this.btnHoverMAV.Location = new System.Drawing.Point(3, 3);
            this.btnHoverMAV.Name = "btnHoverMAV";
            this.btnHoverMAV.Size = new System.Drawing.Size(127, 35);
            this.btnHoverMAV.TabIndex = 0;
            // 
            // btnTriggerCAM
            // 
            this.btnTriggerCAM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTriggerCAM.Location = new System.Drawing.Point(136, 3);
            this.btnTriggerCAM.Name = "btnTriggerCAM";
            this.btnTriggerCAM.Size = new System.Drawing.Size(131, 35);
            this.btnTriggerCAM.TabIndex = 1;
            // 
            // btnThrowMAV
            // 
            this.btnThrowMAV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThrowMAV.Location = new System.Drawing.Point(273, 3);
            this.btnThrowMAV.Name = "btnThrowMAV";
            this.btnThrowMAV.Size = new System.Drawing.Size(128, 35);
            this.btnThrowMAV.TabIndex = 2;
            // 
            // frmWayPointCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(410, 616);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "frmWayPointCommands";
            this.Opacity = 0.9D;
            this.Text = "frmWayPointCommands";
            this.TopMost = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel commandContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.BtnCommand btnHoverMAV;
        private Controls.BtnCommand btnTriggerCAM;
        private Controls.BtnCommand btnThrowMAV;
    }
}