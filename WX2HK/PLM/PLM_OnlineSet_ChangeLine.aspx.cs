using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;
using FineUI;

namespace WX2HK.PLM
{
    public partial class PLM_OnlineSet_ChangeLine : System.Web.UI.Page
    {
        static string orderId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                orderId = HttpContext.Current.Request.QueryString["OnlineId"];
                bindLineInfo();

                //当前产线
                Label1.Text = currentLine(orderId);
            }
        }

        //加载产线信息
        private void bindLineInfo()
        {
            string sqlCmd = "select * from PLM_Product_Line where LineStatus=1";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            ddl_line.DataValueField = "id";
            ddl_line.DataTextField = "LineName";
            ddl_line.DataSource = dt;
            ddl_line.DataBind();
            this.ddl_line.Items.Insert(0, new FineUI.ListItem("请选择", "%"));
        }

        //当前订单产线
        private string currentLine(string orderId) 
        {
            string sqlCmd = "select lineName from PLM_Product_OnLine left join PLM_Product_Line on PLM_Product_Line.id=PLM_Product_OnLine.lineId where PLM_Product_OnLine.id='" + orderId + "'";
            return SqlSel.GetSqlScale(sqlCmd).ToString();
        }
        protected void btn_ok_Click(object sender, EventArgs e)
        {
            if (ddl_line.SelectedValue == "%") 
            {
                Alert.ShowInTop("请选择调整后的产线！");
                return;
            }
            string sqlCmd = "update PLM_Product_OnLine set lineId='" + ddl_line.SelectedValue + "' where id='" + orderId + "'";
            SqlSel.ExeSql(sqlCmd);

            Alert.Show("已调整产线！");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            
        }

        protected void btn_exit_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

    }
}