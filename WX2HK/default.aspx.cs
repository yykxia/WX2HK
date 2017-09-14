using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;
using FineUI;

namespace WX2HK
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = tbxUserName.Text.Trim();
                string password = tbxPassword.Text;

                string sqlCmd = "";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                sqlCmd = "select * from x_user where name='" + userName + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    sqlCmd = "select * from PLM_Sys_UserList where userid='" + userName + "' and enabled=1";
                    DataTable tempDt = new DataTable();
                    SqlSel.GetSqlSel(ref tempDt, sqlCmd);
                    if (tempDt.Rows.Count > 0)
                    {
                        if (PasswordUtil.ComparePasswords(dt.Rows[0]["Password"].ToString(), password))
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["enabled"]))
                            {
                                //保存登录用户名
                                Session["loginUser"] = userName;
                                Response.Redirect("main.aspx");
                            }
                            else
                            {
                                Alert.ShowInTop("用户未启用，请联系管理员！");
                            }
                        }
                        else
                        {
                            Alert.ShowInTop("用户名或密码错误！");
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowInTop("该帐户没有操作权限！请联系管理员。");
                        return;
                    }
                }
                else
                {
                    Alert.ShowInTop("用户不存在！");
                    return;
                }


            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }

        }
    }
}