﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;

namespace PLM_TreatScan
{
    public partial class f_main : Form
    {
        List<SerialPort> serialPortList = new List<SerialPort>();
        List<string> scanList = new List<string>();     //用于保存扫描的数据，为了防止重复刷入
        public object syncObj = new object();           //用于异步锁

        //public delegate void Displaydelegate(byte[] InputBuf);
        public delegate void DisplayDelegate(string scanCode);
        //public DisplayDelegate disp_delegate;
        public DisplayDelegate displayDelegate; //变量命名遵守驼峰命名法

        public f_main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 有参构造函数
        /// writer: 许建
        /// create date: 2017/12/28
        /// </summary>
        /// <param name="serialPortList">已经打开端口的SerialPort</param>
        public f_main(List<SerialPort> serialPortList)
        {
            this.serialPortList = serialPortList;
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出登录？", "提示", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 给每个端口创建事件
        /// writer: 许建
        /// create date： 2017/12/28
        /// </summary>
        /// <param name="port">端口实例</param>
        public void ReadDevice(SerialPort port)
        {
            port.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
        }

        /// <summary>
        /// serialPort接收信息的时间，实际上开辟了一个新的时间线程
        /// writer： 许建
        /// create date： 2017/12/28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort; //读取serialPort实例
            System.Threading.Thread.Sleep(100);
            Byte[] InputBuf = new Byte[serialPort.BytesToRead];
            try
            {
               
                serialPort.Read(InputBuf, '\0',serialPort.BytesToRead);
               
                string scanCode = System.Text.Encoding.ASCII.GetString(InputBuf).Trim('\0');
                if (scanCode != "" && !scanList.Contains(scanCode))
                {
                    //如果ScanCode不是空并且不在List里面的话则进行添加，排队进入，线程异步，防止同一个值进入此方法中
                    lock (syncObj)
                    {
                        if (!scanList.Contains(scanCode))
                        {
                            scanList.Add(scanCode);
                            //回调委托事件
                            this.Invoke(displayDelegate, scanCode);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //发生异常，暂时不做任何事情，后续将日志加进程序，在正常操作的情况下在日志中查看
            }
        }

        private void f_main_Load(object sender, EventArgs e)
        {
            //加载临时工加工信息
            BindTempGrid();

            tslbLoginStatus.Text = "当前登录：" + Program.loginClass + " 班 " + Program.loginGroup+" 组";
            this.WindowState = FormWindowState.Maximized;
            //为每个端口分配接收数据的事件
            foreach (SerialPort serialPort in serialPortList)
            {
                ReadDevice(serialPort);
            }
           
            //无边框显示
            //this.FormBorderStyle = FormBorderStyle.None;


            //下面的代码涉及到业务，暂时不改
            this.dataGridView1.AutoGenerateColumns = false;

            this.dataGridView_WOList.AutoGenerateColumns = false;

            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            displayDelegate = new DisplayDelegate(DispUI);
            //串口1
            //Program.sp1.DataReceived += new SerialDataReceivedEventHandler(Comm_DataReceived);
            ////串口2
            //Program.sp2.DataReceived += new SerialDataReceivedEventHandler(Comm_DataReceived_com2);

            //

            DataTable dt = Program.webServ.WH_TreatProgress(1);
            DataTable storageInfo = Program.webServ.WH_AllStorageInfo();

            dt.Columns.Add("Press", typeof(int));
            dt.Columns.Add("ProcessStr", typeof(string));
            dt.Columns.Add("storageLists", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string storageStr = "";
                //拼接订单库位信息
                DataRow[] drArr = storageInfo.Select("scddcp_jhpch='" + dt.Rows[i]["orderNo"].ToString() + "'");
                for (int j = 0; j < drArr.Length; j++)
                {
                    storageStr += drArr[j]["StorageCode"].ToString() + " ";
                }
                dt.Rows[i]["storageLists"] = storageStr;
                double proc = Math.Round(Convert.ToDouble(dt.Rows[i]["process"]) * 100, 0);
                dt.Rows[i]["Press"] = Convert.ToInt32(proc);
                dt.Rows[i]["ProcessStr"] = proc + "%";
            }
            List<ent.OrderInfo> orderList = ConvertToList(dt);
            dataGridView_WOList.DataSource = orderList;
            //设置列宽
            dataGridView_WOList.Columns[0].FillWeight = 28;
            dataGridView_WOList.Columns[1].FillWeight = 20;
            dataGridView_WOList.Columns[2].FillWeight = 10;
            dataGridView_WOList.Columns[3].FillWeight = 42;
        }

        //public void DispUI(byte[] InputBuf)
        /// <summary>
        /// 委托回调函数，显示页面数据
        /// update writer：许建
        /// update date: 2017/12/28
        /// update description: 将传入的参数由字符数组改为字符串
        /// </summary>
        /// <param name="scanCode">扫到的编码</param>
        public void DispUI(string scanCode)
        {
            //textBox1.Text = Convert.ToString(InputBuf);  

            //string ScanCode = System.Text.Encoding.ASCII.GetString(InputBuf).Trim('\0');

            try
            {
                //处理数据
                DataTable orderInfo = new DataTable();
                orderInfo = Program.webServ.WH_TreatAndOffLine(scanCode, Program.loginClass, Program.loginGroup);
                if (orderInfo != null)
                {
                    DataTable curDt = (DataTable)dataGridView1.DataSource;
                    if (curDt != null)
                    {
                        curDt.Merge(orderInfo);
                    }
                    else 
                    {
                        dataGridView1.DataSource = orderInfo;
                    }

                    AutoSizeColumn(dataGridView1);

                    //
                    this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows.Count - 1; 

                    //合计数量
                    DataTable dt1 = (DataTable)dataGridView1.DataSource;
                    //DataTable dt2 = (DataTable)dataGridView2.DataSource;
                    int count = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        int curRowNumb = Convert.ToInt32(dt1.Rows[i]["BindQty"]);
                        count += curRowNumb;
                    }

                    toolStripStatusLabel3.Text = "合计:" + count.ToString();
                }
                else 
                {
                    //MessageBox.Show("无订单信息！");
                }
            }
            catch (Exception)
            {
            }

        }

        //void Comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{

        //    Byte[] InputBuf = new Byte[128];
        //    try
        //    {
        //        Program.sp1.Read(InputBuf, '\0', Program.sp1.BytesToRead);
        //        System.Threading.Thread.Sleep(50);
        //        this.Invoke(disp_delegate, InputBuf);
        //    }
        //    catch (Exception) 
        //    {
                
        //    }
        //}

        //void Comm_DataReceived_com2(object sender, SerialDataReceivedEventArgs e)
        //{

        //    Byte[] InputBuf = new Byte[128];
        //    try
        //    {
        //        Program.sp2.Read(InputBuf, '\0', Program.sp2.BytesToRead);
        //        System.Threading.Thread.Sleep(50);
        //        this.Invoke(disp_delegate, InputBuf);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        private void f_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.scanList.Clear();  //将list清空
            this.DialogResult = DialogResult.OK;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //if (Program.sp1.IsOpen)
            //{
            //    Program.sp1.Close();
            //}
            //else 
            //{
            //    Program.sp1.Open();
            //}
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //if (Program.sp2.IsOpen)
            //{
            //    Program.sp2.Close();
            //}
            //else
            //{
            //    Program.sp2.Open();
            //}
        }

        /// <summary>
        /// 使DataGridView的列自适应宽度
        /// </summary>
        /// <param name="dgViewFiles"></param>
        private void AutoSizeColumn(DataGridView dgViewFiles)
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgViewFiles.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgViewFiles.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgViewFiles.Size.Width)
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            dgViewFiles.Columns[1].Frozen = true;
        }

