namespace GMap.NET.MapProviders
{
    using GMap.NET;
    using GMap.NET.Projections;
    using System;
    public abstract class AMapProviderBase : GMapProvider
    {
        private GMapProvider[] overlays;

        public AMapProviderBase()
        {
            this.MaxZoom = null;
            base.RefererUrl = "http://www.amap.com/";
            base.Copyright = string.Format("©{0} 高德软件 Image© DigitalGlobe & chinasiwei | AIRBUS & EastDawn", DateTime.Today.Year);
        }

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { this };
                }
                return this.overlays;
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return MercatorProjectionGCJ.Instance;
            }
        }
    }
}
