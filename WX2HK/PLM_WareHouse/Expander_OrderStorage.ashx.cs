using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using IETCsoft.sql;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace WX2HK.PLM_WareHouse
{
    /// <summary>
    /// Expander_OrderStorage 的摘要说明
    /// </summary>
    public class Expander_OrderStorage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string rowId = context.Request.QueryString["id"];
            string sqlCmd = "select StorageCode,CntrCode,BoundQty from PLM_WH_Storage_Actual WHERE OnlineId='" + rowId + "'";
            sqlCmd += " Union select '零散区' AS StorageCode,BarCode,BindQty from PLM_Serials_BindBarCode WHERE OlineStatus='1'";
            sqlCmd += " AND storageSign <> '1' AND TradeNo='" + rowId + "' AND BarCode like 'B%'";
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