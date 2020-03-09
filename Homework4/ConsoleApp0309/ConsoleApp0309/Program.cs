using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp0309
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericList<int> mylist = new GenericList<int>();
            for (int i= 1; i <=10 ; i++)
            {
                mylist.Add(i);
            }

            mylist.ForEach(x => Console.WriteLine(x));

            int max = mylist.Head.Data;
            mylist.ForEach(x => max = x > max ? x : max);
            
            int min = mylist.Head.Data;
            mylist.ForEach(x => min = x < min ? x : min);
            
            int num = 0;
            mylist.ForEach(x => num += x);
            Console.WriteLine($"最大值為{max}，最小值為{min}，合為{num}。");

            Console.ReadKey();

        }
    }

    public class Node<T>
    {
        public Node<T> Next { get; set; }
        public T Data { get; set; }

        public  Node(T t)
        {
            Next = null;
            Data = t;
        }
    }
    public class GenericList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public GenericList()
        {
            tail = head = null;
        }

        public Node<T> Head
        {
            get => head;
        }
        public void Add(T t)
        {
            Node<T> n = new Node<T>(t);
            if (tail == null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next = n;
                tail = n;
            }
        }
        public void ForEach(Action<T> action)
        {
            Node<T> flag = head;
            while (head.Next != null)
            {
                action(flag.Data);
                flag = flag.Next;
            }
        }
    }
}
