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
    public partial class PLM_OffLine_WHMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid(string.Empty, string.Empty);
            }
        }

        private void bindGrid(string orderNo,string itemNo) 
        {
            string sqlCmd = "select (case IsTemp when '1' then '插单' else '' end) as tempDesc,* from PLM_Product_OnLine a left join ";
            sqlCmd += "(select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id ";
            sqlCmd += "where id not in (select OnlineId from PLM_Product_OffLine) and onlineStatus=0";
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(orderNo))
            {
                sqlCmd += " and orderNo like '%" + orderNo + "'%";
            }
            if (!string.IsNullOrEmpty(itemNo))
            {
                sqlCmd += " and itemNo like '%" + itemNo + "'%";
            }
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else 
            {
                Alert.ShowInTop("无数据显示！");
                return;
            }
        }

        protected void tgb_wlbh_TriggerClick(object sender, EventArgs e)
        {
            bindGrid(tgb_wlbh.Text, string.Empty);
        }

        protected void tgb_orderNo_TriggerClick(object sender, EventArgs e)
        {
            bindGrid(string.Empty, tgb_orderNo.Text);
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {

            int[] selections = Grid1.SelectedRowIndexArray;
            if (selections.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (int rowIndex in selections)
                {
                    sb.AppendFormat("{0};", Grid1.DataKeys[rowIndex][0]);
                }
                PageContext.RegisterStartupScript(Window1.GetShowReference("PLM_OffLine_Print.aspx?idList=" + sb + ""));
            }
            else
            {
                Alert.ShowInTop("请先选择相应数据再操作！");
                return;
            }
        }
    }
}