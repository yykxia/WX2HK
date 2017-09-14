using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Text;

namespace WX2HK.PLM
{
    public partial class PLM_Release_Edit : System.Web.UI.Page
    {
        private static string getId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                getId = HttpContext.Current.Request.QueryString["OnlineId"];
                bindGrid("", "");
            }
        }

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            string date1 = DatePicker1.Text + " " + TimePicker1.Text;
            string date2 = DatePicker2.Text + " " + TimePicker2.Text;
            bindGrid(date1, date2);
        }

        protected void bindGrid(string sTime,string eTime)
        {
            string sortField = Grid1.SortField;
            string sortDirection = Grid1.SortDirection;
            string sqlCmd = "select a.id,a.barCode,a.bindQty,a.excUser,a.createTime,b.lineName from PLM_Serials_BindBarCode a left join PLM_Product_Line b on a.lineId=b.id ";
            sqlCmd += "where tradeno='" + getId + "' and a.OlineStatus='1'";
            if (string.IsNullOrEmpty(sTime.Trim()))
            {
                if (!string.IsNullOrEmpty(eTime.Trim()))
                {
                    sqlCmd += " and createTime<='" + eTime + "'";
                }
                else 
                {

                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(eTime.Trim()))
                {
                    sqlCmd += " and createTime>='" + sTime + "' and createTime<='" + eTime + "'";
                }
                else 
                {
                    sqlCmd += " and createTime>='" + sTime + "'";
                }
            }
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            DataView view1 = dt.DefaultView;
            view1.Sort = String.Format("{0} {1}", sortField, sortDirection);

            Grid1.DataSource = view1;
            Grid1.DataBind();
        }

        protected void btn_release_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid1.Rows.Count; i++) 
            {
                int bindId = Convert.ToInt32(Grid1.DataKeys[i][0]);
                string sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where id=" + bindId;
                SqlSel.ExeSql(sqlCmd);
            }
            Alert.Show("当前上架数据清空成功！");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            string date1 = DatePicker1.Text + " " + TimePicker1.Text;
            string date2 = DatePicker2.Text + " " + TimePicker2.Text;
            bindGrid(date1, date2);

        }

        //private void OutputSummaryData()
        //{
        //    DataTable source = DataSourceUtil.GetDataTable2();

        //    float donateTotal = 0.0f;
        //    float feeTotal = 0.0f;
        //    foreach (DataRow row in source.Rows)
        //    {
        //        donateTotal += Convert.ToInt32(row["Donate"]);
        //        feeTotal += Convert.ToInt32(row["Fee"]);
        //    }


        //    JObject summary = new JObject();
        //    //summary.Add("major", "全部合计");
        //    summary.Add("fee", feeTotal.ToString("F2"));
        //    summary.Add("donate", donateTotal.ToString("F2"));


        //    Grid1.SummaryData = summary;

        //}
    }
}