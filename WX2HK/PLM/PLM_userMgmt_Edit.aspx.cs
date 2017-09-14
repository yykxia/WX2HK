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
    public partial class PLM_userMgmt_Edit : System.Web.UI.Page
    {
        private static string userId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                userId = Request["userId"];
                loadRoleItem();
                LoadData();
            }
        }

        private void LoadData() 
        {
            txb_workNo.Text = userId;
            DataTable dt = new DataTable();
            string sqlCmd = "select * from PLM_Sys_UserRole where userid=(select id from x_user where name='" + userId + "')";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            string[] roleArray = new string[dt.Rows.Count];//创建一个角色数组
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                roleArray[i] = dt.Rows[i]["roleId"].ToString();
            }
            ckb_roleList.SelectedValueArray = roleArray;
        }

        private void loadRoleItem() 
        {
            string sqlCmd = "select * from PLM_Sys_Role where useStatus=1";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            ckb_roleList.DataSource = dt;
            ckb_roleList.DataValueField = "id";
            ckb_roleList.DataTextField = "roleName";
            ckb_roleList.DataBind();
        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            try 
            {
                string sqlCmd = "";
                DataTable dt = new DataTable();
                int userStatus = CkeckBox_enabled.Checked ? 1 : 0;
                sqlCmd = "select * from x_user where name='" + txb_workNo.Text + "' and Enabled=1";//是否在办公系统中开通相应账户
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    int fId = Convert.ToInt32(dt.Rows[0]["id"]);//用户流水id
                    if (string.IsNullOrEmpty(userId))//新建用户 
                    {
                        sqlCmd = "insert into PLM_Sys_UserList (userId,enabled) values (";
                        sqlCmd += "'" + txb_workNo.Text + "','" + userStatus + "')";
                        SqlSel.ExeSql(sqlCmd);

                        //删除用户现有角色
                        sqlCmd += "delete from PLM_Sys_UserRole where userid=(select id from x_user where name='" + userId + "')";
                        SqlSel.ExeSql(sqlCmd);

                        //更新角色信息
                        string[] selectValueArray = ckb_roleList.SelectedValueArray;
                        foreach (string item in selectValueArray) 
                        {
                            sqlCmd = "insert into PLM_Sys_UserRole (userid,roleid,addtime) values (";
                            sqlCmd += "'" + fId + "','"+item+"','" + DateTime.Now + "')";
                            SqlSel.ExeSql(sqlCmd);
                        }
                    }
                    else //更新用户信息
                    {
                        sqlCmd = "update PLM_Sys_UserList set enabled='" + userStatus + "' where userId='" + userId + "'";
                        SqlSel.ExeSql(sqlCmd);                        
                        
                        //删除用户现有角色
                        sqlCmd += "delete from PLM_Sys_UserRole where userid=(select id from x_user where name='" + userId + "')";
                        SqlSel.ExeSql(sqlCmd);

                        string[] selectValueArray = ckb_roleList.SelectedValueArray;
                        foreach (string item in selectValueArray)
                        {
                            sqlCmd = "insert into PLM_Sys_UserRole (userid,roleid,addtime) values (";
                            sqlCmd += "'" + fId + "','" + item + "','" + DateTime.Now + "')";
                            SqlSel.ExeSql(sqlCmd);
                        }
                    }
                    Alert.ShowInTop("保存成功！");
                    PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
                }
                else
                {
                    Alert.ShowInTop("系统不存在此工号！请查正或联系管理员。");
                    return;
                }

            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }


    }
}