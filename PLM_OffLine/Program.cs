using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PLM_OffLine
{
    static class Program
    {
        public static string lineGroup = "";//当前组别
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new f_OffLine_config());
        }
    }
}
