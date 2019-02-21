using GMap.NET.Projections;
using System;

namespace GMap.NET.MapProviders
{
	public class TiandituCiaMapProvider : TiandituMapProviderBase
	{
		public static readonly TiandituCiaMapProvider Instance;

		private readonly Guid id = new Guid("ca4226b1-9b58-4914-8734-1be34ee422da");

		private readonly string name = "TiandituCvaMap";

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

		private TiandituCiaMapProvider()
		{
		}

        public override PureProjection Projection
        {
            get
            {
                return MercatorProjectionGCJ.Instance;
            }
        }

        static TiandituCiaMapProvider()
		{
			TiandituCiaMapProvider.UrlFormatRequest = "cia";
			TiandituCiaMapProvider.UrlFormat = "http://t{0}.tianditu.cn/{1}_w/wmts?service=wmts&request=GetTile&version=1.0.0&LAYER={1}&tileMatrixSet=w&TileMatrix={4}&TileRow={3}&TileCol={2}&style=default&format=tiles";
			TiandituCiaMapProvider.Instance = new TiandituCiaMapProvider();
		}

		public override PureImage GetTileImage(GPoint pos, int zoom)
		{
			string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
			return base.GetTileImageUsingHttp(url);
		}

		private string MakeTileImageUrl(GPoint pos, int zoom, string language)
		{
			return string.Format(TiandituCiaMapProvider.UrlFormat, new object[]
			{
				"5",
				TiandituCiaMapProvider.UrlFormatRequest,
				pos.X,
				pos.Y,
				zoom
			});
		}
	}
}
