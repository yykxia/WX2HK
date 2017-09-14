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
    public partial class WH_TreateCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatePicker1.SelectedDate = DateTime.Now.AddDays(-1);
                DatePicker2.SelectedDate = DateTime.Now.AddDays(-1);

            }
        }

        protected void btn_filter_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime date1 = Convert.ToDateTime(DatePicker1.Text);
                DateTime date2 = Convert.ToDateTime(DatePicker2.Text);
                string strTimeC = "", endTimeC = "";
                string strTimeD = "", endTimeD = "";
                //string onlineStatus = "%";
                //if (ckb_onlineStatus.Checked)
                //{
                //    onlineStatus = "0";
                //}
                strTimeD = date1.ToString("yyyy-MM-dd") + " " + "12:00";
                endTimeD = date2.AddDays(1).ToString("yyyy-MM-dd") + " " + "12:00";
                strTimeC = date1.ToString("yyyy-MM-dd");
                endTimeC = date2.AddDays(1).ToString("yyyy-MM-dd");

                DataTable mutiDt = new DataTable();
                mutiDt.Columns.Add("orderId");
                mutiDt.Columns.Add("Sum_C_B");
                mutiDt.Columns.Add("Sum_D_B");
                mutiDt.Columns.Add("Sum_C_S");
                mutiDt.Columns.Add("Sum_D_S");

                //大架剪边-白班
                string sqlCmd = "select bindSumCB,orderNo,itemParm,tradeNo from (";
                sqlCmd += "select sum(bindQty) as bindSumCB,tradeNo from PLM_WH_TreatRecord_B A left join PLM_Serials_BindBarCode B on ";
                sqlCmd += " A.RecordId=B.ID where TreatStatus='0' AND TreatType='1' AND TreatTime>'" + strTimeC + "' and";
                sqlCmd += " TreatTime<'" + endTimeC + "' and TreatClass='C' and TreatGroup like '" + ddl_group.SelectedValue + "' group by tradeNo";
                sqlCmd += ") t1 left join PLM_Product_OnLine t2 on t1.tradeNo=t2.id";
                DataTable dt_CB = new DataTable();
                SqlSel.GetSqlSel(ref dt_CB, sqlCmd);
                Grid_CB.DataSource = dt_CB;
                Grid_CB.DataBind();

                //大架剪边-夜班
                sqlCmd += "select bindSumDB,orderNo,itemParm,tradeNo from (";
                sqlCmd += "select sum(bindQty) as bindSumDB,tradeNo from PLM_WH_TreatRecord_B A left join PLM_Serials_BindBarCode B on ";
                sqlCmd += " A.RecordId=B.ID where TreatStatus='0' AND TreatType='1' AND TreatTime>'" + strTimeD + "' and";
                sqlCmd += " TreatTime<'" + endTimeD + "' and TreatClass='D' and TreatGroup like '" + ddl_group.SelectedValue + "' group by tradeNo";
                sqlCmd += ") t1 left join PLM_Product_OnLine t2 on t1.tradeNo=t2.id";
                DataTable dt_DB = new DataTable();
                SqlSel.GetSqlSel(ref dt_DB, sqlCmd);
                Grid_DB.DataSource = dt_DB;
                Grid_DB.DataBind();

                DataTable dt_CS = new DataTable();
                dt_CS.Columns.Add("itemParm");
                dt_CS.Columns.Add("orderNo");
                dt_CS.Columns.Add("tradeNo");
                dt_CS.Columns.Add("bindSumCS");


                DataTable dt_DS = new DataTable();
                dt_DS.Columns.Add("itemParm");
                dt_DS.Columns.Add("orderNo");
                dt_DS.Columns.Add("tradeNo");
                dt_DS.Columns.Add("bindSumDS");

                if (ddl_group.SelectedValue == "A")
                {
                    //吊挂剪边(A组白班)
                    DataTable dt_CS_A = TreatSum_S("C", date1, date2);
                    for (int i = 0; i < dt_CS_A.Rows.Count; i++)
                    {
                        DataRow dr = dt_CS.NewRow();
                        dr["tradeNo"] = dt_CS_A.Rows[i]["tradeNo"].ToString();
                        dr["itemParm"] = dt_CS_A.Rows[i]["itemParm"].ToString();
                        dr["orderNo"] = dt_CS_A.Rows[i]["orderNo"].ToString();
                        dr["bindSumCS"] = dt_CS_A.Rows[i]["bindSumS"].ToString();
                        dt_CS.Rows.Add(dr);
                        //string tradeNo_CS = dt_CS_A.Rows[i]["tradeNo"].ToString();
                        //int curBindSum = Convert.ToInt32(dt_CS_A.Rows[i]["bindSumS"]);
                        //findRowIndex(tradeNo_CS, dt_CS, "bindSumCS", curBindSum);
                    }


                    //吊挂剪边(A组夜班)
                    DataTable dt_DS_A = TreatSum_S("D", date1, date2);
                    for (int i = 0; i < dt_DS_A.Rows.Count; i++)
                    {
                        DataRow dr = dt_DS.NewRow();
                        dr["tradeNo"] = dt_DS_A.Rows[i]["tradeNo"].ToString();
                        dr["itemParm"] = dt_DS_A.Rows[i]["itemParm"].ToString();
                        dr["orderNo"] = dt_DS_A.Rows[i]["orderNo"].ToString();
                        dr["bindSumDS"] = dt_DS_A.Rows[i]["bindSumS"].ToString();
                        dt_DS.Rows.Add(dr);
                        //string tradeNo_DS = dt_DS_A.Rows[i]["tradeNo"].ToString();
                        //int curBindSum = Convert.ToInt32(dt_DS_A.Rows[i]["bindSumS"]);
                        //findRowIndex(tradeNo_DS, dt_DS, "bindSumDS", curBindSum);
                    }
                }
                if (ddl_group.SelectedValue == "B")
                {

                    //吊挂剪边统计（B组白班）
                    sqlCmd = "select transferCount,orderNo,itemParm,t1.orderId from (";
                    sqlCmd += "SELECT SUM(transferCount) AS transferCount,orderId FROM PLM_WH_Transfer_OrderBind A LEFT JOIN PLM_WH_Transfer B";
                    sqlCmd += " ON A.TransferId=B.Id WHERE B.bindClss='C' AND B.bindGroup='B' AND B.TransferTime>'" + strTimeC + "'";
                    sqlCmd += " AND B.TransferTime<'" + endTimeC + "' GROUP BY A.orderId";
                    sqlCmd += ") t1 left join PLM_Product_OnLine t2 on t1.orderId=t2.id";
                    DataTable dt_CS_B = new DataTable();
                    SqlSel.GetSqlSel(ref dt_CS_B, sqlCmd);
                    for (int i = 0; i < dt_CS_B.Rows.Count; i++)
                    {
                        DataRow dr = dt_CS.NewRow();
                        dr["tradeNo"] = dt_CS_B.Rows[i]["orderId"].ToString();
                        dr["itemParm"] = dt_CS_B.Rows[i]["itemParm"].ToString();
                        dr["orderNo"] = dt_CS_B.Rows[i]["orderNo"].ToString();
                        dr["bindSumCS"] = dt_CS_B.Rows[i]["transferCount"].ToString();
                        dt_CS.Rows.Add(dr);
                        //string tradeNo_CS = dt_CS_B.Rows[i]["orderId"].ToString();
                        //int curBindSum = Convert.ToInt32(dt_CS_B.Rows[i]["transferCount"]);
                        //findRowIndex(tradeNo_CS, dt_CS, "bindSumCS", curBindSum);
                    }



                    //吊挂剪边统计（B组夜班）
                    sqlCmd = "select transferCount,orderNo,itemParm,t1.orderId from (";
                    sqlCmd += "SELECT SUM(transferCount) AS transferCount,orderId FROM PLM_WH_Transfer_OrderBind A LEFT JOIN PLM_WH_Transfer B";
                    sqlCmd += " ON A.TransferId=B.Id WHERE B.bindClss='D' AND B.bindGroup='B' AND B.TransferTime>'" + strTimeD + "'";
                    sqlCmd += " AND B.TransferTime<'" + endTimeD + "' GROUP BY A.orderId";
                    sqlCmd += ") t1 left join PLM_Product_OnLine t2 on t1.orderId=t2.id";
                    DataTable dt_DS_B = new DataTable();
                    SqlSel.GetSqlSel(ref dt_DS_B, sqlCmd);
                    for (int i = 0; i < dt_DS_B.Rows.Count; i++)
                    {
                        DataRow dr = dt_DS.NewRow();
                        dr["tradeNo"] = dt_DS_B.Rows[i]["orderId"].ToString();
                        dr["itemParm"] = dt_DS_B.Rows[i]["itemParm"].ToString();
                        dr["orderNo"] = dt_DS_B.Rows[i]["orderNo"].ToString();
                        dr["bindSumDS"] = dt_DS_B.Rows[i]["transferCount"].ToString();
                        dt_DS.Rows.Add(dr);
                        //string tradeNo_DS = dt_DS_B.Rows[i]["orderId"].ToString();
                        //int curBindSum = Convert.ToInt32(dt_DS_B.Rows[i]["transferCount"]);
                        //findRowIndex(tradeNo_DS, dt_DS, "bindSumDS", curBindSum);
                    }
                }


                Grid_CS.DataSource = dt_CS;
                Grid_CS.DataBind();


                Grid_DS.DataSource = dt_DS;
                Grid_DS.DataBind();

                //将数据进行汇总
                //if (ddl_group.SelectedValue == "A") 
                //{
                //    //添加大架白班剪边统计
                //    for (int aa = 0; aa < dt_CB.Rows.Count; aa++) 
                //    {
                //        string tradeNo_CB = dt_CB.Rows[aa]["tradeNo"].ToString();
                //        int curBindSum = Convert.ToInt32(dt_CB.Rows[aa]["bindSumCB"]);
                //        findRowIndex(tradeNo_CB, mutiDt, "Sum_C_B", curBindSum);
                //    }


                //    //添加大架夜班剪边统计
                //    for (int aa = 0; aa < dt_DB.Rows.Count; aa++)
                //    {
                //        string tradeNo_DB = dt_DB.Rows[aa]["tradeNo"].ToString();
                //        int curBindSum = Convert.ToInt32(dt_DB.Rows[aa]["bindSumDB"]);
                //        findRowIndex(tradeNo_DB, mutiDt, "Sum_D_B", curBindSum);
                //    }

                //    //添加吊挂架白、夜班剪边统计（A组）
                //    for (int aa = 0; aa < dt_S_A.Rows.Count; aa++)
                //    {
                //        string tradeNo_S = dt_S_A.Rows[aa]["tradeNo_S"].ToString();
                //        int curBindSum_C = Convert.ToInt32(dt_S_A.Rows[aa]["bindSum_CS"]);
                //        int curBindSum_D = Convert.ToInt32(dt_S_A.Rows[aa]["bindSum_DS"]);
                //        findRowIndex(tradeNo_S, mutiDt, "Sum_C_S", curBindSum_C);
                //        findRowIndex(tradeNo_S, mutiDt, "Sum_D_S", curBindSum_D);
                //    }

                //    //添加吊挂架白班剪边统计（B组）                 
                //    for (int aa = 0; aa < dt_CS_B.Rows.Count; aa++)
                //    {
                //        string tradeNo_CS_B = dt_CS_B.Rows[aa]["orderId"].ToString();
                //        int curBindSum = Convert.ToInt32(dt_CS_B.Rows[aa]["transferCount"]);
                //        findRowIndex(tradeNo_CS_B, mutiDt, "Sum_C_S", curBindSum);
                //    }


                //    //添加吊挂架夜班剪边统计（B组）                 
                //    for (int aa = 0; aa < dt_DS_B.Rows.Count; aa++)
                //    {
                //        string tradeNo_DB = dt_DS_B.Rows[aa]["orderId"].ToString();
                //        int curBindSum = Convert.ToInt32(dt_DS_B.Rows[aa]["transferCount"]);
                //        findRowIndex(tradeNo_DB, mutiDt, "Sum_C_S", curBindSum);
                //    }
                //}

                //Grid1.DataSource = mutiDt;
                //Grid1.DataBind();
                //getSummaryData();
                
                //
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
        }

        private void findRowIndex(string tradeNo,DataTable dt,string columnName,int orderCount) 
        {
            //订单号是否已存在
            int find = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["tradeNo"].ToString() == tradeNo)
                {
                    find = i;
                }
            }


            //不存在则添加一行
            if (find < 0)
            {
                DataRow dr = dt.NewRow();
                dr["tradeNo"] = tradeNo;
                dr[columnName] = orderCount;
                dt.Rows.Add(dr);
            }
            else
            {
                if (dt.Rows[find][columnName] == null) 
                {
                    dt.Rows[find][columnName] = 0;
                }
                dt.Rows[find][columnName] = Convert.ToInt32(dt.Rows[find][columnName]) + orderCount;
            }
        }

        //吊挂剪边计件（A组）
        private DataTable TreatSum_S(string loginClass, DateTime date1, DateTime date2) 
        {
            string sqlCmd = "SELECT RecordDate FROM PLM_WH_TreatRecord_FlowLine WHERE RecordDate>='" + date1 + "' AND RecordDate<='" + date2 + "'";
            sqlCmd += " AND loginClass='" + loginClass + "' GROUP BY RecordDate";
            DataTable trtDt = new DataTable();
            SqlSel.GetSqlSel(ref trtDt, sqlCmd);
            DataTable dt_S = new DataTable();
            dt_S.Columns.Add("tradeNo");
            dt_S.Columns.Add("bindSumS");
            dt_S.Columns.Add("itemParm");
            dt_S.Columns.Add("orderNo");
            for (int i = 0; i < trtDt.Rows.Count; i++)
            {
                string recordDate = trtDt.Rows[i]["RecordDate"].ToString();
                sqlCmd = "select bindSumCS,orderNo,itemParm,tradeNo from (";
                sqlCmd += "SELECT SUM(BindQty) AS bindSumCS,TradeNo FROM PLM_Serials_BindBarCode WHERE id >=(";
                sqlCmd += " SELECT RecordId FROM PLM_WH_TreatRecord_FlowLine WHERE RecordDate='" + recordDate + "' AND loginGroup='A'";
                sqlCmd += " AND loginClass='" + loginClass + "' AND RecordType='1') AND Id<=(";
                sqlCmd += " SELECT RecordId FROM PLM_WH_TreatRecord_FlowLine WHERE RecordDate='" + recordDate + "' AND loginGroup='A'";
                sqlCmd += " AND loginClass='" + loginClass + "' AND RecordType='2') AND lineId IN ('1','2') GROUP BY TradeNo";
                sqlCmd += ") t1 left join PLM_Product_OnLine t2 on t1.tradeNo=t2.id";
                DataTable tempDt_C = new DataTable();
                SqlSel.GetSqlSel(ref tempDt_C, sqlCmd);
                for (int j = 0; j < tempDt_C.Rows.Count; j++)
                {
                    string curTradeNo = tempDt_C.Rows[j]["TradeNo"].ToString();
                    int curBindSum = Convert.ToInt32(tempDt_C.Rows[j]["bindSumCS"]);
                    //订单号是否已存在
                    int find = -1;
                    for (int a = 0; a < dt_S.Rows.Count; a++)
                    {
                        if (dt_S.Rows[a]["tradeNo"].ToString() == curTradeNo)
                        {
                            find = i;
                        }
                    }
                    //不存在则添加一行
                    if (find < 0)
                    {
                        DataRow dr = dt_S.NewRow();
                        dr["tradeNo"] = curTradeNo;
                        dr["bindSumS"] = curBindSum;
                        dr["itemParm"] = tempDt_C.Rows[j]["itemParm"];
                        dr["orderNo"] = tempDt_C.Rows[j]["orderNo"];
                        dt_S.Rows.Add(dr);
                    }
                    else
                    {
                        dt_S.Rows[find]["bindSumS"] = Convert.ToInt32(dt_S.Rows[find]["bindSumS"]) + curBindSum;
                    }
                }

            }

            return dt_S;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}