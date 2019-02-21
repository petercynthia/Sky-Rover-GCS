﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.Joystick
{
    public partial class Joy_Do_Set_Servo : Form
    {
        public Joy_Do_Set_Servo(string name)
        {
            InitializeComponent();

           // Utilities.ThemeManager.ApplyThemeTo(this);

            this.Tag = name;

            var config = MainUI.joystick.getButton(int.Parse(name));

            numericUpDownservono.Text = config.p1.ToString();
            numericUpDownpwm.Text = config.p2.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var config = MainUI.joystick.getButton(int.Parse(this.Tag.ToString()));

            config.p1 = (float) numericUpDownservono.Value;

            MainUI.joystick.setButton(int.Parse(this.Tag.ToString()), config);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            var config = MainUI.joystick.getButton(int.Parse(this.Tag.ToString()));

            config.p2 = (float) numericUpDownpwm.Value;

            MainUI.joystick.setButton(int.Parse(this.Tag.ToString()), config);
        }
    }
}