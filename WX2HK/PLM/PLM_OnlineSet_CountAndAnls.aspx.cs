using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Text;
using AspNet = System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WX2HK.PLM
{
    public partial class PLM_OnlineSet_CountAndAnls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindLineInfo();
                DatePicker1.SelectedDate = DateTime.Now.AddDays(-1);
                DatePicker2.SelectedDate = DateTime.Now.AddDays(-1);

                //TimePicker1.Text = "12:00";
                //TimePicker2.Text = "12:00";
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
            this.ddl_line.Items.Insert(0, new FineUI.ListItem("全部产线", "%"));
        }

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date1 = Convert.ToDateTime(DatePicker1.Text);
                DateTime date2 = Convert.ToDateTime(DatePicker2.Text);
                string strTimeC = "", endTimeC = "";
                string strTimeD = "", endTimeD = "";
                string onlineStatus = "%";
                if (ckb_onlineStatus.Checked)
                {
                    onlineStatus = "0";
                }
                strTimeD = date1.ToString("yyyy-MM-dd") + " " + "12:00";
                endTimeD = date2.AddDays(1).ToString("yyyy-MM-dd") + " " + "12:00";
                strTimeC = date1.ToString("yyyy-MM-dd");
                endTimeC = date2.AddDays(1).ToString("yyyy-MM-dd");

                string sqlCmd = "select A.*,B.bindTotal,T.orderno,T.plancount,T.itemNo,T.itemParm,T.endTime,T.itemName,(lswlex_c2+'*'+lswlex_c3+'*'+lswlex_c4) as itemParm_size,";
                sqlCmd += " lswlex_c9 as itemParm_weight,lswlex_c5 as itemParm_color,lswlex_c7 as itemParm_sfjz from";
                sqlCmd += " (select isnull(C.tradeNo,D.tradeNo) as tradeNo,isnull(bindSumC,0) as bindSumC,isnull(bindSumCB,0) as bindSumCB,isnull(bindSumCS,0) as bindSumCS,";
                sqlCmd += " isnull(bindSumD,0) as bindSumD,isnull(bindSumDB,0) as bindSumDB,isnull(bindSumDS,0) as bindSumDS,";
                sqlCmd += " (isnull(bindSumC,0)+isnull(bindSumD,0)) as BindSum,(isnull(bindSumCB,0)+isnull(bindSumDB,0)) as BindSumB,(isnull(bindSumCS,0)+isnull(bindSumDS,0)) as BindSumS from ";
                sqlCmd += " (select dd.bindSumD,dd.tradeNo,isnull(ddb.bindSumDB,0) as bindSumDB,(bindSumD - isnull(ddb.bindSumDB,0)) as bindSumDS from ";
                sqlCmd += " (select sum(bindQty) as bindSumD,tradeNo from PLM_Serials_BindBarCode where createTime>'" + strTimeD + "' and createtime<'" + endTimeD + "'";
                sqlCmd += " and excUser='D' and lineId like '" + ddl_line.SelectedValue + "' group by tradeNo) dd";
                sqlCmd += " left join ";
                sqlCmd += " (select sum(bindQty) as bindSumDB,tradeNo from PLM_Serials_BindBarCode where createTime>'" + strTimeD + "' and createtime<'" + endTimeD + "' ";
                sqlCmd += " and excUser='D' and lineId like '" + ddl_line.SelectedValue + "' and barCode like 'B%' group by tradeNo) ddb on dd.tradeNo=ddb.tradeNo) D";
                sqlCmd += " full join ";
                sqlCmd += " (select cc.bindSumC,cc.tradeNo,isnull(ccb.bindSumCB,0) as bindSumCB,(bindSumC - isnull(ccb.bindSumCB,0)) as bindSumCS from ";
                sqlCmd += " (select sum(bindQty) as bindSumC,tradeNo from PLM_Serials_BindBarCode where createTime>'" + strTimeC + "' and createtime<'" + endTimeC + "' ";
                sqlCmd += " and excUser='C' and lineId like '" + ddl_line.SelectedValue + "' group by tradeNo) cc";
                sqlCmd += " left join ";
                sqlCmd += " (select sum(bindQty) as bindSumCB,tradeNo from PLM_Serials_BindBarCode where createTime>'" + strTimeC + "' and createtime<'" + endTimeC + "' ";
                sqlCmd += " and excUser='C' and lineId like '" + ddl_line.SelectedValue + "' and barCode like 'B%' group by tradeNo) ccb on cc.tradeNo=ccb.tradeNo) C";
                sqlCmd += " ON D.tradeNo=C.tradeNo) A";
                sqlCmd += " left join (select sum(bindQty) as bindTotal,tradeno from PLM_Serials_BindBarCode group by tradeNo) B on A.tradeno=B.tradeNo";
                sqlCmd += " left join PLM_Product_OnLine t on a.tradeno=t.id";
                sqlCmd += " left join view_plm_lswlex on lswlex_wlbh=itemNo";
                sqlCmd += " where onlineStatus like '" + onlineStatus + "'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();

                getSummaryData();
            }
            catch (Exception ex) 
            {
                Alert.Show(ex.Message);
            }
        }

        private void getSummaryData()
        {
            DataTable curDt = getDataSource();
            int bindTotal = 0, CTotal = 0, DTotal = 0;
            if (curDt.Rows.Count > 0)
            {
                foreach (DataRow row in curDt.Rows)
                {
                    bindTotal += Convert.ToInt32(row["BindSum"]);
                    CTotal += Convert.ToInt32(row["bindSumC"]);
                    DTotal += Convert.ToInt32(row["bindSumD"]);
                }

                JObject summary = new JObject();
                //summary.Add("major", "全部合计");
                summary.Add("BindSum", bindTotal);
                summary.Add("bindSumC", CTotal);
                summary.Add("bindSumD", DTotal);

                Grid1.SummaryData = summary;
            }
        }

        private DataTable getDataSource()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("tradeNo");
                dt.Columns.Add("orderNo");
                dt.Columns.Add("itemNo");
                dt.Columns.Add("itemName");
                dt.Columns.Add("itemParm_size");
                dt.Columns.Add("planCount");
                dt.Columns.Add("itemParm_weight");
                dt.Columns.Add("itemParm_color");
                dt.Columns.Add("itemParm_sfjz");
                dt.Columns.Add("bindTotal");
                dt.Columns.Add("bindSum");
                dt.Columns.Add("bindSumC");
                dt.Columns.Add("bindSumD");
                dt.Columns.Add("bindSumCS");
                dt.Columns.Add("bindSumDS");
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    GridRow grow = Grid1.Rows[i];
                    dr["tradeNo"] = grow.DataKeys[0].ToString();
                    dr["orderNo"] = grow.Values[0].ToString();
                    dr["itemNo"] = grow.Values[1].ToString();
                    dr["itemName"] = grow.Values[2].ToString();
                    dr["itemParm_size"] = grow.Values[3].ToString();
                    dr["planCount"] = grow.Values[4].ToString();
                    dr["itemParm_weight"] = grow.Values[5].ToString();
                    dr["itemParm_color"] = grow.Values[6].ToString();
                    dr["itemParm_sfjz"] = grow.Values[7].ToString();
                    dr["bindTotal"] = grow.Values[8].ToString();
                    dr["BindSum"] = grow.Values[9].ToString();
                    dr["bindSumC"] = grow.Values[10].ToString();
                    dr["bindSumD"] = grow.Values[11].ToString();
                    dr["bindSumCS"] = grow.Values[12].ToString();
                    dr["bindSumDS"] = grow.Values[13].ToString();
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


        #region Events

        protected void Button1_Click(object sender, EventArgs e)
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
            sb.Append("<caption align=\"top\">");
            sb.AppendFormat("{0}", ddl_line.SelectedText);//导出产线名
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