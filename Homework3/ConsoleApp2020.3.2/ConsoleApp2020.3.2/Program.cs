using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2020._3._2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            Console.WriteLine("功能一：创建Rectangle，Square，Triangle\n");
            while (flag){
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("键入1创建Rectangle，键入2创建Square，键入3创建Triangle,键入q进入功能二\n");
                string key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        Console.WriteLine("请输入长方形的长");
                        double length = double.Parse(Console.ReadLine());
                        Console.WriteLine("请输入长方形的宽");
                        double width = double.Parse(Console.ReadLine());
                        Shape shape = Factory.creatShape("Rectangle", new double[] { length, width });
                        if (shape != null) Console.WriteLine("创建成功，其面积为：" + shape.getS());
                        else Console.WriteLine("创建失败！");
                        break;

                    case "2":
                        Console.WriteLine("请输入正方形的边长");
                        double side = double.Parse(Console.ReadLine());
                        Shape shape2 = Factory.creatShape("Square", new double[] { side });
                        if (shape2 != null) Console.WriteLine("创建成功，其面积为：" + shape2.getS());
                        else Console.WriteLine("创建失败！");
                        break;

                    case "3":
                        Console.WriteLine("请输入三角形的第一条边");
                        double side1 = double.Parse(Console.ReadLine());
                        Console.WriteLine("请输入三角形的第二条边");
                        double side2 = double.Parse(Console.ReadLine());
                        Console.WriteLine("请输入三角形的第三条边");
                        double side3 = double.Parse(Console.ReadLine());
                        Shape shape3 = Factory.creatShape("Triangle", new double[] { side1, side2, side3 });
                        if (shape3 != null) Console.WriteLine("创建成功，其面积为：" + shape3.getS());
                        else Console.WriteLine("创建失败！");
                        break;
                    case "q":
                        flag = false;
                        break;
                    default:
                        {
                            Console.WriteLine("请重新选择！");
                            
                            break;
                        }

                }
            } 
           


            Console.WriteLine("功能二：随机创建十个对象并计算面积之和\n");
            Random r = new Random();
            int num_Rectangle = r.Next(10);
            System.Threading.Thread.Sleep(100);
            int num_Square = r.Next(0, 10- num_Rectangle);
            System.Threading.Thread.Sleep(100);
            int num_Triangle = 10 - num_Rectangle - num_Square;
            int num = 0;double s = 0;
            while (num< num_Rectangle)
            {
                Rectangle f =(Rectangle)Factory.CreateRandomShape("Rectangle");
                Console.WriteLine("创建成功一个长方形，长为" + f.Length + "，宽为：" + f.Width + ",面积为:" + f.getS());
                System.Threading.Thread.Sleep(100);
                s += f.getS();
                num++;
            }
            num = 0;
            while (num < num_Square)
            {
                Square f = (Square)Factory.CreateRandomShape("Square");
                Console.WriteLine("创建成功一个正方形，边长为" + f.Length  + ",面积为:" + f.getS());
                System.Threading.Thread.Sleep(100);
                s += f.getS();
                num++;
            }
            num = 0;
            while (num < num_Triangle)
            {
                Triangle f = (Triangle)Factory.CreateRandomShape("Triangle");
                Console.WriteLine("创建成功一个三角形，三边长为" + f.A+"、"+f.B+"、"+f.B+ ",面积为:" + f.getS());
                System.Threading.Thread.Sleep(100);
                s += f.getS();
                num++;
            }
            Console.WriteLine("总面积为：" + s);


            Console.ReadKey();

        }

        abstract class Shape
        {
            public abstract double getS();
        }

        class Rectangle : Shape
        {
            private double length;
            private double width;

            public double Length
            {
                get
                {
                    return length;
                }
                set
                {
                    if (value > 0 && value > width) length = value;
                    else Console.WriteLine("长度赋值失败");
                }
            }
            public double Width
            {
                get
                {
                    return width;
                }
                set
                {
                    if (value > 0 && value < length) width = value;
                    else Console.WriteLine("宽度赋值失败");
                }
            }

            public Rectangle(double length, double width)
            {
                Length = length;
                Width = width;

            }

            public override double getS()
            {
                return width * length;
            }
        }

        class Square : Shape
        {
            private double length;
            public double Length
            {
                get
                {
                    return length;
                }
                set
                {
                    if (value > 0) length = value;
                    else Console.WriteLine("边长赋值失败。");
                }
            }

            public Square(double length)
            {
                Length = length;
            }
            public override double getS()
            {
                return length * length;
            }
        }

        class Triangle : Shape
        {
            private double a, b, c;
            public double A
            {
                get
                {
                    return a;
                }
                set
                {
                    if (value + b > c && value + c > b && b + c > value) a = value;
                    else if (b == 0 && c == 0) a = value;
                    else Console.WriteLine("赋值失败");
                }
            }
            public double B
            {
                get
                {
                    return b;
                }
                set
                {
                    if (a + value > c && a + c > value && value + c > a) b = value;
                    else if (c == 0) b = value;
                    else Console.WriteLine("赋值失败");
                }
            }
            public double C
            {
                get
                {
                    return c;
                }
                set
                {
                    if (a + b > value && a + value > b && b + value > a) c = value;
                    else Console.WriteLine("赋值失败");
                }
            }

            public Triangle(double x, double y, double z)
            {
                A = x;
                B = y;
                C = z;
            }

            public override double getS()
            {
                return 0.25 * Math.Sqrt((a + b + c) * (a + b - c) * (a + c - b) * (b + c - a));
            }
        }

        class Factory
        {
            public static Shape creatShape(string type, params double[] args)
            {
                Shape s=null;
                switch (type)
                {
                    case "Rectangle":
                            if (args[0] > args[1] && args[1] > 0)
                            s=new Rectangle(args[0], args[1]);
                            break;

                    case "Square":
                           if (args[0] > 0) s=new Square(args[0]);
                           break;

                    case "Triangle":
                           if (args[0] < args[1] + args[2] && args[1] < args[0] + args[2] &&
                           args[2] < args[0] + args[1] && args[0] > 0 && args[1] > 0 && args[2] > 0)
                           s=new Triangle(args[0], args[1], args[2]);
                            break;
                            
                   default: break;

                }
              
                return s;
            }

            public static Shape CreateRandomShape(string type)
            {
                Shape s=null;
                Random r = new Random();
                bool flag = true;
                switch (type)
                {
                    case "Rectangle":
                        while (flag)
                        {
                            double r1 = 10 * r.NextDouble();
                            double r2 = 10 * r.NextDouble();

                             s = Factory.creatShape("Rectangle", new double[] { r1, r2 });
                            if (s != null)  flag = false;
                         }
                         break;
                        
                    case "Square":
                        while (flag)
                        {
                            double r1 = 10 * r.NextDouble();

                             s = Factory.creatShape("Square", new double[] { r1 });
                            if (s != null) flag = false;
                        }
                        break;
                    case "Triangle":
                        while (flag)
                        {
                            double r1 = 10 * r.NextDouble();
                            double r2 = 10 * r.NextDouble();
                            double r3 = 10 * r.NextDouble();

                             s = Factory.creatShape("Triangle", new double[] { r1, r2,r3 });
                            if (s != null) flag = false;
                        }
                        break;
                    default:Console.Write("类型错误"); break;

                }
                return s;
            }
            

        }
    }
}
