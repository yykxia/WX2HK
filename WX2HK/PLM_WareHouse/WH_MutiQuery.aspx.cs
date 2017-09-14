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
    public partial class WH_MutiQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DatePicker1.SelectedDate = DateTime.Now;
                DatePicker2.SelectedDate = DateTime.Now;
            }
        }

        protected void tgb_orderNo_TriggerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tgb_orderNo.Text))
            {
                if (tgb_orderNo.Text.Length >= 6)
                {
                    string sqlCmd = "select T1.Id,scddcp_jhpch,ItemNo,ItemParm,T1.BuildTime,T1.endTime,ISNULL(DistSum,0) as DistSum, ";
                    sqlCmd += " ISNULL(ProductSum,0) as ProductSum,ISNULL(BoundSum,0) as BoundSum,ISNULL(ConfirmSum,0) AS ConfirmSum FROM PLM_Product_Rel A";
                    sqlCmd += " LEFT JOIN PLM_Product_OnLine T1 ON A.ProdId=T1.Id";
                    sqlCmd += " LEFT JOIN (SELECT SUM(BoundQty) AS BoundSum,OnlineId FROM PLM_WH_Storage_Actual GROUP BY OnlineId) T2";
                    sqlCmd += " ON T1.Id=T2.OnlineId LEFT JOIN";
                    sqlCmd += " (SELECT SUM(ActualDistQty) as DistSum,onlineId FROM PLM_WH_Distribution_List GROUP BY onlineId) T3";
                    sqlCmd += " ON T3.onlineId=T1.Id LEFT JOIN";
                    sqlCmd += " (SELECT SUM(BindQty) AS ProductSum,TradeNo FROM PLM_Serials_BindBarCode GROUP BY TradeNo) T4 ON T4.TradeNo=T1.Id";
                    sqlCmd += " LEFT JOIN (SELECT SUM(BoundQty) AS ConfirmSum,OnlineId FROM PLM_WH_Serilas_Bound GROUP BY OnlineId) T5 ON T5.OnlineId=T1.Id";
                    sqlCmd += " WHERE scddcp_jhpch LIKE '%" + tgb_orderNo.Text + "%'";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                }
                else
                {
                    Alert.ShowInTop("查询条件信息过少！");
                }
            }
            else 
            {
                Alert.ShowInTop("订单号不可为空！");
            }
        }

        protected void btn_multQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "select T1.Id,scddcp_jhpch,ItemNo,ItemParm,T1.BuildTime,T1.endTime,ISNULL(DistSum,0) as DistSum, ";
                sqlCmd += " ISNULL(ProductSum,0) as ProductSum,ISNULL(BoundSum,0) as BoundSum,ISNULL(ConfirmSum,0) AS ConfirmSum FROM PLM_Product_Rel A";
                sqlCmd += " LEFT JOIN PLM_Product_OnLine T1 ON A.ProdId=T1.Id";
                sqlCmd += " LEFT JOIN (SELECT SUM(BoundQty) AS BoundSum,OnlineId FROM PLM_WH_Storage_Actual GROUP BY OnlineId) T2";
                sqlCmd += " ON T1.Id=T2.OnlineId LEFT JOIN";
                sqlCmd += " (SELECT SUM(ActualDistQty) as DistSum,onlineId FROM PLM_WH_Distribution_List GROUP BY onlineId) T3";
                sqlCmd += " ON T3.onlineId=T1.Id LEFT JOIN";
                sqlCmd += " (SELECT SUM(BindQty) AS ProductSum,TradeNo FROM PLM_Serials_BindBarCode GROUP BY TradeNo) T4 ON T4.TradeNo=T1.Id";
                sqlCmd += " LEFT JOIN (SELECT SUM(BoundQty) AS ConfirmSum,OnlineId FROM PLM_WH_Serilas_Bound GROUP BY OnlineId) T5 ON T5.OnlineId=T1.Id";
                sqlCmd += " WHERE CONVERT(nvarchar(20),T1.BuildTime,23)>='" + DatePicker1.Text + "' AND CONVERT(nvarchar(20),T1.BuildTime,23)<='" + DatePicker2.Text + "'";
                sqlCmd += " AND T1.OnlineStatus like '" + rdb_onlineStatus.SelectedValue + "'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "details") 
                {
                    string onlineId = Grid1.DataKeys[e.RowIndex][0].ToString();//工单id
                    string sqlCmd = "SELECT (SELECT TOP 1 scddcp_jhpch FROM PLM_Product_Rel WHERE PLM_Product_Rel.ProdId=onLineId) AS orderNo,";
                    sqlCmd += "(CASE WHEN BoundStatus='1' THEN '进库' WHEN BoundStatus='0' THEN '出库' ELSE '其他' END) AS optType,";
                    sqlCmd += "BarCode,StorageCode,BoundQty,boundTime";
                    sqlCmd += " FROM PLM_WH_InBound_Record WHERE onLineId='"+onlineId+"' order by boundTime";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid Grid_boundDetail = Window1.FindControl("Grid_details") as Grid;
                    Grid_boundDetail.DataSource = dt;
                    Grid_boundDetail.DataBind();
                    Window1.Hidden = false;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btn_close_Click(object sender, EventArgs e)
        {
            Window1.Hidden = true;
        }
    }
}