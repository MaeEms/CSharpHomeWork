using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {    
        public Form1()
        {
            InitializeComponent();
           
        }
        

        private Graphics graphics;

        int n = 10;
        double th1 = 30 * Math.PI / 180;
        double th2 = 20 * Math.PI / 180;
        double per1 = 0.6;
        double per2 = 0.7;
        double leng = 80;
        String color = "Blue";

       
            
        
        void drawCayleyTree(int n ,double x0,double y0,double leng,double th)
        {
            if (n == 0) return;
            double x1 = x0 + leng * Math.Cos(th);
            double y1 = y0 + leng * Math.Sin(th);

            drawLine(x0, y0, x1, y1);

            drawCayleyTree(n - 1, x1, y1, per1 * leng, th + th1);
            drawCayleyTree(n - 1, x1, y1, per2 * leng, th - th2);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
             n = trackBar1.Value;
             leng = trackBar2.Value*10;
            per1 = 0.1 * trackBar3.Value;
            per2 = 0.1 * trackBar4.Value;

            //这里出现了bug，如果连续计算的话，会出现错误
            //th1 = (1 / 18) * Math.PI * trackBar5.Value;
            th1 = Math.PI*trackBar5.Value;
            th1 = th1 / 18;

            th2 = Math.PI * trackBar6.Value;
            th2 = th2 / 18;
            




            if (graphics == null) graphics = panel1.CreateGraphics(); ;
            
            graphics.Clear(panel1.BackColor);

            
           
            drawCayleyTree(n, 150, 250, leng, -Math.PI / 2);
            

        }

        void drawLine(double x0,double y0,double x1,double y1)
        {
            switch (color)
            {
                case "Blue":
                    graphics.DrawLine(Pens.Blue, (int)x0, (int)y0, (int)x1, (int)y1);
                    break;
                case "Green":
                    graphics.DrawLine(Pens.Green, (int)x0, (int)y0, (int)x1, (int)y1);
                    break;
                case "Red":
                    graphics.DrawLine(Pens.Red, (int)x0, (int)y0, (int)x1, (int)y1);
                    break;
                default:
                  break;

            }

            

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            color = "Blue";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            color = "Green";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            color = "Red";
        }
    }
}
