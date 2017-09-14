using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using IETCsoft.sql;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace WX2HK.PLM
{
    /// <summary>
    /// 生成排产计划订单明细扩展表格数据
    /// </summary>
    public class Expander_Grid_Data : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string rowId = context.Request.QueryString["id"];
            string sqlCmd = "select a.orderCount,b.orderNo,b.planSum,b.planProdDate from PLM_Product_Rel a left join View_PLM_ERPData b ";
            sqlCmd += "on a.orderid=b.productSN where prodId='" + rowId + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);

            context.Response.ContentType = "text/plain";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt, new DataTableConverter()));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}