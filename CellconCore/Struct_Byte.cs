using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CellconCore
{
    public class JoyState
    {
        // 六个摇杆方向的控制标志位
        public bool left;
        public bool right;
        public bool up;
        public bool down;
        public bool zoomin;
        public bool zoomout;

        public JoyState()
        {
            left = false;
            right = false;
            up = false;
            down = false;
            zoomin = false;
            zoomout = false;
        }
        // 对六个摇杆方向进行设置
        public void setLeft()
        {
            this.left = true;
            this.right = false;
        }
        public void releaseLeft()
        {
            this.left = false;
            this.right = false;
        }
        public void setRight()
        {
            this.left = false;
            this.right = true;
        }
        public void releaseRight()
        {
            this.left = false;
            this.right = false;
        }
        public void setUp()
        {
            this.up = true;
            this.down = false;
        }
        public void releaseUp()
        {
            this.up = false;
            this.down = false;
        }
        public void setDown()
        {
            this.up = false;
            this.down = true;
        }
        public void releaseDown()
        {
            this.up = false;
            this.down = false;
        }
        public void setZoomIn()
        {
            this.zoomin = true;
            this.zoomout = false;
        }
        public void releaseZoomIn()
        {
            this.zoomin = false;
            this.zoomout = false;
        }
        public void setZoomOut()
        {
            this.zoomin = false;
            this.zoomout = true;
        }
        public void releaseZoomOut()
        {
            this.zoomin = false;
            this.zoomout = false;
        }
        public void releaseAll()
        {
            this.left = false;
            this.right = false;
            this.up = false;
            this.down = false;
            this.zoomin = false;
            this.zoomout = false;
        }
        public int getState()
        {
            int count = Convert.ToInt32(left || right) + Convert.ToInt32(up || down) + Convert.ToInt32(zoomin || zoomout);
            return count;
        }
    }

    public struct JoyControl
    {
        public float[] axe { get; set; }
        //        public int[,] hat { get; set; }
        public int[] bt { get; set; }
    }
    //以下为何丹写的，关于C# byte[]与struct的转换
    public static class Struct_Byte
    {
        //struct转换为byte[]
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        //byte[]转换为struct
        public static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            catch
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        //为方便结构体数组的构造
        public static object BytesToStruct(byte[] bytes, int offset, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, offset, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            catch
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
