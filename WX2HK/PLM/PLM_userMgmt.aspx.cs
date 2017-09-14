using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WX2HK.PLM
{
    public partial class PLM_userMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid();
                btn_addNew.OnClientClick = Window1.GetShowReference("PLM_userMgmt_Edit.aspx");
            }
        }

        //加载系统人员
        private void bindGrid() 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select a.id,a.userId,b.chineseName";
            sqlCmd += " from PLM_Sys_UserList a left join x_user b on b.name=a.userId where a.enabled=1";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void btn_unUse_Click(object sender, EventArgs e)
        {
            string sqlCmd = "update PLM_Sys_UserList set enabled=0 where id=";
            string userId = "";
            int[] selections = Grid1.SelectedRowIndexArray;
            foreach (int rowIndex in selections) 
            {
                userId = Grid1.DataKeys[rowIndex][0].ToString();
                sqlCmd += userId;
                SqlSel.ExeSql(sqlCmd);
            }
            bindGrid();
        }
    }
}