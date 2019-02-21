using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CellconCore
{
    public partial class Form1 : Form
    {


        KeyboardHook k_hook ;
        Nacelle nacell = new Nacelle();  //吊舱控制接口
        LocControl locobj = null; //定位数据
        GPSControl test_offset = new GPSControl();
        private KeyboardHookLib _keyboardHook = null;
        public Form1()
        {
            InitializeComponent();
            _keyboardHook = new KeyboardHookLib();

            k_hook = new KeyboardHook();

            k_hook.KeyDownEvent += K_hook_KeyDownEvent;
            k_hook.KeyUpEvent += K_hook_KeyUpEvent;
            k_hook.Start();
          //  _keyboardHook.InstallHook(this.OnKeyPress);

            try {
                //初始化dll
              //  string s = Mingw.shell_cmd("proc_ini");

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            byte[] b = {  0xF3, 0xBB };
            richTextBox1.Text=  ((short)(b[0] << 8) + b[1]).ToString()+"--" + Convert.ToInt16("F3BB", 16).ToString();
        }

        private void K_hook_KeyUpEvent(object sender, KeyEventArgs e)
        {
            string keyName =e.KeyCode.ToString();
            this.richTextBox1.Text += "|" + (e.KeyCode == Keys.None ? "" : keyName);
        }

        private void K_hook_KeyDownEvent(object sender, KeyEventArgs e)
        {
            string keyName = e.KeyCode.ToString();
            this.richTextBox1.Text += "|" + (e.KeyCode == Keys.None ? "" : keyName);
        }

        public void OnKeyPress(KeyboardHookLib.HookStruct hookStruct, out bool handle)
        {
            handle = false; //预设不拦截任何键
            Keys key = (Keys)hookStruct.vkCode;

            //  textBox1.Text = key.ToString();
            string keyName = key.ToString();
            this.richTextBox1.Text += "|" + (key == Keys.None ? "" : keyName);
            return;


        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            PopulateSerialportList();
        }


        /// <summary>
        /// 
        /// </summary>
        private void PopulateSerialportList()
        {
            comm_list comm = new comm_list();
            string[] commPort = comm.GetSericalPortName();
            this.comPort.DataSource = commPort;
            if (this.comPort.Items.Count > 0) this.comPort.SelectedIndex = 0;
        }
            
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "连接")
            {

                try
                {

                    nacell.startString(this.comPort.Text, 115200);
                    nacell.uart_rx_event += Nacell_uart_rx_event;
                    button1.Text = "断开";

                }
                catch (Exception ex)
                {
                    button1.Text = "连接";
                    richTextBox1.AppendText(DateTime.Now.ToShortTimeString() + ":" + ex.Message + "\n");
                    richTextBox1.Focus();
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    richTextBox1.ScrollToCaret();
                   
                }
            }
            else {
                button1.Text = "连接";
                nacell.uart_rx_event -= Nacell_uart_rx_event;
                nacell.stop();
            }
        }
      
        //获取串口数据并解析
        private void Nacell_uart_rx_event(byte[] b)
        {

            GCHandle hObject = GCHandle.Alloc(b, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            hObject.Free();
            string s = Mingw.proc_rx(pObject, b.Length);
            if (s == ""|| s.Contains("tickT") == true)
            {

                //暂不处理
                return;

            }
            else
            {
                locobj = Mingw.jser.Deserialize<LocControl>(s);
                locobj.lon += test_offset.lon;
                locobj.lat += test_offset.lat;
                explainLocCmd(locobj);
            }

        }
      
        /// <summary>
        /// 解析吊舱回传数据
        /// </summary>
        /// <param name="loc"></param>
        private void explainLocCmd(LocControl loc)
        {           
            uint err_flag = loc.cgi_err_res & 0xff;

            richTextBox1.AppendText(DateTime.Now.ToShortTimeString() + ":" + "方位：" + loc.vs_z.ToString("0.0") + "° 俯仰：" + loc.vs_y.ToString("0.0") + "°" + "\n");
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();          
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) {

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {

            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string strHex = "AA01000000000000000000000000DF188E7D1900A200000000000000000000000000000000000000FCFF000000000000000000000000000000000000BB480000000000000000000000000000000000000000000022000088";
            byte[] hex = strToToHexByte(strHex);
            Explane(hex);
           
        }
        double Radiu = 180 / Math.PI;

        private void Explane(byte[] b)
        {

         int packIndex = 0;
         //开始解析数据
          int X = (short)(b[packIndex + 15] << 8) + b[packIndex + 14];
          int  Y = (short)(b[packIndex + 17] << 8) + b[packIndex + 16];
          int  Z = (short)(b[packIndex + 19] << 8) + b[packIndex + 18];

            double pitch = Radiu * Math.Atan2(-1 * Z ,Math.Sqrt(X * X + Y * Y));
            double yaw = Radiu * Math.Atan2(Y,X);

            double zoom = ((short)(b[packIndex + 85] << 8) + b[packIndex + 84]) * 0.43;//可见光焦距 4.3-129mm

            if (zoom == 0) zoom = 19.0;//？红外数据焦距，还需云汉确认

            double temp = ((short)(b[packIndex + 83] << 8) + b[packIndex + 82]) / 10;

            richTextBox1.Text = DateTime.Now.ToShortTimeString() + ":" + "方位：" + yaw + "° 俯仰：" + pitch + "°焦距：" + zoom + "mm; 温度：" + temp + "℃"+" X="+X+" Y="+Y+" Z="+Z;

        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
       /// <param name="hexString"></param>
        /// <returns></returns>
        private  byte[] strToToHexByte(string hexString)
        {
             hexString = hexString.Replace(" ", "");
           if ((hexString.Length % 2) != 0)
                 hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
         }

 
    }
}
