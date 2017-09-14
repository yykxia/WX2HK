using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

namespace WX2HK.CheckOut
{
    public partial class OverTime_leaderView : System.Web.UI.Page
    {
        private static string tabId = string.Empty;
        private static string curWorkNo = string.Empty;//当前用户工号

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tabId = Request["mainId"];
                //string pageCode = HttpContext.Current.Request.QueryString["Code"];
                //curWorkNo = WX2HK.ReturnInfo.getWorkNo(pageCode, "CheckOut%2fOverTime_leaderView");
                loadHeader(tabId);
                //var code = HttpContext.Current.Request.QueryString["Code"];
                ////
                //TimeStampStr = GetTimeStamp();
                //NonceStr = randNonce();
                //string curUrl = string.Format("http://hkoa.hkfoam.com:30018/CheckOut/LeaveOut_leaderView.aspx?code={0}&state=STATE", code);
                //MsgSigStr = WX2HK.VerifyLegal.createSign(NonceStr, TimeStampStr, curUrl);

            }
        }


        public void loadHeader(string mainId)
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select (select name from view_rs_employeeinfo where gh_id=ReqMan) as name,";
                sqlCmd += "(select bm1 from view_rs_employeeinfo where gh_id=ReqMan) as bm1,";
                sqlCmd += "CONVERT(varchar(100), reqDte, 20) as dReqDte,";
                sqlCmd += "CONVERT(varchar(100), ReqDt1, 20) as dleaveDt1,";
                sqlCmd += "CONVERT(varchar(100), ReqDt2, 20) as dleaveDt2,CONVERT(float,AddTime) as fHours,* from t_OverTime where id=" + mainId;
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["stat"].ToString() == "0")
                    {
                        label_status.Text = "待审批";
                    }
                    if (dt.Rows[0]["stat"].ToString() == "1")
                    {
                        label_status.Text = "已完成";
                    }
                    label_dept.Text = dt.Rows[0]["bm1"].ToString();
                    label_name.Text = dt.Rows[0]["name"].ToString();
                    label_startTime.Text = dt.Rows[0]["dleaveDt1"].ToString();
                    label_endTime.Text = dt.Rows[0]["dleaveDt2"].ToString();
                    label_reqTime.Text = dt.Rows[0]["dReqDte"].ToString();
                    label_hours.Text = dt.Rows[0]["fHours"].ToString();
                    label_desc.Text = dt.Rows[0]["Reason"].ToString();

                    //添加请假人员明细
                    sqlCmd = "select View_Rs_employeeinfo.Name,Rs_QjAndJb_Subdtl.Yhbh from Rs_QjAndJb_Subdtl inner join ";
                    sqlCmd += "View_Rs_employeeinfo on Rs_QjAndJb_Subdtl.Yhbh = View_Rs_employeeinfo.Gh_id where Rs_QjAndJb_Subdtl.MainId='" + mainId + "' and Rs_QjAndJb_Subdtl.Typ = '1'";
                    DataTable dt_persondtl = new DataTable();
                    SqlSel.GetSqlSel(ref dt_persondtl, sqlCmd);
                    StringBuilder strbuilder = new StringBuilder();
                    int rowscount = dt_persondtl.Rows.Count;
                    for (int i = 0; i < rowscount; i++)
                    {
                        String jbry = dt_persondtl.Rows[i]["Name"].ToString().Trim() + "/" + dt_persondtl.Rows[i]["Yhbh"].ToString();
                        strbuilder.AppendFormat("{0};", jbry);
                    }
                    label_personDtl.Text = strbuilder.ToString();

                    //是否可以审批
                    if (dt.Rows[0]["stat"].ToString() != "0")
                    {
                        btnSubmit.Hidden = true;//隐藏审批按钮
                    }
                }
                else
                {
                    SimpleForm1.Hidden = true;
                    HttpContext.Current.Response.Write("信息不存在或非企业内部人员！");
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //更新加班申请表状态
                DateTime spDte = System.DateTime.Now;
                String sqlCmd = "update T_Overtime set Stat = '1' where Id = " + tabId + "";
                SqlSel.ExeSql(sqlCmd);
                //更新审批状态
                sqlCmd = "update T_SpFollow set Checked = '1',CheckeDte = '" + spDte + "' where tabId = " + tabId + " and spTyp = '1'";
                SqlSel.ExeSql(sqlCmd);
                //写入考勤数据库 
                sqlCmd = "INSERT INTO [HKKQ_real].[dbo].[KQ_b_Addban] ";
                sqlCmd += "([YhBH] ,[StartRqSj] ,[EndRqSj],[czy] ,[rq],[typ],[times])";
                sqlCmd += "(select yhbh,reqdt1,reqdt2,reqman,Convert(char(10),reqdte,120),addfunc,ADDTIME from T_Overtime,rs_qjandjb_subdtl ";
                sqlCmd += "where T_Overtime.id= " + tabId + " and  typ=1 and rs_qjandjb_subdtl.mainid= " + tabId + ")";
                SqlSel.ExeSql(sqlCmd);

                //Alert.ShowInTop("审批完成！");
                //审批按钮隐藏
                btnSubmit.Hidden = true;

                //将考勤结果反馈给部门考勤员
                sqlCmd = "select x_user.cellPhone from ";
                sqlCmd += "(select a.gh_id from view_banzhi_rsRole a inner join  view_rs_employeeinfo b on a.departid=b.departid and  b.gh_id=(select reqman from T_Overtime where id=" + tabId + ")) c ";
                sqlCmd += "left join x_user on x_user.name=c.gh_id";
                string notifyTarget = SqlSel.GetSqlScale(sqlCmd).ToString();
                //抄送信息至部门相关考勤员
                string messageContent = string.Format("加班人员：{0}\\n加班时长：{3} \\n起始时间：{1} \\n截止时间：{2} ", label_personDtl.Text, label_startTime.Text, label_endTime.Text, label_hours.Text);
                if (!string.IsNullOrEmpty(notifyTarget))
                {
                    WX2HK.ReturnInfo.messagePushByNum(notifyTarget + ";", "加班登记", "", messageContent, "");
                }

                // 关闭本窗体，然后刷新父窗体
                PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

    }
}