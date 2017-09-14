using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IETCsoft.sql;
using System.Globalization;

namespace PLM_ProdBoard
{
    public partial class F_board_main : Form
    {
        int nextRowIndex = -1;

        public F_board_main()
        {
            InitializeComponent();
        }

        private void F_board_main_Load(object sender, EventArgs e)
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

            if (!string.IsNullOrEmpty(Program.lineId))
            {
                if (Program.lineId == "0") 
                {
                    Program.lineId = "%";
                }

                bindGrid();

                //string sqlCmd = "select a.OrderNo,a.ItemParm,a.PlanCount,";
                //sqlCmd += "ISNULL(c.onlineSum,0) AS onlineSum,a.RedLineCount,(PlanCount-ISNULL(c.onlineSum,0)) as payCount from PLM_Product_OnLine a ";
                //sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id";
                //sqlCmd += " left join PLM_Product_Line on PLM_Product_Line.id=a.lineId";
                //sqlCmd += " where onlineStatus=1 and LineId like '" + Program.lineId + "'";
                //sqlCmd += " order by BuildTime desc";
                //DataTable dt = new DataTable();
                //SqlSel.GetSqlSel(ref dt, sqlCmd);
                ////DataTable newTab = newTable(dt);
                //dataGridView1.DataSource = dt;
                ////设置列宽
                //this.dataGridView1.Columns[0].FillWeight = 30;
                //this.dataGridView1.Columns[1].FillWeight = 40;
                //this.dataGridView1.Columns[2].FillWeight = 10;
                ////this.dataGridView1.Columns[3].Visible = false;
                //this.dataGridView1.Columns[3].FillWeight = 10;
                //this.dataGridView1.Columns[4].Visible = false;
                //this.dataGridView1.Columns[5].FillWeight = 10;
                ////this.dataGridView1.Columns[6].FillWeight = 10;
                ////标题居中
                //dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                ////this.dataGridView1.DefaultCellStyle.BackColor = Color.Black;
                ////this.dataGridView1.DefaultCellStyle.ForeColor = Color.Red;

                //updateCurRowInfo();

                //getCurentLineClass(Program.lineId);

                //显示当前时间
                toolStripStatusLabel1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }

        private void F_board_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        //private void SetDataGridViewRowXh(DataGridViewRowPostPaintEventArgs e, DataGridView dataGridView)
        //{
        //    SolidBrush solidBrush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor);
        //    int xh = e.RowIndex + 1;
        //    e.Graphics.DrawString(xh.ToString(CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, solidBrush, e.RowBounds.Location.X + 5, e.RowBounds.Location.Y + 4);
        //}

        //private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //    SetDataGridViewRowXh(e, dataGridView1);
        //}

        private void bindGrid() 
        {
            string sqlCmd = "select a.OrderNo,a.ItemParm,a.PlanCount,";
            sqlCmd += "ISNULL(c.onlineSum,0) AS onlineSum,a.RedLineCount,(PlanCount-ISNULL(c.onlineSum,0)) as payCount from PLM_Product_OnLine a ";
            sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id";
            sqlCmd += " left join PLM_Product_Line on PLM_Product_Line.id=a.lineId";
            sqlCmd += " where onlineStatus=1 and LineId like '" + Program.lineId + "'";
            sqlCmd += " order by BuildTime desc";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            //DataTable newTab = newTable(dt);
            dataGridView1.DataSource = dt;
            //设置列宽
            this.dataGridView1.Columns[0].FillWeight = 30;
            this.dataGridView1.Columns[1].FillWeight = 40;
            this.dataGridView1.Columns[2].FillWeight = 10;
            //this.dataGridView1.Columns[3].Visible = false;
            this.dataGridView1.Columns[3].FillWeight = 10;
            this.dataGridView1.Columns[4].Visible = false;
            this.dataGridView1.Columns[5].FillWeight = 10;
            //this.dataGridView1.Columns[6].FillWeight = 10;
            //标题居中
            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.dataGridView1.DefaultCellStyle.BackColor = Color.Black;
            //this.dataGridView1.DefaultCellStyle.ForeColor = Color.Red;

            updateCurRowInfo();

            getCurentLineClass(Program.lineId);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try 
            {
                if (dataGridView1.Rows.Count > 0)
                {

                    if (nextRowIndex == dataGridView1.Rows.Count - 1)
                    {
                        nextRowIndex = 0;
                    }
                    else
                    {
                        nextRowIndex += 1;
                    }
                    dataGridView1.CurrentCell = dataGridView1.Rows[nextRowIndex].Cells[0];
                }
            }
            catch (Exception ex) 
            {
                timer1.Enabled = false;//出现异常停止
                MessageBox.Show(ex.Message);
            }
        }
        //更新当前行上架信息
        private void updateCurRowInfo() 
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int planCount = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                int curCount = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                int redLineNumb = 0;
                if (string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                {
                    redLineNumb = 0;
                }
                else 
                {
                    redLineNumb = Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                }
                //超过生产红线的标记为黄色
                if (curCount + redLineNumb >= planCount)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                bindGrid();
            }
            catch (Exception ex) 
            {
                timer2.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void getCurentLineClass(string lineId) 
        {
            if (lineId == "0")
            {
                toolStripStatusLabel3.Text = "";
            }
            else
            {
                string sqlCmd = "select lineName,CurentClass from PLM_Product_Line where id='" + lineId + "'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    toolStripStatusLabel3.Text = dt.Rows[0]["lineName"].ToString() + " " + dt.Rows[0]["CurentClass"].ToString() + "班";
                }
            }
        }


        private void toolStripStatusLabel1_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private DataTable newTable(DataTable sourceTable) 
        {
            sourceTable.Columns.Add("progImg",typeof(Image));
            int progNum = 0;
            for (int i = 0; i < sourceTable.Rows.Count; i++) 
            {
                double pre = ((double)(sourceTable.Rows[i]["onlineSum"]) * 100) / (int)(sourceTable.Rows[i]["PlanCount"]);
                progNum = Convert.ToInt32(pre);
                DataRow dr = sourceTable.NewRow();
                dr["progImg"] = Program.PressImg(progNum);
                sourceTable.Rows.Add(dr);
            }

            return sourceTable;
        }
    }
}
