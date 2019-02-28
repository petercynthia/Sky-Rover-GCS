using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Utilities;

namespace SKYROVER.GCS.DeskTop.Controls
{
   
    /// <summary>
    /// 投掷
    /// </summary>
    public partial class CtlDoSetServoMAV : UserControl, ILocationwp
    {
        public event AfterDeleteMAV AfterDeleteMAVEvent;

        private Locationwp locationwp;

        public Locationwp Locationwp { get => locationwp; set => locationwp = value; }


        private int servoNum;
        private int PWM;
        /// <summary>
        /// 设置投掷值
        /// </summary>
        /// <param name="servoNum"></param>
        /// <param name="PWM"></param>
        public void SetParameters(int servoNum,int PWM) {

            this.servoNum = servoNum;
            this.PWM = PWM;
            locationwp.p1 = servoNum;
            locationwp.p2 = PWM;
            if (this.servoNum == int.Parse(Settings.config["txtServoChanel1"]))
            {
                this.jettisonOne.Checked = true;
                if (PWM == int.Parse(Settings.config["txtSC1PWMOn"]))
                {
                    this.btnSwitch.Switched = true;
                }
                else if (PWM == int.Parse(Settings.config["txtSC1PWMOn"]))
                {
                    this.btnSwitch.Switched = false;
                }
            }
            else if (this.servoNum == int.Parse(Settings.config["txtServoChanel2"])) {

                this.jettisonTwo.Checked = true;
                if (PWM == int.Parse(Settings.config["txtSC2PWMOn"]))
                {
                    this.btnSwitch.Switched = true;
                }
                else if (PWM == int.Parse(Settings.config["txtSC2PWMOn"]))
                {
                    this.btnSwitch.Switched = false;
                }
            }


        }

        public CtlDoSetServoMAV()
        {
            InitializeComponent();
            CreateCommand();

        }

        private void CreateCommand()
        {

            locationwp = new Locationwp() {

                id = (ushort)MAVLink.MAV_CMD.DO_SET_SERVO,
                p1= servoNum,
                p2=PWM
                
            };
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (AfterDeleteMAVEvent != null) AfterDeleteMAVEvent(this);
        }

        private void jettisonOne_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings.config["txtServoChanel1"]!=null&&Settings.config["txtSC1PWMOn"] !=null) {
            
             locationwp.p1 = float.Parse(Settings.config["txtServoChanel1"]);
             locationwp.p2= this.PWM;
            }
        }

        private void jettisonTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings.config["txtServoChanel2"] != null && Settings.config["txtSC2PWMOn"] != null)
            {

                locationwp.p1 = float.Parse(Settings.config["txtServoChanel2"]);
                locationwp.p2 = this.PWM;
            }
        }

        private void btnSwitch_SwitchedChanged(object sender)
        {
            if (btnSwitch.Switched)
            {

                if (this.jettisonOne.Checked) { this.PWM = int.Parse(Settings.config["txtSC1PWMOn"]); }
                else if (this.jettisonTwo.Checked) { this.PWM = int.Parse(Settings.config["txtSC2PWMOn"]); }

            }
            else {
                
                if (this.jettisonOne.Checked) { this.PWM = int.Parse(Settings.config["txtSC1PWMOff"]); }
                else if (this.jettisonTwo.Checked) { this.PWM = int.Parse(Settings.config["txtSC2PWMOff"]); }

              
            }
            locationwp.p2 = this.PWM;
        }
    }




}
