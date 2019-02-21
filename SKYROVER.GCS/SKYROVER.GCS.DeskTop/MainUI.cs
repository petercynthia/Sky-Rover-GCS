using CellconCore;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using log4net;
using MissionPlanner;
using MissionPlanner.ArduPilot;
using MissionPlanner.Comms;
using MissionPlanner.Controls;
using MissionPlanner.Controls.MapMarkers;
using MissionPlanner.Controls.Tools;
using MissionPlanner.Maps;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Controls;
using SKYROVER.GCS.DeskTop.MapTools;
using SKYROVER.GCS.DeskTop.MenuItems;
using SKYROVER.GCS.DeskTop.MessagePanel;
using SKYROVER.GCS.DeskTop.Payloads;
using SKYROVER.GCS.DeskTop.Test;
using SKYROVER.GCS.DeskTop.Utilities;
using SKYROVER.GCS.DeskTop.Warnings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;


namespace SKYROVER.GCS.DeskTop
{
    public partial class MainUI : Form
    {

        #region 自定义类
        public abstract class MenuIcons
        {
            public abstract Image fd { get; }
            public abstract Image fp { get; }
            public abstract Image initsetup { get; }
            public abstract Image config_tuning { get; }
            public abstract Image sim { get; }
            public abstract Image terminal { get; }
            public abstract Image help { get; }
            public abstract Image donate { get; }
            public abstract Image connect { get; }
            public abstract Image disconnect { get; }
            public abstract Image bg { get; }
            public abstract Image wizard { get; }
        }

        public class BurntkermItmenuIcons : MenuIcons
        {
            

            public override Image connect
            {
                get { return global::SKYROVER.GCS.DeskTop.Properties.Resources.light_connect_icon; }
            }

            public override Image disconnect
            {
                get { return global::SKYROVER.GCS.DeskTop.Properties.Resources.light_disconnect_icon; }
            }

            public override Image fd => throw new NotImplementedException();

            public override Image fp => throw new NotImplementedException();

            public override Image initsetup => throw new NotImplementedException();

            public override Image config_tuning => throw new NotImplementedException();

            public override Image sim => throw new NotImplementedException();

            public override Image terminal => throw new NotImplementedException();

            public override Image help => throw new NotImplementedException();

            public override Image donate => throw new NotImplementedException();

            public override Image bg => throw new NotImplementedException();

            public override Image wizard => throw new NotImplementedException();

           
        }


        #endregion

        #region 私有变量
        private static readonly ILog log =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        AviWriter aviwriter;

        /// <summary>
        /// 地圖控件
        /// </summary>
        myGMAP mMapControl = null;

        public myGMAP getGMAP { get { return mMapControl; } }

        /// <summary>
        /// 仪表盘
        /// </summary>
        HUD mHUD = null;

        public HUD getHUD { get { return mHUD; } }

        ImportantMessagePanel mImportantMessagePanel;

        /// <summary>
        /// 快速信息面板
        /// </summary>
        QuickInfoPanel quickInfoPanel;

        /// <summary>
        /// 视频窗口
        /// </summary>
        frmVideo vlcControl;
        Panel videoPanel;


        //吊舱控制面板
        NacellePanel nacellePanel;

        private UIManager UIManager = null;

        DateTime connectButtonUpdate = DateTime.Now;

        /// <summary>
        /// controls the main serial reader thread
        /// </summary>
        bool serialThread = false;

        bool pluginthreadrun = false;

        bool joystickthreadrun = false;

        bool threadrun = false;


        Thread httpthread;
        Thread joystickthread;
        Thread serialreaderthread;

        Thread thisthread;

        Thread pluginthread;

        GMapMarker currentMarker; //当前飞机标识
        frmSetting setting; //设置窗体
        SurveyGrid surveyGrid;//测绘航线生成

        private Form connectionStatsForm;

        private ConnectionStats _connectionStats;

        /// <summary>
        /// store the time we first connect
        /// </summary>
        DateTime connecttime = DateTime.Now;

        DateTime nodatawarning = DateTime.Now;

        DateTime OpenTime = DateTime.Now;

        /// <summary>
        /// track the last heartbeat sent
        /// </summary>
        private DateTime heatbeatSend = DateTime.Now;

       
        private bool CameraOverlap=false;
        /// <summary>
        /// 相机拍照图层
        /// </summary>
        /// 
        internal static GMapOverlay tfrpolygons;
        public static GMapOverlay kmlpolygons;
        internal static GMapOverlay photosoverlay;
        internal static GMapOverlay geofence;
        internal static GMapOverlay rallypointoverlay;
        internal static GMapOverlay poioverlay = new GMapOverlay("POI"); // poi layer
        public static GMapOverlay homelayer;

        List<PointLatLng> trackPoints = new List<PointLatLng>();
        private Propagation prop;
        
        #endregion

        #region 公共变量


        /// <summary>
        /// joystick static class
        /// </summary>
        public static Joystick.Joystick joystick { get; set; }


        /// <summary>
        /// track last joystick packet sent. used to control rate
        /// </summary>
        DateTime lastjoystick = DateTime.Now;

        /// <summary>
        /// determine if we are running sitl
        /// </summary>
        public static bool sitl
        {
            get
            {
                if (SITL.SITLSEND == null) return false;
                if (SITL.SITLSEND.Client.Connected) return true;
                return false;
            }
        }

        public List<PointLatLngAlt> pointlist { get; set; }

        
        /// <summary>
        /// 主窗体单例实体
        /// </summary>
        public static MainUI MainUIInstance = null;
        /// <summary>
        /// 显示图标变量
        /// </summary>
        public static MenuIcons displayicons = new BurntkermItmenuIcons();
        /// <summary>
        /// This 'Control' is the toolstrip control that holds the comport combo, baudrate combo etc
        /// Otiginally seperate controls, each hosted in a toolstip sqaure, combined into this custom
        /// control for layout reasons.
        /// </summary>
        public static ConnectionControl _connectionControl;

        public static bool ShowAirports { get; set; }

        public static bool ShowTFR { get; set; }

        private adsb _adsb;

        

        public bool EnableADSB
        {
            get { return _adsb != null; }
            set
            {
                if (value == true)
                {
                    _adsb = new adsb();

                    if (Settings.Instance["adsbserver"] != null)
                        adsb.server = Settings.Instance["adsbserver"];
                    if (Settings.Instance["adsbport"] != null)
                        adsb.serverport = int.Parse(Settings.Instance["adsbport"].ToString());
                }
                else
                {
                    adsb.Stop();
                    _adsb = null;
                }
            }
        }

        /// <summary>
        /// passive comports
        /// </summary>
        public static List<MAVLinkInterface> Comports = new List<MAVLinkInterface>();

        /// <summary>
        /// Active Comport interface
        /// </summary>
        public static MAVLinkInterface comPort
        {
            get
            {
                return _comPort;
            }
            set
            {
                if (_comPort == value)
                    return;
                _comPort = value;
                _comPort.MavChanged -= MainUIInstance.comPort_MavChanged;
                _comPort.MavChanged += MainUIInstance.comPort_MavChanged;
                MainUIInstance.comPort_MavChanged(null, null);
            }
        }

       static MAVLinkInterface _comPort = new MAVLinkInterface();


        /// <summary>
        /// other planes in the area from adsb
        /// </summary>
        public object adsblock = new object();

        public ConcurrentDictionary<string, adsb.PointLatLngAltHdg> adsbPlanes = new ConcurrentDictionary<string, adsb.PointLatLngAltHdg>();

        /// <summary>
        /// 端口名称
        /// </summary>
        public static string comPortName = "";
        /// <summary>
        /// 端口波特率
        /// </summary>
        public static int comPortBaud = 115200;

        /// <summary>
        /// mono detection
        /// </summary>
        public static bool MONO = false;

        /// <summary>
        /// speech engine enable
        /// </summary>
        public static bool speechEnable
        {
            get { return Speech.speechEnable; }
            set { Speech.speechEnable = value; }
        }

        /// <summary>
        /// spech engine static class
        /// </summary>
        public static ISpeech speechEngine { get; set; } = Speech.Instance;


        #endregion


        #region 私有方法

        /// <summary>
        /// needs to be true by default so that exits properly if no joystick used.
        /// </summary>
        volatile private bool joysendThreadExited = true;

