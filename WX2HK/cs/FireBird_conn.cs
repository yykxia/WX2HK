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
using FirebirdSql.Data.FirebirdClient;

namespace WX2HK
{
    public class FireBird_Conn
    {
        static string getconn()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Attend_Temp"].ConnectionString;
            return ConnectionString;
        }

        public static bool GetSqlSel(ref DataTable ODT, string SQL)
        {
            string ConnectionString = getconn();
            FbConnection _SqlConnection1 = new FbConnection();
            FbCommand sc = new FbCommand();
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
                FbDataAdapter sda = new FbDataAdapter(sc);
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
            FbConnection _SqlConnection1 = new FbConnection();
            FbCommand sc = new FbCommand();
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
            FbConnection _SqlConnection1 = new FbConnection();
            FbCommand sc = new FbCommand();
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