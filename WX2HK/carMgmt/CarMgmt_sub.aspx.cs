using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WX2HK.carMgmt
{
    public partial class CarMgmt_sub : System.Web.UI.Page
    {
        public string car_TimeStamp = "";
        public string car_Nonce = "";
        public string car_MsgSig = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //
                string code = string.Empty;
                code = HttpContext.Current.Request.QueryString["Code"];
                //
                car_TimeStamp = WX2HK.ReturnInfo.GetTimeStamp();
                car_Nonce = WX2HK.ReturnInfo.randNonce();
                string curUrl = string.Format("http://hkoa.hkfoam.com:30018/carMgmt/CarMgmt_sub.aspx?code={0}&state=STATE", code);
                car_MsgSig = WX2HK.VerifyLegal.createSign(car_Nonce, car_TimeStamp, curUrl);
            }
        }
    }
}