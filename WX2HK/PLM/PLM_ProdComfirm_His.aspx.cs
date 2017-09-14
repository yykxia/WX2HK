using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IETCsoft.sql;
using System.Text;
using AspNet = System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json.Linq;
using FineUI;

namespace WX2HK.PLM
{
    public partial class PLM_ProdComfirm_His : System.Web.UI.Page
    {
        private static string queryDate = string.Empty;//查询日期

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DatePicker1.SelectedDate = DateTime.Now;
                bindGrid();
            }
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            queryDate = DatePicker1.Text;
            bindGrid();
        }

        private void bindGrid()
        {
            string sqlCmd = "select t.*,LSWLEX_C9,LSWLZD_GGXH,t1.SCDDCP_JHPCH,SCDDCP_JHTCSL from (";
            sqlCmd += " select scddls,SUM(BoundQty) as BoundQty,ItemNo from ";
            sqlCmd += " ( SELECT t1.*,t2.*,t3.ItemNo FROM PLM_Product_Records t1 left join ";
            sqlCmd += " (select MIN(OrderId) as scddls,ProdId from PLM_Product_Rel group by ProdId) t2";
            sqlCmd += " on t1.OnlineId=t2.ProdId left join PLM_Product_OnLine t3";
            sqlCmd += " on t3.id=t1.OnlineId where BillDate='" + DatePicker1.Text + "') t";
            sqlCmd += " group by scddls,ItemNo) t left join View_ERP_SCDDCP t1 on t1.SCDDCP_LSBH=t.scddls";
            sqlCmd += " left join View_LSWLZD on LSWLZD_WLBH=ItemNo";
            sqlCmd += " left join View_PLM_LSWLEX on LSWLEX_WLBH=ItemNo";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }


        #region Events

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Grid1.Rows.Count > 0)
            {
                string fileName = queryDate + "入库明细";
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid1));
                Response.End();
            }
            else 
            {
                Alert.ShowInTop("无数据可操作！");
            }
        }

        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            //加表头
            sb.Append("<caption align=\"top\">");
            sb.AppendFormat("入库日期:{0}", queryDate);//查询日期
            sb.Append("</caption>");
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

        #endregion
    }
}