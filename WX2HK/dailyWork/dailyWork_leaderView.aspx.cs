using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WX2HK.dailyWork
{
    public partial class dailyWork_leaderView : System.Web.UI.Page
    {
        private static string tabId = string.Empty;

        private static string curWorkNo = string.Empty;//当前用户工号

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tabId = Request["mainId"];
                //获取当前登录人员企业内部工号
                //string curPageName = "dailyWork%2fdailyWork_leaderView";
                //string code = string.Empty;
                //code = HttpContext.Current.Request.QueryString["Code"];
                //string userid = WX2HK.ReturnInfo.GetUserId(code, curPageName);
                ////string curUserMobile = WX2HK.ReturnInfo.GetUserInfo(userid);
                //curWorkNo = WX2HK.ReturnInfo.getWorkNo(userid);

                //if (haveNoCheckPrivilege(tabId, curWorkNo))
                //{
                //    btnSubmit.Hidden = true;
                //}

                loadHead(tabId);

            }
        }

        protected void loadHead(string mainId) 
        {
            String sqlcmd = "select * from T_DailyWork left join view_rs_employeeinfo on WorkNo=gh_id where T_DailyWork.Id =" + mainId + " and T_DailyWork.stat>0";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlcmd); 
            if (dt.Rows.Count > 0)
            {
                DateTime reqdate = Convert.ToDateTime(dt.Rows[0]["WorkDte"]);
                label_date.Text = reqdate.ToShortDateString();
                label_dept.Text = dt.Rows[0]["bm1"].ToString();
                label_name.Text = dt.Rows[0]["name"].ToString();
                label_selfGoal.Text = dt.Rows[0]["SelfGrade"].ToString() + " 分";
                label_hidden.Text = dt.Rows[0]["workNo"].ToString();
                //加载工作内容明细
                loadWorkDetail(mainId);
            }
            else
            {
                SimpleForm1.Hidden = true;
                HttpContext.Current.Response.Write("信息不存在或非企业内部人员！");
            }

            //判断日志状态
            if (!string.IsNullOrEmpty(dt.Rows[0]["LeaderGrade"].ToString())) 
            {
                btnSubmit.Hidden = true;
                RadioButtonList_goal.SelectedValue = dt.Rows[0]["LeaderGrade"].ToString();
                RadioButtonList_goal.Readonly = true;
                TextArea_context.Text = dt.Rows[0]["LeaderGradeReason"].ToString();
                TextArea_context.Readonly = true;
            }
        }

        //工作内容明细
        protected void loadWorkDetail(string mainId) 
        {
            string  sqlStr = "select * from T_Work_Detail_Record where MainID=" + mainId + " order by Id";
            DataTable curDt = new DataTable();
            SqlSel.GetSqlSel(ref curDt, sqlStr);
            Grid1.DataSource = curDt;
            Grid1.DataBind();
        }

        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RadioButtonList_goal.SelectedValue))
            {
                string sqlcmd = "update T_DailyWork set LeaderGrade=" + Convert.ToInt32(RadioButtonList_goal.SelectedValue) + ",LeaderGradeReason='" + TextArea_context.Text + "' where Id=" + tabId + " ";
                int exeCount = SqlSel.ExeSql(sqlcmd);
                if (exeCount > 0) 
                {
                    //将审批结果反馈给相关人员
                    string messageContent = string.Format("工作评分：{0}分\\n领导寄语：{1}",RadioButtonList_goal.SelectedValue,TextArea_context.Text);
                    sqlcmd = "select cellPhone from x_user where name='" + label_hidden.Text + "'";
                    string targetNum = SqlSel.GetSqlScale(sqlcmd).ToString();
                    if (!string.IsNullOrEmpty(targetNum))
                    {
                        WX2HK.ReturnInfo.messagePushByNum(targetNum + ";", string.Format("{0}-工作情况", label_date.Text), "", messageContent, "");
                    }
                    //Alert.Show("提交成功！");
                    btnSubmit.Hidden = true;

                    // 关闭本窗体，然后刷新父窗体
                    PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
                }
            }
            else 
            {
                Alert.ShowInTop("请打分!");
                return;
            }
        }

        //判断当前人员是否有页面审批权限
        private bool haveNoCheckPrivilege(int mainId,string loginUser) 
        {
            string sqlCmd = "select rz_parentid from view_rs_employeeinfo where gh_id=(select workno from t_dailywork where id=" + mainId + ")";
            string rz_parentid = SqlSel.GetSqlScale(sqlCmd).ToString();//拥有审批权限的工号
            if (loginUser != rz_parentid)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        
    }
}