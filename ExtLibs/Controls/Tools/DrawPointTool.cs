using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Controls.MapMarkers;

namespace MissionPlanner.Controls.Tools
{
    public class DrawPointTool:IMapTool
    {

        GMapMarker currentMarker;

        GMapOverlay gMapOverlay;

        bool isDrawing = false;

        bool isDraging = false;

       // event OnFinished OnFinishedEvent;

        public DrawPointTool() {

            gMapOverlay = new GMapOverlay("gMapPolygon");

        }

        //event OnFinished IMapTool.OnFinishedEvent
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public myGMAP MapControl { get ; set ; }

        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Cursor Cursor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            throw new NotImplementedException();
        }

        public bool DoMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {

           

            if (mouseEventArgs.Button==MouseButtons.Left&& isDrawing&& currentMarker == null) {
               
                PointLatLng tempPoint = MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);

                try
                {

                    GMarkerGoogle m = new GMarkerGoogle(tempPoint, GMarkerGoogleType.red);

                    m.ToolTipMode = MarkerTooltipMode.Never;
                    m.ToolTipText = "grid" + gMapOverlay.Markers.Count + 1;
                    m.Tag = "grid" + gMapOverlay.Markers.Count + 1;

                    GMapMarkerRect mBorders = new GMapMarkerRect(tempPoint);
                    {
                        mBorders.InnerMarker = m;
                    }

                    gMapOverlay.Markers.Add(m);
                    //gMapOverlay.Markers.Add(mBorders);
                    this.MapControl.UpdateMarkerLocalPosition(m);
                }
                catch (Exception ex)
                {
                    //log.Info(ex.ToString());
                }
            }

            if (mouseEventArgs.Button == MouseButtons.Right) {
                isDrawing = false;
                isDraging = false;
                currentMarker = null;
            }
            return true;
        }


        public bool DoMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }
        public void DoMouseEnter(GMapMarker obj)
        {
            if (obj is GMarkerGoogle) currentMarker = obj as GMarkerGoogle;

              
        }

        public bool DoMouseHover(PointLatLng mapPosition)
        {
            throw new NotImplementedException();
        }

        public void DoMouseLeave(GMapMarker obj)
        {
            //if (obj is GMarkerGoogle) currentMarker = null;
            if(isDrawing)  currentMarker = null;
           
        }

        public bool DoMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Left && currentMarker!=null) {
              
                 currentMarker.Position= MapControl.FromLocalToLatLng(mouseEventArgs.X, mouseEventArgs.Y);
            }         


            return true;
        }

        public bool DoMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            isDraging = false;

           
            currentMarker = null;

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
            isDrawing = true;

            if (!MapControl.Overlays.Contains(gMapOverlay)) MapControl.Overlays.Add(gMapOverlay);
            
        }
    }
}
