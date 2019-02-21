using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace CellconCore
{
    public class uart_dbg : SerialPort
    {
        // 需要一个定时器用来，用来保证即使缓冲区没满也能够及时将数据处理掉，防止一直没有到达
        // 阈值，而导致数据在缓冲区中一直得不到合适的处理。
        private DispatcherTimer checkTimer = new DispatcherTimer();

        public uart_dbg()
        {
            DataReceived += new SerialDataReceivedEventHandler(uart_dbg_DataReceived);
            //InitCheckTimer();
        }
        public event EventHandler data_update;//数据刷新
        public event EventHandler data_rx;    //接收回调函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void open(int c)
        {
            PortName = "COM" + c;
            Open();
        }

        // 数据接收缓冲区
        private List<byte> receiveBuffer = new List<byte>();
        // 一个阈值，当接收的字节数大于这么多字节数之后，就将当前的buffer内容交由数据处理的线程
        // 分析。这里存在一个问题，假如最后一次传输之后，缓冲区并没有达到阈值字节数，那么可能就
        // 没法启动数据处理的线程将最后一次传输的数据处理了。这里应当设定某种策略来保证数据能够
        // 在尽可能短的时间内得到处理。
        private const int THRESH_VALUE = 88;

        bool shouldClear = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void uart_dbg_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = sender as System.IO.Ports.SerialPort;

            if (sp != null) {

                // 临时缓冲区将保存串口缓冲区的所有数据
                int bytesToRead = sp.BytesToRead;
                byte[] tempBuffer = new byte[bytesToRead];
                // 将缓冲区所有字节读取出来
                sp.Read(tempBuffer, 0, bytesToRead);
                // 检查是否需要清空全局缓冲区先
                if (shouldClear)
                {
                    receiveBuffer.Clear();
                    shouldClear = false;
                }

                // 暂存缓冲区字节到全局缓冲区中等待处理
                receiveBuffer.AddRange(tempBuffer);

                if (receiveBuffer.Count >= THRESH_VALUE)
                {                   
                    // 进行数据处理，采用新的线程进行处理。
                    Thread dataHandler = new Thread(new ParameterizedThreadStart(ReceivedDataHandler));
                    dataHandler.Priority = ThreadPriority.Normal;
                    dataHandler.IsBackground = true;
                    dataHandler.Start(receiveBuffer);
                }
              
                // 启动定时器，防止因为一直没有到达缓冲区字节阈值，而导致接收到的数据一直留存在缓冲区中无法处理。
                // StartCheckTimer();

            }
            
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="obj"></param>
        private void ReceivedDataHandler(object obj)
        {
            Thread.Sleep(10);

            List<byte> recvBuffer = new List<byte>();
            recvBuffer.AddRange((List<byte>)obj);

            if (recvBuffer.Count == 0)
            {
                return;
            }

            // 必须应当保证全局缓冲区的数据能够被完整地备份出来，这样才能进行进一步的处理。
            shouldClear = true;
            // 处理数据，比如解析指令等88需等待
            if (recvBuffer != null && recvBuffer.Count >= 88 && data_rx != null) {

                data_rx(recvBuffer.ToArray(), null);
               
            } 
        }

        public void send(byte[] b, int n)
        {
            Write(b, 0, n);
            StringBuilder sb = new StringBuilder(n * 5 + 10);
            sb.Append("\r\n");
            for (int j = 0; j < n; j++)
            {
                sb.Append(string.Format("{0:X2} ", b[j]));
            }
            sb.Append("\r\n");
            if(data_update!=null) data_update(sb.ToString(), null);
        }

        #region 定时器
        /// <summary>
        /// 超时时间为50ms
        /// </summary>
        private const int TIMEOUT = 50;
        /// <summary>
        /// 
        /// </summary>
        private void InitCheckTimer()
        {
            // 如果缓冲区中有数据，并且定时时间达到前依然没有得到处理，将会自动触发处理函数。
            checkTimer.Interval = new TimeSpan(0, 0, 0, 0, TIMEOUT);
            checkTimer.IsEnabled = false;
            checkTimer.Tick += CheckTimer_Tick;
        }
        /// <summary>
        /// 开启时间检查
        /// </summary>
        private void StartCheckTimer()
        {
            checkTimer.IsEnabled = true;
            checkTimer.Start();
        }
        /// <summary>
        /// 停止时间检查
        /// </summary>
        private void StopCheckTimer()
        {
            checkTimer.IsEnabled = false;
            checkTimer.Stop();
        }

        int maxTHRESH_VALUE = 1024;

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            // 触发了就把定时器关掉，防止重复触发。
            StopCheckTimer();

            // 只有到达阈值的情况下才会强制其启动新的线程处理缓冲区数据。
            if (receiveBuffer.Count > maxTHRESH_VALUE)
            {               
                // 进行数据处理，采用新的线程进行处理。
                Thread dataHandler = new Thread(new ParameterizedThreadStart(ReceivedDataHandler));
                dataHandler.Start(receiveBuffer);
            }
        }
        #endregion
    }
}
