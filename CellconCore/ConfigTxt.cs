using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace CellconCore
{
    public class ConfigTxt
    {
        public ConfigTxt()
        {
            方位 = 0;
            俯仰 = 1;
            变焦加 = 0;
            变焦减 = 1;
            模式选择 = 2;
            切换视频源 = 3;
            红外模式 = 4;
            伪彩模式 = 5;
            轴0偏差 = 0.0;
            轴1偏差 = 0.0;
            轴2偏差 = 0.0;
            轴3偏差 = 0.0;
            方位死区 = 0.0;
            俯仰死区 = 0.0;
            波特率 = 115200;
            按键方位轴精度 = 0.0f;
            按键俯仰轴精度 = 0.0f;
            摇杆方位轴精度 = 0.0f;
            摇杆俯仰轴精度 = 0.0f;
        }

        public static ConfigTxt load(string s)
        {
            StreamReader sr = new StreamReader(s);
            string sbuf = sr.ReadToEnd();
            var t = json_ser.Deserialize<ConfigTxt>(sbuf);
            sr.Close();
            return t;
        }
        public static JavaScriptSerializer json_ser = new JavaScriptSerializer();
        public int 方位 { get; set; }
        public int 俯仰 { get; set; }
        public int 变焦加 { get; set; }
        public int 变焦减 { get; set; }
        public int 模式选择 { get; set; }
        public int 切换视频源 { get; set; }
        public int 红外模式 { get; set; }
        public int 伪彩模式 { get; set; }
        public double 轴0偏差 { get; set; }
        public double 轴1偏差 { get; set; }
        public double 轴2偏差 { get; set; }
        public double 轴3偏差 { get; set; }
        public double 方位死区 { get; set; }
        public double 俯仰死区 { get; set; }
        public int 波特率 { get; set; }
        public float 按键方位轴精度 { get; set; }
        public float 按键俯仰轴精度 { get; set; }
        public float 摇杆方位轴精度 { get; set; }
        public float 摇杆俯仰轴精度 { get; set; }
    }
}
