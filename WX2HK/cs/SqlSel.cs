using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace IETCsoft.sql
{
    public class SqlSel
    {
        public static SqlConnection ConectStrMes;
        public static string ConnectionString;
        static string getconn()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            //string ConnectionString = "server=.;uid=sa;pwd=ice_tea.7;database=HKOA;";
            return ConnectionString;
        }

        static string ConMes()
        {  //mes数据库连接
            string ConString = ConfigurationManager.ConnectionStrings["C2M2ERP"].ConnectionString;
            return ConString;
        }
        static string ConErp()
        {  //erp数据库连接
            string ConString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;
            return ConString;
        }
        static string ConMesMiddle()
        {  //中间库数据库连接
            string ConString = ConfigurationManager.ConnectionStrings["C2M2ERPMIDDLE"].ConnectionString;
            return ConString;
        }
        //取得mes的工单和完成销售订单和实际耗料的类方法
        public static bool GetMesDataset(ref DataTable WorderDt, string StrMes, string Begindate, string Enddate, string ConFlag)
        {
            string JudgeStr = "select";                               //判断是否为存储过程的字符串
            if (ConFlag == "MES")                                            //"MES"为连接  mes连接
            {
                string MesCON = ConMes();
                ConectStrMes = new SqlConnection(MesCON);
            }
            else if (ConFlag == "MIDDLE")
            {
                string MiddleCON = ConMesMiddle();                      //"MIDDLE" 为中间库连接
                ConectStrMes = new SqlConnection(MiddleCON);
            }
            else if (ConFlag == "ERP")
            {
                string ErpCon = ConErp();                                //"ERP"浪潮ERP库数据库 
                ConectStrMes = new SqlConnection(ErpCon);
            }

            SqlCommand WorderScmd = new SqlCommand();
            try
            {
                if (ConectStrMes.State != ConnectionState.Open)
                {
                    ConectStrMes.Open();
                }
                //开始填充
                WorderScmd.Connection = ConectStrMes;
                WorderScmd.CommandText = StrMes;
                if (StrMes.IndexOf(JudgeStr) > -1)                       //判断是否为存储过程
                {

                }
                else
                {
                    WorderScmd.CommandType = CommandType.StoredProcedure;
                    switch (StrMes)
                    {
                        case "pc_GetNowTimeActUse":                 //mes中查看完工订单实际耗料的存储过程
                            {
                                WorderScmd.Parameters.Add("@DT1", SqlDbType.VarChar).Value = Begindate;
                                WorderScmd.Parameters.Add("@DT2", SqlDbType.VarChar).Value = Enddate;            //original order no 
                                WorderScmd.Parameters.Add("@KIND", SqlDbType.Int).Value = 3;                    //2产品不区分长度，宽度，厚度 3，区分长度，宽度，厚度。
                                break;
                            }
                    }
                }
                SqlDataAdapter WorderSda = new SqlDataAdapter(WorderScmd);
                WorderDt = new DataTable();
                WorderSda.Fill(WorderDt);
                if (WorderDt.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                ConectStrMes.Close();
            }
        }

        //插入中间库的物料耗用
        public static bool GetMiddleDataset(ref DataTable WorderDt, string StrMes, string ORDERNO, string P_CINVCODE, decimal ORDER_QTY, string CINVCODE, decimal CONSUMEQTY, decimal LENGTH, decimal WIDTH, decimal THICKNESS)
        {
            try
            {
                string MiddleCON = ConMesMiddle();                      //"MIDDLE" 为中间库连接
                ConectStrMes = new SqlConnection(MiddleCON);
                SqlCommand WorderScmd = new SqlCommand();
                WorderScmd.Connection = ConectStrMes;
                WorderScmd.CommandText = StrMes;
                WorderScmd.CommandType = CommandType.StoredProcedure;
                switch (StrMes)
                {
                    case "INSERT_ORDER_ACT_CONSUME":                    //插入中间库实际耗用数据，为插入erp做准备。
                        {
                            WorderScmd.Parameters.Add("@ORDERNO", SqlDbType.VarChar).Value = ORDERNO;
                            WorderScmd.Parameters.Add("@P_CINVCODE", SqlDbType.VarChar).Value = P_CINVCODE;            //original order no 
                            WorderScmd.Parameters.Add("@ORDER_QTY", SqlDbType.Decimal).Value = ORDER_QTY;
                            WorderScmd.Parameters.Add("@CINVCODE", SqlDbType.VarChar).Value = CINVCODE;
                            WorderScmd.Parameters.Add("@LENGTH", SqlDbType.Decimal).Value = CONSUMEQTY;            //original order no 
                            WorderScmd.Parameters["@LENGTH"].Precision = 18;
                            WorderScmd.Parameters["@LENGTH"].Scale = 2;
                            WorderScmd.Parameters.Add("@BLENGTH", SqlDbType.Decimal).Value = LENGTH;
                            WorderScmd.Parameters.Add("@BWIDTH ", SqlDbType.Decimal).Value = WIDTH;
                            WorderScmd.Parameters.Add("@BTHICKNESS", SqlDbType.Decimal).Value = THICKNESS;
                            break;
                        }
                    default:return false;
                }
                SqlDataAdapter WorderSda = new SqlDataAdapter(WorderScmd);
                WorderDt = new DataTable();
                WorderSda.Fill(WorderDt);
                if (WorderDt.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }

            catch
            {
                return false;
            }
            finally
            {
                ConectStrMes.Close();
            }

        }
        public static bool GetSqlSel(ref DataTable ODT, string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                SqlDataAdapter sda = new SqlDataAdapter(sc);
                ODT = new DataTable();
                sda.Fill(ODT);
                if (ODT.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static object GetSqlScale(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static int ExeSql(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始执行
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static string ExeSqlCount(string SQL, string flag)
        {
            int rowCount = 0;
            string uorderList="";
            if (flag == "MES")
            {
                ConnectionString = ConMes();
                rowCount = 30;
            }
            else if (flag == "MIDDLE")
            {

                ConnectionString = ConMesMiddle();
                rowCount = 5;
            }

            SqlConnection _SqlConnection1 = new SqlConnection();
           
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始执行
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter(SQL,_SqlConnection1);
                DataSet ds = new DataSet();
                _SqlDataAdapter.Fill(ds);
                DataTable dt = ds.Tables[0];

                for (int row = 0; row < rowCount; row++)
                {
                    if (row == ds.Tables[0].Rows.Count)
                    {
                        break;
                    }
                    else
                    {
                        uorderList = uorderList + ",'" + ds.Tables[0].Rows[row][0] + "'";
                     }
                 }
                uorderList = uorderList.TrimStart(',');
                uorderList = "("+uorderList+")";
                return uorderList;
            
            }
            catch
            {
                return "";
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }
        
        
        
        
        
        
        
        
        
        public static int ExemesSql(string SQL, string flag)
        {

            if (flag == "MES")
            {
                ConnectionString = ConMes();
            }
            else if (flag == "MIDDLE")
            {

                ConnectionString = ConMesMiddle();
            }

            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始执行
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }
    }
}
