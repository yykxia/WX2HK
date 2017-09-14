using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IETCsoft.sql;
using System.Data;
using FineUI;
using System.Net;

namespace WX2HK.CheckOut
{
    public partial class LeaveOut : System.Web.UI.Page
    {
        static string workNo = "";
        public string TimeStamp = "";
        public string Nonce = "";
        public string MsgSig = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //
                string code = string.Empty;
                code = HttpContext.Current.Request.QueryString["Code"];
                string curPageName = "CheckOut%2fLeaveOut";
                string userid = WX2HK.ReturnInfo.GetUserId(code, curPageName);
                //string curUserMobile = WX2HK.ReturnInfo.GetUserInfo(userid);
                workNo = WX2HK.ReturnInfo.getLoginId(userid);
                loadHeader();
                //
                TimeStamp = GetTimeStamp();
                Nonce = randNonce();
                string curUrl = string.Format("http://hkoa.hkfoam.com:30018/CheckOut/LeaveOut.aspx?code={0}&state=STATE", code);
                MsgSig = WX2HK.VerifyLegal.createSign(Nonce, TimeStamp, curUrl);
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
        //
        public void loadHeader()
        {
            //限制日期选择范围（限当月内选取）
            DateTime curDt = DateTime.Now;
            //起始日期
            DatePicker1.MinDate = new DateTime(curDt.Year, curDt.Month, 1);
            //DatePicker1.MaxDate = curDt.AddDays(1 - curDt.Day).AddMonths(1).AddDays(-1);
            //截止日期
            //DatePicker2.MinDate = new DateTime(curDt.Year, curDt.Month, 1);
            //DatePicker2.MaxDate = curDt.AddDays(1 - curDt.Day).AddMonths(1).AddDays(-1);

            DataTable dt = new DataTable();
            string sqlCmd = "select * from view_rs_employeeinfo where gh_id='" + workNo + "'";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                label_dept.Text = dt.Rows[0]["bm1"].ToString();
                label_name.Text = dt.Rows[0]["name"].ToString();

                sqlCmd = "select * from View_Qqlb";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                RadioButtonList_goal.DataTextField = "qqmc";
                RadioButtonList_goal.DataValueField = "qqbh";
                RadioButtonList_goal.DataSource = dt;
                RadioButtonList_goal.DataBind();

                RadioButtonList_goal.SelectedValue = "03";

                TextArea_desc.Text = MsgSig;
            }
            else 
            {
                SimpleForm1.Hidden = true;
                HttpContext.Current.Response.Write("信息不存在或非企业内部人员！");
            }
        }

