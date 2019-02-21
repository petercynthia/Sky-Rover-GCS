using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner;
using GMap.NET;
using MissionPlanner.Maps;
using GMap.NET.WindowsForms;
using MissionPlanner.Controls;

namespace SKYROVER.GCS.DeskTop.MapTools
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CommandsControl : UserControl
    {
        private myGMAP mMapControl;

        /// <summary>
        /// 
        /// </summary>
        MAVLinkInterface mMAVLinkInterface;
        /// <summary>
        /// 获取MAVLink数据接口
        /// </summary>
        public MAVLinkInterface SetCOMPort { set => mMAVLinkInterface = value; }
        /// <summary>
        /// 获取控件
        /// </summary>
        public myGMAP MMapControl { get => mMapControl; set => mMapControl = value; }
        /// <summary>
        /// 
        /// </summary>
        public CommandsControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置Home点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetHome_Click(object sender, EventArgs e)
        {
            if (mMAVLinkInterface != null && mMAVLinkInterface.BaseStream.IsOpen)
            {
                if (mMAVLinkInterface.MAV.cs.Location.Lat == 0) return;
                mMAVLinkInterface.MAV.cs.HomeLocation = mMAVLinkInterface.MAV.cs.Location;

                PointLatLng point = new PointLatLng(mMAVLinkInterface.MAV.cs.Location.Lat, mMAVLinkInterface.MAV.cs.Location.Lng);
                GMapMarkerWP m = new GMapMarkerWP(point, "H");
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                m.ToolTipText = "Alt: 0";
                m.Tag = "H";
                MainUI.homelayer.Markers.Clear();
                MainUI.homelayer.Markers.Add(m);

                MainUI.MainUIInstance.getGMAP.Position = new GMap.NET.PointLatLng(mMAVLinkInterface.MAV.cs.Location.Lat, mMAVLinkInterface.MAV.cs.Location.Lng);

            }
            else {
                
                PointLatLng point = MainUI.MainUIInstance.getGMAP.Position;
                mMAVLinkInterface.MAV.cs.HomeLocation = point;
                GMapMarkerWP m = new GMapMarkerWP(point, "H");
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                m.ToolTipText = "Alt: 0";
                m.Tag = "H";
                MainUI.homelayer.Markers.Clear();
                MainUI.homelayer.Markers.Add(m);

            }
        }

        /// <summary>
        /// used to add a marker to the map display
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="alt"></param>
        /// <param name="color"></param>
        private void addpolygonmarker(string tag, double lng, double lat, double alt, Color? color, GMapOverlay overlay)
        {
            try
            {
                overlay.Markers.Clear();
                PointLatLng point = new PointLatLng(lat, lng);
                GMapMarkerWP m = new GMapMarkerWP(point, tag);
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                m.ToolTipText = "Alt: " + alt.ToString("0");
                m.Tag = tag;

                int wpno = -1;
                if (int.TryParse(tag, out wpno))
                {
                    // preselect groupmarker
                    //if (groupmarkers.Contains(wpno))
                    //    m.selected = true;
                }

                overlay.Markers.Add(m);
                // overlay.Markers.Add(mBorders);
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMission_Click(object sender, EventArgs e)
        {
            if (mMAVLinkInterface != null && mMAVLinkInterface.BaseStream.IsOpen)
            {
                //判断当前状态
                var mode = mMAVLinkInterface.MAV.cs.mode;


                mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.MISSION_START, 0, 0, 1, 0, 0, 0, 0);

                return;

                switch (mode)
                {
                    case "Auto":
                        if (mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.MISSION_START, 0, 0, 1, 0, 0, 0, 0))
                        {
                            mMAVLinkInterface.setMode("RTL");
                            btnMission.Text = "RTL";
                        }
                        break;
                    case "RTL":

                        if (mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.RETURN_TO_LAUNCH, 0, 0, 0, 0, 0, 0, 0))
                        {
                            mMAVLinkInterface.setMode("Auto");
                            btnMission.Text = "Start";
                        }
                        break;

                }              
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (mMAVLinkInterface != null && mMAVLinkInterface.BaseStream.IsOpen)
            {
                if (mMAVLinkInterface.MAV.cs.failsafe)
                {
                    if (CustomMessageBox.Show("You are in failsafe, are you sure?", "Failsafe", MessageBoxButtons.YesNo) != (int)DialogResult.Yes)
                    {
                        return;
                    }
                }
                mMAVLinkInterface.setMode("Auto");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!mMAVLinkInterface.BaseStream.IsOpen)
                return;

            // arm the MAV
            try
            {
                if (mMAVLinkInterface.MAV.cs.armed)
                    if (CustomMessageBox.Show("确定要加锁？", "加锁?", MessageBoxButtons.YesNo) !=
                        (int)DialogResult.Yes)
                        return;

                bool ans = mMAVLinkInterface.doARM(!mMAVLinkInterface.MAV.cs.armed);
                if (ans == false)
                    CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
            }
            catch
            {
                CustomMessageBox.Show(Strings.ErrorNoResponce, Strings.ERROR);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!mMAVLinkInterface.BaseStream.IsOpen)
                return;
            bool state = mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.MISSION_START, 0, 0, 2, 0, 0, 0, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!mMAVLinkInterface.BaseStream.IsOpen)
                return;
            bool state = mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.MISSION_START, 0, 0, 3, 0, 0, 0, 0);

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (!mMAVLinkInterface.BaseStream.IsOpen)
                return;
            bool state = mMAVLinkInterface.doCommand(MAVLink.MAV_CMD.PREFLIGHT_REBOOT_SHUTDOWN, 1, 0, 1, 0, 0, 0, 0);

        }
    }
}
