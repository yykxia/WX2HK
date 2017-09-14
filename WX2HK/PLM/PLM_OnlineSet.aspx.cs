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
    public partial class PLM_OnlineSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid("1", "%");//首次加载在线计划即onlineStatus=1
                bindLineInfo();
                btn_addNew.OnClientClick = Window1.GetShowReference("PLM_OnlineSet_AddNew.aspx");
                //btn_changeLine.OnClientClick = Window3.GetShowReference(string.Format("PLM_OnlineSet_ChangeLine.aspx?OnlineId={}", Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]));
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

        private void bindGrid(string onlineStatus, string lineId) 
        {
            string sqlCmd = "select (case IsTemp when '1' then '插单' else '' end) as tempDesc,(case OnlineStatus when '1' then '上线' when '0' then '下线' end) as pStatus,";
            sqlCmd += "(case when ImgURL is not null then '查看' else ImgURL end) as ismlOrno,* from PLM_Product_OnLine a ";
            sqlCmd += "left join PLM_Product_Line b on b.id=a.lineId ";
            sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id ";
            sqlCmd += "left join PLM_Product_Image d on a.id=d.OnlineId ";
            sqlCmd += "where onlineStatus like '" + onlineStatus + "' and LineId like '" + lineId + "' order by buildtime desc";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        //计划下线
        protected void btn_offLine_Click(object sender, EventArgs e)
        {
            try 
            {
                if (Grid1.SelectedRowIndexArray.Length < 1) 
                {
                    Alert.Show("没有选中任何行！");
                    return;
                }
                int pId = 0;
                int[] selections = Grid1.SelectedRowIndexArray;
                foreach (int rowIndex in selections)
                {
                    GridRow row = Grid1.Rows[rowIndex];
                    System.Web.UI.WebControls.Label lineStatus = (System.Web.UI.WebControls.Label)row.FindControl("Label2");
                    if (lineStatus.Text == "上线")
                    {
                        pId = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                        string sqlCmd = "update PLM_Product_OnLine set OnlineStatus=0,endTime='" + DateTime.Now + "' where id='" + pId + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }
                }
                Alert.Show("已下线！");
                bindGrid(RadioButtonList1.SelectedValue, ddl_line.SelectedValue);
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(DatePicker1.Text) & !string.IsNullOrEmpty(DatePicker2.Text))
                {
                    string sqlCmd = "select (case IsTemp when '1' then '插单' else '' end) as tempDesc,(case OnlineStatus when '1' then '上线' when '0' then '下线' end) as pStatus,";
                    sqlCmd += "(case when ImgURL is not null then '查看' else ImgURL end) as ismlOrno,* from PLM_Product_OnLine a ";
                    sqlCmd += "left join PLM_Product_Line b on b.id=a.lineId ";
                    sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id ";
                    sqlCmd += "left join PLM_Product_Image d on a.id=d.OnlineId ";
                    sqlCmd += "where onlineStatus like '" + RadioButtonList1.SelectedValue + "' and LineId like '" + ddl_line.SelectedValue + "' ";
                    sqlCmd += "and CONVERT(varchar(100), buildtime, 23)>='" + DatePicker1.Text + "' and CONVERT(varchar(100), buildtime, 23)<='" + DatePicker2.Text + "' order by buildtime desc";
                    DataTable dt = new DataTable();
                    SqlSel.GetSqlSel(ref dt, sqlCmd);
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                }
                else 
                {
                    Alert.ShowInTop("起止日期不可为空！");
                    return;
                }

            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Window2.Hidden = true;
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selections = Grid1.SelectedRowIndexArray;
                if (selections.Length > 0)
                {
                    if (isOneLine())
                    {
                        if (isOneStatus())
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (int rowIndex in selections)
                            {
                                sb.AppendFormat("{0};", Grid1.DataKeys[rowIndex][0]);
                            }
                            PageContext.RegisterStartupScript(Window1.GetShowReference("PLM_OnlineSet_Print.aspx?idList=" + sb + ""));
                        }
                        else
                        {
                            Alert.ShowInTop("存在已下线的订单！请查正。");
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowInTop("不同产线不可同时打印！");
                        return;
                    }
                }
                else 
                {
                    Alert.ShowInTop("请先选择相应数据再操作！");
                    return;
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }
        //验证是否同一产线
        private bool isOneLine() 
        {
            int[] selections = Grid1.SelectedRowIndexArray;
            string lineName = Grid1.Rows[selections[0]].Values[2].ToString();//取选择的第一行产线名
            int find = -1;
            for (int i = 1; i < selections.Length; i++) 
            {
                string compLineName = Grid1.Rows[selections[i]].Values[2].ToString();
                if (lineName != compLineName) 
                {
                    find = i;
                }
            }

            if (find == -1)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        //是否有不在线上的订单
        private bool isOneStatus() 
        {
            int[] selections = Grid1.SelectedRowIndexArray;
            int find = -1;
            for (int i = 0; i < selections.Length; i++)
            {
                GridRow row = Grid1.Rows[selections[i]];
                System.Web.UI.WebControls.Label lineStatus = (System.Web.UI.WebControls.Label)row.FindControl("Label2");
                if (lineStatus.Text != "上线") 
                {
                    find = i;
                }
            }
            if (find == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            int[] selections = Grid1.SelectedRowIndexArray;
            string sqlCmd = "";
            StringBuilder sb_Order = new StringBuilder();
            for (int i = 0; i < selections.Length; i++)
            {
                sqlCmd = "select count(*) from PLM_Serials_BindBarCode where tradeno='" + Grid1.DataKeys[selections[i]][0].ToString() + "'";
                int bindCount = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                if (bindCount > 0)
                {
                    Alert.ShowInTop(string.Format("第{0}行存在已上架数据，不可直接删除！", selections[i] + 1));
                    return;
                }
                else 
                {
                    sqlCmd = "delete from PLM_Product_OnLine where id='" + Grid1.DataKeys[selections[i]][0].ToString() + "'";
                    if (SqlSel.ExeSql(sqlCmd) == 1)
                    {
                        sqlCmd = "delete from PLM_Product_Rel where prodId='" + Grid1.DataKeys[selections[i]][0].ToString() + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }
                }
            }

            Alert.Show("已删除！");
        }

        protected void ddl_line_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindGrid(RadioButtonList1.SelectedValue, ddl_line.SelectedValue);
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindGrid(RadioButtonList1.SelectedValue, ddl_line.SelectedValue);
        }

        protected void btn_selfRefresh_Click(object sender, EventArgs e)
        {
            bindGrid(RadioButtonList1.SelectedValue, ddl_line.SelectedValue);
        }

        protected void btn_changeLine_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length > 1)
            {
                Alert.ShowInTop("只支持单个计划的产线调整！");
                return;
            }
            else 
            {
                PageContext.RegisterStartupScript(Window3.GetShowReference("PLM_OnlineSet_ChangeLine.aspx?OnlineId=" + Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]));
            }
        }


    }
}