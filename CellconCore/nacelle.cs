using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;


namespace CellconCore
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct CELLPACK
    {
        [FieldOffset(0)]
        public byte aa;
        [FieldOffset(1)]
        public byte addr;
        [FieldOffset(2)]
        public byte fun1;
        [FieldOffset(3)]
        public byte fun2;
        [FieldOffset(4)]
        public int data;
        [FieldOffset(4)]
        public float dataf;
        [FieldOffset(8)]
        public byte end;
        public CELLPACK(byte m)
        {
            aa = 0xaa;
            addr = m;
            fun1 = 0;
            fun2 = 0;
            dataf = 0;
            data = 0;
            end = 0x55;
        }
    }
    /// <summary>
    /// 吊舱控制类
    /// </summary>
    public class Nacelle
    {
        public int rx_uart_tick = 30;                   //接收串口的超时时间,由客户定时减

        public delegate void uart_rx_fun(byte[] b);
        public event uart_rx_fun uart_rx_event;         //串口接收事件


        public uart_dbg uart = new uart_dbg();
        public head组包 pack = new head组包();
        public Thread send_th;
        public EventHandler data_update;
        //待发送指令的队列
        public Queue<CELLPACK> pack_list = new Queue<CELLPACK>(15);
        //public CELLPACK cur_cmd;
        //public bool cur_cmd_avai=false;//指令是否有效
        public byte[] command = new byte[9];
        /// <summary>
        /// 吊舱控制类
        /// </summary>
        public Nacelle()
        {
            pack.pack_len = 10;
            pack.pre_cb = pre_cb;
            pack.pre_offset = 1;
            pack.pro = pro;

          //  uart.DataReceived += new SerialDataReceivedEventHandler(Uart_DataReceived);

        }
        /// <summary>
        /// 搜索串口
        /// </summary>
        /// <param name="com"></param>
        public void find_uart(string[] com)
        {
            int i;
            for (i = 0; i < com.Length; i++)
            {
                try
                {
                    uart.PortName = com[i];
                    uart.ReadTimeout = 500;
                    uart.BaudRate = 19200;
                    uart.Open();

                    CELLPACK t1 = new CELLPACK(1);
                    t1.fun1 = 10;
                    t1.fun2 = 2;
                    t1.addr = 1;
                    t1.data = 0;
                    byte[] buf = Struct_Byte.StructToBytes(t1);
                    uart.send(buf, buf.Length); //发送请求
                    uart.Read(buf, 0, buf.Length);
                    if (buf[0] == 0xaa)
                    {
                        break;
                    }
                    uart.Close();
                }
                catch
                {
                }
            }
            if (i == com.Length)//若没有串口响应
            {
                return;
            }
           // uart.data_rx += uart_DataReceived;
        }
        /// <summary>
        /// 需由调用方捕获异常,并检查Port的合法性
        /// </summary>
        /// <param name="port"></param>
        public void start(int port)
        {
            uart.open(port);
            send_th = new Thread(send_task);
            send_th.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baud"></param>
        public bool startString(string port, int baud)
        {
            bool isSuccess = false;
            try
            {

                uart.BaudRate = baud;
                uart.PortName = port;
                uart.Open();

                uart.DataReceived += new SerialDataReceivedEventHandler(Uart_DataReceived);
                //接收串口数据
                 uart.data_rx += Uart_data_rx;
                
                send_th = new Thread(send_task);
                send_th.Start();
                isSuccess = true;

            }
            catch (Exception ex)
            {
               
                isSuccess = false;
            }

            return isSuccess;

        }
        private void Uart_data_rx(object sender, EventArgs e)
        {
            if (uart_rx_event != null) uart_rx_event((byte[])sender);
        }
        private void Uart_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(100);
            int l = uart.BytesToRead;
            byte[] buf = new byte[l];
            uart.Read(buf, 0, l);
            rx_uart_tick = 30;

            if (uart_rx_event != null) uart_rx_event(buf);

        }

        /// <summary>
        /// 
        /// </summary>
        public void stop()
        {
            if (send_th != null)
                send_th.Abort();
            uart.DataReceived -= Uart_DataReceived;
            uart.data_rx -= Uart_data_rx;
            uart.Close();
        }
        /// <summary>
        /// 发送任务
        /// </summary>
        /// <param name="o"></param>
        void send_task(object o)
        {
            while (true)
            {
                //从发送队列中取得一个指令
                try
                {
                    if (pack_list.Count > 0)
                    //if(cur_cmd_avai)//若指令有效
                    {
                        //用于打印时间戳，Debug使用
                        //string str_timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff\n");
                        //byte[] timestamp = System.Text.Encoding.Default.GetBytes(str_timestamp);
                        //uart.send(timestamp, timestamp.Length); 
                        //--END--
                        byte[] buf = Struct_Byte.StructToBytes(pack_list.Dequeue());
                        //cur_cmd_avai=false;
                        //byte[] buf=Struct_Byte.StructToBytes(cur_cmd);
                        uart.send(buf, buf.Length);
                    }
                }
                catch
                {
                    //return;
                }
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// 接收函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void uart_DataReceived(object sender, EventArgs e)
        {
            byte[] buf = sender as byte[];
            for (int i = 0; i < buf.Length; i++)
            {
                pack.rec_byte(buf[i]);
            }
        }

        int pre_cb(byte[] b, int len)
        {
            return 10;
        }
        bool pro(byte[] b, int len)
        {

            return true;
        }
        void enqueue(CELLPACK t)
        {
            if (pack_list.Count < 3)
            {
                pack_list.Enqueue(t);
            }
            command = Struct_Byte.StructToBytes(t);
            //cur_cmd=t;
            //cur_cmd_avai=true;
        }
        #region 接口指令
        public void cmd_up(float y) //俯仰
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 1;
            t1.fun2 = 2;//俯仰
            t1.addr = 1;
            t1.dataf = y;
            enqueue(t1);
        }
        public void cmd_left(float x)   //航向
        {
            CELLPACK t2 = new CELLPACK(1);
            t2.fun1 = 1;
            t2.fun2 = 3;//航向
            t2.addr = 1;
            t2.dataf = x;
            enqueue(t2);
        }
        public void cmd_zero()  //归零
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 1;
            t1.fun2 = 4;//归零
            t1.addr = 1;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void cmd_zoom(int c) //设置摄像头缩放
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 1;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_save()  //设置摄像头录像
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 2;
            t1.data = 1;
            enqueue(t1);
        }
        public void cmd_change(byte c)  //视频切换
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 4;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_eis(byte c) //电子稳像
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 5;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_light(byte c) //低照度切换
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 6;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_width(byte c) //宽动态切换
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 7;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_gyro()	//陀螺仪校准
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 5;
            t1.fun2 = 2;
            t1.addr = 1;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void cmd_machine(byte c) //电机开关
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 5;
            t1.fun2 = 3;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_fog(byte c) //透雾切换
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.fun1 = 4;
            t1.fun2 = 8;
            t1.data = c;
            enqueue(t1);
        }
        /// <summary>
        /// 垂直电机力矩
        /// </summary>
        /// <param name="c"></param>
        public void cmd_VerForce(byte c)
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 6;
            t1.fun2 = 1;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_HorForce(byte c)
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 6;
            t1.fun2 = 2;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_HorCali()
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 7;
            t1.fun2 = 0;
            t1.data = 0;
            enqueue(t1);
        }
        public void cmd_VerCali()
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 7;
            t1.fun2 = 1;
            t1.data = 0;
            enqueue(t1);
        }
        /// <summary>
        /// 0：演示模式
        /// 1：纯手动
        /// 2：稳定模式
        /// 3：跟踪模式
        /// 4：导航模式
        /// </summary>
        public void cmd_mode(byte c)    //设置模式
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 5;
            t1.fun2 = 1;
            t1.data = c;
            enqueue(t1);
        }
        public void cmd_trace(byte c)   //跟踪功能，现在还不对
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 0x11;
            t1.fun2 = c;
            t1.data = 0;
            enqueue(t1);
        }
        public void cmd_TraceBox(int x, int y)
        {
            CELLPACK t1 = new CELLPACK(1);
            CELLPACK t2 = new CELLPACK(1);
            t1.fun1 = 0x10;
            t1.fun2 = 1;//俯仰
            t1.addr = 1;
            t1.data = (Int16)y;
            t2.fun1 = 0x10;
            t2.fun2 = 0;//航向
            t2.addr = 1;
            t2.data = (Int16)x;
            enqueue(t1);
            enqueue(t2);
        }
        public void cmd_1080()      // 设置1080P - 25帧
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 9;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void cmd_720()      // 设置720P - 60帧
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 10;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void cmd_1080i()     // 设置1080i - 60帧
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 11;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void visible()      // 设置720P - 60帧
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 4;
            t1.data = 0x0;
            enqueue(t1);
        }
        public void infra()      // 设置720P - 60帧
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 4;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void check(int cmd)      // 设置自动校准
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 5;
            t1.fun2 = 4;
            t1.data = (Int16)cmd;
            enqueue(t1);
        }
        public void lift(int cmd)      // 设置升降架
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 6;
            t1.fun2 = 1;
            t1.data = (Int16)cmd;
            enqueue(t1);
        }
        public void svideo()      // 开始录像
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 2;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void evideo()      // 停止录像
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 2;
            t1.data = 0x0;
            enqueue(t1);
        }
        public void capture()      // 拍照
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 3;
            t1.data = 0x0001;
            enqueue(t1);
        }
        public void color(byte c)   // 切换伪彩
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 4;
            t1.fun2 = 12;
            t1.data = c;
            enqueue(t1);
        }

        public void track_pos(float x, float y) //跟踪指定目标,输入-1~1
        {
            CELLPACK t1 = new CELLPACK(1);
            t1.addr = 1;
            t1.fun1 = 5;
            t1.fun2 = 5;
            Int16 xx = (Int16)(x);
            Int16 yy = (Int16)(y);
            UInt16 xxt = (UInt16)xx;
            UInt16 yyt = (UInt16)yy;
            t1.data = (xxt << 16) | yyt; //第一版
                                         //t1.data=(yyt<<16) | xxt;
            enqueue(t1);
        }
        #endregion
    }
}
