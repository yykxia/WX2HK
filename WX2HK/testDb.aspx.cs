using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;

namespace WX2HK
{
    public partial class testDb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sqlStr = "select * from oa_tbl_employee where empID=282";
                DataTable dt = new DataTable();
                SqlSel_testDB.GetSqlSel(ref dt, sqlStr);
                Console.Write(dt.Rows[0]["loginId"].ToString());
            }
        }
    }
}