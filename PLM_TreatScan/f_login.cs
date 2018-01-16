using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO.Ports;
using System.IO;
using Microsoft.Win32;

namespace PLM_TreatScan
{
    /// <summary>
    /// 登录页面，选择班次，班组进行登录
    /// </summary>
    public partial class f_login : Form
    {
        //string com1 = "", com2 = "";

        //创建serialList来存储有效的端口
        List<SerialPort> serialPortList = new List<SerialPort>();
        public f_login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从注册表中获得端口，并打开端口
        /// writer：许建
        /// create date: 2017/12/28
        /// </summary>
        private void getAndOpenPort()
        {
            try
            {
                //1、从注册表中获取port
                RegistryKey root = Registry.LocalMachine;
                RegistryKey myKey = root.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM", true);
                string[] portArray = null;
                portArray = myKey.GetValueNames();
                if (portArray.Count() == 0)
                {
                    //如果没有搜到端口，则提示用户没有收到相关端口号
                    MessageBox.Show("没有搜到任何端口号\n请联系有关部门进行处理");
                    this.Close();
                }
                //2、打开搜索到的所有端口
                foreach (string port in portArray)
                {
                    //打开端口，并将端口放到list中保存
                    openPort((string)myKey.GetValue(port));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this.Close();
            }
         
           
        }

        /// <summary>
        /// 打开端口，并将端口放到list中保存
        /// writer: 许建
        /// create date: 2017/12/28
        /// </summary>
        /// <param name="port">端口号</param>
        private void openPort(string port)
        {           
            try
            {
                //实例化端口
                SerialPort sp = new SerialPort();
                sp.PortName = port;
                //设置波特率
                sp.BaudRate = 115200;
                sp.DataBits = 8;//数据位
                sp.StopBits = StopBits.One;//停止位
                sp.Parity = Parity.None;//奇偶校验位
                sp.ReadTimeout = -1;//设置超时
                sp.Open();
                //打开后添加到list中保存
                serialPortList.Add(sp);
            }
            catch(Exception e)
            {
                MessageBox.Show("打开端口:" + port + "失败\n请联系IT人员检查相关端口\n" + e.Message);
                this.Close();
            }
        }

        /// <summary>
        /// 关闭端口
        /// writer: 许建
        /// create date: 2017/12/28
        /// </summary>
        public void closePort()
        {
            if (serialPortList.Count > 0)
            {
                foreach (SerialPort sp in serialPortList)
                {
                    //如果端口是打开状态就进行关闭
                    if (sp.IsOpen)
                    {
                        sp.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 点击登录按钮的时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try 
            {
                //判断班次，班组有没有选择，如果一个都没有选择的话，则不可以登录
                if (!string.IsNullOrEmpty(cmbBC.Text) && !string.IsNullOrEmpty(cmbBZ.Text))
                {
                    Program.webServ.Url = ConfigurationManager.AppSettings.Get("webServiceUrl") + "/PLM_WebService.asmx";
                    if (Program.webServ.isCnnected())
                    {
                        Program.loginClass = cmbBC.Text;
                        Program.loginGroup = cmbBZ.Text;
                        f_main f_main = new f_main(serialPortList);
                        this.Hide();
                        f_main.ShowDialog();
                        //子窗体登录后触发，关闭所有子程序
                        if (f_main.DialogResult == DialogResult.OK)
                        {
                            this.Close();   //在子窗体中触发关闭    
                        }
                    }
                }
                else
                {
                    MessageBox.Show("登录信息不可为空！");
                }
            //    if (!string.IsNullOrEmpty(comboBox1.Text) & !string.IsNullOrEmpty(comboBox1.Text))
            //    {
            //        Program.webServ.Url = ConfigurationManager.AppSettings.Get("webServiceUrl") + "/PLM_WebService.asmx";
            //        if (Program.webServ.isCnnected())
            //        {
            //            //com1 = ConfigurationManager.AppSettings.Get("com1");
            //            //com2 = ConfigurationManager.AppSettings.Get("com2");
            //            //MessageBox.Show(com1);
            //            //打开串口
            //            //OpenPort(com1, Program.sp1);
            //            //OpenPort(com2, Program.sp2);

            //            Program.loginClass = comboBox1.Text;
            //            Program.loginGroup = comboBox2.Text;

            //            f_main f_main = new f_main();
            //            this.Hide();
            //            f_main.ShowDialog();
            //            if (f_main.DialogResult == DialogResult.OK)
            //            {
            //                this.Dispose();//在子窗体中触发关闭
            //            }
            //        }
            //    }
            //    else 
            //    {
            //        MessageBox.Show("登录信息不可为空！");
            //    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 窗口load的时候搜索并打开所有端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void f_login_Load(object sender, EventArgs e)
        {
            //搜索并打开端口
            getAndOpenPort();
            
           // Program.webServ.Url = ConfigurationManager.AppSettings.Get("webServiceUrl") + "/PLM_WebService.asmx";
            //if (!string.IsNullOrEmpty(com1)) 
            //{
            //    comboBox1.SelectedText = com1;
            //}
            //if (!string.IsNullOrEmpty(com2))
            //{
            //    comboBox2.SelectedText = com2;
            //}
        }

        private void OpenPort(string serialName, SerialPort sp)
        {
            sp.PortName = serialName;
            //设置波特率
            sp.BaudRate = 115200;
            sp.DataBits = 8;//数据位
            sp.StopBits = StopBits.One;//停止位
            sp.Parity = Parity.None;//奇偶校验位
            sp.ReadTimeout = -1;//设置超时

            sp.Open();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TreatScan_port port = new TreatScan_port();
            port.ShowDialog();
        }

        private void f_login_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.closePort();
            this.Dispose();
        }

    }
}
