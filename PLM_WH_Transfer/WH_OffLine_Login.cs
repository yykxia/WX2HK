using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PLM_WH_Transfer
{
    public partial class WH_OffLine_Login : Form
    {
        public WH_OffLine_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WH_OffLine_Transfer transfer = new WH_OffLine_Transfer();
            Program.lineGroup = comboBox1.SelectedItem.ToString();
            transfer.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void WH_OffLine_Login_Load(object sender, EventArgs e)
        {
            //添加数据项  
            comboBox1.Items.Add("A");
            comboBox1.Items.Add("B");
        }
    }
}
