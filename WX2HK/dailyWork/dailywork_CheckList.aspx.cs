using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FineUI;
using IETCsoft.sql;

namespace WX2HK.dailyWork
{
    public partial class dailywork_CheckList : System.Web.UI.Page
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
                    string curPageName = "dailyWork%2fdailywork_CheckList";
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

                Grid1Bind2("0");

                //窗口固定最大化
                PageContext.RegisterStartupScript(Window1.GetMaximizeReference());
                Window1.Hidden = true;
                //PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
        }



        private void Grid1Bind2(string loadType)
        {
            string sqlcmd = "";
            DataTable dt = new DataTable();

            if (loadType == "0")
            {
                sqlcmd = "select (select Gh_id +'/'+ Name from View_Rs_employeeinfo where Gh_id = WorkNo)as name,";
                sqlcmd += "CONVERT(varchar(100), WorkDte, 23) as dte,* from T_DailyWork ";
                sqlcmd += "where exists (select Gh_id from View_Rs_employeeinfo where Gh_id=T_DailyWork.WorkNo and rz_ParentID='" + workNo + "') ";
                sqlcmd += "and Stat='11' and leadergrade is null order by WorkDte desc";
            }
            if (loadType == "1")
            {
                sqlcmd = "select (select Gh_id +'/'+ Name from View_Rs_employeeinfo where Gh_id = WorkNo)as name,";
                sqlcmd += "CONVERT(varchar(100), WorkDte, 23) as dte,* from T_DailyWork ";
                sqlcmd += "where exists (select Gh_id from View_Rs_employeeinfo where Gh_id=T_DailyWork.WorkNo and rz_ParentID='" + workNo + "') ";
                sqlcmd += "and workdte>'" + DateTime.Now.AddDays(-3).ToShortDateString() + "' ";
                sqlcmd += "and Stat='11' and leadergrade is not null order by WorkDte desc";
            }
            SqlSel.GetSqlSel(ref dt, sqlcmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        //切换审阅状态事件
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedItem != null)
            {
                Grid1Bind2(RadioButtonList1.SelectedValue);
            }
            else
            {
                Alert.ShowInTop(String.Format("没有选中项！"));
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

    }
}