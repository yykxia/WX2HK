﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WX2HK
{
    public partial class main : System.Web.UI.Page
    {
        private static string userId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["loginUser"] == null)
                {
                    Response.Redirect("default.aspx");
                }
                else 
                {
                    userId = Session["loginUser"].ToString();
                    menuTreeBind(userId);
                    string sqlCmd = "select chineseName from x_user where name='" + userId + "' ";
                    string curUser = SqlSel.GetSqlScale(sqlCmd).ToString();
                    //l_loginInfo.Text = curUser + "，欢迎登陆.";
                }
            }
        }

        protected void menuTreeBind(string userId)
        {
            try
            {
                if (userId != null || userId != "")
                {
                    string sqlCmd = "select * from PLM_Sys_Menu where id in( ";
                    sqlCmd += "select menuId from PLM_Sys_RoleMenu where roleid in ( ";
                    sqlCmd += "select roleid from PLM_Sys_UserRole where userid=( ";
                    sqlCmd += "select id from x_user where name='" + userId + "')) group by menuId) and enabled=1";
                    DataTable treeDt = new DataTable();
                    SqlSel.GetSqlSel(ref treeDt, sqlCmd);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(treeDt);
                    ds.Relations.Add("TreeRelation", ds.Tables[0].Columns["Id"], ds.Tables[0].Columns["ParentMenuId"], false);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["ParentMenuId"].ToString() == "0")
                        {
                            TreeNode node = new TreeNode();
                            node.Text = row["Title"].ToString();
                            node.NodeID = row["id"].ToString();

                            //node.NavigateUrl = ResolveUrl(row["NavigateUrl"].ToString());
                            tree_menu.Nodes.Add(node);

                            ResolveSubTree(row, node);
                        }
                    }
                    tree_menu.Nodes[0].Expanded = true;


                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

        private void ResolveSubTree(DataRow dataRow, TreeNode treeNode)
        {
            DataRow[] rows = dataRow.GetChildRows("TreeRelation");
            if (rows.Length > 0)
            {
                treeNode.Expanded = true;
                foreach (DataRow row in rows)
                {
                    TreeNode node = new TreeNode();
                    node.Text = row["Title"].ToString();
                    node.NodeID = row["id"].ToString();
                    //node.NavigateUrl = ResolveUrl(row["NavigateUrl"].ToString());
                    treeNode.Nodes.Add(node);
                    node.EnableClickEvent = true;

                    ResolveSubTree(row, node);
                }
            }
        }


        protected void tree_menu_NodeCommand1(object sender, TreeCommandEventArgs e)
        {
            DataTable menuDt = new DataTable();
            int menuId = Convert.ToInt32(e.NodeID);
            string sqlCmd = "select * from PLM_Sys_Menu where id=" + menuId;
            SqlSel.GetSqlSel(ref menuDt, sqlCmd);

            string NavigateUrl = menuDt.Rows[0]["NavigateUrl"].ToString();
            string tabName = menuDt.Rows[0]["Title"].ToString();

            PageContext.RegisterStartupScript(mainTabStrip.GetAddTabReference("dynamic_tab" + menuId.ToString(), ResolveUrl(NavigateUrl), tabName, IconHelper.GetIconUrl(Icon.ApplicationAdd), true));
        }

        protected void btn_exit_Click(object sender, EventArgs e)
        {
            if (Session["loginUser"] != null)
            {
                Session["loginUser"] = null;
            }
            PageContext.Redirect("default.aspx");
        }
    }
}