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
    /// 相机拍照
    /// </summary>
    public partial class CtlDigitalCamMAV : UserControl, ILocationwp
    {
        public event AfterDeleteMAV AfterDeleteMAVEvent;

        private Locationwp locationwp;

        public Locationwp Locationwp { get => locationwp; set => locationwp = value; }     


        public CtlDigitalCamMAV()
        {
            InitializeComponent();
            CreateCommand();

        }

        private void CreateCommand()
        {

            locationwp = new Locationwp() {

                id = (ushort)MAVLink.MAV_CMD.DO_DIGICAM_CONTROL,
                
            };
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (AfterDeleteMAVEvent != null) AfterDeleteMAVEvent(this);
        }
       
    }




}
