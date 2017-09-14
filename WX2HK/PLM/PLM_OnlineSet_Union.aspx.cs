using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Text;

namespace WX2HK.PLM
{
    public partial class PLM_OnlineSet_Union : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                DatePicker1.SelectedDate = DateTime.Now;
                DatePicker2.SelectedDate = DateTime.Now;
                bindLineInfo();
            }
        }
        //ERP查询结果
        private void bindGrid(string tradeNo)
        {
            string sqlCmd = "select 0 as canRead,(LSWLEX_C2+'*'+LSWLEX_C3+'*'+LSWLEX_C4+'，'+LSWLEX_C9+'，'+LSWLEX_C5+'，'+LSWLEX_C1+'，'+LSWLEX_C8+'，'+LSWLEX_C7) AS RequireParm,";
            sqlCmd += " isnull(orderSum,0) as historySum,(planSum-isnull(orderSum,0)) as lastSum,* from View_PLM_ERPData a ";
            sqlCmd += "left join  (select SUM(OrderCount) as orderSum,orderid from PLM_Product_Rel group by orderid) b on orderid=productSN ";
            sqlCmd += "left join view_plm_lswlex d on d.lswlex_wlbh=a.itemNo ";
            sqlCmd += "where orderNo like '%" + tradeNo + "%' order by planProdDate desc";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
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
        }

        protected void tgb_orderNo_TriggerClick(object sender, EventArgs e)
        {
            try
            {
                if (tgb_orderNo.Text.Length >= 6)
                {
                    bindGrid(tgb_orderNo.Text);
                }
                else
                {
                    Alert.ShowInTop("输入的订单号位数过短！");
                    return;
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string sqlCmd = "select (LSWLEX_C2+'*'+LSWLEX_C3+'*'+LSWLEX_C4+'，'+LSWLEX_C9+'，'+LSWLEX_C5+'，'+LSWLEX_C1+'，'+LSWLEX_C8+'，'+LSWLEX_C7) AS RequireParm,isnull(orderSum,0) as historySum,* from View_PLM_ERPData a ";
            sqlCmd += "left join  (select SUM(OrderCount) as orderSum,orderid from PLM_Product_Rel group by orderid) b on orderid=productSN ";
            sqlCmd += "left join view_plm_lswlex d on d.lswlex_wlbh=a.itemNo ";
            sqlCmd += "where planProdDate>='" + DatePicker1.Text + "' and planProdDate<='" + DatePicker2.Text + "' order by planProdDate desc";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        protected void tgb_wlbh_TriggerClick(object sender, EventArgs e)
        {
            string sqlCmd = "select (LSWLEX_C2+'*'+LSWLEX_C3+'*'+LSWLEX_C4+'，'+LSWLEX_C9+'，'+LSWLEX_C5+'，'+LSWLEX_C1+'，'+LSWLEX_C8+'，'+LSWLEX_C7) AS RequireParm,isnull(orderSum,0) as historySum,* from View_PLM_ERPData a ";
            sqlCmd += "left join  (select SUM(OrderCount) as orderSum,orderid from PLM_Product_Rel group by orderid) b on b.orderid=a.productSN ";
            sqlCmd += "left join view_plm_lswlex d on d.lswlex_wlbh=a.itemNo ";
            sqlCmd += "where itemNo ='" + tgb_wlbh.Text + "' order by planProdDate desc";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid2.DataSource = dt;
            Grid2.DataBind();
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                FineUI.CheckBoxField canReadField = Grid2.FindColumn("CanRead") as FineUI.CheckBoxField;
                Label1.Text = "";
                if (isOneItem())
                {
                    int sumCount = 0;
                    StringBuilder sb_orderNo = new StringBuilder();
                    StringBuilder sb_msg = new StringBuilder();//超产订单信息

                    DataTable dt = new DataTable();
                    dt.Columns.Add("orderId");
                    dt.Columns.Add("orderCount");
                    dt.Columns.Add("orderNo");
                    int firstRowIndex = -1;
                    int find = 0;
                    for (int i = 0; i < Grid2.Rows.Count; i++)
                    {
                        bool canRead = canReadField.GetCheckedState(i);
                        if (canRead)
                        {
                            find += 1;
                            if (find == 1) 
                            {
                                firstRowIndex = i;
                            }
                            //循环插入至隐藏业务订单列表
                            DataRow newRow = dt.NewRow();
                            GridRow grow = Grid2.Rows[i];
                            newRow["orderId"] = grow.DataKeys[0].ToString();
                            newRow["orderCount"] = grow.Values[8].ToString();
                            newRow["orderNo"] = grow.Values[1].ToString();
                            dt.Rows.Add(newRow);
                            //拼接订单号
                            sb_orderNo.AppendFormat("{0}/", Grid2.Rows[i].Values[1].ToString());
                            sumCount += Convert.ToInt32(Grid2.Rows[i].Values[8]);
                            //超产订单提示
                            if (Convert.ToInt32(Grid2.Rows[i].Values[6]) <= Convert.ToInt32(Grid2.Rows[i].Values[7]))
                            {
                                sb_msg.AppendFormat("订单号:{0} 排产数量已达到计划数量，请确认是否继续排产;\r\n", Grid2.Rows[i].Values[1].ToString());
                            }
                        }
                    }
                    Label1.Text = sb_msg.ToString();
                    Grid_hidden.DataSource = dt;
                    Grid_hidden.DataBind();
                    if (firstRowIndex >= 0)
                    {
                        txb_itemNo.Text = Grid2.Rows[firstRowIndex].Values[2].ToString().Trim();//物料编码
                        string childrenItemInfo = getChildrenItemInfo(Grid2.Rows[firstRowIndex].Values[2].ToString().Trim());
                        txb_itemName.Text = Grid2.Rows[firstRowIndex].Values[5].ToString().Trim();//品名
                        TextArea_parm.Text = Grid2.Rows[firstRowIndex].Values[4].ToString().Trim() + "/" + childrenItemInfo;//规格
                        txb_workNo.Text = sb_orderNo.ToString().Substring(0, sb_orderNo.ToString().Length - 1);//合并订单，删除最后一个‘/’标记
                        numb_planCount.Text = sumCount.ToString();//合并排产数量
                        txb_itemTech.Text = Grid2.Rows[firstRowIndex].Values[9].ToString().Trim();//工艺要求
                    }
                    else 
                    {
                        Alert.ShowInTop("未选中任何数据！");
                        return;
                    }
                }
                else 
                {
                    Alert.ShowInTop("不同物料编码不可合并排产！");
                    return;
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

        //验证是否同一物料编码
        private bool isOneItem()
        {
            //int[] selections = Grid2.SelectedRowIndexArray;
            FineUI.CheckBoxField canReadField = Grid2.FindColumn("CanRead") as FineUI.CheckBoxField;
            string itemNo = "";
            int find = 0;
            for (int i = 0; i < Grid2.Rows.Count; i++) 
            {
                bool canRead = canReadField.GetCheckedState(i);
                if (canRead) 
                {
                    find += 1;
                    string curItemNo = Grid2.Rows[i].Values[2].ToString();
                    if (find == 1)
                    {
                        itemNo = curItemNo;
                    }
                    else 
                    {
                        if (itemNo != curItemNo) 
                        {
                            find = -1;
                            break;
                        }
                    }
                }
            }
            //for (int i = 1; i < selections.Length; i++)
            //{
            //    string compLineName = Grid2.Rows[selections[i]].Values[2].ToString();
            //    if (lineName != compLineName)
            //    {
            //        find = i;
            //    }
            //}

            if (find == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            if (filePhoto.HasFile)
            {
                string fileName = filePhoto.ShortFileName;
                string origImageSavePath = "";
                fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
                fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;
                origImageSavePath = Server.MapPath("~/upload/" + fileName);//原始图片存储路径
                filePhoto.SaveAs(origImageSavePath);
                label_hidden.Text = fileName;//保存路径

                //生成缩略图
                string ThumbImagePath = "~/upload/ThumbNail/" + fileName;
                MakeThumbNail(origImageSavePath, Server.MapPath(ThumbImagePath));

                Image_product.ImageUrl = "~/upload/" + fileName;

            }
        }

        //排产数据的添加
        private void infoInsert()
        {
            string curUser = GetUser();
            if (!string.IsNullOrEmpty(curUser))
            {
                int isTempOrder = 0;
                if (CkeckBox_enabled.Checked)
                {
                    isTempOrder = 1;
                }
                string ImgURL = label_hidden.Text;
                if (!filePhoto.HasFile)
                {
                    ImgURL = "636195999216328808_84U58PICkTn.jpg";//设置默认图片
                }

                string sqlCmd = "insert into [PLM_Product_OnLine] ([LineId],[BuildTime],[OnlineStatus],[OperUser] ,[PlanCount] ,[IsTemp] ,[OrderNo] ,[ItemParm] ,[ItemName],[ItemNo],[RedLineCount],[PreSetCount],[ItemTech])";
                sqlCmd += " values ('" + ddl_line.SelectedValue + "','" + DateTime.Now + "',1,'" + curUser + "','" + numb_planCount.Text + "','" + isTempOrder + "',";
                sqlCmd += "'" + txb_workNo.Text + "','" + TextArea_parm.Text + "','" + txb_itemName.Text + "','" + txb_itemNo.Text + "','" + numb_redCount.Text + "','" + numb_preSet.Text + "','" + txb_itemTech.Text + "')";
                SqlSel.ExeSql(sqlCmd);
                sqlCmd = "select max(id) from PLM_Product_OnLine";
                int onlineId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                sqlCmd = "insert into PLM_Product_Image (OnlineId,ImgURL) values ";
                sqlCmd += "('" + onlineId + "','" + ImgURL + "')";
                SqlSel.ExeSql(sqlCmd);
                //string find = "";
                for (int i = 0; i < Grid_hidden.Rows.Count; i++)
                {
                    //获取合并的订单信息
                    int recId = Convert.ToInt32(Grid_hidden.Rows[i].Values[0]);
                    int orderCount = Convert.ToInt32(Grid_hidden.Rows[i].Values[1]);
                    //执行关联明细信息的数据插入
                    string sqlStr = "insert into PLM_Product_Rel (orderid,ProdId,PriLevel,OrderCount,scddcp_jhpch) values ";
                    sqlStr += "('" + recId + "','" + onlineId + "',1,'" + orderCount + "','" + Grid_hidden.Rows[i].Values[2].ToString()+ "')";
                    SqlSel.ExeSql(sqlStr);
                }
                //表单重置
                PageContext.RegisterStartupScript(Region2.GetResetReference());
                Alert.ShowInTop("操作成功！");
            }

        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                infoInsert();
                //if (filePhoto.HasFile)
                //{
                //    infoInsert();
                //}
                //else
                //{
                //    Alert.ShowInTop("请上传相应的产品图片！");
                //    return;
                //}
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

        private string getChildrenItemInfo(string parentItemNo) 
        {
            string childrenItemInfo = "";
            string sqlCmd = "select top 1 (select lswlex_c1+'/'+lswlex_c2+'/'+lswlex_c3+'/'+lswlex_c4+'/'+lswlex_c5 from view_plm_lswlex where lswlex_wlbh=zxwlbh) as zxwlgg";
            sqlCmd += " from View_PLM_JSBOM where fxwlbh='" + parentItemNo + "'";
            DataTable dt = new DataTable();
            if (dt.Rows.Count > 0) 
            {
                childrenItemInfo = dt.Rows[0]["zxwlgg"].ToString();
            }
            return childrenItemInfo;
        }


    }
}