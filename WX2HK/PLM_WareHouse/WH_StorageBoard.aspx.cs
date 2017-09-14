using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;
using System.Drawing;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_StorageBoard : System.Web.UI.Page
    {
        private string StorageFloor = "3F";//仓库楼层
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                loadStoragePosition_WH();

                loadStoragePosition_WQ();

                loadStoragePosition_EQ();

                loadStoragePosition_EH();

                anncInfo("2");
            }
        }

        //WH
        private void loadStoragePosition_WH() 
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'WH%'";
            DataTable dt_WH = new DataTable();
            SqlSel.GetSqlSel(ref dt_WH, sqlCmd);
            for (int i = 0; i < dt_WH.Rows.Count; i++)
            {
                TableRow tr_WH = new TableRow();
                TableCell tc_WH = new TableCell();
                TableCell tc_Pos = new TableCell();
                tc_Pos.HorizontalAlign = HorizontalAlign.Center;
                tc_WH.HorizontalAlign = HorizontalAlign.Center;
                tc_WH.Text = dt_WH.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_WH.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tr_WH.BackColor = Color.SpringGreen;
                    tc_Pos.Text = AvailablePos.ToString();
                    //tc_WH.BackColor = Color.SpringGreen;
                }
                else 
                {
                    tr_WH.BackColor = Color.Red;
                }
                tr_WH.Controls.Add(tc_WH);
                tr_WH.Controls.Add(tc_Pos);
                Table_WH.Controls.Add(tr_WH);
            }
        }

        //WQ
        private void loadStoragePosition_WQ()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'WQ%'";
            DataTable dt_WQ = new DataTable();
            SqlSel.GetSqlSel(ref dt_WQ, sqlCmd);
            for (int i = 0; i < dt_WQ.Rows.Count; i++)
            {
                TableRow tr_WQ = new TableRow();
                TableCell tc_WQ = new TableCell();
                TableCell tc_Pos = new TableCell();
                tc_Pos.HorizontalAlign = HorizontalAlign.Center;
                tc_WQ.HorizontalAlign = HorizontalAlign.Center;
                tc_WQ.Text = dt_WQ.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_WQ.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tr_WQ.BackColor = Color.SpringGreen;
                    tc_Pos.Text = AvailablePos.ToString();
                    //tc_WQ.BackColor = Color.Green;
                }
                else
                {
                    tr_WQ.BackColor = Color.Red;
                }
                tr_WQ.Controls.Add(tc_WQ);
                tr_WQ.Controls.Add(tc_Pos);
                Table_WQ.Controls.Add(tr_WQ);
            }
        }

        //EQ
        private void loadStoragePosition_EQ()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'EQ%'";
            DataTable dt_EQ = new DataTable();
            SqlSel.GetSqlSel(ref dt_EQ, sqlCmd);
            for (int i = 0; i < dt_EQ.Rows.Count; i++)
            {
                TableRow tr_EQ = new TableRow();
                TableCell tc_EQ = new TableCell();
                TableCell tc_Pos = new TableCell();
                tc_Pos.HorizontalAlign = HorizontalAlign.Center;
                tc_EQ.HorizontalAlign = HorizontalAlign.Center;
                tc_EQ.Text = dt_EQ.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_EQ.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tr_EQ.BackColor = Color.SpringGreen;
                    tc_Pos.Text = AvailablePos.ToString();
                    //tc_WQ.BackColor = Color.Green;
                }
                else
                {
                    tr_EQ.BackColor = Color.Red;
                }
                tr_EQ.Controls.Add(tc_EQ);
                tr_EQ.Controls.Add(tc_Pos);
                Table_EQ.Controls.Add(tr_EQ);
            }
        }

        //EQ
        private void loadStoragePosition_EH()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'EH%'";
            DataTable dt_EH = new DataTable();
            SqlSel.GetSqlSel(ref dt_EH, sqlCmd);
            for (int i = 0; i < dt_EH.Rows.Count; i++)
            {
                TableRow tr_EH = new TableRow();
                TableCell tc_EH = new TableCell();
                TableCell tc_Pos = new TableCell();
                tc_Pos.HorizontalAlign = HorizontalAlign.Center;
                tc_EH.HorizontalAlign = HorizontalAlign.Center;
                tc_EH.Text = dt_EH.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_EH.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tr_EH.BackColor = Color.SpringGreen;
                    tc_Pos.Text = AvailablePos.ToString();
                    //tc_WQ.BackColor = Color.Green;
                }
                else
                {
                    tr_EH.BackColor = Color.Red;
                }
                tr_EH.Controls.Add(tc_EH);
                tr_EH.Controls.Add(tc_Pos);
                Table_EH.Controls.Add(tr_EH);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                loadStoragePosition_WH();

                loadStoragePosition_WQ();

                loadStoragePosition_EQ();

                loadStoragePosition_EH();

                anncInfo("2");
            }
            catch (Exception) 
            {
                Timer1.Enabled = false;
            }
        }

        private void anncInfo(string deptId) 
        {
            string sqlCmd = "select * from PLM_Announcement where AnncDept='" + deptId + "' and isNew='1'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                string anncInfo = dt.Rows[0]["AnncContent"].ToString();//公告内容
                label_anuncInfo.Text = anncInfo;
            }
            else 
            {
                label_anuncInfo.Text = "暂无信息";
            }
        }
    }
}