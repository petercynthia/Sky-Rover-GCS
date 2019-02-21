using System;
using System.Drawing;
using System.Windows.Forms;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using SharpDX.DirectInput;
using SKYROVER.GCS.DeskTop.Utilities;

namespace SKYROVER.GCS.DeskTop.Joystick
{
    public partial class JoystickSetup : Form
    {
        bool startup = true;

        int noButtons = 0;

        public JoystickSetup()
        {
            InitializeComponent();

            Tracking.AddPage(this.GetType().ToString(), this.Text);
        }

        private void Joystick_Load(object sender, EventArgs e)
        {
            try
            {
                var joysticklist = Joystick.getDevices();

                foreach (DeviceInstance device in joysticklist)
                {
                    CMB_joysticks.Items.Add(device.ProductName.TrimUnPrintable());
                }
            }
            catch
            {
                CustomMessageBox.Show("Error geting joystick list: do you have the directx redist installed?");
                this.Close();
                return;
            }

            if (CMB_joysticks.Items.Count > 0 && CMB_joysticks.SelectedIndex == -1)
                CMB_joysticks.SelectedIndex = 0;

            try
            {
                if (Settings.Instance.ContainsKey("joystick_name") && Settings.Instance["joystick_name"].ToString() != "")
                    CMB_joysticks.Text = Settings.Instance["joystick_name"].ToString();
            }
            catch
            {
            }

            CMB_CH1.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH2.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH3.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH4.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH5.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH6.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH7.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));
            CMB_CH8.DataSource = (Enum.GetValues(typeof (Joystick.joystickaxis)));

            try
            {
                if(Settings.Instance.ContainsKey("joy_elevons"))
                    CHK_elevons.Checked = bool.Parse(Settings.Instance["joy_elevons"].ToString());
            }
            catch
            {
            } // IF 1 DOESNT EXIST NONE WILL

            var tempjoystick = new Joystick();

            label14.Text += " " + MainUI.comPort.MAV.cs.firmware.ToString();

            for (int a = 1; a <= 8; a++)
            {
                var config = tempjoystick.getChannel(a);

                findandsetcontrol("CMB_CH" + a, config.axis.ToString());
                findandsetcontrol("revCH" + a, config.reverse.ToString());
                findandsetcontrol("expo_ch" + a, config.expo.ToString());
            }

            if (MainUI.joystick != null && MainUI.joystick.enabled)
            {
                timer1.Start();
                BUT_enable.Text = "Disable";
            }

