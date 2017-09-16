using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Data;

namespace WX2HK
{
    /// <summary>
    /// DRPWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DRPWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// ERP物料信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string DRP_ERP_Items()
        {
            string sqlCmd = "SELECT LSWLZD_WLBH,LSWLZD_WLMC,LSWLZD_GGXH,LSWLZD_LBBH,LSWLZD_ZJM,";
            sqlCmd += "LSWLZD_LSDJ,LSWLZD_PFDJ,LSWLZD_C1 AS BarCode,LSWLZD_JLDW,LSWLZD_WLXZ,LSWLZD_ZXJG,";
            sqlCmd += "LSWLZD_SFFQ,LSWLZD_ZZSL FROM LSWLZD";
            DataTable dt = new DataTable();
            if (IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref dt, sqlCmd))
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ERP计量单位参照
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string DRP_ERP_Units() 
        {
            string sqlCmd = "SELECT JSJLDW_DWDM,JSJLDW_DWMC FROM JSJLDW";
            DataTable dt = new DataTable();
            if (IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref dt, sqlCmd)) 
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回ERP订单信息及相关订单明细
        /// </summary>
        /// <param name="queryMonth">订单月份</param>
        /// <returns></returns>
        [WebMethod]
        public string DRP_ERP_Orders(string queryMonth) 
        {
            string month = string.Format("{0:yyyyMM}", Convert.ToDateTime(queryMonth));
            //string month = string.Format("{0:yyyyMM}", DateTime.Now);
            //string sqlCmd = "SELECT '02' AS DDLX,XSDD_DDLS,XSDD_DDBH,XSDD_SPKH,XSDD_DJRQ,DDZE,XSFPMX_ZE,";
            //sqlCmd += " XSDD_SPKHMC,XSDD_C9,XSDD_C10 FROM XSDD LEFT JOIN VIEW_XSDDMX_ZE ON";
            //sqlCmd += " XSDD.XSDD_DDLS=VIEW_XSDDMX_ZE.XSDDMX_DDLS LEFT JOIN VIEW_XSFPMX_ZE ON";
            //sqlCmd += " XSDD.XSDD_DDLS=VIEW_XSFPMX_ZE.XSFPMX_DDLS WHERE XSDD_DDLS IN (";
            //sqlCmd += " SELECT XSTDMX_DDLS FROM XSTD,XSTDMX ";
            //sqlCmd += " WHERE XSTD_TDLS=XSTDMX_TDLS AND XSTD_YWBH='02'";
            //sqlCmd += " AND XSTDMX_DDLS <>'@' AND XSTDMX_DDLS <>'' AND XSTD_YWRQ<='20170731'";
            //sqlCmd += " GROUP BY XSTDMX_DDLS) AND XSDD_SHBZ='1' AND XSDD_DJRQ LIKE '" + month + "%'";
            //sqlCmd += " UNION ALL";
            //sqlCmd += " SELECT '01' AS DDLX,XSDD_DDLS,XSDD_DDBH,XSDD_SPKH,XSDD_DJRQ,DDZE,XSFPMX_ZE,";
            //sqlCmd += " XSDD_SPKHMC,XSDD_C9,XSDD_C10 FROM XSDD LEFT JOIN VIEW_XSDDMX_ZE ON";
            //sqlCmd += " XSDD.XSDD_DDLS=VIEW_XSDDMX_ZE.XSDDMX_DDLS LEFT JOIN VIEW_XSFPMX_ZE ON";
            //sqlCmd += " XSDD.XSDD_DDLS=VIEW_XSFPMX_ZE.XSFPMX_DDLS  WHERE XSDD_DDLS IN (";
            //sqlCmd += " SELECT XSTDMX_DDLS FROM XSTD,XSTDMX ";
            //sqlCmd += " WHERE XSTD_TDLS=XSTDMX_TDLS AND XSTD_YWBH IN (seLECT XSTDZL_ZLBH FROM XSTDZL WHERE XSTDZL_ZLBH<>'02')";
            //sqlCmd += " AND XSTDMX_DDLS <>'@' AND XSTDMX_DDLS <>'' AND XSTD_YWRQ<='20170731'";
            //sqlCmd += " GROUP BY XSTDMX_DDLS) AND XSDD_SHBZ='1' AND XSDD_DJRQ LIKE '" + month + "%'";

            string sqlCmd = "SELECT (CASE WHEN XSTD_YWBH='02' THEN '02' ELSE '01' END) AS DDLX,";
            sqlCmd += " XSTD_TDLS,XSTD_TDBH,XSTD_YWRQ,XSTD_SPKH,XSTD_SPKHMC,XSTD_C9,XSTD_C10";
            sqlCmd += " FROM XSTD WHERE XSTD_YWRQ LIKE '" + month + "%' AND XSTD_SHBZ='1'";
            DataTable dt = new DataTable();
            IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref dt, sqlCmd);
            dt.Columns.Add("items", typeof(string));
            //DataTable itemDt = new DataTable();
            //sqlCmd = "SELECT XSDDMX_DDLS,XSDDMX_DDFL,XSDDMX_WLBH,XSDDMX_BZHSJ,XSDDMX_ZSL,ISNULL(CKSL,0) AS TDSL";
            //sqlCmd += " FROM XSDDMX LEFT JOIN ";
            //sqlCmd += " (SELECT XSTDMX_DDLS,XSTDMX_DDFL,SUM(XSTDMX_CKSL)AS CKSL";
            //sqlCmd += " FROM XSTDMX GROUP BY XSTDMX_DDLS,XSTDMX_DDFL) t";
            //sqlCmd += " ON T.XSTDMX_DDLS=XSDDMX_DDLS AND T.XSTDMX_DDFL=XSDDMX_DDFL";
            //IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref itemDt, sqlCmd);
            string jsonStr_orders = "{\"orders\":[";
            int index = 0;
            foreach (DataRow dr in dt.Rows) 
            {
                string itemStr = "[";
                DataTable tempDt = new DataTable();
                string ddls = dr["XSTD_TDLS"].ToString();
                sqlCmd = "SELECT XSTDMX_TDLS,XSTDMX_TDFL,XSTDMX_WLBH,XSTDMX_ZSL,XSTDMX_BZHSJ,XSTDMX_BHSE ";
                sqlCmd += " FROM XSTDMX WHERE XSTDMX_TDLS='" + ddls + "'";
                IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref tempDt, sqlCmd);
                //DataRow[] drArr = itemDt.Select("XSDDMX_DDLS='" + ddls + "'");
                //tempDt = itemDt.Clone();
                //for (int i = 0; i < drArr.Length; i++)
                //{
                //    tempDt.ImportRow(drArr[i]);
                //}
                int itemIndex = 0;
                foreach (DataRow itemDr in tempDt.Rows)
                {
                    itemStr += "{";
                    foreach (DataColumn itemDc in tempDt.Columns)
                    {
                        if (itemDc.ColumnName == "XSTDMX_BHSE") 
                        {
                            itemStr += "\"" + itemDc.ColumnName + "\":\"" + itemDr[itemDc].ToString().Replace("\\", "") + "\"";
                        }
                        else
                        {
                            itemStr += "\"" + itemDc.ColumnName + "\":\"" + itemDr[itemDc].ToString().Replace("\\", "") + "\",";
                        }
                    }
                    itemIndex++;
                    if (itemIndex == tempDt.Rows.Count) 
                    {
                        itemStr += "}";
                    }
                    else
                    {
                        itemStr += "},";
                    }

                }
                //itemStr.TrimEnd(',');
                itemStr += "]";
                dr["items"] = itemStr;

