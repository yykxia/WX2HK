using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FineUI;
using System.Text;
using AspNet = System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using IETCsoft.sql;

namespace WX2HK.MES
{
    public partial class C2M2ERP : System.Web.UI.Page
    {

        public string insert_sql,delete_sql,sql,update_sql;
        public static DataTable Worder,MiddleWorder;
        public static string ProductCode,InstockCode,OutstockCode, ChoiceText ;
        public static MESWebservice MesWeb;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid(ChoiceText);
        }

        private void InitGrid(string choicetext)
        {
                
                FineUI.BoundField bf;
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_number";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "订单号";
                bf.Width = 200;
                WorderGrid.Columns.Add(bf);
                 //0
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_productnumber";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "成品编码";
                bf.Width = 150;
                WorderGrid.Columns.Add(bf);
                //1
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_productnum";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "订单数量";
                bf.Width = 80;
                WorderGrid.Columns.Add(bf);
                //2
                bf = new FineUI.BoundField();
                bf.DataField = "worder_number";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "工单号";
                bf.Width = 100;
                WorderGrid.Columns.Add(bf);
                //3
                bf = new FineUI.BoundField();
                bf.DataField = "worder_rmtnumber";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "物料编码";
                bf.Width = 150;
                WorderGrid.Columns.Add(bf);
                //4
                bf = new FineUI.BoundField();
                bf.DataField = "worder_rmtname";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "物料名称";
                bf.Width = 160;
                WorderGrid.Columns.Add(bf);
               //5
                bf = new FineUI.BoundField();
                bf.DataField = "worder_rmttype";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "物料信息";
                bf.Width = 700;
                WorderGrid.Columns.Add(bf);
                //6
                bf = new FineUI.BoundField();
                bf.DataField = "worder_rmtunit";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "计量单位";
                WorderGrid.Columns.Add(bf);
                //7
                bf = new FineUI.BoundField();
                bf.DataField = "worder_rmtnum";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "物料数量";
                bf.Width = 160;
                WorderGrid.Columns.Add(bf);
                //8
                bf = new FineUI.BoundField();
                bf.DataField = "inserttime";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "下发时间";
                bf.Width =0;
                WorderGrid.Columns.Add(bf);
                //9
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_id";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "订单识别码";
                bf.Width = 0;
                WorderGrid.Columns.Add(bf);
                //10
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_Blength";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "产品长度";
                bf.Width = 0;
                WorderGrid.Columns.Add(bf);
                //11
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_Bwidth";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "产品宽度";
                bf.Width = 0;
                WorderGrid.Columns.Add(bf);
                //12
                bf = new FineUI.BoundField();
                bf.DataField = "uorder_Bthickness";
                bf.DataFormatString = "{0}";
                bf.HeaderText = "产品厚度";
                bf.Width = 0;
                WorderGrid.Columns.Add(bf);
                //13
            WorderGrid.DataKeyNames = new string[] { "uorder_number", "uorder_productnumber", "uorder_productnum", "worder_number",
                                                             "worder_rmtnumber","worder_rmtname","worder_rmttype", "worder_rmtunit","worder_rmtnum",
                                                            "inserttime","uorder_id","uorder_Blength","uorder_Bwidth","uorder_Bthickness"};
            }

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            try
            {
               
                 MesWeb = new MESWebservice();
                DateTime dt = Convert.ToDateTime(DatePicker1.Text); 
                string DateBegin = DatePicker1.Text.Substring(0, 10);
                string DateEnd = DatePicker2.Text.Substring(0, 10);
             
                ChoiceText = ddl_line.SelectedItem.Text;
                if (ChoiceText == "MES工单-ERP生产订单")
                {
                    Worder = MesWeb.MES_Worder(DateBegin, DateEnd, ChoiceText, "MES");
                    //"MES"mes工单查询 "MIDDLE" 插入中间库
                    //MES工单过滤
                }
                else if (ChoiceText == "MES销售-ERP入库领料")
                {
                    MiddleWorder = MesWeb.MES_Worder(DateBegin, DateEnd, ChoiceText, "Instock");
                    //取得MES中间库的耗用数据
                    delete_sql = "delete  from ORDER_CONSUME";
                    SqlSel.ExemesSql(delete_sql, "MIDDLE");     //删除临时中间库耗用数据
                    int row;
                  decimal   consume;   //面料和非面料耗用取值字段不同
                    for (row = 0; row < MiddleWorder.Rows.Count; row++)
                     {
                         if (MiddleWorder.Rows[row]["WLBM"].ToString().Substring(0, 2).Equals("ML"))
                              {
                                  consume = Math.Round(Convert.ToDecimal(MiddleWorder.Rows[row]["L"]) / 100,2);  //面料由厘米化为米 
                               }
                             else 
                               {
                                   consume = Math.Round(Convert.ToDecimal(MiddleWorder.Rows[row]["Qty"]));
                                }
                         insert_sql = "insert into ORDER_CONSUME(uorder_number,uorder_productnumber,uorder_productnum,worder_rmtnumber,worder_rmtname,worder_rmtnum";
                         insert_sql += ",worder_source,worder_volume,BLENGTH,BWIDTH,BTHICKNESS";
                         insert_sql += ") values('" + MiddleWorder.Rows[row]["DDH"] + "','" + MiddleWorder.Rows[row]["GDH"] + "'," + Convert.ToInt16(MiddleWorder.Rows[row]["UseQty"]) + ",";
                         insert_sql += " '" + MiddleWorder.Rows[row]["WLBM"] + "','" + MiddleWorder.Rows[row]["WLMC"] + "'," + consume + " ,'" + MiddleWorder.Rows[row]["WLLY"] + "'";
                         insert_sql += "," + Convert.ToDecimal(MiddleWorder.Rows[row]["V"]) + "," + Convert.ToDecimal(MiddleWorder.Rows[row]["DL"]) + "," + Convert.ToDecimal(MiddleWorder.Rows[row]["DW"]) +"";
                         insert_sql += ", " +  Convert.ToDecimal(MiddleWorder.Rows[row]["DH"]) + " ) ";
                         
                        SqlSel.ExemesSql(insert_sql, "MIDDLE");     //插入中间库耗用数据 
                    }
                    Worder = MesWeb.MES_Worder("", "", ChoiceText, "Consume");
                    //"Instock"入库 
                }
                if (Worder.Rows.Count!=0)
                  {
                      WorderGrid.DataSource = Worder;
                      WorderGrid.DataBind();
                      btn_middle.Enabled = true;
                  }
                  else
                  {
                      WorderGrid.DataSource = Worder;
                      WorderGrid.DataBind();
                    // Alert.ShowInTop("你选择的条件无数据");
                  }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strsql = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;//connectstring
            
            if (ddl_line.SelectedText == "MES工单-ERP生产订单")
            {
                 sql = "[lc0019999].[P_MESTOERP_PRODUCTNO]";
            }
            if (ddl_line.SelectedText == "MES销售-ERP入库领料")
            {
                sql = "[lc0019999].[P_MESTOERP_INSTOCK]";
                
            }
            SqlConnection constr = new SqlConnection(strsql);//database connect object  parameter is connectstring
            //SqlCommand comStr = new SqlCommand(sql, constr); // sql execute object  the first is  sql the second is connect
            //comStr.CommandType = CommandType.StoredProcedure; // set execute type
            int selectedCount = WorderGrid.SelectedRowIndexArray.Length;
         
            if (ddl_line.SelectedText == "MES工单-ERP生产订单")
            {
                if (selectedCount > 0)
                {
                    int[] selections = WorderGrid.SelectedRowIndexArray;
                    foreach (int rowIndex in selections)
                    {
                        SqlCommand comStr = new SqlCommand(sql, constr); // sql execute object  the first is  sql the second is connect
                        comStr.CommandType = CommandType.StoredProcedure; // set execute type
                        comStr.Parameters.Add("@GUID", SqlDbType.VarChar, 50).Value = WorderGrid.DataKeys[rowIndex][9];
                        comStr.Parameters.Add("@uorder_number", SqlDbType.VarChar, 30).Value = WorderGrid.DataKeys[rowIndex][0];            //original order no 
                        comStr.Parameters.Add("@operator_id", SqlDbType.VarChar, 30).Value = "2673";                                   //order productno 
                        comStr.Parameters.Add("@completing_time", SqlDbType.VarChar, 10).Value = DateTime.Now.ToString("yyyy-MM-dd");  //original order id
                        constr.Open();//open database connect
                        SqlDataAdapter sda = new SqlDataAdapter(comStr);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            int comLines = Convert.ToInt32(dt.Rows[0]["error_msg"]);
                            ProductCode = dt.Rows[0]["storage_no"].ToString();  //erp入库单号
                            if (dt.Rows[0]["error_msg"].ToString() == "404"
                                || dt.Rows[0]["error_msg"].ToString() == "409")
                            {
                                Response.Write("<script>alert('生成订单失败')</script>");
                                return;
                            }
                            else
                            {
                                //cmd.ExecuteNonQuery();
                            }
                        }
                        constr.Close();
                        insert_sql = "insert into ORDER_ATTRIBUTE(ORDER_GUID,FLAG,BILL_NUMBER,ATTRIBUTE_MEM) values ('" + WorderGrid.DataKeys[rowIndex][9] + "','SCDD','" + ProductCode + "','" + DateTime.Now.ToShortDateString().ToString().Substring(0, 10) + "') ";
                        SqlSel.ExemesSql(insert_sql, "MIDDLE");
                    
                    }
                    //该行已经插入中间库
                    Worder = MesWeb.MES_Worder("", "", ChoiceText, "MIDDLE");  //中间库
                    WorderGrid.DataSource = Worder;
                    WorderGrid.DataBind();
                    Button1.Enabled = true; 
                    Alert.ShowInTop("生产ERP生产订单成功!!");
                
                }
                else
                {
                    Alert.ShowInTop("没有选中任何一行！");
                    // sb.Append("<strong>没有选中任何一行！</strong>");
                }
            }

