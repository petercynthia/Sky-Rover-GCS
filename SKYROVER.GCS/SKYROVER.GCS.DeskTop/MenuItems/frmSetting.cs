using CellconCore;
using GMap.NET;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.MenuItems
{

    public delegate void TurnOnMegaPhone(bool active);
    public delegate void SinglePodSwitched(bool active);
    public delegate void RadiusValueChanged(int radius);
    public delegate void UAVPayloadChanged(MissionPlanner.Maps.GMapMarkerQuadType type);
    public delegate void AfterSinglePodParametersChanged(PointLatLng point, float UAVHight,  float podPitch,  float heading,float roll, float CCDWidth, float CCDHight, float PodFocus);


    /// <summary>
    /// 吊舱参数发生变化
    /// </summary>
    /// <param name="heading">方位角</param>
    /// <param name="pitch">俯仰角</param>
    /// <param name="focalLength">焦距</param>
    public delegate void AfterNacelleParamsChanged(double heading, double pitch, double focalLength);
    /// <summary>
    /// 吊舱是否链接
    /// </summary>
    /// <param name="isConnected">链接为true，断开为false</param>
    public delegate void AfterNacelleConnecteChanged(bool isConnected);
    /// <summary>
    /// 
    /// </summary>
    public partial class frmSetting : Form
    {

        /// <summary>
        /// 窗体动画函数    注意：要引用System.Runtime.InteropServices;
        /// </summary>
        /// <param name="hwnd">指定产生动画的窗口的句柄</param>
        /// <param name="dwTime">指定动画持续的时间</param>
        /// <param name="dwFlags">指定动画类型，可以是一个或多个标志的组合。</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        //下面是可用的常量，根据不同的动画效果声明自己需要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志该标志
        private const int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展
        private const int AW_HIDE = 0x10000;//隐藏窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口，在使用了AW_HIDE标志后不要使用这个标志
        private const int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略
        private const int AW_BLEND = 0x80000;//使用淡入淡出效果

        public event TurnOnMegaPhone TurnOnMegaPhoneEvent;
        public event RadiusValueChanged RadiusValueChangedEvent;

        public event AfterSinglePodParametersChanged AfterSinglePodParametersChangedEvent;
        public event SinglePodSwitched SinglePodSwitchedEvent;

        public Nacelle nacelle;
        private myGMAP mapControl;
        public myGMAP MapControl { get => mapControl; set=> mapControl= value; }
        
        //窗體是否顯示
        public bool isShown = false;

        public void SetUAVStyleValue(bool active,int radius) {

            //this.megaPhoneSwtich.Switched = active;
            //this.RadiusValue.Value = radius;
        }

        public frmSetting()
        {
            InitializeComponent();
            this.Load += FrmSetting_Load;
           
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            isShown = true;
            AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_ACTIVE | AW_HOR_NEGATIVE);
            //吊舱
            nacelle = this.nacellControl1.nacell;
            //初始化投掷器
            InitJettisonConfig();

            //初始化离线地图控件
            InitOfflineMap();
        }
       

        #region 吊舱   



        //private void singlePodSwitch_SwitchedChanged(object sender)
        //{

        //    if (SinglePodSwitchedEvent != null) SinglePodSwitchedEvent(singlePodSwitch.Switched);
        //}



        #endregion

        #region 扩音器
        private void megaPhoneSwtich_SwitchedChanged(object sender)
        {
            if (TurnOnMegaPhoneEvent != null) TurnOnMegaPhoneEvent(megaPhoneSwtich.Switched);
        }

        //private void metroSetTrackBar1_Scroll(object sender)
        //{
        //    if (RadiusValueChangedEvent != null)
        //    {

        //        RadiusValueChangedEvent(RadiusValue.Value);
        //    }
        //}
        #endregion

        #region 投放器
        /// <summary>
        /// 
        /// </summary>
        private void InitJettisonConfig()
        {
            //获取投掷器参数 
            string txtServoChanel1 = "";
            if (Settings.config["txtServoChanel1"] != null)
                txtServoChanel1 = Settings.config["txtServoChanel1"];

            string txtSC1PWMOn = Settings.config["txtSC1PWMOn"];
            string txtSC1PWMOff = Settings.config["txtSC1PWMOff"];

            string txtServoChanel2 = Settings.config["txtServoChanel2"];
            string txtSC2PWMOn = Settings.config["txtSC2PWMOn"];
            string txtSC2PWMOff = Settings.config["txtSC2PWMOff"];

            this.ctlJettisonConfig1.InitControl(txtServoChanel1, txtSC1PWMOn, txtSC1PWMOff, txtServoChanel2, txtSC1PWMOn, txtSC1PWMOff);

            this.ctlJettisonConfig1.SaveJettisonConfigEvent += CtlJettisonConfig1_SaveJettisonConfigEvent;
        }

        private void CtlJettisonConfig1_SaveJettisonConfigEvent(string txtServoChanell, string txtSC1PWMOn, string txtSC1PWMOff, string txtServoChanel2, string txtSC2PWMOn, string txtSC2PWMOff)
        {
            Settings.config["txtServoChanel1"] = txtServoChanell;
            Settings.config["txtSC1PWMOn"] = txtSC1PWMOn;
            Settings.config["txtSC1PWMOff"] = txtSC1PWMOff;

            Settings.config["txtServoChane12"] = txtServoChanel2;
            Settings.config["txtSC2PWMOn"] = txtSC2PWMOn;
            Settings.config["txtSC2PWMOff"] = txtSC2PWMOff;

            Settings.Instance.Save();
        }


        #endregion

        #region 离线地图

        private void InitOfflineMap() {

            this.offlineMap1.MMapControl = this.mapControl;
        }

        #endregion


        public void SetPanelShown() {
            if(isShown) AnimateWindow(this.Handle, 500, AW_SLIDE | AW_HIDE | AW_HOR_POSITIVE);
            else AnimateWindow(this.Handle, 500, AW_SLIDE | AW_ACTIVE | AW_HOR_NEGATIVE);
            isShown = !isShown;
        }

        #region 移動窗體


        private bool m_isDown = false;
        private System.Drawing.Point m_lastMousePosition;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            m_isDown = true;
            m_lastMousePosition = new System.Drawing.Point(e.X, e.Y);

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isDown)
            {

                int x = e.X - m_lastMousePosition.X;
                int y = e.Y - m_lastMousePosition.Y;

                this.Location = new System.Drawing.Point(this.Location.X + x, this.Location.Y + y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            m_isDown = false;
        }

        #endregion

      

        private void metroSetTabControl3_TabIndexChanged(object sender, EventArgs e)
        {
            if (metroSetTabControl3.SelectedIndex == 2)
                this.configRadioInput1.Activate();
            else this.configRadioInput1.Deactivate();
        }

        private void metroSetTabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (metroSetTabControl3.SelectedIndex == 2) {
                this.configRadioInput1.Activate();
               // this.configMotorTest1.Deactivate();
            }
               
            else if (metroSetTabControl3.SelectedIndex == 3) {
                this.configMotorTest1.Activate();
                this.configRadioInput1.Deactivate();
            }
           
        }

        private void phoneEnvirment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int radius = 0;
            switch (phoneEnvirment.SelectedIndex) {
                case 0:
                    radius = 5000;                   
                    break;
                case 1:
                    radius = 3000;                  
                    break;
                case 2:
                    radius = 1000;                  
                    break;
                default:
                    radius = 0;
                    break;
            }
            this.lblRadius.Text = "覆盖半径（"+radius+" m）";
            RadiusValueChangedEvent(radius);
        }
    }
}