        //
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //限当月提交
                //int curMonth = DateTime.Now.Month;
                //int subMonth = Convert.ToDateTime(DatePicker1.Text).Month;
                //if (subMonth != curMonth)
                //{
                //    Alert.ShowInTop("请假申请仅限当月提交");
                //    return;
                //}
                //补充说明不可超出数据库字段长度限制
                if (TextArea_desc.Text.Length > 500)
                {
                    Alert.ShowInTop("补充说明字符超限！");
                    return;
                }
                string date1 = DatePicker1.Text + " " + TimePicker1.Text;
                string date2 = DatePicker2.Text + " " + TimePicker2.Text;
                decimal days, hours;
                string localImg = "";
                //请假天数
                if (string.IsNullOrEmpty(numbbox_days.Text))
                {
                    days = 0;
                }
                else 
                {
                    days = Convert.ToDecimal(numbbox_days.Text);
                }
                //请假时间
                if (string.IsNullOrEmpty(numbbox_hours.Text))
                {
                    hours = 0;
                }
                else 
                {
                    hours = Convert.ToDecimal(numbbox_hours.Text);
                }
                //将图片保存至本地服务器
                if (!string.IsNullOrEmpty(hidden_field.Value))
                {
                    string access_token = VerifyLegal.GetAccess_Token();
                    localImg = GetMultimedia(access_token, hidden_field.Value);

                }
                string sqlCmd = "insert into T_LeaveReq (Typ,ReqMan,ReqDte,LeaveReason,LeaveDt1,LeaveDt2,Stat,Days,Hours,AddFunc,wxImgServId,localImgUrl) values ";
                sqlCmd += "('" + RadioButtonList_goal.SelectedValue + "','" + workNo + "','" + DateTime.Now + "','" + TextArea_desc.Text + "','" + date1 + "','" + date2 + "',0,";
                sqlCmd += "'" + days + "','" + hours + "',3,'" + hidden_field.Value + "','" + localImg + "')";
                if (SqlSel.ExeSql(sqlCmd) == 0)
                {
                    Alert.ShowInTop(sqlCmd, "执行出错！");
                    return;
                }
                else 
                {
                    sqlCmd = "select max(Id) from T_LeaveReq where reqman='" + workNo + "'";
                    int mainId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                    sqlCmd = "insert into Rs_QjAndJb_Subdtl (yhbh,Typ,MainId) values ('" + workNo + "','2'," + mainId + ")";
                    SqlSel.ExeSql(sqlCmd);
                    //插入审批表
                    sqlCmd = "select parent_Gh from View_Rs_employeeinfo where Gh_id = '" + workNo + "'";
                    if (string.IsNullOrEmpty(SqlSel.GetSqlScale(sqlCmd).ToString())) 
                    {
                        sqlCmd = "update T_LeaveReq set stat=-1 where id=" + mainId;
                        SqlSel.ExeSql(sqlCmd);
                        Alert.ShowInTop("您的直属上级没有设置，无法填写请假申请，请联系人事部门。");
                        return;
                    }
                    String spMan = SqlSel.GetSqlScale(sqlCmd).ToString();
                    sqlCmd = "insert into T_SpFollow (spTyp,tabId,WorkNo,Checked,Sort) values ('2'," + mainId + ",'" + spMan + "','0','1')";
                    SqlSel.ExeSql(sqlCmd);
                    //超过5天的请假多层审批
                    //if (days > 5)
                    //{
                    //    sqlCmd = "select parent_Gh from View_Rs_employeeinfo where Gh_id = '" + spMan + "'";
                    //    if (string.IsNullOrEmpty(SqlSel.GetSqlScale(sqlCmd).ToString()))
                    //    {
                    //        //无上上级领导直接跳过
                    //    }
                    //    else
                    //    {
                    //        String spMan2 = SqlSel.GetSqlScale(sqlCmd).ToString();
                    //        sqlCmd = "insert into T_SpFollow (spTyp,tabId,WorkNo,Checked,Sort) values ('2'," + mainId + ",'" + spMan2 + "','0','2')";
                    //        SqlSel.ExeSql(sqlCmd);
                    //    }
                    //}
                    //向对象推送相关消息
                    sqlCmd = "select CellPhone from x_user where name='" + spMan + "'";
                    string targetNum = SqlSel.GetSqlScale(sqlCmd).ToString();
                    if (!string.IsNullOrEmpty(targetNum))
                    {
                        string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx3068893beff3a241&redirect_uri=http%3a%2f%2fhkoa.hkfoam.com%3a30018%2fCheckOut%2fLeaveOut_leaderView.aspx?mainId={0}&response_type=code&scope=SCOPE&state=STATE#wechat_redirect",mainId);
                        WX2HK.ReturnInfo.messagePushByNum(targetNum + ";", "请假单", url, string.Format("来自 {0}的请假申请", label_name.Text), "");
                    }
                    Alert.ShowInTop("提交成功！");
                    //提交按钮隐藏
                    btnSubmit.Hidden = true;
                    //表单重置
                    PageContext.RegisterStartupScript(SimpleForm1.GetResetReference());
                    //Image1.ImageUrl = null;
                    hidden_field.Value = null;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btn_selPushUsers_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(Window1.GetSaveStateReference(TextArea_userList.ClientID)
            //        + Window1.GetShowReference("../selectUser.aspx") + Window1.GetMaximizeReference());

            ////窗口最大化
            //PageContext.RegisterStartupScript(Window1.GetShowReference() + Window1.GetMaximizeReference());

        }

        //测试推送
        protected void btn_test_Click(object sender, EventArgs e)
        {
            //将图片保存至本地服务器
            if (hidden_field.Value != null)
            {
                string access_token = VerifyLegal.GetAccess_Token();
                GetMultimedia(access_token, hidden_field.Value);

            }

            //string num = "";
            //WX2HK.ReturnInfo.messagePushByNum(num, "良辰在此", "", "你们这些渣渣~！", "");
        }

        protected void btn_confirmImg_Click(object sender, EventArgs e)
        {
            string wxServerId = hidden_field.Value;
            Alert.Show(wxServerId);
        }

        /// <SUMMARY> 
        /// 下载保存多媒体文件,返回多媒体保存路径 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="MEDIA_ID"></PARAM> 
        /// <RETURNS></RETURNS> 
        public string GetMultimedia(string ACCESS_TOKEN, string MEDIA_ID)
        {
            string fileName = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                strpath = myResponse.ResponseUri.ToString();
                WebClient mywebclient = new WebClient();
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                savepath = Server.MapPath("/image") + "\\" + fileName;
                //savepath = MapPath("file") + "\\" + "uplaodFile" + "\\" + fileName;
                //Uri uriPath = new Uri("http://192.168.4.253:8810/AddtionFile/uploadFile");
                //savepath = "http://192.168.4.253:8810/AddtionFile/uploadFile" + "\\" + fileName;
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    //fileName = savepath;
                }
                catch (Exception ex)
                {
                    Alert.ShowInTop(ex.Message);
                }

            }
            //Alert.Show(file);
            return fileName; 

        }

    }
}