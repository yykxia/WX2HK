using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;
using System.Security.Cryptography;

namespace WX2HK
{
    public class VerifyLegal
    {

        enum WXBizMsgCryptErrorCode
        {
            WXBizMsgCrypt_OK = 0,
            WXBizMsgCrypt_ValidateSignature_Error = -40001,
            WXBizMsgCrypt_ParseXml_Error = -40002,
            WXBizMsgCrypt_ComputeSignature_Error = -40003,
            WXBizMsgCrypt_IllegalAesKey = -40004,
            WXBizMsgCrypt_ValidateCorpid_Error = -40005,
            WXBizMsgCrypt_EncryptAES_Error = -40006,
            WXBizMsgCrypt_DecryptAES_Error = -40007,
            WXBizMsgCrypt_IllegalBuffer = -40008,
            WXBizMsgCrypt_EncodeBase64_Error = -40009,
            WXBizMsgCrypt_DecodeBase64_Error = -40010
        };

        //public static string GetAccess_Token(string Corpid, string Corpsecret) 
        //{
        //    string sUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", Corpid, Corpsecret);
        //    var client = new System.Net.WebClient();
        //    client.Encoding = System.Text.Encoding.UTF8;
        //    var data = client.DownloadString(sUrl);
        //    //HttpContext.Current.Response.Write(data);
        //    var serializer = new JavaScriptSerializer();
        //    var obj = serializer.Deserialize<Dictionary<string, string>>(data);
        //    string accessToken;
        //    obj.TryGetValue("access_token", out accessToken);
        //    return accessToken;

        //}

        public static string GetAccess_Token()
        {
            string Corpid = "wx3068893beff3a241";
            string Corpsecret = "CZo3q6rQB2b6zqtJGPREqI4COXUX0JtbPSjIo6MvdXTLhmZP8o_5BFMUUu_B_7KW";
            string sUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", Corpid, Corpsecret);
            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var data = client.DownloadString(sUrl);
            //HttpContext.Current.Response.Write(data);
            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<Dictionary<string, string>>(data);
            string accessToken;
            obj.TryGetValue("access_token", out accessToken);
            return accessToken;

        }


        //生成签名
        public static string createSign(string randString, string timestamp, string url) 
        {
            //获取access_token
            string token = GetAccess_Token();
            //根据token获取jsapi
            string apiUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", token);
            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var data = client.DownloadString(apiUrl);
            //HttpContext.Current.Response.Write(data);
            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<Dictionary<string, string>>(data);
            string jsApiTicket;
            obj.TryGetValue("ticket", out jsApiTicket);

            String string1 = "";
            String signature = "";
            string1 = "jsapi_ticket=" + jsApiTicket +
                          "&noncestr=" + randString +
                          "&timestamp=" + timestamp +
                          "&url=" + url;
            signature = GetSha1(string1);
            return signature;
        }

        public static string GetSha1(string str)
        {

            //建立SHA1对象

            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]

            ASCIIEncoding enc = new ASCIIEncoding();

            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算

            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string

            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;

        }

        public static string SHA1Sign(string data)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(data);

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] temp2 = sha.ComputeHash(temp1);
            sha.Clear();

            // 注意， 不能用这个
            //string output = Convert.ToBase64String(temp2);

            string output = BitConverter.ToString(temp2);
            output = output.Replace("-", "");
            output = output.ToLower();
            return output;
        }
    }
}