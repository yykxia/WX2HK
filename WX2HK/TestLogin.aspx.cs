using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WX2HK
{
    public partial class TestLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            int curNumb = Convert.ToInt32(Label2.Text);
        }

        protected void lkb_SendVerfCode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxUserPhoneNumb.Text))
            {
                if (PhoneMsg.VerCode(tbxUserPhoneNumb.Text) != null) 
                {

                }
                
            }
        }
    }
}