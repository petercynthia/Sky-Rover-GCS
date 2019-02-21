
using MissionPlanner.Controls;

namespace SKYROVER.GCS.DeskTop.Calibration
{
    partial class ConfigAccelerometerCalibration
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigAccelerometerCalibration));
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_Accel_user = new System.Windows.Forms.Label();
            this.BUT_calib_accell = new MissionPlanner.Controls.MyButton();
            this.BUT_level = new MissionPlanner.Controls.MyButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 3);
            this.label4.Name = "label4";
            // 
            // lbl_Accel_user
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Accel_user, 3);
            resources.ApplyResources(this.lbl_Accel_user, "lbl_Accel_user");
            this.lbl_Accel_user.Name = "lbl_Accel_user";
            // 
            // BUT_calib_accell
            // 
            resources.ApplyResources(this.BUT_calib_accell, "BUT_calib_accell");
            this.BUT_calib_accell.Name = "BUT_calib_accell";
            this.BUT_calib_accell.UseVisualStyleBackColor = true;
            this.BUT_calib_accell.Click += new System.EventHandler(this.BUT_calib_accell_Click);
            // 
            // BUT_level
            // 
            resources.ApplyResources(this.BUT_level, "BUT_level");
            this.BUT_level.Name = "BUT_level";
            this.BUT_level.UseVisualStyleBackColor = true;
            this.BUT_level.Click += new System.EventHandler(this.BUT_level_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Accel_user, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.BUT_calib_accell, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BUT_level, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ConfigAccelerometerCalibration
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ConfigAccelerometerCalibration";
            resources.ApplyResources(this, "$this");
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private MyButton BUT_calib_accell;
        private System.Windows.Forms.Label lbl_Accel_user;
        private MyButton BUT_level;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
