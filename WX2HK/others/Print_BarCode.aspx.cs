using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WX2HK.others
{
    public partial class Print_BarCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                txb_date.Text = DateTime.Now.ToString("yyyyMMdd");
            }
        }

        protected void btn_build_Click(object sender, EventArgs e)
        {
            try
            {
                int printWeight = 1;//条码厚重度
                int minCode = Convert.ToInt32(txb_code1.Text);
                int maxCode = Convert.ToInt32(txb_code2.Text);
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("ImgUrl");
                dt.Columns.Add("ImgUrl1");
                //dt.Columns.Add("ImgUrl2");
                //dt.Columns.Add("ImgUrl3");
                int rowsCount = (maxCode - minCode) / 2 + 1;
                for (int i = 0; i < rowsCount; i++) 
                {
                    System.Data.DataRow dr = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++) 
                    {
                        if ((minCode + 2 * i + j) > maxCode)
                        {
                            break;
                        }
                        else
                        {
                            dr[j] = "../PLM/code128.aspx?printWeight=" + printWeight + "&num=" + txb_date.Text + (minCode + 2 * i + j).ToString("000");
                        }
                    }
                    dt.Rows.Add(dr);
                }
                //for (int i = minCode; i <= maxCode; i++) 
                //{
                //    System.Data.DataRow dr = dt.NewRow();
                //    dr["ImgUrl"] = "code128.aspx?num=" + i;
                //    dt.Rows.Add(dr);
                //}
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch 
            {
            }
        }


    }
}