                jsonStr_orders += "{";
                foreach (DataColumn dc in dt.Columns) 
                {
                    if (dc.ColumnName == "items") 
                    {
                        jsonStr_orders += "\"" + dc.ColumnName + "\":" + dr[dc].ToString().Replace("\\","") + "";
                    }
                    else
                    {
                        jsonStr_orders += "\"" + dc.ColumnName + "\":\"" + dr[dc].ToString().Replace("\\", "") + "\",";
                    }
                }
                index++;
                if (index == dt.Rows.Count)
                {
                    jsonStr_orders += "}";
                }
                else
                {
                    jsonStr_orders += "},";
                }


            }
            //jsonStr_orders.TrimEnd(',');
            jsonStr_orders += "]}";

            return jsonStr_orders;
        }

        /// <summary>
        /// DRP向ERP推送客户信息
        /// </summary>
        /// <param name="ClientNo">客户编码</param>
        /// <param name="ClientName">客户名称</param>
        /// <param name="ClientType">客户类别</param>
        /// <param name="ClientTel">联系电话</param>
        /// <returns></returns>
        [WebMethod]
        public bool DRP_ERP_ClientsInfo(string ClientNo, string ClientName, string ClientType,string ClientTel) 
        {
            string areaNo = "04";//ERP地区编号默认为其他
            bool result = true;
            string sqlCmd = "SELECT * FROM ZWWLDW WHERE ZWWLDW_DWBH='" + ClientNo + "'";
            DataTable dt = new DataTable();
            //判断客户编码是否存在
            if (IETCsoft.sql.SqlSel_jsl.GetSqlSel(ref dt, sqlCmd))
            {
                //存在则更新
                sqlCmd = "UPDATE ZWWLDW SET ZWWLDW_DWMC,ZWWLDW_TELE,ZWWLDW_LBBH WHERE ZWWLDW_DWBH='" + ClientNo + "'";
                if (IETCsoft.sql.SqlSel_jsl.ExeSql(sqlCmd) <= 0) 
                {
                    result = false;
                }
            }
            else 
            {
                //不存在则插入一条
                sqlCmd = "INSERT INTO ZWWLDW (ZWWLDW_DWBH,ZWWLDW_DWMC,ZWWLDW_DQBH,ZWWLDW_LBBH,ZWWLDW_TELE) VALUES";
                sqlCmd += "('" + ClientNo + "','" + ClientName + "','" + areaNo + "','" + ClientType + "','" + ClientTel + "')";
                if (IETCsoft.sql.SqlSel_jsl.ExeSql(sqlCmd) <= 0)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
