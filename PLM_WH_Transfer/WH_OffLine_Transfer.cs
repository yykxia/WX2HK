using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PLM_WH_Transfer
{
    public partial class WH_OffLine_Transfer : Form
    {
        public WH_OffLine_Transfer()
        {
            InitializeComponent();
        }

        private void WH_OffLine_Transfer_Load(object sender, EventArgs e)
        {
            //窗体最大化
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            OffLine_Transfer();

            int formWeight = this.Width;
            dataGridView1.Width = (formWeight / 3) * 2;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            txb_count.Text = curStr + "0";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string curStr = txb_count.Text;
            if (!string.IsNullOrEmpty(curStr))
            {
                txb_count.Text = curStr.Substring(0, curStr.Length - 1);
            }
        }

        //已下线待配送的订单
        private void OffLine_Transfer()
        {
            try
            {
                PLMWebservice.PLM_WebService wbs = new PLMWebservice.PLM_WebService();
                DataTable orderDt = wbs.Transfer_offLineList(Program.lineGroup);
                dataGridView1.DataSource = orderDt;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        //是否属于同一物料编码
        private bool isSameMatrial() 
        {
            string selMatrialNo = "";
            int find = -1;//异常行号
            for (int i = 0; i < dataGridView1.Rows.Count; i++) 
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells["ckbColumn"];
                if (checkCell.Selected)
                {
                    //选中行的物料编码
                    string curMatial = dataGridView1.Rows[i].Cells[6].Value.ToString();
                    if (!string.IsNullOrEmpty(selMatrialNo))
                    {
                        if (selMatrialNo != curMatial)
                        {
                            find = i;//记录物料编码不同的行号
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        //第一行先赋值物料
                        selMatrialNo = curMatial;
                    }
                }
            }

            if (find >= 0)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

    }
}
