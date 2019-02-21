using System;

namespace GMap.NET.MapProviders
{
	public class TiandituMapProvider : TiandituMapProviderBase
	{
		public static readonly TiandituMapProvider Instance;

		private readonly Guid id = new Guid("98164035-6921-4c14-bd22-27d401416d8b");

		private readonly string name = "天地图矢量地图";

		private GMapProvider[] overlays;

		private static readonly string UrlFormatServer;

		private static readonly string UrlFormatRequest;

		private static readonly string UrlFormat;

		public override Guid Id
		{
			get
			{
				return this.id;
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
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
						this,
						TiandituCvaMapProvider.Instance
					};
				}
				return this.overlays;
			}
		}

		private TiandituMapProvider()
		{
		}

		static TiandituMapProvider()
		{
			TiandituMapProvider.UrlFormatServer = "DataServer";
			TiandituMapProvider.UrlFormatRequest = "vec_w";
			TiandituMapProvider.UrlFormat = "http://t{0}.tianditu.com/{1}?T={2}&x={3}&y={4}&l={5}";
			TiandituMapProvider.Instance = new TiandituMapProvider();
		}

		public override PureImage GetTileImage(GPoint pos, int zoom)
		{
			string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
			return base.GetTileImageUsingHttp(url);
		}

		private string MakeTileImageUrl(GPoint pos, int zoom, string language)
		{
			return string.Format(TiandituMapProvider.UrlFormat, new object[]
			{
				"1",
				TiandituMapProvider.UrlFormatServer,
				TiandituMapProvider.UrlFormatRequest,
				pos.X,
				pos.Y,
				zoom
			});
		}
	}
}
