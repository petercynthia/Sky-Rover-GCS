using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Script.Serialization;

namespace CellconCore
{
    public struct S_IMG
    {
        public int w;
        public int h;
        public IntPtr pbuf;
    }
    public struct S_Vector3D
    {
        public double x;
        public double y;
        public double z;
    }
    static public class Mingw
    {
        //[DllImport(@"D:/cur/proj/d06/output/pflow.so",CallingConvention = CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        //public static extern string so_cmd(string s); //发送shell指令，接收回复字符

        //[DllImport(@"targetp.so",CallingConvention = CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        //public static extern string so_cmd(string s); //发送shell指令，接收回复字符

        [DllImport(@"targetp.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string shell_cmd(string s); //发送shell指令，接收回复字符
        [DllImport(@"targetp.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string proc_rx(IntPtr p, int a); //处理接收数据,返回是否有数据包接收
        [DllImport(@"targetp.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern S_IMG display(); //C#主动要图像

        [DllImport(@"targetp.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern S_Vector3D get_eular(S_Vector3D plane, S_Vector3D targ); //根据目标和飞机位置求绝对欧拉角

        public static JavaScriptSerializer jser = new JavaScriptSerializer();
    }
}
