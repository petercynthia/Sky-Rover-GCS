using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
   public class MAVLinkTextBox:TextBox
    {
        [System.ComponentModel.Browsable(true)]
        public decimal Min { get; set; }

        [System.ComponentModel.Browsable(true)]
        public decimal Max { get; set; }

        [System.ComponentModel.Browsable(true)]
        public string ParamName { get; set; }


        Control _control;
        float _scale = 1;

        //Timer timer = new Timer();

        [System.ComponentModel.Browsable(true)]
        public event EventHandler ValueUpdated;

        public MAVLinkTextBox()
        {
            Min = 0;
            Max = 1;

            this.Name = "MavlinkTextBox";

            //timer.Tick += Timer_Tick;

            this.Enabled = false;
        }

        public void setup(float Min, float Max, float Scale,  string paramname,
            MAVLink.MAVLinkParamList paramlist, Control enabledisable = null)
        {
            setup(Min, Max, Scale,  new string[] { paramname }, paramlist, enabledisable);
        }

        public void setup(float Min, float Max, float Scale, string[] paramname,
            MAVLink.MAVLinkParamList paramlist, Control enabledisable = null)
        {
            this.KeyPress -= MAVLinkTextBox_KeyPress;
            this.KeyUp -= MavlinkNumericUpDown_ValueChanged;


            // default to first item
            this.ParamName = paramname[0];


            // set a new item is first item doesnt exist
            foreach (var paramn in paramname)
            {
                if (paramlist.ContainsKey(paramn))
                {
                    this.ParamName = paramn;
                    break;
                }
            }

            // update local name
            Name = ParamName;
            // set min and max of both are equal
            if (Min == Max)
            {
                double mint = Min, maxt = Max;
                ParameterMetaDataRepository.GetParameterRange(ParamName, ref mint, ref maxt,
                    MainV2.comPort.MAV.cs.firmware.ToString());
                Min = (float)mint;
                Max = (float)maxt;
            }

           
            _scale = Scale;
            this.Min= (decimal)(Min);
            this.Max = (decimal)(Max);
           // this.Increment = (decimal)(Increment);
            // this.DecimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)Increment)[3])[2];

            this._control = enabledisable;

            if (paramlist.ContainsKey(ParamName))
            {
                this.Enabled = true;
                this.Visible = true;

                enableControl(true);

                decimal value = (decimal)((float)paramlist[ParamName] / _scale);

                int dec = BitConverter.GetBytes(decimal.GetBits((decimal)value)[3])[2];

               
                if (value < this.Min)
                    this.Min = value;
                if (value > this.Max)
                    this.Max = value;

                base.Text = value.ToString();
            }
            else
            {
                this.Enabled = false;
                enableControl(false);
            }
            this.KeyPress -= MAVLinkTextBox_KeyPress;
            this.KeyUp += new KeyEventHandler(MavlinkNumericUpDown_ValueChanged);
        }

        private void MAVLinkTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)

                e.Handled = true;

            //小数点的处理。

            if ((int)e.KeyChar == 46)                           //小数点

            {

                if (this.Text.Length <= 0)

                    e.Handled = true;   //小数点不能在第一位

                else

                {

                    float f;

                    float oldf;

                    bool b1 = false, b2 = false;

                    b1 = float.TryParse(this.Text, out oldf);

                    b2 = float.TryParse(this.Text + e.KeyChar.ToString(), out f);

                    if (b2 == false)

                    {

                        if (b1 == true)

                            e.Handled = true;

                        else

                            e.Handled = false;

                    }

                }

            }
        }

        void enableControl(bool enable)
        {
            if (_control != null)
            {
                _control.Enabled = enable;
                _control.Visible = true;
            }
        }

        void MavlinkNumericUpDown_ValueChanged(object sender, KeyEventArgs e)
        {
            string value = base.Text;

            //数据合法性检查
            if (e.KeyCode==Keys.Enter)
            {

                try {

                    decimal.Parse(value);

                } catch (Exception ex) { this.Focus();return; }
        

            if (decimal.Parse(value) > this.Max)
            {
                if (
                    CustomMessageBox.Show(ParamName + " Value out of range\nDo you want to accept the new value?",
                        "Out of range", MessageBoxButtons.YesNo) == (int)DialogResult.Yes)
                {
                   this.Max = decimal.Parse(value);
                  
                }
            }

            if (ValueUpdated != null)
            {
                //this.UpdateEditText();
                ValueUpdated(this, new MAVLinkParamChanged(ParamName, (float.Parse(base.Text)) * (float)_scale));
                return;
            }
            }

            //lock (timer)
            //{
            //    timer.Interval = 300;

            //    if (!timer.Enabled)
            //        timer.Start();
            //}
        }

      

        private void Timer_Tick(object sender, EventArgs e)
        {
            //lock (timer)
            //{
            //    try
            //    {
            //        bool ans = MainV2.comPort.setParam(ParamName, (float.Parse(base.Text)) * (float)_scale);
            //        if (ans == false)
            //            CustomMessageBox.Show(String.Format(Strings.ErrorSetValueFailed, ParamName), Strings.ERROR);
            //    }
            //    catch
            //    {
            //        CustomMessageBox.Show(String.Format(Strings.ErrorSetValueFailed, ParamName), Strings.ERROR);
            //    }

            //    timer.Stop();
            //}
        }

    }
}