        /// <summary>
        /// 将DataTable转换为实体列表
        /// </summary>
        /// <param name="dt">待转换的DataTable</param>
        /// <returns></returns>
        public List<ent.OrderInfo> ConvertToList(DataTable dt)
        {
            // 定义集合  
            var list = new List<ent.OrderInfo>();

            if (0 == dt.Rows.Count)
            {
                return list;
            }

            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new ent.OrderInfo();

                // 获得此模型的公共属性  
                var propertys = entity.GetType().GetProperties();

                //遍历该对象的所有属性  
                foreach (var p in propertys)
                {
                    //将属性名称赋值给临时变量
                    string tmpName = p.Name;

                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (dt.Columns.Contains(tmpName))
                    {
                        // 判断此属性是否有Setter
                        if (!p.CanWrite)
                        {
                            continue; //该属性不可写，直接跳出
                        }

                        //取值  
                        var value = dr[tmpName];

                        //如果非空，则赋给对象的属性  
                        if (value != DBNull.Value)
                        {
                            p.SetValue(entity, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中  
                list.Add(entity);
            }

            return list;
        }

        private void BindTempGrid()
        {
            DataTable dt = new DataTable();
            dt = Program.webServ.MES_GetTempEmployeeInfo();//手动分配的临时工剪边
            dataGridView_temp.DataSource = dt;
        }
    }
}