            startup = false;
        }

        void findandsetcontrol(string ctlname, string value)
        {
            var ctl = this.Controls.Find(ctlname, false)[0];

            if (ctl is CheckBox)
            {
                ((CheckBox) ctl).Checked = (value.ToLower() == "false") ? false : true;
            }
            else
            {
                ((Control) ctl).Text = value;
            }
        }

        int[] getButtonNumbers()
        {
            int[] temp = new int[128];
            temp[0] = -1;
            for (int a = 0; a < temp.Length - 1; a++)
            {
                temp[a + 1] = a;
            }
            return temp;
        }

        private void BUT_enable_Click(object sender, EventArgs e)
        {
            if (MainUI.joystick == null || MainUI.joystick.enabled == false)
            {
                try
                {
                    if (MainUI.joystick != null)
                        MainUI.joystick.UnAcquireJoyStick();
                }
                catch
                {
                }

                // all config is loaded from the xmls
                Joystick joy = new Joystick();

                joy.elevons = CHK_elevons.Checked;

                //show error message if a joystick is not connected when Enable is clicked
                if (!joy.start(CMB_joysticks.Text))
                {
                    CustomMessageBox.Show("Please Connect a Joystick", "No Joystick");
                    joy.Dispose();
                    return;
                }

                Settings.Instance["joystick_name"] = CMB_joysticks.Text;

                MainUI.joystick = joy;
                MainUI.joystick.enabled = true;

                BUT_enable.Text = "Disable";

                //timer1.Start();
            }
            else
            {
                MainUI.joystick.enabled = false;

                MainUI.joystick.clearRCOverride();

                MainUI.joystick = null;


                //timer1.Stop();

                BUT_enable.Text = "Enable";
            }
        }

        private void BUT_save_Click(object sender, EventArgs e)
        {
            if (MainUI.joystick == null)
            {
                CustomMessageBox.Show("Please select a joystick");
                return;
            }
            MainUI.joystick.saveconfig();

            Settings.Instance["joy_elevons"] = CHK_elevons.Checked.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (MainUI.joystick == null || MainUI.joystick.enabled == false)
                {
                    //Console.WriteLine(DateTime.Now.Millisecond + " start ");
                    Joystick joy = MainUI.joystick;
                    if (joy == null)
                    {
                        joy = new Joystick();
                        if (CMB_CH1.Text != "")
                            joy.setChannel(1,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH1.Text),
                                revCH1.Checked, int.Parse(expo_ch1.Text));
                        if (CMB_CH2.Text != "")
                            joy.setChannel(2,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH2.Text),
                                revCH2.Checked, int.Parse(expo_ch2.Text));
                        if (CMB_CH3.Text != "")
                            joy.setChannel(3,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH3.Text),
                                revCH3.Checked, int.Parse(expo_ch3.Text));
                        if (CMB_CH4.Text != "")
                            joy.setChannel(4,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH4.Text),
                                revCH4.Checked, int.Parse(expo_ch4.Text));
                        if (CMB_CH5.Text != "")
                            joy.setChannel(5,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH5.Text),
                                revCH5.Checked, int.Parse(expo_ch5.Text));
                        if (CMB_CH6.Text != "")
                            joy.setChannel(6,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH6.Text),
                                revCH6.Checked, int.Parse(expo_ch6.Text));
                        if (CMB_CH7.Text != "")
                            joy.setChannel(7,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH7.Text),
                                revCH7.Checked, int.Parse(expo_ch7.Text));
                        if (CMB_CH8.Text != "")
                            joy.setChannel(8,
                                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), CMB_CH8.Text),
                                revCH8.Checked, int.Parse(expo_ch8.Text));

                        joy.elevons = CHK_elevons.Checked;

                        joy.AcquireJoystick(CMB_joysticks.Text);

                        joy.name = CMB_joysticks.Text;

                        noButtons = joy.getNumButtons();

                        noButtons = Math.Min(15, noButtons);

                        SuspendLayout();

                        MainUI.joystick = joy;

                        for (int f = 0; f < noButtons; f++)
                        {
                            string name = (f).ToString();

                            doButtontoUI(name, 10, CMB_CH8.Bottom + 20 + f*25);

                            var config = joy.getButton(f);

                            joy.setButton(f, config);
                        }

                        ResumeLayout();

                       // ThemeManager.ApplyThemeTo(this);

                        CMB_joysticks.SelectedIndex = CMB_joysticks.Items.IndexOf(joy.name);
                    }

                    MainUI.joystick.elevons = CHK_elevons.Checked;

                    MainUI.comPort.MAV.cs.rcoverridech1 = joy.getValueForChannel(1, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech2 = joy.getValueForChannel(2, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech3 = joy.getValueForChannel(3, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech4 = joy.getValueForChannel(4, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech5 = joy.getValueForChannel(5, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech6 = joy.getValueForChannel(6, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech7 = joy.getValueForChannel(7, CMB_joysticks.Text);
                    MainUI.comPort.MAV.cs.rcoverridech8 = joy.getValueForChannel(8, CMB_joysticks.Text);

                    //Console.WriteLine(DateTime.Now.Millisecond + " end ");
                }
            }
            catch (SharpDX.SharpDXException ex)
            {
                ex.ToString();
                if (MainUI.joystick != null && MainUI.joystick.enabled == true)
                {
                    BUT_enable_Click(null, null);
                }

                if (ex.Message.Contains("DIERR_NOTACQUIRED"))
                    MainUI.joystick = null;
            }
            catch
            {
            }

            progressBarRoll.Value = MainUI.comPort.MAV.cs.rcoverridech1;
            progressBarPith.Value = MainUI.comPort.MAV.cs.rcoverridech2;
            progressBarThrottle.Value = MainUI.comPort.MAV.cs.rcoverridech3;
            progressBarRudder.Value = MainUI.comPort.MAV.cs.rcoverridech4;
            ProgressBarCH5.Value = MainUI.comPort.MAV.cs.rcoverridech5;
            ProgressBarCH6.Value = MainUI.comPort.MAV.cs.rcoverridech6;
            ProgressBarCH7.Value = MainUI.comPort.MAV.cs.rcoverridech7;
            ProgressBarCH8.Value = MainUI.comPort.MAV.cs.rcoverridech8;

            try
            {
                if (MainUI.joystick != null)
                {
                    progressBarRoll.maxline = MainUI.joystick.getRawValueForChannel(1);
                    progressBarPith.maxline = MainUI.joystick.getRawValueForChannel(2);
                    progressBarThrottle.maxline = MainUI.joystick.getRawValueForChannel(3);
                    progressBarRudder.maxline = MainUI.joystick.getRawValueForChannel(4);
                    ProgressBarCH5.maxline = MainUI.joystick.getRawValueForChannel(5);
                    ProgressBarCH6.maxline = MainUI.joystick.getRawValueForChannel(6);
                    ProgressBarCH7.maxline = MainUI.joystick.getRawValueForChannel(7);
                    ProgressBarCH8.maxline = MainUI.joystick.getRawValueForChannel(8);
                }
            }
            catch
            {
                //Exception Error in the application. -2147024866 (DIERR_INPUTLOST)
            }

            try
            {
                for (int f = 0; f < noButtons; f++)
                {
                    string name = (f).ToString();

                    var items = this.Controls.Find("hbar" + name, false);

                    if(items.Length > 0)
                    ((HorizontalProgressBar)items[0]).Value =
                        MainUI.joystick.isButtonPressed(f) ? 100 : 0;
                }
            }
            catch
            {
            } // this is for buttons - silent fail
        }

        private void CMB_joysticks_Click(object sender, EventArgs e)
        {
            CMB_joysticks.Items.Clear();

            var joysticklist = Joystick.getDevices();

            foreach (DeviceInstance device in joysticklist)
            {
                CMB_joysticks.Items.Add(device.ProductName.TrimUnPrintable());
            }

            if (CMB_joysticks.Items.Count > 0 && CMB_joysticks.SelectedIndex == -1)
                CMB_joysticks.SelectedIndex = 0;
        }

        private void revCH1_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(1, ((CheckBox) sender).Checked);
        }

        private void revCH2_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(2, ((CheckBox) sender).Checked);
        }

        private void revCH3_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(3, ((CheckBox) sender).Checked);
        }

        private void revCH4_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(4, ((CheckBox) sender).Checked);
        }

        private void BUT_detch1_Click(object sender, EventArgs e)
        {
            CMB_CH1.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch2_Click(object sender, EventArgs e)
        {
            CMB_CH2.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch3_Click(object sender, EventArgs e)
        {
            CMB_CH3.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch4_Click(object sender, EventArgs e)
        {
            CMB_CH4.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void CMB_CH1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(1,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(2,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(3,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(4,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void cmbbutton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup)
                return;

            string name = ((ComboBox) sender).Name.Replace("cmbbutton", "");

            MainUI.joystick.changeButton((int.Parse(name)), int.Parse(((ComboBox) sender).Text));
        }

        private void BUT_detbutton_Click(object sender, EventArgs e)
        {
            string name = ((MyButton) sender).Name.Replace("mybut", "");

            ComboBox cmb = (ComboBox) (this.Controls.Find("cmbbutton" + name, false)[0]);
            cmb.Text = Joystick.getPressedButton(CMB_joysticks.Text).ToString();
        }

        void doButtontoUI(string name, int x, int y)
        {
            MyLabel butlabel = new MyLabel();
            ComboBox butnumberlist = new ComboBox();
            MyButton but_detect = new MyButton();
            HorizontalProgressBar hbar = new HorizontalProgressBar();
            ComboBox cmbaction = new ComboBox();
            MyButton but_settings = new MyButton();

            if (MainUI.joystick == null)
            {
                butlabel.Dispose();
                butnumberlist.Dispose();
                but_detect.Dispose();
                hbar.Dispose();
                cmbaction.Dispose();
                but_settings.Dispose();
                return;
            }

            var config = MainUI.joystick.getButton(int.Parse(name));

            // do this here so putting in text works
            this.Controls.AddRange(new Control[] {butlabel, butnumberlist, but_detect, hbar, cmbaction, but_settings});

            butlabel.Location = new Point(x, y);
            butlabel.Size = new Size(47, 13);
            butlabel.Text = "Button " + (int.Parse(name) + 1);

            butnumberlist.Location = new Point(72, y);
            butnumberlist.Size = new Size(70, 21);
            butnumberlist.DataSource = getButtonNumbers();
            butnumberlist.DropDownStyle = ComboBoxStyle.DropDownList;
            butnumberlist.Name = "cmbbutton" + name;
            //if (Settings.Instance["butno" + name] != null)
            //  butnumberlist.Text = (Settings.Instance["butno" + name].ToString());
            //if (config.buttonno != -1)
            butnumberlist.Text = config.buttonno.ToString();
            butnumberlist.SelectedIndexChanged += new EventHandler(cmbbutton_SelectedIndexChanged);

            but_detect.Location = new Point(BUT_detch1.Left, y);
            but_detect.Size = BUT_detch1.Size;
            but_detect.Text = BUT_detch1.Text;
            but_detect.Name = "mybut" + name;
            but_detect.Click += new EventHandler(BUT_detbutton_Click);

            hbar.Location = new Point(progressBarRoll.Left, y);
            hbar.Size = progressBarRoll.Size;
            hbar.Name = "hbar" + name;

            cmbaction.Location = new Point(hbar.Right + 5, y);
            cmbaction.Size = new Size(100, 21);

            cmbaction.DataSource = Enum.GetNames(typeof (Joystick.buttonfunction));
                //Common.getModesList(MainUI.comPort.MAV.cs);
            //cmbaction.ValueMember = "Key";
            //cmbaction.DisplayMember = "Value";
            cmbaction.Tag = name;
            cmbaction.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbaction.Name = "cmbaction" + name;
            //if (Settings.Instance["butaction" + name] != null)
            //  cmbaction.Text = Settings.Instance["butaction" + name].ToString();
            //if (config.function != Joystick.buttonfunction.ChangeMode)
            cmbaction.Text = config.function.ToString();
            cmbaction.SelectedIndexChanged += cmbaction_SelectedIndexChanged;

            but_settings.Location = new Point(cmbaction.Right + 5, y);
            but_settings.Size = BUT_detch1.Size;
            but_settings.Text = "Settings";
            but_settings.Name = "butsettings" + name;
            but_settings.Click += but_settings_Click;
            but_settings.Tag = cmbaction;

            if ((but_settings.Bottom + 30) > this.Height)
                this.Height += 25;
        }

        void cmbaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num = int.Parse(((Control) sender).Tag.ToString());
            var config = MainUI.joystick.getButton(num);
            config.function =
                (Joystick.buttonfunction) Enum.Parse(typeof (Joystick.buttonfunction), ((Control) sender).Text);
            MainUI.joystick.setButton(num, config);
        }

        void but_settings_Click(object sender, EventArgs e)
        {
            var cmb = ((Control) sender).Tag as ComboBox;

            switch ((Joystick.buttonfunction) Enum.Parse(typeof (Joystick.buttonfunction), cmb.SelectedItem.ToString()))
            {
                case Joystick.buttonfunction.ChangeMode:
                    new Joy_ChangeMode((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Mount_Mode:
                    new Joy_Mount_Mode((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Do_Repeat_Relay:
                    new Joy_Do_Repeat_Relay((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Do_Repeat_Servo:
                    new Joy_Do_Repeat_Servo((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Do_Set_Relay:
                    new Joy_Do_Set_Relay((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Do_Set_Servo:
                    new Joy_Do_Set_Servo((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Button_axis0:
                    new Joy_Button_axis((string) cmb.Tag).ShowDialog();
                    break;
                case Joystick.buttonfunction.Button_axis1:
                    new Joy_Button_axis((string) cmb.Tag).ShowDialog();
                    break;
                default:
                    CustomMessageBox.Show("No settings to set", "No settings");
                    break;
            }
        }

        private void CMB_joysticks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (MainUI.joystick != null && MainUI.joystick.enabled == false)
                    MainUI.joystick.UnAcquireJoyStick();
            }
            catch
            {
            }
        }

        private void JoystickSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();

            if (MainUI.joystick != null && MainUI.joystick.enabled == false)
            {
                MainUI.joystick.UnAcquireJoyStick();
                MainUI.joystick = null;
            }
        }

        private void CHK_elevons_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick == null)
            {
                return;
            }
            MainUI.joystick.elevons = CHK_elevons.Checked;
        }

        private void CMB_CH5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(5,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(6,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(7,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void CMB_CH8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (startup || MainUI.joystick == null)
                return;
            MainUI.joystick.setAxis(8,
                (Joystick.joystickaxis) Enum.Parse(typeof (Joystick.joystickaxis), ((ComboBox) sender).Text));
        }

        private void BUT_detch5_Click(object sender, EventArgs e)
        {
            CMB_CH5.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch6_Click(object sender, EventArgs e)
        {
            CMB_CH6.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch7_Click(object sender, EventArgs e)
        {
            CMB_CH7.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void BUT_detch8_Click(object sender, EventArgs e)
        {
            CMB_CH8.Text = Joystick.getMovingAxis(CMB_joysticks.Text, 16000).ToString();
        }

        private void revCH5_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(5, ((CheckBox) sender).Checked);
        }

        private void revCH6_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(6, ((CheckBox) sender).Checked);
        }

        private void revCH7_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(7, ((CheckBox) sender).Checked);
        }

        private void revCH8_CheckedChanged(object sender, EventArgs e)
        {
            if (MainUI.joystick != null)
                MainUI.joystick.setReverse(8, ((CheckBox) sender).Checked);
        }

        private void chk_manualcontrol_CheckedChanged(object sender, EventArgs e)
        {
            MainUI.joystick.manual_control = chk_manualcontrol.Checked;

            if (chk_manualcontrol.Checked)
            {
                CMB_CH5.Enabled = false;
                CMB_CH6.Enabled = false;
                CMB_CH7.Enabled = false;
                CMB_CH8.Enabled = false;
            }
            else
            {
                CMB_CH5.Enabled = true;
                CMB_CH6.Enabled = true;
                CMB_CH7.Enabled = true;
                CMB_CH8.Enabled = true;
            }
        }
    }
}