using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.Joystick
{
    public partial class Joy_Mount_Mode : Form
    {
        public Joy_Mount_Mode(string name)
        {
            InitializeComponent();

           // Utilities.ThemeManager.ApplyThemeTo(this);

            this.Tag = name;

            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("MNT_MODE",
                MainUI.comPort.MAV.cs.firmware.ToString());

            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            var config = MainUI.joystick.getButton(int.Parse(name));

            comboBox1.SelectedValue = (int) config.p1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the index
            int name = int.Parse(this.Tag.ToString());

            // get existing config
            var config = MainUI.joystick.getButton(name);

            // change what we modified
            config.function = Joystick.buttonfunction.Mount_Mode;
            config.p1 = (int) comboBox1.SelectedValue;

            // update entry
            MainUI.joystick.setButton(name, config);
        }
    }
}