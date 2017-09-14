using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using FineUI;
using System.Text;
using AspNet = System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WX2HK.MES
{
    public partial class MES_BomList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //根据成品编码查询
        protected void btn_query_Click(object sender, EventArgs e)
        {
            try 
            {
                MESWebservice MesWeb = new MESWebservice();
                DataTable dt = MesWeb.MES_Bom(txb_cpCode.Text);
                //bom清单
                if (dt != null) 
                {
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                }
                //物料清单
                DataTable wlDt = MesWeb.MES_Matrials(txb_cpCode.Text);
                if (wlDt != null)
                {
                    Grid2.DataSource = wlDt;
                    Grid2.DataBind();
                }
                //物料版本
                DataTable wlbbDt = MesWeb.MES_MatrialsVersion(txb_cpCode.Text);
                if (wlbbDt != null)
                {
                    Grid3.DataSource = wlbbDt;
                    Grid3.DataBind();
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        //根据订单号查询
        protected void btn_orderQuery_Click(object sender, EventArgs e)
        {
            try
            {
                MESWebservice MesWeb = new MESWebservice();
                //bom清单
                DataTable bomDt = MesWeb.MES_Bom_order(txb_orderNo.Text);
                if (bomDt != null)
                {
                    Grid1.DataSource = bomDt;
                    Grid1.DataBind();
                }
                //物料清单
                DataTable wlDt = MesWeb.MES_MatrialsList(txb_orderNo.Text);
                if (wlDt != null) 
                {
                    Grid2.DataSource = wlDt;
                    Grid2.DataBind();
                }
                //物料版本
                DataTable wlbbDt = MesWeb.MES_MatrialsVersion_order(txb_orderNo.Text);
                if (wlbbDt != null)
                {
                    Grid3.DataSource = wlbbDt;
                    Grid3.DataBind();
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        #region Events

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Grid1.Rows.Count > 0)
            {
                string fileName = DateTime.Now.Ticks.ToString() + "_" + "Bom维护";
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid1));
                Response.End();
            }

        }

        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            //加表头
            //sb.Append("<caption align=\"top\">");
            //sb.AppendFormat("{0}", tableName);//
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
            //sb.Append("<tr>");
            //JObject summarty = grid.SummaryData;//获取合计行数据
            //if (summarty != null && summarty.ToString() != "")//判断合计行数据是否为空
            //{
            //    foreach (GridColumn column in grid.Columns)//遍历出列的id
            //    {
            //        if (summarty.Property(column.ColumnID.ToString()) == null || summarty.Property(column.ColumnID.ToString()).ToString() == "")//判断合计行Json是否存在该节点
            //        {
            //            sb.AppendFormat("<td>{0}</td>", "");//如果没有就为空
            //        }
            //        else
            //        {
            //            sb.AppendFormat("<td>{0}</td>", summarty[column.ColumnID.ToString()].ToString());//如果有就写入数据
            //        }
            //    }
            //}
            //sb.Append("</tr>");

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

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (Grid2.Rows.Count > 0)
            {
                string fileName = DateTime.Now.Ticks.ToString() + "_" + "物料维护";
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid2));
                Response.End();
            }

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            if (Grid3.Rows.Count > 0)
            {
                string fileName = DateTime.Now.Ticks.ToString() + "_" + "物料版本维护";
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid3));
                Response.End();
            }
        }
    }
}