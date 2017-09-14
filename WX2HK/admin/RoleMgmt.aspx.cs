using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WX2HK.admin
{
    public partial class RoleMgmt : System.Web.UI.Page
    {
        private bool AppendToEnd = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {

                JObject defaultObj = new JObject();
                defaultObj.Add("id", getMaxId());
                defaultObj.Add("roleName", "");
                defaultObj.Add("roleDesc", "");
                defaultObj.Add("UseStatus", true);

                btn_addNew.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

                bindGrid();
            }
        }

        //模拟数据库id增长列
        protected int getMaxId()
        {
            int maxId;
            if (string.IsNullOrEmpty(label_hidden.Text))
            {
                maxId = 0;
            }
            else
            {
                maxId = Convert.ToInt32(label_hidden.Text) + 1;
            }

            label_hidden.Text = maxId.ToString();
            return maxId;

        }

        protected void bindGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select * from PLM_Sys_Role order by id";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

                sqlCmd = "select max(id) from PLM_Sys_Role";
                string curMaxId = SqlSel.GetSqlScale(sqlCmd).ToString();
                if (string.IsNullOrEmpty(curMaxId))
                {
                    label_hidden.Text = "0";
                }
                else
                {
                    label_hidden.Text = curMaxId;
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "";
                // 修改的现有数据
                Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {

                    if (string.IsNullOrEmpty(Grid1.Rows[rowIndex].Values[0].ToString()))
                    {
                        Alert.ShowInTop("角色名不可为空！");
                        return;
                    }
                    int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                    sqlCmd = "update PLM_Sys_Role set roleName='" + Grid1.Rows[rowIndex].Values[0].ToString() + "',";
                    sqlCmd += "roleDesc='" + Grid1.Rows[rowIndex].Values[1].ToString() + "',";
                    sqlCmd += "UseStatus='" + Convert.ToBoolean(Grid1.Rows[rowIndex].Values[2]) + "',createDate='" + DateTime.Now + "' ";
                    sqlCmd += "where id=" + rowID;
                    SqlSel.ExeSql(sqlCmd);
                }

                // 新增数据
                List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    //DataRow rowData = CreateNewData(table, newAddedList[i]);
                    //table.Rows.Add(rowData);
                    if (string.IsNullOrEmpty(newAddedList[i]["roleName"].ToString()))
                    {
                        Alert.ShowInTop("角色名不可为空！");
                        return;
                    }
                    sqlCmd = "insert into PLM_Sys_Role (roleName,roleDesc,UseStatus,createDate) values ";
                    sqlCmd += "('" + newAddedList[i]["roleName"].ToString().Trim() + "','" + newAddedList[i]["roleDesc"].ToString() + "',";
                    sqlCmd += "'" + Convert.ToBoolean(newAddedList[i]["UseStatus"]) + "','" + DateTime.Now + "')";
                    SqlSel.ExeSql(sqlCmd);

                }
                //表格数据重新绑定
                bindGrid();

                Alert.Show("保存成功！");
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);
                deleteFromDb(rowID);


                Alert.ShowInTop("删除数据成功!（表格数据已重新绑定）");
            }
        }

        // 删除选中行的脚本
        private string GetDeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }

        protected void DeleteRowByID(int rowid)
        {
            List<Dictionary<string, object>> newAddedList = Grid1.GetNewAddedList();
            int find = -1;
            for (int i = 0; i < newAddedList.Count; i++)
            {
                if (rowid == Convert.ToInt32(newAddedList[i]["id"]))
                {
                    find = i;
                    GetDeleteScript();//前端数据直接脚本删除
                    break;
                }
            }

            if (find == -1)
            {
                deleteFromDb(rowid);
            }
        }
        //服务器端删除
        protected void deleteFromDb(int DbId)
        {
            string sqlCmd = "delete from PLM_Sys_Role where id=" + DbId;
            SqlSel.ExeSql(sqlCmd);
            bindGrid();
        }

    }
}