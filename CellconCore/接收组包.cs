using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;

namespace CellconCore
{
    /// <summary>
    /// 字符型数据的组包
    /// 此类默认的接收方式为，无起始字符，有终止字符（例如:\r\n）
    /// </summary>
    public class 接收组包
    {
        public int 接收缓冲长度
        {
            get { return rec_buff.Length; }
            set { rec_buff = new byte[value]; }
        }
        protected byte[] rec_buff = new byte[100];
        public int pack_len = 2;//包长
        protected int rec_p = 0;//偏移指示
                                /// <summary>
                                /// 用户处理函数
                                /// </summary>
                                /// <param name="b"></param>
                                /// <param name="len">为兼容C，需输入数组中有效数据长度</param>
                                /// <returns>接收是否正确？不正确则不丢掉接收数据，继续扫描</returns>
        public delegate bool PRO(byte[] b, int len);
        public PRO pro;
        public virtual void rec_byte(byte b)
        {
            rec_buff[rec_p++] = b;
            if (b == 0x0a || b == 0x0d)
            {
                rec_buff[rec_p] = 0;
                //结束，调用处理函数
                pro(rec_buff, rec_p);
                rec_p = 0;
            }
            if (rec_p >= rec_buff.Length - 1)//为了兼容C，数组的最后一个字节要留0
            {
                rec_p = rec_buff.Length - 2;
            }
        }
    }
    /// <summary>
    /// 带包头的组包
    /// </summary>
    public class head组包 : 接收组包
    {
        /// <summary>
        /// 同步字数组
        /// </summary>
        public byte[] SYNC = { 0xaa };
        /// <summary>
        /// 数据包中表示包长的字段所在位置
        /// 或者是数据包中表示数据包类型的字段所在位置
        /// 可作为可变包长数据包改变包长的信号
        /// </summary>
        public byte pre_offset = 0;

        /// <summary>
        /// 用户改变包长的函数,len为当前接收到的长度
        /// </summary>
        public PRE_CB pre_cb = void_pre_cb;
        public delegate int PRE_CB(byte[] b, int len);
        static int void_pre_cb(byte[] b, int len) { return 2; }

        public override void rec_byte(byte b)
        {
            if (rec_p < SYNC.Length)//正在寻找包头
            {
                if (b == SYNC[rec_p])//引导字正确
                {
                    rec_p++;
                }
                else
                {
                    rec_p = 0;
                }
            }
            else if (rec_p == pre_offset)//可以改变包长
            {
                rec_buff[rec_p++] = b;
                pack_len = pre_cb(rec_buff, rec_p);
            }
            else//正常接收数据包
            {
                rec_buff[rec_p++] = b;
                if (rec_p >= pack_len)
                {
                    //调用处理函数
                    if (!pro(rec_buff, pack_len))
                    {//若接收不正确
                        rec_p = 0;
                        int tem_len = pack_len;
                        for (int i = SYNC.Length; i < tem_len; i++)//从接收同步字后开始查找
                        {
                            rec_byte(rec_buff[i]);
                        }
                        return;
                    }
                    rec_p = 0;
                }
            }
        }
    }
    /// <summary>
    /// 协议中没有同步字符，只通过计时时间进行区分
    /// </summary>
    public class 非同步组包 : 接收组包
    {
        Timer timer = new Timer();
        public int TIME_OUT = 200;//单位10ms
        public 非同步组包()
        {
            timer.Elapsed += timer_cb;
            timer.Interval = 10;//以10ms为单位
            timer.Enabled = true;
            timer.Start();
        }
        int tick = 0;
        void timer_cb(object sender, EventArgs e)
        {
            //这个不是真正的回调函数，而是定时器生成回调时间的基本单位
            int t = (int)((TIME_OUT + timer.Interval / 2) / timer.Interval);
            if ((tick++) >= t)
            {
                tick = 0;
                //此时调用回电函数，清空数组
                if (rec_p != 0)
                {
                    pro(rec_buff, rec_p);
                    rec_p = 0;
                }
            }
        }
        public override void rec_byte(byte b)
        {
            rec_buff[rec_p++] = b;
            if (rec_p >= pack_len)
            {
                //调用处理函数
                if (!pro(rec_buff, pack_len))
                {//若接收不正确
                    rec_p = 0;
                    int tem_len = pack_len;
                    for (int i = 1; i < tem_len; i++)//从接收同步字后开始查找
                    {
                        rec_byte(rec_buff[i]);
                    }
                    return;
                }
                rec_p = 0;
            }
            tick = 0;//清空定时器
        }
    }
}
