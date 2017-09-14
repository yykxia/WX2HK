using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Text;
using System.Data;
using System.Collections;
using System.Web.Script.Serialization;
using IETCsoft.sql;
using FineUI;

namespace WX2HK.dailyWork
{
    public partial class SelfWorkDtl : System.Web.UI.Page
    {
        private static string curUser = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string curPageName = "dailyWork%2fSelfWorkDtl";
                string code = string.Empty;
                code = HttpContext.Current.Request.QueryString["Code"];
                string userid = WX2HK.ReturnInfo.GetUserId(code, curPageName);
                //string curUserMobile = WX2HK.ReturnInfo.GetUserInfo(userid);
                curUser = WX2HK.ReturnInfo.getLoginId(userid);

                loadHeader(curUser);
                bindDropDownList(curUser);
            }
        }

        public void loadHeader(string workNo)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(workNo))
            {
                string sqlCmd = "select * from view_rs_employeeinfo where gh_id='" + workNo + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    label_dept.Text = dt.Rows[0]["bm1"].ToString();
                    label_name.Text = dt.Rows[0]["name"].ToString();
                    label_date.Text = DateTime.Now.ToShortDateString();
                }
            }
            else
            {
                SimpleForm1.Hidden = true;
                HttpContext.Current.Response.Write("信息不存在或非企业内部人员！");
            }
            
        }

        protected void btn_sub_Click(object sender, EventArgs e)
        {
            try 
            {
                if (Grid1.Rows.Count > 0)
                {
                    for (int i = 0; i < Grid1.Rows.Count; i++)
                    {
                        GridRow grow = Grid1.Rows[i];
                        if (string.IsNullOrEmpty(((System.Web.UI.WebControls.TextBox)grow.FindControl("tbxWorkDtl")).Text))
                        {
                            Alert.ShowInTop("工作内容不可为空！");
                            return;
                        }
                    }

                    string sqlCmd = "";
                    sqlCmd = "insert into T_DailyWork (ReqDte,WorkDte,WorkNo,SelfGrade,Stat) values";
                    sqlCmd += "('" + DateTime.Now + "','" + label_date.Text + "','" + curUser + "'," + Convert.ToInt32(RadioButtonList_goal.SelectedValue) + ",11)";
                    SqlSel.ExeSql(sqlCmd);

                    sqlCmd = "select max(Id) from T_DailyWork where workno='" + curUser + "'";
                    int maxId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                    for (int i = 0; i < Grid1.Rows.Count; i++)
                    {
                        GridRow grow = Grid1.Rows[i];
                        sqlCmd = "insert into T_Work_Detail_Record";
                        sqlCmd += "(MainID,WorkDetail,WorkProcess,Mark) values";
                        sqlCmd += "(" + maxId + ",'" + ((System.Web.UI.WebControls.TextBox)grow.FindControl("tbxWorkDtl")).Text + "',100,'" + Grid1.Rows[i].Values[0].ToString() + "')";
                        SqlSel.ExeSql(sqlCmd);
                    }


                    //推送相关提交信息
                    sqlCmd = "select CellPhone from x_user where name=(select rz_parentid from view_rs_employeeinfo where gh_id='" + curUser + "')";
                    string targetNum = SqlSel.GetSqlScale(sqlCmd).ToString();
                    string messageContent = string.Format("来自{0}的工作汇报", label_name.Text);
                    if (!string.IsNullOrEmpty(targetNum))
                    {
                        string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx3068893beff3a241&redirect_uri=http%3a%2f%2fhkoa.hkfoam.com%3a30018%2fdailyWork%2fdailyWork_leaderView.aspx?mainId={0}&response_type=code&scope=SCOPE&state=STATE#wechat_redirect", maxId);
                        WX2HK.ReturnInfo.messagePushByNum(targetNum + ";", string.Format("{0}-工作汇报", label_date.Text), url, messageContent, "");
                    }

                    Alert.Show("提交成功！");
                    PageContext.RegisterStartupScript(SimpleForm1.GetResetReference());
                    Grid1.Reset();
                    btnSubmit.Hidden = true;
                }
                else 
                {
                    Alert.ShowInTop("请填写相关工作内容后再提交！");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //下拉列表选定事件
        protected void ddl_resp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_resp.SelectedItem != null) 
            {
                DataTable curDt = getDataTable();
                DataRow newRow = curDt.NewRow();
                newRow["id"] = ddl_resp.SelectedValue;
                newRow["workDtl"] = "";
                curDt.Rows.Add(newRow);
                Grid1.DataSource = curDt;
                Grid1.DataBind();
            }
        }

        //绑定职责下拉列表
        protected void bindDropDownList(string workNo) 
        {
            try
            {
                String sqlcmd = "select ('[' + cast(id as varchar(20)) + ']' + DutyOfWork) as dutyDtl,";
                sqlcmd += "* from T_Work_Detail_Duty where Gh_Id ='" + workNo + "' order by Sort";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlcmd);
                ddl_resp.DataTextField = "dutyDtl";
                ddl_resp.DataValueField = "id";
                ddl_resp.DataSource = dt;
                ddl_resp.DataBind();
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            LinkButtonField deleteField = Grid1.FindColumn("Delete") as LinkButtonField;
            DataTable curDt = getDataTable();
            curDt.Rows.RemoveAt(e.RowIndex);
            Grid1.DataSource = curDt;
            Grid1.DataBind();
        }
        
        //得到当前grid绑定数据源
        protected DataTable getDataTable()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("workDtl", typeof(string));
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    GridRow grow = Grid1.Rows[i];
                    System.Web.UI.WebControls.TextBox tbxWorkDtl = (System.Web.UI.WebControls.TextBox)grow.FindControl("tbxWorkDtl");
                    dr[0] = grow.Values[0].ToString();
                    dr[1] = tbxWorkDtl.Text;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return null;
            }
        }

        //protected void Grid1_PreDataBound(object sender, EventArgs e)
        //{
        //    LinkButtonField deleteField = Grid1.FindColumn("Delete") as LinkButtonField;
        //    deleteField.OnClientClick = deleteByRowId(Grid1.SelectedRowIndex);
        //}

        //protected string deleteByRowId(int rowId) 
        //{
        //    DataTable curDt = getDataTable();
        //    if (rowId >= 0)
        //    {
        //        curDt.Rows.RemoveAt(rowId);
        //        Grid1.DataSource = curDt;
        //        Grid1.DataBind();
        //        return "1";
        //    }
        //    else 
        //    {
        //        Alert.Show("0");
        //        return "0";
        //    }
        //}

    }
}