using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_offLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                //加载正在下线的订单列表
                OrderList("A");
            }
        }

        private void OrderList(string lineGroup)
        {
            PLM_WebService plmSer = new PLM_WebService();
            DataTable dt = new DataTable();
            dt = plmSer.Transfer_offLineList(lineGroup);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }


        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            string orderId = Grid1.DataKeys[e.RowIndex][0].ToString();
            string sqlCmd = "select t1.Id,t1.ItemName,t1.ItemNo,t1.OrderNo,t1.ItemParm,t1.ItemTech,t2.prodNumb,t1.PlanCount,";
            sqlCmd += " ISNULL(transferCount,0) AS transferCount,t4.SCDDCP_JHTCSL,t4.SCDDCP_JHPCH from PLM_Product_OnLine t1";
            sqlCmd += " left join (select sum(bindQty) as prodNumb,TradeNo from PLM_Serials_BindBarCode group by TradeNo) t2 on t1.id=t2.TradeNo";
            sqlCmd += " LEFT JOIN (SELECT ProdId,MIN(OrderId) AS OrderId FROM PLM_Product_Rel GROUP BY ProdId) T3 ON T3.ProdId=t1.Id";
            sqlCmd += " LEFT JOIN (select orderId,SUM(transferCount) AS transferCount from PLM_WH_Transfer_OrderBind group by orderId) C ON C.orderId=t1.Id";
            sqlCmd += " LEFT JOIN View_ERP_SCDDCP T4 ON T4.SCDDCP_LSBH=T3.OrderId where t1.id='" + orderId + "'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                //加载订单明细
                txb_itemNo.Text = dt.Rows[0]["ItemNo"].ToString();
                txb_itemName.Text = dt.Rows[0]["ItemName"].ToString();
                txb_itemTech.Text = dt.Rows[0]["ItemTech"].ToString();
                txb_workNo.Text = dt.Rows[0]["OrderNo"].ToString();
                numb_planCount.Text = dt.Rows[0]["SCDDCP_JHTCSL"].ToString();
                numb_prodCount.Text = dt.Rows[0]["prodNumb"].ToString();
                numb_TransferCount.Text = dt.Rows[0]["transferCount"].ToString();
                TextArea_parm.Text = dt.Rows[0]["ItemParm"].ToString();
                //生成一维条码图片
                string imgUrl_Code128 = "WH_barCode.aspx?num=P" + orderId;
                Image_product.ImageUrl = imgUrl_Code128;
            }


        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //Label1.Text = "上次刷新时间：" + DateTime.Now;
                string curOnlineId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
                //加载正在下线的订单列表
                PLM_WebService plmSer = new PLM_WebService();
                DataTable dt = new DataTable();
                dt = plmSer.Transfer_offLineList("A");
                //重新定位该订单
                int rowIndex = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (curOnlineId == dt.Rows[i]["TradeNo"].ToString())
                    {
                        rowIndex = i;
                        break;
                    }
                }
                Grid1.DataSource = dt;
                Grid1.DataBind();
                Grid1.SelectedRowIndex = rowIndex;
            }
            catch (Exception) 
            {
                Timer1.Enabled = false;
            }
        }

    }
}