        if (ddl_line.SelectedText == "MES销售-ERP入库领料")
        {
            string  strwh = "";
            if (selectedCount > 0)
                {
                    int[] selections = WorderGrid.SelectedRowIndexArray;
                    foreach (int rowIndex in selections)
                    {
                        SqlCommand comStr = new SqlCommand(sql, constr); // sql execute object  the first is  sql the second is connect
                        comStr.CommandType = CommandType.StoredProcedure; // set execute type
                        comStr.Parameters.Add("@ORDERNO", SqlDbType.VarChar, 30).Value = WorderGrid.DataKeys[rowIndex][0];            //original order no 
                        comStr.Parameters.Add("@P_CINVCODE", SqlDbType.VarChar, 30).Value = WorderGrid.DataKeys[rowIndex][1];
                        comStr.Parameters.Add("@INSTOCK_QTY", SqlDbType.Int).Value = Convert.ToInt16(WorderGrid.DataKeys[rowIndex][2]);
                        comStr.Parameters.Add("@completing_time", SqlDbType.VarChar, 10).Value = DateTime.Now.ToShortDateString().ToString().Substring(0, 10);  //original order id
                        comStr.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt16(WorderGrid.DataKeys[rowIndex][10]);
                        comStr.Parameters.Add("@BLENGTH", SqlDbType.Int).Value = Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][11]);
                        comStr.Parameters.Add("@BWIDTH", SqlDbType.Int).Value = Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][12]);
                        comStr.Parameters.Add("@BTHICKNESS", SqlDbType.Int).Value = Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][13]);
                        comStr.Parameters.Add("@operator_id", SqlDbType.VarChar, 30).Value = "2673";
                        constr.Open();//open database connect
                        SqlDataAdapter sda = new SqlDataAdapter(comStr);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        strwh = strwh + "'" + WorderGrid.DataKeys[rowIndex][0].ToString() + "',";
                        if (dt.Rows.Count > 0)
                        {
                            int comLines = Convert.ToInt32(dt.Rows[0]["error_msg"]);
                            InstockCode = dt.Rows[0]["storage_no"].ToString();  //erp入库单号
                            OutstockCode = dt.Rows[0]["out_no"].ToString();  //erp入库单号
                            if (dt.Rows[0]["error_msg"].ToString() == "404"
                                || dt.Rows[0]["error_msg"].ToString() == "409")
                            {
                                Response.Write("<script>alert('生成入库领料失败')</script>");
                                return;
                            }
                            else
                            {
                                //cmd.ExecuteNonQuery();
                            }
                        }
                        constr.Close();
                        insert_sql = "insert into ORDER_ATTRIBUTE(FLAG,BILL_NUMBER,ATTRIBUTE_MEM) values ('KCRKD','" + InstockCode + "','" + WorderGrid.DataKeys[rowIndex][0] + "') ";
                        SqlSel.ExemesSql(insert_sql, "MIDDLE");
                        insert_sql = "insert into ORDER_ATTRIBUTE(FLAG,BILL_NUMBER,ATTRIBUTE_MEM) values ('KCCKD','" + OutstockCode + "','" + WorderGrid.DataKeys[rowIndex][0] + "') ";
                        SqlSel.ExemesSql(insert_sql, "MIDDLE");
                        update_sql = "update ORDER_ATTRIBUTE set ORDER_GUID=GUID from ORDER_ATTRIBUTE inner join ORDER_MASTER  on ATTRIBUTE_MEM=ORDERNO ";
                        SqlSel.ExemesSql(update_sql, "MIDDLE");
                        //该行已经插入中间库
                    }
                    strwh = strwh.TrimEnd(',');
                    update_sql = "update ORDER_INSTOCK_CONSUME set flag=2 from ORDER_INSTOCK_CONSUME inner join ORDER_INSTOCK on ORDER_INSTOCK_CONSUME.ORDER_INSTOCK_ID=ORDER_INSTOCK.id";
                    update_sql += " where ORDER_INSTOCK.ORDERNO in (" + strwh + ")";
                    SqlSel.ExemesSql(update_sql, "MIDDLE");
                Worder = MesWeb.MES_Worder("", "", ChoiceText, "ErpConsume");
           if (Worder.Rows.Count != 0)
           {
               WorderGrid.DataSource = Worder;
               WorderGrid.DataBind();
               btn_middle.Enabled = false;
               Button1.Enabled = true;
           }
           else
           {
               WorderGrid.DataSource = Worder;
               WorderGrid.DataBind();
               // Alert.ShowInTop("你选择的条件无数据");
           }
           Alert.ShowInTop("生产ERP入库领料成功!!"); 
            }
                else
                {
                    Alert.ShowInTop("没有选中任何一行！");
                    // sb.Append("<strong>没有选中任何一行！</strong>");
                }
            }
            
        }
  

        protected void btn_middle_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int selectedCount = WorderGrid.SelectedRowIndexArray.Length;
            //int selectedCount=          (WorderGrid);
          
       if (ddl_line.SelectedText =="MES工单-ERP生产订单")      
       {
           if (selectedCount > 0)
           {
               int[] selections = WorderGrid.SelectedRowIndexArray;
               foreach (int rowIndex in selections)
               {
                   System.Guid guid = System.Guid.NewGuid();   //produce new guid
                   sb.AppendFormat("行号:{0} 成品编码:{1} 成品编码:{2} <br />", rowIndex + 1, WorderGrid.DataKeys[rowIndex][1], WorderGrid.DataKeys[rowIndex][2]);
                   string strsql = ConfigurationManager.ConnectionStrings["C2M2ERPMIDDLE"].ConnectionString;//connectstring
                   string sql = "INSERT_ORDER";
                   SqlConnection constr = new SqlConnection(strsql);//database connect object  parameter is connectstring
                   SqlCommand comStr = new SqlCommand(sql, constr); // sql execute object  the first is  sql the second is connect 
                   comStr.CommandType = CommandType.StoredProcedure; // set execute type
                   //set parametetlist
                   comStr.Parameters.Add("@GUID", SqlDbType.VarChar, 100).Value = guid.ToString();
                   comStr.Parameters.Add("@ORDERNO", SqlDbType.VarChar, 50).Value = WorderGrid.DataKeys[rowIndex][0];            //original order no 
                   comStr.Parameters.Add("@P_CINVCODE ", SqlDbType.VarChar, 100).Value = WorderGrid.DataKeys[rowIndex][1];        //order productno 
                   comStr.Parameters.Add("@UORDERID", SqlDbType.Int).Value = Convert.ToInt32(WorderGrid.DataKeys[rowIndex][10]);  //original order id
                   comStr.Parameters.Add("@ORDER_QTY ", SqlDbType.Int).Value = Convert.ToInt16(WorderGrid.DataKeys[rowIndex][2]);  //order productno 
                   comStr.Parameters.Add("@WORDERNO ", SqlDbType.VarChar, 50).Value = WorderGrid.DataKeys[rowIndex][3];             //original worder no 
                   comStr.Parameters.Add("@CINVCODE", SqlDbType.VarChar, 100).Value = WorderGrid.DataKeys[rowIndex][4];       //original worder product code 
                   comStr.Parameters.Add("@CINVNAME", SqlDbType.VarChar, 100).Value = WorderGrid.DataKeys[rowIndex][5];       //original worder product name
                   comStr.Parameters.Add("@CINVTYPE ", SqlDbType.VarChar, 100).Value = WorderGrid.DataKeys[rowIndex][6];       //original worder product  type 
                   comStr.Parameters.Add("@ORDER_NUMBER", SqlDbType.Decimal, 18).Value = WorderGrid.DataKeys[rowIndex][8];     //original worder product QUANTITY
                   comStr.Parameters.Add("@UNIT", SqlDbType.VarChar, 10).Value = WorderGrid.DataKeys[rowIndex][7];            //original worder product UNIT
                   comStr.Parameters.Add("@BLENGTH", SqlDbType.Decimal, 18).Value = WorderGrid.DataKeys[rowIndex][11];         //original uorder product length
                   comStr.Parameters.Add("@BWIDTH", SqlDbType.Decimal, 18).Value = WorderGrid.DataKeys[rowIndex][12];          //original uorder product width
                   comStr.Parameters.Add("@BTHICKNESS", SqlDbType.Decimal, 18).Value = WorderGrid.DataKeys[rowIndex][13];       //original uorder product THICKNESS
                   //comStr.Parameters.Add("@cparm", SqlDbType.VarChar, 4000).Value = parmStr.ToString();
                   constr.Open();//open database connect
                   SqlDataAdapter sda = new SqlDataAdapter(comStr);
                   DataTable dt = new DataTable();
                   sda.Fill(dt);
                   //string str = dt.Rows[0][0].ToString() + "#" + dt.Rows[0][1].ToString() + "#" + dt.Rows[0][2].ToString() + "#" + dt.Rows[0][3].ToString() + "#" + dt.Rows[0][4].ToString() + "#" + dt.Rows[0][5].ToString();
                  // Alert.ShowInTop(str);
                   constr.Close();
                   updatesql = "update Worder set insert_status=1 where worder_number='" + WorderGrid.DataKeys[rowIndex][3] + "'";
                   SqlSel.ExemesSql(updatesql, "MES");
                   //该行已经插入中间库
               }
               // labResult.Text = sb.ToString();
               Alert.ShowInTop("导入中间库生产订单数据成功!!");
               // btn_filter_Click(null, null);
               btn_middle.Enabled = false;
               Worder = MesWeb.MES_Worder("", "", ChoiceText, "MIDDLE");  //中间库
               WorderGrid.DataSource = Worder;
               WorderGrid.DataBind();
               Button1.Enabled = true; 
           }    
            else
            {
                Alert.ShowInTop("没有选中任何一行！");
               // sb.Append("<strong>没有选中任何一行！</strong>");
            }
         }
       if (ddl_line.SelectedText == "MES销售-ERP入库领料")
       {
           string update_sql,strwh="";
        
           
           if (selectedCount > 0)
           {
             int[] selections = WorderGrid.SelectedRowIndexArray;
             foreach (int rowIndex in selections)
             {
                 Worder = MesWeb.Insert_Worder(WorderGrid.DataKeys[rowIndex][0].ToString(), WorderGrid.DataKeys[rowIndex][1].ToString(), 
                           Convert.ToInt16(WorderGrid.DataKeys[rowIndex][2].ToString()), WorderGrid.DataKeys[rowIndex][4].ToString(),
                           Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][8]), Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][11]),
                           Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][12]), Convert.ToDecimal(WorderGrid.DataKeys[rowIndex][13]));
                 strwh = strwh+"'"+ WorderGrid.DataKeys[rowIndex][0].ToString()+"',";
             }
             strwh = strwh.TrimEnd(',');
             update_sql = "update ORDER_INSTOCK_CONSUME set flag=1 from ORDER_INSTOCK_CONSUME inner join ORDER_INSTOCK on ORDER_INSTOCK_CONSUME.ORDER_INSTOCK_ID=ORDER_INSTOCK.id";
             update_sql += " where ORDER_INSTOCK.ORDERNO in (" + strwh + ")";
             SqlSel.ExemesSql(update_sql, "MIDDLE");
             Alert.ShowInTop("导入中间库生产耗用数据成功!!"); 
           }
           Worder = MesWeb.MES_Worder("", "", ChoiceText, "ErpConsume");
           if (Worder.Rows.Count != 0)
           {
               WorderGrid.DataSource = Worder;
               WorderGrid.DataBind();
               btn_middle.Enabled = false;
               Button1.Enabled = true;

           }
           else
           {
               WorderGrid.DataSource = Worder;
               WorderGrid.DataBind();
               // Alert.ShowInTop("你选择的条件无数据");
           }
       }
      
    }

        protected void DatePicker1_DateSelect(object sender, EventArgs e)
        {//

        }

        protected void ddl_line_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChoiceText = ddl_line.SelectedItem.Text;
            InitGrid(ChoiceText);
        }

        protected void ddl_line_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存每一页的选择项
        /// </summary>
        public void getCheckedItem()
        {
            for (int i = 0; i < WorderGrid.Rows.Count; i++)
            {
                FineUI.CheckBox currentCbx = WorderGrid.Rows[i].FindControl("CheckBox1") as FineUI.CheckBox;
                //if (currentCbx.Checked && dic[currentValue] == false)
                //{
                //    dic[currentValue] = true;
                //}
                //else if (!currentCbx.Checked && dic[currentValue] == true)
                //{
                //    dic[currentValue] = false;
                //}
            }
        }

        protected void WorderGrid_PageIndexChange(object sender, GridPageEventArgs e)
        {
//HowManyRowsSelected(WorderGrid);
        }
    }
}