using GMap.NET.WindowsForms;
using MissionPlanner;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.MessagePanel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SKYROVER.GCS.DeskTop;
using MissionPlanner.Controls;
using System.Drawing.Drawing2D;
using SKYROVER.GCS.DeskTop.Utilities;
using GMap.NET;
using MissionPlanner.Maps;
using GMap.NET.WindowsForms.Markers;

namespace SKYROVER.GCS.DeskTop.MenuItems
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmLineWayPoints : BaseForm
    {
        #region 属性、字段
        /// <summary>
        /// 地图控件
        /// </summary>
        public myGMAP mapControl { get; set; }
        private int WPIndex = 0;        
        private List<LocationwpEx> locationwpExs;
        private GMapRoute mapRoute;
        /// <summary>
        /// 航线属性
        /// </summary>
        public GMapRoute MapRoute { get => mapRoute; set =>mapRoute = value;}
        /// <summary>
        /// 航点信息记录
        /// </summary>
        public List<LocationwpEx> LocationwpExs { get => locationwpExs; set => locationwpExs = value; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public FrmLineWayPoints()
        {
            InitializeComponent();
        }
        #endregion
       
        #region 私有方法



        private void FrmLineWayPoints_Load(object sender, EventArgs e)
        {
            this.metroSetTabControl1.ItemSize=new Size((this.Width - 10) / 2,40);
        }

        private void RouteSpeedBar_Scroll(object sender)
        {
            this.lblRouteSpeed.Text = RouteSpeedBar.Value.ToString() + "m/s";

            if (locationwpExs.Count > 2)
            {
                for (int index = 1; index < locationwpExs.Count - 1; index++)
                {
                    locationwpExs[index].locationwps[0] = new Locationwp()
                    {
                        id = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED,
                        p1 = 0,
                        p2 = this.RouteSpeedBar.Value
                    };
                }
            }

            CalculateTotalInfo(this.mapRoute,null);
        }

        private void RouteAltBar_Scroll(object sender)
        {
            this.lblRouteAlt.Text = RouteAltBar.Value.ToString() + "m";
            if (locationwpExs.Count > 2)
            {
                for (int index = 1; index < locationwpExs.Count - 1; index++)
                {
                    locationwpExs[index].WP.alt = this.RouteAltBar.Value;
                   
                }
            }

        }

        private void WayPointAltBar_Scroll(object sender)
        {
            this.lblWayPointAlt.Text = this.WayPointAltBar.Value.ToString() + "m";
            int index = WPIndex / 2 + WPIndex % 2;
            if (index < this.locationwpExs.Count && index > 0)
            {
                this.locationwpExs[index].WP.alt=this.WayPointAltBar.Value;
            }
        }

        private void WayPointSpeedBar_Scroll(object sender)
        {
            this.lblWayPointSpeed.Text = this.WayPointSpeedBar.Value.ToString() + "m/s";
            int index = WPIndex / 2 + WPIndex % 2;
            if (index < this.locationwpExs.Count&& index > 0)
            {
                this.locationwpExs[index].locationwps[0] = new Locationwp() {
                    id = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED,
                    p1 = 0,
                    p2 = this.WayPointSpeedBar.Value

                };
            }
        }

        private void btnHiden_Click(object sender, EventArgs e)
        {
            
            base.ShowForm();
        }


        string secondsToNice(double seconds)
        {
            if (seconds < 0)
                return "无效数字";

            double secs = seconds % 60;
            double mins = seconds / 60;
            double hours = seconds / 3600;// % 24;

            if (hours > 1)
            {
                return hours.ToString("f2") + " 小时";
            }
            else if (mins > 1)
            {
                return mins.ToString("f2") + "分钟";
            }
            else
            {
                return secs.ToString("0.00") + "秒";
            }
        }



        /// <summary>
        /// 上传航点
        /// </summary>
        /// <param name="commands"></param>
        private void UploadWPs(List<LocationwpEx> commands)
        {
            try
            {
                MAVLinkInterface port = MainUI.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("请链接飞控!");
                }

                MainUI.comPort.giveComport = true;
                int a = 0;

                //1. 获取home点
                Locationwp home = new Locationwp();
                try
                {
                    home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                    home.lat = MainUI.comPort.MAV.cs.HomeLocation.Lat;
                    home.lng = MainUI.comPort.MAV.cs.HomeLocation.Lng;
                    home.alt = (float)(MainUI.comPort.MAV.cs.HomeLocation.Alt / CurrentState.multiplieralt); // use saved home

                }
                catch
                {
                    throw new Exception("请设置Home点");
                }

                // 2. 获取总航点数               
                int WPsNum = 0;
                // get the command list from the datagrid
                var commandlist = GetCommandList();
                WPsNum = commandlist.Count;

                if (MainUI.comPort.MAV.wps.Values.Count == (WPsNum + 1))
                {
                    Hashtable wpstoupload = new Hashtable();

                    a = -1;
                    foreach (var item in MainUI.comPort.MAV.wps.Values)
                    {
                        // skip home
                        if (a == -1)
                        {
                            a++;
                            continue;
                        }
                        MAVLink.mavlink_mission_item_t temp = commandlist[a];


                        if (temp.command == item.command &&
                            temp.x == item.x &&
                            temp.y == item.y &&
                            temp.z == item.z &&
                            temp.param1 == item.param1 &&
                            temp.param2 == item.param2 &&
                            temp.param3 == item.param3 &&
                            temp.param4 == item.param4
                            )
                        {
                            // log.Info("wp match " + (a + 1));
                        }
                        else
                        {
                            //  log.Info("wp no match" + (a + 1));
                            wpstoupload[a] = "";
                        }

                        a++;
                    }
                }

                bool use_int = (port.MAV.cs.capabilities & (uint)MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_INT) > 0;

                // set wp total
                this.pgBar.Value = 0;

                ushort totalwpcountforupload = (ushort)(WPsNum + 1);

                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    totalwpcountforupload--;
                }

                port.setWPTotal(totalwpcountforupload); // + home

                // set home location - overwritten/ignored depending on firmware.
                this.pgBar.Value = 0;

                // upload from wp0
                a = 0;

                if (port.MAV.apname != MAVLink.MAV_AUTOPILOT.PX4)
                {
                    try
                    {
                        var homeans = port.setWP(home, (ushort)a, MAVLink.MAV_FRAME.GLOBAL, 0, 1, use_int);
                        if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                        {
                            if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                            {
                                CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                                return;
                            }
                        }
                        a++;
                    }
                    catch (TimeoutException)
                    {
                        use_int = false;
                        // added here to prevent timeout errors
                        port.setWPTotal(totalwpcountforupload);
                        var homeans =  port.setWP(home, (ushort)a, MAVLink.MAV_FRAME.GLOBAL, 0, 1, use_int);
                        if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                        {
                            if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                            {
                                CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                                return;
                            }
                        }
                        a++;
                    }
                }
                else
                {
                    use_int = false;
                }

                // define the default frame.
                MAVLink.MAV_FRAME frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;               

                // process commandlist to the mav
                for (a = 1; a <= commandlist.Count; a++)
                {
                    var temp = commandlist[a-1];
                    // make sure we are using the correct frame for these commands
                    this.pgBar.Value = a * 100 / commandlist.Count;
                    if (temp.id < (ushort)MAVLink.MAV_CMD.LAST || temp.id == (ushort)MAVLink.MAV_CMD.DO_SET_HOME)
                    {
                        frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;
                    }

                    // handle current wp upload number
                    int uploadwpno = a;
                    if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                        uploadwpno--;

                    // try send the wp
                    MAVLink.MAV_MISSION_RESULT ans = port.setWP(temp, (ushort)(uploadwpno), frame, 0, 1, use_int);

                    // we timed out while uploading wps/ command wasnt replaced/ command wasnt added
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ERROR)
                    {
                        // resend for partial upload
                        port.setWPPartialUpdate((ushort)(uploadwpno), totalwpcountforupload);
                        // reupload this point.
                        ans = port.setWP(temp, (ushort)(uploadwpno), frame, 0, 1, use_int);
                    }

                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_NO_SPACE)
                    {
                        string ErrorMessage = "上传失败, 请减少航点数量";
                        return;
                    }
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID)
                    {
                        string ErrorMessage =
                             "上传失败, 任务被拒绝 wp# " + a + " " +  ans;
                        return;
                    }
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                    {
                        // invalid sequence can only occur if we failed to see a response from the apm when we sent the request.
                        // or there is io lag and we send 2 mission_items and get 2 responces, one valid, one a ack of the second send

                        // the ans is received via mission_ack, so we dont know for certain what our current request is for. as we may have lost the mission_request

                        // get requested wp no - 1;
                        a = port.getRequestedWPNo() - 1;

                        continue;
                    }
                    if (ans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                    {
                        string ErrorMessage = "上传航点失败 " + Enum.Parse(typeof(MAVLink.MAV_CMD), temp.id.ToString()) +
                                          " " + Enum.Parse(typeof(MAVLink.MAV_MISSION_RESULT), ans.ToString());
                        return;
                    }
                }
                port.setWPACK();
                this.pgBar.Value = 95;
                // m
                port.setParam("WP_RADIUS", 5.0 / CurrentState.multiplierdist);
                // cm's
                port.setParam("WPNAV_RADIUS", 5.0 / CurrentState.multiplierdist * 100.0);
                port.MAV.wps.Clear();
                this.pgBar.Value = 100;
            }
            catch (Exception ex)
            {
                // log.Error(ex);
                MainUI.comPort.giveComport = false;
                throw;
            }



            MainUI.comPort.giveComport = false;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Locationwp> GetCommandList()
        {
            List<Locationwp> commands = new List<Locationwp>();
            if (locationwpExs != null)
            {

                for (int firstIndex = 0; firstIndex < locationwpExs.Count; firstIndex++)
                {
                    if (firstIndex != 0&&firstIndex!=locationwpExs.Count-1) commands.Add(locationwpExs[firstIndex].WP);
                    for (int secondIndex = 0; secondIndex < locationwpExs[firstIndex].locationwps.Count; secondIndex++)
                    {
                        commands.Add(locationwpExs[firstIndex].locationwps[secondIndex]);
                    }
                }
                return commands;
            }


            return commands;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        Locationwp DataViewtoLocationwp(int a)
        {
            try
            {
                if (locationwpExs != null)
                {

                    for (int firstIndex = 0; firstIndex < locationwpExs.Count; firstIndex++)
                    {

                        for (int secondIndex = 0; secondIndex < locationwpExs[firstIndex].locationwps.Count; secondIndex++)
                        {

                            if ((firstIndex + 1) * (secondIndex + 1) == (a + 1))
                            {
                                return locationwpExs[firstIndex].locationwps[secondIndex];
                            }

                        }
                    }
                    return null;
                }
                else return null;



            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid number on row " + (a + 1).ToString(), ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMissions_Click(object sender, EventArgs e)
        {
            int index = WPIndex / 2 + WPIndex % 2;

            frmWayPointCommands frmWayPointCommands = new frmWayPointCommands();
            frmWayPointCommands.MAVCommandsParameterChangeEvent += FrmWayPointCommands_MAVCommandsParameterChangeEvent;
            frmWayPointCommands.Init(locationwpExs[index].locationwps);
            frmWayPointCommands.Location = this.Location;
            frmWayPointCommands.ShowDialog();
        }
        /// <summary>
        /// 关闭添加命令后的回调
        /// </summary>
        /// <param name="cmds"></param>
        private void FrmWayPointCommands_MAVCommandsParameterChangeEvent(List<Locationwp> cmds)
        {
            this.btnMissions.Text = cmds.Count + "项任务";
            int index = WPIndex / 2 + WPIndex % 2;
            locationwpExs[index].locationwps.RemoveRange(1, locationwpExs[index].locationwps.Count-1);
            foreach (var cmd in cmds)
            {
                locationwpExs[index].locationwps.Add(cmd);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            metroSetTabControl1.SelectedTab = metroSetTabControl1.TabPages[0];

            this.pgBar.Visible = true;            
            UploadWPs(locationwpExs);
            this.pgBar.Visible = false;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            metroSetTabControl1.SelectedTab = metroSetTabControl1.TabPages[0];
            this.pgBar.Visible = true;
            //1 检测home点是否存在
            if (MainUI.homelayer.Markers.Count !=1) return;
            MainUI.comPort.MAV.cs.HomeLocation = MainUI.homelayer.Markers[0].Position;

            //2 清除已有航线数据
            //
            if (MainUI.comPort.MAV.wps != null && MainUI.comPort.MAV.wps.Count > 0) MainUI.comPort.MAV.wps.Clear();

            if (locationwpExs != null && locationwpExs.Count > 0)
            {
                locationwpExs.Clear();
            }

            //清除已存在的航线
            foreach (var lyr in mapControl.Overlays)
            {
                if (lyr.Id == "polygons" || lyr.Id == "WPOverlay" || lyr.Id == "surveyGridlayer" || lyr.Id == "WPMapOverlay")
                {

                    lyr.Polygons.Clear();
                    lyr.Markers.Clear();
                    lyr.Routes.Clear();
                }
            }
            //获取航点信息
            getWPs();
            this.pgBar.Visible = false;

        }
        /// <summary>
        /// 
        /// </summary>
        void getWPs()
        {
            List<LocationwpEx> cmds = new List<LocationwpEx>();

            try
            {
                MAVLinkInterface port = MainUI.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("请先连接飞控!");
                }

                MainUI.comPort.giveComport = true;

               
                int flag = 0;
                this.pgBar.Value = 0;
                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    LocationwpEx temp = new LocationwpEx(flag++);
                    temp.locationwps = new List<Locationwp>();
                    try
                    {

                        temp.WP = port.getHomePosition();
                        temp.locationwps.Add(port.getWP(0));

                        cmds.Add(temp);

                    }
                    catch (TimeoutException)
                    {
                        // blank home
                        temp.WP = new Locationwp() { id = (ushort)MAVLink.MAV_CMD.WAYPOINT };
                        temp.locationwps.Add(port.getWP(0));
                        cmds.Add(temp);

                    }
                }

                // log.Info("Getting WP #");


                int cmdcount = port.getWPCount();
                Console.WriteLine("共" + cmdcount + "个航点命令");
                for (ushort a = 1; a <= cmdcount-1; a++)
                {
                    Console.WriteLine("获取第"+a+"个航点命令");
                    Locationwp temp = port.getWP(a);

                    //读取home命令
                    if ((a-1) == 0||(a-2)==0)
                    {
                        if (cmds.Count == 1)
                        {
                            cmds[0].locationwps.Add(temp);
                        }
                        else { 
                        LocationwpEx tempEx = new LocationwpEx(flag++);
                        tempEx.locationwps.Add(temp);
                        cmds.Add(tempEx);
                        }
                        continue;
                    }
                    //读取最后一行数据
                    if (a == (cmdcount-1))
                    {
                        LocationwpEx tempEx = new LocationwpEx(flag++);
                        tempEx.locationwps.Add(temp);
                        cmds.Add(tempEx);
                        break;
                    }
                    //读取中间数据
                    if (temp.id == (ushort)MAVLink.MAV_CMD.WAYPOINT)
                    {
                        
                        LocationwpEx tempEx = new LocationwpEx(flag++);
                        tempEx.WP = temp;
                        cmds.Add(tempEx);
                    }
                    else cmds[flag - 1].locationwps.Add(temp);

                    this.pgBar.Value=a * 100 / cmdcount;

                }

                port.setWPACK();

                this.pgBar.Value = 100;
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.Message);
            }

            WPtoScreen(cmds);
        }

        private void WPtoScreen(List<LocationwpEx> list)
        {
           
            locationwpExs = list;
            if (locationwpExs.Count < 3) return;

            WPIndex = 1;
            int index = WPIndex / 2 + WPIndex % 2;
            this.RouteAltBar.Value = (int)locationwpExs[index].WP.alt;
            this.RouteSpeedBar.Value = (int)locationwpExs[index].locationwps[0].p2;
            this.WayPointAltBar.Value = (int)locationwpExs[index].WP.alt;
            this.WayPointSpeedBar.Value = (int)locationwpExs[index].locationwps[0].p2;            

            mapControl.ZoomAndCenterMarkers("WPMapOverlay");

        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 点击航点信息
        /// </summary>
        /// <param name="gMapMarker"></param>
        public void ClickWP(GMapMarker gMapMarker)
        {
            // if (gMapMarker.Tag.ToString() == "H") return;
            if (gMapMarker != null && gMapMarker.Tag.ToString().Contains("grid"))
            {
                WPIndex = int.Parse(gMapMarker.Tag.ToString().Replace("grid", ""));
                this.txtLng.Text = gMapMarker.Position.Lng.ToString();
                this.txtLon.Text = gMapMarker.Position.Lat.ToString();

                int index = WPIndex / 2 + WPIndex % 2;
                
                if (index!=0&&locationwpExs.Count - 2 >=index)
                {
                    this.btnMissions.Text = (locationwpExs[index].locationwps.Count - 1) + "项任务";

                    this.WayPointAltBar.Value = (int)locationwpExs[index].WP.alt;
                    this.WayPointSpeedBar.Value = (int)locationwpExs[index].locationwps[0].p2;


                    this.btnMissions.Refresh();
                }
               


            }
            else {
                try {
                     int.TryParse(gMapMarker.Tag.ToString(),out WPIndex);
                    if (WPIndex == 0) return;
                    
                    this.txtLng.Text = gMapMarker.Position.Lng.ToString();
                    this.txtLon.Text = gMapMarker.Position.Lat.ToString();

                    int index = (WPIndex/ 2 + WPIndex % 2)-1;
                    WPIndex -= 2;
                    this.btnMissions.Text = (locationwpExs[index].locationwps.Count - 1) + "项任务";
                    this.WayPointAltBar.Value = (int)locationwpExs[index].WP.alt;
                    this.WayPointSpeedBar.Value = (int)locationwpExs[index].locationwps[0].p2;

                    this.btnMissions.Refresh();

                } catch(Exception ex) {


                }

            } 
            

        }
        /// <summary>
        /// 设置Home点
        /// </summary>
        public GMapMarker homeMarker;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapRoute"></param>
        /// <param name="mapMarker"></param>
        public void CalculateTotalInfo(GMapRoute mapRoute,GMapMarker mapMarker)
        {
            this.MapRoute = mapRoute;
            //判断Home点
            if (MainUI.homelayer.Markers.Count != 1)
            {             
                //获取home点
                GMapMarkerWP m = new GMapMarkerWP(getCenterPoint(mapRoute.Points), "H");
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                //m.ToolTipText = "Alt: 0";
                m.Tag = "H";
                MainUI.homelayer.Markers.Clear();
                MainUI.homelayer.Markers.Add(m);
            }
            homeMarker = MainUI.homelayer.Markers[0];

            //根据mapRoute计算总航程和航点数量
            if (mapRoute!=null&& mapRoute.Points.Count>0)
            {

                //连接home点和航线起始点
                //绘制home点到开始点和结束点的连线
                var routeCount = mapRoute.Overlay.Routes.Count;

                if (routeCount == 2)
                {
                    var homeRoute = mapRoute.Overlay.Routes[1];
                    homeRoute.Points[0] = mapRoute.Points[mapRoute.Points.Count - 1];
                    homeRoute.Points[1] = homeMarker.Position;
                    homeRoute.Points[2]= mapRoute.Points[0];

                }
                else {

                    GMapRoute homeroute = new GMapRoute("home route"); 
                    homeroute.Points.Add(mapRoute.Points[mapRoute.Points.Count-1]);
                    homeroute.Points.Add(homeMarker.Position);
                    homeroute.Points.Add(mapRoute.Points[0]);
                    homeroute.Stroke = new Pen(Color.Yellow, 2);
                    homeroute.Stroke.DashStyle = DashStyle.Dash;
                    mapRoute.Overlay.Routes.Add(homeroute);
                }

                if (mapRoute.Overlay.Routes.Count == 1) return;

                MainUI.MainUIInstance.getGMAP.UpdateRouteLocalPosition(mapRoute.Overlay.Routes[1]);

                this.MapRoute = mapRoute;

               this.lblWayPointsNum.Text= mapRoute.Points.Count.ToString();
               this.txtDistance.Text=   (mapRoute.Distance+mapRoute.Overlay.Routes[1].Distance).ToString("0.000");
               
               this.txtEstimatedTime.Text = secondsToNice((mapRoute.Distance *1000/ this.RouteSpeedBar.Value));
                if (mapMarker!=null&&mapMarker.Tag.ToString().Contains("grid"))
                {
                this.txtLng.Text = mapMarker.Position.Lng.ToString();
                this.txtLon.Text = mapMarker.Position.Lat.ToString();
                WPIndex = int.Parse(mapMarker.Tag.ToString().Replace("grid", ""));

                    int index = WPIndex / 2 + WPIndex % 2;
                    locationwpExs[index].WP.lat = mapMarker.Position.Lat;
                    locationwpExs[index].WP.lng = mapMarker.Position.Lng;

                    this.WayPointAltBar.Value = (int)locationwpExs[index].WP.alt;
                    this.WayPointSpeedBar.Value = (int)locationwpExs[index].locationwps[0].p2;

                }

                //获取拍照数量                

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private PointLatLng getCenterPoint(List<PointLatLng> list)
        {
           double[] tempD= CoordConvertHelper.ConvertLngLat2WebMercator(list[0].Lng, list[0].Lat);
           PointLatLng centerPoint = new PointLatLng(tempD[1],tempD[0]);

            for (int index = 1; index < list.Count; index++)
            {
                tempD = CoordConvertHelper.ConvertLngLat2WebMercator(list[index].Lng, list[index].Lat);
                centerPoint.Lat += tempD[1];
                centerPoint.Lng += tempD[0];
            }
            centerPoint.Lat /= list.Count;
            centerPoint.Lng /= list.Count;
            tempD = CoordConvertHelper.ConvertWebMercator2LngLat(centerPoint.Lng, centerPoint.Lat);

            return new PointLatLng(tempD[1],tempD[0]);
        }


        /// <summary>
        /// 将当前节点改为航点
        /// </summary>
        /// <param name="mapRoute"></param>
        /// <param name="mapMarker"></param>
        public void ChangWP(GMapRoute mapRoute,GMapMarker mapMarker)
        {

            //当前节点修改序号，改变样式
            int index = int.Parse(mapMarker.Tag.ToString().Replace("selected", ""));

            var markers = mapRoute.Overlay.Markers;

            PointLatLng prePoint = markers[index-2].Position;
            PointLatLng curPoint = markers[index-1].Position;
           
            markers.Insert(index-1, new GMarkerGoogle(getCenterPoint(new List<PointLatLng>() { prePoint,curPoint}), GMarkerGoogleType.green) {
               
                ToolTipMode=MarkerTooltipMode.Never
            });
            PointLatLng nextPoint = markers[index+1].Position;

            markers.Insert(index+1, new GMarkerGoogle(getCenterPoint(new List<PointLatLng>() { curPoint,nextPoint}), GMarkerGoogleType.green)
            {
                
                ToolTipMode = MarkerTooltipMode.Never
            });

            string tag = "grid" +index;

            markers[index] = new GMarkerGoogle(mapMarker.Position, GMarkerGoogleType.red)
            {
                Tag = tag,
                ToolTipText = tag,
                ToolTipMode = MarkerTooltipMode.Never
            };

            mapMarker = markers[index];

            //marker重新编号
            for (int curIndex=0;curIndex<markers.Count;curIndex++) {

                int result = (curIndex + 1) % 2;
                if (result == 1) {
                    markers[curIndex].Tag = "grid" + (curIndex + 1);
                    markers[curIndex].ToolTipText= "grid" + (curIndex + 1);
                } else if (result == 0) {
                    markers[curIndex].Tag = "changed" + (curIndex + 1);
                    markers[curIndex].ToolTipText = "changed" + (curIndex + 1);
                }
            }

            mapRoute.Points.Insert(index/2,curPoint);


            //当前的WP列表节点进行修改
            InsertLWEx(index/2+1,curPoint);


        }
        /// <summary>
        /// 在对应的位置添加航点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        private void InsertLWEx(int index,PointLatLng point)
        {

            LocationwpEx locationwpEx = new LocationwpEx(index);

            //添加航点
            Locationwp locationwp = new Locationwp();
            locationwp.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            locationwp.p1 = 0.0f;
            locationwp.alt =
                (float)
                    (this.RouteAltBar.Value / CurrentState.multiplieralt);
            locationwp.lat = point.Lat;
            locationwp.lng = point.Lng;

            locationwp.p2 = 0.0f;
            locationwp.p3 = 0.0f;
            locationwp.p4 = 0.0f;

            locationwpEx.WP = locationwp;

            //改变速度
            locationwpEx.locationwps.Add(new Locationwp()
            {
                id = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED,
                p1 = 0,
                p2 = this.RouteSpeedBar.Value

            });

            //将该点航点命令加入队列
            locationwpExs.Insert(index,locationwpEx);

            //更新之后的的序列
            for (int curIndex=index;curIndex<locationwpExs.Count;curIndex++)
            {
                locationwpExs[curIndex].index = curIndex;

            }


        }


        /// <summary>
        /// 设置航点信息
        /// </summary>
        /// <param name="gMapRoute"></param>
        /// <param name="gMapMarker"></param>
        public void SetWayPoints(GMapRoute gMapRoute,GMapMarker gMapMarker)
        {
            //1 初始化航点列表
            if (locationwpExs == null) locationwpExs = new List<LocationwpEx>();
            else locationwpExs.Clear();

         

            int index = 0;

            //2 添加自动起飞
            LocationwpEx startPoint =new LocationwpEx(index++);

            startPoint.locationwps.Add(new Locationwp()
            {
                id = (ushort)MAVLink.MAV_CMD.TAKEOFF,
                p1 = 20,
                p2 = 0,
                p3 = 0,
                p4 = 0,
                lng = 0,
                lat = 0,
                alt = 30
            });

            //改变速度
            startPoint.locationwps.Add(new Locationwp()
            {
                id = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED,
                p1 = 0,
                p2 = this.RouteSpeedBar.Value
            });            

            locationwpExs.Add(startPoint);
            //3 添加航点
            foreach (PointLatLngAlt point in gMapRoute.Points)
            {
                LocationwpEx locationwpEx = new LocationwpEx(index++);
                
                //添加航点
                Locationwp locationwp = new Locationwp();
                locationwp.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                locationwp.p1 = 0.0f;
                locationwp.alt =
                    (float)
                        (this.RouteAltBar.Value / CurrentState.multiplieralt);
                locationwp.lat =point.Lat;
                locationwp.lng = point.Lng;

                locationwp.p2 = 0.0f;
                locationwp.p3 = 0.0f;
                locationwp.p4 = 0.0f;
              
                locationwpEx.WP=locationwp;

                //改变速度
                locationwpEx.locationwps.Add(new Locationwp()
                {
                    id=(ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED,
                    p1=0,
                    p2=this.RouteSpeedBar.Value

                });
               
                //将该点航点命令加入队列
                locationwpExs.Add(locationwpEx);
            }

            WPIndex = mapRoute.Points.Count * 2;

            if (mapRoute.Points.Count % 2 == 1)
            {
                WPIndex -= 1;
            }


            //结束后的航点
            LocationwpEx endPoint =new LocationwpEx(index++);

            ushort mavCMD=(ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;

            switch (AfterMissionCommands.SelectedIndex)
            {
                case 0:
                    mavCMD= (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;
                    break;
                case 1:
                    mavCMD= (ushort)MAVLink.MAV_CMD.LOITER_UNLIM;
                    break;
                case 2:
                    mavCMD= (ushort)MAVLink.MAV_CMD.LAND;
                    break;
                default:
                    mavCMD=(ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;
                    break;

            }
            //mavCMD = (ushort)MAVLink.MAV_CMD.LAND;
            endPoint.locationwps.Add(new Locationwp()
            {
                id = mavCMD
            });
            locationwpExs.Add(endPoint);
        }

        #endregion

        private void btnDeleteWayPoint_Click(object sender, EventArgs e)
        {

            if (WPIndex == 0) return;

            var lyrs = this.mapControl.Overlays.Where(a => a.Id == "WPMapOverlay").ToList();

            if (lyrs[0].Markers.Count < WPIndex) { MessageBox.Show("请先选择要删除的点。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information); return; };

            //1.移除对应航点记录            
            int index = WPIndex / 2 + WPIndex % 2;            
            this.locationwpExs.RemoveAt(index-1);
            for (int cIndex=index-1; cIndex < locationwpExs.Count;cIndex++)
            {
                locationwpExs[cIndex].index = cIndex;
            }

            //2. 移除相应图形航点          

            if (lyrs != null && lyrs.Count == 1&&lyrs[0].Routes.Count==2)
            {

                
                var wproute = lyrs[0].Routes[0];//航线

                var wpMarkers = lyrs[0].Markers;//标记点

                var homeRoute = lyrs[0].Routes[1];//home

                if (wpMarkers.Count == 1)
                {                   
                    wpMarkers.Clear();
                    lyrs[0].Routes.Clear() ;
                    return;
                }


                //移除航线上的点
                wproute.Points.RemoveAt(index-1);

                //移除航点
                if (WPIndex == 1)
                {
                    homeRoute.Points[2] = wproute.Points[0];
                    wpMarkers.RemoveAt(0);
                    wpMarkers.RemoveAt(0);
                }
                else if (WPIndex == wpMarkers.Count)
                {
                    homeRoute.Points[0] = wproute.Points[wproute.Points.Count - 1];
                    wpMarkers.RemoveAt(wpMarkers.Count - 1);
                    wpMarkers.RemoveAt(wpMarkers.Count - 1);
                }
                else {
                    
                    wpMarkers.RemoveAt(WPIndex - 1);
                    wpMarkers.RemoveAt(WPIndex-1);                  
                    wpMarkers[WPIndex - 2].Position = getCenterPoint(new List<PointLatLng>() { wpMarkers[WPIndex - 3].Position, wpMarkers[WPIndex - 1].Position });
                }
                //

                if (wpMarkers.Count == 1)
                    wpMarkers[0].Tag = "grid1";
                else
                {
                    for (int tempIndex = 0; tempIndex < wpMarkers.Count; tempIndex++)
                    {
                        if (tempIndex % 2 == 0)
                        {
                            wpMarkers[tempIndex].Tag = "grid" + (tempIndex + 1);
                            wpMarkers[tempIndex].ToolTipText = (tempIndex + 1).ToString();
                        }
                        else {
                            wpMarkers[tempIndex].Tag = "changed" + (tempIndex + 1);
                            wpMarkers[tempIndex].ToolTipText = (tempIndex + 1).ToString();
                        }
                        
                    }
                }

                WPIndex = wpMarkers.Count;

                mapControl.UpdateRouteLocalPosition(wproute);
                mapControl.UpdateRouteLocalPosition(homeRoute);
                mapControl.Invalidate(true);


                //lyrs[0].Routes[0].Points.RemoveAt(index);



                //点号重新排序

            }


            //3.更新航线信息
        }

        private void RouteDescSpeed_Click(object sender, EventArgs e)
        {
            if (this.RouteSpeedBar.Value>this.RouteSpeedBar.Minimum)
            {
                this.RouteSpeedBar.Value -= 1;
            
            }
        }

        private void RouteAscSpeed_Click(object sender, EventArgs e)
        {
            if (this.RouteSpeedBar.Value < this.RouteSpeedBar.Maximum)
            {
                this.RouteSpeedBar.Value += 1;
            }
        }

        private void RouteAscAlt_Click(object sender, EventArgs e)
        {
            if (this.RouteAltBar.Value < this.RouteAltBar.Maximum) {

                this.RouteAltBar.Value += 1;
            }
        }

        private void RouteDescAlt_Click(object sender, EventArgs e)
        {
            if (this.RouteAltBar.Value > this.RouteAltBar.Minimum) {

                this.RouteAltBar.Value -= 1;
            }
        }

        private void WayPointAscSpeed_Click(object sender, EventArgs e)
        {
            if (this.WayPointSpeedBar.Value < this.WayPointSpeedBar.Maximum)
            {
                this.WayPointSpeedBar.Value += 1;
            }
        }

        private void WayPointDescSpeed_Click(object sender, EventArgs e)
        {
            if (this.WayPointSpeedBar.Value>this.WayPointSpeedBar.Minimum)
            {
                this.WayPointSpeedBar.Value -= 1;
            }
        }

        private void WayPointAscAlt_Click(object sender, EventArgs e)
        {
            if (this.WayPointAltBar.Value < this.WayPointAltBar.Maximum)
            {
                this.WayPointAltBar.Value += 1;
            }
        }

        private void WayPointDescAlt_Click(object sender, EventArgs e)
        {
            if (this.WayPointAltBar.Value > this.WayPointAltBar.Minimum)
            {
                this.WayPointAltBar.Value -= 1;
            }
        }

        private void AfterMissionCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            ushort mavCMD = (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;

            switch (AfterMissionCommands.SelectedIndex)
            {
                case 0:
                    mavCMD = (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;
                    break;
                case 1:
                    mavCMD = (ushort)MAVLink.MAV_CMD.LAND;
                    break;
                case 2:
                    mavCMD = (ushort)MAVLink.MAV_CMD.LOITER_UNLIM;
                    break;
                default:
                    mavCMD = (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;
                    break;

            }

            if (this.locationwpExs.Count > 2) {

                this.locationwpExs[this.locationwpExs.Count-1].locationwps[0]=new Locationwp() { id= mavCMD } ;
            }

        }
    }

}
