using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsForms.Markers;

namespace GMap.NET.Core.GMap.NET.WindowsForms.Markers
{
    using System.Drawing;
    using System.Runtime.Serialization;
    using System;
    using global::GMap.NET.WindowsForms;

    public class GMapMarkerTile : GMapMarker
    {
        static Brush Fill = new SolidBrush(Color.FromArgb(155, Color.Blue));

        public GMapMarkerTile(PointLatLng p, int size) : base(p)
        {
            Size = new System.Drawing.Size(size, size);
        }

        public override void OnRender(Graphics g)
        {
            g.FillRectangle(Fill, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height));
        }
    }
}
