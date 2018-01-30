using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using IETCsoft.sql;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

namespace WX2HK.PLM_WareHouse
{
    public partial class WH_TreatCount_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatePicker1.SelectedDate = DateTime.Now.AddDays(-1);
                DatePicker2.SelectedDate = DateTime.Now;
                TimePicker1.Text = "00:00";
                TimePicker2.Text = "00:00";
            }
        }

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            try
            {
                string SelGroup = ddl_group.SelectedValue;
                string SelClass = ddl_class.SelectedValue;
                if (SelClass == "T")
                {
                    SelGroup = "A";
                }

                if (!string.IsNullOrEmpty(SelGroup) & !string.IsNullOrEmpty(SelClass))
                {
                    //获取起始和截至时间字符串
                    string dt1 = string.Format("{0} {1}", DatePicker1.Text, TimePicker1.Text);
                    string dt2 = string.Format("{0} {1}", DatePicker2.Text, TimePicker2.Text);
                    //转换成时间格式
                    DateTime sDt = Convert.ToDateTime(dt1);
                    DateTime eDt = Convert.ToDateTime(dt2);
                    //统计剪边数据
                    string sqlCmd = "SELECT SUM(BindQty) AS BindQty,SUM(SQty) AS SQty,(SUM(BindQty)-SUM(SQty)) AS BQty,";
                    sqlCmd += " ItemNo,ItemParm,OrderNo,TradeNo,workerNo FROM (";
                    sqlCmd += " select BindQty,ItemNo,ItemParm,OrderNo,TradeNo,(CASE WHEN BarCode LIKE 'B%' THEN 0 ELSE BindQty END) AS SQty";
                    sqlCmd += " ,workerNo from PLM_WH_TreatRecord_B t1 left join ";
                    sqlCmd += " PLM_Serials_BindBarCode t2 on t1.RecordId=t2.id left join PLM_Product_OnLine t3 on t3.Id=t2.TradeNo";
                    sqlCmd += " where t1.TreatTime>'" + sDt + "' and t1.TreatTime<'" + eDt + "' and TreatStatus='0'";
                    sqlCmd += " and t1.TreatType='1' and TreatClass='" + ddl_class.SelectedValue + "' and";
                    sqlCmd += " TreatGroup='" + ddl_group.SelectedValue + "') T group by TradeNo,ItemNo,ItemParm,OrderNo,workerNo ORDER BY workerNo";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();

                    getSummaryData(dt);
                }
                else
                {
                    Alert.ShowInTop("班次或班组信息不可为空！");
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        private void getSummaryData(DataTable curDt)
        {
            int bindTotal = 0;
            int bindTotal_S = 0;
            int bindTotal_B = 0;
            if (curDt.Rows.Count > 0)
            {
                foreach (DataRow row in curDt.Rows)
                {
                    bindTotal += Convert.ToInt32(row["BindQty"]);
                    bindTotal_S += Convert.ToInt32(row["SQty"]);
                    bindTotal_B += Convert.ToInt32(row["BQty"]);
                }

                JObject summary = new JObject();
                //summary.Add("ItemParm", "全部合计");
                summary.Add("BindQty", bindTotal);
                summary.Add("SQty", bindTotal_S);
                summary.Add("BQty", bindTotal_B);

                Grid1.SummaryData = summary;
            }
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            string fileName = DateTime.Now.Ticks.ToString();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGridTableHtml(Grid1));
            Response.End();
        }

        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            //加表头
            //sb.Append("<caption align=\"top\">");
            //sb.AppendFormat("{0}", ddl_line.SelectedText);//导出产线名
            //sb.Append("</caption>");
            //sb.Append("<thead><tr>");
            //sb.AppendFormat("<td>{0}</td>", ddl_line.SelectedText);
            //sb.Append("</tr></thead>");
            sb.Append("<tr>");
            foreach (GridColumn column in grid.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");


            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");
                foreach (object value in row.Values)
                {
                    string html = value.ToString();
                    if (html.StartsWith(Grid.TEMPLATE_PLACEHOLDER_PREFIX))
                    {
                        // 模板列
                        string templateID = html.Substring(Grid.TEMPLATE_PLACEHOLDER_PREFIX.Length);
                        Control templateCtrl = row.FindControl(templateID);
                        html = GetRenderedHtmlSource(templateCtrl);
                    }
                    else
                    {
                        // 处理CheckBox
                        if (html.Contains("f-grid-static-checkbox"))
                        {
                            if (html.Contains("uncheck"))
                            {
                                html = "×";
                            }
                            else
                            {
                                html = "√";
                            }
                        }

                        // 处理图片
                        if (html.Contains("<img"))
                        {
                            string prefix = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
                            html = html.Replace("src=\"", "src=\"" + prefix);
                        }
                    }

                    sb.AppendFormat("<td>{0}</td>", html);
                }
                sb.Append("</tr>");
            }
            //合计行导出
            sb.Append("<tr>");
            JObject summarty = grid.SummaryData;//获取合计行数据
            if (summarty != null && summarty.ToString() != "")//判断合计行数据是否为空
            {
                foreach (GridColumn column in grid.Columns)//遍历出列的id
                {
                    if (summarty.Property(column.ColumnID.ToString()) == null || summarty.Property(column.ColumnID.ToString()).ToString() == "")//判断合计行Json是否存在该节点
                    {
                        sb.AppendFormat("<td>{0}</td>", "");//如果没有就为空
                    }
                    else
                    {
                        sb.AppendFormat("<td>{0}</td>", summarty[column.ColumnID.ToString()].ToString());//如果有就写入数据
                    }
                }
            }
            sb.Append("</tr>");

            sb.Append("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// 获取控件渲染后的HTML源代码
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private string GetRenderedHtmlSource(Control ctrl)
        {
            if (ctrl != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        ctrl.RenderControl(htw);

                        return sw.ToString();
                    }
                }
            }
            return String.Empty;
        }


    }
}