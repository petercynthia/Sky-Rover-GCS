using GMap.NET.Projections;
using System;

namespace GMap.NET.MapProviders
{
	public class TiandituCvaMapProvider : TiandituMapProviderBase
	{
		public static readonly TiandituCvaMapProvider Instance;

		private readonly Guid id = new Guid("97ef8ced-4096-45d2-81de-305235b9d1fe");

		private readonly string name = "TiandituCvaMap";

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

		private TiandituCvaMapProvider()
		{
		}

		static TiandituCvaMapProvider()
		{
			TiandituCvaMapProvider.UrlFormatServer = "DataServer";
			TiandituCvaMapProvider.UrlFormatRequest = "cva_w";
			TiandituCvaMapProvider.UrlFormat = "http://t{0}.tianditu.com/{1}?T={2}&x={3}&y={4}&l={5}";
			TiandituCvaMapProvider.Instance = new TiandituCvaMapProvider();
		}

		public override PureImage GetTileImage(GPoint pos, int zoom)
		{
			string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
			return base.GetTileImageUsingHttp(url);
		}

        public override PureProjection Projection
        {
            get
            {
                return MercatorProjectionGCJ.Instance;
            }
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
		{
			return string.Format(TiandituCvaMapProvider.UrlFormat, new object[]
			{
				"5",
				TiandituCvaMapProvider.UrlFormatServer,
				TiandituCvaMapProvider.UrlFormatRequest,
				pos.X,
				pos.Y,
				zoom
			});
		}
	}
}
