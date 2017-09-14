using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;

namespace WX2HK
{
    /// <summary>
    /// MsgAPI 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MsgAPI : System.Web.Services.WebService
    {

        /// <summary>
        /// 调用批量短信发送接口
        /// </summary>
        /// <param name="mobiles">手机号列表，以','拼接</param>
        /// <param name="MsgContent">短信内容</param>
        /// <returns>返回发送状态码</returns>
        [WebMethod]
        public string SendMsg_Batch(string mobiles, string MsgContent) 
        {
            string result = PhoneMsg.sendBatchOnlySMS(mobiles, MsgContent);

            return result;
        }

        /// <summary>
        /// 调用单条发送短信接口
        /// </summary>
        /// <param name="phoneNumb">单个手机号</param>
        /// <param name="MsgContent">短信内容</param>
        /// <returns>返回发送状态码</returns>
        [WebMethod]
        public string SendMsg_Single(string phoneNumb, string MsgContent) 
        {
            string result = PhoneMsg.sendSingleSMS(phoneNumb, MsgContent);
            return result;
        }

        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="phoneNumb">手机号</param>
        /// <returns>系统生成的短信验证码</returns>
        [WebMethod]
        public string SendMsg_VerfCode(string phoneNumb) 
        {
            string result = PhoneMsg.VerCode(phoneNumb);
            return result;
        }

        /// <summary>
        /// 短信验证码是否有效
        /// </summary>
        /// <param name="phoneNumb">手机号</param>
        /// <param name="verfCode">用户输入的验证码</param>
        /// <returns>true:false</returns>
        [WebMethod]
        public bool SendMsg_IsValidVerfCode(string phoneNumb, string verfCode) 
        {
            bool result = PhoneMsg.isValidCode(phoneNumb, verfCode);

            return result;
        }

        /// <summary>
        /// 微信推送接口
        /// </summary>
        /// <param name="workerNo">员工工号1|员工工号2</param>
        /// <param name="redicUrl">跳转的链接</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <returns>
        /// {
            ///"errcode" : 0,
            ///"errmsg" : "ok",
            ///"invaliduser" : "UserID1", // 不区分大小写，返回的列表都统一转为小写
            ///"invalidparty" : "PartyID1",
            ///"invalidtag":"TagID1"
        ///}
        ///</returns>
        [WebMethod]
        public string Wechat_pushMsg(string workerNo, string redicUrl, string title, string desc) 
        {
            string[] gh = workerNo.Split('|');
            DataTable dt = new DataTable();
            //从协同平台获取用户信息
            string sqlStr = "select * from oa_tbl_employee";
            IETCsoft.sql.SqlSel_testDB.GetSqlSel(ref dt, sqlStr);
            StringBuilder sb = new StringBuilder();
            foreach (string it in gh)
            {
                if (it == "")
                {
                    continue;
                }
                DataRow[] drArr = dt.Select("loginId='" + it + "'");
                if (drArr.Length > 0)
                {
                    sb.AppendFormat("{0}|", drArr[0]["EmpID"].ToString());
                }
                else 
                {
                    continue;
                }
            }

            string result = ReturnInfo.wx_pushMsg(sb.ToString(), "9", title, redicUrl, desc, "");

            return result;

        }
    }
}
