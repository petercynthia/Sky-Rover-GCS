using GMap.NET.Projections;
using System;

namespace GMap.NET.MapProviders
{
	public class TiandituImageProvider : TiandituImageProviderBase
	{
		public static readonly TiandituImageProvider Instance;

		private readonly Guid id = new Guid("786e7343-5b15-4959-9897-8443bc70f98c");

		private readonly string name = "ÌìµØÍ¼Ó°ÏñÍ¼²ã";

		private GMapProvider[] overlays;

		private static readonly string UrlFormatRequest;

		private static readonly string UrlFormat;

        private static readonly string tokenKey = "e6172c883b0288767a84bd3f68c6e663";

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

		private TiandituImageProvider()
		{
		}


        public override PureProjection Projection
        {
            get
            {
                return MercatorProjectionGCJ.Instance;
            }
        }


        static TiandituImageProvider()
		{
			TiandituImageProvider.UrlFormatRequest = "img";
			TiandituImageProvider.UrlFormat = "http://t{0}.tianditu.gov.cn/{1}_w/wmts?service=wmts&request=GetTile&version=1.0.0&LAYER={1}&tileMatrixSet=w&TileMatrix={4}&TileRow={3}&TileCol={2}&style=default&format=tiles&tk={5}";
			TiandituImageProvider.Instance = new TiandituImageProvider();
		}

		public override PureImage GetTileImage(GPoint pos, int zoom)
		{
			string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
			return base.GetTileImageUsingHttp(url);
		}

		private string MakeTileImageUrl(GPoint pos, int zoom, string language)
		{
			return string.Format(TiandituImageProvider.UrlFormat, new object[]
			{
				"5",
				TiandituImageProvider.UrlFormatRequest,
				pos.X,
				pos.Y,
				zoom,
                tokenKey
            });
		}
	}
}
