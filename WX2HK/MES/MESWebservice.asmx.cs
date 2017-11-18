using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using IETCsoft.sql;

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
    }
}
