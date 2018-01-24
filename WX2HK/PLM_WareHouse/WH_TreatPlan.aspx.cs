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
    public partial class WH_TreatPlan : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid1();

                GetEmployeeInfo();
            }
        }

        protected void GetEmployeeInfo()
        {
            DataTable data = GetEmployee_Temp();
            DDL_Emple.DataTextField = "name";
            DDL_Emple.DataValueField = "code";
            DDL_Emple.DataSource = data;
            DDL_Emple.DataBind();
        }

        private void bindGrid1() 
        {
            string processId = "1";//1为剪边
            string sqlCmd = "SELECT OnlineId,OrderNo,ItemNo,ItemParm,remainQty,minProdTime,";
            sqlCmd += "(case when c.OrderId is null then 0 else 1 end) as levelUp FROM";
            sqlCmd += " (SELECT OnlineId,SUM(BoundQty) AS remainQty,MIN(CreateTime) AS minProdTime FROM PLM_WH_Storage_Actual T1";
            sqlCmd += " LEFT JOIN PLM_Serials_BindBarCode T2 ON T2.BarCode= T1.CntrCode AND T2.OlineStatus='1'";
            sqlCmd += " LEFT JOIN PLM_WH_TreatRecord_B T3 ON T3.RecordId=T2.Id AND T3.TreatType='1'";
            sqlCmd += " WHERE CntrCode LIKE 'B%' AND (T3.TreatStatus=1 or T3.TreatStatus is null) GROUP BY OnlineId ) A";
            sqlCmd += " LEFT JOIN PLM_Product_OnLine B ON A.OnlineId=B.Id";
            sqlCmd += " left join PLM_ProcessLevel C on c.OrderId=A.OnlineId and ProcessId='" + processId + "'";
            sqlCmd += " ORDER BY minProdTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string onlineId = Grid1.DataKeys[e.RowIndex][0].ToString();
            BindGrid_Waiting(onlineId);
            BindGrid_Going(onlineId);
        }

        private void BindGrid_Waiting(string onlineId)
        {
            string sqlCmd = "SELECT StorageCode,CntrCode,CreateTime,BoundQty,T2.Id";
            sqlCmd += " FROM PLM_WH_Storage_Actual T1 ";
            sqlCmd += " LEFT JOIN PLM_Serials_BindBarCode T2 ON T2.BarCode= T1.CntrCode AND T2.OlineStatus='1'";
            sqlCmd += " LEFT JOIN PLM_WH_TreatRecord_B T3 ON T3.RecordId=T2.Id AND T3.TreatType='1'";
            sqlCmd += " WHERE CntrCode LIKE 'B%' AND T3.TreatStatus is null";
            sqlCmd += " AND T1.OnlineId='" + onlineId + "' ORDER BY CreateTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid_wating.DataSource = dt;
            Grid_wating.DataBind();
        }

        private void BindGrid_Going(string onlineId)
        {
            string sqlCmd = "SELECT StorageCode,CntrCode,CreateTime,BoundQty,T3.Id,'完工' as Done";
            sqlCmd += ",(case when T3.Picked='1' then '取消' else '' end)as Remove,T3.WorkerNo";
            sqlCmd += " FROM PLM_WH_Storage_Actual T1 ";
            sqlCmd += " LEFT JOIN PLM_Serials_BindBarCode T2 ON T2.BarCode= T1.CntrCode AND T2.OlineStatus='1'";
            sqlCmd += " LEFT JOIN PLM_WH_TreatRecord_B T3 ON T3.RecordId=T2.Id AND T3.TreatType='1'";
            sqlCmd += " WHERE CntrCode LIKE 'B%' AND T3.TreatStatus=1";
            sqlCmd += " AND T1.OnlineId='" + onlineId + "' ORDER BY CreateTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid_doing.DataSource = dt;
            Grid_doing.DataBind();
        }

        protected void Btn_Pick_Click(object sender, EventArgs e)
        {
            try
            {
                string TreatGroup = "A";//临时产线
                string TreatClass = "T";//临时工班次
                int TreateType = 1;
                string sqlCmd = string.Empty;
                string WorkerNo = DDL_Emple.SelectedValue;
                int[] selectedIndex = Grid_wating.SelectedRowIndexArray;
                string SerialsId = string.Empty;
                foreach(int rowIndex in selectedIndex)
                {
                    SerialsId = Grid_wating.DataKeys[rowIndex][0].ToString();
                    sqlCmd = "insert into PLM_WH_TreatRecord_B(RecordId,TreatTime,TreatClass,TreatGroup," +
                        "TreatType,TreatStatus,workerNo,Picked) values('" + SerialsId + "',getdate(),'" + TreatClass + "'" +
                        ",'" + TreatGroup + "','" + TreateType + "','1','" + WorkerNo + "','1')";
                    SqlSel.ExeSql(sqlCmd);
                }

                string OnlineId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
                BindGrid_Waiting(OnlineId);
                BindGrid_Going(OnlineId);

                Alert.Show("已分配！");
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        //{
        //    if (e.CommandName == "LevelUp")
        //    {
        //        string sqlCmd = string.Empty;
        //        int RowIndex = e.RowIndex;
        //        CheckBoxField checkField = (CheckBoxField)Grid1.FindColumn(e.ColumnIndex);
        //        bool checkState = checkField.GetCheckedState(RowIndex);
        //    }
        //}

        protected void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = string.Empty;
                CheckBoxField checkField = (CheckBoxField)Grid1.FindColumn("LevelUp");
                int selectedCount = Grid1.SelectedRowIndexArray.Length;
                if (selectedCount > 0)
                {
                    for (int i = 0; i < selectedCount; i++)
                    {
                        int RowIndex = Grid1.SelectedRowIndexArray[i];
                        bool checkState = checkField.GetCheckedState(RowIndex);
                        if (checkState)
                        {
                            sqlCmd = "insert into PLM_ProcessLevel (orderId,processId) values " +
                                "('" + Grid1.DataKeys[RowIndex][0].ToString() + "','1')";
                        }
                        else
                        {
                            sqlCmd = "delete from PLM_ProcessLevel where orderId='" +
                                "" + Grid1.DataKeys[RowIndex][0].ToString() + "' and processId='1'";
                        }

                        SqlSel.ExeSql(sqlCmd);
                    }

                    bindGrid1();
                    Alert.ShowInTop("设定成功！");
                }
                else
                {
                    Alert.ShowInTop("无可提交的数据！");
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid_doing_RowCommand(object sender, GridCommandEventArgs e)
        {
            string sqlCmd = string.Empty;
            string TreateId = Grid_doing.DataKeys[e.RowIndex][0].ToString();
            if (e.CommandName == "Remove")
            {
                sqlCmd = "delete from PLM_WH_TreatRecord_B where id='" + TreateId + "'";
                SqlSel.ExeSql(sqlCmd);
                //
                string OnlineId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
                BindGrid_Waiting(OnlineId);
                BindGrid_Going(OnlineId);
            }

            if(e.CommandName == "Done")
            {
                sqlCmd = "update PLM_WH_TreatRecord_B set TreatStatus='" + (int)ProcessStatus.DONE
                    + "' where id='" + TreateId + "'";
                SqlSel.ExeSql(sqlCmd);

                //int Grid1Index = Grid1.SelectedRowIndex;
                //bindGrid1();
                string OnlineId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
                BindGrid_Waiting(OnlineId);
                BindGrid_Going(OnlineId);
            }
        }
    }
}