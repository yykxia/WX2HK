using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WX2HK
{
    public class PhoneMsg
    {
        public static string appId = System.Configuration.ConfigurationManager.AppSettings["Msg_appId"];//用户序列号
        public static string secretKey = System.Configuration.ConfigurationManager.AppSettings["Msg_secretKey"];//用户密码
        public static string host = System.Configuration.ConfigurationManager.AppSettings["Msg_url1"];//接口地址（上海）

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="phoneNumbList"></param>
        /// <param name="MsgContent"></param>
        /// <returns></returns>
        public static string sendBatchOnlySMS(string phoneNumbList, string MsgContent) 
        {
            try
            {
                //解析按','隔开的手机号
                string[] phoneNumb = phoneNumbList.Split(',');

                //List<SMSEntity> SMSlist = new List<SMSEntity>();
                string result = "";
                Hashtable headerhs = new Hashtable();
                Byte[] byteArray = null;
                string jsondata = "";
                string url = "http://" + host + "/inter/sendBatchOnlySMS";

                headerhs.Add("appId", appId);

                jsondata = " {\"mobiles\":[";
                foreach (string mobile in phoneNumb)
                {
                    jsondata += "\"" + mobile + "\",";
                }

                jsondata = jsondata.Substring(0, jsondata.Length - 1);//去除尾部逗号
                jsondata += "],\"content\":\"【梦百合】" + MsgContent
                    + "\",\"timerTime\":\"\",\"extendedCode\":\"112\",\"requestTime\":"
                    + DateTime.Now.Ticks.ToString() + ",\"requestValidPeriod\":30}";

                byteArray = HttpHelper.postdata(url, AESHelper.AESEncrypt(jsondata, secretKey), headerhs, Encoding.UTF8, secretKey);


                if (byteArray != null)
                {
                    result = AESHelper.AESDecryptString(byteArray, secretKey);

                    //if (result != "")
                    //{
                    //    if (result.IndexOf("ERROR") == -1)
                    //    {
                    //        result = result.Replace('\"', '"');
                    //        Newtonsoft.Json.Linq.JArray Jarray = new JArray();
                    //        Jarray = (JArray)JsonConvert.DeserializeObject(result);
                    //        if (Jarray != null)
                    //        {
                    //            foreach (JObject j in Jarray)
                    //            {
                    //                SMSEntity SMS = new SMSEntity();
                    //                SMS.customSmsId = j["customSmsId"].ToString().Replace("\"", "");
                    //                SMS.mobile = j["mobile"].ToString().Replace("\"", "");
                    //                SMS.smsId = j["smsId"].ToString().Replace("\"", "");
                    //                SMSlist.Add(SMS);//返回状态报告对象结果集
                    //            }
                    //        }
                    //    }

                    //}

                    return result;

                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// 单条发送短信
        /// </summary>
        /// <param name="pohneNumb">手机号</param>
        /// <param name="MsgContent">短信内容</param>
        /// <returns></returns>
        public static string sendSingleSMS(string pohneNumb, string MsgContent) 
        {
            //List<SMSEntity> SMSlist = new List<SMSEntity>();
            string result = "";
            Hashtable headerhs = new Hashtable();
            Byte[] byteArray = null;
            string jsondata = "";
            string url = "http://" + host + "/inter/sendSingleSMS";

            headerhs.Add("appId", appId);
            jsondata = "{\"mobile\":\"" + pohneNumb + "\",\"content\":\"【梦百合】" + MsgContent
                + "\",\"timerTime\":\"\",\"extendedCode\":\"112\",\"requestTime\":"
                + DateTime.Now.Ticks.ToString() + ",\"requestValidPeriod\":30}";

            byteArray = HttpHelper.postdata(url, AESHelper.AESEncrypt(jsondata, secretKey), headerhs, Encoding.UTF8, secretKey);

            if (byteArray != null)
            {
                result = AESHelper.AESDecryptString(byteArray, secretKey);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="phoneNumb"></param>
        /// <returns>返回验证码</returns>
        public static string VerCode(string phoneNumb)
        {
            string vc = string.Empty;

            //短期内重复获取
            System.Data.DataTable dt = new System.Data.DataTable();
            string sqlCmd = "select * from Msg_Verify where PhoneNumb='" + phoneNumb + "'";
            sqlCmd += " and IsValidated='0'";
            IETCsoft.sql.SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                DateTime curTime = DateTime.Now;
                DateTime InvalidTime = Convert.ToDateTime(dt.Rows[0]["InvalidTime"]);
                TimeSpan ts = curTime - InvalidTime;
                double comMns = ts.TotalMinutes;//时间差计算为分钟
                if (comMns > 3)
                {
                    //原有验证码状态设为失效
                    sqlCmd = "update Msg_Verify set IsValidated='1' where PhoneNumb='" + phoneNumb + "'";
                    IETCsoft.sql.SqlSel.ExeSql(sqlCmd);

                    //重新生成验证码并存储
                    vc = GetRadomStr();//生成四位随机数
                    string MsgContent = "验证码：" + vc + "，3分钟内输入有效。";
                    string result = sendSingleSMS(phoneNumb, MsgContent);//发送验证码短信
                    if (result.IndexOf("ERROR") == -1)//成功发送则记录验证码待用户使用
                    {
                        sqlCmd = "insert into Msg_Verify (PhoneNumb,VerifyCode,InvalidTime,IsValidated) values";
                        sqlCmd += "('" + phoneNumb + "','" + vc + "','" + DateTime.Now + "','0')";
                        IETCsoft.sql.SqlSel.ExeSql(sqlCmd);
                    }
                    else
                    {
                        vc = null;
                    }
                }
                else//三分钟以内不重复获取
                {
                    vc = dt.Rows[0]["VerifyCode"].ToString();
                    string MsgContent = "验证码：" + vc + "，3分钟内输入有效。";
                    string result = sendSingleSMS(phoneNumb, MsgContent);
                    if (result.IndexOf("ERROR") != -1)
                    {
                        vc = null;
                    }
                }
            }
            else 
            {
                vc = GetRadomStr();//生成四位随机数
                string MsgContent = "验证码：" + vc + "，3分钟内输入有效。";
                string result = sendSingleSMS(phoneNumb, MsgContent);
                if (result.IndexOf("ERROR") == -1)
                {
                    sqlCmd = "insert into Msg_Verify (PhoneNumb,VerifyCode,InvalidTime,IsValidated) values";
                    sqlCmd += "('" + phoneNumb + "','" + vc + "','" + DateTime.Now + "','0')";
                    IETCsoft.sql.SqlSel.ExeSql(sqlCmd);
                }
                else
                {
                    vc = null;
                }
            }

            return vc;
        }

        private static string GetRadomStr()
        {
            string vc = string.Empty;
            Random rd = new Random(Guid.NewGuid().GetHashCode());//随机生成类
            int num1 = rd.Next(1, 9);//返回指定范围内的随机数
            int num2 = rd.Next(0, 9);
            int num3 = rd.Next(0, 9);
            int num4 = rd.Next(0, 9);

            int[] nums = new int[4] { num1, num2, num3, num4 };
            for (int i = 0; i < nums.Length; i++)//循环添加四个随机生成数
            {
                vc += nums[i].ToString();
            }
            return vc;
        }

        /// <summary>
        /// 短信验证码是否有效
        /// </summary>
        /// <param name="phoneNumb"></param>
        /// <param name="verfCode"></param>
        /// <returns></returns>
        public static bool isValidCode(string phoneNumb, string verfCode) 
        {
            string sqlCmd = "select * from Msg_Verify where PhoneNumb='" + phoneNumb + "'";
            sqlCmd += " and VerifyCode='" + verfCode + "' and IsValidated='0'";
            System.Data.DataTable dt = new System.Data.DataTable();
            IETCsoft.sql.SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                DateTime curTime = DateTime.Now;
                DateTime InvalidTime = Convert.ToDateTime(dt.Rows[0]["InvalidTime"]);
                TimeSpan ts = curTime - InvalidTime;
                double comMns = ts.TotalMinutes;//时间差计算为分钟
                if (comMns > 3)//3分钟内有效
                {
                    return false;
                }
                else 
                {
                    return true;
                }
            }
            else 
            {
                return false;
            }
        }
    }
}