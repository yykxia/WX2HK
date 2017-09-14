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
    public partial class RoleModuel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                bindGrid1();
                bindGrid2(Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndex][0]));
            }
        }


        private void bindGrid1() 
        {
            string sqlCmd = "select * from PLM_Sys_Role where useStatus=1";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
            Grid1.SelectedRowIndex = 0;
        }

        private void bindGrid2(int roleId) 
        {
            string sqlCmd = "select (case when menuId is null then 0 else 1 end ) as CanRead,* from PLM_Sys_Menu left join (";
            sqlCmd += "select menuId from PLM_Sys_RoleMenu ";
            sqlCmd += "where roleId='" + roleId + "') a on menuId=PLM_Sys_Menu.id where enabled=1";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        /// <summary>
        /// 获取选中的角色ID（没有则返回-1）
        /// </summary>
        /// <returns></returns>
        private int GetSelectedRoleId()
        {
            int roleId = -1;
            int rowIndex = Grid1.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                roleId = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
            }
            return roleId;
        }

        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int roleId = GetSelectedRoleId();
                // 添加所有记录
                FineUI.CheckBoxField canReadField = Grid2.FindColumn("CanRead") as FineUI.CheckBoxField;
                int MenuId = 0;
                foreach (GridRow row in Grid2.Rows)
                {
                    int rowIndex = row.RowIndex;

                    object[] dataKeys = Grid2.DataKeys[rowIndex];
                    // 当前行对应的模块名称
                    MenuId = Convert.ToInt32(dataKeys[0]);
                    bool canRead = canReadField.GetCheckedState(rowIndex);
                    string sqlCmd = "";
                    if (canRead) 
                    {
                        sqlCmd = "insert into PLM_Sys_RoleMenu (roleId,menuId) values (";
                        sqlCmd += "'" + roleId + "','" + MenuId + "')";
                        SqlSel.ExeSql(sqlCmd);
                    }
                    if (!canRead) 
                    {
                        sqlCmd = "delete from PLM_Sys_RoleMenu where roleId='" + roleId + "' and menuId='" + MenuId + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }

                    Alert.ShowInTop("角色权限更新成功！");
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid1_RowSelect(object sender, GridRowSelectEventArgs e)
        {
            try
            {
                int roleId = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);//选中行的角色ID
                bindGrid2(roleId);
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }
    }
}