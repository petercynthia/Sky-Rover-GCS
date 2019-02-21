using GeoUtility.GeoSystem;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner;
using MissionPlanner.ArduPilot;
using MissionPlanner.Controls;
using MissionPlanner.Maps;
using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using SKYROVER.GCS.DeskTop.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SKYROVER.GCS.DeskTop.Utilities
{
    class TripHelper
    {
        myGMAP mMapControl;
        // Variables
        const double rad2deg = (180 / Math.PI);
        const double deg2rad = (1.0 / rad2deg);

        MyDataGridView Commands;

        public delegate void AfterCalculateGrid(TripResult tripResult);
        
        #region 测绘航线

       

        List<PointLatLngAlt> grid;
        List<PointLatLngAlt> list = new List<PointLatLngAlt>();

        public myGMAP MMapControl { get => mMapControl; set => mMapControl = value; }

        private GMapMarker marker;

        private GMapPolygon mapPolygon;

        private TripResult tripResult;

        public TripResult GetTripResult { get => tripResult; }
        public GMapMarker Marker { get => marker; set => marker = value; }

        private TripModel tripModel;
        private DisplayInfos displayInfos;
        /// <summary>
        /// 航线规划图层
        /// </summary>
        GMapOverlay surveyGridlayer;

        double NUM_overshoot = 0;
        double NUM_overshoot2 = 0;
        float NUM_Lane_Dist = 0f;
        float num_corridorwidth = 0f;
        float NUM_leadin = 0;

        double TXT_fovH = 0;
        double TXT_fovW = 0;
        

        string DistUnits = "";

        /// <summary>
        /// 脚印范围图层
        /// </summary>
        GMapMarkerOverlapCount GMapMarkerOverlap = new GMapMarkerOverlapCount(PointLatLng.Empty);
        /// <summary>
        /// 
        /// </summary>
        public TripHelper() {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myGMAP"></param>
        /// <param name="gridView"></param>
        /// <param name="gMapPolygon"></param>
        /// <param name="tripModel"></param>
        /// <param name="displayInfos"></param>
        public TripHelper(myGMAP myGMAP,MyDataGridView gridView,GMapPolygon gMapPolygon,TripModel tripModel,DisplayInfos displayInfos)
        {
            mMapControl = myGMAP;
            Commands = gridView;
            this.mapPolygon = gMapPolygon;
            this.tripModel = tripModel;
            if (displayInfos == null) displayInfos = new DisplayInfos
            {

                markers = false,
                grid = true,
                Internals=false,
                footPrints=false

            };
            this.displayInfos = displayInfos;           
           
        }
        /// <summary>
        /// 
        /// </summary>
        public void runCalculate()
        {

            //表格填充命令
            updateCMDParams();

            //初始化测绘图层
            surveyGridlayer = new GMapOverlay("surveyGridlayer");
            mMapControl.Overlays.Add(surveyGridlayer);

            tripResult = new TripResult();

            //计算航线
            CalculateSurveyGrid(mapPolygon, tripModel, displayInfos);
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
        /// <param name="mapPolygon">测绘区域</param>
        /// <param name="tripModel">航线参数模型</param>
        /// <param name="displayInfos">数据显示模型</param>
        public void CalculateSurveyGrid(GMapPolygon mapPolygon, TripModel tripModel,DisplayInfos displayInfos)
        {
            //数据赋值
            this.mapPolygon = mapPolygon;
            this.tripModel = tripModel;
            this.displayInfos = displayInfos;

            //获取测绘区域所有点坐标   
            list.Clear();
            this.mapPolygon.Points.ForEach(x => { list.Add(x); });



            //根据模型数据计算并显示航线
           
            DoWorkAfterTripModelChange(this.tripModel,this.displayInfos);

        }
               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flyalt"></param>
        /// <param name="fovh"></param>
        /// <param name="fovv"></param>
        void getFOV(double flyalt, ref double fovh, ref double fovv)
        {
            double focallen = (double)tripModel.Camera.focallen;
            double sensorwidth = tripModel.Camera.sensorwidth;
            double sensorheight = tripModel.Camera.sensorheight;

            // scale      mm / mm
            double flscale = (1000 * flyalt) / focallen;

            //   mm * mm / 1000
            double viewwidth = (sensorwidth * flscale / 1000);
            double viewheight = (sensorheight * flscale / 1000);

            float fovh1 = (float)(Math.Atan(sensorwidth / (2 * focallen)) * rad2deg * 2);
            float fovv1 = (float)(Math.Atan(sensorheight / (2 * focallen)) * rad2deg * 2);

            fovh = viewwidth;
            fovv = viewheight;
        }
        /// <summary>
        ///照片间距
        /// </summary>
        decimal NUM_spacing = 0;
        /// <summary>
        /// 行间距
        /// </summary>
        decimal NUM_Distance = 0;

        /// <summary>
        /// 
        /// </summary>
        void doCalc()
        {
            try
            {
                // entered values
                float flyalt = (float)CurrentState.fromDistDisplayUnit((float)tripModel.Altitude);
                int imagewidth = tripModel.Camera.imagewidth;
                int imageheight = tripModel.Camera.imageheight;

                int overlap = (int)tripModel.Overlap;
                int sidelap = (int)tripModel.Sidelap;

                double viewwidth = 0;
                double viewheight = 0;

                getFOV(flyalt, ref viewwidth, ref viewheight);

                TXT_fovH = viewheight;
                TXT_fovW = viewwidth;

                /*
                TXT_fovH.Text = viewwidth.ToString("#.#");
                TXT_fovV.Text = viewheight.ToString("#.#");
                // Imperial
                feet_fovH = (viewwidth * 3.2808399f).ToString("#.#");
                feet_fovV = (viewheight * 3.2808399f).ToString("#.#");

                //    mm  / pixels * 100
                TXT_cmpixel.Text = ((viewheight / imageheight) * 100).ToString("0.00 cm");
                // Imperial
                inchpixel = (((viewheight / imageheight) * 100) * 0.393701).ToString("0.00 inches");

                NUM_spacing.ValueChanged -= domainUpDown1_ValueChanged;
                NUM_Distance.ValueChanged -= domainUpDown1_ValueChanged;

                if (CHK_camdirection.Checked)
                {
                    NUM_spacing.Value = (decimal)((1 - (overlap / 100.0f)) * viewheight);
                    NUM_Distance.Value = (decimal)((1 - (sidelap / 100.0f)) * viewwidth);
                }
                else
                {
                    NUM_spacing.Value = (decimal)((1 - (overlap / 100.0f)) * viewwidth);
                    NUM_Distance.Value = (decimal)((1 - (sidelap / 100.0f)) * viewheight);
                }
                NUM_spacing.ValueChanged += domainUpDown1_ValueChanged;
                NUM_Distance.ValueChanged += domainUpDown1_ValueChanged;
                */
                NUM_spacing = (decimal)((1 - (overlap / 100.0f)) * viewheight);
                NUM_Distance = (decimal)((1 - (sidelap / 100.0f)) * viewwidth);

            }
            catch { return; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripModel"></param>
        /// <param name="displayInfos"></param>
        public void DoWorkAfterTripModelChange(TripModel tripModel,DisplayInfos displayInfos)
        {
            //航线数据清空
            surveyGridlayer.Routes.Clear();
            surveyGridlayer.Polygons.Clear();
            surveyGridlayer.Markers.Clear();       
            
            
            if (tripModel!=null)
            {
                doCalc();
            }

            // new grid system 走廊航线
            if (tripModel.TripType==TripType.Corridor)
            {
                grid =Grid.CreateCorridor(list, CurrentState.fromDistDisplayUnit(tripModel.Altitude),
                    (double)NUM_Distance, (double)NUM_spacing, (double)tripModel.Heading,
                    (double)NUM_overshoot, (double)NUM_overshoot2,
                    (Grid.StartPosition)Enum.Parse(typeof(Grid.StartPosition), tripModel.StartFrom), false,
                    (float)NUM_Lane_Dist, (float)num_corridorwidth, (float)NUM_leadin);
            }
            else if(tripModel.TripType==TripType.Normal)
            {
                grid = Grid.CreateGrid(list, CurrentState.fromDistDisplayUnit(tripModel.Altitude),
                    (double)NUM_Distance, (double)NUM_spacing, (double)tripModel.Heading,
                    (double)NUM_overshoot, (double)NUM_overshoot2,
                    (Grid.StartPosition)Enum.Parse(typeof(Grid.StartPosition), tripModel.StartFrom), false,
                    (float)NUM_Lane_Dist, (float)NUM_leadin, MainUI.comPort.MAV.cs.HomeLocation);
            }

            mMapControl.HoldInvalidation = true;


            if (grid.Count == 0)
            {
                return;
            }
            //交叉（网格）航线
            if (tripModel.TripType==TripType.Cross)
            {
                // add crossover
                Grid.StartPointLatLngAlt = grid[grid.Count - 1];

                grid.AddRange(Grid.CreateGrid(list, CurrentState.fromDistDisplayUnit((double)tripModel.Altitude),
                    (double)NUM_Distance, (double)NUM_spacing, (double)tripModel.Heading + 90.0,
                    (double)NUM_overshoot, (double)NUM_overshoot2,
                    Grid.StartPosition.Point, false,
                    (float)NUM_Lane_Dist, (float)NUM_leadin, MainUI.comPort.MAV.cs.HomeLocation));
            }
            
            int strips = 0;
            int images = 0;
            int a = 1;
            PointLatLngAlt prevprevpoint = grid[0];
            PointLatLngAlt prevpoint = grid[0];


            #region 获取测区几何中心设置为home点

            if (marker == null || marker.Tag.ToString() != "H")
                MainUI.comPort.MAV.cs.HomeLocation = getCenterPoint(list);
            else MainUI.comPort.MAV.cs.HomeLocation = marker.Position;

            var homeplla = new PointLatLngAlt(MainUI.comPort.MAV.cs.HomeLocation.Lat,
                                   MainUI.comPort.MAV.cs.HomeLocation.Lng,
                                   MainUI.comPort.MAV.cs.HomeLocation.Alt / CurrentState.multiplieralt, "H");
            //绘制home点
            addpolygonmarker("H", MainUI.comPort.MAV.cs.HomeLocation.Lng, MainUI.comPort.MAV.cs.HomeLocation.Lat, 0, null, MainUI.homelayer);

            //绘制home点到开始点和结束点的连线
            GMapRoute homeroute = new GMapRoute("home route");

            homeroute.Points.Add(grid.Last());
            homeroute.Points.Add(homeplla);
            homeroute.Points.Add(grid.First());

            homeroute.Stroke = new Pen(Color.Yellow, 2);
            
            homeroute.Stroke.DashStyle = DashStyle.Dash;

            surveyGridlayer.Routes.Add(homeroute);

            #endregion


            // distance to/from home
            double routetotal = grid.First().GetDistance(MainUI.comPort.MAV.cs.HomeLocation) / 1000.0 +
                               grid.Last().GetDistance(MainUI.comPort.MAV.cs.HomeLocation) / 1000.0;
            List<PointLatLng> segment = new List<PointLatLng>();
            double maxgroundelevation = double.MinValue;
            double mingroundelevation = double.MaxValue;
            double startalt = MainUI.comPort.MAV.cs.HomeAlt;

            
           // string pos = "";
            foreach (var item in grid)
            {
                //pos += item.ToString() + "\n";
                double currentalt = srtm.getAltitude(item.Lat, item.Lng).alt;
                mingroundelevation = Math.Min(mingroundelevation, currentalt);
                maxgroundelevation = Math.Max(maxgroundelevation, currentalt);

                prevprevpoint = prevpoint;
                //item.Tag==“M” 改点为航点信息；item.Tag=="SM"或“ME”开始任务或任务结束
                if (item.Tag == "M")
                {
                    images++;
                    //显示内部航点
                    if (displayInfos.Internals)
                    {
                        surveyGridlayer.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.red_small) { ToolTipText = a.ToString(), ToolTipMode = MarkerTooltipMode.OnMouseOver });
                        a++;

                        segment.Add(prevpoint);
                        segment.Add(item);
                        prevpoint = item;
                    }
                    try
                    {
                        if (TXT_fovH != 0)
                        {
                            if (displayInfos.footPrints)
                            {
                                double fovh =TXT_fovH;
                                double fovv = TXT_fovW;

                                getFOV(item.Alt + startalt - currentalt, ref fovh, ref fovv);

                                double startangle = 0;

                                if (false)
                                {
                                    startangle += 90;
                                }

                                double angle1 = startangle - (Math.Sin((fovh / 2.0) / (fovv / 2.0)) * rad2deg);
                                double dist1 = Math.Sqrt(Math.Pow(fovh / 2.0, 2) + Math.Pow(fovv / 2.0, 2));

                                double bearing = (double)tripModel.Heading;

                                /*
                                if (CHK_copter_headinghold.Checked)
                                {
                                    bearing = Convert.ToInt32(TXT_headinghold.Text);
                                }

                                if (chk_test.Checked)
                                    bearing = prevprevpoint.GetBearing(item);
                                */
                                double fovha = 0;
                                double fovva = 0;
                                getFOVangle(ref fovha, ref fovva);
                                var itemcopy = new PointLatLngAlt(item);
                                itemcopy.Alt += startalt;
                                var temp = ImageProjection.calc(itemcopy, 0, 0, bearing + startangle, fovha, fovva);

                                List<PointLatLng> footprint = new List<PointLatLng>();
                                footprint.Add(temp[0]);
                                footprint.Add(temp[1]);
                                footprint.Add(temp[2]);
                                footprint.Add(temp[3]);

                                GMapPolygon poly = new GMapPolygon(footprint, a.ToString());
                                poly.Stroke =
                                    new Pen(Color.FromArgb(250 - ((a * 5) % 240), 250 - ((a * 3) % 240), 250 - ((a * 9) % 240)), 1);
                                poly.Fill = new SolidBrush(Color.Transparent);

                                GMapMarkerOverlap.Add(poly);

                                surveyGridlayer.Polygons.Add(poly);
                                a++;
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    if (item.Tag != "SM" && item.Tag != "ME")
                        strips++;

                    if (displayInfos.markers)
                    {
                        var marker = new GMapMarkerWP(item, a.ToString()) { ToolTipText = a.ToString(), ToolTipMode = MarkerTooltipMode.OnMouseOver };
                        surveyGridlayer.Markers.Add(marker);
                    }

                    segment.Add(prevpoint);
                    segment.Add(item);
                    prevpoint = item;
                    a++;
                }
                GMapRoute seg = new GMapRoute(segment, "segment" + a.ToString());
                seg.Stroke = new Pen(Color.Yellow, 4);
                seg.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                seg.IsHitTestVisible = true;
                routetotal = routetotal + (float)seg.Distance;

                //show GRID
                if (displayInfos.grid)
                {
                    surveyGridlayer.Routes.Add(seg);
                }
                else
                {
                    seg.Dispose();
                }

                segment.Clear();
            }


            //使用grids填充表格
            FillDGVGrids();

            if (displayInfos.footPrints)
                surveyGridlayer.Markers.Add(GMapMarkerOverlap);
           
            // turn radrad = tas^2 / (tan(angle) * G)
            float v_sq = (float)(((float)tripModel.Speed / CurrentState.multiplierspeed) * ((float)tripModel.Speed / CurrentState.multiplierspeed));
            float turnrad = (float)(v_sq / (float)(9.808f * Math.Tan(35 * deg2rad)));
          
     
            // Meters
            tripResult.area = (calcpolygonarea(list)/1000000).ToString("#");
            tripResult.tripLength = routetotal.ToString("0.00");
              
           
            double flyspeedms = CurrentState.fromSpeedDisplayUnit((double)tripModel.Speed);
           

            tripResult.pictureCount = images.ToString();
            tripResult.tripCount = ((int)(strips / 2)).ToString();

            double seconds = ((double.Parse(tripResult.tripLength) * 1000.0) / flyspeedms);
           
            tripResult.flyTime = secondsToNice(seconds);

            tripResult.trigerCamTime = secondsToNice(((double)NUM_spacing / flyspeedms));
            mMapControl.HoldInvalidation = false;
          
           // mMapControl.ZoomAndCenterMarkers("routes");
          
            //  CalcHeadingHold();

            mMapControl.Invalidate();

           
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private PointLatLngAlt getCenterPoint(List<PointLatLngAlt> list)
        {
            //经纬度转墨卡托
            double[] tempD = CoordConvertHelper.ConvertLngLat2WebMercator(list[0].Lng, list[0].Lat);
            PointLatLngAlt centerPoint = new PointLatLngAlt(tempD[1], tempD[0]);

            for (int index = 1; index < list.Count; index++)
            {
                tempD = CoordConvertHelper.ConvertLngLat2WebMercator(list[index].Lng, list[index].Lat);
                centerPoint.Lat += tempD[1];
                centerPoint.Lng += tempD[0];
            }
            centerPoint.Lat /= list.Count;
            centerPoint.Lng /= list.Count;
            //墨卡托转经纬度
            tempD = CoordConvertHelper.ConvertWebMercator2LngLat(centerPoint.Lng, centerPoint.Lat);

            return new PointLatLngAlt(tempD[1], tempD[0]);
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
                return hours.ToString("f2") + " 时";
            }
            else if (mins > 1)
            {
                return mins.ToString("f2") + " 分";
            }
            else
            {
                return secs.ToString("0.00") + " 秒";
            }
        }

        //private void CalcHeadingHold()
        //{
        //    int previous = (int)Math.Round(Convert.ToDecimal(((UpDownBase)NUM_angle).Text)); //((UpDownBase)sender).Text
        //    int current = (int)Math.Round(NUM_angle.Value);

        //    int change = current - previous;

        //    if (change > 0) // Positive change
        //    {
        //        int val = Convert.ToInt32(TXT_headinghold.Text) + change;
        //        if (val > 359)
        //        {
        //            val = val - 360;
        //        }
        //        TXT_headinghold.Text = val.ToString();
        //    }

        //    if (change < 0) // Negative change
        //    {
        //        int val = Convert.ToInt32(TXT_headinghold.Text) + change;
        //        if (val < 0)
        //        {
        //            val = val + 360;
        //        }
        //        TXT_headinghold.Text = val.ToString();
        //    }
        //}

        double calcpolygonarea(List<PointLatLngAlt> polygon)
        {
            // should be a closed polygon
            // coords are in lat long
            // need utm to calc area

            if (polygon.Count == 0)
            {
                CustomMessageBox.Show("请绘制一个多边形!");
                return 0;
            }

            // close the polygon
            if (polygon[0] != polygon[polygon.Count - 1])
                polygon.Add(polygon[0]); // make a full loop

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int)((polygon[0].Lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone, polygon[0].Lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double prod1 = 0;
            double prod2 = 0;

            for (int a = 0; a < (polygon.Count - 1); a++)
            {
                double[] pll1 = { polygon[a].Lng, polygon[a].Lat };
                double[] pll2 = { polygon[a + 1].Lng, polygon[a + 1].Lat };

                double[] p1 = trans.MathTransform.Transform(pll1);
                double[] p2 = trans.MathTransform.Transform(pll2);

                prod1 += p1[0] * p2[1];
                prod2 += p1[1] * p2[0];
            }

            double answer = (prod1 - prod2) / 2;

            if (polygon[0] == polygon[polygon.Count - 1])
                polygon.RemoveAt(polygon.Count - 1); // unmake a full loop

            return Math.Abs(answer);
        }

        void getFOVangle(ref double fovh, ref double fovv)
        {
            double focallen = (double)tripModel.Camera.focallen;
            double sensorwidth = tripModel.Camera.sensorwidth;
            double sensorheight = tripModel.Camera.sensorheight;

            fovh = (float)(Math.Atan(sensorwidth / (2 * focallen)) * rad2deg * 2);
            fovv = (float)(Math.Atan(sensorheight / (2 * focallen)) * rad2deg * 2);
        }

        void AddDrawPolygon()
        {
            List<PointLatLng> list2 = new List<PointLatLng>();

            list.ForEach(x => { list2.Add(x); });

            var poly = new GMapPolygon(list2, "poly");
            poly.Stroke = new Pen(Color.Red, 2);
            poly.Fill = Brushes.Transparent;

            surveyGridlayer.Polygons.Add(poly);

            foreach (var item in list)
            {
                surveyGridlayer.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.red));
            }
        }

        int selectedrow = 0;

        private Dictionary<string, string[]> cmdParamNames = new Dictionary<string, string[]>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private int AddCommand(MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x, double y,
           double z, object tag = null)
        {
            selectedrow = Commands.Rows.Add();

            FillCommand(this.selectedrow, cmd, p1, p2, p3, p4, x, y, z, tag);

            return selectedrow;
          
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cmd"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="tag"></param>
        private void FillCommand(int rowIndex, MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x,
           double y, double z, object tag = null)
        {
            Commands.Rows[rowIndex].Cells[0].Value = rowIndex+1;
            Commands.Rows[rowIndex].Cells[1].Value = cmd.ToString();

            Commands.Rows[rowIndex].Cells[2].Value = p1;
            Commands.Rows[rowIndex].Cells[3].Value = p2;
            Commands.Rows[rowIndex].Cells[4].Value = p3;
            Commands.Rows[rowIndex].Cells[5].Value = p4;
            Commands.Rows[rowIndex].Cells[6].Value = y;
            Commands.Rows[rowIndex].Cells[7].Value = x;
            Commands.Rows[rowIndex].Cells[8].Value = z;

            Commands.Rows[rowIndex].Cells[19].Tag = tag;
            Commands.Rows[rowIndex].Cells[19].Value = tag;

        }

        List<List<Locationwp>> history = new List<List<Locationwp>>();
        /// <summary>
        /// Actualy Sets the values into the datagrid and verifys height if turned on
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="alt"></param>
        private void setfromMap(double lat, double lng, int alt, double p1 = 0)
        {

            if (selectedrow > Commands.RowCount)
            {
                CustomMessageBox.Show("Invalid coord, How did you do this?");
                return;
            }

            DataGridViewTextBoxCell cell;

            //地形高度
            //int newsrtm = (int)((srtm.getAltitude(lat, lng).alt) * CurrentState.multiplieralt);

            if (Commands.Columns[6].HeaderText.Equals(cmdParamNames["WAYPOINT"][4] /*"Lat"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[6] as DataGridViewTextBoxCell;
                cell.Value = lat.ToString("0.0000000");
                cell.DataGridView.EndEdit();
            }
            if (Commands.Columns[7].HeaderText.Equals(cmdParamNames["WAYPOINT"][5] /*"Long"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[7] as DataGridViewTextBoxCell;
                cell.Value = lng.ToString("0.0000000");
                cell.DataGridView.EndEdit();
            }
            if (alt != -1 && alt != -2 &&Commands.Columns[8].HeaderText.Equals(cmdParamNames["WAYPOINT"][6] /*"Alt"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[8] as DataGridViewTextBoxCell;


                cell.Value = alt;

                float ans;
                if (float.TryParse(cell.Value.ToString(), out ans))
                {
                    ans = (int)ans;
                    if (alt != 0) // use passed in value;
                        cell.Value = alt.ToString();
                    if (ans == 0) // default
                        cell.Value = 100;
                    if (ans == 0 && (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduCopter2))
                        cell.Value = 15;

                    cell.DataGridView.EndEdit();
                }
                else
                {
                    CustomMessageBox.Show("Invalid Home or wp Alt");
                    cell.Style.BackColor = Color.Red;
                }
            }

            // convert to utm
            convertFromGeographic(lat, lng);

            // Add more for other params
            if (Commands.Columns[2].HeaderText.Equals(cmdParamNames["WAYPOINT"][1] /*"Delay"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[2] as DataGridViewTextBoxCell;
                cell.Value = p1;
                cell.DataGridView.EndEdit();
            }
            Commands.EndEdit();

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
                Commands[9, selectedrow].Value = zone;
                Commands[10, selectedrow].Value = temp2[0].ToString("0.000");
                Commands[11, selectedrow].Value = temp2[1].ToString("0.000");
            }
            catch (Exception ex)
            {
                // log.Error(ex);
            }
            try
            {
                //MGRS
                Commands[12, selectedrow].Value = ((MGRS)new Geographic(lng, lat)).ToString();
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
        }


        List<Locationwp> GetCommandList()
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
                if (Commands.Rows[a].Cells[1].Value.ToString().Contains("UNKNOWN"))
                {
                    temp.id = (ushort)Commands.Rows[a].Cells[1].Tag;
                }
                else
                {
                    temp.id =
                        (ushort)
                                Enum.Parse(typeof(MAVLink.MAV_CMD),
                                    Commands.Rows[a].Cells[1].Value.ToString(),
                                    false);
                }
                temp.p1 = float.Parse(Commands.Rows[a].Cells[2].Value.ToString());

                temp.alt =
                    (float)
                        (double.Parse(Commands.Rows[a].Cells[8].Value.ToString()) / CurrentState.multiplieralt);
                temp.lat = (double.Parse(Commands.Rows[a].Cells[6].Value.ToString()));
                temp.lng = (double.Parse(Commands.Rows[a].Cells[7].Value.ToString()));

                temp.p2 = (float)(double.Parse(Commands.Rows[a].Cells[3].Value.ToString()));
                temp.p3 = (float)(double.Parse(Commands.Rows[a].Cells[4].Value.ToString()));
                temp.p4 = (float)(double.Parse(Commands.Rows[a].Cells[5].Value.ToString()));

                temp.Tag = Commands.Rows[a].Cells[19].Value;

                return temp;
        }
            catch (Exception ex)
            {
                throw new FormatException("Invalid number on row " + (a + 1).ToString(), ex);
            }
}

        void updateCMDParams()
        {
            cmdParamNames = readCMDXML();

            List<string> cmds = new List<string>();

            foreach (string item in cmdParamNames.Keys)
            {
                cmds.Add(item);
            }

            cmds.Add("UNKNOWN");

            ((DataGridViewComboBoxColumn)this.Commands.Columns[1]).DataSource = cmds;
        }

        Dictionary<string, string[]> readCMDXML()
        {
            Dictionary<string, string[]> cmd = new Dictionary<string, string[]>();

            // do lang stuff here

            string file = Settings.GetRunningDirectory() + "mavcmd.xml";

            if (!File.Exists(file))
            {
                CustomMessageBox.Show("Missing mavcmd.xml file");
                return cmd;
            }

            //log.Info("Reading MAV_CMD for " + MainUI.comPort.MAV.cs.firmware);

            using (XmlReader reader = XmlReader.Create(file))
            {
                reader.Read();
                reader.ReadStartElement("CMD");
                if (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduPlane ||
                    MainUI.comPort.MAV.cs.firmware == Firmwares.Ateryx)
                {
                    reader.ReadToFollowing("APM");
                }
                else if (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduRover)
                {
                    reader.ReadToFollowing("APRover");
                }
                else
                {
                    reader.ReadToFollowing("AC2");
                }

                XmlReader inner = reader.ReadSubtree();

                inner.Read();

                inner.MoveToElement();

                inner.Read();

                while (inner.Read())
                {
                    inner.MoveToElement();
                    if (inner.IsStartElement())
                    {
                        string cmdname = inner.Name;
                        string[] cmdarray = new string[7];
                        int b = 0;

                        XmlReader inner2 = inner.ReadSubtree();

                        inner2.Read();

                        while (inner2.Read())
                        {
                            inner2.MoveToElement();
                            if (inner2.IsStartElement())
                            {
                                cmdarray[b] = inner2.ReadString();
                                b++;
                            }
                        }

                        cmd[cmdname] = cmdarray;
                    }
                }
            }

            return cmd;
        }

        /// <summary>
        /// 使用格网填充表格
        /// </summary>
        private void FillDGVGrids()
        {
            //清除已有表格数据
            if(this.Commands!=null&&this.Commands.Rows.Count>0) this.Commands.Rows.Clear();

            if (grid != null && grid.Count > 0)
            {
               //保存成果数据
                var gridobject = savegriddata();
                //分区数量
                double NUM_split = 1.0;

                //定义每个分区内有多少航点
                int wpsplit = (int)Math.Round(grid.Count / NUM_split, MidpointRounding.AwayFromZero);

                List<int> wpsplitstart = new List<int>();

                for (int splitno = 0; splitno < NUM_split; splitno++)
                {
                    //第splitno分区内的起点和终点下标
                    int wpstart = wpsplit * splitno;
                    int wpend = wpsplit * (splitno + 1);

                    while (wpstart != 0 && wpstart < grid.Count && grid[wpstart].Tag != "E")
                    {
                        wpstart--;
                    }

                    while (wpend > 0 && wpend < grid.Count && grid[wpend].Tag != "S")
                    {
                        wpend--;
                    }


                    //01 设置飞机在解锁点起飞
                    var wpno = AddCommand(MAVLink.MAV_CMD.TAKEOFF, 20, 0, 0, 0, 0, 0, (int)(30 * CurrentState.multiplierdist), gridobject);
                    wpsplitstart.Add(wpno);
                    //02 设置飞机飞行速度                   
                    AddCommand(MAVLink.MAV_CMD.DO_CHANGE_SPEED, 0,(int)((float)tripModel.Speed / CurrentState.multiplierspeed), 0, 0, 0, 0, 0,gridobject);
                   
                    int i = 0;
                    bool startedtrigdist = false;
                    PointLatLngAlt lastplla = PointLatLngAlt.Zero;

                    //03 开始循环获取并设置航点信息
                    foreach (var plla in grid)
                    {
                        // skip before start point
                        if (i < wpstart)
                        {
                            i++;
                            continue;
                        }
                        // skip after endpoint
                        if (i >= wpend) break;

                        if (i > wpstart)
                        {
                            // internal point check
                            if (plla.Tag == "M")
                            {
                               AddWP(plla.Lng, plla.Lat, plla.Alt);
                               AddCommand(MAVLink.MAV_CMD.DO_SET_CAM_TRIGG_DIST, 1, 0, 0, 0, 0, 1, 0,gridobject);
                              
                            }
                            else
                            {
                                // only add points that are ends
                                if (plla.Tag == "S" || plla.Tag == "E")
                                {
                                    if (plla.Lat != lastplla.Lat || plla.Lng != lastplla.Lng ||
                                        plla.Alt != lastplla.Alt)
                                        AddWP(plla.Lng, plla.Lat, plla.Alt);
                                }

                                if (plla.Tag == "SM")
                                {
                                    //  s > sm, need to dup check
                                    if (plla.Lat != lastplla.Lat || plla.Lng != lastplla.Lng ||
                                         plla.Alt != lastplla.Alt)
                                        AddWP(plla.Lng, plla.Lat, plla.Alt);

                                    AddCommand(MAVLink.MAV_CMD.DO_SET_CAM_TRIGG_DIST,
                                            (float)NUM_spacing,
                                            0, 0, 0, 0, 0, 0, gridobject);
                                }
                                else if (plla.Tag == "ME")
                                {
                                    AddWP(plla.Lng, plla.Lat, plla.Alt);

                                    AddCommand(MAVLink.MAV_CMD.DO_SET_CAM_TRIGG_DIST, 0,
                                            0, 0, 0, 0, 0, 0, gridobject);
                                }
                                else {

                                    // add single start trigger
                                    if (!startedtrigdist)
                                    {
                                        AddCommand(MAVLink.MAV_CMD.DO_SET_CAM_TRIGG_DIST,
                                            (float)NUM_spacing,0, 0, 0, 0, 0, 0, gridobject);
                                        startedtrigdist = true;
                                    }
                                    else if (plla.Tag == "ME")
                                    {
                                        AddWP(plla.Lng, plla.Lat, plla.Alt);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddWP(plla.Lng, plla.Lat, plla.Alt, gridobject);
                        }
                        lastplla = plla;
                        ++i;
                    }

                    //end
                    AddCommand(MAVLink.MAV_CMD.DO_SET_CAM_TRIGG_DIST, 0, 0, 0, 0, 0, 0, 0, gridobject);
                    ////change speed
                    //double speed = MainUI.comPort.MAV.param["WPNAV_SPEED"].Value;
                    //speed = speed / 100;
                    //AddCommand(MAVLink.MAV_CMD.DO_CHANGE_SPEED, 0, speed, 0, 0, 0, 0, 0, gridobject);

                    //
                    AddCommand(MAVLink.MAV_CMD.RETURN_TO_LAUNCH, 0, 0, 0, 0, 0, 0, 0, gridobject);
                    
                }

                if (NUM_split > 1)
                {
                    int index = 0;
                    foreach (var i in wpsplitstart)
                    {
                        // add do jump
                        InsertCommand(index, MAVLink.MAV_CMD.DO_JUMP, i + wpsplitstart.Count + 1, 1, 0, 0, 0, 0, 0, gridobject);
                        index++;
                    }

                }
                // save camera fov's for use with footprints
                double fovha = 0;
                double fovva = 0;
                try
                {
                    getFOVangle(ref fovha, ref fovva);
                    Settings.Instance["camera_fovh"] = fovha.ToString();
                    Settings.Instance["camera_fovv"] = fovva.ToString();
                  
                }
                catch (Exception ex)
                {
                   // log.Error(ex);
                }

               // savesettings();

             
            }
            else
            {
                CustomMessageBox.Show("Bad Grid", "Error");
            }
        }

        void savesettings()
        {
            //plugin.Host.config["grid_camera"] = CMB_camera.Text;
            //plugin.Host.config["grid_alt"] = NUM_altitude.Value.ToString();
            //plugin.Host.config["grid_angle"] = NUM_angle.Value.ToString();
            //plugin.Host.config["grid_camdir"] = CHK_camdirection.Checked.ToString();

            //plugin.Host.config["grid_usespeed"] = CHK_usespeed.Checked.ToString();

            //plugin.Host.config["grid_dist"] = NUM_Distance.Value.ToString();
            //plugin.Host.config["grid_overshoot1"] = NUM_overshoot.Value.ToString();
            //plugin.Host.config["grid_overshoot2"] = NUM_overshoot2.Value.ToString();
            //plugin.Host.config["grid_leadin"] = NUM_leadin.Value.ToString();
            //plugin.Host.config["grid_overlap"] = num_overlap.Value.ToString();
            //plugin.Host.config["grid_sidelap"] = num_sidelap.Value.ToString();
            //plugin.Host.config["grid_spacing"] = NUM_spacing.Value.ToString();
            //plugin.Host.config["grid_crossgrid"] = chk_crossgrid.Checked.ToString();

            //plugin.Host.config["grid_startfrom"] = CMB_startfrom.Text;

            //plugin.Host.config["grid_autotakeoff"] = CHK_toandland.Checked.ToString();
            //plugin.Host.config["grid_autotakeoff_RTL"] = CHK_toandland_RTL.Checked.ToString();

            //plugin.Host.config["grid_internals"] = CHK_internals.Checked.ToString();
            //plugin.Host.config["grid_footprints"] = CHK_footprints.Checked.ToString();
            //plugin.Host.config["grid_advanced"] = CHK_advanced.Checked.ToString();

            //plugin.Host.config["grid_trigdist"] = rad_trigdist.Checked.ToString();
            //plugin.Host.config["grid_digicam"] = rad_digicam.Checked.ToString();
            //plugin.Host.config["grid_repeatservo"] = rad_repeatservo.Checked.ToString();
            //plugin.Host.config["grid_breakstopstart"] = chk_stopstart.Checked.ToString();

            //// Copter Settings
            //plugin.Host.config["grid_copter_delay"] = NUM_copter_delay.Value.ToString();
            //plugin.Host.config["grid_copter_headinghold_chk"] = CHK_copter_headinghold.Checked.ToString();

            //// Plane Settings
            //plugin.Host.config["grid_min_lane_separation"] = NUM_Lane_Dist.Value.ToString();
        }


        //rad_trigdist


        //rad_digicam


        //rad_repeatservo

        
        //rad_do_set_servo



        private void AddWP(double Lng, double Lat, double Alt, object gridobject = null)
        {
            //if (CHK_copter_headinghold.Checked)
            //{
            //    AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, Convert.ToInt32(TXT_headinghold.Text), 0, 0, 0, 0, 0, 0, gridobject);
            //}

            //if (NUM_copter_delay.Value > 0)
            //{
            //    AddCommand(MAVLink.MAV_CMD.WAYPOINT, (double)NUM_copter_delay.Value, 0, 0, 0, Lng, Lat, Alt * CurrentState.multiplierdist, gridobject);
            //}
            //else
            //{
                AddCommand(MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, Lng, Lat, (int)(Alt * CurrentState.multiplierdist), gridobject);
            //}
        }


        public void InsertCommand(int rowIndex, MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x, double y,
           double z, object tag = null)
        {
            if (Commands.Rows.Count <= rowIndex)
            {
                AddCommand(cmd, p1, p2, p3, p4, x, y, z, tag);
                return;
            }

            Commands.Rows.Insert(rowIndex);

            this.selectedrow = rowIndex;

            FillCommand(this.selectedrow, cmd, p1, p2, p3, p4, x, y, z, tag);

            
        }

        /// <summary>
        /// 保存航线数据
        /// </summary>
        /// <returns></returns>
        GridData savegriddata()
        {
            GridData griddata = new GridData();

            griddata.poly = list;

            //griddata.camera = CMB_camera.Text;
            //griddata.alt = NUM_altitude.Value;
            //griddata.angle = NUM_angle.Value;
            //griddata.camdir = CHK_camdirection.Checked;
            //griddata.speed = NUM_UpDownFlySpeed.Value;
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

        #endregion

    }

    public struct GridData
    {
        public List<PointLatLngAlt> poly;
        //simple
        public string camera;
        public decimal alt;
        public decimal angle;
        public bool camdir;
        public decimal speed;
        public bool usespeed;
        public bool autotakeoff;
        public bool autotakeoff_RTL;

        public decimal splitmission;

        public bool internals;
        public bool footprints;
        public bool advanced;

        //options
        public decimal dist;
        public decimal overshoot1;
        public decimal overshoot2;
        public decimal leadin;
        public string startfrom;
        public decimal overlap;
        public decimal sidelap;
        public decimal spacing;
        public bool crossgrid;
        // Copter Settings
        public decimal copter_delay;
        public bool copter_headinghold_chk;
        public decimal copter_headinghold;
        // plane settings
        public bool alternateLanes;
        public decimal minlaneseparation;

        // camera config
        public bool trigdist;
        public bool digicam;
        public bool repeatservo;

        public bool breaktrigdist;

        public decimal repeatservo_no;
        public decimal repeatservo_pwm;
        public decimal repeatservo_cycle;

        // do set servo
        public decimal setservo_no;
        public decimal setservo_low;
        public decimal setservo_high;
    }
}
