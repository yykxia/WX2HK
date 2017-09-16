using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Newtonsoft.Json;

namespace WX2HK
{
    /// <summary>
    /// SRMWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SRMWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取SRM供应商未送货或未送货完成的信息
        /// </summary>
        /// <param name="supplierNo">供应商编码</param>
        /// <returns></returns>
        [WebMethod]
        public string getRemainSendList(string supplierNo) 
        {
            string sqlCmd = "SELECT * FROM VIEW_SRM_RemainSend where GYSCGDD_DDJSF = '" + supplierNo + "'";
            DataTable dt = new DataTable();
            if (IETCsoft.sql.SqlSel_SRM.GetSqlSel(ref dt, sqlCmd))
            {
                Ent.SRM.RemainSend remaind = new Ent.SRM.RemainSend();//实例化一个供应商送货信息
                remaind.supplierNo = supplierNo;
                remaind.remainCounts = dt.Rows.Count;
                List<Ent.SRM.Item> items = new List<Ent.SRM.Item>();//采购订单明细
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    Ent.SRM.Item item = new Ent.SRM.Item();
                    item.contactNo = dt.Rows[i]["GYSCGDD_SJDH"].ToString();
                    item.orderNo = dt.Rows[i]["GYSCGDD_C1"].ToString();
                    item.materialNo = dt.Rows[i]["GYSCGDD_WLBH"].ToString();
                    item.materialName = dt.Rows[i]["wlmc"].ToString();
                    item.contactCount = Convert.ToDouble(dt.Rows[i]["GYSCGDD_SL"]);
                    item.curCount = Convert.ToDouble(dt.Rows[i]["curTotal"]);
                    item.deliveryDate = dt.Rows[i]["curTotal"].ToString();
                    items.Add(item);
                }
                remaind.Items = items.ToArray();
                return JsonConvert.SerializeObject(remaind);

            }
            else 
            {
                return null;
            }
        }
    }
}
