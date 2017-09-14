using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using FineUI;
using System.Data;
using System.Text;
using System.Data.OleDb;

namespace WX2HK
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {

        }

        protected string GetUser()
        {
            try
            {
                //if (Session["loginUser"] != null)
                //{
                //    return Session["loginUser"].ToString();
                //}
                //else
                //{
                //    PageContext.Redirect("~/default.aspx", "_top");
                //    return null;
                //}
                string userName = Session["loginUser"].ToString();
                string sqlCmd = "select id from x_user where name='" + userName + "'";
                int userId = Convert.ToInt32(IETCsoft.sql.SqlSel.GetSqlScale(sqlCmd));

                return userName;
            }
            catch 
            {
                PageContext.Redirect("~/default.aspx", "_top");
                return null;
            }
        }

        /// <summary>
        /// 对给定的一个图片文件生成一个指定大小的缩略图，并将缩略图保存到指定位置。
        /// </summary>
        /// <param name="originalImageFile">图片的物理文件地址</param>
        /// <param name="thumbNailImageFile">缩略图的物理文件地址</param>
        public static void MakeThumbNail(string originalImageFile, string thumbNailImageFile)
        {
            try
            {
                int thumbWidth = 231;    //要生成的缩略图的宽度
                int thumbHeight = 80;    //要生成的缩略图的高度

                System.Drawing.Image image = System.Drawing.Image.FromFile(originalImageFile); //利用Image对象装载源图像

                //接着创建一个System.Drawing.Bitmap对象，并设置你希望的缩略图的宽度和高度。

                int srcWidth = image.Width;
                int srcHeight = image.Height;
                //double theRatio = (double)srcHeight / srcWidth;
                //int thumbHeight = Convert.ToInt32(theRatio * thumbWidth);
                Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
                //if (srcHeight > srcWidth)
                //{
                //    bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
                //}

                //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。

                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);

                //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality

                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //下面这个也设成高质量

                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                //下面这个设成High

                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //把原始图像绘制成上面所设置宽高的缩小图

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
                gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

                //保存图像

                bmp.Save(thumbNailImageFile);

                //释放资源
                bmp.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        #region dataTable转换成Json格式
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        #endregion dataTable转换成Json格式

        /// <summary>
        /// excel转datatable
        /// </summary>
        /// <param name="Path">excel路径</param>
        /// <returns></returns>
        public DataTable InputExcel(string Path)
        {
            try
            {
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Server.MapPath(Path) + "; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
                //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。
                //      "IMEX=1 "如果列中的数据类型不一致，使用"IMEX=1"可必免数据类型冲突。
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                OleDbDataAdapter myCommand = null;

                string strExcel = "select * from [Sheet1$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                DataTable dt = new DataTable();
                myCommand.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        ///  删除选中行的脚本
        /// </summary>
        /// <param name="grid">指定的表格</param>
        /// <returns></returns>
        public static string GetDeleteScript(FineUI.Grid grid)
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, grid.GetDeleteSelectedRowsReference(), String.Empty);
        }

        /// <summary>
        /// 数据集中是否存在某列的值已存在的行
        /// </summary>
        /// <param name="dt">目标数据集</param>
        /// <param name="columnName">目标列名</param>
        /// <param name="compareValue">比较值</param>
        /// <returns>查找到的首行</returns>
        public static int tableValueIndex(DataTable dt, string columnName, string compareValue)
        {
            int find = -1;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][columnName].ToString() == compareValue)
                    {
                        find = i;
                        break;
                    }
                }
            }

            return find;
        }
    }
}