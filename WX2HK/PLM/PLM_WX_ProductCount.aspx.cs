using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Data;
using IETCsoft.sql;
using Newtonsoft.Json.Linq;

namespace WX2HK.PLM
{
    public partial class PLM_WX_ProductCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid("day");
                getSummaryData();
            }
        }

        private void bindGrid(string queryType) 
        {
            DataTable tempDt = new DataTable();
            string strTimeC="", endTimeC = "";
            string strTimeD="", endTimeD = "";
            string sqlCmd = "";
            if (queryType == "day") 
            {
                strTimeD = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " " + "12:00";
                endTimeD = DateTime.Now.ToString("yyyy-MM-dd") + " " + "12:00";
                strTimeC = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                endTimeC = DateTime.Now.ToString("yyyy-MM-dd");
                sqlCmd = "select dd.id,dd.lineName,cc.sumC,dd.sumD,(cc.sumC+dd.sumD) as bindSum from ";
                sqlCmd += "(select a.id,a.lineName,isnull(D.bindSumD,0) as sumD from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumD,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeD + "' and createtime<'" + endTimeD + "' and excUser='D' group by lineId)D ";
                sqlCmd += "on a.id=D.lineId) dd left join ";
                sqlCmd += "(select a.id,isnull(C.bindSumC,0) as sumC from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumC,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeC + "' and createtime<'" + endTimeC + "' and excUser='C' group by lineId)C ";
                sqlCmd += "on a.id=C.lineId) cc on cc.id=dd.id";
            }
            if (queryType == "week") 
            {
                DateTime dt = DateTime.Now;  //当前时间
                int dayOfWeek = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
                DateTime weekStart = dt.AddDays(1 - ((dayOfWeek == 0) ? 7 : dayOfWeek));   //本周周一
                DateTime weekEnd = weekStart.AddDays(6);  //本周周日
                DateTime lastWeekStart = weekStart.AddDays(-7);  //上周周一
                DateTime lastWeekEnd = weekEnd.AddDays(-7); //上周周日
                strTimeD = lastWeekStart.AddDays(-1).ToString("yyyy-MM-dd") + " " + "12:00";
                endTimeD = lastWeekEnd.ToString("yyyy-MM-dd") + " " + "12:00";
                strTimeC = lastWeekStart.ToString("yyyy-MM-dd");
                endTimeC = lastWeekEnd.AddDays(1).ToString("yyyy-MM-dd");
                sqlCmd = "select dd.id,dd.lineName,cc.sumC,dd.sumD,(cc.sumC+dd.sumD) as bindSum from ";
                sqlCmd += "(select a.id,a.lineName,isnull(D.bindSumD,0) as sumD from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumD,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeD + "' and createtime<'" + endTimeD + "' and excUser='D' group by lineId)D ";
                sqlCmd += "on a.id=D.lineId) dd left join ";
                sqlCmd += "(select a.id,isnull(C.bindSumC,0) as sumC from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumC,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeC + "' and createtime<'" + endTimeC + "' and excUser='C' group by lineId)C ";
                sqlCmd += "on a.id=C.lineId) cc on cc.id=dd.id";
            } 
            if (queryType == "month") 
            {
                DateTime lastMonth = DateTime.Now.AddMonths(-1);
                DateTime LastMonth_firstDay = lastMonth.AddDays(1 - lastMonth.Day);
                DateTime LastMonth_lastDay = lastMonth.AddDays(1 - lastMonth.Day).AddMonths(1).AddDays(-1);
                strTimeD = LastMonth_firstDay.AddDays(-1).ToString("yyyy-MM-dd") + " " + "12:00";
                endTimeD = LastMonth_lastDay.ToString("yyyy-MM-dd") + " " + "12:00";
                strTimeC = LastMonth_firstDay.ToString("yyyy-MM-dd");
                endTimeC = LastMonth_lastDay.AddDays(1).ToString("yyyy-MM-dd");
                sqlCmd = "select dd.id,dd.lineName,cc.sumC,dd.sumD,(cc.sumC+dd.sumD) as bindSum from ";
                sqlCmd += "(select a.id,a.lineName,isnull(D.bindSumD,0) as sumD from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumD,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeD + "' and createtime<'" + endTimeD + "' and excUser='D' group by lineId)D ";
                sqlCmd += "on a.id=D.lineId) dd left join ";
                sqlCmd += "(select a.id,isnull(C.bindSumC,0) as sumC from PLM_Product_Line a ";
                sqlCmd += "left join (select sum(bindQty) as bindSumC,lineId from PLM_Serials_BindBarCode where createTime>'" + strTimeC + "' and createtime<'" + endTimeC + "' and excUser='C' group by lineId)C ";
                sqlCmd += "on a.id=C.lineId) cc on cc.id=dd.id";

            }
            SqlSel.GetSqlSel(ref tempDt, sqlCmd);
            Grid1.DataSource = tempDt;
            Grid1.DataBind();
        }

        protected void btn_day_Click(object sender, EventArgs e)
        {
            bindGrid("day");
        }

        protected void rdbtn1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectValue = rdbtn1.SelectedValue;
            bindGrid(selectValue);
            getSummaryData();
        }

        private void getSummaryData() 
        {
            DataTable curDt = getDataSource();
            int bindTotal = 0, CTotal = 0, DTotal = 0;
            if (curDt.Rows.Count > 0) 
            {
                foreach (DataRow row in curDt.Rows)
                {
                    bindTotal += Convert.ToInt32(row["bindSum"]);
                    CTotal += Convert.ToInt32(row["sumC"]);
                    DTotal += Convert.ToInt32(row["sumD"]);
                }

                JObject summary = new JObject();
                //summary.Add("major", "全部合计");
                summary.Add("bindSum", bindTotal);
                summary.Add("sumC", CTotal);
                summary.Add("sumD", DTotal);

                Grid1.SummaryData = summary;
            }
        }

        private DataTable getDataSource() 
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("lineName");
                dt.Columns.Add("sumC");
                dt.Columns.Add("sumD");
                dt.Columns.Add("bindSum");
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    GridRow grow = Grid1.Rows[i];
                    dr["id"] = grow.DataKeys[0].ToString();
                    dr["lineName"] = grow.Values[0].ToString();
                    dr["bindSum"] = grow.Values[1].ToString();
                    dr["sumC"] = grow.Values[2].ToString();
                    dr["sumD"] = grow.Values[3].ToString();
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return null;
            }
        }

    }
}