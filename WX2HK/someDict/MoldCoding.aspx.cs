using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using FineUI;
using IETCsoft.sql;

namespace WX2HK.someDict
{
    public partial class MoldCoding : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
            //    // 删除Grid1选中单元格的客户端脚本
            //    string deleteScript1 = GetDeleteScript(Grid1);
            //    // 新增数据初始值
            //    JObject defaultObj = new JObject();
            //    defaultObj.Add("id", getMaxId());
            //    defaultObj.Add("MJTypeName", "");
            //    defaultObj.Add("MJTypeCode", "");
            //    defaultObj.Add("Delete1", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>", deleteScript1, IconHelper.GetResolvedIconUrl(Icon.Delete)));

            //    // 在第一行新增一条数据
            //    btnNew.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, true);
                // 在第一行新增一条数据
                //Button1.OnClientClick = Grid2.GetAddNewRecordReference(defaultObj2, false);
                bindGrid1();
            }
        }

        //模拟数据库id增长列
        protected int getMaxId2()
        {
            int maxId;
            if (string.IsNullOrEmpty(label_hidden2.Text))
            {
                maxId = 0;
            }
            else
            {
                maxId = Convert.ToInt32(label_hidden2.Text) + 1;
            }

            label_hidden2.Text = maxId.ToString();
            return maxId;

        }
        private void bindGrid1() 
        {
            string sqlCmd = "select * from Dict_MJCoding_TopType order by MJTypeName";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();

            sqlCmd = "select max(id) from Dict_MJCoding_TopType";
            string curMaxId = SqlSel.GetSqlScale(sqlCmd).ToString();
            if (string.IsNullOrEmpty(curMaxId))
            {
                label_hidden.Text = "0";
            }
            else
            {
                label_hidden.Text = curMaxId;
            }
        }

        protected void fileData_FileSelected(object sender, EventArgs e)
        {
            if (hasEditData(Grid2)) 
            {
                Alert.ShowInTop("存在未保存数据，请先保存后再操作！");
            }
            else
            {
                if (Grid1.SelectedRowIndex >= 0)
                {
                    string fileName = fileData.ShortFileName;
                    //获取文件格式
                    string filetyp = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    if (!isValidType(filetyp))
                    {
                        fileData.Reset();
                        Alert.ShowInTop("文件格式无效！");
                        return;
                    }
                    else
                    {
                        fileName = DateTime.Now.Ticks.ToString() + "." + filetyp;
                        string filePath = "~/upload/" + fileName;
                        fileData.SaveAs(Server.MapPath(filePath));//上传文件
                        DataTable uploadData = new DataTable();
                        uploadData = InputExcel(filePath);//得到excel数据

                        //判断列数是否符合
                        if (uploadData.Columns.Count != 7)
                        {
                            Alert.ShowInTop("导入文件格式不符，请校验！");
                            return;
                        }
                        else
                        {
                            //插入数据
                            string typeId = Grid1.Rows[Grid1.SelectedRowIndex].Values[1].ToString();
                            uploadData.Columns.Add("TopTypeId");
                            uploadData.Columns.Add("batchNo");
                            foreach (DataRow dr in uploadData.Rows)
                            {
                                dr["TopTypeId"] = typeId;
                                dr["batchNo"] = "01";
                                InsertData(dr["TopTypeId"].ToString(), dr["Length"].ToString(), dr["LenCode"].ToString(), dr["Width"].ToString()
                                    , dr["WidCode"].ToString(), dr["Height"].ToString(), dr["HeiCode"].ToString()
                                    , Convert.ToInt32(dr["StockQty"]), dr["batchNo"].ToString());
                            }

                            //重新刷新数据
                            BindGrid2(typeId);
                            ////获取现有Grid2参数信息
                            //DataTable curDt = getData();
                            //foreach (DataRow dr in uploadData.Rows)
                            //{
                            //    curDt.ImportRow(dr);
                            //}

                            //Grid2.DataSource = curDt;
                            //Grid2.DataBind();
                        }


                    }
                }
                else 
                {
                    Alert.ShowInTop("请先选择相应的大类后再导入!");
                }
            }
        }

        //加载Grid2数据
        private void BindGrid2(string typeId) 
        {
            string sqlCmd = "select * from Dict_MJCoding_Parm where TopTypeId='" + typeId + "' order by Length,Width,Height";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();

            sqlCmd = "select max(id) from Dict_MJCoding_Parm";
            string curMaxId = SqlSel.GetSqlScale(sqlCmd).ToString();
            if (string.IsNullOrEmpty(curMaxId))
            {
                label_hidden2.Text = "0";
            }
            else
            {
                label_hidden2.Text = curMaxId;
            }

            //编码合成
            DataTable unionCodeDt = new DataTable();
            unionCodeDt.Columns.Add("unionCode");
            foreach (DataRow dr in dt.Rows) 
            {
                DataRow codeRow = unionCodeDt.NewRow();
                codeRow["unionCode"] = "MJ-" + dr["TopTypeId"].ToString() + "-" + dr["LenCode"].ToString() +
                    dr["WidCode"].ToString() + dr["HeiCode"].ToString() + "-" + dr["batchNo"].ToString();
                unionCodeDt.Rows.Add(codeRow);
            }
            Grid3.DataSource = unionCodeDt;
            Grid3.DataBind();
        }

        //
        private void InsertData(string typeId, string Length, string LenCode, string Width, string WidCode, string Height, string HeiCode, int StockQty, string batchNo) 
        {
            string sqlCmd = "insert into Dict_MJCoding_Parm (TopTypeId,Length,LenCode,Width,WidCode,Height,HeiCode,StockQty,batchNo) values ";
            sqlCmd += "('" + typeId + "','" + Length + "','" + LenCode + "','" + Width + "','" + WidCode + "','" + Height + "',";
            sqlCmd += "'" + HeiCode + "','" + StockQty + "','" + batchNo + "')";
            SqlSel.ExeSql(sqlCmd);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                // 新增数据
                List<Dictionary<string, object>> newAddedList = Grid2.GetNewAddedList();
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    InsertData(Grid1.Rows[Grid1.SelectedRowIndex].Values[1].ToString(), newAddedList[i]["Length"].ToString(), newAddedList[i]["LenCode"].ToString(),
                        newAddedList[i]["Width"].ToString(), newAddedList[i]["WidCode"].ToString(), newAddedList[i]["Height"].ToString(),
                        newAddedList[i]["HeiCode"].ToString(), Convert.ToInt32(newAddedList[i]["StockQty"]), newAddedList[i]["batchNo"].ToString());
                }

                // 修改的现有数据
                Dictionary<int, Dictionary<string, object>> modifiedDict = Grid2.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {
                    int rowID = Convert.ToInt32(Grid2.DataKeys[rowIndex][0]);
                    GridRow gr = Grid2.Rows[rowIndex];
                    string sqlCmd = "update Dict_MJCoding_Parm set Length='" + gr.Values[2].ToString() + "',LenCode='" + gr.Values[3].ToString() + "',";
                    sqlCmd += "Width='" + gr.Values[4].ToString() + "',WidCode='" + gr.Values[5].ToString() + "',Height='" + gr.Values[6].ToString() + "',";
                    sqlCmd += "HeiCode='" + gr.Values[7].ToString() + "',StockQty='" + Convert.ToInt32(gr.Values[8]) + "'";
                    sqlCmd += ",batchNo='" + gr.Values[9].ToString() + "' where id='" + rowID + "'";
                    SqlSel.ExeSql(sqlCmd);
                }

                BindGrid2(Grid1.Rows[Grid1.SelectedRowIndex].Values[1].ToString());
                Alert.ShowInTop("已保存！");
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            BindGrid2(Grid1.Rows[e.RowIndex].Values[1].ToString());
        }

        //验证文件格式合法性
        protected static bool isValidType(string fileType)
        {
            string[] typeName = new string[] { "xls", "xlsx" };
            int id = Array.IndexOf(typeName, fileType);
            if (id != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DataTable getData() 
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Length");
            dt.Columns.Add("LenCode");
            dt.Columns.Add("Width");
            dt.Columns.Add("WidCode");
            dt.Columns.Add("Height");
            dt.Columns.Add("HeiCode");
            dt.Columns.Add("StockQty");

            for (int i = 0; i < Grid2.Rows.Count; i++) 
            {
                DataRow rw = dt.NewRow();
                rw["Length"] = Grid2.Rows[i].Values[0].ToString();
                rw["LenCode"] = Grid2.Rows[i].Values[1].ToString();
                rw["Width"] = Grid2.Rows[i].Values[2].ToString();
                rw["WidCode"] = Grid2.Rows[i].Values[3].ToString();
                rw["Height"] = Grid2.Rows[i].Values[4].ToString();
                rw["HeiCode"] = Grid2.Rows[i].Values[5].ToString();
                rw["StockQty"] = Grid2.Rows[i].Values[6].ToString();
                dt.Rows.Add(rw);
            }

            return dt;
        }

        //是否存在未保存的数据，在数据覆盖前进行判断
        private bool hasEditData(FineUI.Grid grid)
        {
            // 新增数据
            List<Dictionary<string, object>> newAddedList = grid.GetNewAddedList();

            // 修改的现有数据
            Dictionary<int, Dictionary<string, object>> modifiedDict = grid.GetModifiedDict();

            if (newAddedList.Count > 0 || modifiedDict.Count > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndex >= 0)
            {
                // 删除Grid2选中单元格的客户端脚本
                string deleteScript2 = GetDeleteScript(Grid2);

                // 新增数据初始值
                JObject defaultObj2 = new JObject();
                defaultObj2.Add("id", getMaxId2());
                defaultObj2.Add("TopTypeId", Grid1.Rows[Grid1.SelectedRowIndex].Values[1].ToString());
                defaultObj2.Add("Length", "");
                defaultObj2.Add("LenCode", "");
                defaultObj2.Add("Width", "");
                defaultObj2.Add("WidCode", "");
                defaultObj2.Add("Height", "");
                defaultObj2.Add("HeiCode", "");
                defaultObj2.Add("StockQty", "");
                defaultObj2.Add("batchNo", "01");
                defaultObj2.Add("Delete2", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>", deleteScript2, IconHelper.GetResolvedIconUrl(Icon.Delete)));

                PageContext.RegisterStartupScript(Grid2.GetAddNewRecordReference(defaultObj2, false));
            }
            else 
            {
                Alert.ShowInTop("请先选择相应的大类后再添加!");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!hasEditData(Grid2))
            {
                PageContext.RegisterStartupScript(Window1.GetShowReference("MoldCoding_TypeEdit.aspx"));
            }
            else 
            {
                Alert.ShowInTop("存在未保存数据，请先保存后再操作！");
            }
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            string rowId = Grid1.DataKeys[Grid1.SelectedRowIndex][0].ToString();
            string sqlCmd = "delete from Dict_MJCoding_TopType where id='" + rowId + "'";
            SqlSel.ExeSql(sqlCmd);
            bindGrid1();
            Alert.Show("已删除！");
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int rowID = Convert.ToInt32(Grid2.DataKeys[e.RowIndex][0]);

                List<Dictionary<string, object>> newAddedList = Grid2.GetNewAddedList();
                int find = -1;
                for (int i = 0; i < newAddedList.Count; i++)
                {
                    if (rowID == Convert.ToInt32(newAddedList[i]["id"]))
                    {
                        find = i;
                        GetDeleteScript(Grid2);//前端数据直接脚本删除
                        break;
                    }
                }

                if (find == -1)
                {
                    string sqlCmd = "delete from Dict_MJCoding_Parm where id='" + rowID + "'";
                    SqlSel.ExeSql(sqlCmd);
                    BindGrid2(Grid1.Rows[Grid1.SelectedRowIndex].Values[1].ToString());
                }


                Alert.ShowInTop("删除成功!");
            }

        }

        protected void btn_saveAs_Click(object sender, EventArgs e)
        {
            if (Grid3.Rows.Count > 0)
            {
                string fileName = "模具编码" + "_" + Grid1.Rows[Grid1.SelectedRowIndex].Values[0].ToString();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(Grid3));
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

    }
}