using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp0309_02
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Write("設定的時間為2020-03-09 16:45:00\n");
           
            //該操設置時間           
            DateTime dt = DateTime.Now;
            string strNow = dt.ToString("2020-03-09 16:49:00");
            dt = DateTime.Parse(strNow);

            Form form = new Form();
            form.clock.work(dt);
            Console.ReadKey();

        }
    }

    //使用事件机制，模拟实现一个闹钟功能。
    //闹钟可以有嘀嗒（Tick）事件和响铃（Alarm）两个事件。
    //在闹钟走时时或者响铃时，在控制台显示提示信息。
    public delegate void ClickHandler();
    
    public class myClock
    {
        public event ClickHandler Tick;
        public event ClickHandler Alarm;

        public void work(DateTime time)
        {
            bool flag=false;
            while (true)
            {
                if(DateTime.Now < time||flag)
                {
                    Tick();
                    Thread.Sleep(1000);
                }
                else {
                    Alarm();
                    flag = true;
                    Thread.Sleep(1000);
                }
                
            }
           
        }

    }

    public class Form
    {
        public myClock clock = new myClock();

        public Form()
        {
            clock.Tick += clockTick;
            clock.Alarm += clockAlarm;

        }

        void clockTick()
        {
            Console.Write("Tick\n");
        }

        void clockAlarm()
        {
            Console.Write("Ding~~~!!!!\n");
        }
    }
}
