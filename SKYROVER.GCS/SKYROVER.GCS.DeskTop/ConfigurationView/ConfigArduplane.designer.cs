using MissionPlanner.Controls;
using System.Windows.Controls;

namespace SKYROVER.GCS.DeskTop.ConfigurationView
{
    partial class ConfigArduplane
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigArduplane));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BUT_writePIDS = new System.Windows.Forms.Button();
            this.BUT_rerequestparams = new System.Windows.Forms.Button();
            this.BUT_refreshpart = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Q_FRAME_TYPE = new SKYROVER.GCS.DeskTop.Controls.MavlinkComboBox();
            this.Q_FRAME_CLASS = new SKYROVER.GCS.DeskTop.Controls.MavlinkComboBox();
            this.Q_ENABLE = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Q_A_ANG_RLL_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.Q_A_ANG_PIT_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.Q_A_ANG_YAW_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.Q_A_RAT_RLL_D = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.Q_A_RAT_RLL_FILT = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.Q_A_RAT_RLL_FF = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.Q_A_RAT_RLL_IMAX = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.Q_A_RAT_RLL_I = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.Q_A_RAT_RLL_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.Q_A_RAT_YAW_D = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.Q_A_RAT_YAW_FILT = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.Q_A_RAT_YAW_FF = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.Q_A_RAT_YAW_IMAX = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.Q_A_RAT_YAW_I = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.Q_A_RAT_YAW_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.Q_A_RAT_PIT_D = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.Q_A_RAT_PIT_FILT = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.Q_A_RAT_PIT_FF = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.Q_A_RAT_PIT_IMAX = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.Q_A_RAT_PIT_I = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.Q_A_RAT_PIT_P = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.Q_ASSIST_SPEED = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label43 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.Q_M_HOVER_LEARN = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_ASSIST_ANGLE = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.Q_TRANSITION_MS = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.ARSPD_FBW_MIN_1 = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.Q_RTL_ALT = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.Q_RTL_MODE = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_GUIDED_MODE = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_LAND_FINAL_ALT = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.Q_WP_SPEED_UP = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label81 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.Q_LAND_SPEED = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.Q_WP_RADIUS = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label80 = new System.Windows.Forms.Label();
            this.Q_WP_SPEED_DN = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.Q_WP_SPEED = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.Q_TILT_RATE_UP = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.Q_TILT_TYPE = new SKYROVER.GCS.DeskTop.Controls.MavlinkComboBox();
            this.label82 = new System.Windows.Forms.Label();
            this.Q_TILT_MASK_4 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_6 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_8 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_3 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_5 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_7 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_2 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_MASK_1 = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBox();
            this.Q_TILT_YAW_ANGLE = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label84 = new System.Windows.Forms.Label();
            this.Q_TILT_MAX = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label85 = new System.Windows.Forms.Label();
            this.Q_TILT_RATE_DN = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label86 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.Q_TAILSIT_MASK = new SKYROVER.GCS.DeskTop.Controls.MavlinkCheckBoxBitMask();
            this.Q_TAILSIT_INPUT = new SKYROVER.GCS.DeskTop.Controls.MavlinkComboBox();
            this.Q_TAILSIT_MASKCH = new SKYROVER.GCS.DeskTop.Controls.MavlinkComboBox();
            this.label91 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.Q_TAILSIT_ANGLE = new SKYROVER.GCS.DeskTop.Controls.MavlinkNumericUpDown();
            this.label93 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_RLL_P)).BeginInit();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_PIT_P)).BeginInit();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_YAW_P)).BeginInit();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_FILT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_FF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_P)).BeginInit();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_FILT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_FF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_P)).BeginInit();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_FILT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_FF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_P)).BeginInit();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_ASSIST_SPEED)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_ASSIST_ANGLE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TRANSITION_MS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARSPD_FBW_MIN_1)).BeginInit();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_RTL_ALT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_LAND_FINAL_ALT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED_UP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_LAND_SPEED)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_RADIUS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED_DN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED)).BeginInit();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_RATE_UP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_YAW_ANGLE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_MAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_RATE_DN)).BeginInit();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TAILSIT_ANGLE)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // BUT_writePIDS
            // 
            resources.ApplyResources(this.BUT_writePIDS, "BUT_writePIDS");
            this.BUT_writePIDS.Name = "BUT_writePIDS";
            this.BUT_writePIDS.UseVisualStyleBackColor = true;
            this.BUT_writePIDS.Click += new System.EventHandler(this.BUT_writePIDS_Click);
            // 
            // BUT_rerequestparams
            // 
            resources.ApplyResources(this.BUT_rerequestparams, "BUT_rerequestparams");
            this.BUT_rerequestparams.Name = "BUT_rerequestparams";
            this.BUT_rerequestparams.UseVisualStyleBackColor = true;
            this.BUT_rerequestparams.Click += new System.EventHandler(this.BUT_rerequestparams_Click);
            // 
            // BUT_refreshpart
            // 
            resources.ApplyResources(this.BUT_refreshpart, "BUT_refreshpart");
            this.BUT_refreshpart.Name = "BUT_refreshpart";
            this.BUT_refreshpart.UseVisualStyleBackColor = true;
            this.BUT_refreshpart.Click += new System.EventHandler(this.BUT_refreshpart_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Q_FRAME_TYPE);
            this.groupBox6.Controls.Add(this.Q_FRAME_CLASS);
            this.groupBox6.Controls.Add(this.Q_ENABLE);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label18);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // Q_FRAME_TYPE
            // 
            this.Q_FRAME_TYPE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Q_FRAME_TYPE, "Q_FRAME_TYPE");
            this.Q_FRAME_TYPE.FormattingEnabled = true;
            this.Q_FRAME_TYPE.Items.AddRange(new object[] {
            resources.GetString("Q_FRAME_TYPE.Items"),
            resources.GetString("Q_FRAME_TYPE.Items1"),
            resources.GetString("Q_FRAME_TYPE.Items2"),
            resources.GetString("Q_FRAME_TYPE.Items3"),
            resources.GetString("Q_FRAME_TYPE.Items4"),
            resources.GetString("Q_FRAME_TYPE.Items5"),
            resources.GetString("Q_FRAME_TYPE.Items6"),
            resources.GetString("Q_FRAME_TYPE.Items7")});
            this.Q_FRAME_TYPE.Name = "Q_FRAME_TYPE";
            this.Q_FRAME_TYPE.ParamName = null;
            this.Q_FRAME_TYPE.SubControl = null;
            this.Q_FRAME_TYPE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_FRAME_CLASS
            // 
            this.Q_FRAME_CLASS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Q_FRAME_CLASS, "Q_FRAME_CLASS");
            this.Q_FRAME_CLASS.FormattingEnabled = true;
            this.Q_FRAME_CLASS.Items.AddRange(new object[] {
            resources.GetString("Q_FRAME_CLASS.Items"),
            resources.GetString("Q_FRAME_CLASS.Items1"),
            resources.GetString("Q_FRAME_CLASS.Items2"),
            resources.GetString("Q_FRAME_CLASS.Items3"),
            resources.GetString("Q_FRAME_CLASS.Items4"),
            resources.GetString("Q_FRAME_CLASS.Items5"),
            resources.GetString("Q_FRAME_CLASS.Items6"),
            resources.GetString("Q_FRAME_CLASS.Items7")});
            this.Q_FRAME_CLASS.Name = "Q_FRAME_CLASS";
            this.Q_FRAME_CLASS.ParamName = null;
            this.Q_FRAME_CLASS.SubControl = null;
            this.Q_FRAME_CLASS.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_ENABLE
            // 
            resources.ApplyResources(this.Q_ENABLE, "Q_ENABLE");
            this.Q_ENABLE.Name = "Q_ENABLE";
            this.Q_ENABLE.OffValue = 0D;
            this.Q_ENABLE.OnValue = 1D;
            this.Q_ENABLE.ParamName = null;
            this.Q_ENABLE.UseVisualStyleBackColor = true;
            this.Q_ENABLE.CheckStateChanged += new System.EventHandler(this.Q_ENABLE_CheckStateChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Q_A_ANG_RLL_P);
            this.groupBox7.Controls.Add(this.label19);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // Q_A_ANG_RLL_P
            // 
            resources.ApplyResources(this.Q_A_ANG_RLL_P, "Q_A_ANG_RLL_P");
            this.Q_A_ANG_RLL_P.Max = 1F;
            this.Q_A_ANG_RLL_P.Min = 0F;
            this.Q_A_ANG_RLL_P.Name = "Q_A_ANG_RLL_P";
            this.Q_A_ANG_RLL_P.ParamName = null;
            this.Q_A_ANG_RLL_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.Q_A_ANG_PIT_P);
            this.groupBox11.Controls.Add(this.label20);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // Q_A_ANG_PIT_P
            // 
            resources.ApplyResources(this.Q_A_ANG_PIT_P, "Q_A_ANG_PIT_P");
            this.Q_A_ANG_PIT_P.Max = 1F;
            this.Q_A_ANG_PIT_P.Min = 0F;
            this.Q_A_ANG_PIT_P.Name = "Q_A_ANG_PIT_P";
            this.Q_A_ANG_PIT_P.ParamName = null;
            this.Q_A_ANG_PIT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.Q_A_ANG_YAW_P);
            this.groupBox15.Controls.Add(this.label21);
            resources.ApplyResources(this.groupBox15, "groupBox15");
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.TabStop = false;
            // 
            // Q_A_ANG_YAW_P
            // 
            resources.ApplyResources(this.Q_A_ANG_YAW_P, "Q_A_ANG_YAW_P");
            this.Q_A_ANG_YAW_P.Max = 1F;
            this.Q_A_ANG_YAW_P.Min = 0F;
            this.Q_A_ANG_YAW_P.Name = "Q_A_ANG_YAW_P";
            this.Q_A_ANG_YAW_P.ParamName = null;
            this.Q_A_ANG_YAW_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_D);
            this.groupBox17.Controls.Add(this.label22);
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_FILT);
            this.groupBox17.Controls.Add(this.label27);
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_FF);
            this.groupBox17.Controls.Add(this.label23);
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_IMAX);
            this.groupBox17.Controls.Add(this.label24);
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_I);
            this.groupBox17.Controls.Add(this.label25);
            this.groupBox17.Controls.Add(this.Q_A_RAT_RLL_P);
            this.groupBox17.Controls.Add(this.label26);
            resources.ApplyResources(this.groupBox17, "groupBox17");
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.TabStop = false;
            // 
            // Q_A_RAT_RLL_D
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_D, "Q_A_RAT_RLL_D");
            this.Q_A_RAT_RLL_D.Max = 1F;
            this.Q_A_RAT_RLL_D.Min = 0F;
            this.Q_A_RAT_RLL_D.Name = "Q_A_RAT_RLL_D";
            this.Q_A_RAT_RLL_D.ParamName = null;
            this.Q_A_RAT_RLL_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // Q_A_RAT_RLL_FILT
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_FILT, "Q_A_RAT_RLL_FILT");
            this.Q_A_RAT_RLL_FILT.Max = 1F;
            this.Q_A_RAT_RLL_FILT.Min = 0F;
            this.Q_A_RAT_RLL_FILT.Name = "Q_A_RAT_RLL_FILT";
            this.Q_A_RAT_RLL_FILT.ParamName = null;
            this.Q_A_RAT_RLL_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // Q_A_RAT_RLL_FF
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_FF, "Q_A_RAT_RLL_FF");
            this.Q_A_RAT_RLL_FF.Max = 1F;
            this.Q_A_RAT_RLL_FF.Min = 0F;
            this.Q_A_RAT_RLL_FF.Name = "Q_A_RAT_RLL_FF";
            this.Q_A_RAT_RLL_FF.ParamName = null;
            this.Q_A_RAT_RLL_FF.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // Q_A_RAT_RLL_IMAX
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_IMAX, "Q_A_RAT_RLL_IMAX");
            this.Q_A_RAT_RLL_IMAX.Max = 1F;
            this.Q_A_RAT_RLL_IMAX.Min = 0F;
            this.Q_A_RAT_RLL_IMAX.Name = "Q_A_RAT_RLL_IMAX";
            this.Q_A_RAT_RLL_IMAX.ParamName = null;
            this.Q_A_RAT_RLL_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // Q_A_RAT_RLL_I
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_I, "Q_A_RAT_RLL_I");
            this.Q_A_RAT_RLL_I.Max = 1F;
            this.Q_A_RAT_RLL_I.Min = 0F;
            this.Q_A_RAT_RLL_I.Name = "Q_A_RAT_RLL_I";
            this.Q_A_RAT_RLL_I.ParamName = null;
            this.Q_A_RAT_RLL_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // Q_A_RAT_RLL_P
            // 
            resources.ApplyResources(this.Q_A_RAT_RLL_P, "Q_A_RAT_RLL_P");
            this.Q_A_RAT_RLL_P.Max = 1F;
            this.Q_A_RAT_RLL_P.Min = 0F;
            this.Q_A_RAT_RLL_P.Name = "Q_A_RAT_RLL_P";
            this.Q_A_RAT_RLL_P.ParamName = null;
            this.Q_A_RAT_RLL_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_D);
            this.groupBox18.Controls.Add(this.label28);
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_FILT);
            this.groupBox18.Controls.Add(this.label29);
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_FF);
            this.groupBox18.Controls.Add(this.label30);
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_IMAX);
            this.groupBox18.Controls.Add(this.label31);
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_I);
            this.groupBox18.Controls.Add(this.label32);
            this.groupBox18.Controls.Add(this.Q_A_RAT_YAW_P);
            this.groupBox18.Controls.Add(this.label33);
            resources.ApplyResources(this.groupBox18, "groupBox18");
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.TabStop = false;
            // 
            // Q_A_RAT_YAW_D
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_D, "Q_A_RAT_YAW_D");
            this.Q_A_RAT_YAW_D.Max = 1F;
            this.Q_A_RAT_YAW_D.Min = 0F;
            this.Q_A_RAT_YAW_D.Name = "Q_A_RAT_YAW_D";
            this.Q_A_RAT_YAW_D.ParamName = null;
            this.Q_A_RAT_YAW_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // Q_A_RAT_YAW_FILT
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_FILT, "Q_A_RAT_YAW_FILT");
            this.Q_A_RAT_YAW_FILT.Max = 1F;
            this.Q_A_RAT_YAW_FILT.Min = 0F;
            this.Q_A_RAT_YAW_FILT.Name = "Q_A_RAT_YAW_FILT";
            this.Q_A_RAT_YAW_FILT.ParamName = null;
            this.Q_A_RAT_YAW_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // Q_A_RAT_YAW_FF
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_FF, "Q_A_RAT_YAW_FF");
            this.Q_A_RAT_YAW_FF.Max = 1F;
            this.Q_A_RAT_YAW_FF.Min = 0F;
            this.Q_A_RAT_YAW_FF.Name = "Q_A_RAT_YAW_FF";
            this.Q_A_RAT_YAW_FF.ParamName = null;
            this.Q_A_RAT_YAW_FF.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // Q_A_RAT_YAW_IMAX
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_IMAX, "Q_A_RAT_YAW_IMAX");
            this.Q_A_RAT_YAW_IMAX.Max = 1F;
            this.Q_A_RAT_YAW_IMAX.Min = 0F;
            this.Q_A_RAT_YAW_IMAX.Name = "Q_A_RAT_YAW_IMAX";
            this.Q_A_RAT_YAW_IMAX.ParamName = null;
            this.Q_A_RAT_YAW_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // Q_A_RAT_YAW_I
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_I, "Q_A_RAT_YAW_I");
            this.Q_A_RAT_YAW_I.Max = 1F;
            this.Q_A_RAT_YAW_I.Min = 0F;
            this.Q_A_RAT_YAW_I.Name = "Q_A_RAT_YAW_I";
            this.Q_A_RAT_YAW_I.ParamName = null;
            this.Q_A_RAT_YAW_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // Q_A_RAT_YAW_P
            // 
            resources.ApplyResources(this.Q_A_RAT_YAW_P, "Q_A_RAT_YAW_P");
            this.Q_A_RAT_YAW_P.Max = 1F;
            this.Q_A_RAT_YAW_P.Min = 0F;
            this.Q_A_RAT_YAW_P.Name = "Q_A_RAT_YAW_P";
            this.Q_A_RAT_YAW_P.ParamName = null;
            this.Q_A_RAT_YAW_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_D);
            this.groupBox19.Controls.Add(this.label34);
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_FILT);
            this.groupBox19.Controls.Add(this.label35);
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_FF);
            this.groupBox19.Controls.Add(this.label36);
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_IMAX);
            this.groupBox19.Controls.Add(this.label40);
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_I);
            this.groupBox19.Controls.Add(this.label41);
            this.groupBox19.Controls.Add(this.Q_A_RAT_PIT_P);
            this.groupBox19.Controls.Add(this.label42);
            resources.ApplyResources(this.groupBox19, "groupBox19");
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.TabStop = false;
            // 
            // Q_A_RAT_PIT_D
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_D, "Q_A_RAT_PIT_D");
            this.Q_A_RAT_PIT_D.Max = 1F;
            this.Q_A_RAT_PIT_D.Min = 0F;
            this.Q_A_RAT_PIT_D.Name = "Q_A_RAT_PIT_D";
            this.Q_A_RAT_PIT_D.ParamName = null;
            this.Q_A_RAT_PIT_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.Name = "label34";
            // 
            // Q_A_RAT_PIT_FILT
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_FILT, "Q_A_RAT_PIT_FILT");
            this.Q_A_RAT_PIT_FILT.Max = 1F;
            this.Q_A_RAT_PIT_FILT.Min = 0F;
            this.Q_A_RAT_PIT_FILT.Name = "Q_A_RAT_PIT_FILT";
            this.Q_A_RAT_PIT_FILT.ParamName = null;
            this.Q_A_RAT_PIT_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // Q_A_RAT_PIT_FF
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_FF, "Q_A_RAT_PIT_FF");
            this.Q_A_RAT_PIT_FF.Max = 1F;
            this.Q_A_RAT_PIT_FF.Min = 0F;
            this.Q_A_RAT_PIT_FF.Name = "Q_A_RAT_PIT_FF";
            this.Q_A_RAT_PIT_FF.ParamName = null;
            this.Q_A_RAT_PIT_FF.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // Q_A_RAT_PIT_IMAX
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_IMAX, "Q_A_RAT_PIT_IMAX");
            this.Q_A_RAT_PIT_IMAX.Max = 1F;
            this.Q_A_RAT_PIT_IMAX.Min = 0F;
            this.Q_A_RAT_PIT_IMAX.Name = "Q_A_RAT_PIT_IMAX";
            this.Q_A_RAT_PIT_IMAX.ParamName = null;
            this.Q_A_RAT_PIT_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // Q_A_RAT_PIT_I
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_I, "Q_A_RAT_PIT_I");
            this.Q_A_RAT_PIT_I.Max = 1F;
            this.Q_A_RAT_PIT_I.Min = 0F;
            this.Q_A_RAT_PIT_I.Name = "Q_A_RAT_PIT_I";
            this.Q_A_RAT_PIT_I.ParamName = null;
            this.Q_A_RAT_PIT_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
            // 
            // Q_A_RAT_PIT_P
            // 
            resources.ApplyResources(this.Q_A_RAT_PIT_P, "Q_A_RAT_PIT_P");
            this.Q_A_RAT_PIT_P.Max = 1F;
            this.Q_A_RAT_PIT_P.Min = 0F;
            this.Q_A_RAT_PIT_P.Name = "Q_A_RAT_PIT_P";
            this.Q_A_RAT_PIT_P.ParamName = null;
            this.Q_A_RAT_PIT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.Q_ASSIST_SPEED);
            this.groupBox20.Controls.Add(this.label43);
            this.groupBox20.Controls.Add(this.label45);
            this.groupBox20.Controls.Add(this.Q_M_HOVER_LEARN);
            this.groupBox20.Controls.Add(this.Q_ASSIST_ANGLE);
            this.groupBox20.Controls.Add(this.label46);
            this.groupBox20.Controls.Add(this.Q_TRANSITION_MS);
            this.groupBox20.Controls.Add(this.label47);
            this.groupBox20.Controls.Add(this.ARSPD_FBW_MIN_1);
            this.groupBox20.Controls.Add(this.label48);
            resources.ApplyResources(this.groupBox20, "groupBox20");
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.TabStop = false;
            // 
            // Q_ASSIST_SPEED
            // 
            resources.ApplyResources(this.Q_ASSIST_SPEED, "Q_ASSIST_SPEED");
            this.Q_ASSIST_SPEED.Max = 1F;
            this.Q_ASSIST_SPEED.Min = 0F;
            this.Q_ASSIST_SPEED.Name = "Q_ASSIST_SPEED";
            this.Q_ASSIST_SPEED.ParamName = null;
            this.Q_ASSIST_SPEED.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // Q_M_HOVER_LEARN
            // 
            resources.ApplyResources(this.Q_M_HOVER_LEARN, "Q_M_HOVER_LEARN");
            this.Q_M_HOVER_LEARN.Name = "Q_M_HOVER_LEARN";
            this.Q_M_HOVER_LEARN.OffValue = 0D;
            this.Q_M_HOVER_LEARN.OnValue = 1D;
            this.Q_M_HOVER_LEARN.ParamName = null;
            this.Q_M_HOVER_LEARN.UseVisualStyleBackColor = true;
            // 
            // Q_ASSIST_ANGLE
            // 
            resources.ApplyResources(this.Q_ASSIST_ANGLE, "Q_ASSIST_ANGLE");
            this.Q_ASSIST_ANGLE.Max = 1F;
            this.Q_ASSIST_ANGLE.Min = 0F;
            this.Q_ASSIST_ANGLE.Name = "Q_ASSIST_ANGLE";
            this.Q_ASSIST_ANGLE.ParamName = null;
            this.Q_ASSIST_ANGLE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // Q_TRANSITION_MS
            // 
            resources.ApplyResources(this.Q_TRANSITION_MS, "Q_TRANSITION_MS");
            this.Q_TRANSITION_MS.Max = 1F;
            this.Q_TRANSITION_MS.Min = 0F;
            this.Q_TRANSITION_MS.Name = "Q_TRANSITION_MS";
            this.Q_TRANSITION_MS.ParamName = null;
            this.Q_TRANSITION_MS.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // ARSPD_FBW_MIN_1
            // 
            resources.ApplyResources(this.ARSPD_FBW_MIN_1, "ARSPD_FBW_MIN_1");
            this.ARSPD_FBW_MIN_1.Max = 1F;
            this.ARSPD_FBW_MIN_1.Min = 0F;
            this.ARSPD_FBW_MIN_1.Name = "ARSPD_FBW_MIN_1";
            this.ARSPD_FBW_MIN_1.ParamName = null;
            this.ARSPD_FBW_MIN_1.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.Q_RTL_ALT);
            this.groupBox21.Controls.Add(this.label44);
            this.groupBox21.Controls.Add(this.Q_RTL_MODE);
            this.groupBox21.Controls.Add(this.Q_GUIDED_MODE);
            this.groupBox21.Controls.Add(this.Q_LAND_FINAL_ALT);
            this.groupBox21.Controls.Add(this.Q_WP_SPEED_UP);
            this.groupBox21.Controls.Add(this.label81);
            this.groupBox21.Controls.Add(this.label61);
            this.groupBox21.Controls.Add(this.Q_LAND_SPEED);
            this.groupBox21.Controls.Add(this.Q_WP_RADIUS);
            this.groupBox21.Controls.Add(this.label80);
            this.groupBox21.Controls.Add(this.Q_WP_SPEED_DN);
            this.groupBox21.Controls.Add(this.label62);
            this.groupBox21.Controls.Add(this.label79);
            this.groupBox21.Controls.Add(this.Q_WP_SPEED);
            this.groupBox21.Controls.Add(this.label63);
            this.groupBox21.Controls.Add(this.label64);
            this.groupBox21.Controls.Add(this.label77);
            resources.ApplyResources(this.groupBox21, "groupBox21");
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.TabStop = false;
            // 
            // Q_RTL_ALT
            // 
            resources.ApplyResources(this.Q_RTL_ALT, "Q_RTL_ALT");
            this.Q_RTL_ALT.Max = 1F;
            this.Q_RTL_ALT.Min = 0F;
            this.Q_RTL_ALT.Name = "Q_RTL_ALT";
            this.Q_RTL_ALT.ParamName = null;
            this.Q_RTL_ALT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // Q_RTL_MODE
            // 
            resources.ApplyResources(this.Q_RTL_MODE, "Q_RTL_MODE");
            this.Q_RTL_MODE.Name = "Q_RTL_MODE";
            this.Q_RTL_MODE.OffValue = 0D;
            this.Q_RTL_MODE.OnValue = 1D;
            this.Q_RTL_MODE.ParamName = null;
            this.Q_RTL_MODE.UseVisualStyleBackColor = true;
            // 
            // Q_GUIDED_MODE
            // 
            resources.ApplyResources(this.Q_GUIDED_MODE, "Q_GUIDED_MODE");
            this.Q_GUIDED_MODE.Name = "Q_GUIDED_MODE";
            this.Q_GUIDED_MODE.OffValue = 0D;
            this.Q_GUIDED_MODE.OnValue = 1D;
            this.Q_GUIDED_MODE.ParamName = null;
            this.Q_GUIDED_MODE.UseVisualStyleBackColor = true;
            // 
            // Q_LAND_FINAL_ALT
            // 
            resources.ApplyResources(this.Q_LAND_FINAL_ALT, "Q_LAND_FINAL_ALT");
            this.Q_LAND_FINAL_ALT.Max = 1F;
            this.Q_LAND_FINAL_ALT.Min = 0F;
            this.Q_LAND_FINAL_ALT.Name = "Q_LAND_FINAL_ALT";
            this.Q_LAND_FINAL_ALT.ParamName = null;
            this.Q_LAND_FINAL_ALT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_WP_SPEED_UP
            // 
            resources.ApplyResources(this.Q_WP_SPEED_UP, "Q_WP_SPEED_UP");
            this.Q_WP_SPEED_UP.Max = 1F;
            this.Q_WP_SPEED_UP.Min = 0F;
            this.Q_WP_SPEED_UP.Name = "Q_WP_SPEED_UP";
            this.Q_WP_SPEED_UP.ParamName = null;
            this.Q_WP_SPEED_UP.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label81
            // 
            resources.ApplyResources(this.label81, "label81");
            this.label81.Name = "label81";
            // 
            // label61
            // 
            resources.ApplyResources(this.label61, "label61");
            this.label61.Name = "label61";
            // 
            // Q_LAND_SPEED
            // 
            resources.ApplyResources(this.Q_LAND_SPEED, "Q_LAND_SPEED");
            this.Q_LAND_SPEED.Max = 1F;
            this.Q_LAND_SPEED.Min = 0F;
            this.Q_LAND_SPEED.Name = "Q_LAND_SPEED";
            this.Q_LAND_SPEED.ParamName = null;
            this.Q_LAND_SPEED.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_WP_RADIUS
            // 
            resources.ApplyResources(this.Q_WP_RADIUS, "Q_WP_RADIUS");
            this.Q_WP_RADIUS.Max = 1F;
            this.Q_WP_RADIUS.Min = 0F;
            this.Q_WP_RADIUS.Name = "Q_WP_RADIUS";
            this.Q_WP_RADIUS.ParamName = null;
            this.Q_WP_RADIUS.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label80
            // 
            resources.ApplyResources(this.label80, "label80");
            this.label80.Name = "label80";
            // 
            // Q_WP_SPEED_DN
            // 
            resources.ApplyResources(this.Q_WP_SPEED_DN, "Q_WP_SPEED_DN");
            this.Q_WP_SPEED_DN.Max = 1F;
            this.Q_WP_SPEED_DN.Min = 0F;
            this.Q_WP_SPEED_DN.Name = "Q_WP_SPEED_DN";
            this.Q_WP_SPEED_DN.ParamName = null;
            this.Q_WP_SPEED_DN.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label62
            // 
            resources.ApplyResources(this.label62, "label62");
            this.label62.Name = "label62";
            // 
            // label79
            // 
            resources.ApplyResources(this.label79, "label79");
            this.label79.Name = "label79";
            // 
            // Q_WP_SPEED
            // 
            resources.ApplyResources(this.Q_WP_SPEED, "Q_WP_SPEED");
            this.Q_WP_SPEED.Max = 1F;
            this.Q_WP_SPEED.Min = 0F;
            this.Q_WP_SPEED.Name = "Q_WP_SPEED";
            this.Q_WP_SPEED.ParamName = null;
            this.Q_WP_SPEED.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label63
            // 
            resources.ApplyResources(this.label63, "label63");
            this.label63.Name = "label63";
            // 
            // label64
            // 
            resources.ApplyResources(this.label64, "label64");
            this.label64.Name = "label64";
            // 
            // label77
            // 
            resources.ApplyResources(this.label77, "label77");
            this.label77.Name = "label77";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.Q_TILT_RATE_UP);
            this.groupBox22.Controls.Add(this.Q_TILT_TYPE);
            this.groupBox22.Controls.Add(this.label82);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_4);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_6);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_8);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_3);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_5);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_7);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_2);
            this.groupBox22.Controls.Add(this.Q_TILT_MASK_1);
            this.groupBox22.Controls.Add(this.Q_TILT_YAW_ANGLE);
            this.groupBox22.Controls.Add(this.label84);
            this.groupBox22.Controls.Add(this.Q_TILT_MAX);
            this.groupBox22.Controls.Add(this.label85);
            this.groupBox22.Controls.Add(this.Q_TILT_RATE_DN);
            this.groupBox22.Controls.Add(this.label86);
            this.groupBox22.Controls.Add(this.label87);
            this.groupBox22.Controls.Add(this.label88);
            resources.ApplyResources(this.groupBox22, "groupBox22");
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.TabStop = false;
            // 
            // Q_TILT_RATE_UP
            // 
            resources.ApplyResources(this.Q_TILT_RATE_UP, "Q_TILT_RATE_UP");
            this.Q_TILT_RATE_UP.Max = 1F;
            this.Q_TILT_RATE_UP.Min = 0F;
            this.Q_TILT_RATE_UP.Name = "Q_TILT_RATE_UP";
            this.Q_TILT_RATE_UP.ParamName = null;
            this.Q_TILT_RATE_UP.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_TILT_TYPE
            // 
            this.Q_TILT_TYPE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Q_TILT_TYPE, "Q_TILT_TYPE");
            this.Q_TILT_TYPE.FormattingEnabled = true;
            this.Q_TILT_TYPE.Items.AddRange(new object[] {
            resources.GetString("Q_TILT_TYPE.Items"),
            resources.GetString("Q_TILT_TYPE.Items1"),
            resources.GetString("Q_TILT_TYPE.Items2")});
            this.Q_TILT_TYPE.Name = "Q_TILT_TYPE";
            this.Q_TILT_TYPE.ParamName = null;
            this.Q_TILT_TYPE.SubControl = null;
            this.Q_TILT_TYPE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.Name = "label82";
            // 
            // Q_TILT_MASK_4
            // 
            resources.ApplyResources(this.Q_TILT_MASK_4, "Q_TILT_MASK_4");
            this.Q_TILT_MASK_4.Name = "Q_TILT_MASK_4";
            this.Q_TILT_MASK_4.OffValue = 0D;
            this.Q_TILT_MASK_4.OnValue = 1D;
            this.Q_TILT_MASK_4.ParamName = null;
            this.Q_TILT_MASK_4.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_4.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_6
            // 
            resources.ApplyResources(this.Q_TILT_MASK_6, "Q_TILT_MASK_6");
            this.Q_TILT_MASK_6.Name = "Q_TILT_MASK_6";
            this.Q_TILT_MASK_6.OffValue = 0D;
            this.Q_TILT_MASK_6.OnValue = 1D;
            this.Q_TILT_MASK_6.ParamName = null;
            this.Q_TILT_MASK_6.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_6.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_8
            // 
            resources.ApplyResources(this.Q_TILT_MASK_8, "Q_TILT_MASK_8");
            this.Q_TILT_MASK_8.Name = "Q_TILT_MASK_8";
            this.Q_TILT_MASK_8.OffValue = 0D;
            this.Q_TILT_MASK_8.OnValue = 1D;
            this.Q_TILT_MASK_8.ParamName = null;
            this.Q_TILT_MASK_8.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_8.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_3
            // 
            resources.ApplyResources(this.Q_TILT_MASK_3, "Q_TILT_MASK_3");
            this.Q_TILT_MASK_3.Name = "Q_TILT_MASK_3";
            this.Q_TILT_MASK_3.OffValue = 0D;
            this.Q_TILT_MASK_3.OnValue = 1D;
            this.Q_TILT_MASK_3.ParamName = null;
            this.Q_TILT_MASK_3.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_3.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_5
            // 
            resources.ApplyResources(this.Q_TILT_MASK_5, "Q_TILT_MASK_5");
            this.Q_TILT_MASK_5.Name = "Q_TILT_MASK_5";
            this.Q_TILT_MASK_5.OffValue = 0D;
            this.Q_TILT_MASK_5.OnValue = 1D;
            this.Q_TILT_MASK_5.ParamName = null;
            this.Q_TILT_MASK_5.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_5.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_7
            // 
            resources.ApplyResources(this.Q_TILT_MASK_7, "Q_TILT_MASK_7");
            this.Q_TILT_MASK_7.Name = "Q_TILT_MASK_7";
            this.Q_TILT_MASK_7.OffValue = 0D;
            this.Q_TILT_MASK_7.OnValue = 1D;
            this.Q_TILT_MASK_7.ParamName = null;
            this.Q_TILT_MASK_7.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_7.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_2
            // 
            resources.ApplyResources(this.Q_TILT_MASK_2, "Q_TILT_MASK_2");
            this.Q_TILT_MASK_2.Name = "Q_TILT_MASK_2";
            this.Q_TILT_MASK_2.OffValue = 0D;
            this.Q_TILT_MASK_2.OnValue = 1D;
            this.Q_TILT_MASK_2.ParamName = null;
            this.Q_TILT_MASK_2.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_2.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_MASK_1
            // 
            resources.ApplyResources(this.Q_TILT_MASK_1, "Q_TILT_MASK_1");
            this.Q_TILT_MASK_1.Name = "Q_TILT_MASK_1";
            this.Q_TILT_MASK_1.OffValue = 0D;
            this.Q_TILT_MASK_1.OnValue = 1D;
            this.Q_TILT_MASK_1.ParamName = null;
            this.Q_TILT_MASK_1.UseVisualStyleBackColor = true;
            this.Q_TILT_MASK_1.CheckStateChanged += new System.EventHandler(this.Q_TILT_MASK_1_CheckStateChanged);
            // 
            // Q_TILT_YAW_ANGLE
            // 
            resources.ApplyResources(this.Q_TILT_YAW_ANGLE, "Q_TILT_YAW_ANGLE");
            this.Q_TILT_YAW_ANGLE.Max = 1F;
            this.Q_TILT_YAW_ANGLE.Min = 0F;
            this.Q_TILT_YAW_ANGLE.Name = "Q_TILT_YAW_ANGLE";
            this.Q_TILT_YAW_ANGLE.ParamName = null;
            this.Q_TILT_YAW_ANGLE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label84
            // 
            resources.ApplyResources(this.label84, "label84");
            this.label84.Name = "label84";
            // 
            // Q_TILT_MAX
            // 
            resources.ApplyResources(this.Q_TILT_MAX, "Q_TILT_MAX");
            this.Q_TILT_MAX.Max = 1F;
            this.Q_TILT_MAX.Min = 0F;
            this.Q_TILT_MAX.Name = "Q_TILT_MAX";
            this.Q_TILT_MAX.ParamName = null;
            this.Q_TILT_MAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label85
            // 
            resources.ApplyResources(this.label85, "label85");
            this.label85.Name = "label85";
            // 
            // Q_TILT_RATE_DN
            // 
            resources.ApplyResources(this.Q_TILT_RATE_DN, "Q_TILT_RATE_DN");
            this.Q_TILT_RATE_DN.Max = 1F;
            this.Q_TILT_RATE_DN.Min = 0F;
            this.Q_TILT_RATE_DN.Name = "Q_TILT_RATE_DN";
            this.Q_TILT_RATE_DN.ParamName = null;
            this.Q_TILT_RATE_DN.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label86
            // 
            resources.ApplyResources(this.label86, "label86");
            this.label86.Name = "label86";
            // 
            // label87
            // 
            resources.ApplyResources(this.label87, "label87");
            this.label87.Name = "label87";
            // 
            // label88
            // 
            resources.ApplyResources(this.label88, "label88");
            this.label88.Name = "label88";
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.Q_TAILSIT_MASK);
            this.groupBox23.Controls.Add(this.Q_TAILSIT_INPUT);
            this.groupBox23.Controls.Add(this.Q_TAILSIT_MASKCH);
            this.groupBox23.Controls.Add(this.label91);
            this.groupBox23.Controls.Add(this.label92);
            this.groupBox23.Controls.Add(this.Q_TAILSIT_ANGLE);
            this.groupBox23.Controls.Add(this.label93);
            resources.ApplyResources(this.groupBox23, "groupBox23");
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.TabStop = false;
            // 
            // Q_TAILSIT_MASK
            // 
            resources.ApplyResources(this.Q_TAILSIT_MASK, "Q_TAILSIT_MASK");
            this.Q_TAILSIT_MASK.Name = "Q_TAILSIT_MASK";
            this.Q_TAILSIT_MASK.ParamName = null;
            this.Q_TAILSIT_MASK.Value = 0F;
            // 
            // Q_TAILSIT_INPUT
            // 
            this.Q_TAILSIT_INPUT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Q_TAILSIT_INPUT, "Q_TAILSIT_INPUT");
            this.Q_TAILSIT_INPUT.FormattingEnabled = true;
            this.Q_TAILSIT_INPUT.Items.AddRange(new object[] {
            resources.GetString("Q_TAILSIT_INPUT.Items"),
            resources.GetString("Q_TAILSIT_INPUT.Items1")});
            this.Q_TAILSIT_INPUT.Name = "Q_TAILSIT_INPUT";
            this.Q_TAILSIT_INPUT.ParamName = null;
            this.Q_TAILSIT_INPUT.SubControl = null;
            this.Q_TAILSIT_INPUT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // Q_TAILSIT_MASKCH
            // 
            this.Q_TAILSIT_MASKCH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Q_TAILSIT_MASKCH, "Q_TAILSIT_MASKCH");
            this.Q_TAILSIT_MASKCH.FormattingEnabled = true;
            this.Q_TAILSIT_MASKCH.Items.AddRange(new object[] {
            resources.GetString("Q_TAILSIT_MASKCH.Items"),
            resources.GetString("Q_TAILSIT_MASKCH.Items1"),
            resources.GetString("Q_TAILSIT_MASKCH.Items2"),
            resources.GetString("Q_TAILSIT_MASKCH.Items3"),
            resources.GetString("Q_TAILSIT_MASKCH.Items4"),
            resources.GetString("Q_TAILSIT_MASKCH.Items5"),
            resources.GetString("Q_TAILSIT_MASKCH.Items6"),
            resources.GetString("Q_TAILSIT_MASKCH.Items7"),
            resources.GetString("Q_TAILSIT_MASKCH.Items8")});
            this.Q_TAILSIT_MASKCH.Name = "Q_TAILSIT_MASKCH";
            this.Q_TAILSIT_MASKCH.ParamName = null;
            this.Q_TAILSIT_MASKCH.SubControl = null;
            this.Q_TAILSIT_MASKCH.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label91
            // 
            resources.ApplyResources(this.label91, "label91");
            this.label91.Name = "label91";
            // 
            // label92
            // 
            resources.ApplyResources(this.label92, "label92");
            this.label92.Name = "label92";
            // 
            // Q_TAILSIT_ANGLE
            // 
            resources.ApplyResources(this.Q_TAILSIT_ANGLE, "Q_TAILSIT_ANGLE");
            this.Q_TAILSIT_ANGLE.Max = 1F;
            this.Q_TAILSIT_ANGLE.Min = 0F;
            this.Q_TAILSIT_ANGLE.Name = "Q_TAILSIT_ANGLE";
            this.Q_TAILSIT_ANGLE.ParamName = null;
            this.Q_TAILSIT_ANGLE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label93
            // 
            resources.ApplyResources(this.label93, "label93");
            this.label93.Name = "label93";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BUT_writePIDS);
            this.groupBox1.Controls.Add(this.BUT_refreshpart);
            this.groupBox1.Controls.Add(this.BUT_rerequestparams);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ConfigArduplane
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox23);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.groupBox21);
            this.Controls.Add(this.groupBox22);
            this.Controls.Add(this.groupBox19);
            this.Controls.Add(this.groupBox18);
            this.Controls.Add(this.groupBox17);
            this.Name = "ConfigArduplane";
            resources.ApplyResources(this, "$this");
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_RLL_P)).EndInit();
            this.groupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_PIT_P)).EndInit();
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_ANG_YAW_P)).EndInit();
            this.groupBox17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_FILT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_FF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_RLL_P)).EndInit();
            this.groupBox18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_FILT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_FF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_YAW_P)).EndInit();
            this.groupBox19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_FILT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_FF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_A_RAT_PIT_P)).EndInit();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_ASSIST_SPEED)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_ASSIST_ANGLE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TRANSITION_MS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARSPD_FBW_MIN_1)).EndInit();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_RTL_ALT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_LAND_FINAL_ALT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED_UP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_LAND_SPEED)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_RADIUS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED_DN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_WP_SPEED)).EndInit();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_RATE_UP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_YAW_ANGLE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_MAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Q_TILT_RATE_DN)).EndInit();
            this.groupBox23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Q_TAILSIT_ANGLE)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BUT_writePIDS;
        private System.Windows.Forms.Button BUT_rerequestparams;
        private System.Windows.Forms.Button BUT_refreshpart;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private Controls.MavlinkCheckBox Q_ENABLE;
        private Controls.MavlinkComboBox Q_FRAME_TYPE;
        private Controls.MavlinkComboBox Q_FRAME_CLASS;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label19;
        private Controls.MavlinkNumericUpDown Q_A_ANG_RLL_P;
        private System.Windows.Forms.GroupBox groupBox11;
        private Controls.MavlinkNumericUpDown Q_A_ANG_PIT_P;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox15;
        private Controls.MavlinkNumericUpDown Q_A_ANG_YAW_P;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox17;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_D;
        private System.Windows.Forms.Label label22;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_FF;
        private System.Windows.Forms.Label label23;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_IMAX;
        private System.Windows.Forms.Label label24;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_I;
        private System.Windows.Forms.Label label25;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_P;
        private System.Windows.Forms.Label label26;
        private Controls.MavlinkNumericUpDown Q_A_RAT_RLL_FILT;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.GroupBox groupBox18;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_D;
        private System.Windows.Forms.Label label28;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_FILT;
        private System.Windows.Forms.Label label29;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_FF;
        private System.Windows.Forms.Label label30;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_IMAX;
        private System.Windows.Forms.Label label31;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_I;
        private System.Windows.Forms.Label label32;
        private Controls.MavlinkNumericUpDown Q_A_RAT_YAW_P;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.GroupBox groupBox19;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_D;
        private System.Windows.Forms.Label label34;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_FILT;
        private System.Windows.Forms.Label label35;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_FF;
        private System.Windows.Forms.Label label36;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_IMAX;
        private System.Windows.Forms.Label label40;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_I;
        private System.Windows.Forms.Label label41;
        private Controls.MavlinkNumericUpDown Q_A_RAT_PIT_P;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox20;
        private Controls.MavlinkNumericUpDown Q_ASSIST_SPEED;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label45;
        private Controls.MavlinkNumericUpDown Q_ASSIST_ANGLE;
        private System.Windows.Forms.Label label46;
        private Controls.MavlinkNumericUpDown Q_TRANSITION_MS;
        private System.Windows.Forms.Label label47;
        private Controls.MavlinkNumericUpDown ARSPD_FBW_MIN_1;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.GroupBox groupBox21;
        private Controls.MavlinkNumericUpDown Q_RTL_ALT;
        private System.Windows.Forms.Label label44;
        private Controls.MavlinkNumericUpDown Q_WP_SPEED_UP;
        private System.Windows.Forms.Label label61;
        private Controls.MavlinkNumericUpDown Q_WP_RADIUS;
        private System.Windows.Forms.Label label62;
        private Controls.MavlinkNumericUpDown Q_WP_SPEED;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label77;
        private Controls.MavlinkNumericUpDown Q_LAND_FINAL_ALT;
        private System.Windows.Forms.Label label81;
        private Controls.MavlinkNumericUpDown Q_LAND_SPEED;
        private System.Windows.Forms.Label label80;
        private Controls.MavlinkNumericUpDown Q_WP_SPEED_DN;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.GroupBox groupBox22;
        private Controls.MavlinkNumericUpDown Q_TILT_RATE_UP;
        private System.Windows.Forms.Label label82;
        private Controls.MavlinkNumericUpDown Q_TILT_YAW_ANGLE;
        private System.Windows.Forms.Label label84;
        private Controls.MavlinkNumericUpDown Q_TILT_MAX;
        private System.Windows.Forms.Label label85;
        private Controls.MavlinkNumericUpDown Q_TILT_RATE_DN;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.Label label92;
        private Controls.MavlinkNumericUpDown Q_TAILSIT_ANGLE;
        private System.Windows.Forms.Label label93;
        private Controls.MavlinkComboBox Q_TAILSIT_MASKCH;
        private Controls.MavlinkComboBox Q_TAILSIT_INPUT;
        private Controls.MavlinkCheckBox Q_RTL_MODE;
        private Controls.MavlinkCheckBox Q_GUIDED_MODE;
        private Controls.MavlinkComboBox Q_TILT_TYPE;
        private Controls.MavlinkCheckBox Q_TILT_MASK_1;
        private Controls.MavlinkCheckBox Q_M_HOVER_LEARN;
        private Controls.MavlinkCheckBox Q_TILT_MASK_4;
        private Controls.MavlinkCheckBox Q_TILT_MASK_6;
        private Controls.MavlinkCheckBox Q_TILT_MASK_8;
        private Controls.MavlinkCheckBox Q_TILT_MASK_3;
        private Controls.MavlinkCheckBox Q_TILT_MASK_5;
        private Controls.MavlinkCheckBox Q_TILT_MASK_7;
        private Controls.MavlinkCheckBox Q_TILT_MASK_2;
        private Controls.MavlinkCheckBoxBitMask Q_TAILSIT_MASK;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
