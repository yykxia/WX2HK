using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using IETCsoft.sql;
using FineUI;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_OrderList_BarCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void tgb_queryStr_TriggerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tgb_queryStr.Text))
            {
                string sqlCmd = "select * from PLM_Product_OnLine where orderNo like '%" + tgb_queryStr.Text + "%'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else 
            {
                Alert.ShowInTop("请输入查询条件！");
            }
        }

        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            string orderId = Grid1.DataKeys[e.RowIndex][0].ToString();
            string sqlCmd = "select t1.Id,t1.ItemName,t1.ItemNo,t1.OrderNo,t1.ItemParm,t1.ItemTech,t2.prodNumb,t1.PlanCount from PLM_Product_OnLine t1";
            sqlCmd += " left join (select sum(bindQty) as prodNumb,TradeNo from PLM_Serials_BindBarCode group by TradeNo) t2";
            sqlCmd += " on t1.id=t2.TradeNo where t1.id='" + orderId + "'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                //加载订单明细
                txb_itemNo.Text = dt.Rows[0]["ItemNo"].ToString();
                txb_itemName.Text = dt.Rows[0]["ItemName"].ToString();
                txb_itemTech.Text = dt.Rows[0]["ItemTech"].ToString();
                txb_workNo.Text = dt.Rows[0]["OrderNo"].ToString();
                numb_planCount.Text = dt.Rows[0]["PlanCount"].ToString();
                numb_prodCount.Text = dt.Rows[0]["prodNumb"].ToString();
                TextArea_parm.Text = dt.Rows[0]["ItemParm"].ToString();
                //生成一维条码图片
                string imgUrl_Code128 = "WH_barCode.aspx?num=P" + orderId;
                Image_product.ImageUrl = imgUrl_Code128;
            }


        }

    }
}