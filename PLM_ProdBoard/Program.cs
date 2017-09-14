using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace PLM_ProdBoard
{
    static class Program
    {
        public static string lineId = string.Empty;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new f_setUp());
        }

        //进度条图片属性
        public static Image PressImg(int Press)
        {
            Bitmap bmp = new Bitmap(104, 30); //这里给104是为了左边和右边空出2个像素，剩余的100就是百分比的值
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White); //背景填白色
            //g.FillRectangle(Brushes.Red, 2, 2, this.Press, 26);  //普通效果
            //填充渐变效果
            g.FillRectangle(new LinearGradientBrush(new Point(30, 2), new Point(30, 30), Color.Black, Color.Gray), 2, 2, Press, 26);
            return bmp;
        }

    }
}
