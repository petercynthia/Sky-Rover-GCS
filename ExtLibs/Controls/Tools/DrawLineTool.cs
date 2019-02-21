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
    public delegate void OnDrawRouteFinished(GMapRoute gMapRoute, GMapMarker gMapMarker);

    public delegate void OnWayPointDoubleClicked(GMapMarker gMapMarker);

    public delegate void OnWayPointClicked(GMapMarker gMapMarker);

    public delegate void OnWayPointMoved(GMapRoute gMapRoute, GMapMarker gMapMarker);

    public delegate void OnMouseUp(GMapRoute gMapRoute, GMapMarker gMapMarker);

    public class DrawLineTool : IMapTool
    {
        public event OnDrawRouteFinished OnDrawRouteFinishedEvent;

        public event OnWayPointDoubleClicked OnWayPointDoubleClickedEvent;

        public event OnWayPointClicked OnWayPointClickedEvent;

        public event OnWayPointMoved OnWayPointMovedEvent;

        public event OnMouseUp OnMouseUpEvent;

        GMapMarker currentMarker;

        GMapRoute gMapRoute;
        

        GMapOverlay gMapOverlay;

        Size selSize = new Size(40, 40);

        Size unSelSize = new Size(32, 32);

        bool isDrawing = false;

        bool isDragMarker = false;

        /// <summary>
        /// 
        /// </summary>
        public DrawLineTool()
        {

            if (gMapOverlay == null) gMapOverlay = new GMapOverlay("WPMapOverlay");

            if (gMapRoute == null) gMapRoute = new GMapRoute(new List<PointLatLng>(), "tempPolygon");
            gMapRoute.Stroke = new Pen(Color.Red, 2);
          
            gMapOverlay.Routes.Clear();
            gMapOverlay.Routes.Add(gMapRoute);
        }

        public myGMAP MapControl { get; set; }

        public string Name => "绘制线段";

        public string Description => "鼠标左键点击地图，绘制线，至少绘制两个点，点击右键结束绘制";

        private bool enabled;
        public bool Enabled { get => enabled; set => enabled = value; }

        private Cursor cursor;
        public Cursor Cursor { get => cursor; set => cursor = value; }// throw new NotImplementedException(); }

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
            if (OnWayPointDoubleClickedEvent != null&&currentMarker!=null) {

                OnWayPointDoubleClickedEvent(currentMarker);
            }

            return true;
        }


        public bool DoMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return true;
            if (OnWayPointClickedEvent!=null&&currentMarker!=null)
            {
                OnWayPointClickedEvent(currentMarker);
            }
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

           
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                if (isDrawing)
                {
                    PointLatLng tempPoint = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);
                    gMapRoute.Points.Add(tempPoint);
                    
                    int count = gMapRoute.Points.Count;
                    if (count >= 2) {

                        AddVirualKeyPoint(gMapOverlay,new List<PointLatLng>{gMapRoute.Points[count-1], gMapRoute.Points[count-2]});
                    }
                    AddKeyPoint(gMapOverlay, tempPoint);
                    MapControl.UpdateRouteLocalPosition(gMapRoute);
                    MapControl.Invalidate(true);
                    return true;
                }
                if (currentMarker != null)
                {
                    isDragMarker = true;
                    MapControl.CanDragMap = false;
                    return true;
                }

                if (!isDragMarker && !isDrawing) MapControl.CanDragMap = true;

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
            if (isDragMarker && isMouseDown)
            {
                this.MapControl.OnMarkerEnter -= DoMouseEnter;
                this.MapControl.OnMarkerLeave -= DoMouseLeave;

                MapControl.CanDragMap = false;

               
                if (currentMarker.Tag.ToString().Contains("grid"))
                {
                    currentMarker.Position = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);

                    int pointIndex = int.Parse(currentMarker.Tag.ToString().Replace("grid", ""));
                    int routeIndex = pointIndex % 2;

                    if (routeIndex != 0)
                    {
                        gMapRoute.Points[(pointIndex + 1)/2-1] =
                                                  new PointLatLng(currentMarker.Position.Lat, currentMarker.Position.Lng);
                        var markers = gMapOverlay.Markers;
                        if (pointIndex == 1)
                            markers[pointIndex].Position = new PointLatLng((currentMarker.Position.Lat + gMapOverlay.Markers[pointIndex + 1].Position.Lat) / 2, (currentMarker.Position.Lng + gMapOverlay.Markers[pointIndex + 1].Position.Lng) / 2);

                        else if (pointIndex > 1 && pointIndex < markers.Count)
                        {
                            markers[pointIndex - 2].Position = new PointLatLng((currentMarker.Position.Lat + markers[pointIndex - 3].Position.Lat) / 2, (currentMarker.Position.Lng + markers[pointIndex - 3].Position.Lng) / 2);
                            markers[pointIndex].Position = new PointLatLng((currentMarker.Position.Lat + markers[pointIndex +1].Position.Lat) / 2, (currentMarker.Position.Lng + markers[pointIndex +1].Position.Lng) / 2);

                        }
                        else if (pointIndex==markers.Count) markers[pointIndex - 2].Position = new PointLatLng((currentMarker.Position.Lat + markers[pointIndex - 3].Position.Lat) / 2, (currentMarker.Position.Lng + markers[pointIndex - 3].Position.Lng) / 2);
                        
                        
                        //鼠标移动后的事件回调
                        if (OnWayPointMovedEvent != null) OnWayPointMovedEvent(gMapRoute,currentMarker);

                    }

                    this.MapControl.UpdateRouteLocalPosition(gMapRoute);
                }
                else if(currentMarker.Tag.ToString().Contains("H"))
                {
                    currentMarker.Position = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);


                    //更新home点位置及连线

                    var lyrs = MapControl.Overlays.Where(a => a.Id == "WPMapOverlay").ToList();
                    if (lyrs.Count ==1)
                    {
                        //lyrs[0].Markers[0].Position = currentMarker.Position;
                        foreach (var lyr in lyrs)
                        {
                            var homeRoute = lyr.Routes.Where(b => b.Name == "home route").ToList();
                            if (homeRoute.Count == 0) return false;
                            homeRoute[0].Points[1] = currentMarker.Position;
                            this.MapControl.UpdateRouteLocalPosition(homeRoute[0]);
                            if (homeRoute.Count > 1) homeRoute.RemoveRange(2, homeRoute.Count - 1);
                            break;
                        }
                    }

                }



                this.MapControl.Invalidate();

            }
            else if (isDrawing)
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

            if (mouseEventArgs.Button == MouseButtons.Right)
            {
                if (isDrawing)
                {
                    isDrawing = false;
                    if (OnDrawRouteFinishedEvent != null) OnDrawRouteFinishedEvent(gMapRoute, currentMarker);
                }

                //isDrawing = false;
                //currentMarker = null;

                return true;
            }


            if (isDragMarker&&currentMarker!=null&&currentMarker.Tag.ToString().Contains("changed"))
            {
                isDragMarker = false;
                if (OnMouseUpEvent != null)
                {
                    currentMarker.Tag=currentMarker.Tag.ToString().Replace("changed","selected");

                    OnMouseUpEvent(gMapRoute, currentMarker);
                }
                
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
            if (isDrawing) return;

            if (item.Tag == null) return;

            if ((item.Tag.ToString().Contains("grid") || (item.Tag.ToString().Contains("H"))) && (item is GMarkerGoogle))
            {

                currentMarker = item as GMapMarker;
                currentMarker.Size = selSize;
                Cursor = Cursors.SizeAll;
                MapControl.CanDragMap = false;
                isDragMarker = true;
            }
            else {
                currentMarker = item as GMapMarker;
                isDragMarker = false;
            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void DoMouseLeave(GMapMarker item)
        {
            if (isDrawing) return;

            if (item is GMarkerGoogle)
            {
                (item as GMapMarker).Size = unSelSize;

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
            isDrawing = true;


            foreach (var lyr in MapControl.Overlays)
            {
                if (lyr.Id == "WPMapOverlay")
                {
                    this.MapControl.Overlays.Remove(lyr);
                    break;
                }
            }

            this.MapControl.Overlays.Add(gMapOverlay);

            //this.Cursor = Cursors.Cross;
        }

        private void ClearOverlay(GMapOverlay polygons)
        {


            polygons.Routes.Clear();
            polygons.Markers.Clear();
            polygons.Polygons.Clear();


        }


        private void InsertKeyPoint(GMapRoute mapRoute, GMapMarker currentMarker)
        {

            if (currentMarker.Tag.ToString().Contains("H")) return;

            if (currentMarker.ToolTipText.Contains("changed")) return;

            if (mapRoute == null || mapRoute.Points.Count < 2) return;



            int currentIndex = int.Parse(currentMarker.Tag.ToString().Replace("grid", "")) - 1;

            int firstIndex = 0, secondIndex = 0;

            if (currentIndex == 0)
            {
                firstIndex = currentIndex + 1;
                secondIndex = mapRoute.Points.Count - 1;
            }
            else if (currentIndex == mapRoute.Points.Count - 1)
            {
                firstIndex = 0;
                secondIndex = currentIndex - 1;
            }
            else
            {
                firstIndex = currentIndex + 1;
                secondIndex = currentIndex - 1;
            }

            PointLatLng firstPoint = new PointLatLng((mapRoute.Points[firstIndex].Lat + mapRoute.Points[currentIndex].Lat) / 2, (mapRoute.Points[firstIndex].Lng + mapRoute.Points[currentIndex].Lng) / 2);
            PointLatLng secondPoint = new PointLatLng((mapRoute.Points[secondIndex].Lat + mapRoute.Points[currentIndex].Lat) / 2, (mapRoute.Points[secondIndex].Lng + mapRoute.Points[currentIndex].Lng) / 2);


            InsertPoint(gMapOverlay, secondPoint, currentIndex);
            InsertPoint(gMapOverlay, firstPoint, currentIndex + 2);

            mapRoute.Points.Insert(currentIndex, secondPoint);
            mapRoute.Points.Insert(currentIndex + 2, firstPoint);

            currentMarker.ToolTipText += "changed";

            for (int index = 0; index < gMapOverlay.Markers.Count; index++)
            {
                gMapOverlay.Markers[index].Tag = "grid" + (index + 1);
            }


        }

        private void InsertPoint(GMapOverlay gMapOverlay, PointLatLng pointLatLng, int index)
        {

            GMarkerGoogle m = new GMarkerGoogle(pointLatLng, GMarkerGoogleType.waypoint_normal);

            m.ToolTipMode = MarkerTooltipMode.Never;
            m.ToolTipText = "grid" + (index + 1);
            m.Tag = "grid" + (index + 1);
            gMapOverlay.Markers.Insert(index, m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapPolygon"></param>
        /// <param name="tempPoint"></param>
        private void AddVirualKeyPoint(GMapOverlay mapPolygon,List<PointLatLng> tempPoints)
        {
            try
            {
                //double[] p1=Coords

                GMarkerGoogle m = new GMarkerGoogle(new PointLatLng((tempPoints[0].Lat+ tempPoints[1].Lat)/2, (tempPoints[1].Lng+ tempPoints[0].Lng)/2), GMarkerGoogleType.green);

                m.ToolTipMode = MarkerTooltipMode.Never;
                m.ToolTipText = "changed" + (mapPolygon.Markers.Count + 1);
                m.Tag = "changed" + (mapPolygon.Markers.Count + 1);
                //m.IsVisible = false;
                mapPolygon.Markers.Add(m);
               
            }
            catch (Exception ex)
            {
                //log.Info(ex.ToString());
            }


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
