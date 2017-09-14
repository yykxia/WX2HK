using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PLM_TreatScan.ent
{
    public class OrderInfo
    {
        public string orderNo { get; set; }
        public int Press { get; set; }
        public string ProcessStr { get; set; }
        public string storageLists { get; set; }
        //进度条图片属性
        public Image PressImg
        {
            get
            {
                Bitmap bmp = new Bitmap(104, 30); //这里给104是为了左边和右边空出2个像素，剩余的100就是百分比的值
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Black); //背景填黑色
                g.FillRectangle(Brushes.LightGreen, 0, 2, this.Press, 26);  //普通效果
                //填充渐变效果
                //if (Press < 30) 
                //{
                //    g.FillRectangle(new LinearGradientBrush(new Point(30, 2), new Point(30, 30), Color.Red, Color.Gray), 0, 0, this.Press, 26);
                //}
                //else
                //{
                //    g.FillRectangle(new LinearGradientBrush(new Point(30, 2), new Point(30, 30), Color.LightGreen, Color.Gray), 0, 0, this.Press, 26);
                //}
                return bmp;
            }
        }
    }
}
