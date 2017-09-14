using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using IETCsoft.sql;
using FineUI;

namespace WX2HK.someDict
{
    public partial class MoldCoding_TypeEdit : System.Web.UI.Page
    {
        private static string recvId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                recvId = Request["id"];

                loadData();
            }
        }

        //加载数据
        private void loadData() 
        {
            if (!string.IsNullOrEmpty(recvId)) 
            {
                string sqlCmd = "select * from Dict_MJCoding_TopType where id='" + recvId + "'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                txb_MJTypeName.Text = dt.Rows[0]["MJTypeName"].ToString();
                txb_MJTypeCode.Text = dt.Rows[0]["MJTypeCode"].ToString();
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string sqlCmd = "";
            if (!string.IsNullOrEmpty(recvId))
            {
                sqlCmd = "update Dict_MJCoding_TopType set MJTypeName='" + txb_MJTypeName.Text + "',MJTypeCode='" + txb_MJTypeCode.Text + "' where id='" + recvId + "'";
            }
            else 
            {
                sqlCmd = "insert into Dict_MJCoding_TopType (MJTypeName,MJTypeCode) values ('" + txb_MJTypeName.Text + "','" + txb_MJTypeCode.Text + "')";
            }
            SqlSel.ExeSql(sqlCmd);
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}