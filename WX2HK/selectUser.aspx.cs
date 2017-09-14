using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;
using FineUI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace WX2HK
{
    public partial class selectUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindGrid2();
            }
        }

        protected void tgbox1_TriggerClick(object sender, EventArgs e)
        {
            DataTable curDt = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(tgbox1.Text))
                {
                    curDt = getUserList();
                    string inputStr = tgbox1.Text;
                    DataRow[] drArr = curDt.Select("name LIKE '%" + inputStr + "%'");

                    DataTable dtNew = curDt.Clone();
                    for (int i = 0; i < drArr.Length; i++)
                    {
                        dtNew.ImportRow(drArr[i]);
                    }

                    Grid2.DataSource = dtNew;
                    Grid2.DataBind();
                }
                else 
                {
                    Alert.Show("查询条件不能为空！");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void bindGrid2()
        {
            Grid2.DataSource = getUserList();
            Grid2.DataBind();
        }

        //获取成员列表
        public static DataTable getUserList()
        {
            try
            {
                DataTable dt = new DataTable();
                string accessToken = VerifyLegal.GetAccess_Token();
                //department_id=10：协同办公平台所创建的企业号所有人员列表；status=1：状态为已关注的人员；fetch_child=1：递归获取所有子集部门人员
                string wxUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id=1&fetch_child=1&status=1", accessToken);
                var client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                var data = client.DownloadString(wxUrl);
                //DataTable userList = JsonToDataTable(data);
                //解析json
                JavaScriptSerializer js = new JavaScriptSerializer();
                Rootobject returnObj = js.Deserialize<Rootobject>(data);
                int errcode = returnObj.errcode;
                if (errcode == 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    js.Serialize(returnObj.userlist, sb);
                    List<Userlist> curUserList = new List<Userlist>();
                    curUserList = JSONStringToList<Userlist>(sb.ToString());
                    dt = ToDataTable(curUserList);
                    return dt;
                }
                else
                {
                    Alert.Show(string.Format("errmsg:{0}", returnObj.errmsg));
                    return null;
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return null;
            }
        }

        /// <summary>   
        /// Json转DataTable   
        /// </summary>   
        /// <param name="json"></param>   
        /// <returns></returns>   
        //public static DataTable Json2Dtb(string json)
        //{
        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    ArrayList dic = jss.Deserialize<ArrayList>(json);
        //    DataTable dtb = new DataTable();
        //    if (dic.Count > 0)
        //    {
        //        foreach (Dictionary<string, object> drow in dic)
        //        {
        //            if (dtb.Columns.Count == 0)
        //            {
        //                foreach (string key in drow.Keys)
        //                {
        //                    dtb.Columns.Add(key, drow[key].GetType());
        //                }
        //            }
        //            DataRow row = dtb.NewRow();
        //            foreach (string key in drow.Keys)
        //            {
        //                row[key] = drow[key];
        //            }
        //            dtb.Rows.Add(row);
        //        }
        //    }

        //    return dtb;
        //}


        public class Rootobject
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public Userlist[] userlist { get; set; }
        }

        public class Userlist
        {
            public string userid { get; set; }
            public string name { get; set; }
            public int[] department { get; set; }
            public string mobile { get; set; }
            public string gender { get; set; }
            public string email { get; set; }
            public string weixinid { get; set; }
            public string avatar { get; set; }
            public int status { get; set; }
        }

        #region json字符串转对象集合
        /// <summary>
        /// json字符串转对象集合
        /// </summary>
        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
        #endregion

        //实体类转datatable
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            Type elementType = typeof(T);

            var t = new DataTable();

            elementType.GetProperties().ToList().ForEach(propInfo => t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            foreach (T item in list)
            {
                var row = t.NewRow();
                elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
                t.Rows.Add(row);
            }
            return t;
        }

        //点击添加至已选列表
        protected void Grid2_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                DataTable newDt = new DataTable();
                newDt = GetDataTable();
                DataRow dr = newDt.NewRow();
                string userid = Grid2.DataKeys[e.RowIndex][0].ToString();
                //判断人员是否已添加
                int find = -1;
                for (int i = 0; i < newDt.Rows.Count; i++)
                {
                    if (newDt.Rows[i]["userid"].ToString() == userid) 
                    {
                        find = i;
                    }
                }
                if (find < 0)
                {
                    dr[0] = userid;
                    dr[1] = Grid2.Rows[e.RowIndex].Values[0];
                    newDt.Rows.Add(dr);
                    Grid1.DataSource = newDt;
                    Grid1.DataBind();
                }
                else 
                {
                    Alert.ShowInTop("人员已存在！");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //获取当前已添加人员表
        protected DataTable GetDataTable()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("userid", typeof(string));
                dt.Columns.Add("name", typeof(string));
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    GridRow grow = Grid1.Rows[i];
                    dr[0] = grow.DataKeys[0].ToString();
                    dr[1] = grow.Values[0].ToString();
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

        //移除已选人员
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int rowID = Grid1.SelectedRowIndex;
                    DataTable curDtb = GetDataTable();
                    curDtb.Rows.RemoveAt(rowID);
                    Grid1.DataSource = curDtb;
                    Grid1.DataBind();
                    //Alert.ShowInTop("已删除!");
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void btn_return_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        protected void btn_sub_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    String userId = Grid1.DataKeys[i][0].ToString();

                    String selectedName = Grid1.Rows[i].Values[0].ToString();

                    sb.AppendFormat("@{0}-{1};", userId, selectedName);

                }
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(sb.ToString()) + ActiveWindow.GetHideReference());
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

    }
}