using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FineUI;
using IETCsoft.sql;

namespace WX2HK.CheckOut
{
    public partial class LeaveOut_subList : System.Web.UI.Page
    {
        protected static string workNo = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["workerNo"] == null)
                {
                    string code = string.Empty;
                    code = HttpContext.Current.Request.QueryString["Code"];
                    string curPageName = "CheckOut%2fLeaveOut_subList";
                    string userid = "";
                    userid = WX2HK.ReturnInfo.GetUserId(code, curPageName);
                    //string curUserMobile = WX2HK.ReturnInfo.GetUserInfo(userid);
                    workNo = WX2HK.ReturnInfo.getLoginId(userid);
                    Session["workerNo"] = workNo;
                }
                else
                {
                    workNo = Session["workerNo"].ToString();
                }

                bindGrid(workNo, "未阅");

                //窗口固定最大化
                PageContext.RegisterStartupScript(Window1.GetMaximizeReference());
                Window1.Hidden = true;
                //PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
        }

        protected void bindGrid(string curUser,string viweType)
        {
            try
            {
                if (!string.IsNullOrEmpty(curUser))
                {
                    string sqlCmd = "";
                    DataTable dt = new DataTable();
                    if (viweType == "未阅")
                    {
                        sqlCmd = "select * from ";
                        sqlCmd += "(select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name ,";
                        sqlCmd += "CONVERT(varchar(100), T_Overtime.ReqDte, 23) as ReqDte,T_Overtime.ReqMan,T_Overtime.ReqDt1,T_Overtime.ReqDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'加班申请' as viewTyp,'OverTime_leaderView' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_Overtime on T_SpFollow.tabId = T_Overtime.Id ";
                        sqlCmd += "where WorkNo = '" + curUser + "' and checkedte is null and spTyp = '1' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is  null and spTyp = '1' group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = Yhbh) as name,";
                        sqlCmd += "CONVERT(varchar(100), T_CorpOut.SendTime, 23) as ReqDte,T_CorpOut.Yhbh,T_CorpOut.Stime,T_CorpOut.Etime ,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'外出申请' as viewTyp,'' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_CorpOut on T_SpFollow.tabId = T_CorpOut.Id ";
                        sqlCmd += "where WorkNo = '" + curUser + "' and checkedte is null and spTyp = '3'and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is  null and spTyp='3' group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name,";
                        sqlCmd += "CONVERT(varchar(100), T_LeaveReq.ReqDte, 23) as ReqDte,T_LeaveReq.ReqMan,T_LeaveReq.LeaveDt1,T_LeaveReq.LeaveDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'请假申请' as viewTyp,'LeaveOut_leaderView' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_LeaveReq on T_SpFollow.tabId = T_LeaveReq.Id where WorkNo = '" + curUser + "' and checkedte is null and spTyp = '2' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is  null and sptyp=2 group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name,";
                        sqlCmd += "CONVERT(varchar(100), T_PaidLeave.ReqDte, 23) as ReqDte,T_PaidLeave.ReqMan,T_PaidLeave.ReqDt1,T_PaidLeave.ReqDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'调休申请' as viewTyp,'' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_PaidLeave on T_SpFollow.tabId = T_PaidLeave.Id where WorkNo = '" + curUser + "' and checkedte is null and spTyp = '4' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is  null and sptyp=4 group by tabid))a order by reqdte desc";
                    }
                    if (viweType == "已阅")
                    {
                        sqlCmd = "select * from ";
                        sqlCmd += "(select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name ,";
                        sqlCmd += "CONVERT(varchar(100), T_Overtime.ReqDte, 23) as ReqDte,T_Overtime.ReqMan,T_Overtime.ReqDt1,T_Overtime.ReqDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'加班申请' as viewTyp,'OverTime_leaderView' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_Overtime on T_SpFollow.tabId = T_Overtime.Id ";
                        sqlCmd += "where WorkNo = '" + curUser + "' and checkedte is not null and spTyp = '1' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is not  null and spTyp = '1' group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = Yhbh) as name,";
                        sqlCmd += "CONVERT(varchar(100), T_CorpOut.SendTime, 23) as ReqDte,T_CorpOut.Yhbh,T_CorpOut.Stime,T_CorpOut.Etime ,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'外出申请' as viewTyp,'' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_CorpOut on T_SpFollow.tabId = T_CorpOut.Id ";
                        sqlCmd += "where WorkNo = '" + curUser + "' and checkedte is not null and spTyp = '3'and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is not  null and spTyp='3' group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name,";
                        sqlCmd += "CONVERT(varchar(100), T_LeaveReq.ReqDte, 23) as ReqDte,T_LeaveReq.ReqMan,T_LeaveReq.LeaveDt1,T_LeaveReq.LeaveDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'请假申请' as viewTyp,'LeaveOut_leaderView' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_LeaveReq on T_SpFollow.tabId = T_LeaveReq.Id where WorkNo = '" + curUser + "' and checkedte is not null and spTyp = '2' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is not  null and sptyp=2 group by tabid) ";
                        sqlCmd += "union all select (select Name +'/'+ Gh_id from View_Rs_employeeinfo where Gh_id = ReqMan)as name,";
                        sqlCmd += "CONVERT(varchar(100), T_PaidLeave.ReqDte, 23) as ReqDte,T_PaidLeave.ReqMan,T_PaidLeave.ReqDt1,T_PaidLeave.ReqDt2,T_SpFollow.tabId,T_SpFollow.spTyp,T_SpFollow.Sort,'调休申请' as viewTyp,'' as linkPage ";
                        sqlCmd += "from T_SpFollow inner join T_PaidLeave on T_SpFollow.tabId = T_PaidLeave.Id where WorkNo = '" + curUser + "' and checkedte is not null and spTyp = '4' and ";
                        sqlCmd += "T_SpFollow.id in (select min(T_SpFollow.id) from T_SpFollow where checkedte is not  null and sptyp=4 group by tabid))a order by reqdte desc";
                    }

                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                }
                else 
                {
                    Panel1.Hidden = true;
                    HttpContext.Current.Response.Write("信息不存在或非企业内部人员！");
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void rbtnFirstAuto_CheckedChanged(object sender, CheckedEventArgs e)
        {
            if (!e.Checked)
            {
                return;
            }

            string checkedValue = String.Empty;
            if (rbtnFirstAuto.Checked)
            {
                checkedValue = rbtnFirstAuto.Text;
                bindGrid(workNo, checkedValue);
                Grid1.PageIndex = 0;
            }
            else if (rbtnSecondAuto.Checked)
            {
                checkedValue = rbtnSecondAuto.Text;
                bindGrid(workNo, checkedValue);
                Grid1.PageIndex = 0;

            }
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
        }

        protected void btn_closeWindow_Click(object sender, EventArgs e)
        {
            Window1.Hidden = true;
        }

        //protected void rbtnAuto_CheckedChanged(object sender, CheckedEventArgs e)
        //{
        //    if (!e.Checked)
        //    {
        //        return;
        //    }

        //    string checkedValue = String.Empty;
        //    if (rbtnFirstAuto.Checked)
        //    {
        //        checkedValue = rbtnFirstAuto.Text;
        //        bindGrid(workNo, checkedValue);
        //    }
        //    else if (rbtnSecondAuto.Checked)
        //    {
        //        checkedValue = rbtnSecondAuto.Text;
        //        bindGrid(workNo, checkedValue);
        //    }
        //}
    }
}