using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace PLM_OffLine
{
    public partial class f_OffLine_config : Form
    {
        private SerialPort sp = new SerialPort();//实例一个串口

        int FlagConnect = 0;

        public delegate void Displaydelegate(byte[] InputBuf);
        Byte[] OutputBuf = new Byte[128];
        public Displaydelegate disp_delegate; 


        public f_OffLine_config()
        {
            InitializeComponent();
        }

        private void f_OffLineList_Load(object sender, EventArgs e)
        {
            loadSerialList();

            disp_delegate = new Displaydelegate(DispUI);
            sp.DataReceived += new SerialDataReceivedEventHandler(Comm_DataReceived); 

        }

        private void loadSerialList()
        {
            //添加数据项  
            cmb_lineGroup.Items.Add("A");
            cmb_lineGroup.Items.Add("B");

            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目  
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口  
                cbSerial.Items.Add(s);
            }

            //串口设置默认选择项  
            //cbSerial.SelectedIndex = 0; 
        }

        private void btn_checkStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (FlagConnect == 0)
                {
                    FlagConnect = 1;
                    this.Text = this.Text + cbSerial.Text;
                    //设置串口号  
                    string serialName = cbSerial.SelectedItem.ToString();
                    sp.PortName = serialName;
                    //设置波特率
                    sp.BaudRate = 115200;
                    sp.DataBits = 8;//数据位
                    sp.StopBits = StopBits.One;//停止位
                    sp.Parity = Parity.None;//奇偶校验位
                    sp.ReadTimeout = -1;//设置超时

                    sp.Open();

                    //
                    Program.lineGroup = cmb_lineGroup.SelectedItem.ToString();

                    btn_checkStatus.Text = "停止读取";
                }
                else 
                {
                    FlagConnect = 0;
                    this.Text = "自动下线程序";
                    sp.Close();
                    btn_checkStatus.Text = "开始读取";
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message,"打开串口出错！");
            }
        }

        void Comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            Byte[] InputBuf = new Byte[128];

            try
            {
                sp.Read(InputBuf, '\0', sp.BytesToRead);
                //sp.Read(InputBuf, 0, sp.BytesToRead);                                //读取缓冲区的数据直到“}”即0x7D为结束符  
                //InputBuf = UnicodeEncoding.Default.GetBytes(strRD);             //将得到的数据转换成byte的格式  
                System.Threading.Thread.Sleep(50);
                this.Invoke(disp_delegate, InputBuf);

                //中文字符编码
                //sp.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                //从串口读取字符
                //this.DispUI(sp.ReadExisting().ToString());

            }
            catch (TimeoutException ex)         //超时处理  
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //delegate void SetTextCallBack(string ScanCode);

        //public void DispUI(string ScanCode)
        //{
        //    //textBox1.Text = Convert.ToString(InputBuf);  

        //    ASCIIEncoding encoding = new ASCIIEncoding();
        //    //richTextBox1.Text = encoding.GetString(InputBuf);
        //    //验证条码是否有效
        //    if (this.InvokeRequired)
        //    {
        //        SetTextCallBack d = new SetTextCallBack(DispUI);
        //        this.Invoke(d, new object[] { ScanCode });
        //    }
        //    else
        //    {
        //        try
        //        {
        //            //调用web服务
        //            PLMService.PLM_WebService PLMService = new PLMService.PLM_WebService();

        //            //string ScanCode = encoding.GetString(InputBuf);
        //            DateTime optTime = new DateTime();
        //            int exeResult = PLMService.offLine(ref optTime, ScanCode, "3310g");
        //            if (exeResult == 1)
        //            {
        //                //添加到执行成功列表
        //                dataGridView1.Rows.Add(ScanCode, optTime);
        //            }
        //            //else
        //            //{
        //            //    richTextBox1.Text = string.Format("无效的条码：{0}", ScanCode);
                        
        //            //}
        //        }
        //        catch (Exception) 
        //        {
        //            //若服务器不通,则添加至未执行列表，待后续手动提交
        //            dataGridView2.Rows.Add(ScanCode, DateTime.Now);
        //        }
        //    }
        //}

        public void DispUI(byte[] InputBuf)
        {
            //textBox1.Text = Convert.ToString(InputBuf);  

            string ScanCode = System.Text.Encoding.ASCII.GetString(InputBuf).Trim('\0');

            try
            {
                //调用web服务
                PLMService.PLM_WebService PLMService = new PLMService.PLM_WebService();

                //string ScanCode = encoding.GetString(InputBuf);
                DateTime optTime = new DateTime();
                int exeResult = PLMService.offLine(ref optTime, ScanCode, Program.lineGroup);
                if (exeResult == 1)
                {
                    //添加到执行成功列表
                    dataGridView1.Rows.Add(ScanCode, optTime);
                }
                //else
                //{
                //    richTextBox1.Text = string.Format("无效的条码：{0}", ScanCode);

                //}
            }
            catch (Exception)
            {
                //若服务器不通,则添加至未执行列表，待后续手动提交
                dataGridView2.Rows.Add(ScanCode, DateTime.Now);
            }

        }  
        //清空已执行数据
        private void btn_clearSucData_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        } 
 
        //手动执行未读取数据，并清空无效条码信息
        private void subMTData() 
        {
            try
            {
                //调用web服务
                PLMService.PLM_WebService PLMService = new PLMService.PLM_WebService();
                if (PLMService.isCnnected())
                {
                    DateTime optTime = new DateTime();
                    int dgv2Count = dataGridView2.Rows.Count;
                    for (int i = 0; i < dgv2Count; i++)
                    {
                        string barCode = dataGridView2.Rows[i].Cells[0].Value.ToString();
                        int optCount = PLMService.offLine(ref optTime, barCode, Program.lineGroup);
                        if (optCount == 1) //有效数据则添加到已执行列表
                        {
                            dataGridView1.Rows.Add(barCode, optTime);
                            //dataGridView2.Rows.RemoveAt(i);
                        }
                        else
                        {
                            //dataGridView2.Rows.RemoveAt(i);//移除无效条码信息
                        }
                        //System.Threading.Thread.Sleep(2000);
                    }
                    dataGridView2.Rows.Clear();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
        //手动执行
        private void btn_MT_Click(object sender, EventArgs e)
        {
            if (FlagConnect == 0)
            {
                subMTData();
            }
            else 
            {
                MessageBox.Show("请先停止串口读取后再手动执行！");
            }
        }
    }
}
