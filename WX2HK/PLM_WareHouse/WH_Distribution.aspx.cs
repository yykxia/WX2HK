using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using Newtonsoft.Json.Linq;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_Distribution : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string curUser = GetUser();
                bindCartGrid();

                //加载配送产线信息
                loadDistLines();
            }
        }

        //配送产线信息
        private void loadDistLines() 
        {
            string sqlCmd = "select * from PLM_WH_Distribution_Lines where InUse='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            ddl_DistLine.DataValueField = "LineName";
            ddl_DistLine.DataTextField = "LineName";
            ddl_DistLine.DataSource = dt;
            ddl_DistLine.DataBind();
        }

        protected void trgb_sjdh_TriggerClick(object sender, EventArgs e)
        {
            try
            {
                string sjdh = trgb_sjdh.Text;
                if (!string.IsNullOrEmpty(sjdh))
                {
                    string sqlCmd = "SELECT KCCKD2_SCDDLS,KCCKD2_FLBH,ISNULL(ListDistSum,0) AS OutSum,ISNULL(TempDistSum,0) AS TempSum,";
                    sqlCmd += "(CONVERT(nvarchar(50),KCCKD2_LSBH) +'-'+CONVERT(nvarchar(50),KCCKD2_FLBH)) AS SourceNo,";
                    sqlCmd += "KCCKD1_SJDH,KCCKD2_WLBH,KCCKD2_PCH,LSWLZD_GGXH,LSWLZD_WLMC,KCCKD2_QLSL,SCDDCP_JHPCH";
                    sqlCmd += " FROM view_erp_kcckd1,view_erp_kcckd2 LEFT JOIN View_LSWLZD ON LSWLZD_WLBH=KCCKD2_WLBH";
                    sqlCmd += " LEFT JOIN View_ERP_SCDDCP ON KCCKD2_SCDDLS=SCDDCP_LSBH";
                    sqlCmd += " LEFT JOIN (SELECT sourceNo,SUM(ActualDistQty) AS ListDistSum FROM PLM_WH_Distribution_List GROUP BY sourceNo) T1";
                    sqlCmd += " ON T1.sourceNo=(CONVERT(nvarchar(50),KCCKD2_LSBH) +'-'+CONVERT(nvarchar(50),KCCKD2_FLBH))";
                    sqlCmd += " LEFT JOIN (SELECT SourceNo,SUM(distCount)AS TempDistSum FROM PLM_WH_Distribution_Temp GROUP BY SourceNo) T2";
                    sqlCmd += " ON T2.sourceNo=(CONVERT(nvarchar(50),KCCKD2_LSBH) +'-'+CONVERT(nvarchar(50),KCCKD2_FLBH))";
                    sqlCmd += " WHERE KCCKD1_LSBH=KCCKD2_LSBH AND KCCKD1_SJDH='" + sjdh + "' ORDER BY KCCKD2_FLBH";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid2.DataSource = dt;
                    Grid2.DataBind();

                    //清空库存数据
                    Grid1.DataSource = null;
                    Grid1.DataBind();

                    Grid3.DataSource = null;
                    Grid3.DataBind();
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

        protected void Grid2_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                string scddcp_jhpch = Grid2.DataKeys[e.RowIndex][0].ToString();//点击行的计划批次号
                string sourceNo = Grid2.DataKeys[e.RowIndex][1].ToString();//点击行的领料单内码
                string itemNo = Grid2.Rows[e.RowIndex].Values[1].ToString();//物料编码
                //
                cb_otherOrders.Checked = false;
                //
                bindGrid1(scddcp_jhpch, itemNo);
                //重新加载待配送数据
                bindGrid3(sourceNo);
                //合计数量
                summryDistList();
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //加载库存明细
        private void bindGrid1(string scddcp_jhpch,string wlbh) 
        {
            string sqlCmd = "select 0 as selected,A.id,StorageCode,orderNo,CntrCode,BoundQty,BoundTime ";
            sqlCmd += "from PLM_WH_Storage_Actual A left join PLM_Product_OnLine B on A.OnlineId=B.id";
            sqlCmd += " WHERE A.OnlineId in (select ProdId from PLM_Product_Rel t where t.scddcp_jhpch like '" + scddcp_jhpch + "')";
            sqlCmd += " AND A.isLocked <> '1' AND B.ItemNo='" + wlbh + "'";
            sqlCmd += " Union select 0 as selected,t1.id,'零散区' AS StorageCode,orderNo,t1.BarCode,BindQty,CreateTime";
            sqlCmd += " from PLM_Serials_BindBarCode t1 left join PLM_Product_OnLine t2 on t1.TradeNo=t2.id";
            sqlCmd += " WHERE t1.TradeNo in (select ProdId from PLM_Product_Rel t where t.scddcp_jhpch like '" + scddcp_jhpch + "')";
            sqlCmd += " AND t1.OlineStatus='1' AND t1.storageSign<>'1' AND t1.BarCode LIKE 'B%' AND t1.DistLocked <> '1' AND t2.ItemNo='" + wlbh + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.Columns.Add("BoundDays");//添加库龄列
            DateTime curTime = DateTime.Now;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                DateTime boundTime = Convert.ToDateTime(dr["BoundTime"]);//最早入库时间
                TimeSpan ts = curTime - boundTime;//时间差
                double days = ts.TotalDays;
                int BoundDays = Convert.ToInt32(days);//转换成整天数，不满一天为0
                dr["BoundDays"] = BoundDays;
            }

            dt.DefaultView.Sort = "BoundDays desc";//按库龄倒序排列
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CheckBox1")
                {
                    summarySelRows();
                }
            }
            catch (Exception ex) 
            {
                Alert.Show(ex.Message);
            }
        }

        private void summarySelRows()
        {
            try
            {

                int selectedSum = 0;
                FineUI.CheckBoxField selectedField = Grid1.FindColumn("isSel") as FineUI.CheckBoxField;
                foreach (GridRow row in Grid1.Rows)
                {
                    bool isSelected = selectedField.GetCheckedState(row.RowIndex);
                    if (isSelected)
                    {
                        int curCount = Convert.ToInt32(row.Values[6]);//选中行的库存数量
                        selectedSum += curCount;
                    }
                }
                label_Sum.Text = "<span style='color:blue;font-size:x-large'>" + selectedSum.ToString() + "</span>";
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }

        }

        private void ChangeCheckBoxStatus(bool checkedState)
        {
            CheckBoxField field1 = (CheckBoxField)Grid1.FindColumn("isSel");


            foreach (GridRow row in Grid1.Rows)
            {
                field1.SetCheckedState(row.RowIndex, checkedState);
            }

        }



        protected void btnSelectRows_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheckBoxStatus(true);
                summarySelRows();
            }
            catch (Exception ex) 
            {
                Alert.Show(ex.Message);
            }
        }

        protected void btnUnselectRows_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheckBoxStatus(false);
                summarySelRows();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
        }

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            summarySelRows();
        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            string sourceNo = Grid2.DataKeys[Grid2.SelectedRowIndex][1].ToString();//生产领料单内码
            string itemNo = Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString();//领料单点击行的物料编码
            string curUser = GetUser();//当前登录用户
            string StorageCode = "", ContCode = "", PlanDistQty = "";
            string sqlCmd = "";
            if (!string.IsNullOrEmpty(curUser))
            {
                try
                {
                    //获取当前数据
                    DataTable dt = new DataTable();

                    FineUI.CheckBoxField selectedField = Grid1.FindColumn("isSel") as FineUI.CheckBoxField;

                    //是否选取相关配送流水线
                    if (ddl_DistLine.SelectedItem == null) 
                    {
                        Alert.ShowInTop("请选择相关配送流水线！");
                        return;
                    }

                    for (int i = 0; i < Grid1.Rows.Count; i++)
                    {
                        GridRow row = Grid1.Rows[i];
                        bool isSelected = selectedField.GetCheckedState(i);
                        string storageId = Grid1.DataKeys[i][0].ToString();
                        if (isSelected)
                        {
                            StorageCode = row.Values[1].ToString();//货位
                            ContCode = row.Values[3].ToString();//货架号
                            PlanDistQty = row.Values[6].ToString();//拟出库数量        
                            int scatSign = 0;//零散区标志
                            if (StorageCode != "零散区")
                            {
                                //锁定真实库位
                                sqlCmd = "update PLM_WH_Storage_Actual set isLocked='1' where id='" + storageId + "'";
                            }
                            else 
                            {
                                scatSign = 1;
                                sqlCmd = "update PLM_Serials_BindBarCode set DistLocked='1' where id='" + storageId + "'";
                            }
                            if (SqlSel.ExeSql(sqlCmd) > 0)
                            {
                                //生成草稿状态配送数据
                                sqlCmd = "insert into PLM_WH_Distribution_Temp (SourceNo,strorageId,distCount,optUser,addTime,itemNo,isTemp,scatSign,distLine)";
                                sqlCmd += " values ('" + sourceNo + "','" + storageId + "','" + PlanDistQty + "',";
                                sqlCmd += "'" + curUser + "','" + DateTime.Now + "','" + itemNo + "','1','" + scatSign + "','" + ddl_DistLine.SelectedValue + "')";
                                SqlSel.ExeSql(sqlCmd);
                            }
                        }
                    }

                    //加载剩余库存明细
                    if (cb_otherOrders.Checked)
                    {
                        bindGrid1("%", Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString());
                    }
                    else
                    {
                        bindGrid1(Grid2.DataKeys[Grid2.SelectedRowIndex][0].ToString(), Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString());
                    }                    
                    //重新加载待配送数据
                    bindGrid3(sourceNo);
                    //刷新配送车数据
                    bindCartGrid();
                    //库存合计
                    summarySelRows();
                    //合计数量
                    summryDistList();
                }
                catch (Exception ex)
                {
                    Alert.Show(ex.Message);
                }
            }

        }

        //待生成配送数据清单
        private void bindGrid3(string sourceNo)
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select strorageId,scatSign,StorageCode,CntrCode,distCount,distLine from PLM_WH_Distribution_Temp A";
            sqlCmd += " LEFT JOIN PLM_WH_Storage_Actual B ON A.strorageId=B.id";
            sqlCmd += " where optUser='" + GetUser() + "' and SourceNo='" + sourceNo + "' and isTemp='1' and scatSign='0'";
            sqlCmd += " union select strorageId,scatSign,'零散区' as StorageCode,BarCode,distCount,distLine from PLM_WH_Distribution_Temp t1";
            sqlCmd += " left join PLM_Serials_BindBarCode t2 on t1.strorageId=t2.id";
            sqlCmd += " where optUser='" + GetUser() + "' and SourceNo='" + sourceNo + "' and isTemp='1' and scatSign='1'";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid3.DataSource = dt;
            Grid3.DataBind();
        }

        //是否已存在于配送列表
        private bool AlreadyExsit(string contCode) 
        {
            int find = -1;
            for (int i = 0; i < Grid3.Rows.Count; i++) 
            {
                string curContCode = Grid3.Rows[i].Values[2].ToString();
                if (curContCode == contCode) 
                {
                    find = i;
                    break;
                }
            }

            if (find >= 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        //统计已添加配送列表数量
        private void summryDistList() 
        {
            int sumDist = 0;
            for (int i = 0; i < Grid3.Rows.Count; i++) 
            {
                int curCount = Convert.ToInt32(Grid3.Rows[i].Values[2]);
                sumDist += curCount;
            }

            label_distSum.Text = "<span style='color:red;font-size:x-large'>" + sumDist.ToString() + "</span>";
        }

        protected void Grid3_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "move")
            {
                try
                {
                    string storageId = Grid3.DataKeys[e.RowIndex][0].ToString();
                    string scatSign = Grid3.DataKeys[e.RowIndex][1].ToString();
                    string sqlCmd = "delete from PLM_WH_Distribution_Temp where strorageId='" + storageId + "'";
                    if (SqlSel.ExeSql(sqlCmd) > 0)
                    {
                        if (scatSign == "0")
                        {
                            sqlCmd = "update PLM_WH_Storage_Actual set isLocked='0' where id='" + storageId + "'";
                        }
                        if (scatSign == "1") 
                        {
                            sqlCmd = "update PLM_Serials_BindBarCode set DistLocked='0' where id='" + storageId + "'";
                        }
                    }
                    SqlSel.ExeSql(sqlCmd);

                    //加载剩余库存明细
                    bindGrid1(Grid2.DataKeys[Grid2.SelectedRowIndex][0].ToString(), Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString());
                    //重新加载待配送数据
                    bindGrid3(Grid2.DataKeys[Grid2.SelectedRowIndex][1].ToString());
                    //刷新配送车数据
                    bindCartGrid();
                    //合计数量
                    summryDistList();
                    //库存合计
                    summarySelRows();
                }
                catch (Exception ex)
                {
                    Alert.Show(ex.Message);
                }
            }
        }

        //private DataTable getGridData(FineUI.Grid grid)
        //{
        //    DataTable dt = new DataTable();

        //    dt.Columns.Add("DistNo");
        //    dt.Columns.Add("StorageCode");
        //    dt.Columns.Add("ContCode");
        //    dt.Columns.Add("outStatus");
        //    dt.Columns.Add("PlanDistQty");
        //    dt.Columns.Add("ActualDistQty");

        //    foreach (GridRow row in grid.Rows) 
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["DistNo"] = row.Values[0];
        //        dr["StorageCode"] = row.Values[1];
        //        dr["ContCode"] = row.Values[2];
        //        dr["outStatus"] = row.Values[3];
        //        dr["PlanDistQty"] = row.Values[4];
        //        dr["ActualDistQty"] = row.Values[5];
        //        dt.Rows.Add(dr);
        //    }
            
        //    return dt;
        //}

        private void bindCartGrid() 
        {
            string sqlCmd = "select SourceNo,SUM(distCount) AS distSum,itemNo from PLM_WH_Distribution_Temp where";
            sqlCmd += " optUser='" + GetUser() + "' and isTemp='1' group by SourceNo,itemNo";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid grid_cart = Window2.FindControl("Grid_cart") as Grid;
            grid_cart.DataSource = dt;
            grid_cart.DataBind();
        }

        protected void btn_dist_Click(object sender, EventArgs e)
        {
            //string sqlCmd = "select SourceNo,SUM(distCount) AS distSum,itemNo from PLM_WH_Distribution_Temp where";
            //sqlCmd += " optUser='" + GetUser() + "' and isTemp='1' group by SourceNo,itemNo";
            //DataTable dt = new DataTable();
            //SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid grid_cart = Window2.FindControl("Grid_cart") as Grid;

            if (grid_cart.Rows.Count > 0)
            {
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                ////sb.AppendFormat(":\r\n");
                //for (int i = 0; i < dt.Rows.Count; i++) 
                //{
                //    sb.AppendFormat("物料:{0},数量:{1}<br/>", dt.Rows[i]["itemNo"].ToString(), dt.Rows[i]["distSum"].ToString());
                //}
                //sb.AppendFormat("是否继续？");
                //string tipContext = sb.ToString();
                PageContext.RegisterStartupScript(Confirm.GetShowReference("确认提交配送？",
                    string.Empty, MessageBoxIcon.Question, PageManager1.GetCustomEventReference("Confirm_OK"),
                    PageManager1.GetCustomEventReference("Confirm_Cancel")));
            }
            else 
            {
                Alert.ShowInTop("尚未添加任何配送数据！");
                return;
            }

        }

        protected void PageManager1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.EventArgument == "Confirm_OK") 
            {
                string sqlCmd = "update PLM_WH_Distribution_Temp set isTemp='0' where optUser='" + GetUser() + "' and isTemp='1'";
                if (SqlSel.ExeSql(sqlCmd) > 0) 
                {
                    Alert.Show("配送计划添加成功！");
                    Grid grid_cart = Window2.FindControl("Grid_cart") as Grid;
                    grid_cart.DataSource = null;
                    grid_cart.DataBind();

                    //
                    Grid3.DataSource = null;
                    Grid3.DataBind();
                }
            }
            if (e.EventArgument == "Confirm_Cancel")
            {

            }
        }

        protected void cb_otherOrders_CheckedChanged(object sender, CheckedEventArgs e)
        {
            if (cb_otherOrders.Checked)
            {
                bindGrid1("%", Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString());
            }
            else
            {
                bindGrid1(Grid2.DataKeys[Grid2.SelectedRowIndex][0].ToString(), Grid2.Rows[Grid2.SelectedRowIndex].Values[1].ToString());
            }
        }
    }
}