        /// <summary>
        /// thread used to send joystick packets to the MAV
        /// </summary>
        private void joysticksend()
        {
            float rate = 50; // 1000 / 50 = 20 hz
            int count = 0;

            DateTime lastratechange = DateTime.Now;

            joystickthreadrun = true;

            while (joystickthreadrun)
            {
                joysendThreadExited = false;
                //so we know this thread is stil alive.           
                try
                {
                    if (MONO)
                    {
                        log.Error("Mono: closing joystick thread");
                        break;
                    }

                    if (!MONO)
                    {
                        //joystick stuff

                        if (joystick != null && joystick.enabled)
                        {
                            if (!joystick.manual_control)
                            {
                                MAVLink.mavlink_rc_channels_override_t rc = new MAVLink.mavlink_rc_channels_override_t();

                                rc.target_component = comPort.MAV.compid;
                                rc.target_system = comPort.MAV.sysid;

                                if (joystick.getJoystickAxis(1) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan1_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech1;
                                if (joystick.getJoystickAxis(2) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan2_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech2;
                                if (joystick.getJoystickAxis(3) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan3_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech3;
                                if (joystick.getJoystickAxis(4) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan4_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech4;
                                if (joystick.getJoystickAxis(5) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan5_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech5;
                                if (joystick.getJoystickAxis(6) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan6_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech6;
                                if (joystick.getJoystickAxis(7) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan7_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech7;
                                if (joystick.getJoystickAxis(8) != Joystick.Joystick.joystickaxis.None)
                                    rc.chan8_raw = (ushort)MainUI.comPort.MAV.cs.rcoverridech8;

                                if (lastjoystick.AddMilliseconds(rate) < DateTime.Now)
                                {
                                    /*
                                if (MainUI.comPort.MAV.cs.rssi > 0 && MainUI.comPort.MAV.cs.remrssi > 0)
                                {
                                    if (lastratechange.Second != DateTime.Now.Second)
                                    {
                                        if (MainUI.comPort.MAV.cs.txbuffer > 90)
                                        {
                                            if (rate < 20)
                                                rate = 21;
                                            rate--;

                                            if (MainUI.comPort.MAV.cs.linkqualitygcs < 70)
                                                rate = 50;
                                        }
                                        else
                                        {
                                            if (rate > 100)
                                                rate = 100;
                                            rate++;
                                        }

                                        lastratechange = DateTime.Now;
                                    }
                                 
                                }
                                */
                                    //  Console.WriteLine(DateTime.Now.Millisecond + " {0} {1} {2} {3} {4}", rc.chan1_raw, rc.chan2_raw, rc.chan3_raw, rc.chan4_raw,rate);

                                    //Console.WriteLine("Joystick btw " + comPort.BaseStream.BytesToWrite);

                                    if (!comPort.BaseStream.IsOpen)
                                        continue;

                                    if (comPort.BaseStream.BytesToWrite < 50)
                                    {
                                        if (sitl)
                                        {
                                            SITL.rcinput();
                                        }
                                        else
                                        {
                                            comPort.sendPacket(rc, rc.target_system, rc.target_component);
                                        }
                                        count++;
                                        lastjoystick = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                MAVLink.mavlink_manual_control_t rc = new MAVLink.mavlink_manual_control_t();

                                rc.target = comPort.MAV.compid;

                                if (joystick.getJoystickAxis(1) != Joystick.Joystick.joystickaxis.None)
                                    rc.x = MainUI.comPort.MAV.cs.rcoverridech1;
                                if (joystick.getJoystickAxis(2) != Joystick.Joystick.joystickaxis.None)
                                    rc.y = MainUI.comPort.MAV.cs.rcoverridech2;
                                if (joystick.getJoystickAxis(3) != Joystick.Joystick.joystickaxis.None)
                                    rc.z = MainUI.comPort.MAV.cs.rcoverridech3;
                                if (joystick.getJoystickAxis(4) != Joystick.Joystick.joystickaxis.None)
                                    rc.r = MainUI.comPort.MAV.cs.rcoverridech4;

                                if (lastjoystick.AddMilliseconds(rate) < DateTime.Now)
                                {
                                    if (!comPort.BaseStream.IsOpen)
                                        continue;

                                    if (comPort.BaseStream.BytesToWrite < 50)
                                    {
                                        if (sitl)
                                        {
                                            SITL.rcinput();
                                        }
                                        else
                                        {
                                            comPort.sendPacket(rc, comPort.MAV.sysid, comPort.MAV.compid);
                                        }
                                        count++;
                                        lastjoystick = DateTime.Now;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(20);
                }
                catch
                {
                } // cant fall out
            }
            joysendThreadExited = true; //so we know this thread exited.    
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                log.Info("Loading config");

                Settings.Instance.Load();

                comPortName = Settings.Instance.ComPort;
            }
            catch (Exception ex)
            {
                log.Error("Bad Config File", ex);
            }
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                log.Info("Saving config");
                Settings.Instance.ComPort = comPortName;

                //if (_connectionControl != null)
                //    Settings.Instance.BaudRate = _connectionControl.CMB_baudrate.Text;

                //Settings.Instance.APMFirmware = MainUI.comPort.MAV.cs.firmware.ToString();

                Settings.Instance.Save();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString());
            }
        }

        ManualResetEvent SerialThreadrunner = new ManualResetEvent(false);
        /// <summary>
        /// 读取串口数据
        /// </summary>
        private void SerialReader() {

            if (serialThread) return;

            serialThread = true;
            SerialThreadrunner.Reset();

            int minbytes = 0; 
            int altwarningmax = 0;

            bool armedstatus = false;

            string lastmessagehigh = "";

            DateTime speechcustomtime = DateTime.Now;

            DateTime speechlowspeedtime = DateTime.Now;

            DateTime linkqualitytime = DateTime.Now;

            while (serialThread) {

                try {
                    Thread.Sleep(1);


                    #region update connect/disconnect button and info stats
                    try
                    {

                        UpdateConnectIcon();

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    #endregion


                    #region 30 seconds interval speech options
                   if (speechEnable && speechEngine != null && (DateTime.Now - speechcustomtime).TotalSeconds > 30 &&
                        (MainUI.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (MainUI.speechEngine.IsReady)
                        {
                            if (Settings.Instance.GetBoolean("speechcustomenabled"))
                            {
                                MainUI.speechEngine.SpeakAsync(MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechcustom"]));
                            }

                            speechcustomtime = DateTime.Now;
                        }

                        // speech for battery alerts
                        //speechbatteryvolt
                        float warnvolt = Settings.Instance.GetFloat("speechbatteryvolt");
                        float warnpercent = Settings.Instance.GetFloat("speechbatterypercent");

                        if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                            MainUI.comPort.MAV.cs.battery_voltage <= warnvolt &&
                            MainUI.comPort.MAV.cs.battery_voltage >= 5.0)
                        {
                            if (MainUI.speechEngine.IsReady)
                            {
                                MainUI.speechEngine.SpeakAsync(MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechbattery"]));
                            }
                        }
                        else if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                                 (MainUI.comPort.MAV.cs.battery_remaining) < warnpercent &&
                                 MainUI.comPort.MAV.cs.battery_voltage >= 5.0 &&
                                 MainUI.comPort.MAV.cs.battery_remaining != 0.0)
                        {
                            if (MainUI.speechEngine.IsReady)
                            {
                                MainUI.speechEngine.SpeakAsync(
                                    MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechbattery"]));
                            }
                        }
                    }
                    #endregion

                    #region speech for airspeed alerts
                    // 
                    if (speechEnable && speechEngine != null && (DateTime.Now - speechlowspeedtime).TotalSeconds > 10 &&
                        (MainUI.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (Settings.Instance.GetBoolean("speechlowspeedenabled") == true && MainUI.comPort.MAV.cs.armed)
                        {
                            float warngroundspeed = Settings.Instance.GetFloat("speechlowgroundspeedtrigger");
                            float warnairspeed = Settings.Instance.GetFloat("speechlowairspeedtrigger");

                            if (MainUI.comPort.MAV.cs.airspeed < warnairspeed)
                            {
                                if (MainUI.speechEngine.IsReady)
                                {
                                    MainUI.speechEngine.SpeakAsync(
                                        MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechlowairspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else if (MainUI.comPort.MAV.cs.groundspeed < warngroundspeed)
                            {
                                if (MainUI.speechEngine.IsReady)
                                {
                                    MainUI.speechEngine.SpeakAsync(
                                        MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechlowgroundspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else
                            {
                                speechlowspeedtime = DateTime.Now;
                            }
                        }
                    }
                    #endregion

                    #region speech altitude warning - message high warning
                    // 
                    if (speechEnable && speechEngine != null &&
                        (MainUI.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        float warnalt = float.MaxValue;
                        if (Settings.Instance.ContainsKey("speechaltheight"))
                        {
                            warnalt = Settings.Instance.GetFloat("speechaltheight");
                        }
                        try
                        {
                            altwarningmax = (int)Math.Max(MainUI.comPort.MAV.cs.alt, altwarningmax);

                            if (Settings.Instance.GetBoolean("speechaltenabled") == true && MainUI.comPort.MAV.cs.alt != 0.00 &&
                                (MainUI.comPort.MAV.cs.alt <= warnalt) && MainUI.comPort.MAV.cs.armed)
                            {
                                if (altwarningmax > warnalt)
                                {
                                    if (MainUI.speechEngine.IsReady)
                                        MainUI.speechEngine.SpeakAsync(
                                            MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, "" + Settings.Instance["speechalt"]));
                                }
                            }
                        }
                        catch
                        {
                        } // silent fail


                        try
                        {
                            // say the latest high priority message
                            if (MainUI.speechEngine.IsReady &&
                                lastmessagehigh != MainUI.comPort.MAV.cs.messageHigh && MainUI.comPort.MAV.cs.messageHigh != null)
                            {
                                if (!MainUI.comPort.MAV.cs.messageHigh.StartsWith("PX4v2 "))
                                {
                                    MainUI.speechEngine.SpeakAsync(MainUI.comPort.MAV.cs.messageHigh);
                                    lastmessagehigh = MainUI.comPort.MAV.cs.messageHigh;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    #endregion

                    // not doing anything
                    if (!MainUI.comPort.logreadmode && !comPort.BaseStream.IsOpen)
                    {
                        altwarningmax = 0;
                    }

                    #region attenuate the link qualty over time
                    // 
                    if ((DateTime.Now - MainUI.comPort.MAV.lastvalidpacket).TotalSeconds >= 1)
                    {
                        if (linkqualitytime.Second != DateTime.Now.Second)
                        {
                            MainUI.comPort.MAV.cs.linkqualitygcs = (ushort)(MainUI.comPort.MAV.cs.linkqualitygcs * 0.8f);
                            linkqualitytime = DateTime.Now;

                            // force redraw if there are no other packets are being read
                            this.mHUD.Invalidate();
                        }
                    }
                    #endregion


                    #region data loss warning - wait min of 10 seconds, ignore first 30 seconds of connect, repeat at 5 seconds interval
                    // 
                    if ((DateTime.Now - MainUI.comPort.MAV.lastvalidpacket).TotalSeconds > 10
                        && (DateTime.Now - connecttime).TotalSeconds > 30
                        && (DateTime.Now - nodatawarning).TotalSeconds > 5
                        && (MainUI.comPort.logreadmode || comPort.BaseStream.IsOpen)
                        && MainUI.comPort.MAV.cs.armed)
                    {
                        if (speechEnable && speechEngine != null)
                        {
                            if (MainUI.speechEngine.IsReady)
                            {
                                MainUI.speechEngine.SpeakAsync("WARNING No Data for " +
                                                               (int)
                                                                   (DateTime.Now - MainUI.comPort.MAV.lastvalidpacket)
                                                                       .TotalSeconds + " Seconds");
                                nodatawarning = DateTime.Now;
                            }
                        }
                    }
                    #endregion


                    #region get home point on armed status change.
                    // 
                    if (armedstatus != MainUI.comPort.MAV.cs.armed && comPort.BaseStream.IsOpen)
                    {
                        armedstatus = MainUI.comPort.MAV.cs.armed;
                        // status just changed to armed
                        if (MainUI.comPort.MAV.cs.armed == true && MainUI.comPort.MAV.aptype != MAVLink.MAV_TYPE.GIMBAL)
                        {
                            System.Threading.ThreadPool.QueueUserWorkItem(state =>
                            {
                                try
                                {
                                   // if (MainUI.comPort.getWP(0).lat != 0 && MainUI.comPort.getWP(0).lng != 0)
                                        MainUI.comPort.MAV.cs.HomeLocation = new PointLatLngAlt(MainUI.comPort.getWP(0));
                                   // else MainUI.comPort.MAV.cs.HomeLocation = MainUI.comPort.MAV.cs.Location;
                                    //if (MainUI.current != null && MyView.current.Name == "FlightPlanner")
                                    //{
                                    //    // update home if we are on flight data tab
                                        this.BeginInvoke((Action)delegate { UIManager.updateHome(); });
                                    //}

                                }
                                catch
                                {
                                    // dont hang this loop
                                    this.BeginInvoke(
                                        (Action)
                                            delegate
                                            {
                                                CustomMessageBox.Show("Failed to update home location (" +
                                                                      MainUI.comPort.MAV.sysid + ")");
                                            });
                                }
                            });
                        }

                        if (speechEnable && speechEngine != null)
                        {
                            if (Settings.Instance.GetBoolean("speecharmenabled"))
                            {
                                string speech = armedstatus ? Settings.Instance["speecharm"] : Settings.Instance["speechdisarm"];
                                if (!string.IsNullOrEmpty(speech))
                                {
                                    MainUI.speechEngine.SpeakAsync(MissionPlanner.ArduPilot.Common.speechConversion(comPort.MAV, speech));
                                }
                            }
                        }
                    }
                    #endregion

                    #region  send a hb every seconds from gcs to ap
                    //
                    if (heatbeatSend.Second != DateTime.Now.Second)
                    {
                        MAVLink.mavlink_heartbeat_t htb = new MAVLink.mavlink_heartbeat_t()
                        {
                            type = (byte)MAVLink.MAV_TYPE.GCS,
                            autopilot = (byte)MAVLink.MAV_AUTOPILOT.INVALID,
                            mavlink_version = 3 // MAVLink.MAVLINK_VERSION
                        };

                        // enumerate each link
                        foreach (var port in Comports)
                        {
                            if (!port.BaseStream.IsOpen)
                                continue;

                            // poll for params at heartbeat interval - primary mav on this port only
                            if (!port.giveComport)
                            {
                                try
                                {
                                    // poll only when not armed
                                    if (!port.MAV.cs.armed)
                                    {
                                        port.getParamPoll();
                                        port.getParamPoll();
                                    }
                                }
                                catch
                                {
                                }
                            }

                            // there are 3 hb types we can send, mavlink1, mavlink2 signed and unsigned
                            bool sentsigned = false;
                            bool sentmavlink1 = false;
                            bool sentmavlink2 = false;

                            // enumerate each mav
                            foreach (var MAV in port.MAVlist)
                            {
                                try
                                {
                                    // are we talking to a mavlink2 device
                                    if (MAV.mavlinkv2)
                                    {
                                        // is signing enabled
                                        if (MAV.signing)
                                        {
                                            // check if we have already sent
                                            if (sentsigned)
                                                continue;
                                            sentsigned = true;
                                        }
                                        else
                                        {
                                            // check if we have already sent
                                            if (sentmavlink2)
                                                continue;
                                            sentmavlink2 = true;
                                        }
                                    }
                                    else
                                    {
                                        // check if we have already sent
                                        if (sentmavlink1)
                                            continue;
                                        sentmavlink1 = true;
                                    }

                                    port.sendPacket(htb, MAV.sysid, MAV.compid);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    // close the bad port
                                    try
                                    {
                                        port.Close();
                                    }
                                    catch
                                    {
                                    }
                                    // refresh the screen if needed
                                    if (port == MainUI.comPort)
                                    {
                                        // refresh config window if needed
                                        //if (MyView.current != null)
                                        //{
                                        //    this.BeginInvoke((MethodInvoker)delegate ()
                                        //    {
                                        //        if (MyView.current.Name == "HWConfig")
                                        //            MyView.ShowScreen("HWConfig");
                                        //        if (MyView.current.Name == "SWConfig")
                                        //            MyView.ShowScreen("SWConfig");
                                        //    });
                                        //}
                                    }
                                }
                            }
                        }

                        heatbeatSend = DateTime.Now;
                    }

                    #endregion

                    #region if not connected or busy, sleep and loop
                    // 
                    if (!comPort.BaseStream.IsOpen || comPort.giveComport == true)
                    {
                        if (!comPort.BaseStream.IsOpen)
                        {
                            // check if other ports are still open
                            foreach (var port in Comports)
                            {
                                if (port.BaseStream.IsOpen)
                                {
                                    Console.WriteLine("Main comport shut, swapping to other mav");
                                    comPort = port;
                                    break;
                                }
                            }
                        }

                        System.Threading.Thread.Sleep(100);
                    }

                    #endregion

                    #region read the interfaces
                    // 
                    foreach (var port in Comports.ToArray())
                    {
                        if (!port.BaseStream.IsOpen)
                        {
                            // skip primary interface
                            if (port == comPort)
                                continue;

                            // modify array and drop out
                            Comports.Remove(port);
                            port.Dispose();
                            break;
                        }

                        while (port.BaseStream.IsOpen && port.BaseStream.BytesToRead > minbytes &&
                               port.giveComport == false && serialThread)
                        {
                            try
                            {
                                port.readPacket();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                        // update currentstate of sysids on the port
                        foreach (var MAV in port.MAVlist)
                        {
                            try
                            {
                                MAV.cs.UpdateCurrentSettings(null, false, port, MAV);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    #endregion

                    


                }
                catch (Exception e) {

                    Tracking.AddException(e);
                    log.Error("Serial Reader fail :" + e.ToString());
                    try
                    {
                        comPort.Close();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

            }

            Console.WriteLine("SerialReader Done");
            SerialThreadrunner.Set();

        }

        /// <summary>
        /// Used to fix the icon status for unexpected unplugs etc...
        /// </summary>
        private void UpdateConnectIcon()
        {
            if ((DateTime.Now - connectButtonUpdate).Milliseconds > 500)
            {

                if (comPort.BaseStream.IsOpen)
                {
                    if ((string)this.btnConnect.Image.Tag != "Disconnect")
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.btnConnect.Image = displayicons.disconnect;
                            this.btnConnect.Image.Tag = "Disconnect";
                           // this.btnConnect.Text = Strings.DISCONNECTc;
                            _connectionControl.IsConnected(true);
                        });
                    }
                }
                else
                {
                    if (this.btnConnect.Image != null && (string)this.btnConnect.Image.Tag != "Connect")
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.btnConnect.Image = displayicons.connect;
                            this.btnConnect.Image.Tag = "Connect";
                            //this.btnConnect.Text = Strings.CONNECTc;
                            _connectionControl.IsConnected(false);
                            if (_connectionStats != null)
                            {
                                _connectionStats.StopUpdates();
                            }
                        });
                    }

                    if (comPort.logreadmode)
                    {
                        this.BeginInvoke((MethodInvoker)delegate { _connectionControl.IsConnected(true); });
                    }
                }
                connectButtonUpdate = DateTime.Now;
            }
        }
       
        #region 初始化连接控件
        
        /// <summary>
        /// 初始链接
        /// </summary>
        private void InitConnection() {

            _connectionControl =this.connectionControl1;
            _connectionControl.CMB_baudrate.TextChanged += this.CMB_baudrate_TextChanged;
            _connectionControl.CMB_serialport.SelectedIndexChanged += this.CMB_serialport_SelectedIndexChanged;
            _connectionControl.CMB_serialport.Click += this.CMB_serialport_Click;
            _connectionControl.cmb_sysid.Click += cmb_sysid_Click;
            _connectionControl.ShowLinkStats += (sender, e) => ShowConnectionStatsForm();

        }

        /// <summary>
        /// 更新绑定的hud和面板重要数据
        /// </summary>
        private void updateBindingSourceWork() {

            try {

                //hud
                MainUI.comPort.MAV.cs.UpdateCurrentSettings(bindingSourceHud.UpdateDataSource(MainUI.comPort.MAV.cs));
                //重要信息面板
                MainUI.comPort.MAV.cs.UpdateCurrentSettings(bindingSourceQuickTab.UpdateDataSource(MainUI.comPort.MAV.cs));
                //
               

            } catch (Exception e) {

                CustomMessageBox.Show(e.Message);
            }



        }

        /// <summary>
        /// 波特率改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMB_baudrate_TextChanged(object sender, EventArgs e)
        {
            comPortBaud = int.Parse(_connectionControl.CMB_baudrate.Text);
            var sb = new StringBuilder();
            int baud = 0;
            for (int i = 0; i < _connectionControl.CMB_baudrate.Text.Length; i++)
                if (char.IsDigit(_connectionControl.CMB_baudrate.Text[i]))
                {
                    sb.Append(_connectionControl.CMB_baudrate.Text[i]);
                    baud = baud * 10 + _connectionControl.CMB_baudrate.Text[i] - '0';
                }
            if (_connectionControl.CMB_baudrate.Text != sb.ToString())
            {
                _connectionControl.CMB_baudrate.Text = sb.ToString();
            }
            try
            {
                if (baud > 0 && comPort.BaseStream.BaudRate != baud)
                    comPort.BaseStream.BaudRate = baud;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        /// <summary>
        /// 连接方式改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMB_serialport_SelectedIndexChanged(object sender, EventArgs e)
        {
            comPortName = _connectionControl.CMB_serialport.Text;
            if (comPortName == "UDP" || comPortName == "UDPCl" || comPortName == "TCP" || comPortName == "AUTO")
            {
                _connectionControl.CMB_baudrate.Enabled = false;
            }
            else
            {
                _connectionControl.CMB_baudrate.Enabled = true;
            }

            try
            {
                // check for saved baud rate and restore
                if (Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"] != null)
                {
                    _connectionControl.CMB_baudrate.Text = Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"];
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 获取并设置链接方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMB_serialport_Click(object sender, EventArgs e)
        {
            string oldport = _connectionControl.CMB_serialport.Text;
            PopulateSerialportList();
            if (_connectionControl.CMB_serialport.Items.Contains(oldport))
                _connectionControl.CMB_serialport.Text = oldport;
        }

        private void PopulateSerialportList()
        {
            _connectionControl.CMB_serialport.Items.Clear();
            _connectionControl.CMB_serialport.Items.Add("AUTO");
            _connectionControl.CMB_serialport.Items.AddRange(SerialPort.GetPortNames());
            _connectionControl.CMB_serialport.Items.Add("TCP");
            _connectionControl.CMB_serialport.Items.Add("UDP");
            _connectionControl.CMB_serialport.Items.Add("UDPCl");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmb_sysid_Click(object sender, EventArgs e)
        {
            _connectionControl.UpdateSysIDS();
        }
        /// <summary>
        /// 显示链接状态
        /// </summary>
        private void ShowConnectionStatsForm()
        {
            if (this.connectionStatsForm == null || this.connectionStatsForm.IsDisposed)
            {
                // If the form has been closed, or never shown before, we need all new stuff
                this.connectionStatsForm = new Form
                {
                    Width = 430,
                    Height = 180,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                   // Text = Strings.LinkStats
                };
                // Change the connection stats control, so that when/if the connection stats form is showing,
                // there will be something to see
                this.connectionStatsForm.Controls.Clear();
                _connectionStats = new ConnectionStats(comPort);
                this.connectionStatsForm.Controls.Add(_connectionStats);
                this.connectionStatsForm.Width = _connectionStats.Width;
            }

            this.connectionStatsForm.Show();
            // ThemeManager.ApplyThemeTo(this.connectionStatsForm);
        }
        #endregion


        #region 图层初始化

        private void InitOverlay() {

            tfrpolygons = new GMapOverlay("tfrpolygons");
            mMapControl.Overlays.Add(tfrpolygons);

            kmlpolygons = new GMapOverlay("kmlpolygons");
            mMapControl.Overlays.Add(kmlpolygons);

            geofence = new GMapOverlay("geofence");
            mMapControl.Overlays.Add(geofence);

            polygons = new GMapOverlay("polygons");
            mMapControl.Overlays.Add(polygons);

            photosoverlay = new GMapOverlay("photos overlay");
            mMapControl.Overlays.Add(photosoverlay);

            routes = new GMapOverlay("routes");
            mMapControl.Overlays.Add(routes);

            rallypointoverlay = new GMapOverlay("rally points");
            mMapControl.Overlays.Add(rallypointoverlay);

            mMapControl.Overlays.Add(poioverlay);

            homelayer = new GMapOverlay("homelayer");
            mMapControl.Overlays.Add(homelayer);

            
        }


        #endregion

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            updateBindingSourceWork();
        }

        void comPort_MavChanged(object sender, EventArgs e)
        {
            log.Info("Mav Changed " + MainUI.comPort.MAV.sysid);

            HUD.Custom.src = MainUI.comPort.MAV.cs;

            CustomWarning.defaultsrc = MainUI.comPort.MAV.cs;

            //MissionPlanner.Controls.PreFlight.CheckListItem.defaultsrc = MainUI.comPort.MAV.cs;

            //// when uploading a firmware we dont want to reload this screen.
            //if (instance.MyView.current.Control != null && instance.MyView.current.Control.GetType() == typeof(GCSViews.InitialSetup))
            //{
            //    var page = ((GCSViews.InitialSetup)instance.MyView.current.Control).backstageView.SelectedPage;
            //    if (page != null && page.Text == "Install Firmware")
            //    {
            //        return;
            //    }
            //}

            //if (this.InvokeRequired)
            //{
            //    this.Invoke((MethodInvoker)delegate
            //    {
            //        //enable the payload control page if a mavlink gimbal is detected
            //        if (instance.FlightData != null)
            //        {
            //            instance.FlightData.updatePayloadTabVisible();
            //        }

            //        instance.MyView.Reload();
            //    });
            //}
            //else
            //{
            //    //enable the payload control page if a mavlink gimbal is detected
            //    if (instance.FlightData != null)
            //    {
            //        instance.FlightData.updatePayloadTabVisible();
            //    }

            //    instance.MyView.Reload();
            //}
        }

        #endregion


        #region 公共方法

        public void ChangeUnits()
        {
            try
            {
                // dist
                if (Settings.Instance["distunits"] != null)
                {
                    switch (
                        (distances)Enum.Parse(typeof(distances), Settings.Instance["distunits"].ToString()))
                    {
                        case distances.Meters:
                            CurrentState.multiplierdist = 1;
                            CurrentState.DistanceUnit = "m";
                            break;
                        case distances.Feet:
                            CurrentState.multiplierdist = 3.2808399f;
                            CurrentState.DistanceUnit = "ft";
                            break;
                    }
                }
                else
                {
                    CurrentState.multiplierdist = 1;
                    CurrentState.DistanceUnit = "m";
                }

                // alt
                if (Settings.Instance["altunits"] != null)
                {
                    switch (
                        (distances)Enum.Parse(typeof(altitudes), Settings.Instance["altunits"].ToString()))
                    {
                        case distances.Meters:
                            CurrentState.multiplieralt = 1;
                            CurrentState.AltUnit = "m";
                            break;
                        case distances.Feet:
                            CurrentState.multiplieralt = 3.2808399f;
                            CurrentState.AltUnit = "ft";
                            break;
                    }
                }
                else
                {
                    CurrentState.multiplieralt = 1;
                    CurrentState.AltUnit = "m";
                }

                // speed
                if (Settings.Instance["speedunits"] != null)
                {
                    switch ((speeds)Enum.Parse(typeof(speeds), Settings.Instance["speedunits"].ToString()))
                    {
                        case speeds.meters_per_second:
                            CurrentState.multiplierspeed = 1;
                            CurrentState.SpeedUnit = "m/s";
                            break;
                        case speeds.fps:
                            CurrentState.multiplierdist = 3.2808399f;
                            CurrentState.SpeedUnit = "fps";
                            break;
                        case speeds.kph:
                            CurrentState.multiplierspeed = 3.6f;
                            CurrentState.SpeedUnit = "kph";
                            break;
                        case speeds.mph:
                            CurrentState.multiplierspeed = 2.23693629f;
                            CurrentState.SpeedUnit = "mph";
                            break;
                        case speeds.knots:
                            CurrentState.multiplierspeed = 1.94384449f;
                            CurrentState.SpeedUnit = "kts";
                            break;
                    }
                }
                else
                {
                    CurrentState.multiplierspeed = 1;
                    CurrentState.SpeedUnit = "m/s";
                }
            }
            catch
            {
            }
        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        public MainUI()
        {

          
            log.Info("MainUI ctor");

            SetStyle(
                     ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor,
                     true);
             //this.DoubleBuffered = true;

            // 加载配置文件
            LoadConfig();
            // set this before we reset it
            Settings.Instance["NUM_tracklength"] = "200";
            // create one here - but override on load
            Settings.Instance["guid"] = Guid.NewGuid().ToString();


            // force language to be loaded
            L10N.GetConfigLang();

            ShowAirports = true;

            // setup adsb
            adsb.UpdatePlanePosition += adsb_UpdatePlanePosition;


            InitializeComponent();

            //初始高程数据目录
            srtm.datadirectory = Settings.GetDataDirectory() +"srtm";
            var t = Type.GetType("Mono.Runtime");
            MONO = (t != null);

            speechEngine = new Speech();

            Warnings.CustomWarning.defaultsrc = comPort.MAV.cs;
            Warnings.WarningEngine.Start();


            //主窗体实例赋值
            MainUIInstance = this;
            //界面管理器
            UIManager = new UIManager(MainUIInstance);
            //创建地图控件
            mMapControl=UIManager.CreateMap();


            #region 仪表盘
            //创建仪表盘
            mHUD = UIManager.CreateHUD();

            if (Settings.Instance["CHK_GDIPlus"] != null)
               mHUD.opengl = !bool.Parse(Settings.Instance["CHK_GDIPlus"].ToString());

            if (Settings.Instance["CHK_hudshow"] != null)
                mHUD.hudon = bool.Parse(Settings.Instance["CHK_hudshow"].ToString());
            #endregion



            CommandsControl commandsControl= UIManager.CreateCommands();
            commandsControl.SetCOMPort = MainUI.comPort;
          

            //初始化链接控件
            InitConnection();         

            InitHUD();

          
            //改变单位
            ChangeUnits();
            //初始化图层
            InitOverlay();


            #region 测试创建飞机图标
            //GMapOverlay top = new GMapOverlay();
            //mMapControl.Overlays.Add(top);

            //current = new Markers.GMapMarkerQuad(mMapControl.Position, 60, 12, 95, 2);
            //(current as Markers.GMapMarkerQuad).Radius = 3000;
            //(current as Markers.GMapMarkerQuad).ActiveMegaPhone(false);
            //top.Markers.Add(current);
            #endregion


            //
            Comports.Add(comPort);
            MainUI.comPort.MavChanged += comPort_MavChanged;
            //save config to test we have write access
            SaveConfig();

        
            ////创建视频控件
            //vlcControl = UIManager.CreateVideoPanel();
            
            //if (videoPanel == null) videoPanel= vlcControl.getVideoPanel;
            //vlcControl.getClickPanel.MouseDoubleClick += GetVLCControl_MouseDoubleClick;
            // vlcControl.getClickPanel.DoubleClick += GetVLCControl_DoubleClick;

            //  vlcControl.MouseDoubleClick += VlcControl_MouseDoubleClick;

            //创建重要信息面板
            quickInfoPanel = UIManager.CreateQuickInfoPanel();

            InitMessagePanel();

        }

        private void MainUI_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData) {

                case (Keys.Control | Keys.S):
                    SwapMapAndVideo();
                    break;
                default:
                    break;

            } 
        }

       
    

        private void GetVLCControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //
            vlcControl.getClickPanel.MouseClick -= GetVLCControl_MouseClick;
            showMessage("鼠标双击时坐标 X=" + e.X + ",Y=" + e.Y);
            SwapMapAndVideo();
            vlcControl.getClickPanel.MouseClick += GetVLCControl_MouseClick;

        }

        private void SwapMapAndVideo() {


            mainPanel.SuspendLayout();
                     

            if (this.mainPanel.Controls.Contains(mMapControl))
            {
                //  Settings.Instance["HudSwap"] = "true";
                mainPanel.Controls.Add(videoPanel);
                vlcControl.Controls.Add(mMapControl);
            }
            else
            {
                // Settings.Instance["HudSwap"] = "false";
                mainPanel.Controls.Add(mMapControl);
                vlcControl.Controls.Add(videoPanel);
            }

            mainPanel.ResumeLayout();



        }


        private void VlcControl_Log(object sender, Vlc.DotNet.Core.VlcMediaPlayerLogEventArgs e)
        {
            string message = string.Format("libVlc : {0} {1} @ {2}", e.Level, e.Message, e.Module);
            msgInfoPanel.InvokeIfRequired(c => {
                showMessage(message);
            });

        }

        delegate void ShowMessageDelegate(string message);
        /// <summary>
        /// 面板显示信息
        /// </summary>
        /// <param name="message"></param>
        private void showMessage(string message)
        {
            if (this.msgInfoPanel.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = new ShowMessageDelegate(showMessage);
                BeginInvoke(showMessageDelegate, new object[] { message });
            }
            else
            {
                msgInfoPanel.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":" + message + "\n");
                msgInfoPanel.ScrollToCaret();
            }


        }

        #region 初始化仪表盘参数
        /// <summary>
        /// 
        /// </summary>
        private void InitHUD()
        {
            mHUD.altunit = CurrentState.DistanceUnit;
            mHUD.speedunit = CurrentState.SpeedUnit;
            mHUD.distunit = CurrentState.DistanceUnit;
            this.mHUD.airspeed = 0F;
            this.mHUD.alt = 0F;
            this.mHUD.altunit = null;
            this.mHUD.AOA = 0F;
            this.mHUD.BackColor = System.Drawing.Color.Black;
            this.mHUD.batterylevel = 0F;
            this.mHUD.batteryremaining = 0F;
            this.mHUD.bgimage = null;
            this.mHUD.connected = false;
           // this.mHUD.ContextMenuStrip = this.contextMenuStripHud;
            this.mHUD.critAOA = 25F;
            this.mHUD.critSSA = 30F;
            this.mHUD.current = 0F;
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("airspeed", this.bindingSourceHud, "airspeed", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("alt", this.bindingSourceHud, "alt", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("batterylevel", this.bindingSourceHud, "battery_voltage", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("batteryremaining", this.bindingSourceHud, "battery_remaining", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("connected", this.bindingSourceHud, "connected", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("current", this.bindingSourceHud, "current", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("datetime", this.bindingSourceHud, "datetime", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("disttowp", this.bindingSourceHud, "wp_dist", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("ekfstatus", this.bindingSourceHud, "ekfstatus", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("failsafe", this.bindingSourceHud, "failsafe", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("gpsfix", this.bindingSourceHud, "gpsstatus", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("gpsfix2", this.bindingSourceHud, "gpsstatus2", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("gpshdop", this.bindingSourceHud, "gpshdop", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("gpshdop2", this.bindingSourceHud, "gpshdop2", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("groundalt", this.bindingSourceHud, "HomeAlt", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("groundcourse", this.bindingSourceHud, "groundcourse", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("groundspeed", this.bindingSourceHud, "groundspeed", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("heading", this.bindingSourceHud, "yaw", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("linkqualitygcs", this.bindingSourceHud, "linkqualitygcs", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("message", this.bindingSourceHud, "messageHigh", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("messagetime", this.bindingSourceHud, "messageHighTime", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("mode", this.bindingSourceHud, "mode", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("navpitch", this.bindingSourceHud, "nav_pitch", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("navroll", this.bindingSourceHud, "nav_roll", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("pitch", this.bindingSourceHud, "pitch", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("roll", this.bindingSourceHud, "roll", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("status", this.bindingSourceHud, "armed", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("targetalt", this.bindingSourceHud, "targetalt", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("targetheading", this.bindingSourceHud, "nav_bearing", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("targetspeed", this.bindingSourceHud, "targetairspeed", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("turnrate", this.bindingSourceHud, "turnrate", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("verticalspeed", this.bindingSourceHud, "verticalspeed", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("vibex", this.bindingSourceHud, "vibex", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("vibey", this.bindingSourceHud, "vibey", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("vibez", this.bindingSourceHud, "vibez", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("wpno", this.bindingSourceHud, "wpno", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("xtrack_error", this.bindingSourceHud, "xtrack_error", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("AOA", this.bindingSourceHud, "AOA", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("SSA", this.bindingSourceHud, "SSA", true));
            this.mHUD.DataBindings.Add(new System.Windows.Forms.Binding("critAOA", this.bindingSourceHud, "crit_AOA", true));
            this.mHUD.datetime = new System.DateTime(((long)(0)));
            this.mHUD.displayAOASSA = false;
            this.mHUD.disttowp = 0F;
            this.mHUD.distunit = null;
           
            this.mHUD.ekfstatus = 0F;
            this.mHUD.failsafe = false;
            this.mHUD.gpsfix = 0F;
            this.mHUD.gpsfix2 = 0F;
            this.mHUD.gpshdop = 0F;
            this.mHUD.gpshdop2 = 0F;
            this.mHUD.groundalt = 0F;
            this.mHUD.groundcourse = 0F;
            this.mHUD.groundspeed = 0F;
            this.mHUD.heading = 0F;
            this.mHUD.hudcolor = System.Drawing.Color.LightGray;
            this.mHUD.linkqualitygcs = 0F;
            this.mHUD.lowairspeed = false;
            this.mHUD.lowgroundspeed = false;
            this.mHUD.lowvoltagealert = false;
            this.mHUD.message = "";
            this.mHUD.messagetime = new System.DateTime(((long)(0)));
            this.mHUD.mode = "Unknown";
            this.mHUD.Name = "mHUD";
            this.mHUD.navpitch = 0F;
            this.mHUD.navroll = 0F;
            this.mHUD.pitch = 0F;
            this.mHUD.roll = 0F;
            this.mHUD.Russian = false;
            this.mHUD.speedunit = null;
            this.mHUD.SSA = 0F;
            this.mHUD.status = false;
            this.mHUD.streamjpg = null;
            this.mHUD.targetalt = 0F;
            this.mHUD.targetheading = 0F;
            this.mHUD.targetspeed = 0F;
            this.mHUD.turnrate = 0F;
            this.mHUD.verticalspeed = 0F;
            this.mHUD.vibex = 0F;
            this.mHUD.vibey = 0F;
            this.mHUD.vibez = 0F;
            this.mHUD.VSync = false;
            this.mHUD.wpno = 0;
            this.mHUD.xtrack_error = 0F;
            mHUD.doResize();
        }
       
        #endregion

        /// <summary>
        /// 初始化信息面板
        /// </summary>
        /// <param name="currentState"></param>
        private void InitMessagePanel()
        {

            if (Settings.Instance.ContainsKey("quickViewRows"))
            {
               // setQuickViewRowsCols(Settings.Instance["quickViewCols"], Settings.Instance["quickViewRows"]);
            }

            //mImportantMessagePanel.BindingDataSource(this.bindingSourceQuickTab);
            quickInfoPanel.Owner = this;
            quickInfoPanel.BindingDataSource(this.bindingSourceQuickTab);
            

        }

        private void setQuickViewRowsCols(string cols, string rows)
        {
            mImportantMessagePanel.tableLayoutPanelQuick.ColumnCount = Math.Max(1, int.Parse(cols));
            mImportantMessagePanel.tableLayoutPanelQuick.RowCount = Math.Max(1, int.Parse(rows));

            Settings.Instance["quickViewRows"] = mImportantMessagePanel.tableLayoutPanelQuick.RowCount.ToString();
            Settings.Instance["quickViewCols"] = mImportantMessagePanel.tableLayoutPanelQuick.ColumnCount.ToString();

            int total = mImportantMessagePanel.tableLayoutPanelQuick.ColumnCount * mImportantMessagePanel.tableLayoutPanelQuick.RowCount;

            // clean up extra
            while (mImportantMessagePanel.tableLayoutPanelQuick.Controls.Count > total)
                mImportantMessagePanel.tableLayoutPanelQuick.Controls.RemoveAt(mImportantMessagePanel.tableLayoutPanelQuick.Controls.Count - 1);

            // add extra
            while (total != mImportantMessagePanel.tableLayoutPanelQuick.Controls.Count)
            {
                var QV = new MissionPlanner.Controls.QuickView()
                {
                    Name = "quickView" + (mImportantMessagePanel.tableLayoutPanelQuick.Controls.Count + 1)
                };
                //QV.DoubleClick += quickView_DoubleClick;
                //QV.ContextMenuStrip = contextMenuStripQuickView;
                QV.Dock = DockStyle.Fill;
                QV.numberColor = GetColor();
                QV.number = 0;

                mImportantMessagePanel.tableLayoutPanelQuick.Controls.Add(QV);
                QV.GetFontSize();
            }

            for (int i = 0; i < mImportantMessagePanel.tableLayoutPanelQuick.ColumnCount; i++)
            {
                if (mImportantMessagePanel.tableLayoutPanelQuick.ColumnStyles.Count <= i)
                    mImportantMessagePanel.tableLayoutPanelQuick.ColumnStyles.Add(new ColumnStyle());
                mImportantMessagePanel.tableLayoutPanelQuick.ColumnStyles[i].SizeType = SizeType.Percent;
                mImportantMessagePanel.tableLayoutPanelQuick.ColumnStyles[i].Width = 100.0f / mImportantMessagePanel.tableLayoutPanelQuick.ColumnCount;
            }
            for (int j = 0; j < mImportantMessagePanel.tableLayoutPanelQuick.RowCount; j++)
            {
                if (mImportantMessagePanel.tableLayoutPanelQuick.RowStyles.Count <= j)
                    mImportantMessagePanel.tableLayoutPanelQuick.RowStyles.Add(new RowStyle());
                mImportantMessagePanel.tableLayoutPanelQuick.RowStyles[j].SizeType = SizeType.Percent;
                mImportantMessagePanel.tableLayoutPanelQuick.RowStyles[j].Height = 100.0f / mImportantMessagePanel.tableLayoutPanelQuick.RowCount;
            }
        }

        Random random = new Random();

        Color GetColor()
        {
            Color mix = Color.White;

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            // mix the color
            if (mix != null)
            {
                red = (red + mix.R) / 2;
                green = (green + mix.G) / 2;
                blue = (blue + mix.B) / 2;
            }

            var col = Color.FromArgb(red, green, blue);

            //this.LogInfo("GetColor() " + col);

            return col;
        }



        void adsb_UpdatePlanePosition(object sender, MissionPlanner.Utilities.adsb.PointLatLngAltHdg adsb)
        {
            lock (adsblock)
            {
                var id = adsb.Tag;

                if (MainUI.MainUIInstance.adsbPlanes.ContainsKey(id))
                {
                    // update existing
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).Lat = adsb.Lat;
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).Lng = adsb.Lng;
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).Alt = adsb.Alt;
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).Heading = adsb.Heading;
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).Time = DateTime.Now;
                    ((adsb.PointLatLngAltHdg)MainUIInstance.adsbPlanes[id]).CallSign = adsb.CallSign;
                }
                else
                {
                    // create new plane
                    MainUI.MainUIInstance.adsbPlanes[id] =
                        new adsb.PointLatLngAltHdg(adsb.Lat, adsb.Lng,
                            adsb.Alt, adsb.Heading, id,
                            DateTime.Now)
                        { CallSign = adsb.CallSign };
                }

                try
                {
                    MAVLink.mavlink_adsb_vehicle_t packet = new MAVLink.mavlink_adsb_vehicle_t();

                    packet.altitude = (int)(MainUI.MainUIInstance.adsbPlanes[id].Alt * 1000);
                    packet.altitude_type = (byte)MAVLink.ADSB_ALTITUDE_TYPE.GEOMETRIC;
                    packet.callsign = ASCIIEncoding.ASCII.GetBytes(adsb.CallSign);
                    packet.emitter_type = (byte)MAVLink.ADSB_EMITTER_TYPE.NO_INFO;
                    packet.heading = (ushort)(MainUI.MainUIInstance.adsbPlanes[id].Heading * 100);
                    packet.lat = (int)(MainUI.MainUIInstance.adsbPlanes[id].Lat * 1e7);
                    packet.lon = (int)(MainUI.MainUIInstance.adsbPlanes[id].Lng * 1e7);
                    packet.ICAO_address = uint.Parse(id, NumberStyles.HexNumber);

                    packet.flags = (ushort)(MAVLink.ADSB_FLAGS.VALID_ALTITUDE | MAVLink.ADSB_FLAGS.VALID_COORDS |
                        MAVLink.ADSB_FLAGS.VALID_HEADING | MAVLink.ADSB_FLAGS.VALID_CALLSIGN);

                    //send to current connected
                    MainUI.comPort.sendPacket(packet, MainUI.comPort.MAV.sysid, MainUI.comPort.MAV.compid);
                }
                catch
                {

                }
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (joystick != null)
            {
                while (!joysendThreadExited)
                    Thread.Sleep(10);

                joystick.Dispose(); //proper clean up of joystick.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // setup http server
            try
            {
                log.Info("start http");
                httpthread = new Thread(new httpserver().listernforclients)
                {
                    Name = "motion jpg stream-network kml",
                    IsBackground = true
                };
                httpthread.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error starting TCP listener thread: ", ex);
                CustomMessageBox.Show(ex.ToString());
            }


            log.Info("start joystick");
            // setup joystick packet sender
            joystickthread = new Thread(new ThreadStart(joysticksend))
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal,
                Name = "Main joystick sender"
            };
            joystickthread.Start();

            log.Info("start serialreader");
            // 启动读取串口数据线程
            serialreaderthread = new Thread(SerialReader)
            {
                IsBackground = true,
                Name = "MainUI Serial reader",
                Priority = ThreadPriority.AboveNormal
            };
            serialreaderthread.Start();

            log.Info("start udpvideoshim");
            // start listener
            UDPVideoShim.Start();

            prop = new Propagation(mMapControl);

            //飞行数据
            log.Info("flight data");
            thisthread = new Thread(mainloop);
            thisthread.Name = "FD Mainloop";
            thisthread.IsBackground = true;
            thisthread.Start();

            ThreadPool.QueueUserWorkItem(BGLoadAirports);
            ThreadPool.QueueUserWorkItem(BGCreateMaps);
            

        }
        GMapOverlay polygons;
        GMapOverlay routes;
        GMapRoute route;
        /// <summary>
        /// 
        /// </summary>
        private void mainloop()
        {
            threadrun = true;
            EndPoint Remote = new IPEndPoint(IPAddress.Any, 0);

            DateTime tracklast = DateTime.Now.AddSeconds(0);

            DateTime tunning = DateTime.Now.AddSeconds(0);

            DateTime mapupdate = DateTime.Now.AddSeconds(0);

            DateTime vidrec = DateTime.Now.AddSeconds(0);

            DateTime waypoints = DateTime.Now.AddSeconds(0);

            DateTime updatescreen = DateTime.Now;

            DateTime tsreal = DateTime.Now;
            double taketime = 0;
            double timeerror = 0;

            while (!IsHandleCreated)
                Thread.Sleep(500);
          
            while (threadrun)
            {
                if (MainUI.comPort.giveComport)
                {
                    Thread.Sleep(50);
                    //更新绑定数据
                    updateBindingSource();
                    continue;
                }

                // max is only ever 10 hz but we go a little faster to empty the serial queue
                if (!MainUI.comPort.logreadmode)
                    Thread.Sleep(50); 

                if (this.IsDisposed)
                {
                    threadrun = false;
                    break;
                }

                #region hud avi
                //try
                //{
                //    if (aviwriter != null && vidrec.AddMilliseconds(100) <= DateTime.Now)
                //    {
                //        vidrec = DateTime.Now;

                //        mHUD.streamjpgenable = true;

                //        //aviwriter.avi_start("test.avi");
                //        // add a frame
                //        aviwriter.avi_add(mHUD.streamjpg.ToArray(), (uint)mHUD.streamjpg.Length);
                //        // write header - so even partial files will play
                //        aviwriter.avi_end(mHUD.Width, mHUD.Height, 10);
                //    }
                //}
                //catch
                //{
                //    log.Error("Failed to write avi");
                //}
                #endregion

                #region   log playback

                if (MainUI.comPort.logreadmode && MainUI.comPort.logplaybackfile != null)
                {
                    if (MainUI.comPort.BaseStream.IsOpen)
                    {
                        MainUI.comPort.logreadmode = false;
                        try
                        {
                            MainUI.comPort.logplaybackfile.Close();
                        }
                        catch
                        {
                            log.Error("Failed to close logfile");
                        }
                        MainUI.comPort.logplaybackfile = null;
                    }


                    //Console.WriteLine(DateTime.Now.Millisecond);

                    if (updatescreen.AddMilliseconds(300) < DateTime.Now)
                    {
                        try
                        {
                            // updatePlayPauseButton(true);
                            // updateLogPlayPosition();
                        }
                        catch
                        {
                            log.Error("Failed to update log playback pos");
                        }
                        updatescreen = DateTime.Now;
                    }

                    //Console.WriteLine(DateTime.Now.Millisecond + " done ");

                    DateTime logplayback = MainUI.comPort.lastlogread;
                    try
                    {
                        if (!MainUI.comPort.giveComport)
                            MainUI.comPort.readPacket();

                        // update currentstate of sysids on the port
                        foreach (var MAV in MainUI.comPort.MAVlist)
                        {
                            try
                            {
                                MAV.cs.UpdateCurrentSettings(null, false, MainUI.comPort, MAV);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    catch
                    {
                        log.Error("Failed to read log packet");
                    }

                    double act = (MainUI.comPort.lastlogread - logplayback).TotalMilliseconds;

                    if (act > 9999 || act < 0)
                        act = 0;

                    double ts = 0;
                    /* if (LogPlayBackSpeed == 0)
                         LogPlayBackSpeed = 0.01;
                     try
                     {
                         ts = Math.Min((act / LogPlayBackSpeed), 1000);
                     }
                     catch
                     {
                     }

                     if (LogPlayBackSpeed >= 4 && MainUI.speechEnable)
                         MainUI.speechEngine.SpeakAsyncCancelAll();
                     */
                    double timetook = (DateTime.Now - tsreal).TotalMilliseconds;
                    if (timetook != 0)
                    {
                        //Console.WriteLine("took: " + timetook + "=" + taketime + " " + (taketime - timetook) + " " + ts);
                        //Console.WriteLine(MainUI.comPort.lastlogread.Second + " " + DateTime.Now.Second + " " + (MainUI.comPort.lastlogread.Second - DateTime.Now.Second));
                        //if ((taketime - timetook) < 0)
                        {
                            timeerror += (taketime - timetook);
                            if (ts != 0)
                            {
                                ts += timeerror;
                                timeerror = 0;
                            }
                        }
                        if (Math.Abs(ts) > 1000)
                            ts = 1000;
                    }

                    taketime = ts;
                    tsreal = DateTime.Now;

                    if (ts > 0 && ts < 1000)
                        Thread.Sleep((int)ts);

                    tracklast = tracklast.AddMilliseconds(ts - act);
                    tunning = tunning.AddMilliseconds(ts - act);

                    if (tracklast.Month != DateTime.Now.Month)
                    {
                        tracklast = DateTime.Now;
                        tunning = DateTime.Now;
                    }

                    try
                    {
                        if (MainUI.comPort.logplaybackfile != null &&
                            MainUI.comPort.logplaybackfile.BaseStream.Position ==
                            MainUI.comPort.logplaybackfile.BaseStream.Length)
                        {
                            MainUI.comPort.logreadmode = false;
                        }
                    }
                    catch
                    {
                        MainUI.comPort.logreadmode = false;
                    }
                }
                else
                {
                    // ensure we know to stop
                    if (MainUI.comPort.logreadmode)
                        MainUI.comPort.logreadmode = false;
                    // updatePlayPauseButton(false);

                    /* if (!playingLog && MainUI.comPort.logplaybackfile != null)
                     {
                         continue;
                     }*/
                }
                #endregion

                #region

                #endregion



                try
                {
                    //CheckAndBindPreFlightData();
                    //Console.WriteLine(DateTime.Now.Millisecond);
                    //int fixme;
                     updateBindingSource();
                    // Console.WriteLine(DateTime.Now.Millisecond + " done ");

                    // battery warning.
                    float warnvolt = Settings.Instance.GetFloat("speechbatteryvolt");
                    float warnpercent = Settings.Instance.GetFloat("speechbatterypercent");

                    if (MainUI.comPort.MAV.cs.battery_voltage <= warnvolt)
                    {
                        mHUD.lowvoltagealert = true;
                    }
                    else if ((MainUI.comPort.MAV.cs.battery_remaining) < warnpercent)
                    {
                        mHUD.lowvoltagealert = true;
                    }
                    else
                    {
                        mHUD.lowvoltagealert = false;
                    }

                    // update opengltest
                    if (OpenGLtest.instance != null)
                    {
                        OpenGLtest.instance.rpy = new OpenTK.Vector3(MainUI.comPort.MAV.cs.roll, MainUI.comPort.MAV.cs.pitch,
                            MainUI.comPort.MAV.cs.yaw);
                        OpenGLtest.instance.LocationCenter = new PointLatLngAlt(MainUI.comPort.MAV.cs.lat,
                            MainUI.comPort.MAV.cs.lng, MainUI.comPort.MAV.cs.altasl / CurrentState.multiplieralt, "here");
                    }

                    // update opengltest2
                    if (OpenGLtest2.instance != null)
                    {
                        OpenGLtest2.instance.rpy = new OpenTK.Vector3(MainUI.comPort.MAV.cs.roll, MainUI.comPort.MAV.cs.pitch,
                            MainUI.comPort.MAV.cs.yaw);
                        OpenGLtest2.instance.LocationCenter = new PointLatLngAlt(MainUI.comPort.MAV.cs.lat,
                            MainUI.comPort.MAV.cs.lng, MainUI.comPort.MAV.cs.altasl / CurrentState.multiplieralt, "here");
                    }

                    // update vario info
                    Vario.SetValue(MainUI.comPort.MAV.cs.climbrate);



                    #region  udpate tunning tab
                    /*
                    if (tunning.AddMilliseconds(50) < DateTime.Now)
                    {
                        double time = (Environment.TickCount - tickStart) / 1000.0;
                        if (list1item != null)
                            list1.Add(time, ConvertToDouble(list1item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list2item != null)
                            list2.Add(time, ConvertToDouble(list2item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list3item != null)
                            list3.Add(time, ConvertToDouble(list3item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list4item != null)
                            list4.Add(time, ConvertToDouble(list4item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list5item != null)
                            list5.Add(time, ConvertToDouble(list5item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list6item != null)
                            list6.Add(time, ConvertToDouble(list6item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list7item != null)
                            list7.Add(time, ConvertToDouble(list7item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list8item != null)
                            list8.Add(time, ConvertToDouble(list8item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list9item != null)
                            list9.Add(time, ConvertToDouble(list9item.GetValue(MainUI.comPort.MAV.cs, null)));
                        if (list10item != null)
                            list10.Add(time, ConvertToDouble(list10item.GetValue(MainUI.comPort.MAV.cs, null)));
                    }
                    *
                     */

                    #endregion
                    
                    
                    #region update map

                    if (tracklast.AddSeconds(1.2) < DateTime.Now)
                    {

                        #region 是否显示遥控按钮
                        /*
                        if (MainUI.joystick != null && MainUI.joystick.enabled)
                        {
                            this.Invoke((MethodInvoker)delegate {
                              but_disablejoystick.Visible = true;
                            });
                        }*/
                        #endregion


                        adsb.CurrentPosition = MainUI.comPort.MAV.cs.HomeLocation;

                        // show proximity screen
                        if (MainUI.comPort.MAV?.Proximity != null && MainUI.comPort.MAV.Proximity.DataAvailable)
                        {
                            //this.BeginInvoke((MethodInvoker)delegate { new ProximityControl(MainUI.comPort.MAV).Show(); });
                        }

                        if (Settings.Instance.GetBoolean("CHK_maprotation"))
                        {
                            // dont holdinvalidation here
                            setMapBearing();
                        }

                        if (route == null)
                        {
                            route = new GMapRoute(trackPoints, "track");
                            routes.Routes.Add(route);
                        }

                        PointLatLng currentloc = new PointLatLng(MainUI.comPort.MAV.cs.lat, MainUI.comPort.MAV.cs.lng);

                        mMapControl.HoldInvalidation = true;

                        int numTrackLength = Settings.Instance.GetInt32("NUM_tracklength");
                        // maintain route history length
                        if (route.Points.Count > numTrackLength)
                        {
                            route.Points.RemoveRange(0,
                                route.Points.Count - numTrackLength);
                        }
                        // add new route point
                        if (MainUI.comPort.MAV.cs.lat != 0 && MainUI.comPort.MAV.cs.lng != 0)
                        {
                            route.Points.Add(currentloc);
                        }

                        if (!this.IsHandleCreated)
                            continue;

                        updateRoutePosition();

                        // update programed wp course
                        if (waypoints.AddSeconds(5) < DateTime.Now)
                        {
                            //Console.WriteLine("Doing FD WP's");
                            updateClearMissionRouteMarkers();

                            var wps = MainUI.comPort.MAV.wps.Values.ToList();
                            if (wps.Count > 1)
                            {
                                var homeplla = new PointLatLngAlt(MainUI.comPort.MAV.cs.HomeLocation.Lat,
                                    MainUI.comPort.MAV.cs.HomeLocation.Lng,
                                    MainUI.comPort.MAV.cs.HomeLocation.Alt / CurrentState.multiplieralt, "H");

                                var overlay = new WPOverlay();
                                overlay.CreateOverlay((MAVLink.MAV_FRAME)wps[1].frame, homeplla,
                                    wps.Select(a => (Locationwp)a).ToList(),
                                    0 / CurrentState.multiplieralt, 0 / CurrentState.multiplieralt);

                                var existing = mMapControl.Overlays.Where(a => a.Id == overlay.overlay.Id).ToList();
                                foreach (var b in existing)
                                {
                                    mMapControl.Overlays.Remove(b);
                                }

                                mMapControl.Overlays.Insert(1, overlay.overlay);

                                overlay.overlay.ForceUpdate();

                                var i = -1;
                                var travdist = 0.0;
                                var lastplla = overlay.pointlist.First();
                                foreach (var plla in overlay.pointlist)
                                {
                                    i++;
                                    if (plla == null)
                                        continue;

                                    var dist = lastplla.GetDistance(plla);

                                    // distanceBar1.AddWPDist((float)dist);

                                    if (i <= MainUI.comPort.MAV.cs.wpno)
                                    {
                                        travdist += dist;
                                    }
                                }

                                travdist -= MainUI.comPort.MAV.cs.wp_dist;

                                //if (MainUI.comPort.MAV.cs.mode.ToUpper() == "AUTO")
                                //    distanceBar1.traveleddist = (float)travdist;
                            }

                            RegeneratePolygon();

                            // update rally points

                            rallypointoverlay.Markers.Clear();

                            foreach (var mark in MainUI.comPort.MAV.rallypoints.Values)
                            {
                                rallypointoverlay.Markers.Add(new GMapMarkerRallyPt(mark));
                            }

                            // optional on Flight data
                            if (MainUI.ShowAirports)
                            {
                                
                                foreach (var item in Utilities.Airports.getAirports(mMapControl.Position).ToArray())
                                {
                                    try
                                    {
                                        rallypointoverlay.Markers.Add(new GMapMarkerAirport(item)
                                        {
                                            ToolTipText = item.Tag,
                                            ToolTipMode = MarkerTooltipMode.OnMouseOver
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                }
                            }

                            waypoints = DateTime.Now;
                        }

                        updateClearRoutesMarkers();

                        // add this after the mav icons are drawn
                        if (MainUI.comPort.MAV.cs.MovingBase != null && MainUI.comPort.MAV.cs.MovingBase == PointLatLngAlt.Zero)
                        {
                            addMissionRouteMarker(new GMarkerGoogle(currentloc, GMarkerGoogleType.blue_dot)
                            {
                                Position = MainUI.comPort.MAV.cs.MovingBase,
                                ToolTipText = "Moving Base",
                                ToolTipMode = MarkerTooltipMode.OnMouseOver
                            });
                        }

                        // add gimbal point center
                        try
                        {
                            if (MainUI.comPort.MAV.param.ContainsKey("MNT_STAB_TILT")
                                && MainUI.comPort.MAV.param.ContainsKey("MNT_STAB_ROLL")
                                && MainUI.comPort.MAV.param.ContainsKey("MNT_TYPE"))
                            {
                                float temp1 = (float)MainUI.comPort.MAV.param["MNT_STAB_TILT"];
                                float temp2 = (float)MainUI.comPort.MAV.param["MNT_STAB_ROLL"];

                                float temp3 = (float)MainUI.comPort.MAV.param["MNT_TYPE"];

                                if (MainUI.comPort.MAV.param.ContainsKey("MNT_STAB_PAN") &&
                                    // (float)MainUI.comPort.MAV.param["MNT_STAB_PAN"] == 1 &&
                                    ((float)MainUI.comPort.MAV.param["MNT_STAB_TILT"] == 1 &&
                                      (float)MainUI.comPort.MAV.param["MNT_STAB_ROLL"] == 0) ||
                                     (float)MainUI.comPort.MAV.param["MNT_TYPE"] == 4) // storm driver
                                {
                                    var marker = GimbalPoint.ProjectPoint();

                                    if (marker != PointLatLngAlt.Zero)
                                    {
                                        MainUI.comPort.MAV.cs.GimbalPoint = marker;

                                        addMissionRouteMarker(new GMarkerGoogle(marker, GMarkerGoogleType.blue_dot)
                                        {
                                            ToolTipText = "Camera Target\n" + marker,
                                            ToolTipMode = MarkerTooltipMode.OnMouseOver
                                        });
                                    }
                                }
                            }


                            // cleanup old - no markers where added, so remove all old 
                            if (MainUI.comPort.MAV.camerapoints.Count == 0)
                                photosoverlay.Markers.Clear();

                            var min_interval = 0.0;
                            if (MainUI.comPort.MAV.param.ContainsKey("CAM_MIN_INTERVAL"))
                                min_interval = MainUI.comPort.MAV.param["CAM_MIN_INTERVAL"].Value / 1000.0;

                            // set fov's based on last grid calc
                            //if (Settings.Instance["camera_fovh"] != null)
                            //{
                            //    GMapMarkerPhoto.hfov = Settings.Instance.GetDouble("camera_fovh");
                            //    GMapMarkerPhoto.vfov = Settings.Instance.GetDouble("camera_fovv");
                            //}

                            // add new - populate camera_feedback to map
                            double oldtime = double.MinValue;
                            foreach (var mark in MainUI.comPort.MAV.camerapoints.ToArray())
                            {
                                var timesincelastshot = (mark.time_usec / 1000.0) / 1000.0 - oldtime;
                                MainUI.comPort.MAV.cs.timesincelastshot = timesincelastshot;
                                bool contains = photosoverlay.Markers.Any(p => p.Tag.Equals(mark.time_usec));
                                if (!contains)
                                {
                                    if (timesincelastshot < min_interval)
                                        addMissionPhotoMarker(new GMapMarkerPhoto(mark, true));
                                    else
                                        addMissionPhotoMarker(new GMapMarkerPhoto(mark, false));
                                }
                                oldtime = (mark.time_usec / 1000.0) / 1000.0;
                            }

                            // age current
                            int camcount = MainUI.comPort.MAV.camerapoints.Count;
                            int a = 0;
                            foreach (var mark in photosoverlay.Markers)
                            {
                                if (mark is GMapMarkerPhoto)
                                {
                                    if (CameraOverlap)
                                    {
                                        var marker = ((GMapMarkerPhoto)mark);
                                        // abandon roll higher than 25 degrees
                                        if (Math.Abs(marker.Roll) < 25)
                                        {
                                            MainUI.comPort.MAV.GMapMarkerOverlapCount.Add(
                                                ((GMapMarkerPhoto)mark).footprintpoly);
                                        }
                                    }
                                    if (a < (camcount - 4))
                                        ((GMapMarkerPhoto)mark).drawfootprint = false;
                                }
                                a++;
                            }

                            if (CameraOverlap)
                            {
                                if (!kmlpolygons.Markers.Contains(MainUI.comPort.MAV.GMapMarkerOverlapCount) &&
                                    camcount > 0)
                                {
                                    kmlpolygons.Markers.Clear();
                                    kmlpolygons.Markers.Add(MainUI.comPort.MAV.GMapMarkerOverlapCount);
                                }
                            }
                            else if (kmlpolygons.Markers.Contains(MainUI.comPort.MAV.GMapMarkerOverlapCount))
                            {
                                kmlpolygons.Markers.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }

                        lock (MainUI.MainUIInstance.adsblock)
                        {
                            foreach (adsb.PointLatLngAltHdg plla in MainUI.MainUIInstance.adsbPlanes.Values)
                            {
                                // 30 seconds history
                                if (((DateTime)plla.Time) > DateTime.Now.AddSeconds(-30))
                                {
                                    var adsbplane = new GMapMarkerADSBPlane(plla, plla.Heading)
                                    {
                                        ToolTipText = "ICAO: " + plla.Tag + " " + plla.Alt.ToString("0"),
                                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                                        Tag = plla
                                    };

                                    if (plla.DisplayICAO)
                                        adsbplane.ToolTipMode = MarkerTooltipMode.Always;

                                    switch (plla.ThreatLevel)
                                    {
                                        case MAVLink.MAV_COLLISION_THREAT_LEVEL.NONE:
                                            adsbplane.AlertLevel = GMapMarkerADSBPlane.AlertLevelOptions.Green;
                                            break;
                                        case MAVLink.MAV_COLLISION_THREAT_LEVEL.LOW:
                                            adsbplane.AlertLevel = GMapMarkerADSBPlane.AlertLevelOptions.Orange;
                                            break;
                                        case MAVLink.MAV_COLLISION_THREAT_LEVEL.HIGH:
                                            adsbplane.AlertLevel = GMapMarkerADSBPlane.AlertLevelOptions.Red;
                                            break;
                                    }

                                    addMissionRouteMarker(adsbplane);
                                }
                            }
                        }


                        if (route.Points.Count > 0)
                        {
                            // add primary route icon

                            // draw guide mode point for only main mav
                            if (MainUI.comPort.MAV.cs.mode.ToLower() == "guided" && MainUI.comPort.MAV.GuidedMode.x != 0)
                            {
                                addpolygonmarker("Guided Mode", MainUI.comPort.MAV.GuidedMode.y,
                                    MainUI.comPort.MAV.GuidedMode.x, (int)MainUI.comPort.MAV.GuidedMode.z, Color.Blue,
                                    routes);
                            }

                            // draw all icons for all connected mavs
                            foreach (var port in MainUI.Comports.ToArray())
                            {
                                // draw the mavs seen on this port
                                foreach (var MAV in port.MAVlist)
                                {

                                    if (currentMarker == null) currentMarker = MissionPlanner.ArduPilot.Common.getMAVMarker(MAV);
                                    else {
                                        var marker = MissionPlanner.ArduPilot.Common.getMAVMarker(MAV);
                                        if (marker is GMapMarkerPlane)
                                        {
                                            (currentMarker as GMapMarkerPlane).updateMarkerValue(marker as GMapMarkerPlane);
                                        } else if (marker is GMapMarkerQuad)
                                        {
                                            (currentMarker as GMapMarkerQuad).updateMarkerValue(marker as GMapMarkerQuad);
                                        }

                                    } 
                                    //Console.WriteLine("mavs: mMegaPhoneStatus={0},mRadius={1}", (currentMarker as IPayload).MMegaPhoneStatus.ToString(), (currentMarker as IPayload).MRadius.ToString());

                                    if (currentMarker.Position.Lat == 0 && currentMarker.Position.Lng == 0)
                                        continue;

                                    addMissionRouteMarker(currentMarker as GMapMarker);
                                }
                            }

                            if (route.Points.Count == 0 || route.Points[route.Points.Count - 1].Lat != 0 &&
                                (mapupdate.AddSeconds(3) < DateTime.Now))
                            {
                                updateMapPosition(currentloc);
                                mapupdate = DateTime.Now;
                            }

                            if (route.Points.Count == 1 && mMapControl.Zoom == 3) // 3 is the default load zoom
                            {
                                updateMapPosition(currentloc);
                                updateMapZoom(17);
                            }
                        }

                        prop.Update(MainUI.comPort.MAV.cs.HomeLocation, MainUI.comPort.MAV.cs.Location,
                            MainUI.comPort.MAV.cs.battery_kmleft);

                        prop.alt = MainUI.comPort.MAV.cs.alt;
                        prop.altasl = MainUI.comPort.MAV.cs.altasl;
                        prop.center = mMapControl.Position;

                        mMapControl.HoldInvalidation = false;

                        //if (mMapControl.Visible)
                        //{
                        //    this.mMapControl.Invalidate();
                        //}

                        tracklast = DateTime.Now;
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Tracking.AddException(ex);
                    Console.WriteLine("FD Main loop exception " + ex);
                }
            }
            Console.WriteLine("FD Main loop exit");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="alt"></param>
        /// <param name="color"></param>
        /// <param name="overlay"></param>
        private void addpolygonmarker(string tag, double lng, double lat, int alt, Color? color, GMapOverlay overlay)
        {
            try
            {
                PointLatLng point = new PointLatLng(lat, lng);
                GMarkerGoogle m = new GMarkerGoogle(point, GMarkerGoogleType.green);
                m.ToolTipMode = MarkerTooltipMode.Always;
                m.ToolTipText = tag;
                m.Tag = tag;

                MissionPlanner.Maps.GMapMarkerRect mBorders = new MissionPlanner.Maps.GMapMarkerRect(point);
                {
                    mBorders.InnerMarker = m;
                    try
                    {
                        mBorders.wprad =
                            (int)(Settings.Instance.GetFloat("TXT_WPRad") / CurrentState.multiplierdist);
                    }
                    catch
                    {
                    }
                    if (color.HasValue)
                    {
                        mBorders.Color = color.Value;
                    }
                }

                Invoke((MethodInvoker)delegate
                {
                    overlay.Markers.Add(m);
                    overlay.Markers.Add(mBorders);
                });
            }
            catch (Exception)
            {
            }
        }

        private void updateClearRoutesMarkers()
        {
            Invoke((MethodInvoker)delegate
            {
                routes.Markers.Clear();
            });
        }

        /// <summary>
        /// used to redraw the polygon
        /// </summary>
        void RegeneratePolygon()
        {
            List<PointLatLng> polygonPoints = new List<PointLatLng>();

            if (routes == null)
                return;

            foreach (GMapMarker m in polygons.Markers)
            {
                if (m is MissionPlanner.Controls.MapMarkers.GMapMarkerRect)
                {
                    m.Tag = polygonPoints.Count;
                    polygonPoints.Add(m.Position);
                }
            }

            if (polygonPoints.Count < 2)
                return;

            GMapRoute homeroute = new GMapRoute("homepath");
            homeroute.Stroke = new Pen(Color.Yellow, 2);
            homeroute.Stroke.DashStyle = DashStyle.Dash;
            // add first point past home
            homeroute.Points.Add(polygonPoints[1]);
            // add home location
            homeroute.Points.Add(polygonPoints[0]);
            // add last point
            homeroute.Points.Add(polygonPoints[polygonPoints.Count - 1]);

            GMapRoute wppath = new GMapRoute("wp path");
            wppath.Stroke = new Pen(Color.Yellow, 4);
            wppath.Stroke.DashStyle = DashStyle.Dash;

            for (int a = 1; a < polygonPoints.Count-1; a++)
            {
                wppath.Points.Add(polygonPoints[a]);
            }

            Invoke((MethodInvoker)delegate
            {
                polygons.Routes.Add(homeroute);
                polygons.Routes.Add(wppath);
            });
        }

        /// <summary>
        /// 旋转地图
        /// </summary>
        private void setMapBearing()
        {
            Invoke((MethodInvoker)delegate { mMapControl.Bearing = (int)((MainUI.comPort.MAV.cs.yaw + 360) % 360); });
        }

        // to prevent cross thread calls while in a draw and exception
        private void updateClearMissionRouteMarkers()
        {
            // not async
            Invoke((MethodInvoker)delegate
            {
                polygons.Routes.Clear();
                polygons.Markers.Clear();
                routes.Markers.Clear();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateRoutePosition()
        {
            // not async
            Invoke((MethodInvoker)delegate
            {
                mMapControl.UpdateRouteLocalPosition(route);
            });
        }

        private void addMissionRouteMarker(GMapMarker marker)
        {
            // not async
            Invoke((MethodInvoker)delegate
            {
                routes.Markers.Add(marker);
            });
        }

        private void addMissionPhotoMarker(GMapMarker marker)
        {
            // not async
            Invoke((MethodInvoker)delegate
            {
                photosoverlay.Markers.Add(marker);
            });
        }

        /// <summary>
        /// Try to reduce the number of map position changes generated by the code
        /// </summary>
        DateTime lastmapposchange = DateTime.MinValue;
        /// <summary>
        /// 更新地图位置
        /// </summary>
        /// <param name="currentloc"></param>
        private void updateMapPosition(PointLatLng currentloc)
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    if (lastmapposchange.Second != DateTime.Now.Second)
                    {
                        if (Math.Abs(currentloc.Lat - mMapControl.Position.Lat) > 0.00001 || Math.Abs(currentloc.Lng - mMapControl.Position.Lng) > 0.00001)
                        {
                            mMapControl.Position = currentloc;
                        }
                        lastmapposchange = DateTime.Now;
                    }
                    //hud1.Refresh();
                }
                catch
                {
                }
            });
        }


        private void updateMapZoom(int zoom)
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    mMapControl.Zoom = zoom;
                }
                catch
                {
                }
            });
        }



        DateTime lastscreenupdate = DateTime.Now;
        object updateBindingSourcelock = new object();
        volatile int updateBindingSourcecount;
        string updateBindingSourceThreadName = "";

        /// <summary>
        /// 
        /// </summary>
        private void updateBindingSource()
        {
            //  run at 25 hz.
            if (lastscreenupdate.AddMilliseconds(40) < DateTime.Now)
            {
                lock (updateBindingSourcelock)
                {
                    // this is an attempt to prevent an invoke queue on the binding update on slow machines
                    if (updateBindingSourcecount > 0)
                    {
                        if (lastscreenupdate < DateTime.Now.AddSeconds(-5))
                        {
                            updateBindingSourcecount = 0;
                        }
                        return;
                    }

                    updateBindingSourcecount++;
                    updateBindingSourceThreadName = Thread.CurrentThread.Name;
                }

                this.BeginInvokeIfRequired((MethodInvoker)delegate
                {
                    updateBindingSourceWork();

                    lock (updateBindingSourcelock)
                    {
                        updateBindingSourcecount--;
                    }
                });
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED  
                return cp;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (MessageBox.Show("确定要退出程序吗？", "提示", MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.Cancel)

            {

                e.Cancel = true;

            }


            log.Info("closing serialthread");

            serialThread = false;

            if (serialreaderthread != null)
                serialreaderthread.Join();

            // speed up tile saving on exit
            GMap.NET.GMaps.Instance.CacheOnIdleRead = false;
            GMap.NET.GMaps.Instance.BoostCacheEngine = true;

            log.Info("MainUI_FormClosing");

            Settings.Instance["MainHeight"] = this.Height.ToString();
            Settings.Instance["MainWidth"] = this.Width.ToString();
            Settings.Instance["MainMaximised"] = this.WindowState.ToString();

            Settings.Instance["MainLocX"] = this.Location.X.ToString();
            Settings.Instance["MainLocY"] = this.Location.Y.ToString();

            // close bases connection
            try
            {
                comPort.logreadmode = false;
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();

                comPort.logfile = null;
                comPort.rawlogfile = null;
            }
            catch
            {
            }

            // close all connections
            foreach (var port in Comports)
            {
                try
                {
                    port.logreadmode = false;
                    if (port.logfile != null)
                        port.logfile.Close();

                    if (port.rawlogfile != null)
                        port.rawlogfile.Close();

                    port.logfile = null;
                    port.rawlogfile = null;
                }
                catch
                {
                }
            }

            adsb.Stop();

            Warnings.WarningEngine.Stop();

            UDPVideoShim.Stop();

            GStreamer.StopAll();

            log.Info("closing vlcrender");
            try
            {
                while (vlcrender.store.Count > 0)
                    vlcrender.store[0].Stop();
            }
            catch
            {
            }

            log.Info("closing pluginthread");

            pluginthreadrun = false;

            if (pluginthread != null)
                pluginthread.Join();

            log.Info("closing serialthread");

            serialThread = false;

            if (serialreaderthread != null)
                serialreaderthread.Join();

            log.Info("closing joystickthread");

            joystickthreadrun = false;

            if (joystickthread != null)
                joystickthread.Join();

            log.Info("closing httpthread");

            // if we are waiting on a socket we need to force an abort
            httpserver.Stop();

            log.Info("sorting tlogs");
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    try
                    {
                        SKYROVER.GCS.DeskTop.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                    );
            }
            catch
            {
            }

           

            try
            {
                if (comPort.BaseStream.IsOpen)
                    comPort.Close();
            }
            catch
            {
            } // i get alot of these errors, the port is still open, but not valid - user has unpluged usb

            // save config
            SaveConfig();

            Console.WriteLine(httpthread?.IsAlive);
            Console.WriteLine(joystickthread?.IsAlive);
            Console.WriteLine(serialreaderthread?.IsAlive);
            Console.WriteLine(pluginthread?.IsAlive);

            log.Info("MainUI_FormClosing done");

            if (MONO)
                this.Dispose();

        }
        //吊舱控制类
        private Nacelle nacelle;

        private void btnSetting_Click(object sender, EventArgs e)
        {
           
            if (setting == null)
            {
                setting = GenericSingleton<frmSetting>.CreateInstrance();
                //setting.TopMost = true;
                setting.Owner = this;
                setting.StartPosition = FormStartPosition.Manual;
                setting.Location = new Point(this.mainPanel.Width - setting.Width, this.topPanel.Height + 22);
                //喊话器参数变化
                setting.TurnOnMegaPhoneEvent += Setting_TurnOnMegaPhoneEvent;
                setting.RadiusValueChangedEvent += Setting_RadiusValueChangedEvent;

                //单光吊仓参数变化
                setting.AfterSinglePodParametersChangedEvent += Setting_AfterSinglePodParametersChangedEvent;
                setting.SinglePodSwitchedEvent += Setting_SinglePodSwitchedEvent;
                nacelle = setting.nacelle;
                setting.Show();
            }
            else {

                setting.SetPanelShown();
            }
           
        }

        private void Setting_SinglePodSwitchedEvent(bool active)
        {
            if (currentMarker!=null) {

                (currentMarker as IPayload).ActiveSinglePod(active);
            }
        }

        private void Setting_AfterSinglePodParametersChangedEvent(GMap.NET.PointLatLng point, float UAVHight, float podPitch, float heading,float roll, float CCDWidth, float CCDHight, float PodFocus)
        {
            if (currentMarker != null) {

                (currentMarker as IPayload).DrawRegion(point, UAVHight, podPitch, heading,roll, CCDWidth, CCDHight, PodFocus);

            }
        }

        private void Setting_UAVPayloadChangedEvent(MissionPlanner.Maps.GMapMarkerQuadType type)
        {
            
        }

        private void Setting_RadiusValueChangedEvent(int radius)
        {
            if (currentMarker != null)
            {

                (currentMarker as IPayload).MRadius=radius;

            }
        }

        private void Setting_TurnOnMegaPhoneEvent(bool active)
        {
            if (currentMarker != null) {

                (currentMarker as IPayload).activeMegaPhone(active);

            }
        }

        enum DrawMode
        {
            survey,
            line
        }

        DrawMode curreyDrawMode = DrawMode.survey;

        private void button1_Click(object sender, EventArgs e)
        {

            curreyDrawMode = DrawMode.survey;
            //1.清除之前航点
            if (MainUI.comPort.MAV.wps!=null&& MainUI.comPort.MAV.wps.Count>0) MainUI.comPort.MAV.wps.Clear();
           
            //2.清除已存在的航线
            foreach (var lyr in mMapControl.Overlays)
            {
                if (lyr.Id == "polygons"||lyr.Id== "WPOverlay"||lyr.Id== "surveyGridlayer") {

                    lyr.Polygons.Clear();
                    lyr.Markers.Clear();
                    lyr.Routes.Clear();
                }
            }

            //3.绘制多边形测区工具
            IMapTool drawPolygonTool = new DrawPolygonTool();
            mMapControl.CurrentTool = drawPolygonTool;
            (drawPolygonTool as DrawPolygonTool).OnFinishedEvent += DrawPolygonTool_OnFinishedEvent;//绘制结束事件
            drawPolygonTool.OnClick();
        }

        /// <summary>
        /// 结束绘制测区
        /// </summary>
        /// <param name="gMapPolygon"></param>
        private void DrawPolygonTool_OnFinishedEvent(GMapPolygon gMapPolygon, GMapMarker gMapMarker)
        {
            if (surveyGrid == null)
            {
                btnSurvey_Click(null,null);
            }
            else {

                surveyGrid.MapPolygon = gMapPolygon;
                //计算生成航线
                tripHelper.Marker = gMapMarker;
                tripHelper.CalculateSurveyGrid(gMapPolygon, surveyGrid.TripModel, surveyGrid.DisplayInfos);

                if (surveyGrid != null) surveyGrid.SetTripResultValue(tripHelper.GetTripResult);
                //tripHelper.runCalculate();
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            curreyDrawMode = DrawMode.line;
            //1.清除之前航点
            if (MainUI.comPort.MAV.wps != null && MainUI.comPort.MAV.wps.Count > 0) MainUI.comPort.MAV.wps.Clear();

            //2.清除已存在的航线
            foreach (var lyr in mMapControl.Overlays)
            {
                if (lyr.Id == "polygons" || lyr.Id == "WPOverlay" || lyr.Id == "surveyGridlayer")
                {

                    lyr.Polygons.Clear();
                    lyr.Markers.Clear();
                    lyr.Routes.Clear();
                }
            }

            IMapTool drawPointTool = new DrawLineTool();
            mMapControl.CurrentTool = drawPointTool;
            (drawPointTool as DrawLineTool).OnDrawRouteFinishedEvent += DrawLineTool_OnDrawRouteFinishedEvent;
            (drawPointTool as DrawLineTool).OnWayPointDoubleClickedEvent += OnWayPointDoubleClickedEvent;
            (drawPointTool as DrawLineTool).OnWayPointClickedEvent += OnWayPointClickedEvent;
            (drawPointTool as DrawLineTool).OnWayPointMovedEvent += OnWayPointMovedEvent;
            (drawPointTool as DrawLineTool).OnMouseUpEvent += MainUI_OnMouseUpEvent;
            drawPointTool.OnClick();
           // SetHome.setHome();
        }

        private void MainUI_OnMouseUpEvent(GMapRoute gMapRoute, GMapMarker gMapMarker)
        {
            if (frmLineWayPoints != null) {

                frmLineWayPoints.ChangWP(gMapRoute,gMapMarker);
            }
        }

        private void OnWayPointMovedEvent(GMapRoute gMapRoute,GMapMarker gMapMarker)
        {
            //计算航程
            if (frmLineWayPoints!=null)
            {
                frmLineWayPoints.CalculateTotalInfo(gMapRoute,gMapMarker);
            }

        }

        private void OnWayPointClickedEvent(GMapMarker gMapMarker)
        {
            if (frmLineWayPoints != null)
            {
                frmLineWayPoints.ClickWP(gMapMarker);
            }
        }

        private void OnWayPointDoubleClickedEvent(GMapMarker gMapMarker)
        {
            if (frmLineWayPoints!=null&& !frmLineWayPoints.isShown) {

                frmLineWayPoints.ShowForm();
            }
        }

        FrmLineWayPoints frmLineWayPoints;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gMapRoute"></param>
        /// <param name="gMapMarker"></param>
        private void DrawLineTool_OnDrawRouteFinishedEvent(GMapRoute gMapRoute, GMapMarker gMapMarker)
        {
            if (frmLineWayPoints == null)
            {
                frmLineWayPoints = GenericSingleton<FrmLineWayPoints>.CreateInstrance();
                frmLineWayPoints.Owner = this;
                frmLineWayPoints.StartPosition = FormStartPosition.Manual;
                frmLineWayPoints.Width = 368;
                frmLineWayPoints.Location = new Point(this.mainPanel.Width - frmLineWayPoints.Width, this.topPanel.Height + 25);
                frmLineWayPoints.isShown = true;
                frmLineWayPoints.Show();
            }
            else if(!frmLineWayPoints.isShown)frmLineWayPoints.ShowForm();

            if (frmLineWayPoints != null)
            {
                frmLineWayPoints.CalculateTotalInfo(gMapRoute,gMapMarker);
                frmLineWayPoints.SetWayPoints(gMapRoute,gMapMarker);
                frmLineWayPoints.mapControl = this.mMapControl;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            comPort.giveComport = false;

            log.Info("MenuConnect Start");

            // sanity check
            if (comPort.BaseStream.IsOpen && comPort.MAV.cs.groundspeed > 4)
            {
                if ((int)DialogResult.No ==
                    CustomMessageBox.Show(Strings.Stillmoving, Strings.Disconnect, MessageBoxButtons.YesNo))
                {
                    return;
                }
            }

            try
            {
                log.Info("Cleanup last logfiles");
                // cleanup from any previous sessions
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.ErrorClosingLogFile + ex.Message, Strings.ERROR);
            }

            comPort.logfile = null;
            comPort.rawlogfile = null;
           
            // decide if this is a connect or disconnect
            if (comPort.BaseStream.IsOpen)
            {
                doDisconnect(comPort);
            }
            else
            {
              doConnect(comPort, _connectionControl.CMB_serialport.Text, _connectionControl.CMB_baudrate.Text);
            }

            _connectionControl.UpdateSysIDS();

            loadph_serial();
        }

        void loadph_serial()
        {
            try
            {
                if (comPort.MAV.SerialString == "") return;

                var serials = File.ReadAllLines("ph2_serial.csv");

                foreach (var serial in serials)
                {
                    if (serial.Contains(comPort.MAV.SerialString.Substring(comPort.MAV.SerialString.Length - 26, 26)) &&
                        !Settings.Instance.ContainsKey(comPort.MAV.SerialString.Replace(" ", "")))
                    {
                        CustomMessageBox.Show(
                            "Your board has a Critical service bulletin please see [link;http://discuss.ardupilot.org/t/sb-0000001-critical-service-bulletin-for-beta-cube-2-1/14711;Click here]",
                            Strings.ERROR);

                        Settings.Instance[comPort.MAV.SerialString.Replace(" ", "")] = true.ToString();
                    }
                }
            }
            catch
            {

            }
        }

        private void doDisconnect(MAVLinkInterface comPort)
        {
            log.Info("We are disconnecting");
            try
            {
                if (speechEngine != null) // cancel all pending speech
                    speechEngine.SpeakAsyncCancelAll();
                comPort.BaseStream.DtrEnable = false;
                comPort.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            // now that we have closed the connection, cancel the connection stats
            // so that the 'time connected' etc does not grow, but the user can still
            // look at the now frozen stats on the still open form
            try
            {
                // if terminal is used, then closed using this button.... exception
                if (this.connectionStatsForm != null)
                    ((ConnectionStats)this.connectionStatsForm.Controls[0]).StopUpdates();
            }
            catch
            {
            }

            //// refresh config window if needed
            //if (MyView.current != null)
            //{
            //    if (MyView.current.Name == "HWConfig")
            //        MyView.ShowScreen("HWConfig");
            //    if (MyView.current.Name == "SWConfig")
            //        MyView.ShowScreen("SWConfig");
            //}

            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    try
                    {
                        Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                    );
            }
            catch
            {
            }

          this.btnConnect.Image = global::SKYROVER.GCS.DeskTop.Properties.Resources.light_connect_icon;
        }

        /// <summary>
        /// 连接飞控系统
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="portname"></param>
        /// <param name="baud"></param>
        /// <param name="getparams"></param>
        public void doConnect(MAVLinkInterface comPort, string portname, string baud, bool getparams = true)
        {
            bool skipconnectcheck = false;
            log.Info("We are connecting to " + portname + " " + baud);
            switch (portname)
            {
                case "preset":
                    skipconnectcheck = true;
                    if (comPort.BaseStream is TcpSerial)
                        _connectionControl.CMB_serialport.Text = "TCP";
                    if (comPort.BaseStream is UdpSerial)
                        _connectionControl.CMB_serialport.Text = "UDP";
                    if (comPort.BaseStream is UdpSerialConnect)
                        _connectionControl.CMB_serialport.Text = "UDPCl";
                    if (comPort.BaseStream is SerialPort)
                    {
                        _connectionControl.CMB_serialport.Text = comPort.BaseStream.PortName;
                        _connectionControl.CMB_baudrate.Text = comPort.BaseStream.BaudRate.ToString();
                    }
                    break;
                case "TCP":
                    comPort.BaseStream = new TcpSerial();
                    _connectionControl.CMB_serialport.Text = "TCP";
                    break;
                case "UDP":
                    comPort.BaseStream = new UdpSerial();
                    _connectionControl.CMB_serialport.Text = "UDP";
                    break;
                case "UDPCl":
                    comPort.BaseStream = new UdpSerialConnect();
                    _connectionControl.CMB_serialport.Text = "UDPCl";
                    break;
                case "AUTO": // do autoscan
                    MissionPlanner.Comms.CommsSerialScan.Scan(true);
                    DateTime deadline = DateTime.Now.AddSeconds(50);
                    while (MissionPlanner.Comms.CommsSerialScan.foundport == false || MissionPlanner.Comms.CommsSerialScan.run == 1)
                    {
                        System.Threading.Thread.Sleep(100);

                        if (DateTime.Now > deadline)
                        {
                            CustomMessageBox.Show(Strings.Timeout);
                            _connectionControl.IsConnected(false);
                            return;
                        }
                    }
                    return;
                default:
                    comPort.BaseStream = new SerialPort();
                    break;
            }

            // Tell the connection UI that we are now connected.
            _connectionControl.IsConnected(true);

            // Here we want to reset the connection stats counter etc.
            this.ResetConnectionStats();

            comPort.MAV.cs.ResetInternals();

            //cleanup any log being played
            comPort.logreadmode = false;
            if (comPort.logplaybackfile != null)
                comPort.logplaybackfile.Close();
            comPort.logplaybackfile = null;
           
           try
           {
               log.Info("Set Portname");
               // set port, then options
               if (portname.ToLower() != "preset")
                   comPort.BaseStream.PortName = portname;

               log.Info("Set Baudrate");
               try
               {
                   if (baud != "" && baud != "0")
                       comPort.BaseStream.BaudRate = int.Parse(baud);
               }
               catch (Exception exp)
               {
                   log.Error(exp);
               }

               // prevent serialreader from doing anything
               comPort.giveComport = true;

               log.Info("About to do dtr if needed");
               // reset on connect logic.
               if (Settings.Instance.GetBoolean("CHK_resetapmonconnect") == true)
               {
                   log.Info("set dtr rts to false");
                   comPort.BaseStream.DtrEnable = false;
                   comPort.BaseStream.RtsEnable = false;

                   comPort.BaseStream.toggleDTR();
               }

               comPort.giveComport = false;

               // setup to record new logs
               try
               {
                   Directory.CreateDirectory(Settings.Instance.LogDir);
                   lock (this)
                   {
                       // create log names
                       var dt = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                       var tlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                  dt + ".tlog";
                       var rlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                  dt + ".rlog";

                       // check if this logname already exists
                       int a = 1;
                       while (File.Exists(tlog))
                       {
                           Thread.Sleep(1000);
                           // create new names with a as an index
                           dt = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-" + a.ToString();
                           tlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                  dt + ".tlog";
                           rlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                  dt + ".rlog";
                       }

                       //open the logs for writing
                       comPort.logfile =
                           new BufferedStream(File.Open(tlog, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                       comPort.rawlogfile =
                           new BufferedStream(File.Open(rlog, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                       log.Info("creating logfile " + dt + ".tlog");
                   }
               }
               catch (Exception exp2)
               {
                   log.Error(exp2);
                   CustomMessageBox.Show(Strings.Failclog);
               } // soft fail

               // reset connect time - for timeout functions
               connecttime = DateTime.Now;

                // do the connect
               comPort.ProgressReportEvent += ComPort_ProgressReportEvent;

                comPort.OpenComport(false, skipconnectcheck,false);

               if (!comPort.BaseStream.IsOpen)
               {
                   log.Info("comport is closed. existing connect");
                   try
                   {
                       _connectionControl.IsConnected(false);
                       UpdateConnectIcon();
                       comPort.Close();
                   }
                   catch
                   {
                   }
                   return;
               }

               if (getparams)
                   comPort.getParamList();

               _connectionControl.UpdateSysIDS();

               // detect firmware we are conected to.
               if (comPort.MAV.cs.firmware == Firmwares.ArduCopter2)
               {
                   _connectionControl.TOOL_APMFirmware.SelectedIndex =
                       _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduCopter2);
               }
               else if (comPort.MAV.cs.firmware == Firmwares.Ateryx)
               {
                   _connectionControl.TOOL_APMFirmware.SelectedIndex =
                       _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.Ateryx);
               }
               else if (comPort.MAV.cs.firmware == Firmwares.ArduRover)
               {
                   _connectionControl.TOOL_APMFirmware.SelectedIndex =
                       _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduRover);
               }
               else if (comPort.MAV.cs.firmware == Firmwares.ArduSub)
               {
                   _connectionControl.TOOL_APMFirmware.SelectedIndex =
                       _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduSub);
               }
               else if (comPort.MAV.cs.firmware == Firmwares.ArduPlane)
               {
                   _connectionControl.TOOL_APMFirmware.SelectedIndex =
                       _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduPlane);
               }

               // check for newer firmware
               var softwares = Firmware.LoadSoftwares();

               if (softwares.Count > 0)
               {
                   try
                   {
                       string[] fields1 = comPort.MAV.VersionString.Split(' ');

                       foreach (Firmware.software item in softwares)
                       {
                           string[] fields2 = item.name.Split(' ');

                           // check primare firmware type. ie arudplane, arducopter
                           if (fields1[0] == fields2[0])
                           {
                               Version ver1 = VersionDetection.GetVersion(comPort.MAV.VersionString);
                               Version ver2 = VersionDetection.GetVersion(item.name);

                               if (ver2 > ver1)
                               {
                                    Common.MessageShowAgain(Strings.NewFirmware + "-" + item.name,
                                       Strings.NewFirmwareA + item.name + Strings.Pleaseup);
                                   break;
                               }

                               // check the first hit only
                               break;
                           }
                       }
                   }
                   catch (Exception ex)
                   {
                       log.Error(ex);
                   }
               }

            //   FlightData.CheckBatteryShow();

               Utilities.Tracking.AddEvent("Connect", "Connect", comPort.MAV.cs.firmware.ToString(),
                   comPort.MAV.param.Count.ToString());
              Utilities.Tracking.AddTiming("Connect", "Connect Time",
                   (DateTime.Now - connecttime).TotalMilliseconds, "");

              Utilities.Tracking.AddEvent("Connect", "Baud", comPort.BaseStream.BaudRate.ToString(), "");

               if (comPort.MAV.param.ContainsKey("SPRAY_ENABLE"))
                  Utilities.Tracking.AddEvent("Param", "Value", "SPRAY_ENABLE", comPort.MAV.param["SPRAY_ENABLE"].ToString());

               if (comPort.MAV.param.ContainsKey("CHUTE_ENABLE"))
                   Utilities.Tracking.AddEvent("Param", "Value", "CHUTE_ENABLE", comPort.MAV.param["CHUTE_ENABLE"].ToString());

               if (comPort.MAV.param.ContainsKey("TERRAIN_ENABLE"))
                   Utilities.Tracking.AddEvent("Param", "Value", "TERRAIN_ENABLE", comPort.MAV.param["TERRAIN_ENABLE"].ToString());

               if (comPort.MAV.param.ContainsKey("ADSB_ENABLE"))
                   Utilities.Tracking.AddEvent("Param", "Value", "ADSB_ENABLE", comPort.MAV.param["ADSB_ENABLE"].ToString());

               if (comPort.MAV.param.ContainsKey("AVD_ENABLE"))
                   Utilities.Tracking.AddEvent("Param", "Value", "AVD_ENABLE", comPort.MAV.param["AVD_ENABLE"].ToString());

               // save the baudrate for this port
               Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"] = _connectionControl.CMB_baudrate.Text;

               this.Text = "Sky Rover GCS "+comPort.MAV.VersionString;
        

               // load wps on connect option.
               if (Settings.Instance.GetBoolean("loadwpsonconnect") == true)
               {
                   // only do it if we are connected.
                   if (comPort.BaseStream.IsOpen)
                   {
                       //ZHAO
                       //MenuFlightPlanner_Click(null, null);
                       //FlightPlanner.BUT_read_Click(null, null);
                   }
               }

               // get any rallypoints
               if (MainUI.comPort.MAV.param.ContainsKey("RALLY_TOTAL") &&
                   int.Parse(MainUI.comPort.MAV.param["RALLY_TOTAL"].ToString()) > 0)
               {
                  //ZHAO
                  // FlightPlanner.getRallyPointsToolStripMenuItem_Click(null, null);

                   double maxdist = 0;

                   foreach (var rally in comPort.MAV.rallypoints)
                   {
                       foreach (var rally1 in comPort.MAV.rallypoints)
                       {
                           var pnt1 = new PointLatLngAlt(rally.Value.lat / 10000000.0f, rally.Value.lng / 10000000.0f);
                           var pnt2 = new PointLatLngAlt(rally1.Value.lat / 10000000.0f, rally1.Value.lng / 10000000.0f);

                           var dist = pnt1.GetDistance(pnt2);

                           maxdist = Math.Max(maxdist, dist);
                       }
                   }

                   if (comPort.MAV.param.ContainsKey("RALLY_LIMIT_KM") &&
                       (maxdist / 1000.0) > (float)comPort.MAV.param["RALLY_LIMIT_KM"])
                   {
                       CustomMessageBox.Show(Strings.Warningrallypointdistance + " " +
                                             (maxdist / 1000.0).ToString("0.00") + " > " +
                                             (float)comPort.MAV.param["RALLY_LIMIT_KM"]);
                   }
               }

               // get any fences
               if (MainUI.comPort.MAV.param.ContainsKey("FENCE_TOTAL") &&
                   int.Parse(MainUI.comPort.MAV.param["FENCE_TOTAL"].ToString()) > 1 &&
                   MainUI.comPort.MAV.param.ContainsKey("FENCE_ACTION"))
               {
                  //zhao
                  // FlightPlanner.GeoFencedownloadToolStripMenuItem_Click(null, null);
               }

               // set connected icon
               this.btnConnect.Image = displayicons.disconnect;
           }
           catch (Exception ex)
           {
               log.Warn(ex);
               try
               {
                   _connectionControl.IsConnected(false);
                   UpdateConnectIcon();
                   comPort.Close();
               }
               catch (Exception ex2)
               {
                   log.Warn(ex2);
               }
               CustomMessageBox.Show("Can not establish a connection\n\n" + ex.Message);
               return;
           }

        }

        private void ComPort_ProgressReportEvent(int progress, string status)
        {
            showMessage(status);
        }

        private void ResetConnectionStats()
        {
            log.Info("Reset connection stats");
            // If the form has been closed, or never shown before, we need do nothing, as 
            // connection stats will be reset when shown
            if (this.connectionStatsForm != null && connectionStatsForm.Visible)
            {
                // else the form is already showing.  reset the stats
                this.connectionStatsForm.Controls.Clear();
                _connectionStats = new ConnectionStats(comPort);
                this.connectionStatsForm.Controls.Add(_connectionStats);
              
            }
        }


        private void btnAccount_Click(object sender, EventArgs e)
        {
            int hight = this.topPanel.Height + 25 ;

            if (nacellePanel == null)
            {
                nacellePanel = GenericSingleton<NacellePanel>.CreateInstrance();

                nacellePanel.Location = new Point(Screen.GetWorkingArea(this).Width - nacellePanel.Width - 2, hight);
                nacellePanel.Owner = this;
                nacellePanel.AfterNacelleParamsChangedEvent += NacellePanel_AfterNacelleParamsChangedEvent;
                nacellePanel.AfterNacelleConnecteChangedEvent += NacellePanel_AfterNacelleConnecteChangedEvent;
                nacellePanel.Show();
            }
            else
            {
                nacellePanel.Location = new Point(Screen.GetWorkingArea(this).Width - nacellePanel.Width - 2, hight);
                nacellePanel.SetNacellePanelShown();
            }
            if (nacellePanel.isShown)
            {
                
                nacellePanel.SetHotRedEdgeTempValue(50);//设置热红外温度
            }
        }


        /// <summary>
        /// 吊舱参数改变
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="pitch"></param>
        /// <param name="focalLength"></param>
        private void NacellePanel_AfterNacelleParamsChangedEvent(double heading, double pitch, double focalLength)
        {
            if (currentMarker != null )
            {

                (currentMarker as IPayload).DrawRegion(currentMarker.Position,comPort.MAV.cs.alt , (float)pitch, (float)heading, 0, 4.57143f, 3.42857f, (float)focalLength);
              
            }
        }
        private void NacellePanel_AfterNacelleConnecteChangedEvent(bool isConnected)
        {
            if (currentMarker != null )
            {
                (currentMarker as IPayload).ActiveSinglePod(isConnected);// DrawRegion((FlightData.currentPlaneMarker as GMapMarkerQuad).Position, CurrentState.my_alt, (float)pitch, (float)heading, 0, 23, 15, (float)focalLength);

            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnSurvey_Click(object sender, EventArgs e)
        {
            GMapPolygon mapPolygon=null;

            foreach (var lyr in mMapControl.Overlays)
            {
                if (lyr.Id == "WPMapOverlay"&& lyr.Polygons.Count > 0) {
                    mapPolygon = lyr.Polygons[0];
                    break;
                }
            }
            if (mapPolygon == null) return;
           

            if (surveyGrid == null)
            {
                surveyGrid =  GenericSingleton<SurveyGrid>.CreateInstrance();
                surveyGrid.Owner = this;
                surveyGrid.StartPosition = FormStartPosition.Manual;
                surveyGrid.Width = 368;
                surveyGrid.Location = new Point(this.mainPanel.Width - surveyGrid.Width, this.topPanel.Height + 25);
                surveyGrid.AfterTripParamsChangeEvent += SurveyGrid_AfterTripParamsChangeEvent;
               // surveyGrid.MapPolygon = mapPolygon;
                surveyGrid.Show() ;
            }
            else surveyGrid.ShowForm();
           
            surveyGrid.MapPolygon = mapPolygon;
            surveyGrid.MapControl = mMapControl;

            if (tripHelper == null && surveyGrid != null && !surveyGrid.isShown)
            {
                tripHelper = new TripHelper(mMapControl, surveyGrid.getDataGridView, surveyGrid.MapPolygon, surveyGrid.TripModel, surveyGrid.DisplayInfos);
                tripHelper.Marker = null;
                tripHelper.runCalculate();
            } 
        }

        TripHelper tripHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripModel"></param>
        private void SurveyGrid_AfterTripParamsChangeEvent(GMapPolygon gMapPolygon, TripModel tripModel,DisplayInfos displayInfos)
        {
           
            tripHelper.CalculateSurveyGrid(gMapPolygon, tripModel, displayInfos);

            if (surveyGrid != null) surveyGrid.SetTripResultValue(tripHelper.GetTripResult);

        }

        private void btnConfigPlane_Click(object sender, EventArgs e)
        {
            ConfigParameters frm = new ConfigParameters();
            frm.Activate();
            frm.Enabled = true;
            frm.ShowDialog();
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            if (this.vlcControl != null)
            {

                vlcControl.SetNacellePanelShown();
            }
            else {

                vlcControl = UIManager.CreateVideoPanel();

                if (videoPanel == null) videoPanel = vlcControl.getVideoPanel;
                vlcControl.getClickPanel.MouseDoubleClick += GetVLCControl_MouseDoubleClick;
                vlcControl.getClickPanel.MouseClick += GetVLCControl_MouseClick;

            }
        }

        private void GetVLCControl_MouseClick(object sender, MouseEventArgs e)
        {
                     
            if (setting!=null&&setting.nacelle != null)
            {
                //
                
                float xScale = (float)e.X / vlcControl.getClickPanel.Width;
                float yScale = (float)e.Y / vlcControl.getClickPanel.Height;

                float X = xScale * 1920;
                float Y = yScale * 1080;
                showMessage("鼠标点击时坐标 X=" + X + ",Y=" + Y);
                setting.nacelle.track_pos(X,Y);


            }
        }

       

       
        private void MainUI_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BGCreateMaps(object state)
        {
            // sort logs
            try
            {
                Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));

                Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.rlog"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                // create maps
               MissionPlanner.Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog", SearchOption.AllDirectories));
                MissionPlanner.Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.bin", SearchOption.AllDirectories));
                MissionPlanner.Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.log", SearchOption.AllDirectories));

                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    MissionPlanner.Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    MissionPlanner.Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void BGLoadAirports(object nothing)
        {
            // read airport list
            try
            {
                Utilities.Airports.ReadOurairports(Settings.GetRunningDirectory() +
                                                   "airports.csv");

                Utilities.Airports.checkdups = true;

                //Utilities.Airports.ReadOpenflights(Application.StartupPath + Path.DirectorySeparatorChar + "airports.dat");

                log.Info("Loaded " + Utilities.Airports.GetAirportCount + " airports");
            }
            catch
            {
            }
        }


        /****
         * 
         * 
        Size beforeSize;

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            beforeSize=this.Size;

        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            Size endSize = this.Size;
            float parentWidth = (float)endSize.Width/ beforeSize.Width;
            float percentHeight =(float)endSize.Height/ beforeSize.Height;


            foreach (Control ctl in this.Controls)
            {
                ctl.Width = (int)(parentWidth * ctl.Width);
                ctl.Height = (int)(percentHeight * ctl.Height);

                ctl.Left = (int)(ctl.Left * parentWidth);
                ctl.Top=(int)(ctl.Top* percentHeight);

            }


        }*/


    }
}
