using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WX2HK.PLM
{
    public partial class PLM_WH_StorageCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_build_Click(object sender, EventArgs e)
        {
            try
            {
                string str = ddl_area.SelectedValue + ddl_middleArea.SelectedValue;
                int minCode = Convert.ToInt32(txb_code1.Text);
                int maxCode = Convert.ToInt32(txb_code2.Text);
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("ImgUrl");
                int rowsCount = (maxCode - minCode) + 1;
                for (int i = 0; i < rowsCount; i++)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["ImgUrl"] = "Code128_WH.aspx?num=" + str + (minCode + i).ToString();
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

        protected void btn_special_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ImgUrl");
            System.Data.DataRow dr = dt.NewRow();
            dr["ImgUrl"] = "Code128_WH.aspx?num=" + txb_special.Text;
            dt.Rows.Add(dr);
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

    }
}