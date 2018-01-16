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
    public partial class WH_TreatPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid1();
            }
        }

        private void bindGrid1() 
        {
            string sqlCmd = "SELECT OnlineId,OrderNo,ItemNo,ItemParm,remainQty,minProdTime FROM";
            sqlCmd += " (SELECT OnlineId,SUM(BoundQty) AS remainQty,MIN(CreateTime) AS minProdTime FROM PLM_WH_Storage_Actual T1";
            sqlCmd += " LEFT JOIN PLM_Serials_BindBarCode T2 ON T2.BarCode= T1.CntrCode AND T2.OlineStatus='1'";
            sqlCmd += " LEFT JOIN PLM_WH_TreatRecord_B T3 ON T3.RecordId=T2.Id AND T3.TreatType='1'";
            sqlCmd += " WHERE CntrCode LIKE 'B%' AND T3.RecordId IS NULL GROUP BY OnlineId ) A LEFT JOIN PLM_Product_OnLine B";
            sqlCmd += " ON A.OnlineId=B.Id ORDER BY minProdTime";

            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string onlineId = Grid1.DataKeys[e.RowIndex][0].ToString();
            string sqlCmd = "SELECT StorageCode,CntrCode,CreateTime,BoundQty FROM PLM_WH_Storage_Actual T1 ";
            sqlCmd += " LEFT JOIN PLM_Serials_BindBarCode T2 ON T2.BarCode= T1.CntrCode AND T2.OlineStatus='1'";
            sqlCmd += " LEFT JOIN PLM_WH_TreatRecord_B T3 ON T3.RecordId=T2.Id AND T3.TreatType='1'";
            sqlCmd += " WHERE CntrCode LIKE 'B%' AND T3.RecordId IS NULL AND T1.OnlineId='" + onlineId + "' ORDER BY CreateTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        protected void Btn_Pick_Click(object sender, EventArgs e)
        {

        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "LevelUp")
            {
                int RowIndex = e.RowIndex;

            }
        }
    }
}