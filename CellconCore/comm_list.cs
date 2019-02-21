/*
 * 该文件用于从注册表中动态获取当前串口设备的端口号
 * comm_list comm = new comm_list();
 * string[] commList = comm.GetSericalPortName();
 * 获取的串口端口格式为：COM1
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO.Ports;

namespace CellconCore
{
   public class comm_list
    {
        public string[] GetSericalPortName()
        {

            string[] values = SerialPort.GetPortNames();
            return values;
        }
    }
}
