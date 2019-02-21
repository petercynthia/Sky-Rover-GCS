using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Controls.MapMarkers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MissionPlanner.Controls.Tools
{
    public delegate void OnFinished(GMapPolygon gMapPolygon, GMapMarker gMapMarker);

    public class DrawPolygonTool : IMapTool
    {
      public event OnFinished OnFinishedEvent;

        GMapMarker currentMarker;

        GMapPolygon gMapPolygon;

        GMapOverlay gMapOverlay;

        Size selSize=new Size(40,40);

        Size unSelSize=new Size(32,32);

        bool isDrawPolygon = false;

        bool isDragMarker = false;

        /// <summary>
        /// 
        /// </summary>
        public DrawPolygonTool()
        {

            if (gMapOverlay == null) gMapOverlay = new GMapOverlay("WPMapOverlay");        

            if (gMapPolygon == null) gMapPolygon = new GMapPolygon(new List<PointLatLng>(), "tempPolygon");           
            gMapPolygon.Stroke = new Pen(Color.Red, 2);
            gMapPolygon.Fill = Brushes.Transparent;
            gMapOverlay.Polygons.Add(gMapPolygon);
        }

        public myGMAP MapControl { get ; set; }

        public string Name => "航测区域";

        public string Description => "鼠标左键点击地图，绘制多边形，至少绘制三个点，点击右键结束绘制";

        private bool enabled;
        public bool Enabled { get => enabled; set => enabled=value; }

        private Cursor cursor;
        public Cursor Cursor { get => cursor; set => cursor=value; }// throw new NotImplementedException(); }

        public event EventHandler EnabledChanged;

        public event EventHandler CursorChanged;

        public bool DoKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            throw new NotImplementedException();
        }

        public bool DoKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            throw new NotImplementedException();
        }

        public bool DoMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            return true;
        }

        public bool DoMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            return true;
        }

        private bool isMouseDown = false;     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        /// <returns></returns>
        public bool DoMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {

            if (mouseEventArgs.Button == MouseButtons.Right)
            {
                if (isDrawPolygon)
                {
                    isDrawPolygon = false;
                    if (OnFinishedEvent != null) OnFinishedEvent(gMapPolygon,null);
                }
             
                isDragMarker = false;
                currentMarker = null;
                
                return true;
            }else if (mouseEventArgs.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                if (isDrawPolygon)
                {
                    PointLatLng tempPoint = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);
                    gMapPolygon.Points.Add(tempPoint);
                    AddKeyPoint(gMapOverlay, tempPoint);
                    MapControl.UpdatePolygonLocalPosition(gMapPolygon);
                    MapControl.Invalidate(true);
                    return true;
                }
                if (currentMarker!=null)
                {
                    isDragMarker = true;
                    MapControl.CanDragMap = false;
                    return true;
                }

                if (!isDragMarker && !isDrawPolygon) MapControl.CanDragMap = true;

            }
                       

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        /// <returns></returns>
        public bool DoMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (isDragMarker&& isMouseDown)
            {
                this.MapControl.OnMarkerEnter -= DoMouseEnter;
                this.MapControl.OnMarkerLeave -= DoMouseLeave;

                MapControl.CanDragMap = false;

                currentMarker.Position = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);
                if (!currentMarker.Tag.ToString().Contains("H"))
                {
                    gMapPolygon.Points[int.Parse(currentMarker.Tag.ToString().Replace("grid", "")) - 1] =
                                                  new PointLatLng(currentMarker.Position.Lat, currentMarker.Position.Lng);
                    this.MapControl.UpdatePolygonLocalPosition(gMapPolygon);
                }
                else {

                    //更新home点位置及连线
                    var lyrs = MapControl.Overlays.Where(a => a.Id == "surveyGridlayer").ToList();
                    if (lyrs.Count > 0) {
                        foreach (var lyr in lyrs)
                        {
                           var homeRoute=lyr.Routes.Where(b => b.Name == "home route").ToList();
                            homeRoute[0].Points[1] = currentMarker.Position;
                            this.MapControl.UpdateRouteLocalPosition(homeRoute[0]);
                            break;
                        }
                    }

                }

               

                this.MapControl.Invalidate();

            }
            else if (isDrawPolygon)
            {

                MapControl.CanDragMap = false;
            }
            else MapControl.CanDragMap = true;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        /// <returns></returns>
        public bool DoMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            isMouseDown = false;

            if (isDragMarker)
            {
                isDragMarker = false;
                if (OnFinishedEvent != null) OnFinishedEvent(gMapPolygon,currentMarker);
            }


            if (currentMarker != null) {

                InsertKeyPoint(gMapPolygon, currentMarker);

                currentMarker = null;
            } 

            this.MapControl.OnMarkerEnter += DoMouseEnter;
            this.MapControl.OnMarkerLeave += DoMouseLeave;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void DoMouseEnter(GMapMarker item)
        {
            if (isDrawPolygon) return ;

            if (item.Tag == null) return;
            
            if ((item.Tag.ToString().Contains("grid")|| (item.Tag.ToString().Contains("H"))) &&(item is GMarkerGoogle)) {

                currentMarker = item as GMapMarker;
                currentMarker.Size = selSize;
                Cursor = Cursors.SizeAll;
                MapControl.CanDragMap = false;
                isDragMarker = true;
            }

           
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void DoMouseLeave(GMapMarker item)
        {
            if (isDrawPolygon) return ;



            if (item is GMarkerGoogle)
            {
                (item as GMapMarker).Size=unSelSize;
                
            }
          


        }

        public bool DoMouseHover(PointLatLng mapPosition)
        {
            return true;
        }


        public bool DoMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        public void DoPaint(PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnClick()
        {
            //设定开始画多边形
            isDrawPolygon = true;

           
            foreach (var lyr in MapControl.Overlays)
            {
                if (lyr.Id == "WPMapOverlay")
                {
                    this.MapControl.Overlays.Remove(lyr);
                    break;
                }
                //else if (lyr.Id == "surveyGridlayer") {

                //    lyr.Routes.Clear();
                //    lyr.Markers.Clear();
                //}
            }

            this.MapControl.Overlays.Add(gMapOverlay);

            this.Cursor = Cursors.Cross;
        }

        private void ClearOverlay(GMapOverlay polygons) {

            
                polygons.Routes.Clear();
                polygons.Markers.Clear();
                polygons.Polygons.Clear();
                
            
        }


        private void InsertKeyPoint(GMapPolygon mapPolygon, GMapMarker currentMarker)
        {

            if (currentMarker.Tag.ToString().Contains("H")) return;

            if (currentMarker.ToolTipText.Contains("changed")) return;

            if (mapPolygon == null || mapPolygon.Points.Count < 3) return;

            
 
            int currentIndex = int.Parse(currentMarker.Tag.ToString().Replace("grid", ""))-1;

            int firstIndex = 0, secondIndex = 0;

            if (currentIndex == 0)
            {
                firstIndex = currentIndex + 1;
                secondIndex = mapPolygon.Points.Count - 1;
            }
            else if (currentIndex == mapPolygon.Points.Count - 1)
            {
                firstIndex = 0;
                secondIndex = currentIndex - 1;
            }
            else {
                firstIndex = currentIndex + 1;
                secondIndex = currentIndex-1;
            }

            PointLatLng firstPoint = new PointLatLng((mapPolygon.Points[firstIndex].Lat+ mapPolygon.Points[currentIndex].Lat)/2, (mapPolygon.Points[firstIndex].Lng + mapPolygon.Points[currentIndex].Lng) / 2);
            PointLatLng secondPoint = new PointLatLng((mapPolygon.Points[secondIndex].Lat + mapPolygon.Points[currentIndex].Lat) / 2, (mapPolygon.Points[secondIndex].Lng + mapPolygon.Points[currentIndex].Lng) / 2);


            InsertPoint(gMapOverlay, secondPoint, currentIndex);
            InsertPoint(gMapOverlay, firstPoint, currentIndex + 2);

            mapPolygon.Points.Insert(currentIndex, secondPoint);
            mapPolygon.Points.Insert(currentIndex + 2, firstPoint);

            currentMarker.ToolTipText += "changed";

            for (int index=0;index<gMapOverlay.Markers.Count;index++)
            {
                gMapOverlay.Markers[index].Tag = "grid"+(index+1);
            }


        }

        private void InsertPoint(GMapOverlay gMapOverlay,PointLatLng pointLatLng,int index)
        {

            GMarkerGoogle m = new GMarkerGoogle(pointLatLng, GMarkerGoogleType.red);

            m.ToolTipMode = MarkerTooltipMode.Never;
            m.ToolTipText = "grid" + (index + 1);
            m.Tag = "grid" + (index + 1);
            gMapOverlay.Markers.Insert(index,m);
        }


        private void AddKeyPoint(GMapOverlay mapPolygon, PointLatLng tempPoint)
        {
            try
            {

                GMarkerGoogle m = new GMarkerGoogle(tempPoint, GMarkerGoogleType.red);
               
                m.ToolTipMode = MarkerTooltipMode.Never;
                m.ToolTipText = "grid" + (mapPolygon.Markers.Count + 1);
                m.Tag = "grid" + (mapPolygon.Markers.Count + 1);

                //GMapMarkerRect mBorders = new GMapMarkerRect(tempPoint);
                //{
                //    mBorders.InnerMarker = m;
                //}

                mapPolygon.Markers.Add(m);
               // mapPolygon.Markers.Add(mBorders);
            }
            catch (Exception ex)
            {
                //log.Info(ex.ToString());
            }

        }

    }
}
