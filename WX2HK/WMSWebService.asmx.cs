using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Data;
using IETCsoft.sql;

namespace WX2HK
{
    /// <summary>
    /// WMSWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WMSWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// WMS获取物料信息
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <returns></returns>
        [WebMethod]
        public string WMS_Materials(string MaterialNo)
        {
            Ent.WMS_GetMaterials wMS_Get = new Ent.WMS_GetMaterials();
            string sqlCmd = "select * from T_Interface_Material where Code='" + MaterialNo + "'";
            DataTable QueryDt = new DataTable();
            if (SqlSel_WMS.GetSqlSel(ref QueryDt, sqlCmd))
            {
                wMS_Get.Result = 0;
            }
            else
            {
                sqlCmd = "select LSWLEX_C50,LSWLZD_WLMC,LSWLZD_GGXH,JSJLDW_DWMC,LSWLZD_WLBH " +
                    "from LSWLZD left join lswlex on LSWLZD_WLBH=lswlex_WLBH" +
                    " left join JSJLDW on JSJLDW_DWDM=LSWLZD_JLDW where LSWLZD_WLBH='" + MaterialNo + "'";
                DataTable dt = new DataTable();
                if (SqlSel_ERP.GetSqlSel(ref dt, sqlCmd))
                {
                    if (dt.Rows.Count > 1)
                    {
                        wMS_Get.Result = 0;
                    }
                    else
                    {
                        Ent.Material sWLZD = new Ent.Material
                        {
                            Name = dt.Rows[0]["LSWLZD_WLMC"].ToString(),
                            No = dt.Rows[0]["LSWLZD_WLBH"].ToString(),
                            Type = dt.Rows[0]["LSWLEX_C50"].ToString(),
                            Remark = dt.Rows[0]["LSWLZD_GGXH"].ToString(),
                            Unit = dt.Rows[0]["JSJLDW_DWMC"].ToString()
                        };
                        //插入接口中间数据库
                        sqlCmd = "INSERT INTO [T_Interface_Material]" +
                            " ([Oid],[Name],[Code],[Remark],[MaterialType],[Packspec],[Unit],[Remark1],[Remark2]) VALUES" +
                            " (newid(),'" + sWLZD.Name + "','" + sWLZD.No + "','" + sWLZD.Remark + "'," +
                            "'" + sWLZD.Type + "','','" + sWLZD.Unit + "','','')";
                        if (SqlSel_WMS.ExeSql(sqlCmd) > 0)
                        {
                            wMS_Get.Result = 1;
                            wMS_Get.Material = sWLZD;
                        }
                        else
                        {
                            wMS_Get.Result = 0;
                        }
                    }
                }
                else
                {
                    wMS_Get.Result = 0;
                }
            }
            return JsonConvert.SerializeObject(wMS_Get);
        }

        /// <summary>
        /// ERP物料类别
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string WMS_MaterialType()
        {
            string sqlCmd = "select WLLBEX_LBBH,WLLBEX_LBMC,WLLBEX_SJLB from T_WLLBEX";
            DataTable data = new DataTable();
            if (SqlSel_ERP.GetSqlSel(ref data, sqlCmd))
            {
                List<Ent.MaterialType> list = new List<Ent.MaterialType>();
                foreach (DataRow dr in data.Rows)
                {
                    list.Add(new Ent.MaterialType {
                        TypeNo =dr["WLLBEX_LBBH"].ToString(),
                        TypeName = dr["WLLBEX_LBMC"].ToString(),
                        ParentType = dr["WLLBEX_SJLB"].ToString()
                    });
                }

                return JsonConvert.SerializeObject(list);
            }
            else
            {
                return null;
            }

        }
    }
}
