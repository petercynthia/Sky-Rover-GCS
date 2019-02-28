using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Controls;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using System.Threading;
using GMap.NET.Core.GMap.NET.WindowsForms.Markers;
using GMap.NET.Internals;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace SKYROVER.GCS.DeskTop.MenuItems.GroundControlStation
{
    public partial class OfflineMap : UserControl
    {
        myGMAP mMapControl;
        public myGMAP MMapControl { get => mMapControl; set => mMapControl = value; }
        GMapOverlay progressLayer;
        public GMapOverlay ProgressLayer { get => progressLayer; set => progressLayer = value; }

        BackgroundWorker worker = new BackgroundWorker();

        public OfflineMap()
        {
            InitializeComponent();

            GMaps.Instance.OnTileCacheComplete += new TileCacheComplete(OnTileCacheComplete);
            //GMaps.Instance.OnTileCacheStart += new TileCacheStart(OnTileCacheStart);
            //GMaps.Instance.OnTileCacheProgress += new TileCacheProgress(OnTileCacheProgress);

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
           
          

        }

        void OnTileCacheComplete()
        {

            Debug.WriteLine("OnTileCacheComplete");
            long size = 0;
            int db = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(mMapControl.CacheLocation);
                var dbs = di.GetFiles("*.gmdb", SearchOption.AllDirectories);
                foreach (var d in dbs)
                {
                    size += d.Length;
                    db++;
                }
            }
            catch
            {
            }

            if (!IsDisposed)
            {
                MethodInvoker m = delegate
                {
                    textBoxMemory.Text = string.Format(CultureInfo.InvariantCulture, "{0} db in {1:00} MB", db, size / (1024.0 * 1024.0));
                  
                };

                if (!IsDisposed)
                {
                    try
                    {
                        Invoke(m);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }

        void OnTileCacheStart()
        {
           
                done.Reset();

                MethodInvoker m = delegate
                {
                    lblStatus.Text = "开始下载数据...";
                };
                try
                {
                    Invoke(m);
                }
                catch
                {
                }
           
        }

        void OnTileCacheProgress(int left)
        {
           
                MethodInvoker m = delegate
                {
                    lblStatus.Text = left + "数据正在存储...";
                };
                try
                {
                    Invoke(m);
                }
                catch
                {
                }
          
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           

            list.Clear();

            GMaps.Instance.UseMemoryCache = true;
            GMaps.Instance.CacheOnIdleRead = true;
            GMaps.Instance.BoostCacheEngine = false;
            lock (this)  Overlay.Markers.Clear();

            if (!e.Cancelled)
            {

                this.lblStatus.Text = "下载完成";
                this.prgDownload.Value = 0;
            }
            else if (e.Cancelled) {
                this.lblStatus.Text = "取消下载";
                this.prgDownload.Value = 0;
            }

            if (e.Error!=null) {
                Type errorType = e.Error.GetType();
                this.lblStatus.Text = errorType.Name;
            }


          
        }

        bool CacheTiles(int zoom, GPoint p)
        {
            foreach (var pr in provider.Overlays)
            {
                Exception ex;
                PureImage img;

                // tile number inversion(BottomLeft -> TopLeft)
                if (pr.InvertedAxisY)
                {
                    img = GMaps.Instance.GetImageFrom(pr, new GPoint(p.X, maxOfTiles.Height - p.Y), zoom, out ex);
                }
                else // ok
                {
                    img = GMaps.Instance.GetImageFrom(pr, p, zoom, out ex);
                }

                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public readonly Queue<GPoint> CachedTiles = new Queue<GPoint>();

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // int zoom=(int)e.Result;

            int count = Tilescount(area,zoom);
            int curCount = 0;

            MethodInvoker m = delegate
            {
                prgDownload.Maximum = count;
            };


            Invoke(m);
           
           

            for (int currentZoomLevel = 1; currentZoomLevel <= zoom; currentZoomLevel++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (list != null)
                {
                    list.Clear();
                    list = null;
                }
                list = provider.Projection.GetAreaTileList(area, currentZoomLevel, 0);

                maxOfTiles = provider.Projection.GetTileMatrixMaxXY(currentZoomLevel);

                

                //瓦片数量
                all = list.Count;

                int countOk = 0;
                int retryCount = 0;


                if (Shuffle)
                {
                    Stuff.Shuffle<GPoint>(list);
                }

                lock (this)
                {
                    CachedTiles.Clear();
                    Overlay.Markers.Clear();
                }
                for (int i = 0; i < all; i++)
                {

                    if (worker.CancellationPending == true) {
                        e.Cancel = true;
                            break;
                    }
                        
                   
                    GPoint p = list[i];
                    {
                        if (CacheTiles(currentZoomLevel, p))
                        { 
                            if (Overlay != null)
                            {
                                lock (this)
                                {
                                    CachedTiles.Enqueue(p);
                                }
                            }
                            curCount += 1;
                            worker.ReportProgress(curCount, (float)curCount/count);
                            countOk++;
                            retryCount = 0;
                        }
                        else
                        {
                            if (++retryCount <= retry) // retry only one
                            {
                                i--;
                                System.Threading.Thread.Sleep(1111);
                                continue;
                            }
                            else
                            {
                                retryCount = 0;
                            }
                        }
                    }

                    if (sleep > 0)
                    {
                        System.Threading.Thread.Sleep(sleep);
                    }
                }
            }

           

           
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblStatus.Text = "完成: " +((float)e.UserState*100).ToString("f2")+ "%";
            this.prgDownload.Value = e.ProgressPercentage;

            if (Overlay != null)
            {
                GPoint? l = null;

                lock (this)
                {
                    if (CachedTiles.Count > 0)
                    {
                        l = CachedTiles.Dequeue();
                    }
                }

                if (l.HasValue)
                {
                    var px = Overlay.Control.MapProvider.Projection.FromTileXYToPixel(l.Value);
                    var p = Overlay.Control.MapProvider.Projection.FromPixelToLatLng(px, zoom);

                    var r1 = Overlay.Control.MapProvider.Projection.GetGroundResolution(zoom, p.Lat);
                    var r2 = Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, p.Lat);
                    var sizeDiff = r2 / r1;

                    GMapMarkerTile m = new GMapMarkerTile(p, (int)(Overlay.Control.MapProvider.Projection.TileSize.Width / sizeDiff));
                    Overlay.Markers.Add(m);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        private int Tilescount(RectLatLng rectangle,int zoom) {
            int count = 0;
            for (int index=1;index<=zoom;index++) {
                count+= provider.Projection.GetAreaTileList(rectangle, index, 0).Count;
            }
            return count;
        }

        private void btnDrawarea_Click(object sender, EventArgs e)
        {
            RectLatLng area = mMapControl.SelectedArea;
            int zoomLevel = 16;
            if (noramlMap.Checked) zoomLevel = 16;
            else if (moreDetailMap.Checked) zoomLevel = 18;

            downloadCacheMap(area,zoomLevel);
        }       
        List<GPoint> list;
        int zoom;
        GMapProvider provider;
        int sleep;
        int all;
     
        RectLatLng area;
        GMap.NET.GSize maxOfTiles;
        public GMapOverlay Overlay;
        int retry;
        bool Shuffle = true;
       
        readonly AutoResetEvent done = new AutoResetEvent(true);

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="zoomLevel"></param>
        private void downloadCacheMap(RectLatLng area,int zoomLevel) {

            if (area.IsEmpty) {
                MessageBox.Show("按住Alt键绘制地图区域", "绘制范围", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.btnCancle.Enabled = true;

            var lyrs = mMapControl.Overlays.Where(a => a.Id == "cacheLayer").ToList();
            if (lyrs.Count == 1) this.Overlay = lyrs[0]; // set overlay if you want to see cache progress on the map

            //开始下载数据
            this.prgDownload.Maximum = zoomLevel;
            this.Start(area, zoomLevel, mMapControl.MapProvider, mMapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 100, mMapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 1);



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="zoom"></param>
        /// <param name="provider"></param>
        /// <param name="sleep"></param>
        /// <param name="retry"></param>
        private void Start(RectLatLng area, int zoom, GMapProvider provider, int sleep, int retry)
        {
            if (!worker.IsBusy)
            {
                this.lblStatus.Text = "...";
                this.prgDownload.Value = 0;

                this.area = area;
                this.zoom = zoom;
                this.provider = provider;
                this.sleep = sleep;
                this.retry = retry;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;
                GMaps.Instance.BoostCacheEngine = true;

                if (Overlay != null)
                {
                    Overlay.Markers.Clear();
                }

                worker.RunWorkerAsync(zoom);

            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if(worker!=null)
                worker.CancelAsync();
            this.btnCancle.Enabled = false;
        }
    }
}
