using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IETCsoft.sql;

namespace PLM_ProdBoard
{
    public partial class f_setUp : Form
    {
        public f_setUp()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void f_setUp_Load(object sender, EventArgs e)
        {
            loadLineInfo();
        }

        private void loadLineInfo() 
        {
            string sqlCmd = "select * from PLM_Product_Line";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            DataRow dr = dt.NewRow();
            dr["LineName"] = "全部产线";//
            dr["id"] = "0";
            dt.Rows.InsertAt(dr, 0);//指定起始位置插入
            cbb_line.DataSource = dt;
            cbb_line.DisplayMember = "LineName";
            cbb_line.ValueMember = "id";
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            Program.lineId = cbb_line.SelectedValue.ToString();
            F_board_main f_board_main = new F_board_main();
            this.Hide();
            f_board_main.ShowDialog();
            if (f_board_main.DialogResult == DialogResult.OK)
            {
                this.Dispose();//在子窗体中触发关闭
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
