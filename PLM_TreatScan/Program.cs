using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace PLM_TreatScan
{
    static class Program
    {
        public static SerialPort sp1 = new SerialPort();//实例化串口1

        public static SerialPort sp2 = new SerialPort();//实例化串口2

        public static PLMWebService.PLM_WebService webServ = new PLMWebService.PLM_WebService();//web服务

        public static string loginClass = "";//登录班次
        public static string loginGroup = "";//登录班组
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
