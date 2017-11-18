using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;

namespace WX2HK.PLM
{
    public partial class PLM_OnlineSet_Print : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (GetUser() != null)
                {
                    string idList = HttpContext.Current.Request.QueryString["idList"];
                    sendDtl(idList);
                }
            }
        }


        protected void sendDtl(string ddlsStr)
        {
            int printWeight = 2;//条码厚重度
            //表头
            DataTable dt = new DataTable();
            dt.Columns.Add("itemNo");
            dt.Columns.Add("OrderNo");
            dt.Columns.Add("ItemParm");
            dt.Columns.Add("ImgUrl");
            dt.Columns.Add("BarCode");
            dt.Columns.Add("PlanCount");
            string sqlCmd = "select (case IsTemp when '1' then '插单' else '' end) as tempDesc,(case OnlineStatus when '1' then '上线' when '0' then '下线' end) as pStatus,";
            sqlCmd += "(case when ImgURL is not null then '查看' else ImgURL end) as ismlOrno,* from PLM_Product_OnLine a ";
            sqlCmd += "left join PLM_Product_Line b on b.id=a.lineId ";
            sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id ";
            sqlCmd += "left join PLM_Product_Image d on a.id=d.OnlineId ";
            sqlCmd += " where a.id=";
            //解析明细Id
            String[] str = ddlsStr.Split(';');
            //临时datatable
            DataTable tmpDt = new DataTable();
            //string find = "";
            foreach (string it in str)
            {
                if (it == "")
                {
                    break;
                }
                //获取订单流水
                int recId = Convert.ToInt32(it);
                //得到订单明细
                string sqlStr = sqlCmd + recId;
                SqlSel.GetSqlSel(ref tmpDt, sqlStr);
                //查询结果添加至主表dt
                if (tmpDt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["OrderNo"] = tmpDt.Rows[0]["OrderNo"].ToString();
                    dr["ItemParm"] = tmpDt.Rows[0]["ItemParm"].ToString();
                    dr["ImgUrl"] = "../upload/ThumbNail/" + tmpDt.Rows[0]["ImgURL"].ToString();
                    dr["BarCode"] = "code128.aspx?printWeight=" + printWeight + "&num=P" + recId;
                    dr["itemNo"] = tmpDt.Rows[0]["itemNo"].ToString();
                    dr["PlanCount"] = tmpDt.Rows[0]["PlanCount"].ToString();
                    dt.Rows.Add(dr);
                }
                else
                {
                    //find += it;
                }
            }
            //txt_ddls.Value = find;
            //生成明细项
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            //设置gridView表头
            GridView1.Caption = tmpDt.Rows[0]["LineName"].ToString() + "派工单 |" + "  日期:" + DateTime.Now.ToShortDateString();
            //label_printDlt.Text = "打印人:" + GetUser() + " | 打印时间:" + DateTime.Now;
        }

        //添加表格页脚打印信息
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string printUser = "";
            string sqlCmd = "select chinesename from x_user where name='" + GetUser() + "'";
            printUser = SqlSel.GetSqlScale(sqlCmd).ToString();
            if (e.Row.RowType == DataControlRowType.Footer) 
            {
                //e.Row.Cells[0].ColumnSpan = e.Row.Cells.Count;
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[0].Text = "打印人:" + GetUser() + " | 打印时间:" + DateTime.Now;
                TableCell oldTc = e.Row.Cells[0];
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    TableCell tc = e.Row.Cells[i];
                    if (oldTc.Text == tc.Text)
                    {
                        tc.Visible = false;
                        if (oldTc.ColumnSpan == 0)
                        {
                            oldTc.ColumnSpan = 1;
                        }
                        oldTc.ColumnSpan++;
                        //oldTc.VerticalAlign = VerticalAlign.Middle;
                    }
                    else
                    {
                        oldTc = tc;
                    }
                }
                oldTc.Text = "打印人:" + printUser + " | 打印时间:" + DateTime.Now;
                oldTc.HorizontalAlign = HorizontalAlign.Right;
            }
        }



    }
}