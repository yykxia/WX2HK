using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent;

namespace AppBox
{
    public partial class wxReturnBack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
                {
                    WxUrlVerify();
                }
            }
        }


        public void WxUrlVerify()
        {
            string sToken = "iQtIwTbylISk";
            string sEncodingAESKey = "yqwmQGX8gd3p6cEaeYA9xsPXm2e3VMjnuZzQOv3YvF1";
            string sCorpID = "wx3068893beff3a241";
            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
            //获取企业号将发送GET请求到填写的URL上携带的四个参数（微信加密签名，时间戳，随机数，加密的随机字符串）
            string sVerifyMsgSig = HttpContext.Current.Request.QueryString["msg_signature"];
            string sVerifyTimeStamp = HttpContext.Current.Request.QueryString["timestamp"];
            string sVerifyNonce = HttpContext.Current.Request.QueryString["nonce"];
            string sVerifyEchoStr = HttpContext.Current.Request.QueryString["echostr"];
            string sEchoStr = "";

            if (CheckSignature(sToken, sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sCorpID, sEncodingAESKey, sVerifyEchoStr, ref sEchoStr))
            {
                if (!string.IsNullOrEmpty(sEchoStr))
                {
                    HttpContext.Current.Response.Write(sEchoStr);
                    HttpContext.Current.Response.End();
                }
            }
            //int ret = 0;
            //ret = wxcpt.VerifyURL(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sVerifyEchoStr, ref sEchoStr);
            //if (ret != 0)
            //{
            //    System.Console.WriteLine("ERR: VerifyURL fail, ret: " + ret);
            //    return;
            //}
            //else 
            //{
            //    context.Response.Write(sVerifyEchoStr);
            //}
        }

        /// <summary>
        /// 验证企业号签名
        /// </summary>
        /// <param name="token">企业号配置的Token</param>
        /// <param name="signature">签名内容</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">nonce参数</param>
        /// <param name="corpId">企业号ID标识</param>
        /// <param name="encodingAESKey">加密键</param>
        /// <param name="echostr">内容字符串</param>
        /// <param name="retEchostr">返回的字符串</param>
        /// <returns></returns>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce, string corpId, string encodingAESKey, string echostr, ref string retEchostr)
        {
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, encodingAESKey, corpId);
            int result = wxcpt.VerifyURL(signature, timestamp, nonce, echostr, ref retEchostr);
            if (result != 0)
            {
                //LogTextHelper.Error("ERR: VerifyURL fail, ret: " + result);
                return false;
            }

            return true;

            //ret==0表示验证成功，retEchostr参数表示明文，用户需要将retEchostr作为get请求的返回参数，返回给企业号。
            // HttpUtils.SetResponse(retEchostr);
        }
    }
}