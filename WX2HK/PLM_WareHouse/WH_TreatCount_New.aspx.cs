using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_TreatCount_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DatePicker1.SelectedDate = DateTime.Now.AddDays(-1);
                DatePicker2.SelectedDate = DateTime.Now;
                TimePicker1.Text = "00:00";
                TimePicker2.Text = "00:00";
            }
        }

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddl_group.SelectedValue) & !string.IsNullOrEmpty(ddl_group.SelectedValue))
                {
                    //获取起始和截至时间字符串
                    string dt1 = string.Format("{0} {1}", DatePicker1.Text, TimePicker1.Text);
                    string dt2 = string.Format("{0} {1}", DatePicker2.Text, TimePicker2.Text);
                    //转换成时间格式
                    DateTime sDt = Convert.ToDateTime(dt1);
                    DateTime eDt = Convert.ToDateTime(dt2);
                    //统计剪边数据
                    string sqlCmd = "SELECT SUM(BindQty) AS BindQty,SUM(SQty) AS SQty,(SUM(BindQty)-SUM(SQty)) AS BQty,";
                    sqlCmd += " ItemNo,ItemParm,OrderNo,TradeNo FROM (";
                    sqlCmd += " select BindQty,ItemNo,ItemParm,OrderNo,TradeNo,(CASE WHEN BarCode LIKE 'B%' THEN 0 ELSE BindQty END) AS SQty";
                    sqlCmd += " from PLM_WH_TreatRecord_B t1 left join ";
                    sqlCmd += " PLM_Serials_BindBarCode t2 on t1.RecordId=t2.id left join PLM_Product_OnLine t3 on t3.Id=t2.TradeNo";
                    sqlCmd += " where t1.TreatTime>'" + sDt + "' and t1.TreatTime<'" + eDt + "' and TreatStatus='0'";
                    sqlCmd += " and t1.TreatType='1' and TreatClass='" + ddl_class.SelectedValue + "' and";
                    sqlCmd += " TreatGroup='" + ddl_group.SelectedValue + "') T group by TradeNo,ItemNo,ItemParm,OrderNo";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();

                    getSummaryData(dt);
                }
                else 
                {
                    Alert.ShowInTop("班次或班组信息不可为空！");
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        private void getSummaryData(DataTable curDt)
        {
            int bindTotal = 0;
            int bindTotal_S = 0;
            int bindTotal_B = 0;
            if (curDt.Rows.Count > 0)
            {
                foreach (DataRow row in curDt.Rows)
                {
                    bindTotal += Convert.ToInt32(row["BindQty"]);
                    bindTotal_S += Convert.ToInt32(row["SQty"]);
                    bindTotal_B += Convert.ToInt32(row["BQty"]);
                }

                JObject summary = new JObject();
                //summary.Add("ItemParm", "全部合计");
                summary.Add("BindQty", bindTotal);
                summary.Add("SQty", bindTotal_S);
                summary.Add("BQty", bindTotal_B);

                Grid1.SummaryData = summary;
            }
        }
    }
}