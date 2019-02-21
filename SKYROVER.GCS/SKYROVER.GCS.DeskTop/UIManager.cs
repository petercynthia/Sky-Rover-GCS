using GMap.NET;
using GMap.NET.MapProviders;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Controls;
using SKYROVER.GCS.DeskTop.MapTools;
using SKYROVER.GCS.DeskTop.MessagePanel;
using SKYROVER.GCS.DeskTop.Payloads;
using SKYROVER.GCS.DeskTop.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace SKYROVER.GCS.DeskTop
{
  public class UIManager
    {
        HUD mHUD;
        public HUD MHUD { get => mHUD; set => mHUD = value; }

        myGMAP mMapControl;
        public myGMAP MMapControl { get => mMapControl; set => mMapControl = value; }

        ImportantMessagePanel mQuickInfoPanel;

        QuickInfoPanel quickInfoPanel;

        public ImportantMessagePanel MQuickInfoPanel { get => mQuickInfoPanel; set => mQuickInfoPanel = value; }
        /// <summary>
        /// 
        /// </summary>
        frmVideo frmVideo;


        /// <summary>
        /// 主窗体实例
        /// </summary>
        MainUI mMainUI;

        PickBox pickBox;
        /// <summary>
        ///                                          
        /// </summary>
        /// <param name="mainUI"></param>
        public UIManager(MainUI mainUI) {
            
            if (mainUI != null) mMainUI = mainUI;

            pickBox = new PickBox();

        }

       

        #region 创建管理视频
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public frmVideo CreateVideoPanel()
        {
            if (mMapControl == null) return null;

            if (frmVideo == null) frmVideo = GenericSingleton<frmVideo>.CreateInstrance();
            frmVideo.Location = new System.Drawing.Point(2,mMainUI.bottomPanel.Top+mMainUI.bottomPanel.Height-this.frmVideo.Height-5);
            this.frmVideo.Owner = mMainUI;
            this.frmVideo.isShown = true;
            frmVideo.Show();
            return frmVideo;

           

        }

        private void PictureBox_VlcLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            // Default installation path of VideoLAN.LibVLC.Windows
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        #endregion

        #region 创建管理仪表盘
        /// <summary>
        /// 创建仪表盘
        /// </summary>
        /// <returns></returns>
        public HUD CreateHUD()
        {

            if (mHUD == null) mHUD = new HUD();
            mHUD.Size = new System.Drawing.Size(300, 300);

            if (mMapControl != null) mHUD.Parent = mMapControl;//.Controls.Add(mHUD);
            mHUD.Location = new System.Drawing.Point(2, 2);
            // mHUD.Anchor = AnchorStyles.Left;

            pickBox.WireControl(mHUD);

            return mHUD;
        }

        #endregion

        #region 创建命令按钮
       

        public CommandsControl CreateCommands() {

            CommandsControl commandsControl = new CommandsControl();
            if (mMapControl != null) commandsControl.Parent = mMapControl;
            commandsControl.Location = new System.Drawing.Point(mMapControl.Width-commandsControl.Width, mMapControl.Height-commandsControl.Height);
            commandsControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            pickBox.WireControl(commandsControl);
            return commandsControl;

        }


        #endregion


        #region 创建重要信息面板

        //public ImportantMessagePanel CreateImportantMessagePanel()
        //{

        //    if (mQuickInfoPanel == null) mQuickInfoPanel = new ImportantMessagePanel();

        //    if (mMapControl != null) mMapControl.Controls.Add(mQuickInfoPanel);
        //    mQuickInfoPanel.Location = new System.Drawing.Point(2, mMapControl.Bottom-12);
        //  //  mQuickInfoPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        //    pickBox.WireControl(mQuickInfoPanel);

        //    return mQuickInfoPanel;
        //}

        public QuickInfoPanel CreateQuickInfoPanel()
        {

            if (quickInfoPanel == null) quickInfoPanel = GenericSingleton<QuickInfoPanel>.CreateInstrance();
            quickInfoPanel.Location = new System.Drawing.Point(2, mMapControl.Height - quickInfoPanel.Height);
            quickInfoPanel.Owner = mMainUI;
            quickInfoPanel.isShown = true;
            quickInfoPanel.Visible = false;
            quickInfoPanel.Show();
               
            return quickInfoPanel;


        }



        #endregion
        readonly SQLiteIpCache IpCache = new SQLiteIpCache();
       
        #region 创建管理地图控件
        /// <summary>
        /// 创建地图控件
        /// </summary>
        /// <param name="mMapControl"></param>
        public myGMAP CreateMap()
        {

            if (mMapControl == null)
                mMapControl = new myGMAP();


            if (mMainUI != null) mMainUI.mainPanel.Controls.Add(mMapControl);

            mMapControl.Dock = System.Windows.Forms.DockStyle.Fill;



            mMapControl.DragButton = System.Windows.Forms.MouseButtons.Right;
            mMapControl.CanDragMap = true;


            mMapControl.MapProvider =  GoogleChinaHybridMapProvider.Instance;//AMapSateliteProvider.Instance;//

            mMapControl.MaxZoom = 24;
            mMapControl.MinZoom = 1;

            mMapControl.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;// ServerAndCache;
            //string cachePath = Settings.GetDataDirectory() + "gmapcache" + Path.DirectorySeparatorChar; 
            //if (cachePath != null && Directory.Exists(cachePath))
            
            // IpCache.CacheLocation = mMapControl.CacheLocation;
            Settings.Instance.Load();
            
            if (Settings.Instance["currentZoomLevel"] == null)
            {
                Settings.Instance["currentZoomLevel"] = "18";
                mMapControl.Zoom = 18;
            }
            else
                mMapControl.Zoom = int.Parse(Settings.Instance["currentZoomLevel"]);

            mMapControl.Zoom = 18;

            if (Settings.Instance["currentPosition"] == null)
            {
                Settings.Instance["currentPosition"] = "116.39,39.90";
                mMapControl.Position = new GMap.NET.PointLatLng(39.90, 116.39);
            }
            else
            {
                string[] points = Settings.Instance["currentPosition"].Split(',');
                mMapControl.Position = new GMap.NET.PointLatLng(double.Parse(points[1]), double.Parse(points[0]));
            }
            Settings.Instance.Save();

            mMapControl.MapScaleInfoEnabled = true;
            mMapControl.OnTileLoadStart += MMapControl_OnTileLoadStart;
            mMapControl.OnTileLoadComplete += MMapControl_OnTileLoadComplete;
            mMapControl.Manager.OnTileCacheStart+= new TileCacheStart(OnTileCacheStart);
            mMapControl.Manager.OnTileCacheComplete += new TileCacheComplete(OnTileCacheComplete);           
            mMapControl.Manager.OnTileCacheProgress += new TileCacheProgress(OnTileCacheProgress);

            return mMapControl;
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

            //if (!IsDisposed)
            //{
            //    MethodInvoker m = delegate
            //    {
            //        textBoxCacheSize.Text = string.Format(CultureInfo.InvariantCulture, "{0} db in {1:00} MB", db, size / (1024.0 * 1024.0));
            //        textBoxCacheStatus.Text = "all tiles saved!";
            //    };

            //    if (!IsDisposed)
            //    {
            //        try
            //        {
            //            Invoke(m);
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //}
        }

        void OnTileCacheStart()
        {
            Debug.WriteLine("OnTileCacheStart");

            //if (!IsDisposed)
            //{
            //    MethodInvoker m = delegate
            //    {
            //        textBoxCacheStatus.Text = "saving tiles...";
            //    };
            //    Invoke(m);
            //}
        }

        void OnTileCacheProgress(int left)
        {
            //if (!IsDisposed)
            //{
            //    MethodInvoker m = delegate
            //    {
            //        textBoxCacheStatus.Text = left + " tile to save...";
            //    };
            //    Invoke(m);
            //}
        }

        private void MMapControl_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            mMapControl.ElapsedMilliseconds = ElapsedMilliseconds;

            //MethodInvoker m = delegate ()
            //{
            //    panelMenu.Text = "Menu, last load in " + MainMap.ElapsedMilliseconds + "ms";

            //    textBoxMemory.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.00} MB of {1:0.00} MB", MainMap.Manager.MemoryCache.Size, MainMap.Manager.MemoryCache.Capacity);
            //};
            //try
            //{
            //    BeginInvoke(m);
            //}
            //catch
            //{
            //}
        }

        private void MMapControl_OnTileLoadStart()
        {
            //MethodInvoker m = delegate ()
            //{
            //    panelMenu.Text = "Menu: loading tiles...";
            //};
            //try
            //{
            //    BeginInvoke(m);
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// 设置地图源
        /// </summary>
        /// <param name="gMapProvider"></param>
        public void SetMapProvider(GMapProvider gMapProvider)
        {

            if (this.mMapControl != null) this.mMapControl.MapProvider = gMapProvider;
        }
        /// <summary>
        /// 更新地图home点位置
        /// </summary>
        public void updateHome()
        {
            //quickadd = true;
            updateHomeText();
            //quickadd = false;
        }

        private void updateHomeText()
        {
            // set home location
            if (MainUI.comPort.MAV.cs.HomeLocation.Lat != 0 && MainUI.comPort.MAV.cs.HomeLocation.Lng != 0)
            {               

                writeKML();
            }
        }

        /// <summary>
        /// used to write a KML, update the Map view polygon, and update the row headers
        /// </summary>
        public void writeKML()
        {
            //// quickadd is for when loading wps from eeprom or file, to prevent slow, loading times
            //if (quickadd)
            //    return;

            //updateRowNumbers();

            //PointLatLngAlt home = new PointLatLngAlt();
            //if (TXT_homealt.Text != "" && TXT_homelat.Text != "" && TXT_homelng.Text != "")
            //{
            //    try
            //    {
            //        home = new PointLatLngAlt(
            //            double.Parse(TXT_homelat.Text), double.Parse(TXT_homelng.Text),
            //            double.Parse(TXT_homealt.Text) / CurrentState.multiplieralt, "H")
            //        { Tag2 = CMB_altmode.SelectedValue.ToString() };
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //    }
            //}

            //var overlay = new WPOverlay();

            //overlay.CreateOverlay((MAVLink.MAV_FRAME)(altmode)CMB_altmode.SelectedValue, home, GetCommandList(), double.Parse(TXT_WPRad.Text) / CurrentState.multiplieralt,
            //    double.Parse(TXT_loiterrad.Text) / CurrentState.multiplieralt);

            //MainMap.HoldInvalidation = true;

            //var existing = MainMap.Overlays.Where(a => a.Id == overlay.overlay.Id).ToList();
            //foreach (var b in existing)
            //{
            //    MainMap.Overlays.Remove(b);
            //}

            //MainMap.Overlays.Insert(1, overlay.overlay);

            //overlay.overlay.ForceUpdate();

            //lbl_distance.Text = rm.GetString("lbl_distance.Text") + ": " +
            //                    FormatDistance((
            //                        overlay.route.Points.Select(a => (PointLatLngAlt)a)
            //                            .Aggregate(0.0, (d, p1, p2) => d + p1.GetDistance(p2)) +
            //                        overlay.homeroute.Points.Select(a => (PointLatLngAlt)a)
            //                            .Aggregate(0.0, (d, p1, p2) => d + p1.GetDistance(p2))) / 1000.0, false);

            //setgradanddistandaz(overlay.pointlist, home);

            //if (overlay.pointlist.Count <= 1)
            //{
            //    RectLatLng? rect = MainMap.GetRectOfAllMarkers(overlay.overlay.Id);
            //    if (rect.HasValue)
            //    {
            //        MainMap.Position = rect.Value.LocationMiddle;
            //    }

            //    MainMap_OnMapZoomChanged();
            //}

            //pointlist = overlay.pointlist;

            //MainMap.Refresh();
        }


        #endregion
    }
}
