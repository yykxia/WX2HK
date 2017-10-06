using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;

namespace WX2HK.PLM
{
    public partial class PLM_classInit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初    
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末

                DatePicker1.SelectedDate = startMonth;
                DatePicker2.SelectedDate = endMonth;
            }
        }

        protected void btn_create_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = string.Empty;
                DateTime startDate = Convert.ToDateTime(DatePicker1.Text);//起始日期
                DateTime endDate = Convert.ToDateTime(DatePicker2.Text);//结束日期
                for (DateTime eachDay = startDate;
                    DateTime.Compare(eachDay, endDate) <= 0; eachDay = eachDay.AddDays(1))
                {
                    string dateStr = eachDay.ToString("yyyy-MM-dd");//转换成短日期格式
                    sqlCmd = "insert into PLM_classInit (InitDay,CheckFlag) values ('" + dateStr + "','0')";
                    SqlSel.ExeSql(sqlCmd);
                }

                bindGrid();
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBoxField field1 = (CheckBoxField)Grid1.FindColumn("CheckBoxField1");
                foreach (GridRow gr in Grid1.Rows)
                {
                    int rowIndex = gr.RowIndex;
                    bool isSel = field1.GetCheckedState(rowIndex);
                    string rq = gr.Values[0].ToString();//日期
                    int checkStatus = 0;
                    if(isSel)
                    {
                        checkStatus=1;
                    }
                    SqlSel.ExeSql("update PLM_classInit set CheckFlag='" + checkStatus + "' where InitDay='" + rq + "'");
                }

                bindGrid();

                Alert.Show("保存成功！");
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }
        //刷新数据
        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            try 
            {
                bindGrid();
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }
        //重新绑定数据
        private void bindGrid()
        {
            string sqlCmd = "select * from PLM_classInit where CONVERT(datetime,InitDay,101) between";
            sqlCmd += " '" + DatePicker1.Text + "' and '" + DatePicker2.Text + "' order by CONVERT(datetime,InitDay,101)";
            DataTable dt = null;
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else
            {
                Grid1.DataSource = dt;
                Grid1.DataBind();
                Alert.Show("无数据可以显示！");
            }
        }
    }
}