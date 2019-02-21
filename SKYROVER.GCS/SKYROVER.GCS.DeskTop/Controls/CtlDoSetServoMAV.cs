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

            if (this.servoNum == 5) this.jettisonOne.Checked = true;
            else if (this.servoNum == 6) this.jettisonTwo.Checked = true;


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
            locationwp.p1 = 5;
        }

        private void jettisonTwo_CheckedChanged(object sender, EventArgs e)
        {
            locationwp.p1 = 6;
        }
    }




}
