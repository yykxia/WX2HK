using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WX2HK;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using FineUI;

namespace WX2HK
{
    public class ReturnInfo
    {
        //获取微信企业号人员通讯id
        public static string GetUserId(string code,string pageName)
        {
            try
            {
                string sCorpid = "wx3068893beff3a241";
                //string sSecret = "CZo3q6rQB2b6zqtJGPREqI4COXUX0JtbPSjIo6MvdXTLhmZP8o_5BFMUUu_B_7KW";

                string userid = "";
                string access_token = VerifyLegal.GetAccess_Token();
                if (string.IsNullOrEmpty(code))
                {
                    var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=http%3a%2f%2fhkoa.hkfoam.com%3a30018%2f{1}.aspx&response_type=code&scope=SCOPE&state=STATE#wechat_redirect", sCorpid, pageName);
                    HttpContext.Current.Response.Redirect(url);
                }
                else
                {
                    var client = new System.Net.WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;

                    var urlStr = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", access_token, code);
                    var data = client.DownloadString(urlStr);

                    var serializer = new JavaScriptSerializer();
                    var obj = serializer.Deserialize<Dictionary<string, string>>(data);


                    if (!obj.TryGetValue("UserId", out userid))
                    {
                        obj.TryGetValue("OpenId", out userid);
                    }
                }

                return userid;
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(string.Format("获取userID出错：{0}", ex.ToString()));
                return null;
            }
        }

        //通过企业号员工Id获取人员手机信息
        public static string GetUserInfo(string userid) 
        {
            try
            {
                string access_token = VerifyLegal.GetAccess_Token();
                var client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}", access_token, userid);
                var data = client.DownloadString(url);
                var serializer = new JavaScriptSerializer();
                var obj = serializer.Deserialize<Dictionary<string, object>>(data);
                string phoneNum = obj["mobile"].ToString();
                return phoneNum;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(string.Format("获取微信手机号信息错误：{0}", ex.Message));
                return null;
            }
        }


        //返回企业内部员工工号
        public static string getWorkNo(string userId)
        {
            try
            {
                string phoneNum = "";
                string workno = string.Empty;
                //userId = GetUserId(code, pageName);
                //获取当前人员手机号
                phoneNum = GetUserInfo(userId);
                //从本地服务器获取人员工号
                if (!string.IsNullOrEmpty(phoneNum))
                {
                    string sqlCmd = "select name from x_user where cellphone='" + phoneNum + "'";
                    workno = IETCsoft.sql.SqlSel.GetSqlScale(sqlCmd).ToString();
                }
                return workno;
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(string.Format("获取工号错误：{0}",ex.ToString()));
                return null;
            }
            //System.Web.HttpContext.Current.Session["loginUser"] = workno;
        } 

        //返回办公平台登录工号
        public static string getLoginId(string userId) 
        {
            try
            {
                string workno = string.Empty;
                //从本地服务器获取人员工号
                if (!string.IsNullOrEmpty(userId))
                {
                    string sqlCmd = "select loginId from oa_tbl_employee where empID='" + userId + "'";
                    workno = IETCsoft.sql.SqlSel_testDB.GetSqlScale(sqlCmd).ToString();
                }
                return workno;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(string.Format("获取工号错误：{0}", ex.ToString()));
                return null;
            }
        }

        //执行推送
        public static void pushMessage(string userList, string msgTitle, string redicUrl, string msgDesc, string picUrl)
        {
            try
            {
                //news类型json格式
                String jsonContext = "{" +
                    "\"touser\":\"" + userList + "\"," +
                    "\"msgtype\":\"news\"," +
                    "\"agentid\":9," +
                    "\"news\":" +
                    "{" +
                        "\"articles\":[" +
                        "{" +
                           "\"title\": \"" + msgTitle + "\"," +
                           "\"description\": \"" + msgDesc + "\"," +
                           "\"url\": \"" + redicUrl + "\"," +
                           "\"picurl\": \"" + picUrl + "\"" +
                        "}" +
                        "]" +
                    "}" +
                "}";
                //获取accessToken
                string access_token = VerifyLegal.GetAccess_Token();
                string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", access_token);
                PostWebRequest(url, jsonContext, Encoding.UTF8);
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(string.Format("pushErr:{0}", ex.Message));
            }
        }

        //根据手机号推送消息
        public static void messagePushByNum(string PhoneNumList,string msgTitle,string redicUrl,string msgDesc,string picUrl)
        {
            try
            {
                if (PhoneNumList != "@all")
                {
                    StringBuilder sb = new StringBuilder();
                    String[] phoneNum = PhoneNumList.Split(';');
                    DataTable curDt = new DataTable();
                    curDt = WX2HK.selectUser.getUserList();
                    //Alert.Show(curDt.Rows.Count.ToString());
                    foreach (string it in phoneNum)
                    {
                        if (it == "")
                        {
                            break;
                        }
                        DataRow[] drArr = curDt.Select("mobile='" + it + "'");
                        DataTable dtNew = curDt.Clone();
                        dtNew.ImportRow(drArr[0]);
                        sb.AppendFormat("{0}|", dtNew.Rows[0]["userid"].ToString());
                    }
                    pushMessage(sb.ToString(), msgTitle, redicUrl, msgDesc, picUrl);
                }
                else
                {
                    pushMessage(PhoneNumList, msgTitle, redicUrl, msgDesc, picUrl);
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(string.Format("getUserIdErr:{0}", ex.Message));
            }
        }


        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }


        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds).ToString();
        }

        /// <summary>  
        /// 返回一个八位的随机号，用于签名  
        /// </summary>  
        /// <returns></returns>  
        public static string randNonce()
        {
            var result = "";
            var random = new Random((int)DateTime.Now.Ticks);
            const string textArray = "123456789abcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 8; i++)
            {
                result = result + textArray.Substring(random.Next() % textArray.Length, 1);
            }

            return result;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userList">UserID1|UserID2|UserID3</param>
        /// <param name="agentId">企业应用的id，整型</param>
        /// <param name="msgTitle">标题</param>
        /// <param name="redicUrl">点击后跳转的链接</param>
        /// <param name="msgDesc">描述</param>
        /// <param name="picUrl">图文消息的图片链接</param>
        /// <returns>
        /// {
            ///"errcode" : 0,
            ///"errmsg" : "ok",
            ///"invaliduser" : "UserID1", // 不区分大小写，返回的列表都统一转为小写
            ///"invalidparty" : "PartyID1",
            ///"invalidtag":"TagID1"
        ///}
        ///</returns>
        public static string wx_pushMsg(string userList, string agentId, 
            string msgTitle, string redicUrl, string msgDesc, string picUrl)
        {
            try
            {
                //news类型json格式
                String jsonContext = "{" +
                    "\"touser\":\"" + userList + "\"," +
                    "\"msgtype\":\"news\"," +
                    "\"agentid\":" + agentId + "," +
                    "\"news\":" +
                    "{" +
                        "\"articles\":[" +
                        "{" +
                           "\"title\": \"" + msgTitle + "\"," +
                           "\"description\": \"" + msgDesc + "\"," +
                           "\"url\": \"" + redicUrl + "\"," +
                           "\"picurl\": \"" + picUrl + "\"" +
                        "}" +
                        "]" +
                    "}" +
                "}";
                //获取accessToken
                string access_token = VerifyLegal.GetAccess_Token();
                string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", access_token);
                string result = PostWebRequest(url, jsonContext, Encoding.UTF8);
                return result;
            }
            catch
            {
                return null;
            }
        }

    }
}