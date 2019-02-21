namespace SKYROVER.GCS.DeskTop.Payloads
{
    partial class NacellePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NacellePanel));
            this.container = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.COMboBox = new MetroSet_UI.Controls.MetroSetComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.baud = new MetroSet_UI.Controls.MetroSetTextBox();
            this.btnConnect = new MetroSet_UI.Controls.MetroSetButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.KBYawRangeValue = new MetroSet_UI.Controls.MetroSetTextBox();
            this.KBPitchRangeValue = new MetroSet_UI.Controls.MetroSetTextBox();
            this.JoyYawRangeValue = new MetroSet_UI.Controls.MetroSetTextBox();
            this.JoyPitchRangeValue = new MetroSet_UI.Controls.MetroSetTextBox();
            this.FocalLengthRangeValue = new MetroSet_UI.Controls.MetroSetTextBox();
            this.KBYawRange = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.KBPitchRange = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.JoyYawRange = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.JoyPitchRange = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.FocalLengthRange = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.receiveData = new System.Windows.Forms.RichTextBox();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            resources.ApplyResources(this.container, "container");
            this.container.Controls.Add(this.label1, 0, 0);
            this.container.Controls.Add(this.COMboBox, 1, 0);
            this.container.Controls.Add(this.label2, 2, 0);
            this.container.Controls.Add(this.baud, 3, 0);
            this.container.Controls.Add(this.btnConnect, 4, 0);
            this.container.Controls.Add(this.label3, 0, 1);
            this.container.Controls.Add(this.label4, 0, 2);
            this.container.Controls.Add(this.label5, 0, 3);
            this.container.Controls.Add(this.label6, 0, 4);
            this.container.Controls.Add(this.label7, 0, 5);
            this.container.Controls.Add(this.KBYawRangeValue, 4, 1);
            this.container.Controls.Add(this.KBPitchRangeValue, 4, 2);
            this.container.Controls.Add(this.JoyYawRangeValue, 4, 3);
            this.container.Controls.Add(this.JoyPitchRangeValue, 4, 4);
            this.container.Controls.Add(this.FocalLengthRangeValue, 4, 5);
            this.container.Controls.Add(this.KBYawRange, 1, 1);
            this.container.Controls.Add(this.KBPitchRange, 1, 2);
            this.container.Controls.Add(this.JoyYawRange, 1, 3);
            this.container.Controls.Add(this.JoyPitchRange, 1, 4);
            this.container.Controls.Add(this.FocalLengthRange, 1, 5);
            this.container.Controls.Add(this.receiveData, 0, 6);
            this.container.Name = "container";
            this.container.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.container.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.container.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Name = "label1";
            // 
            // COMboBox
            // 
            this.COMboBox.AllowDrop = true;
            resources.ApplyResources(this.COMboBox, "COMboBox");
            this.COMboBox.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.COMboBox.BackColor = System.Drawing.Color.Transparent;
            this.COMboBox.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.COMboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.COMboBox.CausesValidation = false;
            this.COMboBox.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.COMboBox.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.COMboBox.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.COMboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.COMboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.COMboBox.FormattingEnabled = true;
            this.COMboBox.Name = "COMboBox";
            this.COMboBox.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.COMboBox.SelectedItemForeColor = System.Drawing.Color.White;
            this.COMboBox.Style = MetroSet_UI.Design.Style.Light;
            this.COMboBox.StyleManager = null;
            this.COMboBox.ThemeAuthor = "Narwin";
            this.COMboBox.ThemeName = "MetroLite";
            this.COMboBox.Click += new System.EventHandler(this.COMboBox_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Name = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // baud
            // 
            resources.ApplyResources(this.baud, "baud");
            this.baud.AutoCompleteCustomSource = null;
            this.baud.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.baud.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.baud.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.baud.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.baud.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.baud.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.baud.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.baud.Image = null;
            this.baud.Lines = null;
            this.baud.MaxLength = 32767;
            this.baud.Multiline = false;
            this.baud.Name = "baud";
            this.baud.ReadOnly = true;
            this.baud.Style = MetroSet_UI.Design.Style.Light;
            this.baud.StyleManager = null;
            this.baud.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.baud.ThemeAuthor = "Narwin";
            this.baud.ThemeName = "MetroLite";
            this.baud.UseSystemPasswordChar = false;
            this.baud.WatermarkText = "";
            // 
            // btnConnect
            // 
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.btnConnect.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.btnConnect.DisabledForeColor = System.Drawing.Color.Gray;
            this.btnConnect.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(207)))), ((int)(((byte)(255)))));
            this.btnConnect.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(207)))), ((int)(((byte)(255)))));
            this.btnConnect.HoverTextColor = System.Drawing.Color.White;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.btnConnect.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.btnConnect.NormalTextColor = System.Drawing.Color.White;
            this.btnConnect.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(147)))), ((int)(((byte)(195)))));
            this.btnConnect.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(147)))), ((int)(((byte)(195)))));
            this.btnConnect.PressTextColor = System.Drawing.Color.White;
            this.btnConnect.Style = MetroSet_UI.Design.Style.Light;
            this.btnConnect.StyleManager = null;
            this.btnConnect.ThemeAuthor = "Narwin";
            this.btnConnect.ThemeName = "MetroLite";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Name = "label7";
            // 
            // KBYawRangeValue
            // 
            resources.ApplyResources(this.KBYawRangeValue, "KBYawRangeValue");
            this.KBYawRangeValue.AutoCompleteCustomSource = null;
            this.KBYawRangeValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.KBYawRangeValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.KBYawRangeValue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.KBYawRangeValue.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.KBYawRangeValue.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.KBYawRangeValue.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.KBYawRangeValue.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.KBYawRangeValue.Image = null;
            this.KBYawRangeValue.Lines = null;
            this.KBYawRangeValue.MaxLength = 32767;
            this.KBYawRangeValue.Multiline = false;
            this.KBYawRangeValue.Name = "KBYawRangeValue";
            this.KBYawRangeValue.ReadOnly = true;
            this.KBYawRangeValue.Style = MetroSet_UI.Design.Style.Light;
            this.KBYawRangeValue.StyleManager = null;
            this.KBYawRangeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.KBYawRangeValue.ThemeAuthor = "Narwin";
            this.KBYawRangeValue.ThemeName = "MetroLite";
            this.KBYawRangeValue.UseSystemPasswordChar = false;
            this.KBYawRangeValue.WatermarkText = "";
            // 
            // KBPitchRangeValue
            // 
            resources.ApplyResources(this.KBPitchRangeValue, "KBPitchRangeValue");
            this.KBPitchRangeValue.AutoCompleteCustomSource = null;
            this.KBPitchRangeValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.KBPitchRangeValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.KBPitchRangeValue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.KBPitchRangeValue.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.KBPitchRangeValue.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.KBPitchRangeValue.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.KBPitchRangeValue.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.KBPitchRangeValue.Image = null;
            this.KBPitchRangeValue.Lines = null;
            this.KBPitchRangeValue.MaxLength = 32767;
            this.KBPitchRangeValue.Multiline = false;
            this.KBPitchRangeValue.Name = "KBPitchRangeValue";
            this.KBPitchRangeValue.ReadOnly = true;
            this.KBPitchRangeValue.Style = MetroSet_UI.Design.Style.Light;
            this.KBPitchRangeValue.StyleManager = null;
            this.KBPitchRangeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.KBPitchRangeValue.ThemeAuthor = "Narwin";
            this.KBPitchRangeValue.ThemeName = "MetroLite";
            this.KBPitchRangeValue.UseSystemPasswordChar = false;
            this.KBPitchRangeValue.WatermarkText = "";
            // 
            // JoyYawRangeValue
            // 
            resources.ApplyResources(this.JoyYawRangeValue, "JoyYawRangeValue");
            this.JoyYawRangeValue.AutoCompleteCustomSource = null;
            this.JoyYawRangeValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.JoyYawRangeValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.JoyYawRangeValue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.JoyYawRangeValue.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.JoyYawRangeValue.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.JoyYawRangeValue.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.JoyYawRangeValue.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.JoyYawRangeValue.Image = null;
            this.JoyYawRangeValue.Lines = null;
            this.JoyYawRangeValue.MaxLength = 32767;
            this.JoyYawRangeValue.Multiline = false;
            this.JoyYawRangeValue.Name = "JoyYawRangeValue";
            this.JoyYawRangeValue.ReadOnly = true;
            this.JoyYawRangeValue.Style = MetroSet_UI.Design.Style.Light;
            this.JoyYawRangeValue.StyleManager = null;
            this.JoyYawRangeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JoyYawRangeValue.ThemeAuthor = "Narwin";
            this.JoyYawRangeValue.ThemeName = "MetroLite";
            this.JoyYawRangeValue.UseSystemPasswordChar = false;
            this.JoyYawRangeValue.WatermarkText = "";
            // 
            // JoyPitchRangeValue
            // 
            resources.ApplyResources(this.JoyPitchRangeValue, "JoyPitchRangeValue");
            this.JoyPitchRangeValue.AutoCompleteCustomSource = null;
            this.JoyPitchRangeValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.JoyPitchRangeValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.JoyPitchRangeValue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.JoyPitchRangeValue.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.JoyPitchRangeValue.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.JoyPitchRangeValue.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.JoyPitchRangeValue.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.JoyPitchRangeValue.Image = null;
            this.JoyPitchRangeValue.Lines = null;
            this.JoyPitchRangeValue.MaxLength = 32767;
            this.JoyPitchRangeValue.Multiline = false;
            this.JoyPitchRangeValue.Name = "JoyPitchRangeValue";
            this.JoyPitchRangeValue.ReadOnly = true;
            this.JoyPitchRangeValue.Style = MetroSet_UI.Design.Style.Light;
            this.JoyPitchRangeValue.StyleManager = null;
            this.JoyPitchRangeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JoyPitchRangeValue.ThemeAuthor = "Narwin";
            this.JoyPitchRangeValue.ThemeName = "MetroLite";
            this.JoyPitchRangeValue.UseSystemPasswordChar = false;
            this.JoyPitchRangeValue.WatermarkText = "";
            // 
            // FocalLengthRangeValue
            // 
            resources.ApplyResources(this.FocalLengthRangeValue, "FocalLengthRangeValue");
            this.FocalLengthRangeValue.AutoCompleteCustomSource = null;
            this.FocalLengthRangeValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.FocalLengthRangeValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.FocalLengthRangeValue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.FocalLengthRangeValue.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.FocalLengthRangeValue.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.FocalLengthRangeValue.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.FocalLengthRangeValue.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.FocalLengthRangeValue.Image = null;
            this.FocalLengthRangeValue.Lines = null;
            this.FocalLengthRangeValue.MaxLength = 32767;
            this.FocalLengthRangeValue.Multiline = false;
            this.FocalLengthRangeValue.Name = "FocalLengthRangeValue";
            this.FocalLengthRangeValue.ReadOnly = true;
            this.FocalLengthRangeValue.Style = MetroSet_UI.Design.Style.Light;
            this.FocalLengthRangeValue.StyleManager = null;
            this.FocalLengthRangeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FocalLengthRangeValue.ThemeAuthor = "Narwin";
            this.FocalLengthRangeValue.ThemeName = "MetroLite";
            this.FocalLengthRangeValue.UseSystemPasswordChar = false;
            this.FocalLengthRangeValue.WatermarkText = "";
            // 
            // KBYawRange
            // 
            resources.ApplyResources(this.KBYawRange, "KBYawRange");
            this.KBYawRange.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.container.SetColumnSpan(this.KBYawRange, 3);
            this.KBYawRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KBYawRange.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.KBYawRange.DisabledBorderColor = System.Drawing.Color.Empty;
            this.KBYawRange.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.KBYawRange.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.KBYawRange.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.KBYawRange.Maximum = 32;
            this.KBYawRange.Minimum = 0;
            this.KBYawRange.Name = "KBYawRange";
            this.KBYawRange.Style = MetroSet_UI.Design.Style.Light;
            this.KBYawRange.StyleManager = null;
            this.KBYawRange.ThemeAuthor = "Narwin";
            this.KBYawRange.ThemeName = "MetroLite";
            this.KBYawRange.Value = 19;
            this.KBYawRange.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.KBYawRange.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.KBYawRange_Scroll);
            // 
            // KBPitchRange
            // 
            resources.ApplyResources(this.KBPitchRange, "KBPitchRange");
            this.KBPitchRange.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.container.SetColumnSpan(this.KBPitchRange, 3);
            this.KBPitchRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KBPitchRange.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.KBPitchRange.DisabledBorderColor = System.Drawing.Color.Empty;
            this.KBPitchRange.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.KBPitchRange.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.KBPitchRange.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.KBPitchRange.Maximum = 32;
            this.KBPitchRange.Minimum = 0;
            this.KBPitchRange.Name = "KBPitchRange";
            this.KBPitchRange.Style = MetroSet_UI.Design.Style.Light;
            this.KBPitchRange.StyleManager = null;
            this.KBPitchRange.ThemeAuthor = "Narwin";
            this.KBPitchRange.ThemeName = "MetroLite";
            this.KBPitchRange.Value = 19;
            this.KBPitchRange.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.KBPitchRange.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.KBPitchRange_Scroll);
            // 
            // JoyYawRange
            // 
            resources.ApplyResources(this.JoyYawRange, "JoyYawRange");
            this.JoyYawRange.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.container.SetColumnSpan(this.JoyYawRange, 3);
            this.JoyYawRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JoyYawRange.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.JoyYawRange.DisabledBorderColor = System.Drawing.Color.Empty;
            this.JoyYawRange.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.JoyYawRange.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.JoyYawRange.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.JoyYawRange.Maximum = 32;
            this.JoyYawRange.Minimum = 0;
            this.JoyYawRange.Name = "JoyYawRange";
            this.JoyYawRange.Style = MetroSet_UI.Design.Style.Light;
            this.JoyYawRange.StyleManager = null;
            this.JoyYawRange.ThemeAuthor = "Narwin";
            this.JoyYawRange.ThemeName = "MetroLite";
            this.JoyYawRange.Value = 19;
            this.JoyYawRange.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.JoyYawRange.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.JoyYawRange_Scroll);
            // 
            // JoyPitchRange
            // 
            resources.ApplyResources(this.JoyPitchRange, "JoyPitchRange");
            this.JoyPitchRange.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.container.SetColumnSpan(this.JoyPitchRange, 3);
            this.JoyPitchRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JoyPitchRange.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.JoyPitchRange.DisabledBorderColor = System.Drawing.Color.Empty;
            this.JoyPitchRange.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.JoyPitchRange.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.JoyPitchRange.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.JoyPitchRange.Maximum = 32;
            this.JoyPitchRange.Minimum = 0;
            this.JoyPitchRange.Name = "JoyPitchRange";
            this.JoyPitchRange.Style = MetroSet_UI.Design.Style.Light;
            this.JoyPitchRange.StyleManager = null;
            this.JoyPitchRange.ThemeAuthor = "Narwin";
            this.JoyPitchRange.ThemeName = "MetroLite";
            this.JoyPitchRange.Value = 19;
            this.JoyPitchRange.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.JoyPitchRange.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.JoyPitchRange_Scroll);
            // 
            // FocalLengthRange
            // 
            resources.ApplyResources(this.FocalLengthRange, "FocalLengthRange");
            this.FocalLengthRange.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.container.SetColumnSpan(this.FocalLengthRange, 3);
            this.FocalLengthRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FocalLengthRange.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.FocalLengthRange.DisabledBorderColor = System.Drawing.Color.Empty;
            this.FocalLengthRange.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.FocalLengthRange.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.FocalLengthRange.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.FocalLengthRange.Maximum = 8;
            this.FocalLengthRange.Minimum = 1;
            this.FocalLengthRange.Name = "FocalLengthRange";
            this.FocalLengthRange.Style = MetroSet_UI.Design.Style.Light;
            this.FocalLengthRange.StyleManager = null;
            this.FocalLengthRange.ThemeAuthor = "Narwin";
            this.FocalLengthRange.ThemeName = "MetroLite";
            this.FocalLengthRange.Value = 4;
            this.FocalLengthRange.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.FocalLengthRange.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.FocalLengthRange_Scroll);
            // 
            // receiveData
            // 
            this.receiveData.BackColor = System.Drawing.SystemColors.InfoText;
            this.container.SetColumnSpan(this.receiveData, 5);
            resources.ApplyResources(this.receiveData, "receiveData");
            this.receiveData.ForeColor = System.Drawing.SystemColors.Window;
            this.receiveData.Name = "receiveData";
            this.receiveData.ReadOnly = true;
            // 
            // NacellePanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(6)))), ((int)(((byte)(6)))));
            this.Controls.Add(this.container);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NacellePanel";
            this.Opacity = 0.75D;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.NacellePanel_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NacellePanel_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NacellePanel_KeyUp);
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel container;
        private System.Windows.Forms.Label label1;
        private MetroSet_UI.Controls.MetroSetComboBox COMboBox;
        private MetroSet_UI.Controls.MetroSetButton btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private MetroSet_UI.Controls.MetroSetTextBox KBYawRangeValue;
        private MetroSet_UI.Controls.MetroSetTextBox KBPitchRangeValue;
        private MetroSet_UI.Controls.MetroSetTextBox JoyYawRangeValue;
        private MetroSet_UI.Controls.MetroSetTextBox JoyPitchRangeValue;
        private MetroSet_UI.Controls.MetroSetTextBox FocalLengthRangeValue;
        private MetroSet_UI.Controls.MetroSetTrackBar KBYawRange;
        private MetroSet_UI.Controls.MetroSetTrackBar KBPitchRange;
        private MetroSet_UI.Controls.MetroSetTrackBar JoyYawRange;
        private MetroSet_UI.Controls.MetroSetTrackBar JoyPitchRange;
        private MetroSet_UI.Controls.MetroSetTrackBar FocalLengthRange;
        private System.Windows.Forms.Label label2;
        private MetroSet_UI.Controls.MetroSetTextBox baud;
        private System.Windows.Forms.RichTextBox receiveData;
    }
}