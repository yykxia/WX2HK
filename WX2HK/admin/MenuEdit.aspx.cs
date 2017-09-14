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
    public partial class MenuEdit : System.Web.UI.Page
    {
        private static string recvId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                recvId = Request["id"];
                if (string.IsNullOrEmpty(recvId))
                {
                    ddlBind("0");
                }
                else 
                {
                    ddlBind(recvId);
                }
            }
        }

        private void ddlBind(string loadType) 
        {
            try
            {
                string sqlCmd = "select * from PLM_Sys_Menu where ParentMenuId=0 and enabled=1";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                ddl_parentMenu.DataTextField = "title";
                ddl_parentMenu.DataValueField = "id";
                ddl_parentMenu.DataSource = dt;
                ddl_parentMenu.DataBind();
                if (loadType != "0")//非新建菜单绑定现有数据
                {
                    sqlCmd = "select * from PLM_Sys_Menu where id='" + recvId + "'";
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    txb_title.Text = dt.Rows[0]["Title"].ToString();
                    txb_url.Text = dt.Rows[0]["NavigateUrl"].ToString();
                    txa_desc.Text = dt.Rows[0]["Remark"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ParentMenuId"].ToString()))
                    {
                        ddl_parentMenu.SelectedValue = dt.Rows[0]["ParentMenuId"].ToString();
                    }
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            try 
            {
                string sqlCmd = "", parentId = string.Empty;
                int sortIndex = 0;
                if (ddl_parentMenu.SelectedItem != null) 
                {
                    parentId = ddl_parentMenu.SelectedValue;
                    sortIndex = getSortIndex(parentId);
                    sortIndex += 1;
                }
                //新建菜单直接添加
                if (string.IsNullOrEmpty(recvId))
                {
                    sqlCmd = "insert into PLM_Sys_Menu (Title,NavigateUrl,Remark,ParentMenuId,SortIndex,enabled) values ";
                    sqlCmd += "('" + txb_title.Text + "','" + txb_url.Text + "','" + txa_desc.Text + "','" + parentId + "','" + sortIndex + "','1')";
                    SqlSel.ExeSql(sqlCmd);
                }
                else//历史项进行更新 
                {
                    sqlCmd = "update PLM_Sys_Menu set Title='" + txb_title.Text + "',NavigateUrl='" + txb_url.Text + "',";
                    sqlCmd += "Remark='" + txa_desc.Text + "',ParentMenuId='" + parentId + "' where id='" + recvId + "'";
                    SqlSel.ExeSql(sqlCmd);
                }
                Alert.Show("编辑成功！");
                PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());//关闭并刷新数据
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        //获取菜单序列号
        private int getSortIndex(string parentId) 
        {
            int sortIndex = 0;
            DataTable dt = new DataTable();
            string sqlCmd = "select max(sortIndex) from PLM_Sys_Menu where ParentMenuId='" + parentId + "'";
            string res=SqlSel.GetSqlScale(sqlCmd).ToString();
            if (!string.IsNullOrEmpty(res)) 
            {
                sortIndex = Convert.ToInt32(res);
            }
            return sortIndex;
        }

    }
}