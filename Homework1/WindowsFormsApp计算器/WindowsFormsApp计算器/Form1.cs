using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp计算器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //给下拉框添加选项
        private void Form1_Load(object sender, EventArgs e)
        {  
                comboBox1.Items.Add('+');
                comboBox1.Items.Add('-');
                comboBox1.Items.Add('*');
                comboBox1.Items.Add('/');
        }

    

        //计算方法
        private string calculate(char symbol, double n1, double n2)
        {
            string result = "";
            switch (symbol)
            {
                case '+':
                    result = (n1 + n2).ToString();
                    break;
                case '-':
                    result = (n1 - n2).ToString();
                    break;
                case '*':
                    result = (n1 * n2).ToString();
                    break;
                case '/':
                   result = n2 == 0 ? "除数不能为零。" : (n1 / n2).ToString();
                  
                    break;
                default:
                    break;
            }
            return result;
        }

        //设置点击事件
        private void button1_Click_1(object sender, EventArgs e)
        {   double n1, n2;
            try
            {
               n1 = Convert.ToDouble(textBox1.Text);
               n2 = Convert.ToDouble(textBox2.Text);
                label3.Text = calculate((char)comboBox1.SelectedItem, n1, n2);
            }
            catch(Exception error)
            {
                label3.Text = "出错啦!";
            }
           
          

        }
    }
}
