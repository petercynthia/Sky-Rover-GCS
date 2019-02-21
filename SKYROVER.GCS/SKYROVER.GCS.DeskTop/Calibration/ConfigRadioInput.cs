using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MissionPlanner.ArduPilot;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Utilities;
using Timer = System.Windows.Forms.Timer;

namespace SKYROVER.GCS.DeskTop.Calibration
{
    public partial class ConfigRadioInput : UserControl, IActivate, IDeactivate
    {
        private readonly float[] rcmax = new float[16];
        private readonly float[] rcmin = new float[16];
        private readonly float[] rctrim = new float[16];
        private readonly Timer timer = new Timer();
        private int chpitch = -1;
        private int chroll = -1;
        private int chthro = -1;
        private int chyaw = -1;
        private bool run;
        private bool startup;

        public ConfigRadioInput()
        {
            InitializeComponent();

            // setup rc calib extents
            for (var a = 0; a < rcmin.Length; a++)
            {
                rcmin[a] = 3000;
                rcmax[a] = 0;
                rctrim[a] = 1500;
            }


            // setup rc update
            timer.Tick += timer_Tick;
        }

        public void Activate()
        {
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Start();

            if (!MainUI.comPort.MAV.param.ContainsKey("RCMAP_ROLL"))
            {
                chroll = 1;
                chpitch = 2;
                chthro = 3;
                chyaw = 4;
            }
            else
            {
                //setup bindings
                chroll = (int) (float) MainUI.comPort.MAV.param["RCMAP_ROLL"];
                chpitch = (int) (float) MainUI.comPort.MAV.param["RCMAP_PITCH"];
                chthro = (int) (float) MainUI.comPort.MAV.param["RCMAP_THROTTLE"];
                chyaw = (int) (float) MainUI.comPort.MAV.param["RCMAP_YAW"];
            }

            BARroll.DataBindings.Clear();
            BARpitch.DataBindings.Clear();
            BARthrottle.DataBindings.Clear();
            BARyaw.DataBindings.Clear();
            BAR5.DataBindings.Clear();
            BAR6.DataBindings.Clear();
            BAR7.DataBindings.Clear();
            BAR8.DataBindings.Clear();
            BAR9.DataBindings.Clear();
            BAR10.DataBindings.Clear();
            BAR11.DataBindings.Clear();
            BAR12.DataBindings.Clear();
            BAR13.DataBindings.Clear();
            BAR14.DataBindings.Clear();

            BARroll.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chroll + "in", true));
            BARpitch.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chpitch + "in", true));
            BARthrottle.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chthro + "in", true));
            BARyaw.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chyaw + "in", true));

            BAR5.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch5in", true));
            BAR6.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch6in", true));
            BAR7.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch7in", true));
            BAR8.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch8in", true));

            BAR9.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch9in", true));
            BAR10.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch10in", true));
            BAR11.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch11in", true));
            BAR12.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch12in", true));
            BAR13.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch13in", true));
            BAR14.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch14in", true));

            try
            {
                // force this screen to work
                MainUI.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, 2);
            }
            catch
            {
            }

            startup = true;

            if (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduPlane ||
                MainUI.comPort.MAV.cs.firmware == Firmwares.Ateryx)
            {
                CHK_mixmode.setup(1, 0, "ELEVON_MIXING", MainUI.comPort.MAV.param);
                CHK_elevonrev.setup(1, 0, "ELEVON_REVERSE", MainUI.comPort.MAV.param);
                CHK_elevonch1rev.setup(1, 0, "ELEVON_CH1_REV", MainUI.comPort.MAV.param);
                CHK_elevonch2rev.setup(1, 0, "ELEVON_CH2_REV", MainUI.comPort.MAV.param);
            }
            else
            {
                groupBoxElevons.Visible = false;
            }

            // this controls the direction of the output, not the input.
            CHK_revch1.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC1_REV", "RC1_REVERSED"},
                MainUI.comPort.MAV.param);
            CHK_revch2.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC2_REV", "RC2_REVERSED"},
                MainUI.comPort.MAV.param);
            CHK_revch3.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC3_REV", "RC3_REVERSED"},
                MainUI.comPort.MAV.param);
            CHK_revch4.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC4_REV", "RC4_REVERSED"},
                MainUI.comPort.MAV.param);

            // run after to ensure they are disabled on copter
            if (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduCopter2)
            {
                CHK_revch1.Visible = false;
                CHK_revch2.Visible = false;
                CHK_revch3.Visible = false;
                CHK_revch4.Visible = false;
            }

            startup = false;
        }

        public void Deactivate()
        {
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // update all linked controls - 10hz
            try
            {
                MainUI.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource.UpdateDataSource(MainUI.comPort.MAV.cs));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void BUT_Calibrateradio_Click(object sender, EventArgs e)
        {
            if (run)
            {
                BUT_Calibrateradio.Text = Strings.Completed;
                run = false;
                return;
            }

            CustomMessageBox.Show(
                "Ensure your transmitter is on and receiver is powered and connected\nEnsure your motor does not have power/no props!!!");

            var oldrc = MainUI.comPort.MAV.cs.raterc;
            var oldatt = MainUI.comPort.MAV.cs.rateattitude;
            var oldpos = MainUI.comPort.MAV.cs.rateposition;
            var oldstatus = MainUI.comPort.MAV.cs.ratestatus;

            MainUI.comPort.MAV.cs.raterc = 10;
            MainUI.comPort.MAV.cs.rateattitude = 0;
            MainUI.comPort.MAV.cs.rateposition = 0;
            MainUI.comPort.MAV.cs.ratestatus = 0;

            try
            {
                MainUI.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, 10);
            }
            catch
            {
            }

            BUT_Calibrateradio.Text = Strings.Click_when_Done;

            CustomMessageBox.Show(
                "Click OK and move all RC sticks and switches to their\nextreme positions so the red bars hit the limits.");

            run = true;


            while (run)
            {
                Application.DoEvents();

                Thread.Sleep(5);

                MainUI.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource.UpdateDataSource(MainUI.comPort.MAV.cs), true, MainUI.comPort);

                // check for non 0 values
                if (MainUI.comPort.MAV.cs.ch1in > 800 && MainUI.comPort.MAV.cs.ch1in < 2200)
                {
                    rcmin[0] = Math.Min(rcmin[0], MainUI.comPort.MAV.cs.ch1in);
                    rcmax[0] = Math.Max(rcmax[0], MainUI.comPort.MAV.cs.ch1in);

                    rcmin[1] = Math.Min(rcmin[1], MainUI.comPort.MAV.cs.ch2in);
                    rcmax[1] = Math.Max(rcmax[1], MainUI.comPort.MAV.cs.ch2in);

                    rcmin[2] = Math.Min(rcmin[2], MainUI.comPort.MAV.cs.ch3in);
                    rcmax[2] = Math.Max(rcmax[2], MainUI.comPort.MAV.cs.ch3in);

                    rcmin[3] = Math.Min(rcmin[3], MainUI.comPort.MAV.cs.ch4in);
                    rcmax[3] = Math.Max(rcmax[3], MainUI.comPort.MAV.cs.ch4in);

                    rcmin[4] = Math.Min(rcmin[4], MainUI.comPort.MAV.cs.ch5in);
                    rcmax[4] = Math.Max(rcmax[4], MainUI.comPort.MAV.cs.ch5in);

                    rcmin[5] = Math.Min(rcmin[5], MainUI.comPort.MAV.cs.ch6in);
                    rcmax[5] = Math.Max(rcmax[5], MainUI.comPort.MAV.cs.ch6in);

                    rcmin[6] = Math.Min(rcmin[6], MainUI.comPort.MAV.cs.ch7in);
                    rcmax[6] = Math.Max(rcmax[6], MainUI.comPort.MAV.cs.ch7in);

                    rcmin[7] = Math.Min(rcmin[7], MainUI.comPort.MAV.cs.ch8in);
                    rcmax[7] = Math.Max(rcmax[7], MainUI.comPort.MAV.cs.ch8in);

                    rcmin[8] = Math.Min(rcmin[8], MainUI.comPort.MAV.cs.ch9in);
                    rcmax[8] = Math.Max(rcmax[8], MainUI.comPort.MAV.cs.ch9in);

                    rcmin[9] = Math.Min(rcmin[9], MainUI.comPort.MAV.cs.ch10in);
                    rcmax[9] = Math.Max(rcmax[9], MainUI.comPort.MAV.cs.ch10in);

                    rcmin[10] = Math.Min(rcmin[10], MainUI.comPort.MAV.cs.ch11in);
                    rcmax[10] = Math.Max(rcmax[10], MainUI.comPort.MAV.cs.ch11in);

                    rcmin[11] = Math.Min(rcmin[11], MainUI.comPort.MAV.cs.ch12in);
                    rcmax[11] = Math.Max(rcmax[11], MainUI.comPort.MAV.cs.ch12in);

                    rcmin[12] = Math.Min(rcmin[12], MainUI.comPort.MAV.cs.ch13in);
                    rcmax[12] = Math.Max(rcmax[12], MainUI.comPort.MAV.cs.ch13in);

                    rcmin[13] = Math.Min(rcmin[13], MainUI.comPort.MAV.cs.ch14in);
                    rcmax[13] = Math.Max(rcmax[13], MainUI.comPort.MAV.cs.ch14in);

                    rcmin[14] = Math.Min(rcmin[14], MainUI.comPort.MAV.cs.ch15in);
                    rcmax[14] = Math.Max(rcmax[14], MainUI.comPort.MAV.cs.ch15in);

                    rcmin[15] = Math.Min(rcmin[15], MainUI.comPort.MAV.cs.ch16in);
                    rcmax[15] = Math.Max(rcmax[15], MainUI.comPort.MAV.cs.ch16in);

                    BARroll.minline = (int) rcmin[chroll - 1];
                    BARroll.maxline = (int) rcmax[chroll - 1];

                    BARpitch.minline = (int) rcmin[chpitch - 1];
                    BARpitch.maxline = (int) rcmax[chpitch - 1];

                    BARthrottle.minline = (int) rcmin[chthro - 1];
                    BARthrottle.maxline = (int) rcmax[chthro - 1];

                    BARyaw.minline = (int) rcmin[chyaw - 1];
                    BARyaw.maxline = (int) rcmax[chyaw - 1];

                    setBARStatus(BAR5, rcmin[4], rcmax[4]);
                    setBARStatus(BAR6, rcmin[5], rcmax[5]);
                    setBARStatus(BAR7, rcmin[6], rcmax[6]);
                    setBARStatus(BAR8, rcmin[7], rcmax[7]);

                    setBARStatus(BAR9, rcmin[8], rcmax[8]);
                    setBARStatus(BAR10, rcmin[9], rcmax[9]);
                    setBARStatus(BAR11, rcmin[10], rcmax[10]);
                    setBARStatus(BAR12, rcmin[11], rcmax[11]);
                    setBARStatus(BAR13, rcmin[12], rcmax[12]);
                    setBARStatus(BAR14, rcmin[13], rcmax[13]);
                }
            }

            if (rcmin[0] > 800 && rcmin[0] < 2200)
            {
            }
            else
            {
                CustomMessageBox.Show("Bad channel 1 input, canceling");
                return;
            }

            CustomMessageBox.Show("Ensure all your sticks are centered and throttle is down, and click ok to continue");

            MainUI.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource.UpdateDataSource(MainUI.comPort.MAV.cs), true, MainUI.comPort);

            rctrim[0] = MainUI.comPort.MAV.cs.ch1in;
            rctrim[1] = MainUI.comPort.MAV.cs.ch2in;
            rctrim[2] = MainUI.comPort.MAV.cs.ch3in;
            rctrim[3] = MainUI.comPort.MAV.cs.ch4in;
            rctrim[4] = MainUI.comPort.MAV.cs.ch5in;
            rctrim[5] = MainUI.comPort.MAV.cs.ch6in;
            rctrim[6] = MainUI.comPort.MAV.cs.ch7in;
            rctrim[7] = MainUI.comPort.MAV.cs.ch8in;

            rctrim[8] = MainUI.comPort.MAV.cs.ch9in;
            rctrim[9] = MainUI.comPort.MAV.cs.ch10in;
            rctrim[10] = MainUI.comPort.MAV.cs.ch11in;
            rctrim[11] = MainUI.comPort.MAV.cs.ch12in;
            rctrim[12] = MainUI.comPort.MAV.cs.ch13in;
            rctrim[13] = MainUI.comPort.MAV.cs.ch14in;
            rctrim[14] = MainUI.comPort.MAV.cs.ch15in;
            rctrim[15] = MainUI.comPort.MAV.cs.ch16in;

            var data = "---------------\n";

            for (var a = 0; a < rctrim.Length; a++)
            {
                // we want these to save no matter what
                BUT_Calibrateradio.Text = Strings.Saving;
                try
                {
                    if (rcmin[a] != rcmax[a])
                    {
                        MainUI.comPort.setParam("RC" + (a + 1).ToString("0") + "_MIN", rcmin[a]);
                        MainUI.comPort.setParam("RC" + (a + 1).ToString("0") + "_MAX", rcmax[a]);
                    }
                    if (rctrim[a] < 1195 || rctrim[a] > 1205)
                        MainUI.comPort.setParam("RC" + (a + 1).ToString("0") + "_TRIM", rctrim[a]);
                }
                catch
                {
                    if (MainUI.comPort.MAV.param.ContainsKey("RC" + (a + 1).ToString("0") + "_MIN"))
                        CustomMessageBox.Show("Failed to set Channel " + (a + 1));
                }

                data = data + "CH" + (a + 1) + " " + rcmin[a] + " | " + rcmax[a] + "\n";
            }

            MainUI.comPort.MAV.cs.raterc = oldrc;
            MainUI.comPort.MAV.cs.rateattitude = oldatt;
            MainUI.comPort.MAV.cs.rateposition = oldpos;
            MainUI.comPort.MAV.cs.ratestatus = oldstatus;

            try
            {
                MainUI.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, oldrc);
            }
            catch
            {
            }

            CustomMessageBox.Show(
                "Here are the detected radio options\nNOTE Channels not connected are displayed as 1500 +-2\nNormal values are around 1100 | 1900\nChannel:Min | Max \n" +
                data, "Radio");

            BUT_Calibrateradio.Text = Strings.Completed;
        }

        private void setBARStatus(HorizontalProgressBar2 bar, float min, float max)
        {
            bar.minline = (int) min;
            bar.maxline = (int) max;
        }

        private void CHK_revch1_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARroll);
        }

        private void CHK_revch2_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARpitch);
        }

        private void CHK_revch3_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARthrottle);
        }

        private void CHK_revch4_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARyaw);
        }

        private void reverseChannel(bool normalreverse, Control progressbar)
        {
            if (normalreverse)
            {
                ((HorizontalProgressBar2) progressbar).reverse = true;
                ((HorizontalProgressBar2) progressbar).BackgroundColor = Color.FromArgb(148, 193, 31);
                ((HorizontalProgressBar2) progressbar).ValueColor = Color.FromArgb(0x43, 0x44, 0x45);
            }
            else
            {
                ((HorizontalProgressBar2) progressbar).reverse = false;
                ((HorizontalProgressBar2) progressbar).BackgroundColor = Color.FromArgb(0x43, 0x44, 0x45);
                ((HorizontalProgressBar2) progressbar).ValueColor = Color.FromArgb(148, 193, 31);
            }

            if (startup)
                return;
            if (MainUI.comPort.MAV.param["SWITCH_ENABLE"] != null &&
                (float) MainUI.comPort.MAV.param["SWITCH_ENABLE"] == 1)
            {
                try
                {
                    MainUI.comPort.setParam("SWITCH_ENABLE", 0);
                    CustomMessageBox.Show("Disabled Dip Switchs");
                }
                catch
                {
                    CustomMessageBox.Show("Error Disableing Dip Switch");
                }
            }
        }

        private void BUT_Bindradiodsm2_Click(object sender, EventArgs e)
        {
            try
            {
                MainUI.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 0, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }

        private void BUT_BindradiodsmX_Click(object sender, EventArgs e)
        {
            try
            {
                MainUI.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 1, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }

        private void BUT_Bindradiodsm8_Click(object sender, EventArgs e)
        {
            try
            {
                MainUI.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 2, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }
    }
}