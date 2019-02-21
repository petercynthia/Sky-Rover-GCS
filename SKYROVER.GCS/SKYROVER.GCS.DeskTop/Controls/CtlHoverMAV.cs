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
    public delegate void AfterDeleteMAV(Control ctlHoverMAV);
    /// <summary>
    /// 悬停控件
    /// </summary>
    public partial class CtlHoverMAV : UserControl, ILocationwp
    {
        public event AfterDeleteMAV AfterDeleteMAVEvent;

        private Locationwp locationwp;

        public Locationwp Locationwp { get => locationwp; set => locationwp = value; }

       


        public CtlHoverMAV()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (AfterDeleteMAVEvent != null) AfterDeleteMAVEvent(this);
        }

        public void SetDefaultValue(int max,int min,int value)
        {
            this.HoverTrackBar.Maximum = max;
            this.HoverTrackBar.Minimum = min;
            this.HoverTrackBar.Value = value;
            this.lblHoverValue.Text = value.ToString();

        }

        private void HoverTrackBar_Scroll(object sender)
        {
            this.lblHoverValue.Text = this.HoverTrackBar.Value.ToString();
            if (this.HoverTrackBar.Value > this.HoverTrackBar.Minimum && this.HoverTrackBar.Value < this.HoverTrackBar.Maximum) {
                this.btnDesc.Enabled = true;
                this.btnAsc.Enabled = true;
            }
            else if (this.HoverTrackBar.Value == this.HoverTrackBar.Minimum) this.btnDesc.Enabled = false;
            else if (this.HoverTrackBar.Value == this.HoverTrackBar.Maximum) this.btnAsc.Enabled = false;

            locationwp.id=(ushort)MAVLink.MAV_CMD.LOITER_TIME;
            locationwp.p1 = (float)this.HoverTrackBar.Value;

         
        }

        private void btnAsc_Click(object sender, EventArgs e)
        {
            if (this.HoverTrackBar.Value < this.HoverTrackBar.Maximum) this.HoverTrackBar.Value += 1;
        }

        private void btnDesc_Click(object sender, EventArgs e)
        {
            if (this.HoverTrackBar.Value > this.HoverTrackBar.Minimum) this.HoverTrackBar.Value -= 1;
           
        }
    }




}
