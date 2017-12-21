using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using IETCsoft.sql;
using Newtonsoft.Json;

namespace WX2HK.MES
{
    /// <summary>
    /// MESWebservice 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MESWebservice : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取bom清单
        /// </summary>
        /// <param name="cpCode">成品编码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_Bom(string cpCode) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select jswlzd_lsbh from View_jswlzd where jswlzd_wlbh='" + cpCode + "'";
                string jswlzd_lsbh = SqlSel.GetSqlScale(sqlCmd).ToString();
                sqlCmd = "SELECT PARENTID,PWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=pwl) AS PWLMC,";
                sqlCmd += "ID,CWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=CWL) AS CWLMC,'1' AS PCOUNT,JSBOM_XS,LSWLZD_FPL";
                sqlCmd += " FROM (select (select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ParentID) as pwl,(select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ID) as cwl,* from f_BOM('" + jswlzd_lsbh + "')) A";
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    dt.TableName = "MES_Bom";
                    return dt;
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception) 
            {
                return null;
            }
        }
        /// <summary>
        /// 根据订单号循环获取bom清单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_Bom_order(string orderNo) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "SELECT XSDDMX_BOMLSBH FROM View_XSDD,View_XSDDMX WHERE XSDD_DDLS=XSDDMX_DDLS AND XSDD_DDBH='" + orderNo + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0) 
                {
                    string jswlzd_lsbh = "";
                    DataTable bomDt = new DataTable();
                    bomDt.Columns.Add("PARENTID");
                    bomDt.Columns.Add("PWL");
                    bomDt.Columns.Add("PWLMC");
                    bomDt.Columns.Add("ID");
                    bomDt.Columns.Add("CWL");
                    bomDt.Columns.Add("CWLMC");
                    bomDt.Columns.Add("PCOUNT");
                    bomDt.Columns.Add("JSBOM_XS");
                    bomDt.Columns.Add("LSWLZD_FPL");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable eachBomDt = new DataTable();
                        jswlzd_lsbh = dt.Rows[i]["XSDDMX_BOMLSBH"].ToString();
                        sqlCmd = "SELECT PARENTID,PWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=pwl) AS PWLMC,";
                        sqlCmd += "ID,CWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=CWL) AS CWLMC,'1' AS PCOUNT,JSBOM_XS,LSWLZD_FPL";
                        sqlCmd += " FROM (select (select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ParentID) as pwl,";
                        sqlCmd += "(select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ID) as cwl,* from f_BOM('" + jswlzd_lsbh + "')) A";
                        SqlSel.GetSqlSel(ref eachBomDt, sqlCmd);
                        //添加eachBomDt的数据
                        foreach (DataRow dr in eachBomDt.Rows)
                        {
                            bomDt.ImportRow(dr);
                        }
                    }
                    bomDt.TableName = "MES_Bom_order";
                    return bomDt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据订单号获取物料清单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_MatrialsList(string orderNo) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "SELECT XSDDMX_BOMLSBH FROM View_XSDD,View_XSDDMX WHERE XSDD_DDLS=XSDDMX_DDLS AND XSDD_DDBH='" + orderNo + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0) 
                {
                    string jswlzd_lsbh = "";
                    DataTable wlDt = new DataTable();
                    wlDt.Columns.Add("lswlzd_wlbh");
                    wlDt.Columns.Add("lswlzd_wlmc");
                    wlDt.Columns.Add("LSWLZD_GGXH");
                    wlDt.Columns.Add("jldw");
                    wlDt.Columns.Add("ly");
                    wlDt.Columns.Add("LSWLEX_C50");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable eachBomDt = new DataTable();
                        jswlzd_lsbh = dt.Rows[i]["XSDDMX_BOMLSBH"].ToString();
                        sqlCmd = "select lswlzd_wlbh,lswlzd_wlmc,LSWLZD_GGXH,(select JSJLDW_DWMC from View_JSJLDW  where JSJLDW_DWDM=LSWLZD_JLDW) as jldw,";
                        sqlCmd += "(CASE WHEN LSWLZD_WLLY=1 THEN '采购' WHEN LSWLZD_WLLY=2 THEN '自制' WHEN LSWLZD_WLLY=3 THEN '自制' end) as ly,LSWLEX_C50 from (";
                        sqlCmd += " select (select jswlzd_wlbh from View_jswlzd where jswlzd_lsbh=id) as jswlzd_wlbh from f_BOM('" + jswlzd_lsbh + "')";
                        sqlCmd += " union (select jswlzd_wlbh from View_jswlzd where jswlzd_lsbh='" + jswlzd_lsbh + "')) A";
                        sqlCmd += " left join View_LSWLZD on LSWLZD_wlbh=A.jswlzd_wlbh left join View_LSWLEX on LSWLEX_WLBH=LSWLZD_WLBH";
                        SqlSel.GetSqlSel(ref eachBomDt, sqlCmd);
                        //添加eachBomDt的数据
                        foreach (DataRow dr in eachBomDt.Rows)
                        {
                            wlDt.ImportRow(dr);
                        }
                    }
                    DataView dv = new DataView(wlDt);
                    dv.ToTable(true, "lswlzd_wlbh");
                    wlDt.TableName = "MES_wlbh_order";
                    return wlDt;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据成品编码获取物料信息
        /// </summary>
        /// <param name="cpCode">成品编码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_Matrials(string cpCode) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select jswlzd_lsbh from View_jswlzd where jswlzd_wlbh='" + cpCode + "'";
                string jswlzd_lsbh = SqlSel.GetSqlScale(sqlCmd).ToString();
                sqlCmd = "select lswlzd_wlbh,lswlzd_wlmc,LSWLZD_GGXH,(select JSJLDW_DWMC from View_JSJLDW  where JSJLDW_DWDM=LSWLZD_JLDW) as jldw,";
                sqlCmd += "(CASE WHEN LSWLZD_WLLY=1 THEN '采购' WHEN LSWLZD_WLLY=2 THEN '自制' WHEN LSWLZD_WLLY=3 THEN '自制' end) as ly,LSWLEX_C50,";
                sqlCmd += "(case when lswlzd_wlbh like 'ML%' then (select color from View_Ycbh where bh=LSWLEX_C3) end) as mlColor, ";
                sqlCmd += "(case when lswlzd_wlbh like 'ML%' then ('http://192.168.10.208:82/'";
                sqlCmd += "+(select filepath+filename from View_Ycbh where bh=LSWLEX_C3)) end) as addtionFile";
                sqlCmd += " from (";
                sqlCmd += " select (select jswlzd_wlbh from View_jswlzd where jswlzd_lsbh=id) as jswlzd_wlbh from f_BOM('" + jswlzd_lsbh + "')";
                sqlCmd += " union (select jswlzd_wlbh from View_jswlzd where jswlzd_lsbh='" + jswlzd_lsbh + "')) A";
                sqlCmd += " left join View_LSWLZD on LSWLZD_wlbh=A.jswlzd_wlbh left join View_LSWLEX on LSWLEX_WLBH=LSWLZD_WLBH";
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    dt.TableName = "MES_wlbh";
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据成品编码获取所有编码及版本号
        /// </summary>
        /// <param name="cpCode">成品编码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_MatrialsVersion(string cpCode) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "select jswlzd_lsbh from View_jswlzd where jswlzd_wlbh='" + cpCode + "'";
                string jswlzd_lsbh = SqlSel.GetSqlScale(sqlCmd).ToString();
                sqlCmd = "select jswlzd_lsbh,JSWLZD_WLBH,jswlzd_bbbh,'1' as isNew from (";
                sqlCmd += " select id from f_BOM('" + jswlzd_lsbh + "') ";
                sqlCmd += " union select JSWLZD_LSBH from View_JSWLZD where JSWLZD_LSBH='" + jswlzd_lsbh + "') A LEFT JOIN View_JSWLZD ON";
                sqlCmd += " JSWLZD_LSBH=A.ID";
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    dt.TableName = "MES_MatrialsVersion";
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据订单号获取所有物料及其版本号
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable MES_MatrialsVersion_order(string orderNo) 
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlCmd = "SELECT XSDDMX_BOMLSBH FROM View_XSDD,View_XSDDMX WHERE XSDD_DDLS=XSDDMX_DDLS AND XSDD_DDBH='" + orderNo + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    string jswlzd_lsbh = "";
                    DataTable wlDt = new DataTable();
                    wlDt.Columns.Add("jswlzd_lsbh");
                    wlDt.Columns.Add("JSWLZD_WLBH");
                    wlDt.Columns.Add("jswlzd_bbbh");
                    wlDt.Columns.Add("isNew");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable eachBomDt = new DataTable();
                        jswlzd_lsbh = dt.Rows[i]["XSDDMX_BOMLSBH"].ToString();
                        sqlCmd = "select jswlzd_lsbh,JSWLZD_WLBH,jswlzd_bbbh,'1' as isNew from (";
                        sqlCmd += " select id from f_BOM('" + jswlzd_lsbh + "') ";
                        sqlCmd += " union select JSWLZD_LSBH from View_JSWLZD where JSWLZD_LSBH='" + jswlzd_lsbh + "') A LEFT JOIN View_JSWLZD ON";
                        sqlCmd += " JSWLZD_LSBH=A.ID";
                        SqlSel.GetSqlSel(ref eachBomDt, sqlCmd);
                        //添加eachBomDt的数据
                        foreach (DataRow dr in eachBomDt.Rows)
                        {
                            wlDt.ImportRow(dr);
                        }
                    }
                    DataView dv = new DataView(wlDt);
                    dv.ToTable(true, "JSWLZD_WLBH");
                    wlDt.TableName = "MES_MatrialsVersion_order";
                    return wlDt;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据物料编号获取附件列表
        /// </summary>
        /// <param name="matrialNo">物料编码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable INV_Files(string matrialNo) 
        {
            string sqlCmd = "select bomid,('http://192.168.10.208:82/AddtionFile/'+ substring(docdir,6,len(docdir))) as fileUrl ";
            sqlCmd += " from view_sys_fj,view_sys_ftpfile where view_sys_fj.fileid=view_sys_ftpfile.id";
            sqlCmd += " and bomid='" + matrialNo + "' and filedel='0'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "AddtionFile";
            return dt;
        }

        /// <summary>
        /// 根据成品编码返回相关bom及物料信息
        /// </summary>
        /// <param name="productNo">成品编码</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet MES_BomAndMaterials(string productNo) 
        {
            try
            {
                DataSet erpData = new DataSet();
                //bom数据
                DataTable bomDt = new DataTable();
                bomDt.TableName = "BomList";
                string sqlCmd = "select jswlzd_lsbh from View_jswlzd where jswlzd_wlbh='" + productNo + "'";
                string jswlzd_lsbh = SqlSel.GetSqlScale(sqlCmd).ToString();
                sqlCmd = "SELECT PARENTID,PWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=pwl) AS PWLMC,";
                sqlCmd += "ID,CWL,(SELECT LSWLZD_WLMC FROM View_LSWLZD WHERE LSWLZD_WLBH=CWL) AS CWLMC,'1' AS PCOUNT,JSBOM_XS,LSWLZD_FPL";
                sqlCmd += " FROM (select (select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ParentID) as pwl,(select JSWLZD_wlbh from View_jswlzd where JSWLZD_LSBH=ID) as cwl,* from f_BOM('" + jswlzd_lsbh + "')) A";
                SqlSel.GetSqlSel(ref bomDt, sqlCmd);
                erpData.Tables.Add(bomDt);

                //物料数据
                DataTable materialsDt = new DataTable();
                materialsDt.TableName = "MaterialsList";
                sqlCmd = "select bomls,lswlzd_wlbh,lswlzd_wlmc,LSWLZD_GGXH,(select JSJLDW_DWMC from View_JSJLDW  where JSJLDW_DWDM=LSWLZD_JLDW) as jldw,";
                sqlCmd += "(CASE WHEN LSWLZD_WLLY=1 THEN '采购' WHEN LSWLZD_WLLY=2 THEN '自制' WHEN LSWLZD_WLLY=3 THEN '自制' end) as ly,LSWLEX_C50 from (";
                sqlCmd += " select (select jswlzd_wlbh from View_jswlzd where jswlzd_lsbh=id) as jswlzd_wlbh,ID as bomls from f_BOM('" + jswlzd_lsbh + "')";
                sqlCmd += " union (select jswlzd_wlbh,JSWLZD_LSBH from View_jswlzd where jswlzd_lsbh='" + jswlzd_lsbh + "')) A";
                sqlCmd += " left join View_LSWLZD on LSWLZD_wlbh=A.jswlzd_wlbh left join View_LSWLEX on LSWLEX_WLBH=LSWLZD_WLBH";
                SqlSel.GetSqlSel(ref materialsDt, sqlCmd);
                erpData.Tables.Add(materialsDt);

                return erpData;
            }
            catch (Exception)
            {
                return null;
            }

        }

        [WebMethod]
        public DataTable MES_Worder(string BeginDate, string EndDate, string BillType, string flag)
        {                         
            //起始时间，单据类型 ,连接标识 
            //北厂定制线数据库抓取工单信息/订单信息/物料耗用信息
            try
            {
                // DataSet WorderData = new DataSet();
                //工单数据
                DataTable WorderDt = new DataTable();
                string sqlCmd = "";
                if (BillType == "MES工单-ERP生产订单" && flag == "MES")                //MES中未插入中间库的单据
                {
                    WorderDt.TableName = "Worder";//工单

                    sqlCmd = "select uorder_number,worder_number,worder_rmtnumber,worder_rmtname,worder_rmttype,worder_rmtunit,";
                    sqlCmd += "worder_rmtsource,worder_rmtlength,worder_rmtlgtunit,worder_rmtwidth,worder_rmtwdtunit,";
                    sqlCmd += "worder_rmtthickness,worder_rmttikunit,worder_rmtcolour,worder_rmtmaterial,worder_rmtnum,";
                    sqlCmd += "uorder_ordertime,uorder_finishtime,worder_status,worder_status,worder_status,worder_status,convert(varchar(100),inserttime,23) as inserttime,";
                    sqlCmd += " uorder_productnumber,uorder_id,uorder_productnum,uorder_Blength,uorder_Bwidth,uorder_Bthickness from InterFace_WorderExcute where 1=1 ";

                    if (!BeginDate.Equals("") && !EndDate.Equals(""))
                    {
                        sqlCmd += "and inserttime>='" + BeginDate + " 00:00:00'      and inserttime<'" + EndDate + " 23:59:59 ' ";
                    }
                    sqlCmd += "order by uorder_number,inserttime";
                    SqlSel.GetMesDataset(ref WorderDt, sqlCmd, "", "", "MES");  //"MES" MES连接  "MIDDLE"中间库连接 "ERP" ERP连接
                }

                if (BillType == "MES工单-ERP生产订单" && flag == "MIDDLE")                //中间库未插入ERP生产订单单据
                {
                    WorderDt.TableName = "Worder";//工单
                    sqlCmd = "select  uorder_number,uorder_productnumber,uorder_productnum,worder_number,worder_rmtnumber ,worder_rmtname";
                    sqlCmd += ",worder_rmttype,worder_rmtunit,worder_rmtnum ,inserttime FROM ImportErp ";
                    //sqlCmd += "order by inserttime";
                    SqlSel.GetMesDataset(ref WorderDt, sqlCmd, "", "", "MIDDLE");
                }
                if (BillType == "MES销售-ERP入库领料" && flag == "Instock")
                {
                    WorderDt.TableName = "Instock"; //MES入库数据
                    sqlCmd = "pc_GetNowTimeActUse";
                    SqlSel.GetMesDataset(ref WorderDt, sqlCmd, BeginDate, EndDate, "MES");
                }
                if (BillType == "MES销售-ERP入库领料" && flag == "Consume")
                {
                    WorderDt.TableName = "Consume"; //中间库入库数据
                    sqlCmd = "select top 100  uorder_number,uorder_productnumber,uorder_productnum,worder_rmtnumber,worder_rmtname,worder_rmtnum,BLENGTH as uorder_Blength   ";
                    sqlCmd += ",BWIDTH  as uorder_Bwidth, BTHICKNESS as uorder_Bthickness from ORDER_CONSUME where exists(select orderno from ordernoConsume    ";
                    sqlCmd += " where orderno=ORDER_CONSUME.uorder_number and flag=0 )  or uorder_number not in  (select orderno from  ordernoConsume)  order by uorder_number ";
                    SqlSel.GetMesDataset(ref WorderDt, sqlCmd, "", "", "MIDDLE");
                }
                if (BillType == "MES销售-ERP入库领料" && flag == "ErpConsume")
                {
                    WorderDt.TableName = "ErpConsume"; //需要导入ERP的耗用数据   flag=2 已经插入erp
                    sqlCmd = " select distinct   ORDERNO  as uorder_number,P_CINVCODE as uorder_productnumber ,INSTOCK_QTY as uorder_productnum ,id as uorder_id, Blength as uorder_Blength,  ";
                    sqlCmd += " Bwidth as uorder_Bwidth,BTHICKNESS as uorder_Bthickness from ordernoConsume   where flag=1  ";
                    SqlSel.GetMesDataset(ref WorderDt, sqlCmd, "", "", "MIDDLE");
                }


                // WorderData.Tables.Add(WorderDt); 
                return WorderDt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从MES系统中取出耗用，插入到中间库耗用表中，为插入ERP做数据
        /// </summary>
        /// <param name="ORDERNO"></param>
        /// <param name="P_CINVCODE"></param>
        /// <param name="ORDER_QTY"></param>
        /// <param name="CINVCODE"></param>
        /// <param name="LENGTH"></param>
        /// <param name="VOLUME"></param>
        /// <param name="COSUME"></param>
        /// <returns></returns>
        [WebMethod]
        public DataTable Insert_Worder(string ORDERNO, string P_CINVCODE, decimal ORDER_QTY, string CINVCODE, decimal COSUMEQTY, decimal LENGTH, decimal WIDTH, decimal THICKNESS)
        {
            DataTable WorderDt = new DataTable();
            WorderDt.TableName = "Act_Consume"; //中间库耗用数据
            String SqlCmd = "INSERT_ORDER_ACT_CONSUME";
            try
            {
                SqlSel.GetMiddleDataset(ref WorderDt, SqlCmd, ORDERNO, P_CINVCODE, ORDER_QTY, CINVCODE, COSUMEQTY, LENGTH, WIDTH, THICKNESS);
                return WorderDt;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
