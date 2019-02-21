
using CellconCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.Payloads
{
    /// <summary>
    /// 吊舱参数发生变化
    /// </summary>
    /// <param name="heading">方位角</param>
    /// <param name="pitch">俯仰角</param>
    /// <param name="focalLength">焦距</param>
    public delegate void AfterNacelleParamsChanged(double heading,double pitch,double focalLength);
    /// <summary>
    /// 吊舱是否链接
    /// </summary>
    /// <param name="isConnected">链接为true，断开为false</param>
    public delegate void AfterNacelleConnecteChanged(bool isConnected);
    
    /// <summary>
    /// 吊舱面板
    /// </summary>
    public partial class NacellePanel : Form
    {
        /// <summary>
        /// 吊舱参数变化事件
        /// </summary>
        public event AfterNacelleParamsChanged AfterNacelleParamsChangedEvent;
        /// <summary>
        /// 吊舱链接事件
        /// </summary>
        public event AfterNacelleConnecteChanged AfterNacelleConnecteChangedEvent;

        int HotRedEdgeTemp = 0;
        /// <summary>
        /// 设置温度阈值
        /// </summary>
        /// <param name="temp"></param>
        public void SetHotRedEdgeTempValue(int temp) {

            HotRedEdgeTemp = temp;
        }


        /// <summary>
        /// 窗体动画函数 注意：要引用System.Runtime.InteropServices;
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

        /// <summary>
        /// 吊舱控制类
        /// </summary>
        Nacelle nacell = new Nacelle();     
        /// <summary>
        /// 键盘钩子
        /// </summary>
        KeyboardHook k_hook;   
            
        UserConfig config = new UserConfig();    //将用户的config文件保存成object

        int BaudRate = 115200;                   //用于记录波特率
       
        //吊舱是否打开
        public bool IsNacelleOn { get; set; }

        Color normalColor = Color.FromArgb(255, 0, 0, 0);
        Color warnmingColor = Color.FromArgb(255, 255, 0, 0);


        #region 精度/比例
        float levelAZ = 15.0f;                   //用于记录方位轴转动幅度
        float levelEL = 15.0f;                   //用于记录俯仰轴转动幅度

        float levelJoyAZ = 15.0f;                //用于记录摇杆方位轴的控制比例
        float levelJoyEL = 15.0f;                //用于记录摇杆俯仰轴的控制比例

        int levelZoom = 4;                       //用于记录变焦幅度
        #endregion

        #region 摇杆死区
        double DeadZoneJoyAZ = 0.0;              //用于记录摇杆方位轴死区
        double DeadZoneJoyEL = 0.0;              //用于记录摇杆俯仰轴死区
        double DeadZoneJoyZoom = 0.0;            //用于记录摇杆变焦轴死区
        #endregion

        bool eis = false;                        //用于记录电子稳像开关
        int light = 0;                           //用于记录夜视切换
        bool width = false;                      //用于记录宽动态开关
        bool fog = false;                        //用于记录透雾开关


        byte[] arrRec = new byte[1024 * 60];     //udp接收区缓存 byte[]类型
        public string strRec = "";               //udp接收区缓存 string类型

        int stPort = 44454;
        UdpClient JoystickUdpClient;             //udp接收端，用于接收Python版本Joystick数据包
        IPEndPoint JoystickUdpClientIPEndPoint;  //用于配置udp的ip和port
        Thread JoystickReceiverThread;           //为udp接收单开线程

        Thread KBReceiverThread;                 //为keyboard轮询单开线程

        int tick_kb = 0;                //键盘轮询量
        bool joyChecked = false;        //用于检测配置文件是否超长
        JavaScriptSerializer json = new JavaScriptSerializer();

        JoyState joystate = new JoyState();
        int lengthBt = 0;
        //轴的数量
        int lengthAxe = 0;
        
        //方位轴旋转微调幅度
        public float AZ_tuning = 0.0f;
        //俯仰轴旋转微调幅度
        public float EL_tuning = 0.0f;

        public float AZ_tuning_tmp = 0.0f;

        public bool Flag_mode = false;

        float AZjoy = 0.0f;//摇杆方位
        float ELjoy = 0.0f;//摇杆俯仰
        int ZOOMjoy = 0;   //镜头缩放

        int azNo = 0;       // 方位
        int elNo = 0;       // 俯仰
        int zoomNo = 0;     // 变焦
        int zeroNo = 0;     // 回中
        int eisNo = 0;      // 电子稳像
        int lightNo = 0;    // 夜视切换
        int azpNo = 0;      // 方位微调左
        int azmNo = 0;      // 方位微调右
        int widthNo = 0;    // 宽动态开关
        int elpNo = 0;      // 俯仰微调上
        int elmNo = 0;      // 俯仰微调下
        int fogNo = 0;      // 透雾开关
        int takoNo = 0;     // 稳定模式
        int takNo = 0;      // 跟踪模式
        int takfNo = 0;     // 跟随模式
        int visibleNo = 0;  // 可见光
        int infraNo = 0;    // 热成像
        int startVideoNo = 0;   // 开始录像
        int stopVideoNo = 0;    // 停止录像
        int takePhotoNo = 0;    // 拍照

        double[] axeBias = { 0, 0, 0, 0 };  //用于记录摇杆的机械偏差

        //记录上次按键按下，其中【0=回中 | 1=电子稳像 | 2=夜视切换 | 3=方位微调左 | 4=方位微调右 | 5=宽动态开关 | 6=俯仰微调上 | 7=俯仰微调下 | 8=透雾开关 | 9=稳定模式 | 10=跟踪模式 | 11=跟随模式 | 12=可见光 | 13=热成像】
        int[] joyBtFlag = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        

        public NacellePanel()
        {
            InitializeComponent();

            IsNacelleOn = false;

            #region KeyboardHook
            k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += NacellePanel_KeyDown;
            k_hook.KeyUpEvent += NacellePanel_KeyUp;
            k_hook.Start();
            #endregion
            
            #region 根据config.txt对Joystick进行配置
            string configFile = AppDomain.CurrentDomain.BaseDirectory + "/config.txt";
            if (File.Exists(configFile)) {
                config = UserConfig.load(configFile);
                // axe
                azNo = config.方位;
                elNo = config.俯仰;
                zoomNo = config.变焦;
                // btn
                zeroNo = config.回中;
                eisNo = config.电子稳像;
                lightNo = config.夜视切换;
                azpNo = config.方位微调左;
                azmNo = config.方位微调右;
                widthNo = config.宽动态开关;
                elpNo = config.俯仰微调上;
                elmNo = config.俯仰微调下;
                fogNo = config.透雾开关;
                takoNo = config.稳定模式;
                takNo = config.跟踪模式;
                takfNo = config.跟随模式;
                visibleNo = config.可见光;
                infraNo = config.热成像;
                startVideoNo = config.开始录像;
                stopVideoNo = config.停止录像;
                takePhotoNo = config.拍照;
                //轴
                axeBias[0] = config.轴0偏差;
                axeBias[1] = config.轴1偏差;
                axeBias[2] = config.轴2偏差;
                axeBias[3] = config.轴3偏差;
                //精度
                levelAZ = config.按键方位轴精度;
                levelEL = config.按键俯仰轴精度;
                levelJoyAZ = config.摇杆方位轴精度;
                levelJoyEL = config.摇杆俯仰轴精度;
                levelZoom = config.变焦精度;
                //死区
                DeadZoneJoyAZ = config.方位死区;
                DeadZoneJoyEL = config.俯仰死区;
                DeadZoneJoyZoom = config.变焦死区;
                //波特率
                BaudRate = config.波特率;
                baud.Text = BaudRate.ToString();
            }

            #endregion

            #region  配置joystick接收端，启动新线程JoystickReceiverThread用于接收遥控器输入信息
            try
            {
                JoystickUdpClientIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), stPort);
                JoystickUdpClient = new UdpClient(JoystickUdpClientIPEndPoint);
                JoystickReceiverThread = new Thread(JoystickReceiver);
                JoystickReceiverThread.Start();

            } catch (Exception ex)
            {
                showMessage(ex.Message);
            }

            #endregion

            #region  启动新线程KBReceiverThread用于扫描按键中的轴状态
            KBReceiverThread = new Thread(searchKeyboard);
            KBReceiverThread.Name = "键盘监听线程";
            KBReceiverThread.Start();
            #endregion

        }
        
        
        /// <summary>
        /// 用于接收到joystick反馈
        /// </summary>
        private void JoystickReceiver()
        {
            while (true)
            {
                try
                {
                    
                    arrRec = JoystickUdpClient.Receive(ref JoystickUdpClientIPEndPoint);
                    strRec = Encoding.ASCII.GetString(arrRec);
                    //Console.WriteLine(strRec);
                    JoyControl getjson = json.Deserialize<JoyControl>(strRec);
                    if (!joyChecked)
                    {
                        checkConfigOverNum(getjson);
                        joyChecked = true;
                    }
                    explainCmd(getjson);    // 调用explainCmd方法实现对接收到的joystick数据进行处理
                }
                catch (Exception e)
                {
                    showMessage("joystick Receive get an exception:"+e.Message);
                    //Console.WriteLine(e.ToString());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="joy"></param>
        private void checkConfigOverNum(JoyControl joy)
        {
            lengthBt = joy.bt.Length;
            lengthAxe = joy.axe.Length;
            if (zeroNo >= lengthBt || eisNo >= lengthBt || lightNo >= lengthBt
                || azpNo >= lengthBt || azmNo >= lengthBt || widthNo >= lengthBt
                || elpNo >= lengthBt || elmNo >= lengthBt || fogNo >= lengthBt
                || takoNo >= lengthBt || takNo >= lengthBt || takfNo >= lengthBt
                || azNo >= lengthAxe || elNo >= lengthAxe || zoomNo >= lengthAxe
                || visibleNo >= lengthBt || infraNo >= lengthBt || startVideoNo >= lengthBt
                || stopVideoNo >= lengthBt || takePhotoNo >= lengthBt)
            {
                //showMessage("警告：手柄配置有误，如需使用，请修改后重启！");
                //MessageBox.Show("警告：手柄配置有误，存在空键！");
            }
        }

        /// <summary>
        /// 解析Joystick反馈信息，并将控制信息发送至串口发送队列对按键立刻响应，对摇杆进行轮询发送
        /// </summary>
        /// <param name="joy"></param>
        private void explainCmd(JoyControl joy)
        {
            try
            {
               
                if (zeroNo < lengthBt)
                {
                    if (joy.bt[zeroNo] == 1)
                    {
                        joyBtFlag[0]++;
                    }
                    else
                    {
                        joyBtFlag[0] = 0;
                    }
                }
                else
                {
                    joyBtFlag[0] = 0;
                }

                if (eisNo < lengthBt)
                {
                    if (joy.bt[eisNo] == 1)
                    {
                        joyBtFlag[1]++;
                    }
                    else
                    {
                        joyBtFlag[1] = 0;
                    }
                }
                else
                {
                    joyBtFlag[1] = 0;
                }

                if (lightNo < lengthBt)
                {
                    if (joy.bt[lightNo] == 1)
                    {
                        joyBtFlag[2]++;
                    }
                    else
                    {
                        joyBtFlag[2] = 0;
                    }
                }
                else
                {
                    joyBtFlag[2] = 0;
                }

                if (azpNo < lengthBt)
                {
                    if (joy.bt[azpNo] == 1)
                    {
                        joyBtFlag[3]++;
                    }
                    else
                    {
                        joyBtFlag[3] = 0;
                    }
                }
                else
                {
                    joyBtFlag[3] = 0;
                }

                if (azmNo < lengthBt)
                {
                    if (joy.bt[azmNo] == 1)
                    {
                        joyBtFlag[4]++;
                    }
                    else
                    {
                        joyBtFlag[4] = 0;
                    }
                }
                else
                {
                    joyBtFlag[4] = 0;
                }

                if (widthNo < lengthBt)
                {
                    if (joy.bt[widthNo] == 1)
                    {
                        joyBtFlag[5]++;
                    }
                    else
                    {
                        joyBtFlag[5] = 0;
                    }
                }
                else
                {
                    joyBtFlag[5] = 0;
                }
                if (elpNo < lengthBt)
                {
                    if (joy.bt[elpNo] == 1)
                    {
                        joyBtFlag[6]++;
                    }
                    else
                    {
                        joyBtFlag[6] = 0;
                    }
                }
                else
                {
                    joyBtFlag[6] = 0;
                }
                if (elmNo < lengthBt)
                {
                    if (joy.bt[elmNo] == 1)
                    {
                        joyBtFlag[7]++;
                    }
                    else
                    {
                        joyBtFlag[7] = 0;
                    }
                }
                else
                {
                    joyBtFlag[7] = 0;
                }
                if (fogNo < lengthBt)
                {
                    if (joy.bt[fogNo] == 1)
                    {
                        joyBtFlag[8]++;
                    }
                    else
                    {
                        joyBtFlag[8] = 0;
                    }
                }
                else
                {
                    joyBtFlag[8] = 0;
                }
                if (takoNo < lengthBt)
                {
                    if (joy.bt[takoNo] == 1)
                    {
                        joyBtFlag[9]++;
                    }
                    else
                    {
                        joyBtFlag[9] = 0;
                    }
                }
                else
                {
                    joyBtFlag[9] = 0;
                }
                if (takNo < lengthBt)
                {
                    if (joy.bt[takNo] == 1)
                    {
                        joyBtFlag[10]++;
                    }
                    else
                    {
                        joyBtFlag[10] = 0;
                    }
                }
                else
                {
                    joyBtFlag[10] = 0;
                }
                if (takfNo < lengthBt)
                {
                    if (joy.bt[takfNo] == 1)
                    {
                        joyBtFlag[11]++;
                    }
                    else
                    {
                        joyBtFlag[11] = 0;
                    }
                }
                else
                {
                    joyBtFlag[11] = 0;
                }
                if (visibleNo < lengthBt)
                {
                    if (joy.bt[visibleNo] == 1)
                    {
                        joyBtFlag[12]++;
                    }
                    else
                    {
                        joyBtFlag[12] = 0;
                    }
                }
                else
                {
                    joyBtFlag[12] = 0;
                }
                if (infraNo < lengthBt)
                {
                    if (joy.bt[infraNo] == 1)
                    {
                        joyBtFlag[13]++;
                    }
                    else
                    {
                        joyBtFlag[13] = 0;
                    }
                }
                else
                {
                    joyBtFlag[13] = 0;
                }
                if (startVideoNo < lengthBt)
                {
                    if (joy.bt[startVideoNo] == 1)
                    {
                        joyBtFlag[14]++;
                    }
                    else
                    {
                        joyBtFlag[14] = 0;
                    }
                }
                else
                {
                    joyBtFlag[14] = 0;
                }
                if (stopVideoNo < lengthBt)
                {
                    if (joy.bt[stopVideoNo] == 1)
                    {
                        joyBtFlag[15]++;
                    }
                    else
                    {
                        joyBtFlag[15] = 0;
                    }
                }
                else
                {
                    joyBtFlag[15] = 0;
                }
                if (takePhotoNo < lengthBt)
                {
                    if (joy.bt[takePhotoNo] == 1)
                    {
                        joyBtFlag[16]++;
                    }
                    else
                    {
                        joyBtFlag[16] = 0;
                    }
                }
                else
                {
                    joyBtFlag[16] = 0;
                }

                if (azNo < lengthAxe)  //发送方位轴控制指令
                {
                    float azTemp = joy.axe[azNo] - (float)axeBias[azNo];
                    if (System.Math.Abs(azTemp) > DeadZoneJoyAZ)
                    {
                        AZjoy = azTemp * levelJoyAZ;
                    }
                    else
                    {
                        AZjoy = 0.0f;
                    }
                }
                if (elNo < lengthAxe)  //发送俯仰轴控制指令
                {
                    float elTemp = joy.axe[elNo] - (float)axeBias[elNo];
                    if (System.Math.Abs(elTemp) > DeadZoneJoyEL)
                    {
                        ELjoy = elTemp * levelJoyEL;
                    }
                    else
                    {
                        ELjoy = 0.0f;
                    }
                }
                if (zoomNo < lengthAxe)        //发送变焦控制指令
                {
                    float zoomTemp = joy.axe[zoomNo] - (float)axeBias[zoomNo];
                    if (System.Math.Abs(zoomTemp) > DeadZoneJoyZoom)
                    {
                        int zoomValue = (int)(zoomTemp * levelZoom + 0.5);
                        if (System.Math.Abs(zoomValue) != 0)
                        {
                            ZOOMjoy = zoomValue;
                        }
                        else
                        {
                            ZOOMjoy = 0;
                        }
                    }
                    else
                    {
                        ZOOMjoy = 0;
                    }
                }


                // 所有开关量如果触发，立刻发送，但是一次触发仅发送一次
                if (joyBtFlag[0] == 1)  // 回中
                {
                    nacell.cmd_zero();
                    //udpCommandSend(nacell.command);

                }
                if (joyBtFlag[1] == 1)  // 电子稳像
                {
                    byte cmd = eis ? (byte)1 : (byte)0;
                    eis = !eis;
                    nacell.cmd_eis(cmd);
                    //udpCommandSend(nacell.command);
                }
                if (joyBtFlag[2] == 1)  // 夜视切换
                {
                    nacell.cmd_light((byte)light);
                    //udpCommandSend(nacell.command);
                    light = (light + 1) % 3;
                }
                if (joyBtFlag[3] == 1)  // 方位微调左
                {
                    if (!Flag_mode)
                    {
                        AZ_tuning += 0.03f;
                    }
                }
                if (joyBtFlag[4] == 1)  // 方位微调右
                {
                    if (!Flag_mode)
                    {
                        AZ_tuning -= 0.03f;
                    }
                }
                if (joyBtFlag[5] == 1)  // 宽动态开关
                {
                    byte cmd = width ? (byte)1 : (byte)0;
                    width = !width;
                    nacell.cmd_width(cmd);
                    //udpCommandSend(nacell.command);
                }
                if (joyBtFlag[6] == 1)  // 俯仰微调上
                {
                    if (!Flag_mode)
                    {
                        EL_tuning += 0.03f;
                    }
                }
                if (joyBtFlag[7] == 1)  // 俯仰微调下
                {
                    if (!Flag_mode)
                    {
                        EL_tuning -= 0.03f;
                    }
                }
                if (joyBtFlag[8] == 1)  // 透雾开关
                {
                    byte cmd = fog ? (byte)1 : (byte)0;
                    fog = !fog;
                    nacell.cmd_fog(cmd);
                    //udpCommandSend(nacell.command);
                }
                if (joyBtFlag[9] == 1)  // 稳定模式
                {
                    // 恢复方位轴微调设置
                    if (Flag_mode)
                    {    // 原本非稳定模式
                        AZ_tuning = AZ_tuning_tmp;
                        AZ_tuning_tmp = 0.0f;
                        Flag_mode = false;
                    }
                    // enable方位俯仰微调
                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    //{
                    //    btnAZP.IsEnabled = true;
                    //    btnAZM.IsEnabled = true;
                    //    btnAZZ.IsEnabled = true;
                    //    btnELP.IsEnabled = true;
                    //    btnELM.IsEnabled = true;
                    //    btnELZ.IsEnabled = true;
                    //});
                    nacell.cmd_mode(0);
                    //udpCommandSend(nacell.command);
                }
                if (joyBtFlag[10] == 1)  // 跟踪模式
                {
                    // 方位轴微调清零
                    if (!Flag_mode)
                    {   // 原本为稳定模式
                        AZ_tuning_tmp = AZ_tuning;
                        AZ_tuning = 0.0f;
                        Flag_mode = true;
                    }
                    // disable方位俯仰微调
                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    //{
                    //    btnAZP.IsEnabled = false;
                    //    btnAZM.IsEnabled = false;
                    //    btnAZZ.IsEnabled = false;
                    //    btnELP.IsEnabled = false;
                    //    btnELM.IsEnabled = false;
                    //    btnELZ.IsEnabled = false;
                    //});
                    nacell.cmd_mode(1);
                    //udpCommandSend(nacell.command);
                }
                if (joyBtFlag[11] == 1)  // 跟踪模式
                {
                    // 方位轴微调清零
                    if (!Flag_mode)
                    {   // 原本为稳定模式
                        AZ_tuning_tmp = AZ_tuning;
                        AZ_tuning = 0.0f;
                        Flag_mode = true;
                    }
                    // disable方位俯仰微调
                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    //{
                    //    btnAZP.IsEnabled = false;
                    //    btnAZM.IsEnabled = false;
                    //    btnAZZ.IsEnabled = false;
                    //    btnELP.IsEnabled = false;
                    //    btnELM.IsEnabled = false;
                    //    btnELZ.IsEnabled = false;
                    //});
                    nacell.cmd_mode(2);
                    //udpCommandSend(nacell.command);
                }
                // 可见光
                if (joyBtFlag[12] == 1)  
                {
                    nacell.visible();
                    //udpCommandSend(nacell.command);

                }
                // 热成像
                if (joyBtFlag[13] == 1)  
                {
                    nacell.infra();
                    //udpCommandSend(nacell.command);

                }
                // 开始录像
                if (joyBtFlag[14] == 1)  
                {
                    nacell.svideo();
                    //udpCommandSend(nacell.command);

                }
                if (joyBtFlag[15] == 1)  // 停止录像
                {
                    nacell.evideo();
                    //udpCommandSend(nacell.command);

                }
                if (joyBtFlag[16] == 1)  // 拍照
                {
                    nacell.capture();
                    //udpCommandSend(nacell.command);

                }
               
                //setMonitorStatus(joy);

              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 键盘
        /// </summary>
        private void searchKeyboard()
        {
            //吊舱链接上
            while (true)
            {
                if (IsNacelleOn) {

                // 根据当前joystick状态发送当前值
                switch (tick_kb)
                {
                    // 0 1 2 3 4 5 6 7
                    // x y x z y x y z
                    case 0:
                    case 2:
                    case 5: //此时为方位轴被触发
                        if (Math.Abs(AZjoy)> 0.0f)
                        {
                            nacell.cmd_left(AZjoy);
                            //udpCommandSend(nacell.command);
                            break;
                        }
                        if (joystate.left)
                        {
                            nacell.cmd_left(levelAZ + AZ_tuning);
                            //udpCommandSend(nacell.command);
                        }
                        else if (joystate.right)
                        {
                            nacell.cmd_left(-(levelAZ + AZ_tuning));
                            //udpCommandSend(nacell.command);
                        }
                        //else
                        //{
                        //    nacell.cmd_left(AZ_tuning);
                        ////    udpCommandSend(nacell.command);
                        //}
                        break;
                    case 1:
                    case 4:
                    case 6: //此时为俯仰轴被触发
                        if (Math.Abs(ELjoy)> 0.0f)
                        {
                            nacell.cmd_up(ELjoy);
                            //udpCommandSend(nacell.command);
                            break;
                        }
                        if (joystate.up)
                        {
                            nacell.cmd_up(levelEL + EL_tuning);
                            //udpCommandSend(nacell.command);
                        }
                        else if (joystate.down)
                        {
                            nacell.cmd_up(-(levelEL + EL_tuning));
                            //udpCommandSend(nacell.command);
                        }
                        //else
                        //{
                        //    nacell.cmd_up(EL_tuning);
                        //    //udpCommandSend(nacell.command);
                        //}
                        break;
                    case 3:
                    case 7: //此时为变焦被触发
                        if (ZOOMjoy != 0)
                        {
                            nacell.cmd_zoom(ZOOMjoy);
                            //udpCommandSend(nacell.command);
                            break;
                        }
                        if (joystate.zoomin)
                        {
                            nacell.cmd_zoom(levelZoom);
                            //udpCommandSend(nacell.command);
                        }
                        else if (joystate.zoomout)
                        {
                            nacell.cmd_zoom(-levelZoom);
                            //udpCommandSend(nacell.command);
                        }
                        else
                        {
                            nacell.cmd_zoom(0);
                            //udpCommandSend(nacell.command);
                        }
                        break;
                    default:
                        break;
                }
                tick_kb = (tick_kb + 1) % 8;
                }
                Thread.Sleep(120);
            }
           
        }

   
        private void NacellePanel_Load(object sender, EventArgs e)
        {          
            SetNacellePanelShown();
        }

        public bool isShown = false;
        /// <summary>
        /// 设置吊舱是否显示
        /// </summary>
        public void SetNacellePanelShown()
        {

            if (!isShown)
            {
                AnimateWindow(this.Handle, 600, AW_SLIDE | AW_ACTIVE | AW_HOR_NEGATIVE);
                k_hook.Start();
            }
            else
            {
                AnimateWindow(this.Handle, 600, AW_SLIDE | AW_HIDE | AW_HOR_POSITIVE);
                k_hook.Stop();
            }

            isShown =!isShown;
        }



        double Radiu = 180 / Math.PI;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //
           if (this.COMboBox.SelectedIndex < 0) { showMessage("请先连接吊舱串口！"); return; }

            if (btnConnect.Text == "连接")
            {

                try
                {
                    IsNacelleOn = true;
                    if (nacell.startString(this.COMboBox.Text, int.Parse(baud.Text)))
                    {
                        if (AfterNacelleConnecteChangedEvent != null) AfterNacelleConnecteChangedEvent(IsNacelleOn);
                       // nacell.uart_rx_event += Nacell_uart_rx_event;
                        btnConnect.Text = "断开";
                        COMboBox.Enabled = false;
                        //if (KBReceiverThread != null || KBReceiverThread.ThreadState == ThreadState.Stopped)
                        //    KBReceiverThread.Start();
                    }
                    else IsNacelleOn = false;
                   

                }
                catch (Exception ex)
                {
                    IsNacelleOn = false;
                    if (AfterNacelleConnecteChangedEvent != null) AfterNacelleConnecteChangedEvent(IsNacelleOn);
                   // nacell.uart_rx_event -= Nacell_uart_rx_event;
                    btnConnect.Text = "连接";
                    COMboBox.Enabled = true;
                   
                    this.receiveData.Invoke((Action)delegate {
                        receiveData.AppendText(ex.Message + "\n");
                        receiveData.ScrollToCaret();
                        receiveData.BackColor = normalColor;
                    });

                }
            }
            else
            {
                    
                IsNacelleOn = false;
                if (AfterNacelleConnecteChangedEvent != null) AfterNacelleConnecteChangedEvent(IsNacelleOn);
                COMboBox.Enabled = true;
                btnConnect.Text = "连接";
               // nacell.uart_rx_event -= Nacell_uart_rx_event;
                nacell.stop();
                this.receiveData.Invoke((Action)delegate {
                    receiveData.BackColor = normalColor;
                });
            }
        }
        int currentLengh = 0;
        int sumCount = 0;
        /// <summary>
        /// 接收并解析串口数据
        /// </summary>
        /// <param name="b"></param>
        private void Nacell_uart_rx_event(byte[] b)
        {
            //显示16进制数据
            //showMessage(DateTime.Now.ToString("HH:mm:ss") + ":" + byteToHexStr(b));
            //showMessage(DateTime.Now.ToShortTimeString() + "数据统计:" + (sumCount++) + "/" + b.Length);

            //数据为空或者长度不足，直接丢弃
            if (b == null || b.Length < 88)
            {
                this.receiveData.Invoke((Action)delegate {
                    receiveData.BackColor = normalColor;
                });
               
                return;
            }
                   
            //记录
            int X = 0;
            int Y = 0;
            int Z = 0;      
            int packIndex = 0;
            //88
            while ((packIndex + 88)<=b.Length)
            {
               //找到数据头
               if (b[packIndex] == 170&&b[packIndex+1]==1)
               {
                        Thread.Sleep(10);
                        //开始解析数据
                        X = (short)(b[packIndex + 15] << 8) + b[packIndex + 14];
                        Y = (short)(b[packIndex + 17] << 8) + b[packIndex + 16];
                        Z = (short)(b[packIndex + 19] << 8) + b[packIndex + 18];

                        double pitch = Radiu * Math.Atan2(-1 * Z,Math.Sqrt(X * X + Y * Y));
                        double yaw =90- Radiu * Math.Atan2(Y,X);
                        ////可见光焦距 4.3-129mm
                        double zoom = ((short)(b[packIndex + 85] << 8) + b[packIndex + 84]) * 0.43;
                        //？红外数据焦距，还需云汉确认
                        if (zoom == 0) zoom = 19.0;
                        //温度数据
                        double temp = ((short)(b[packIndex + 83] << 8) + b[packIndex + 82]) / 10;
                        // 
                        if (AfterNacelleParamsChangedEvent != null) AfterNacelleParamsChangedEvent(yaw, pitch, zoom);
                        //温度高于阈值，背景变红，否则正常                       
                        if (temp >= HotRedEdgeTemp) this.receiveData.Invoke((Action)delegate 
                        {
                            receiveData.BackColor = warnmingColor;
                        });
                        else this.receiveData.Invoke((Action)delegate {
                            receiveData.BackColor = normalColor;
                        });

                        // showMessage("方位：" + yaw + "° 俯仰：" + pitch + "°焦距：" + zoom + "mm; 温度：" + temp + "℃");
                    packIndex += 88;

                    }
                    else packIndex++;
                }
           
         
          
        }
        /// <summary>
        /// 16进制转成字节
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }


        // 字节数组转16进制字符串   
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 解析吊舱回传数据
        /// </summary>
        /// <param name="loc"></param>
        private void explainLocCmd(LocControl loc)
        {
            uint err_flag = loc.cgi_err_res & 0xff;
            receiveData.AppendText(DateTime.Now.ToShortTimeString() + ":" + "方位：" + loc.vs_z.ToString("0.0") + "° 俯仰：" + loc.vs_y.ToString("0.0") + "°" + "\n");
            receiveData.Focus();
            receiveData.Select(receiveData.TextLength, 0);
            receiveData.ScrollToCaret();
        }

        /// <summary>
        /// 获取串口数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void COMboBox_Click(object sender, EventArgs e)
        {
            PopulateSerialportList();
        }

        comm_list comList;
        /// <summary>
        /// 获取串口列表
        /// </summary>
        private void PopulateSerialportList()
        {

            comList = new comm_list();
            this.COMboBox.Items.Clear();           
            this.COMboBox.Items.AddRange(comList.GetSericalPortName());           
            if (this.COMboBox.Items.Count > 0) this.COMboBox.SelectedIndex= 0;
        }
        /// <summary>
        /// 航向精度控制
        /// </summary>
        /// <param name="sender"></param>
        private void KBYawRange_Scroll(object sender)
        {
            this.KBYawRangeValue.Text = ((this.KBYawRange.Value - 16) * 5).ToString();
            levelAZ = float.Parse(this.KBYawRangeValue.Text);
        }
        /// <summary>
        /// 俯仰精度控制
        /// </summary>
        /// <param name="sender"></param>
        private void KBPitchRange_Scroll(object sender)
        {
            this.KBPitchRangeValue.Text = ((this.KBPitchRange.Value - 16) * 5).ToString();
            levelEL = float.Parse(this.KBPitchRangeValue.Text);
        }
        /// <summary>
        /// 摇杆方位
        /// </summary>
        /// <param name="sender"></param>
        private void JoyYawRange_Scroll(object sender)
        {
            this.JoyYawRangeValue.Text = ((this.JoyYawRange.Value - 16) * 5).ToString();
            levelJoyAZ = float.Parse(this.JoyYawRangeValue.Text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void JoyPitchRange_Scroll(object sender)
        {
            this.JoyPitchRangeValue.Text = ((this.JoyPitchRange.Value - 16) * 5).ToString();
            levelJoyEL = float.Parse(this.JoyPitchRangeValue.Text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void FocalLengthRange_Scroll(object sender)
        {
            this.FocalLengthRangeValue.Text = this.FocalLengthRange.Value.ToString();
            levelZoom = int.Parse(this.FocalLengthRangeValue.Text);
        }

        /// <summary>
        /// 陀螺仪校准
        /// </summary>
        private void CalibrationIMU()
        {

            AZ_tuning = 0.0f;
            EL_tuning = 0.0f;
            nacell.cmd_gyro();
            showMessage("陀螺仪校准：" + EL_tuning.ToString());
            //udpCommandSend(nacell.command);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NacellePanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsNacelleOn) return;

            if (e.KeyCode == Keys.A)
            {
                showMessage("控制：吊舱向左转");
                joystate.setLeft();
            }
            else if (e.KeyCode == Keys.D)
            {
                showMessage("控制：吊舱向右转");
                joystate.setRight();
            }
            else if (e.KeyCode == Keys.W)
            {
                showMessage("控制：吊舱向上转");
                joystate.setUp();
            }
            else if (e.KeyCode == Keys.S)
            {
                showMessage("控制：吊舱向下转");
                joystate.setDown();
            }
            else if (e.KeyCode == Keys.F)
            {
                showMessage("控制：吊舱变焦+");
                joystate.setZoomIn();
            }
            else if (e.KeyCode == Keys.G)
            {
                showMessage("控制：吊舱变焦-");
                joystate.setZoomOut();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NacellePanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsNacelleOn) return;

            if (e.KeyCode == Keys.A)
            {
                showMessage("控制：释放控制-向左转");              
                joystate.releaseLeft();
            }
            else if (e.KeyCode == Keys.W)
            {
                
                showMessage("控制：释放吊舱控制-向上转");
                joystate.releaseUp();
            }
            else if (e.KeyCode == Keys.S)
            {
              
                showMessage("控制：释放吊舱控制-向下转");
                joystate.releaseDown();
            }
            else if (e.KeyCode == Keys.D)
            {
               
                showMessage("控制：释放吊舱控制-向右转");
                joystate.releaseRight();
            }
            else if (e.KeyCode == Keys.F)
            {
               
                showMessage("控制：释放吊舱控制-焦距+");
                joystate.releaseZoomIn();
            }
            else if (e.KeyCode == Keys.G)
            {
               showMessage("控制：释放吊舱控制-焦距-");
                joystate.releaseZoomOut();
            }
            else if (e.KeyCode == Keys.E)
            {
                showMessage("指令：回中");
                nacell.cmd_zero();
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.R)
            {
                showMessage("指令：电子稳像开");
                nacell.cmd_eis(1);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.T)
            {
                showMessage("指令：电子稳像关");
                nacell.cmd_eis(0);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.Y)
            {
                showMessage("指令：夜视关");
                nacell.cmd_light(0);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.U)
            {
                showMessage("指令：彩色夜视");
                nacell.cmd_light(1);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.I)
            {
                showMessage("指令：黑白夜视");
                nacell.cmd_light(2);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.O)
            {
                showMessage("指令：宽动态开");
                nacell.cmd_width(1);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.P)
            {
                showMessage("指令：宽动态关");
                nacell.cmd_width(0);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.H)
            {
                showMessage("指令：陀螺仪校准");
                AZ_tuning = 0.0f;
                EL_tuning = 0.0f;
                nacell.cmd_gyro();
              //  udpCommandSend(nacell.command);
            }
            //else if (e.KeyCode == Keys.J)
            //{
            //    showMessage("指令：电机开");
            //    nacell.cmd_machine(1);
            //    udpCommandSend(nacell.command);
            //}
            //else if (e.KeyCode == Keys.K)
            //{
            //    showMessage("指令：电机关");
            //    nacell.cmd_machine(0);
            //    udpCommandSend(nacell.command);
            //}
            //else if (e.KeyCode == Keys.H)
            //{
            //    showMessage("指令：开启自动校准");
            //    nacell.check(1);
            //    //udpCommandSend(nacell.command);
            //}
            //else if (e.KeyCode == Keys.J)
            //{
            //    showMessage("指令：关闭自动校准");
            //    nacell.check(0);
            //    //udpCommandSend(nacell.command);
            //}
            //else if (e.KeyCode == Keys.K)
            //{
            //    showMessage("指令：清除校准数据");
            //    nacell.check(2);
            //    //udpCommandSend(nacell.command);
            //}
            else if (e.KeyCode == Keys.Z)
            {
                showMessage("指令：透雾开");
                nacell.cmd_fog(1);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.X)
            {
                showMessage("指令：透雾关");
                nacell.cmd_fog(0);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.C)
            {
                AZ_tuning += 0.03f;
                showMessage("方位微调设置：" + Math.Round(AZ_tuning, 2).ToString());
            }
            else if (e.KeyCode == Keys.V)
            {
                AZ_tuning -= 0.03f;
                showMessage("方位微调设置：" + Math.Round(AZ_tuning, 2).ToString());
            }
            else if (e.KeyCode == Keys.B)
            {
                AZ_tuning = 0.0f;
                showMessage("方位微调设置：" + AZ_tuning.ToString());
            }
            else if (e.KeyCode == Keys.N)
            {
                EL_tuning += 0.03f;
                showMessage("俯仰微调设置：" + Math.Round(EL_tuning, 2).ToString());
            }
            else if (e.KeyCode == Keys.M)
            {
                EL_tuning -= 0.03f;
                showMessage("俯仰微调设置：" + Math.Round(EL_tuning, 2).ToString());
            }
            else if (e.KeyCode == Keys.L)
            {
                EL_tuning = 0.0f;
                showMessage("俯仰微调设置：" + EL_tuning.ToString());
            }
            else if (e.KeyCode == Keys.D1)
            {
                // 恢复方位轴微调设置
                if (Flag_mode)
                {    // 原本非稳定模式
                    AZ_tuning = AZ_tuning_tmp;
                    AZ_tuning_tmp = 0.0f;
                    Flag_mode = false;
                }
                showMessage("方位微调设置：" + AZ_tuning.ToString());
                // enable方位俯仰微调
                //btnAZP.IsEnabled = true;
                //btnAZM.IsEnabled = true;
                //btnAZZ.IsEnabled = true;
                //btnELP.IsEnabled = true;
                //btnELM.IsEnabled = true;
                //btnELZ.IsEnabled = true;
                nacell.cmd_mode(0);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.D2)
            {
                // 方位轴微调清零
                if (!Flag_mode)
                {   // 原本为稳定模式
                    AZ_tuning_tmp = AZ_tuning;
                    AZ_tuning = 0.0f;
                    Flag_mode = true;
                }
                showMessage("方位微调设置：" + AZ_tuning.ToString());
                // disable方位俯仰微调
                //btnAZP.IsEnabled = false;
                //btnAZM.IsEnabled = false;
                //btnAZZ.IsEnabled = false;
                //btnELP.IsEnabled = false;
                //btnELM.IsEnabled = false;
                //btnELZ.IsEnabled = false;
                nacell.cmd_mode(1);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.D3)
            {
                // 方位轴微调清零
                if (!Flag_mode)
                {   // 原本为稳定模式
                    AZ_tuning_tmp = AZ_tuning;
                    AZ_tuning = 0.0f;
                    Flag_mode = true;
                }
                showMessage("方位微调设置：" + AZ_tuning.ToString());
                // disable方位俯仰微调
                //btnAZP.IsEnabled = false;
                //btnAZM.IsEnabled = false;
                //btnAZZ.IsEnabled = false;
                //btnELP.IsEnabled = false;
                //btnELM.IsEnabled = false;
                //btnELZ.IsEnabled = false;
                nacell.cmd_mode(2);
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.D4)
            {
                showMessage("指令：可见光");
                nacell.visible();
                //udpCommandSend(nacell.command);
            }
            else if (e.KeyCode == Keys.D5)
            {
                showMessage("指令：热成像");
                nacell.infra();
                //udpCommandSend(nacell.command);
            }
            //else if (e.KeyCode == Keys.D4)
            //{
            //    showMessage("指令：1080P25帧");
            //    nacell.cmd_1080();
            //    udpCommandSend(nacell.command);
            //}
            //else if (e.KeyCode == Keys.D5)
            //{
            //    showMessage("指令：720P60帧");
            //    nacell.cmd_720();
            //    udpCommandSend(nacell.command);
            //}
        }

        delegate void ShowMessageDelegate(string message);

        /// <summary>
        /// 面板显示信息
        /// </summary>
        /// <param name="message"></param>
        private void showMessage(string message)
        {
            if (message != "")
            {
                if (this.receiveData.InvokeRequired)
                {
                    ShowMessageDelegate showMessageDelegate = new ShowMessageDelegate(showMessage);
                    BeginInvoke(showMessageDelegate, new object[] { message });
                }
                else {
                    receiveData.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":" + message + "\n");
                    receiveData.ScrollToCaret();
                }
               
            }
            

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

        private void label2_Click(object sender, EventArgs e)
        {
            //AA 01 04 04 01 00 00 00 55
            //byte[] B = new byte[]{ 0XAA,0X01,0X04, 0X04, 0X01, 0X00, 0X00 , 0X00, 0X55 };
            //nacell.uart.send(B, B.Length);
        }
    }
}
