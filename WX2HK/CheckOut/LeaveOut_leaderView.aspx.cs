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
    public partial class LeaveOut_leaderView : System.Web.UI.Page
    {

        //public string TimeStampStr = "";
        //public string NonceStr = "";
        //public string MsgSigStr = "";

        private static string tabId = string.Empty;
        private static int spSort = 0;
        private static string curWorkNo = string.Empty;//当前用户工号

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                tabId = Request["mainId"];
                spSort =Convert.ToInt32( Request["spsort"]);
                //string pageCode = HttpContext.Current.Request.QueryString["Code"];
                //curWorkNo = WX2HK.ReturnInfo.getWorkNo(pageCode, "CheckOut%2fLeaveOut_leaderView");
                loadHeader(tabId);
                //var code = HttpContext.Current.Request.QueryString["Code"];
                ////
                //TimeStampStr = GetTimeStamp();
                //NonceStr = randNonce();
                //string curUrl = string.Format("http://hkoa.hkfoam.com:30018/CheckOut/LeaveOut_leaderView.aspx?code={0}&state=STATE", code);
                //MsgSigStr = WX2HK.VerifyLegal.createSign(NonceStr, TimeStampStr, curUrl);

            }
        }

        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds).ToString();
        }

        /// <summary>  
        /// 返回一个八位的随机号，用于签名  
        /// </summary>  
        /// <returns></returns>  
        public static string randNonce()
        {
            var result = "";
            var random = new Random((int)DateTime.Now.Ticks);
            const string textArray = "123456789abcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 8; i++)
            {
                result = result + textArray.Substring(random.Next() % textArray.Length, 1);
            }

            return result;
        }  


        public void loadHeader(string mainId)
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select (select name from view_rs_employeeinfo where gh_id=reqman) as name,";
                sqlCmd += "(select bm1 from view_rs_employeeinfo where gh_id=reqman) as bm1,";
                sqlCmd += "(select qqmc from View_Qqlb where qqbh=typ) as qjlb,";
                sqlCmd += "CONVERT(varchar(100), reqDte, 20) as dReqDte,";
                sqlCmd += "CONVERT(varchar(100), leaveDt1, 20) as dleaveDt1,";
                sqlCmd += "CONVERT(varchar(100), leaveDt2, 20) as dleaveDt2,CONVERT(float,days) as fDays,CONVERT(float,hours) as fHours,* from t_leaveReq where id=" + mainId;
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
                    label_days.Text = dt.Rows[0]["fDays"].ToString();
                    label_hours.Text = dt.Rows[0]["fHours"].ToString();
                    label_desc.Text = dt.Rows[0]["leavereason"].ToString();

                    //添加请假人员明细
                    sqlCmd = "select View_Rs_employeeinfo.Name,Rs_QjAndJb_Subdtl.Yhbh from Rs_QjAndJb_Subdtl inner join ";
                    sqlCmd+="View_Rs_employeeinfo on Rs_QjAndJb_Subdtl.Yhbh = View_Rs_employeeinfo.Gh_id where Rs_QjAndJb_Subdtl.MainId='" + mainId + "' and Rs_QjAndJb_Subdtl.Typ = '2'";
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

                    //加载图片信息
                    if (!string.IsNullOrEmpty(dt.Rows[0]["localImgUrl"].ToString()))
                    {
                        //Image_upload.ImageWidth = 550;
                        label_pic.Text = "是";
                        hidden_field.Value = dt.Rows[0]["localImgUrl"].ToString();
                    }
                    else
                    {
                        label_pic.Text = "否";
                        btn_loadImg.Hidden = true;
                    }
                    //是否可以审批
                    if (dt.Rows[0]["stat"].ToString() == "1")
                    {
                        btnSubmit.Hidden = true;//隐藏审批按钮
                    }
                    //
                    //if (haveNoCheckPrivilege(mainId))
                    //{
                    //    btnSubmit.Hidden = true;
                    //}
                }
                else
                {
                    SimpleForm1.Hidden = true;
                    HttpContext.Current.Response.Write("信息内容不存在或非企业内部人员！");
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //判断当前人员是否有页面审批权限
        //private bool haveNoCheckPrivilege(string mainId)
        //{
        //    string sqlCmd = "select WorkNo from T_SpFollow where tabId=" + mainId + " and spTyp = '2'";
        //    string spMan = SqlSel.GetSqlScale(sqlCmd).ToString();//拥有审批权限的工号
        //    //if (spMan != curWorkNo)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //更新审批表
                String sqlCmd = "update T_SpFollow set Checked = '1',CheckeDte = '" + DateTime.Now + "' where tabId = " + tabId + " and spTyp = '2'";
                SqlSel.ExeSql(sqlCmd);

                sqlCmd = "select max(Sort) as maxsort from T_SpFollow where tabId = " + tabId + " and spTyp = '2'";
                int maxSort = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                if (spSort == maxSort)
                {
                    //更新请假单状态
                    sqlCmd = "update T_LeaveReq set Stat = '1' where Id = " + tabId + "";
                    SqlSel.ExeSql(sqlCmd);
                    //Alert.ShowInTop("审批完成！");


                    //将考勤结果反馈给部门考勤员
                    sqlCmd = "select x_user.cellPhone from ";
                    sqlCmd += "(select a.gh_id from view_banzhi_rsRole a inner join  view_rs_employeeinfo b on a.departid=b.departid and  b.gh_id=(select reqman from T_LeaveReq where id=" + tabId + ")) c ";
                    sqlCmd += "left join x_user on x_user.name=c.gh_id";
                    string notifyTarget = SqlSel.GetSqlScale(sqlCmd).ToString();
                    //抄送信息至部门相关考勤员
                    string messageContent = string.Format("请假人员：{0}\\n天数：{3} \\n小时：{4} \\n起始时间：{1} \\n截止时间：{2} ", label_personDtl.Text, label_startTime.Text, label_endTime.Text, label_days.Text, label_hours.Text);
                    if (!string.IsNullOrEmpty(notifyTarget))
                    {
                        WX2HK.ReturnInfo.messagePushByNum(notifyTarget + ";", "请假登记", "", messageContent, "");
                    }
                    //审批按钮隐藏
                    btnSubmit.Hidden = true;
                    //写入考勤数据库 
                    sqlCmd = "INSERT INTO [HKKQ_real].[dbo].[KQ_b_Leave] ";
                    sqlCmd += "([YhBH],[StartRqSj],[EndRqSj],[Hours],[Days],[Cause],[Memo],[czy]) ";
                    sqlCmd += "(select yhbh,leavedt1,leavedt2,hours,days,T_LeaveReq.typ,addfunc,reqman from T_LeaveReq,rs_qjandjb_subdtl ";
                    sqlCmd += "where T_LeaveReq.id= " + tabId + " and  rs_qjandjb_subdtl.typ=2 and rs_qjandjb_subdtl.mainid= " + tabId + ")";
                    SqlSel.ExeSql(sqlCmd);
                }
                else
                {
                    sqlCmd = "update T_LeaveReq set Stat = '10' where Id = " + tabId + "";
                    SqlSel.ExeSql(sqlCmd);
                    //Alert.ShowInTop("审批完成！");
                }

                // 关闭本窗体，然后刷新父窗体
                PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());


            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }

        }
        //加载图片
        protected void btn_loadImg_Click(object sender, EventArgs e)
        {
            if (label_pic.Text == "是")
            {
                string imgUrl = "../image/" + hidden_field.Value;
                labResult.Text = "<p><img src=\"" + imgUrl + "\" /></p>";
            }

        }



    }
}