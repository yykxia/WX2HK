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
    public partial class PLM_BarCode_Release : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindGrid();
            }
        }

        private void bindGrid() 
        {
            string sqlCmd = "select a.tradeNo,isnull(a.bindSum,0) as bindSum,b.orderno,b.plancount,b.itemNo,b.itemParm,(case when onlineStatus='1' then '在产' else '完工' end) as status,b.endTime,isnull(c.bindTotal,0) as bindTotal,isnull(d.offlinecount,0) as offlinecount from ";
            sqlCmd += "(select sum(bindQty) as bindSum,tradeno from PLM_Serials_BindBarCode where olineStatus='1' group by tradeNo) a ";
            sqlCmd += "left join PLM_Product_OnLine b on a.tradeno=b.id";
            sqlCmd += " left join (select sum(bindQty) as bindTotal,tradeno from PLM_Serials_BindBarCode group by tradeNo) c on c.tradeno=a.tradeNo";
            sqlCmd += " left join (select sum(bindQty) as offLineCount,tradeno from PLM_Serials_BindBarCode where olineStatus='0' group by tradeNo) d on d.tradeno=a.tradeNo order by b.endTime";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        protected void rbt_query_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "select a.tradeNo,isnull(a.bindSum,0) as bindSum,b.orderno,b.plancount,b.itemNo,b.itemParm,(case when onlineStatus='1' then '在产' else '完工' end) as status,b.endTime,isnull(c.bindTotal,0) as bindTotal,isnull(d.offlinecount,0) as offlinecount from ";
                sqlCmd += "(select sum(bindQty) as bindSum,tradeno from PLM_Serials_BindBarCode where olineStatus='1' group by tradeNo) a ";
                sqlCmd += "left join PLM_Product_OnLine b on a.tradeno=b.id";
                sqlCmd += " left join (select sum(bindQty) as bindTotal,tradeno from PLM_Serials_BindBarCode group by tradeNo) c on c.tradeno=a.tradeNo";
                sqlCmd += " left join (select sum(bindQty) as offLineCount,tradeno from PLM_Serials_BindBarCode where olineStatus='0' group by tradeNo) d on d.tradeno=a.tradeNo";
                string selectedValue = rbt_query.SelectedValue;
                DateTime curDte = DateTime.Now;
                if (selectedValue == "3")//三天前
                {
                    sqlCmd += " where endTime<='" + curDte.AddDays(-3) + "' order by endTime";
                }
                if (selectedValue == "2")
                {
                    sqlCmd += " where endTime<='" + curDte.AddDays(-2) + "' order by endTime";
                }
                if (selectedValue == "1")
                {
                    sqlCmd += " where endTime<='" + curDte.AddDays(-1) + "' order by endTime";
                }
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            catch (Exception ex) 
            {
                Alert.Show(ex.Message);
            }
        }

        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            bindGrid();
        }
    }
}