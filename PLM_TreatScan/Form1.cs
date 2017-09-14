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

namespace PLM_TreatScan
{
    public partial class Form1 : Form
    {
        string com1 = "", com2 = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(comboBox1.Text) & !string.IsNullOrEmpty(comboBox1.Text))
                {
                    Program.webServ.Url = ConfigurationManager.AppSettings.Get("webServiceUrl") + "/PLM_WebService.asmx";
                    if (Program.webServ.isCnnected())
                    {
                        //com1 = ConfigurationManager.AppSettings.Get("com1");
                        //com2 = ConfigurationManager.AppSettings.Get("com2");
                        //MessageBox.Show(com1);
                        //打开串口
                        //OpenPort(com1, Program.sp1);
                        //OpenPort(com2, Program.sp2);

                        Program.loginClass = comboBox1.Text;
                        Program.loginGroup = comboBox2.Text;

                        f_main f_main = new f_main();
                        this.Hide();
                        f_main.ShowDialog();
                        if (f_main.DialogResult == DialogResult.OK)
                        {
                            this.Dispose();//在子窗体中触发关闭
                        }
                    }
                }
                else 
                {
                    MessageBox.Show("登录信息不可为空！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
    }
}
