using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WX2HK.admin
{
    public partial class MenuMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid();
                btn_addNew.OnClientClick = Window1.GetShowReference("MenuEdit.aspx");
            }

        }

        private void bindGrid() 
        {
            string sqlCmd = "select b.title as parentTitle,a.title,a.ImageUrl,a.NavigateUrl,a.Remark,a.id ";
            sqlCmd += "from PLM_Sys_Menu a left join PLM_Sys_Menu b ";
            sqlCmd += "on b.id=a.ParentMenuId where a.enabled=1";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);
                deleteFromDb(rowID);
            }
        }

        private void deleteFromDb(int rowID) 
        {
            string sqlCmd = "update PLM_Sys_Menu set enabled=0 where id='" + rowID + "'";
            SqlSel.ExeSql(sqlCmd);
            bindGrid();
        }
    }
}