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
    public partial class NumbKeyBoard : Form
    {
        static string inputNumb = string.Empty;

        public NumbKeyBoard()
        {
            InitializeComponent();
        }

        //键盘1
        private void button1_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "9";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr.Substring(0, curStr.Length - 1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string curStr = textBox1.Text;
            textBox1.Text = curStr + "0";
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

    }
}
