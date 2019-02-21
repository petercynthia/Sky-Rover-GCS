using GMap.NET.Projections;
using System;

namespace GMap.NET.MapProviders
{
	public abstract class TiandituMapProviderBase : GMapProvider
	{
		private GMapProvider[] overlays;

		public override Guid Id
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override PureProjection Projection
		{
			get
			{
				return MercatorProjectionGCJ.Instance;
			}
		}

		public override GMapProvider[] Overlays
		{
			get
			{
				if (this.overlays == null)
				{
					this.overlays = new GMapProvider[]
					{
						this
					};
				}
				return this.overlays;
			}
		}

		public TiandituMapProviderBase()
		{
			this.RefererUrl = "http://www.tianditu.com/";
			this.MaxZoom = new int?(18);
		}

		public override PureImage GetTileImage(GPoint pos, int zoom)
		{
			throw new NotImplementedException();
		}
	}
}
