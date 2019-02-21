using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellconCore
{
    class Map
    {
    }
    // GPS Struct
    public struct GPSControl
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    // Location Struct
    public class LocControl
    {
        public double absHeight { get; set; }           // 飞机绝对海拔
        public double libHeight { get; set; }           //目标的高度
        public uint cgi_err_res { get; set; }           // 定位状态 0=正常定位|1=俯仰角过小|2=无高程数据 【提示】
        public GPSControl cgi_plane_set { get; set; }   // 飞机GPS坐标 【Map】
        public GPSControl cgi_target_set { get; set; }  // 目标GPS坐标 【Map】
        public double lat { get; set; }                 // 飞机GPS坐标-lat 【显示】
        public double lon { get; set; }                 // 飞机GPS坐标-lon 【显示】
        public double targ_lat { get; set; }            // 目标GPS坐标-lat 【显示】
        public double targ_lon { get; set; }            // 目标GPS坐标-lon 【显示】
        public double vs_y { get; set; }                // 吊舱视轴方位角
        public double vs_z { get; set; }                // 吊舱视轴俯仰角

        public float[] eu_x { get; set; }
        public float[] eu_y { get; set; }
        public float[] eu_z { get; set; }
        public LocControl()
        {
        }
    }
}
