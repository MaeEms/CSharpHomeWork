using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp计算器
{
    //计算单元
    class Calculator
    {
        public static double calculate(double firstnum, double secondnum, String op)
        {
            if (op == "+") return firstnum + secondnum;
            else if (op == "-") return firstnum - secondnum;
            else if (op == "*") return firstnum * secondnum;
            else if (op == "/") return firstnum / secondnum;
            else return 0;
            
        }
    }


    //执行单元
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("这个程序用来计算两个数做一次基本运算的结果\n");
            
            double num1 = 0;
            double num2 = 0;
            String op = "";
            String inputNum = "";
            bool flag=true;

            while (flag)
            {
                Console.WriteLine("请输入第一个数字:");
                inputNum = Console.ReadLine();
                check_and_transf(inputNum, out num1);

                Console.WriteLine("请输入第二个数字");
                inputNum = Console.ReadLine();
                check_and_transf(inputNum, out num2);

                Console.WriteLine("请选择运算：+ - * /");
                inputNum = Console.ReadLine();
                check_op(inputNum,out op);
                


                try
                {
                    if (op == "/" && num2 == 0)     Console.WriteLine("0不能做除数。\n");
                    else
                    {
                        double result = Calculator.calculate(num1, num2, op);
                       if (double.IsNaN(result))
                       Console.WriteLine("此操作将导致数学错误。\n");
                       else 
                        Console.WriteLine("结果是：" + result); 
                    }
                }catch (Exception e)
                {
                    Console.WriteLine("出错了😓");
                }

                          Console.WriteLine("输入q退出程序，输入其他任意字符继续程序：");
                          String n=Console.ReadLine();
                          if (n == "q") flag = false;
            }

           


        }

        //数字检测
        //检测输入的数字是否合法，如果合法，将其转换成double类型
        static void check_and_transf(String inputNum,out double num)
        {
            while (!double.TryParse(inputNum, out num))
            {
                Console.WriteLine("输入的不是有效的数字，请重新输入：");
                inputNum = Console.ReadLine();
            }
          
        }

        //运算符检测
        //检测运算符的合法性，直到输入的运算符合法为止
        static void check_op(String inputNum,out String op)
        {
            while(inputNum != "+" && inputNum != "-" && inputNum != "*" && inputNum != "/")
           {
                Console.WriteLine("请输入正确的运算符:\n");
                inputNum = Console.ReadLine();
            }
            op = inputNum;
        }
    }
}
