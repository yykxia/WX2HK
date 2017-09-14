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
    public partial class WH_Distribution_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void trgb_sjdh_TriggerClick(object sender, EventArgs e)
        {
            try
            {
                string sjdh = trgb_sjdh.Text;
                if (!string.IsNullOrEmpty(sjdh))
                {
                    if (sjdh.Length < 6)
                    {
                        Alert.ShowInTop("查询单号位数至少六位！");
                    }
                    else
                    {
                        string sqlCmd = "select SourceNo,SUM(distCount) AS distCount from PLM_WH_Distribution_Temp A ";
                        sqlCmd += " left join PLM_WH_Storage_Actual B ON A.strorageId=B.Id WHERE A.scatSign='0' AND A.isTemp='0'";
                        sqlCmd += " AND OnlineId IN (SELECT ProdId FROM PLM_Product_Rel WHERE scddcp_jhpch LIKE '%" + sjdh + "%') GROUP BY SourceNo";
                        DataTable dt = new DataTable();
                        SqlSel.GetSqlSel(ref dt, sqlCmd);
                        Grid1.DataSource = dt;
                        Grid1.DataBind();
                    }

                }
                else
                {
                    Alert.ShowInTop("查询单号不可为空！");
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            bindGrid2(Grid1.Rows[e.RowIndex].Values[0].ToString());
        }

        private void bindGrid2(string sourceNo) 
        {
            string sqlCmd = "select * from PLM_WH_Distribution_Temp A left join PLM_WH_Storage_Actual B ON A.strorageId=B.Id";
            sqlCmd += " where SourceNo='" + sourceNo + "' and scatSign='0' AND isTemp='0'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void btn_remove_Click(object sender, EventArgs e)
        {
            int[] selections = Grid2.SelectedRowIndexArray;
            foreach (int rowIndex in selections) 
            {
                string tempId = Grid2.DataKeys[rowIndex][0].ToString();
                string storageId = Grid2.DataKeys[rowIndex][1].ToString();
                //相关货架解除绑定
                string sqlCmd = "update PLM_WH_Storage_Actual set isLocked='0' where id='" + storageId + "'";
                if (SqlSel.ExeSql(sqlCmd) == 1) 
                {
                    sqlCmd = "delete from PLM_WH_Distribution_Temp where id='" + tempId + "'";
                    SqlSel.ExeSql(sqlCmd);
                }
            }

            bindGrid2(Grid1.Rows[Grid1.SelectedRowIndex].Values[0].ToString());

            Alert.ShowInTop("删除成功！");
        }
    }
}