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
    public partial class WH_StorageList : System.Web.UI.Page
    {
        private static string storageId = "";
        private static string curBarCode = "";
        private static string curOrderNo = "";
        private static string curOrderCount = "";
        private static string storageCode = "";
        private static string curOnlineId = "";
        private static string curDistLocked = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindStorageList();
            }
        }

        private void bindStorageList() 
        {
            string sqlCmd = "select * from PLM_WH_Storage WHERE UseStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        protected void Grid2_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            storageCode = Grid2.DataKeys[e.RowIndex][0].ToString();
            string sqlCmd = "select A.ID AS StorageId,* from PLM_WH_Storage_Actual A left join PLM_Product_OnLine B ";
            sqlCmd+="on A.OnlineId=B.id where StorageCode='" + storageCode + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                string specType = rdbt_specType.SelectedValue;
                string others = txa_others.Text;
                //是否有配送信息
                if (curDistLocked == "1") 
                {
                    Alert.ShowInTop("当前信息被配送锁定，不可操作！");
                    return;
                }
                //是否正在剪边
                string sqlCmd = "select * from PLM_Serials_BindBarCode t1 left join PLM_WH_TreatRecord_B t2";
                sqlCmd += " on t1.id=t2.RecordId where t1.BarCode='" + curBarCode + "' and t1.OlineStatus='1' and TradeNo='" + curOnlineId + "'";
                DataTable treatDt = new DataTable();
                if (SqlSel.GetSqlSel(ref treatDt, sqlCmd)) 
                {
                    if (treatDt.Rows[0]["TreatStatus"].ToString() == "1") 
                    {
                        Alert.ShowInTop("尚有加工未完成，不可直接操作出库！");
                        return;
                    }
                }               
                //插入操作日志
                sqlCmd = "insert into PLM_WH_SpecOptRecord (CntrCode,OnlineId,BoundCount,OptTime,OptType,others)";
                sqlCmd += " values ('" + curBarCode + "','" + curOnlineId + "','" + curOrderCount + "','" + DateTime.Now + "','" + specType + "','" + others + "')";
                if (SqlSel.ExeSql(sqlCmd) > 0)
                {
                    PLM_WebService plm = new PLM_WebService();
                    plm.Storage_BoundState(curBarCode, storageCode, curBarCode, "0", curOnlineId, curOrderCount);//执行出库
                    Window1.Hidden = true;
                    //更新库位信息
                    sqlCmd = "select A.ID AS StorageId,* from PLM_WH_Storage_Actual A left join PLM_Product_OnLine B ";
                    sqlCmd += "on A.OnlineId=B.id where StorageCode='" + storageCode + "'";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                    Alert.Show("操作成功！");
                }
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
                int rowIndex = -1;
                if (e.CommandName == "remove")
                {
                    rowIndex = e.RowIndex;
                    storageId = Grid1.DataKeys[rowIndex][0].ToString();
                    curOnlineId = Grid1.DataKeys[rowIndex][1].ToString();
                    curDistLocked = Grid1.DataKeys[rowIndex][2].ToString();
                    curBarCode = Grid1.Rows[rowIndex].Values[2].ToString();
                    curOrderNo = Grid1.Rows[rowIndex].Values[0].ToString();
                    curOrderCount = Grid1.Rows[rowIndex].Values[1].ToString();
                    if (storageId != "")
                    {
                        Label label = SimpleForm1.FindControl("label_info") as Label;
                        label.Text = "条码:" + curBarCode + "/库位:" + storageCode
                            + "/订单号:" + curOrderNo + "/数量:" + curOrderCount;
                        Window1.Hidden = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }
    }
}