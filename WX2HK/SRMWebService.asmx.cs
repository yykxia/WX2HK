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
        public void getRemainSendList(string supplierNo) 
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
                    item.contractNo = dt.Rows[i]["GYSCGDD_SJDH"].ToString();
                    item.orderNo = dt.Rows[i]["GYSCGDD_C1"].ToString();
                    item.materialNo = dt.Rows[i]["GYSCGDD_WLBH"].ToString();
                    item.materialName = dt.Rows[i]["wlmc"].ToString();
                    item.contactCount = Convert.ToDouble(dt.Rows[i]["GYSCGDD_SL"]);
                    item.curCount = Convert.ToDouble(dt.Rows[i]["curTotal"]);
                    item.deliveryDate = dt.Rows[i]["JQBG_BGRQ"].ToString();
                    items.Add(item);
                }
                remaind.Items = items.ToArray();
                Context.Response.ContentType = "text/xml; charset=utf-8";
                Context.Response.Write(JsonConvert.SerializeObject(remaind));
                Context.Response.End();
                //return JsonConvert.SerializeObject(remaind);
            }
        }

        /// <summary>
        /// 人事招聘信息接收
        /// </summary>
        /// <param name="DataJson"></param>
        /// <returns></returns>
        [WebMethod]
        public void HR_ReceiveApply(string DataJson)
        {
            PostResult Result = new PostResult()
            {
                Success = 0,
                ErrorMsg = ""
            };
            //求职信息json字符串转换
            ApplyInfo ApplyInfo = JsonConvert.DeserializeObject<ApplyInfo>(DataJson);
            string sqlCmd = "insert into HR_ApplyInfo (ApplyName,PhoneNumber,ApplyPosition,ApplyTime,Others)";
            sqlCmd += "values ('" + ApplyInfo.ApplyName + "','" + ApplyInfo.PhoneNumber + "','" + ApplyInfo.ApplyPosition + "',";
            sqlCmd += "getdate(),'" + ApplyInfo.Others + "')";
            if (IETCsoft.sql.SqlSel.ExeSql(sqlCmd) > 0)
            {
                Result.Success = 1;
            }
            else
            {
                Result.ErrorMsg = "信息提交失败！";
            }

            Context.Response.ContentType = "text/json; charset=utf-8";
            Context.Response.Write(JsonConvert.SerializeObject(Result));
            Context.Response.End();

        }
        /// <summary>
        /// 返回值信息
        /// </summary>
        public class PostResult 
        {
            public int Success { get; set; }
            public string ErrorMsg { get; set; }
        }
    }
}
