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
    public partial class WH_StorageBoard_2nd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                loadStoragePosition_A();

                loadStoragePosition_B();

                loadStoragePosition_C();

                loadStoragePosition_D();

                loadStoragePosition_L();

                loadStoragePosition_P();

                anncInfo("2");
            }
        }

        //A
        private void loadStoragePosition_A()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'A%'";
            DataTable dt_A = new DataTable();
            SqlSel.GetSqlSel(ref dt_A, sqlCmd);
            for (int i = 0; i < dt_A.Rows.Count; i++)
            {
                TableRow tr_A = new TableRow();
                TableCell tc_A = new TableCell();
                tc_A.HorizontalAlign = HorizontalAlign.Center;
                tc_A.Text = dt_A.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_A.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_A.BackColor = Color.SpringGreen;
                }
                else
                {
                    tc_A.BackColor = Color.Red;
                }
                tr_A.Controls.Add(tc_A);
                Table_A.Controls.Add(tr_A);
            }
        }

        //B
        private void loadStoragePosition_B()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'B%'";
            DataTable dt_B = new DataTable();
            SqlSel.GetSqlSel(ref dt_B, sqlCmd);
            for (int i = 0; i < dt_B.Rows.Count; i++)
            {
                TableRow tr_B = new TableRow();
                TableCell tc_B = new TableCell();
                tc_B.HorizontalAlign = HorizontalAlign.Center;
                tc_B.Text = dt_B.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_B.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_B.BackColor = Color.SpringGreen;
                }
                else
                {
                    tc_B.BackColor = Color.Red;
                }
                tr_B.Controls.Add(tc_B);
                Table_B.Controls.Add(tr_B);
            }
        }

        //C
        private void loadStoragePosition_C()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'C%'";
            DataTable dt_C = new DataTable();
            SqlSel.GetSqlSel(ref dt_C, sqlCmd);
            for (int i = 0; i < dt_C.Rows.Count; i++)
            {
                TableRow tr_C = new TableRow();
                TableCell tc_C = new TableCell();
                tc_C.HorizontalAlign = HorizontalAlign.Center;
                tc_C.Text = dt_C.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_C.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_C.BackColor = Color.SpringGreen;
                    //tc_C.BackColor = Color.Green;
                }
                else
                {
                    tc_C.BackColor = Color.Red;
                }
                tr_C.Controls.Add(tc_C);
                Table_C.Controls.Add(tr_C);
            }
        }

        //D
        private void loadStoragePosition_D()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'D%'";
            DataTable dt_D = new DataTable();
            SqlSel.GetSqlSel(ref dt_D, sqlCmd);
            for (int i = 0; i < dt_D.Rows.Count; i++)
            {
                TableRow tr_D = new TableRow();
                TableCell tc_D = new TableCell();
                tc_D.HorizontalAlign = HorizontalAlign.Center;
                tc_D.Text = dt_D.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_D.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_D.BackColor = Color.SpringGreen;
                    //tc_D.BackColor = Color.Green;
                }
                else
                {
                    tc_D.BackColor = Color.Red;
                }
                tr_D.Controls.Add(tc_D);
                Table_D.Controls.Add(tr_D);
            }
        }

        //L
        private void loadStoragePosition_L()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'L%'";
            DataTable dt_L = new DataTable();
            SqlSel.GetSqlSel(ref dt_L, sqlCmd);
            for (int i = 0; i < dt_L.Rows.Count; i++)
            {
                TableRow tr_L = new TableRow();
                TableCell tc_L = new TableCell();
                tc_L.HorizontalAlign = HorizontalAlign.Center;
                tc_L.Text = dt_L.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_L.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_L.BackColor = Color.SpringGreen;
                    //tc_L.BackColor = Color.Green;
                }
                else
                {
                    tc_L.BackColor = Color.Red;
                }
                tr_L.Controls.Add(tc_L);
                Table_L.Controls.Add(tr_L);
            }
        }

        //P
        private void loadStoragePosition_P()
        {
            string sqlCmd = "select * from PLM_WH_Storage where UseStatus='1' and StorageCode like 'P%'";
            DataTable dt_P = new DataTable();
            SqlSel.GetSqlSel(ref dt_P, sqlCmd);
            for (int i = 0; i < dt_P.Rows.Count; i++)
            {
                TableRow tr_P = new TableRow();
                TableCell tc_P = new TableCell();
                tc_P.HorizontalAlign = HorizontalAlign.Center;
                tc_P.Text = dt_P.Rows[i]["StorageCode"].ToString();
                int AvailablePos = Convert.ToInt32(dt_P.Rows[i]["AvailablePos"]);
                if (AvailablePos > 0)
                {
                    tc_P.BackColor = Color.SpringGreen;
                    //tc_P.BackColor = Color.Green;
                }
                else
                {
                    tc_P.BackColor = Color.Red;
                }
                tr_P.Controls.Add(tc_P);
                Table_P.Controls.Add(tr_P);
            }
        }


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                loadStoragePosition_A();

                loadStoragePosition_B();

                loadStoragePosition_C();

                loadStoragePosition_D();

                loadStoragePosition_L();

                loadStoragePosition_P();

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