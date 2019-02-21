using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace CellconCore
{
    public class UserConfig
    {
        public UserConfig()
        {
            方位 = 0;
            俯仰 = 1;
            变焦 = 2;
            跟踪 = 0;
            切换视频源 = 1;
            录像 = 2;
            变焦p按键 = 3;
            变焦m按键 = 4;
            轴0偏差 = 0.0;
            轴1偏差 = 0.0;
            轴2偏差 = 0.0;
            轴3偏差 = 0.0;
            方位死区 = 0.0;
            俯仰死区 = 0.0;
            变焦死区 = 0.0;
            波特率 = 19200;
            按键方位轴精度 = 0.0f;
            按键俯仰轴精度 = 0.0f;
            摇杆方位轴精度 = 0.0f;
            摇杆俯仰轴精度 = 0.0f;
            变焦精度 = 4;
            回中 = 0;
            电子稳像 = 1;
            夜视切换 = 2;
            方位微调左 = 3;
            方位微调右 = 4;
            宽动态开关 = 5;
            俯仰微调上 = 6;
            俯仰微调下 = 7;
            透雾开关 = 8;
            稳定模式 = 9;
            跟踪模式 = 10;
            跟随模式 = 11;
            可见光 = 12;
            热成像 = 13;
            开始录像 = 14;
            停止录像 = 15;
            拍照 = 16;
        }

        public static UserConfig load(string s)
        {
            StreamReader sr = new StreamReader(s);
            string sbuf = sr.ReadToEnd();
            var t = json_ser.Deserialize<UserConfig>(sbuf);
            sr.Close();
            return t;
        }

        public static JavaScriptSerializer json_ser = new JavaScriptSerializer();

        public int 方位 { get; set; }
        public int 俯仰 { get; set; }
        public int 变焦 { get; set; }
        public int 回中 { get; set; }
        public int 电子稳像 { get; set; }
        public int 夜视切换 { get; set; }
        public int 方位微调左 { get; set; }
        public int 方位微调右 { get; set; }
        public int 宽动态开关 { get; set; }
        public int 俯仰微调上 { get; set; }
        public int 俯仰微调下 { get; set; }
        public int 透雾开关 { get; set; }
        public int 稳定模式 { get; set; }
        public int 跟踪模式 { get; set; }
        public int 跟随模式 { get; set; }
        public int 跟踪 { get; set; }
        public int 切换视频源 { get; set; }
        public int 录像 { get; set; }
        public int 变焦p按键 { get; set; }
        public int 变焦m按键 { get; set; }
        public double 轴0偏差 { get; set; }
        public double 轴1偏差 { get; set; }
        public double 轴2偏差 { get; set; }
        public double 轴3偏差 { get; set; }
        public double 方位死区 { get; set; }
        public double 俯仰死区 { get; set; }
        public double 变焦死区 { get; set; }
        public int 波特率 { get; set; }
        public float 按键方位轴精度 { get; set; }
        public float 按键俯仰轴精度 { get; set; }
        public float 摇杆方位轴精度 { get; set; }
        public float 摇杆俯仰轴精度 { get; set; }
        public int 变焦精度 { get; set; }
        public int 可见光 { get; set; }
        public int 热成像 { get; set; }
        public int 开始录像 { get; set; }
        public int 停止录像 { get; set; }
        public int 拍照 { get; set; }
    }
}
