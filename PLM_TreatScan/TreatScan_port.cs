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
    public partial class TreatScan_port : Form
    {
        public TreatScan_port()
        {
            InitializeComponent();
        }

        private void TreatScan_port_Load(object sender, EventArgs e)
        {
            //读取相关配置信息
            string webUrl = ConfigurationManager.AppSettings.Get("webServiceUrl");
            string com1 = ConfigurationManager.AppSettings.Get("com1");
            string com2 = ConfigurationManager.AppSettings.Get("com2");

            //加载串口列表
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }
            else
            {
                //添加串口项目  
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    //COM1
                    comboBox1.Items.Add(s);

                    //COM2 
                    comboBox2.Items.Add(s);
                }
            }

            comboBox1.SelectedText = com1;

            comboBox2.SelectedText = com2;

            textBox1.Text = webUrl;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["com1"].Value = comboBox1.Text;

                config.AppSettings.Settings["com2"].Value = comboBox2.Text;

                //尝试连接Web服务器
                Program.webServ.Url = textBox1.Text + "/PLM_WebService.asmx";
                if (Program.webServ.isCnnected())
                {
                    config.AppSettings.Settings["webServiceUrl"].Value = textBox1.Text;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");  

                this.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //是否与com2重叠
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                if (comboBox1.Text == comboBox2.Text)
                {
                    MessageBox.Show("不可使用同一串口！");
                    return;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //是否与com1重叠
            if (!string.IsNullOrEmpty(comboBox2.Text))
            {
                if (comboBox1.Text == comboBox2.Text)
                {
                    MessageBox.Show("不可使用同一串口！");
                    return;
                }
            }
        }

    }
}
