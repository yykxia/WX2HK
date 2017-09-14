using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using System.Data.SqlClient;
using System.Configuration;

namespace WX2HK.PLM
{
    public partial class PLM_ProdConfirm : BasePage
    {
        private static string curUser = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //当前登录人员
                curUser = GetUser();
                bindGrid();

                //
                DatePicker1.SelectedDate = DateTime.Now;
            }
        }

        protected void btn_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                //验证人员ERP权限
                string DefualtUser = ConfigurationManager.AppSettings["User_ERP"];
                if (curUser == DefualtUser)
                {
                    if (string.IsNullOrEmpty(DatePicker1.Text)) 
                    {
                        Alert.ShowInTop("单据日期不可为空！");
                        return;
                    }
                    System.Text.StringBuilder parmStr = new System.Text.StringBuilder();
                    int[] selections = Grid1.SelectedRowIndexArray;
                    string sqlCmd = "", onlineId = "", scddls = "";
                    int bindQty = 0;
                    DateTime billDate = Convert.ToDateTime(DatePicker1.Text);
                    foreach (int rowIndex in selections)
                    {
                        scddls = Grid1.DataKeys[rowIndex][1].ToString();
                        bindQty = Convert.ToInt32(Grid1.Rows[rowIndex].Values[5]);//本次入库数量
                        parmStr.AppendFormat("{0}@@ @@ @@{1}@@", scddls, bindQty);
                        if (bindQty <= 0) //如果有入库数量为0的则跳出
                        {
                            break;
                        }
                        //onlineId = Grid1.DataKeys[rowIndex][0].ToString();
                        //sqlCmd = "update PLM_Product_OnLine set onlineStatus=0,endTime='" + DateTime.Now + "'";
                        //sqlCmd += " where id='" + onlineId + "'";
                        //SqlSel.ExeSql(sqlCmd);
                    }
                    if (bindQty <= 0) 
                    {
                        Alert.ShowInTop("存在数量为0入库数据，不可提交！");
                        return;
                    }

                    //提交存储过程
                    string strsql = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;//数据库链接字符串  
                    string sql = "P_ErpToPLM_RKD";//要调用的存储过程名  
                    SqlConnection conStr = new SqlConnection(strsql);//SQL数据库连接对象，以数据库链接字符串为参数  
                    SqlCommand comStr = new SqlCommand(sql, conStr);//SQL语句执行对象，第一个参数是要执行的语句，第二个是数据库连接对象  
                    comStr.CommandType = CommandType.StoredProcedure;//因为要使用的是存储过程，所以设置执行类型为存储过程  
                    //依次设定存储过程的参数  
                    comStr.Parameters.Add("@company_id", SqlDbType.VarChar, 30).Value = "";
                    comStr.Parameters.Add("@operator_id", SqlDbType.VarChar, 30).Value = curUser;
                    comStr.Parameters.Add("@completing_time", SqlDbType.VarChar, 30).Value = billDate.ToString();//入库单单据日期
                    comStr.Parameters.Add("@xtype", SqlDbType.VarChar, 30).Value = "22";//入库业务类别
                    comStr.Parameters.Add("@warehouse_no", SqlDbType.VarChar, 30).Value = "15";//模塑仓编号
                    comStr.Parameters.Add("@cparm", SqlDbType.VarChar, 4000).Value = parmStr.ToString();
                    conStr.Open();//打开数据库连接  
                    SqlDataAdapter sda = new SqlDataAdapter(comStr);
                    DataTable resultDt = new DataTable();
                    sda.Fill(resultDt);
                    conStr.Close();

                    if (resultDt.Rows.Count > 0)
                    {
                        int comLines = Convert.ToInt32(resultDt.Rows[0]["error_msg"]);
                        if (resultDt.Rows[0]["error_msg"].ToString() == "404"
                            || resultDt.Rows[0]["error_msg"].ToString() == "409")
                        {
                            Alert.Show("生成入库单失败！");
                        }
                        else
                        {
                            if (comLines > 0)
                            {
                                //生成的入库单号
                                string storageNo = resultDt.Rows[0]["storage_no"].ToString();
                                //工单完结
                                foreach (int rowIndex in selections)
                                {
                                    onlineId = Grid1.DataKeys[rowIndex][0].ToString();
                                    bindQty = Convert.ToInt32(Grid1.Rows[rowIndex].Values[5]);//本次入库数量
                                    //如果是已达排产数量的工单则进行完工操作
                                    if (!cb_others.Checked) 
                                    {
                                        sqlCmd = "update PLM_Product_OnLine set onlineStatus=0,endTime='" + DateTime.Now + "'";
                                        sqlCmd += ",StockInNo='" + storageNo + "' where id='" + onlineId + "'";
                                        SqlSel.ExeSql(sqlCmd);
                                    }
                                    //插入工单ERP入库明细
                                    sqlCmd = "INSERT INTO PLM_Product_Records (OnlineId,RecordsNo,CreateTime,BoundQty,BillDate) VALUES";
                                    sqlCmd += "('" + onlineId + "','" + storageNo + "','" + DateTime.Now + "','" + bindQty + "','" + billDate + "')";
                                    SqlSel.ExeSql(sqlCmd);
                                }
                                //刷新表格数据
                                bindGrid();

                                Alert.Show("生成入库单成功！");
                            }
                        }
                    }
                }
                else 
                {
                    Alert.Show("操作终止：非法人员！");
                    return;
                }
                
            }
            catch (Exception ex) 
            {
                Alert.Show(ex.Message);
            }
        }

        private void bindGrid()
        {
            try
            {
                string sqlCmd = "";
                if (!cb_others.Checked)
                {
                    sqlCmd = "select t2.OrderId,ISNULL(t3.BoundQty,0) AS BoundQty,ISNULL(BindQty,0) AS BindQty,";
                    sqlCmd += "ISNULL(t5.RecordQty,0) AS RecordQty,(ISNULL(BindQty,0)-ISNULL(t5.RecordQty,0)) AS ConfirmQty,t1.* from PLM_Product_OnLine t1 left join";
                    sqlCmd += " (select min(OrderId) as OrderId, ProdId from PLM_Product_Rel group by ProdId) t2";
                    sqlCmd += " on t1.Id=t2.ProdId left join (select SUM(BoundQty) as BoundQty,OnlineId ";
                    sqlCmd += " from PLM_WH_Serilas_Bound group by OnlineId) t3 on t3.OnlineId=t1.Id LEFT JOIN";
                    sqlCmd += " (SELECT SUM(BindQty) AS BindQty,TradeNo FROM PLM_Serials_BindBarCode GROUP BY TradeNo) t4";
                    sqlCmd += " ON t4.TradeNo=t1.Id left join";
                    sqlCmd += " (select OnlineId,SUM(BoundQty) AS RecordQty from PLM_Product_Records GROUP BY OnlineId) t5 on t5.OnlineId=t1.Id";
                    sqlCmd += " where t1.OnlineStatus='1' AND BindQty>=PlanCount";
                }
                else
                {
                    //业务日期晚上12点之前的产量加上第二天的夜班产量作为未完工但在业务日期内的合计
                    DateTime billDate = Convert.ToDateTime(DatePicker1.Text).AddDays(1);//业务日期为2017-08-31，则billDate为2017-09-01 00:00
                    string billDate_D = billDate.ToString("yyyy-MM-dd") + " " + "12:00";//后一天12点以前D班及夜班产量合计到该业务日期内
                    sqlCmd = "select t2.OrderId,ISNULL(t3.BoundQty,0) AS BoundQty,(ISNULL(BindQty,0)+ISNULL(BindQtyD,0)) AS BindQty,";
                    sqlCmd += "ISNULL(t5.RecordQty,0) AS RecordQty,((ISNULL(BindQty,0)+ISNULL(BindQtyD,0))-ISNULL(t5.RecordQty,0)) AS ConfirmQty,t1.* from PLM_Product_OnLine t1 left join";
                    sqlCmd += " (select min(OrderId) as OrderId, ProdId from PLM_Product_Rel group by ProdId) t2";
                    sqlCmd += " on t1.Id=t2.ProdId left join (select SUM(BoundQty) as BoundQty,OnlineId ";
                    sqlCmd += " from PLM_WH_Serilas_Bound group by OnlineId) t3 on t3.OnlineId=t1.Id LEFT JOIN";
                    sqlCmd += " (SELECT SUM(BindQty) AS BindQty,TradeNo FROM PLM_Serials_BindBarCode WHERE CreateTime<'" + billDate + "' GROUP BY TradeNo) t4";
                    sqlCmd += " ON t4.TradeNo=t1.Id left join";
                    sqlCmd += " (SELECT SUM(BindQty) AS BindQtyD,TradeNo FROM PLM_Serials_BindBarCode WHERE CreateTime<'" + billDate_D + "' AND CreateTime>'" + billDate + "'";
                    sqlCmd += " AND ExcUser='D' GROUP BY TradeNo) A ON A.TradeNo=T4.TradeNo LEFT JOIN";
                    sqlCmd += " (select OnlineId,SUM(BoundQty) AS RecordQty from PLM_Product_Records GROUP BY OnlineId) t5 on t5.OnlineId=t1.Id";
                    sqlCmd += " where t1.OnlineStatus='1' AND BindQty<PlanCount ORDER BY (ISNULL(BindQty,0)+ISNULL(BindQtyD,0)) DESC";
                }
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
            if (e.CommandName == "bindSerials")
            {
                Window1.Hidden = false;
                Grid grid2 = Window1.FindControl("Grid2") as Grid;
                string onlineId = Grid1.DataKeys[e.RowIndex][0].ToString();
                string sqlCmd = "select * from PLM_Serials_BindBarCode where tradeNo='" + onlineId + "' order by createTime";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                grid2.DataSource = dt;
                grid2.DataBind();
            }
        }

        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            bindGrid();
        }

        protected void cb_others_CheckedChanged(object sender, CheckedEventArgs e)
        {
            if (string.IsNullOrEmpty(DatePicker1.Text)) 
            {
                Alert.ShowInTop("日期不可为空！");
                return;
            }
            else
            {
                bindGrid();
            }
        }
    }
}