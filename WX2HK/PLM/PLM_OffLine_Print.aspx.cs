using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;

namespace WX2HK.PLM
{
    public partial class PLM_OffLine_Print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idList = HttpContext.Current.Request.QueryString["idList"];
                sendDtl(idList);
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
            //dt.Columns.Add("ImgUrl");
            dt.Columns.Add("BarCode");
            dt.Columns.Add("PlanCount");
            dt.Columns.Add("onlineSum");
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
                    //dr["ImgUrl"] = "../upload/ThumbNail/" + tmpDt.Rows[0]["ImgURL"].ToString();
                    dr["BarCode"] = "code128.aspx?printWeight=" + printWeight + "&num=P" + recId;
                    dr["itemNo"] = tmpDt.Rows[0]["itemNo"].ToString();
                    dr["PlanCount"] = tmpDt.Rows[0]["PlanCount"].ToString();
                    dr["onlineSum"] = tmpDt.Rows[0]["onlineSum"].ToString();
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
            GridView1.Caption = "下架清单 |" + "  日期:" + DateTime.Now.ToShortDateString();
            //label_printDlt.Text = "打印人:" + GetUser() + " | 打印时间:" + DateTime.Now;
        }

    }
}