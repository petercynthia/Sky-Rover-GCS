using MissionPlanner.Controls.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MissionPlanner.Controls
{
    /// <summary>
    /// Mono handles calls from other thread difrently - this prevents those crashs
    /// </summary>
    public class myGMAP : GMap.NET.WindowsForms.GMapControl
    {


        public bool inOnPaint = false;
        string otherthread = "";
        int lastx = 0;
        int lasty = 0;
        public long ElapsedMilliseconds;

        private int counter;
        readonly Font DebugFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular);
        readonly Font DebugFontSmall = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
        DateTime start;
        DateTime end;
        int delta;

        public myGMAP()
            : base()
        {
            this.Text = "MapControl";
            this.MouseDown += MyGMAP_MouseDown;
            this.MouseMove += MyGMAP_MouseMove;
            this.MouseUp += MyGMAP_MouseUp;
            this.MouseDoubleClick += MyGMAP_MouseDoubleClick;
            this.MouseClick += MyGMAP_MouseClick;

            this.OnMarkerEnter += MyGMAP_OnMarkerEnter;
            this.OnMarkerLeave += MyGMAP_OnMarkerLeave;


        }

        private void MyGMAP_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.currentTool != null) this.currentTool.DoMouseClick(sender, e);
        }

        private void MyGMAP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.currentTool != null) this.currentTool.DoMouseDoubleClick(sender,e);
        }

        private void MyGMAP_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.currentTool != null)
                this.currentTool.DoMouseUp(sender, e);
        }

        private void MyGMAP_OnMarkerLeave(GMap.NET.WindowsForms.GMapMarker item)
        {
            if (this.currentTool != null)
                this.currentTool.DoMouseLeave(item);
        }

        private void MyGMAP_OnMarkerEnter(GMap.NET.WindowsForms.GMapMarker item)
        {
            if (this.currentTool != null)
                this.currentTool.DoMouseEnter(item);
        }


        private void MyGMAP_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.currentTool != null)
                this.currentTool.DoMouseMove(sender, e);
        }


        private void MyGMAP_MouseDown(object sender, MouseEventArgs e)
        {
          
            if (this.currentTool != null)
                this.currentTool.DoMouseDown(sender, e);
        }



        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
             start = DateTime.Now;



            if (inOnPaint)
            {
                Console.WriteLine("Was in onpaint Gmap th:" + System.Threading.Thread.CurrentThread.Name + " in " + otherthread);
                return;
            }

            otherthread = System.Threading.Thread.CurrentThread.Name;

            inOnPaint = true;

            try
            {
                base.OnPaint(e);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            inOnPaint = false;

            end = DateTime.Now;

            delta =  (int)(end - start).TotalMilliseconds;

            System.Diagnostics.Debug.WriteLine("map draw time " + delta);
        }


        protected override void OnPaintOverlays(Graphics g)
        {
            base.OnPaintOverlays(g);
           
            g.DrawString(string.Format(CultureInfo.InvariantCulture, "{0:0.0}", Zoom) + "z, " + MapProvider + ", refresh: " + counter++ + ", load: " + ElapsedMilliseconds + "ms, render: " + delta + "ms", DebugFont, Brushes.Red, DebugFont.Height, this.Height-40);

        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                var buffer = 1;
                // try prevent alot of cpu usage
                if (e.X >= lastx - buffer && e.X <= lastx + buffer && e.Y >= lasty - buffer && e.Y <= lasty + buffer)
                    return;

                if (HoldInvalidation)
                    return;

                lastx = e.X;
                lasty = e.Y;

                base.OnMouseMove(e);

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }



        #region private var
        private IMapTool currentTool;

        public IMapTool CurrentTool
        {

            get { return currentTool; }
            set
            {
                if (value != null)
                {
                    currentTool = value;
                    currentTool.MapControl = this;
                }


            }
        }

        #endregion

        private void SetCursor()
        {



        }


        #region Tools enumerator

        /// <summary>
        /// Map tools enumeration
        /// </summary>
        public enum Tools
        {
            /// <summary>
            /// Pan
            /// </summary>
            Pan,

            /// <summary>
            /// Zoom in
            /// </summary>
            ZoomIn,

            /// <summary>
            /// Zoom out
            /// </summary>
            ZoomOut,

            /// <summary>
            /// Query bounding boxes for intersection
            /// </summary>
            QueryBox,

            /// <summary>
            /// Query tool
            /// </summary>
            [Obsolete("Use QueryBox")]
            Query = QueryBox,

            /// <summary>
            /// Attempt true intersection query on geometry
            /// </summary>
            QueryPoint,

            /// <summary>
            /// Attempt true intersection query on geometry
            /// </summary>
            [Obsolete("Use QueryPoint")]
            QueryGeometry = QueryPoint,

            ///// <summary>
            ///// Attempt true intersection query on polygonal geometry
            ///// </summary>
            //QueryPolygon,

            /// <summary>
            /// Zoom window tool
            /// </summary>
            ZoomWindow,

            /// <summary>
            /// Define Point on Map
            /// </summary>
            DrawPoint,

            /// <summary>
            /// Define Line on Map
            /// </summary>
            DrawLine,

            /// <summary>
            /// Define Polygon on Map
            /// </summary>
            DrawPolygon,

            /// <summary>
            /// No active tool
            /// </summary>
            None,

            /// <summary>
            /// Custom tool, implementing <see cref="IMapTool"/>
            /// </summary>
            Custom
        }
        #endregion
    }
}
