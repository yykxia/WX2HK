using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using IETCsoft.sql;
using FineUI;

namespace WX2HK.others
{
    public partial class barCodePrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatePicker1.SelectedDate = DateTime.Now;
                LoadData();
            }
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            try
            {

                //更新当前日期打印数
                int afterPtintedNumb = Convert.ToInt32(hiddenValue.Text) + Convert.ToInt32(nmb_printCount.Text) - 1;
                string sqlCmd = "update DZ_barCode set curNumb='" + afterPtintedNumb + "' where printDate='" + DatePicker1.Text + "'";
                if (SqlSel.ExeSql(sqlCmd) == 0)
                {
                    sqlCmd = "insert into DZ_barCode (printDate,curNumb) values ('" + DatePicker1.Text + "','" + afterPtintedNumb + "')";
                    SqlSel.ExeSql(sqlCmd);
                }

                Alert.Show("打印成功！");

                PageContext.RegisterStartupScript("print()");
                


            } catch(Exception ex)
            {
                Alert.Show(ex.Message);
            }
        }

        protected void DatePicker1_DateSelect(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        private void LoadData()
        {
            int curNumb = 0;
            string sqlCmd = "select * from DZ_barCode where printDate='" + DatePicker1.Text + "'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                curNumb = Convert.ToInt32(dt.Rows[0]["curNumb"]) + 1;
            }
            else
            {
                curNumb = 1;
            }

            string curBarCode = Convert.ToDateTime(DatePicker1.Text).ToString("yyyyMMdd") + curNumb.ToString("000");
            txb_curBarCode.Text = curBarCode;

            hiddenValue.Text = curNumb.ToString();
        }
    }
}