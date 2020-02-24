﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("请选择要查看的作业：                          键入0--退出程序   \n");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("1--输出用户指定数据的所有素数因子\n");
            Console.WriteLine("2--求一个整数数组的最大值、最小值、平均值和所有数组元素的和\n");
            Console.WriteLine("3--用“埃氏筛法”求2~ num以内的素数\n");
            Console.WriteLine("------------------------------------------------------------");
            int flag=1;
            while (flag != 0)
            {
                String result="";
                int num;
               Console.WriteLine("请输入选项序号：");
                bool t=int.TryParse(Console.ReadLine(),out num);
                if (t == false) { Console.WriteLine("输入的序号格式不对。"); continue; }
            switch (num)
             {
                 case 1:
                        bool f = false; int n;
                        while (!f)
                        {
                            Console.WriteLine("请输入一个指定整数：");
                            f = int.TryParse(Console.ReadLine(), out n);
                            if (f == false) Console.WriteLine("输入整数的格式不对，请重试！");
                            else task_1(n, ref result);
                        }
                        break;
                    case 2:
                        Console.WriteLine("下列三个数组的测试结果分别是\n");
                        Console.WriteLine("1--{1,2,3,4,5,6,7,8,9}\n");
                        Console.WriteLine("2--{-5,-1,1,5,10,20}\n");
                        Console.WriteLine("3--{-5,-4,-3,-2,-1}\n");
                        int[] array1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                        int[] array2 = { -5, -1, 1, 5, 10, 20 };
                        int[] array3 = { -5, -4, -3, -2, -1 };
                        String result1 = "";String result2 = "";String result3 = "";
                        task_2(array1, ref result1);
                        task_2(array2, ref result2);
                        task_2(array3, ref result3);
                        result = result1 + " " + result2 + " " + result3;
                        break;
                   case 3:
                        bool f1 = false; int n1;
                        while (!f1)
                        {
                            Console.WriteLine("请输入一个指定整数：");
                            f1 = int.TryParse(Console.ReadLine(), out n1);
                            if (f1 == false) Console.WriteLine("输入整数的格式不对，请重试！");
                            else task_3(n1, ref result);
                        }
                         break;

                    case 0:
                        flag = 0;break;
                 default:
                        Console.Write("请输入1、2、3或者0！");break;
             }
                Console.Write(result+"\n");
             
            }
           
                 

            Console.ReadKey();
        }


        //输出用户指定数据的所有素数因子
        static void task_1(int num,ref String result)
        {  int[] array = new int[num];
            result = num+"进行素因数分解为";
            int k = 2;int i = 0;
            while (true)
            {    
                if (k == num)
                {
                    if (i==0||array[i-1] != k)
                    {  
                        array[i] = k;
                        i++;
                    }
                    result += k + ","; break;
                }
              if (num > k && num % k == 0)
                {
                    if (i==0||array[i-1] != k)
                    { 
                        array[i] = k;
                        i++;
                    }
                    result += k + "*";
                    num = num / k;
                 }
              else  k = k + 1;   
            }
            String result2 = "";
            int j = 0;
            while(array[j]!=0) result2 += " " + array[j++];
            result += "故所有素数因子为：" + result2;
        }

        //求一个整数数组的最大值、最小值、平均值和所有数组元素的和
        static void task_2(int[] array,ref String result)
        {  
            int max = int.MinValue;
            int min = int.MaxValue;
            int num = 0;
            int a = int.MinValue;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] > max) max = array[i];
                if (array[i] < min) min = array[i];
                num += array[i];
            }
            result=max + min + num + num * 1.0 / array.Length+"";            
        }

        //用“埃氏筛法”求2~ 100以内的素数
        static void task_3(int num,ref String result)
        {
            //初始化
            bool[] array = new bool[num+1];
            //执行
            for (int i = 2; i * i< num; i++)
            for (int j = 2; i * j <= num; j++)
                    array[i * j] = true;
            array[0] = true;
            array[1] = true;
            //输出
            result = "";
            for (int a = 0; a <= num; a++)
            if (array[a] == false)
            result += a + " ";
            
        }
    }
}
