using GeoUtility.GeoSystem;
using GMap.NET.WindowsForms;
using MetroSet_UI.Controls;
using MissionPlanner;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Controls;
using SKYROVER.GCS.DeskTop.Grids;
using SKYROVER.GCS.DeskTop.MessagePanel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SKYROVER.GCS.DeskTop.MenuItems
{

    public delegate void AfterTripParamsChange(GMapPolygon gMapPolygon, TripModel tripModel,DisplayInfos displayInfos);
   /// <summary>
   /// 
   /// </summary>
    public partial class SurveyGrid : BaseForm
    {

        public event AfterTripParamsChange AfterTripParamsChangeEvent;      

        private TripModel tripModel;

        private DisplayInfos displayInfos;

        cameraInfo camera;
        Dictionary<string, cameraInfo> cameras = new Dictionary<string, cameraInfo>();
        /// <summary>
        /// 
        /// </summary>
        GMapPolygon mapPolygon;

        myGMAP mapControl;

        /// <summary>
        /// 
        /// </summary>
        public GMapPolygon MapPolygon
        {
            get => mapPolygon; 
            set {
                mapPolygon = value;
                if (mapPolygon != null)
                {
                    List<PointLatLngAlt> list = new List<PointLatLngAlt>();
                    mapPolygon.Points.ForEach(x => { list.Add(x); });
                    headingBar.Value = (int)((getAngleOfLongestSide(list) + 360) % 360);
                    TripModel.Heading = headingBar.Value;
                    lblHeading.Text = headingBar.Value.ToString();
                }
                else headingBar.Value = 0;
            }
        }

        public MyDataGridView getDataGridView { get { return this.Commands; } }

        public myGMAP MapControl { get => mapControl; set => mapControl = value; }
        public TripModel TripModel { get => tripModel; set => tripModel = value; }
        public DisplayInfos DisplayInfos { get => displayInfos; set => displayInfos = value; }

        public string DistUnits = "";

        public SurveyGrid()
        {
            InitializeComponent();
            InitBaseSet();
        }

        //初始设置
        private void InitBaseSet()
        {
            //初始化相机
            xmlcamera(false, Settings.GetRunningDirectory() + "camerasBuiltin.xml");
          
            //开始位置
            CMB_startfrom.DataSource = Enum.GetNames(typeof(Grid.StartPosition));
            CMB_startfrom.SelectedIndex = 0;

            loadsettings();
            //

                  
          


        }

        double getAngleOfLongestSide(List<PointLatLngAlt> list)
        {
            if (list.Count == 0)
                return 0;
            double angle = 0;
            double maxdist = 0;
            PointLatLngAlt last = list[list.Count - 1];
            foreach (PointLatLngAlt item in list)
            {
                if (item.GetDistance(last) > maxdist)
                {
                    angle = item.GetBearing(last);
                    maxdist = item.GetDistance(last);
                }
                last = item;
            }

            return (angle + 360) % 360;
        }


        /// <summary>
        /// 获取航线参数
        /// </summary>
        private void getTripValue() {

            this.lblAltitude.Text = altitudeBar.Value.ToString();
            this.lblHeading.Text = headingBar.Value.ToString();
            this.lblOverlap.Text = overlapBar.Value.ToString();
            this.lblResolution.Text = resolutionBar.Value.ToString();
            lblSidelap.Text = sidelapBar.Value.ToString();
            lblSpeed.Text = speedBar.Value.ToString();

            tripModel = new TripModel();
            tripModel.Altitude = altitudeBar.Value;
            tripModel.Heading = headingBar.Value;
            tripModel.Overlap = overlapBar.Value;
            tripModel.Sidelap = sidelapBar.Value;
            tripModel.Resoluton = resolutionBar.Value;
            tripModel.Speed = speedBar.Value;
            tripModel.StartFrom = CMB_startfrom.Text;

            if (cmbCameras.Text != "")
            {
                camera = cameras[cmbCameras.Text];
                tripModel.Camera = camera;
            }
           

         
           
            tripModel.TripType = TripType.Normal;

        }
        /// <summary>
        /// 获取航线显示内容
        /// </summary>
        private void getDisplayValue()
        {
            displayInfos = new DisplayInfos();

            //displayInfos.boundary = CHK_boundary.Checked;
            displayInfos.markers = CHK_markers.Checked;
            displayInfos.grid = CHK_grid.Checked;
            displayInfos.Internals = CHK_internals.Checked;
            displayInfos.footPrints = CHK_footprints.Checked;

        }


        /// <summary>
        /// 
        /// </summary>
        private void setTripValue() {


        }
        /// <summary>
        /// 获取显示样式信息
        /// </summary>
        private void getDisplayInofs() { }
        /// <summary>
        /// 
        /// </summary>
        private void setDisplayInfos() { }

              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="write"></param>
        /// <param name="filename"></param>
        private void xmlcamera(bool write, string filename)
        {
            bool exists = File.Exists(filename);

            if (write || !exists)
            {
                try
                {
                    XmlTextWriter xmlwriter = new XmlTextWriter(filename, Encoding.ASCII);
                    xmlwriter.Formatting = Formatting.Indented;

                    xmlwriter.WriteStartDocument();

                    xmlwriter.WriteStartElement("Cameras");

                    foreach (string key in cameras.Keys)
                    {
                        try
                        {
                            if (key == "")
                                continue;
                            xmlwriter.WriteStartElement("Camera");
                            xmlwriter.WriteElementString("name", cameras[key].name);
                            xmlwriter.WriteElementString("flen", cameras[key].focallen.ToString(new System.Globalization.CultureInfo("en-US")));
                            xmlwriter.WriteElementString("imgh", cameras[key].imageheight.ToString(new System.Globalization.CultureInfo("en-US")));
                            xmlwriter.WriteElementString("imgw", cameras[key].imagewidth.ToString(new System.Globalization.CultureInfo("en-US")));
                            xmlwriter.WriteElementString("senh", cameras[key].sensorheight.ToString(new System.Globalization.CultureInfo("en-US")));
                            xmlwriter.WriteElementString("senw", cameras[key].sensorwidth.ToString(new System.Globalization.CultureInfo("en-US")));
                            xmlwriter.WriteEndElement();
                        }
                        catch { }
                    }

                    xmlwriter.WriteEndElement();

                    xmlwriter.WriteEndDocument();
                    xmlwriter.Close();

                }
                catch (Exception ex) { CustomMessageBox.Show(ex.ToString()); }
            }
            else
            {
                try
                {
                    using (XmlTextReader xmlreader = new XmlTextReader(filename))
                    {
                        while (xmlreader.Read())
                        {
                            xmlreader.MoveToElement();
                            try
                            {
                                switch (xmlreader.Name)
                                {
                                    case "Camera":
                                        {
                                            cameraInfo camera = new cameraInfo();

                                            while (xmlreader.Read())
                                            {
                                                bool dobreak = false;
                                                xmlreader.MoveToElement();
                                                switch (xmlreader.Name)
                                                {
                                                    case "name":
                                                        camera.name = xmlreader.ReadString();
                                                        break;
                                                    case "imgw":
                                                        camera.imagewidth = int.Parse(xmlreader.ReadString(), new System.Globalization.CultureInfo("en-US"));
                                                        break;
                                                    case "imgh":
                                                        camera.imageheight = int.Parse(xmlreader.ReadString(), new System.Globalization.CultureInfo("en-US"));
                                                        break;
                                                    case "senw":
                                                        camera.sensorwidth = float.Parse(xmlreader.ReadString(), new System.Globalization.CultureInfo("en-US"));
                                                        break;
                                                    case "senh":
                                                        camera.sensorheight = float.Parse(xmlreader.ReadString(), new System.Globalization.CultureInfo("en-US"));
                                                        break;
                                                    case "flen":
                                                        camera.focallen = float.Parse(xmlreader.ReadString(), new System.Globalization.CultureInfo("en-US"));
                                                        break;
                                                    case "Camera":
                                                        cameras[camera.name] = camera;
                                                        dobreak = true;
                                                        break;
                                                }
                                                if (dobreak)
                                                    break;
                                            }
                                            string temp = xmlreader.ReadString();
                                        }
                                        break;
                                    case "Config":
                                        break;
                                    case "xml":
                                        break;
                                    default:
                                        if (xmlreader.Name == "") // line feeds
                                            break;
                                        //config[xmlreader.Name] = xmlreader.ReadString();
                                        break;
                                }
                            }
                            catch (Exception ee) { Console.WriteLine(ee.Message); } // silent fail on bad entry
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine("Bad Camera File: " + ex.ToString()); } // bad config file

                // populate list
                foreach (var camera in cameras.Values)
                {
                    if (!cmbCameras.Items.Contains(camera.name))
                        cmbCameras.Items.Add(camera.name);
                }
                                
                    
            }
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        private void savesettings()
        {
            Settings.config["grid_camera"] = cmbCameras.Text;
            Settings.config["grid_alt"] =  altitudeBar.Value.ToString();
            Settings.config["grid_overlap"] = overlapBar.Value.ToString();
            Settings.config["grid_sidelap"] = sidelapBar.Value.ToString();
            Settings.config["grid_startfrom"] = CMB_startfrom.Text;
            Settings.config["grid_internals"] = CHK_internals.Checked.ToString();
            Settings.config["grid_footprints"] = CHK_footprints.Checked.ToString();
            Settings.config["grid_angle"] = headingBar.Value.ToString();

            Settings.Instance.Save();
            //  Settings.config["grid_camdir"] = CHK_camdirection.Checked.ToString();

            //  Settings.config["grid_usespeed"] = CHK_usespeed.Checked.ToString();

            //  Settings.config["grid_dist"] = NUM_Distance.Value.ToString();
            //Settings.config["grid_overshoot1"] = NUM_overshoot.Value.ToString();
            //Settings.config["grid_overshoot2"] = NUM_overshoot2.Value.ToString();
            //Settings.config["grid_leadin"] = NUM_leadin.Value.ToString();

            //Settings.config["grid_spacing"] = NUM_spacing.Value.ToString();
            //Settings.config["grid_crossgrid"] = chk_crossgrid.Checked.ToString();



            //Settings.config["grid_autotakeoff"] = CHK_toandland.Checked.ToString();
            //Settings.config["grid_autotakeoff_RTL"] = CHK_toandland_RTL.Checked.ToString();


            //Settings.config["grid_advanced"] = CHK_advanced.Checked.ToString();

            //Settings.config["grid_trigdist"] = rad_trigdist.Checked.ToString();
            //Settings.config["grid_digicam"] = rad_digicam.Checked.ToString();
            //Settings.config["grid_repeatservo"] = rad_repeatservo.Checked.ToString();
            //Settings.config["grid_breakstopstart"] = chk_stopstart.Checked.ToString();

            //// Copter Settings
            //Settings.config["grid_copter_delay"] = NUM_copter_delay.Value.ToString();
            //Settings.config["grid_copter_headinghold_chk"] = CHK_copter_headinghold.Checked.ToString();

            //// Plane Settings
            //Settings.config["grid_min_lane_separation"] = NUM_Lane_Dist.Value.ToString();
        }

        /// <summary>
        /// 保存航线信息
        /// </summary>
        /// <returns></returns>
        GridData savegriddata()
        {
            GridData griddata = new GridData();
            //mapPolygon.Points.ForEach( x => { griddata.poly.Add(x); });
            
            //griddata.camera = cmbCameras.Text;
            //griddata.alt = altitudeBar.Value;
            //griddata.angle = headingBar.Value;
            //griddata.camdir = CHK_camdirection.Checked;
            //griddata.speed = speedBar.Value;
            //griddata.usespeed = CHK_usespeed.Checked;
            //griddata.autotakeoff = CHK_toandland.Checked;
            //griddata.autotakeoff_RTL = CHK_toandland_RTL.Checked;
            //griddata.splitmission = NUM_split.Value;

            //griddata.internals = CHK_internals.Checked;
            //griddata.footprints = CHK_footprints.Checked;
            //griddata.advanced = CHK_advanced.Checked;

            //griddata.dist = NUM_Distance.Value;
            //griddata.overshoot1 = NUM_overshoot.Value;
            //griddata.overshoot2 = NUM_overshoot2.Value;
            //griddata.leadin = NUM_leadin.Value;
            //griddata.startfrom = CMB_startfrom.Text;
            //griddata.overlap = num_overlap.Value;
            //griddata.sidelap = num_sidelap.Value;
            //griddata.spacing = NUM_spacing.Value;
            //griddata.crossgrid = chk_crossgrid.Checked;

            //// Copter Settings
            //griddata.copter_delay = NUM_copter_delay.Value;
            //griddata.copter_headinghold_chk = CHK_copter_headinghold.Checked;
            //griddata.copter_headinghold = decimal.Parse(TXT_headinghold.Text);

            //// Plane Settings
            //griddata.minlaneseparation = NUM_Lane_Dist.Value;

            //griddata.trigdist = rad_trigdist.Checked;
            //griddata.digicam = rad_digicam.Checked;
            //griddata.repeatservo = rad_repeatservo.Checked;
            //griddata.breaktrigdist = chk_stopstart.Checked;

            //griddata.repeatservo_no = NUM_reptservo.Value;
            //griddata.repeatservo_pwm = num_reptpwm.Value;
            //griddata.repeatservo_cycle = NUM_repttime.Value;

            //griddata.setservo_no = num_setservono.Value;
            //griddata.setservo_low = num_setservolow.Value;
            //griddata.setservo_high = num_setservohigh.Value;

            return griddata;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        void loadsettings()
        {
            if (Settings.config.ContainsKey("grid_camera"))
            {
                // camera last to it invokes a reload
                loadsetting("grid_camera", cmbCameras);
                loadsetting("grid_alt",altitudeBar);

                loadsetting("grid_speed", speedBar);


                loadsetting("grid_overlap", overlapBar);
                loadsetting("grid_sidelap", sidelapBar);

                loadsetting("grid_startfrom", CMB_startfrom);
                // loadsetting("grid_angle", headingBar);

                // loadsetting("grid_camdir", CHK_camdirection);
                //  loadsetting("grid_usespeed", CHK_usespeed);
              
                //loadsetting("grid_autotakeoff", CHK_toandland);
                //loadsetting("grid_autotakeoff_RTL", CHK_toandland_RTL);

                //loadsetting("grid_dist", NUM_Distance);
                //loadsetting("grid_overshoot1",this.overlapBar);
                //loadsetting("grid_overshoot2",this.sidelapBar);
                //loadsetting("grid_leadin", NUM_leadin);
               
               
               // loadsetting("grid_spacing", NUM_spacing);
                //loadsetting("grid_crossgrid", chk_crossgrid);

                // Should probably be saved as one setting, and us logic
                //loadsetting("grid_trigdist", rad_trigdist);
                //loadsetting("grid_digicam", rad_digicam);
                //loadsetting("grid_repeatservo", rad_repeatservo);
                //loadsetting("grid_breakstopstart", chk_stopstart);

                //loadsetting("grid_repeatservo_no", NUM_reptservo);
                //loadsetting("grid_repeatservo_pwm", num_reptpwm);
                //loadsetting("grid_repeatservo_cycle", NUM_repttime);

              

                // Copter Settings
                //loadsetting("grid_copter_delay", NUM_copter_delay);
                //loadsetting("grid_copter_headinghold_chk", CHK_copter_headinghold);

                // Plane Settings
                //loadsetting("grid_min_lane_separation", NUM_Lane_Dist);

                loadsetting("grid_internals", CHK_internals);
                loadsetting("grid_footprints", CHK_footprints);

                //loadsetting("grid_advanced", CHK_advanced);
            }
        }

        void loadsetting(string key, Control item)
        {
            // soft fail on bad param
            try
            {
                if (Settings.config.ContainsKey(key))
                {
                    if (item is NumericUpDown)
                    {
                        ((NumericUpDown)item).Value = decimal.Parse(Settings.config[key].ToString());
                    }
                    else if (item is ComboBox)
                    {
                        ((ComboBox)item).Text = Settings.config[key].ToString();
                    }
                    else if (item is CheckBox)
                    {
                        ((CheckBox)item).Checked = bool.Parse(Settings.config[key].ToString());
                    }
                    else if (item is RadioButton)
                    {
                        ((RadioButton)item).Checked = bool.Parse(Settings.config[key].ToString());
                    }
                    else if (item is MetroSetTrackBar) {

                        ((MetroSetTrackBar)item).Value = int.Parse(Settings.config[key].ToString());
                    }
                }
            }
            catch { }
        }


        /// <summary>
        /// 上传航线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnConfirm_Click(object sender, EventArgs e)
        {
            //1 检查home点
            Locationwp home = new Locationwp();
            try
            {
                home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                home.lat =MainUI.comPort.MAV.cs.HomeLocation.Lat;
                home.lng = MainUI.comPort.MAV.cs.HomeLocation.Lng;
                home.alt = (float)(MainUI.comPort.MAV.cs.HomeLocation.Alt / CurrentState.multiplierdist); // use saved home
            }
            catch
            {
                CustomMessageBox.Show("Home点无效", Strings.ERROR);
                return;
            }

            //2 检查grid数据有效性
            for (int a = 0; a < Commands.Rows.Count - 0; a++)
            {
                for (int b = 1; b < Commands.ColumnCount - 0; b++)
                {
                    double answer;
                    if (b >= 2 && b <= 8)
                    {
                        if (!double.TryParse(Commands[b, a].Value.ToString(), out answer))
                        {
                            CustomMessageBox.Show("任务信息有错误");
                            return;
                        }
                    }

                    //if (TXT_altwarn.Text == "")
                    //    TXT_altwarn.Text = (0).ToString();

                    if (Commands.Rows[a].Cells[Command.Index].Value.ToString().Contains("UNKNOWN"))
                        continue;

                    ushort cmd =
                        (ushort)
                                Enum.Parse(typeof(MAVLink.MAV_CMD),
                                    Commands.Rows[a].Cells[Command.Index].Value.ToString(), false);

                    if (cmd < (ushort)MAVLink.MAV_CMD.LAST && double.Parse(Commands[Alt.Index, a].Value.ToString()) < 0)
                    {
                        if (cmd != (ushort)MAVLink.MAV_CMD.TAKEOFF &&
                            cmd != (ushort)MAVLink.MAV_CMD.LAND &&
                            cmd != (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH)
                        {
                            CustomMessageBox.Show("Low alt on WP#" + (a + 1) +
                                                  "\nPlease reduce the alt warning, or increase the altitude");
                            return;
                        }
                    }
                }
            }

            IProgressReporterDialogue frmProgressReporter = new ProgressReporterDialogue
            {
                StartPosition = FormStartPosition.CenterScreen,
                Text = "Sending WP's"
            };
            //3 上传航点数据
            frmProgressReporter.DoWork += saveWPs;
            frmProgressReporter.UpdateProgressAndStatus(-1, "Sending WP's");

          

            //4 保存航线参数信息
            savesettings();


                     
            frmProgressReporter.RunBackgroundOperationAsync();

            frmProgressReporter.Dispose();

             mapControl.Focus();
        }

        void saveWPs(IProgressReporterDialogue sender)
        {
            try
            {
                MAVLinkInterface port = MainUI.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("请先连接飞控!");
                }

                MainUI.comPort.giveComport = true;
                int a = 0;

                // 定义home点
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
                    throw new Exception("Your home location is invalid");
                }

                // log
                //log.Info("wps values " + MainUI.comPort.MAV.wps.Values.Count);
                //log.Info("cmd rows " + (Commands.Rows.Count + 1)); // + home

                // check for changes / future mod to send just changed wp's
                if (MainUI.comPort.MAV.wps.Values.Count == (Commands.Rows.Count + 1))
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

                        MAVLink.mavlink_mission_item_t temp = DataViewtoLocationwp(a);

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
                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(0, "Set total wps ");

                ushort totalwpcountforupload = (ushort)(Commands.Rows.Count + 1);

                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    totalwpcountforupload--;
                }

                port.setWPTotal(totalwpcountforupload); // + home

                // set home location - overwritten/ignored depending on firmware.
                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(0, "Set home");

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
                }
                else
                {
                    use_int = false;
                }

                // define the default frame.
                MAVLink.MAV_FRAME frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;

                // get the command list from the datagrid
                var commandlist = GetCommandList();

                // process commandlist to the mav
                for (a = 1; a <= commandlist.Count; a++)
                {
                    var temp = commandlist[a - 1];

                    ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(a * 100 / Commands.Rows.Count,
                        "Setting WP " + a);

                    // make sure we are using the correct frame for these commands
                    if (temp.id < (ushort)MAVLink.MAV_CMD.LAST || temp.id == (ushort)MAVLink.MAV_CMD.DO_SET_HOME)
                    {
                        //var mode = currentaltmode;

                        //if (mode == altmode.Terrain)
                        //{
                        //    frame = MAVLink.MAV_FRAME.GLOBAL_TERRAIN_ALT;
                        //}
                        //else if (mode == altmode.Absolute)
                        //{
                        //    frame = MAVLink.MAV_FRAME.GLOBAL;
                        //}
                        //else
                        //{
                        //    frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;
                        //}

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
                        sender.doWorkArgs.ErrorMessage = "Upload failed, please reduce the number of wp's";
                        return;
                    }
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID)
                    {
                        sender.doWorkArgs.ErrorMessage =
                            "Upload failed, mission was rejected byt the Mav,\n item had a bad option wp# " + a + " " +
                            ans;
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
                        sender.doWorkArgs.ErrorMessage = "Upload wps failed " + Enum.Parse(typeof(MAVLink.MAV_CMD), temp.id.ToString()) +
                                         " " + Enum.Parse(typeof(MAVLink.MAV_MISSION_RESULT), ans.ToString());
                        return;
                    }
                }

                port.setWPACK();

                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(95, "Setting params");




                //// m
                //port.setParam("WP_RADIUS", float.Parse(TXT_WPRad.Text) / CurrentState.multiplierdist);

                //// cm's
                //port.setParam("WPNAV_RADIUS", float.Parse(TXT_WPRad.Text) / CurrentState.multiplierdist * 100.0);

                // m
                port.setParam("WP_RADIUS", 5.0 / CurrentState.multiplierdist);

                // cm's
                port.setParam("WPNAV_RADIUS", 5.0 / CurrentState.multiplierdist * 100.0);


                port.MAV.wps.Clear();

                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(100, "Done.");
            }
            catch (Exception ex)
            {
               // log.Error(ex);
                MainUI.comPort.giveComport = false;
                throw;
            }


            
            MainUI.comPort.giveComport = false;
        }


       public List<Locationwp> GetCommandList()
        {
            List<Locationwp> commands = new List<Locationwp>();

            for (int a = 0; a < Commands.Rows.Count - 0; a++)
            {
                var temp = DataViewtoLocationwp(a);

                commands.Add(temp);
            }

            return commands;
        }

        Locationwp DataViewtoLocationwp(int a)
        {
            try
            {
                Locationwp temp = new Locationwp();
                if (Commands.Rows[a].Cells[Command.Index].Value.ToString().Contains("UNKNOWN"))
                {
                    temp.id = (ushort)Commands.Rows[a].Cells[Command.Index].Tag;
                }
                else
                {
                    temp.id =
                        (ushort)
                                Enum.Parse(typeof(MAVLink.MAV_CMD),
                                    Commands.Rows[a].Cells[Command.Index].Value.ToString(),
                                    false);
                }
                temp.p1 = float.Parse(Commands.Rows[a].Cells[Param1.Index].Value.ToString());

                temp.alt =
                    (float)
                        (double.Parse(Commands.Rows[a].Cells[Alt.Index].Value.ToString()) / CurrentState.multiplieralt);
                temp.lat = (double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString()));
                temp.lng = (double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString()));

                temp.p2 = (float)(double.Parse(Commands.Rows[a].Cells[Param2.Index].Value.ToString()));
                temp.p3 = (float)(double.Parse(Commands.Rows[a].Cells[Param3.Index].Value.ToString()));
                temp.p4 = (float)(double.Parse(Commands.Rows[a].Cells[Param4.Index].Value.ToString()));

                temp.Tag = Commands.Rows[a].Cells[TagData.Index].Value;

                return temp;
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid number on row " + (a + 1).ToString(), ex);
            }
        }

        /// <summary>
        /// 下载航线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            //1 检测home点是否存在
            if (MainUI.comPort.MAV.cs.HomeLocation.Lat == 0 && MainUI.comPort.MAV.cs.HomeLocation.Lng == 0) return;

            //2 清除已有航线数据
            //
            if (MainUI.comPort.MAV.wps != null && MainUI.comPort.MAV.wps.Count > 0) MainUI.comPort.MAV.wps.Clear();

            if (Commands.Rows.Count > 0)
            {
                Commands.Rows.Clear();
            }

            //清除已存在的航线
            foreach (var lyr in mapControl.Overlays)
            {
                if (lyr.Id == "polygons" || lyr.Id == "WPOverlay" || lyr.Id == "surveyGridlayer"||lyr.Id== "WPMapOverlay")
                {

                    lyr.Polygons.Clear();
                    lyr.Markers.Clear();
                    lyr.Routes.Clear();
                }
            }

            //
            IProgressReporterDialogue frmProgressReporter = new ProgressReporterDialogue
            {
                StartPosition = FormStartPosition.CenterScreen,
                Text = "Receiving WP's"
            };

            frmProgressReporter.DoWork += getWPs;
            frmProgressReporter.UpdateProgressAndStatus(-1, "Receiving WP's");

           //ThemeManager.ApplyThemeTo(frmProgressReporter);

            frmProgressReporter.RunBackgroundOperationAsync();

            frmProgressReporter.Dispose();
        }

        void getWPs(IProgressReporterDialogue sender)
        {
            List<Locationwp> cmds = new List<Locationwp>();

            try
            {
                MAVLinkInterface port = MainUI.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("Please Connect First!");
                }

                MainUI.comPort.giveComport = true;

                //log.Info("Getting Home");

                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(0, "Getting WP count");

                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    try
                    {
                        cmds.Add(port.getHomePosition());
                    }
                    catch (TimeoutException)
                    {
                        // blank home
                        cmds.Add(new Locationwp() { id = (ushort)MAVLink.MAV_CMD.WAYPOINT });
                    }
                }

               // log.Info("Getting WP #");

                int cmdcount = port.getWPCount();

                for (ushort a = 0; a < cmdcount; a++)
                {
                    if (((ProgressReporterDialogue)sender).doWorkArgs.CancelRequested)
                    {
                        ((ProgressReporterDialogue)sender).doWorkArgs.CancelAcknowledged = true;
                        throw new Exception("Cancel Requested");
                    }

                   // log.Info("Getting WP" + a);
                    ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(a * 100 / cmdcount, "Getting WP " + a);
                    cmds.Add(port.getWP(a));
                }

                port.setWPACK();

                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(100, "Done");

               // log.Info("Done");
            }
            catch
            {
                throw;
            }

            WPtoScreen(cmds);
        }

        private void WPtoScreen(List<Locationwp> cmds, bool withrally = true)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        //log.Info("Process " + cmds.Count);
                        processToScreen(cmds);
                    }
                    catch (Exception ex)
                    {
                       // log.Info(exx.ToString());
                    }

                    MainUI.comPort.giveComport = false;

                   
                });
            }
            catch (Exception ex)
            {
                //log.Info(exx.ToString());
            }
        }

        int selectedrow = 0;
        /// <summary>
        /// Processes a loaded EEPROM to the map and datagrid
        /// </summary>
        void processToScreen(List<Locationwp> cmds, bool append = false)
        {
          
            // mono fix
            Commands.CurrentCell = null;

            while (Commands.Rows.Count > 0 && !append)
                Commands.Rows.Clear();

            if (cmds.Count == 0)
            {
               
                return;
            }

            Commands.SuspendLayout();
            Commands.Enabled = false;

            int i = Commands.Rows.Count - 1;
            int cmdidx = -1;
            foreach (Locationwp temp in cmds)
            {
                i++;
                cmdidx++;
                //Console.WriteLine("FP processToScreen " + i);
                if (temp.id == 0 && i != 0) // 0 and not home
                    break;
                if (temp.id == 255 && i != 0) // bad record - never loaded any WP's - but have started the board up.
                    break;
                if (cmdidx == 0 && append)
                {
                    // we dont want to add home again.
                    i--;
                    continue;
                }
                if (i + 1 >= Commands.Rows.Count)
                {
                    selectedrow = Commands.Rows.Add();
                    Commands.Rows[selectedrow].Cells[0].Value = selectedrow;
                }
                if (i == 0 && temp.alt == 0) // skip 0 home
                    continue;
                DataGridViewTextBoxCell cell;
                DataGridViewComboBoxCell cellcmd;
                cellcmd = Commands.Rows[i].Cells[Command.Index] as DataGridViewComboBoxCell;
                cellcmd.Value = "UNKNOWN";
                cellcmd.Tag = temp.id;

                foreach (object value in Enum.GetValues(typeof(MAVLink.MAV_CMD)))
                {
                    if ((ushort)value == temp.id)
                    {
                        cellcmd.Value = value.ToString();
                        break;
                    }
                }

                cell = Commands.Rows[i].Cells[Alt.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.alt * CurrentState.multiplieralt;
                cell = Commands.Rows[i].Cells[Lat.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.lat;
                cell = Commands.Rows[i].Cells[Lon.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.lng;

                cell = Commands.Rows[i].Cells[Param1.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p1;
                cell = Commands.Rows[i].Cells[Param2.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p2;
                cell = Commands.Rows[i].Cells[Param3.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p3;
                cell = Commands.Rows[i].Cells[Param4.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p4;

                // convert to utm/other
                convertFromGeographic(temp.lat, temp.lng);
            }

            Commands.Enabled = true;
            Commands.ResumeLayout();

            setWPParams();

            if (!append)
            {
                try
                {
                    DataGridViewTextBoxCell cellhome;
                    cellhome = Commands.Rows[0].Cells[Lat.Index] as DataGridViewTextBoxCell;
                    if (cellhome.Value != null)
                    {
                        if (cellhome.Value.ToString() != "0")
                        {
                            var dr = CustomMessageBox.Show("Reset Home to loaded coords", "Reset Home Coords",
                                MessageBoxButtons.YesNo);

                            if (dr == (int)DialogResult.Yes)
                            {
                                //TXT_homelat.Text = (double.Parse(cellhome.Value.ToString())).ToString();
                                cellhome = Commands.Rows[0].Cells[Lon.Index] as DataGridViewTextBoxCell;
                                //TXT_homelng.Text = (double.Parse(cellhome.Value.ToString())).ToString();
                                cellhome = Commands.Rows[0].Cells[Alt.Index] as DataGridViewTextBoxCell;
                                //TXT_homealt.Text = (double.Parse(cellhome.Value.ToString()) * CurrentState.multiplieralt).ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                   // log.Error(ex.ToString());
                } // if there is no valid home

                if (Commands.RowCount > 0)
                {
                    //log.Info("remove home from list");
                    Commands.Rows.Remove(Commands.Rows[0]); // remove home row
                }
            }

            //绘制范围


             mapControl.ZoomAndCenterMarkers("surveyGridlayer");

         
        }
        

        void setWPParams()
        {
            try
            {
               // log.Info("Loading wp params");

                Dictionary<string, double> param = new Dictionary<string, double>((Dictionary<string, double>)MainUI.comPort.MAV.param);

                if (param.ContainsKey("WP_RADIUS"))
                {
                   // TXT_WPRad.Text = (((double)param["WP_RADIUS"] * CurrentState.multiplierdist)).ToString();
                }
                if (param.ContainsKey("WPNAV_RADIUS"))
                {
                   // TXT_WPRad.Text = (((double)param["WPNAV_RADIUS"] * CurrentState.multiplierdist / 100.0)).ToString();
                }

               // log.Info("param WP_RADIUS " + TXT_WPRad.Text);

                //try
                //{
                //    TXT_loiterrad.Enabled = false;
                //    if (param.ContainsKey("LOITER_RADIUS"))
                //    {
                //        TXT_loiterrad.Text = (((double)param["LOITER_RADIUS"] * CurrentState.multiplierdist)).ToString();
                //        TXT_loiterrad.Enabled = true;
                //    }
                //    else if (param.ContainsKey("WP_LOITER_RAD"))
                //    {
                //        TXT_loiterrad.Text = (((double)param["WP_LOITER_RAD"] * CurrentState.multiplierdist)).ToString();
                //        TXT_loiterrad.Enabled = true;
                //    }

                //    log.Info("param LOITER_RADIUS " + TXT_loiterrad.Text);
                //}
                //catch (Exception ex)
                //{
                //   // log.Error(ex);
                //}
            }
            catch (Exception ex)
            {
               // log.Error(ex);
            }
        }

        private void convertFromGeographic(double lat, double lng)
        {
            if (lat == 0 && lng == 0)
            {
                return;
            }

            // always update other systems, incase user switchs while planning
            try
            {
                //UTM
                var temp = new PointLatLngAlt(lat, lng);
                int zone = temp.GetUTMZone();
                var temp2 = temp.ToUTM();
                Commands[coordZone.Index, selectedrow].Value = zone;
                Commands[coordEasting.Index, selectedrow].Value = temp2[0].ToString("0.000");
                Commands[coordNorthing.Index, selectedrow].Value = temp2[1].ToString("0.000");
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
            try
            {
                //MGRS
                Commands[MGRS.Index, selectedrow].Value = ((MGRS)new Geographic(lng, lat)).ToString();
            }
            catch (Exception ex)
            {
               // log.Error(ex);
            }
        }


        #region 航线参数变化

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cameras.ContainsKey(cmbCameras.Text))
            {
                camera = cameras[cmbCameras.Text];
                TripModel.Camera = camera;
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void altitudeBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (altitudeBar.Value!=TripModel.Altitude)
            {
                TripModel.Altitude = altitudeBar.Value;
                this.lblAltitude.Text = altitudeBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resolutionBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (resolutionBar.Value != TripModel.Resoluton) {

                TripModel.Resoluton = resolutionBar.Value;
                lblResolution.Text = resolutionBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speedBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (speedBar.Value!=TripModel.Speed) {
                TripModel.Speed = speedBar.Value;
                lblSpeed.Text = speedBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void overlapBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (overlapBar.Value != TripModel.Overlap)
            {
                TripModel.Overlap = overlapBar.Value;
               lblOverlap.Text = overlapBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sidelapBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (sidelapBar.Value != TripModel.Sidelap)
            {
                TripModel.Sidelap = sidelapBar.Value;
                lblSidelap.Text = sidelapBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headingBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (headingBar.Value != TripModel.Heading)
            {
                TripModel.Heading = headingBar.Value;
                lblHeading.Text = headingBar.Value.ToString();
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMB_startfrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TripModel!=null&&CMB_startfrom.Text != TripModel.StartFrom)
            {
                TripModel.StartFrom = CMB_startfrom.Text;
                if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);

            }
        }
        #endregion

        #region 显示选项设置

        //private void CHK_boundary_CheckedChanged(object sender, EventArgs e)
        //{
        //    displayInfos.boundary = CHK_boundary.Checked;
        //    if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, tripModel, displayInfos);
        //}


        private void CHK_markers_CheckedChanged(object sender, EventArgs e)
        {
            DisplayInfos.markers = CHK_markers.Checked;
            if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
        }

        private void CHK_grid_CheckedChanged(object sender, EventArgs e)
        {
            DisplayInfos.grid = CHK_grid.Checked;
            if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
        }

        private void CHK_internals_CheckedChanged(object sender, EventArgs e)
        {
            DisplayInfos.Internals = CHK_internals.Checked;
            if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
        }

        private void CHK_footprints_CheckedChanged(object sender, EventArgs e)
        {
            DisplayInfos.footPrints = CHK_footprints.Checked;
            if (AfterTripParamsChangeEvent != null) AfterTripParamsChangeEvent(mapPolygon, TripModel, DisplayInfos);
        }
        #endregion

        /// <summary>
        /// 设置航线结果
        /// </summary>
        /// <param name="tripResult"></param>
        public void SetTripResultValue(TripResult tripResult) {

            this.lblArea.Text = tripResult.area;
            this.lblTripLength.Text = tripResult.tripLength;
            this.lblPictureCount.Text = tripResult.pictureCount;
            this.lbltrigerTime.Text = tripResult.trigerCamTime;
            this.lblTripCount.Text = tripResult.tripCount;
            this.lblflyTime.Text = tripResult.flyTime;
        }

        private void SurveyGrid_Load(object sender, EventArgs e)
        {
            //显示参数
            getDisplayValue();
            //航线参数
            getTripValue();
            
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            base.Hide();
        }
    }
}
