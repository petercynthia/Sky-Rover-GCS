using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.ArduPilot;

namespace SKYROVER.GCS.DeskTop.Joystick
{
    public partial class Joy_ChangeMode : Form
    {
        public Joy_ChangeMode(string name)
        {
            InitializeComponent();

            //Utilities.ThemeManager.ApplyThemeTo(this);

            this.Tag = name;

            comboBox1.DataSource = Common.getModesList(MainUI.comPort.MAV.cs.firmware);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            var config = MainUI.joystick.getButton(int.Parse(name));

            comboBox1.Text = config.mode;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the index
            int name = int.Parse(this.Tag.ToString());

            // get existing config
            var config = MainUI.joystick.getButton(name);

            // change what we modified
            config.function = Joystick.buttonfunction.ChangeMode;
            config.mode = comboBox1.Text.ToString();

            // update entry
            MainUI.joystick.setButton(name, config);
        }
    }
}