using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using FineUI;
using System.Data;
using IETCsoft.sql;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WX2HK.PLM
{
    public partial class PLM_OnlineSet_AddNew : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bindLineInfo();
            }
        }
        //ERP查询结果
        private void bindGrid(string tradeNo) 
        {
            string sqlCmd = "select * from View_PLM_ERPData where orderNo like '%" + tradeNo + "%' order by planProdDate desc";
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

        protected void Grid2_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int rowIndex = e.RowIndex;
            string tradeSN = Grid2.DataKeys[rowIndex][0].ToString();
            LoadDataBySN(tradeSN);
        }

        private void LoadDataBySN(string tradeSN)
        {
            string sqlCmd = "select (itemParm +'/'+ (select lswlex_c1+'/'+lswlex_c2+'/'+lswlex_c3+'/'+lswlex_c4+'/'+lswlex_c5 from view_plm_lswlex where lswlex_wlbh=zxwlbh)) as fullItemParm,* from View_PLM_ERPData";
            sqlCmd += " left join view_plm_jsbom on itemNo=fxwlbh";
            sqlCmd+=" where productSN='" + tradeSN + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            txb_workNo.Text = dt.Rows[0]["orderNo"].ToString();
            Label_sn.Text = dt.Rows[0]["productSN"].ToString();
            txb_itemName.Text = dt.Rows[0]["itemName"].ToString();
            TextArea_parm.Text = dt.Rows[0]["fullItemParm"].ToString();
            numb_planCount.Text = dt.Rows[0]["planSum"].ToString();
            txb_itemNo.Text = dt.Rows[0]["itemNo"].ToString();
            txb_itemTech.Text = dt.Rows[0]["itemTech"].ToString();
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

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            try 
            {
                if (filePhoto.HasFile)
                {
                    string sqlCmd = "";
                    sqlCmd = "select * from PLM_Product_OnLine where OrderId='" + Label_sn.Text + "'";
                    DataTable tmpDt = new DataTable();
                    SqlSel.GetSqlSel(ref tmpDt, sqlCmd);
                    if (tmpDt.Rows.Count > 0)//如果当前产品已经排产过则提示是否继续操作
                    {
                        string scriptString = Confirm.GetShowReference("当前产品已经下达过计划，是否继续安排生产计划？", string.Empty, MessageBoxIcon.Question, PageManager1.GetCustomEventReference("Confirm_OK"), PageManager1.GetCustomEventReference("Confirm_Cancel"));
                        PageContext.RegisterStartupScript(scriptString);
                    }
                    else 
                    {
                        infoInsert();
                    }
                }
                else 
                {
                    Alert.ShowInTop("请上传相应的产品图片！");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Alert.ShowInTop(ex.Message);
                return;
            }
        }

        ///// <summary>
        ///// 对给定的一个图片文件生成一个指定大小的缩略图，并将缩略图保存到指定位置。
        ///// </summary>
        ///// <param name="originalImageFile">图片的物理文件地址</param>
        ///// <param name="thumbNailImageFile">缩略图的物理文件地址</param>
        //public static void MakeThumbNail(string originalImageFile, string thumbNailImageFile)
        //{
        //    try
        //    {
        //        int thumbWidth = 208;    //要生成的缩略图的宽度
        //        int thumbHeight = 80;    //要生成的缩略图的高度

        //        System.Drawing.Image image = System.Drawing.Image.FromFile(originalImageFile); //利用Image对象装载源图像

        //        //接着创建一个System.Drawing.Bitmap对象，并设置你希望的缩略图的宽度和高度。

        //        int srcWidth = image.Width;
        //        int srcHeight = image.Height;
        //        //double theRatio = (double)srcHeight / srcWidth;
        //        //int thumbHeight = Convert.ToInt32(theRatio * thumbWidth);
        //        Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
        //        //if (srcHeight > srcWidth)
        //        //{
        //        //    bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
        //        //}

        //        //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。

        //        System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);

        //        //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality

        //        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //        //下面这个也设成高质量

        //        gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

        //        //下面这个设成High

        //        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //        //把原始图像绘制成上面所设置宽高的缩小图

        //        System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
        //        gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

        //        //保存图像

        //        bmp.Save(thumbNailImageFile);

        //        //释放资源
        //        bmp.Dispose();
        //        image.Dispose();
        //    }
        //    catch (Exception ex) 
        //    {
        //        Alert.ShowInTop(ex.Message);
        //        return;
        //    }
        //}

        protected void PageManager1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.EventArgument == "Confirm_OK") 
            {
                infoInsert();
            }
            else if (e.EventArgument == "Confirm_Cancel") 
            {
                return;
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
                string sqlCmd = "insert into [PLM_Product_OnLine] ([LineId],[OrderId],[BuildTime],[OnlineStatus],[OperUser] ,[PlanCount] ,[IsTemp] ,[OrderNo] ,[ItemParm] ,[ItemName],[ItemNo],[RedLineCount],[PreSetCount],[ItemTech])";
                sqlCmd += " values ('" + ddl_line.SelectedValue + "','" + Label_sn.Text + "','" + DateTime.Now + "',1,'" + curUser + "','" + numb_planCount.Text + "','" + isTempOrder + "',";
                sqlCmd += "'" + txb_workNo.Text + "','" + TextArea_parm.Text + "','" + txb_itemName.Text + "','" + txb_itemNo.Text + "','" + numb_redCount.Text + "','" + numb_preSet.Text + "','" + txb_itemTech.Text + "')";
                SqlSel.ExeSql(sqlCmd);
                sqlCmd = "select max(id) from PLM_Product_OnLine";
                int onlineId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                sqlCmd = "insert into PLM_Product_Image (OnlineId,ImgURL) values ";
                sqlCmd += "('" + onlineId + "','" + ImgURL + "')";
                SqlSel.ExeSql(sqlCmd);
                //执行关联明细信息的数据插入(单条)
                string sqlStr = "insert into PLM_Product_Rel (orderid,ProdId,PriLevel,OrderCount) values ";
                sqlStr += "('" + Label_sn.Text + "','" + onlineId + "',1,'" + numb_redCount.Text + "')";
                SqlSel.ExeSql(sqlStr);
                Alert.ShowInTop("操作成功！");
                PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}