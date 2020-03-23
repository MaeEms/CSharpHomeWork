using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Order_management_System
{
    public class Program
    {
        static void Main(string[] args)
        {
            //五种货物
            Goods apple = new Goods("apple", 10.8);
            Goods banana = new Goods("banana", 22.8);
            Goods water = new Goods("water", 9.8);
            Goods cake = new Goods("cake", 49.99);
            Goods pencil = new Goods("pencil", 5.0);
            //四位顾客
            Customer Mike = new Customer("Mike", "New York");
            Customer Ammy = new Customer("Ammy", "London");
            Customer LiHua = new Customer("LiHua", "Shanghai");
            Customer Koya = new Customer("Koya", "Tokyo");

            OrderService os = new OrderService();

            Order order1 = new Order(1, Mike);
            order1.AddGoods(apple, 5);
            order1.AddGoods(banana, 3);
            order1.AddGoods(cake, 1);
            os.AddOrder(order1);

            Order order2 = new Order(2, Koya);
            order2.AddGoods(apple, 5);
            order2.AddGoods(pencil, 6);
            order2.AddGoods(water, 8);
            os.AddOrder(order2);

            Order order3 = new Order(3, LiHua);
            order3.AddGoods(pencil, 3);
           // order3.AddGoods(pencil, 4);     会抛异常：“货品添加重复”
            order3.AddGoods(cake, 1);
            order3.AddGoods(water, 2);
            os.AddOrder(order3);

            Order order4 = new Order(4, Ammy);
            order4.AddGoods(apple, 7);
            order4.AddGoods(pencil, 8);
            order4.AddGoods(water, 3);
            os.AddOrder(order4);

            Order order5 = new Order(5, Ammy);
            order5.AddGoods(cake, 2);
            order5.AddGoods(pencil, 15);
            order5.AddGoods(water, 3);
            os.AddOrder(order5);

            //展示所有订单
            os.QueryOrders();
            
            //进行序列化
            os.Export("Orders");
            Console.WriteLine("\n将所有订单序列化");
            Console.WriteLine(File.ReadAllText("orders.xml"));
            //将所有的订单删除
            Console.WriteLine("将除订单1以外的所有订单删除\n");
            os.DeleteOrderByID(2);
            os.DeleteOrderByID(3);
            os.DeleteOrderByID(4);
            os.DeleteOrderByID(5);
            Console.WriteLine("现在还剩的订单是");
            os.QueryOrders();
            //将存在orders.xml中的订单导入
            Console.WriteLine("将所有订单导入\n");
            os.Import("Orders");
            os.QueryOrders();
            Console.ReadKey();
            


        }
    }

    [Serializable]
     public class Goods
    {
        public string GoodsName { set; get; }
        public double GoodsPrice { set; get; }

        public Goods() { }
        public Goods(string name,double price)
        {
            GoodsName = name;
            GoodsPrice = price;
        }
        public override string ToString()
        {
            return $"{GoodsName}   ${GoodsPrice}";
        }
    }

    [Serializable]
   public class Customer
    {
        public string name { set; get; }
        public string address { set; get; }

        public Customer() { }
        public Customer(string name,string address)
        {
            this.name = name;
            this.address = address;
        }
        public override string ToString()
        {
            return $"{name} {address}";
        }
    }

    [Serializable]
    public class OrderItem
    {
        public Goods Good { set; get; }
        public int Count { set; get; }

        public OrderItem() { }
        public OrderItem(Goods Good,int Count)
        {
            this.Good = Good;
            this.Count = Count;
        }

        public override bool Equals(object obj)
        {
            OrderItem oi = obj as OrderItem;
            if (oi == null) return false;
            else return Good.GoodsName == oi.Good.GoodsName;
        }

        public override int GetHashCode()
        {
            return Good.GetHashCode()+100*Count;
        }

        public double GetCount()
        {
            return Good.GoodsPrice* Count;
        }
        public override string ToString()
        {
            
            return $"{Good}     {Count}     {GetCount()}";
        }
    }

    [Serializable]
    public class Order
    {
        public int ID { set; get; }
        public List<OrderItem> Goods { set; get; }
        public Customer Customer { set; get; }
        public DateTime Time { set; get; }

        public Order() { }

        public Order(int ID, Customer Customer) 
        {
            this.ID = ID;
            Goods = new List<OrderItem>();
            this.Customer = Customer;
            Time = DateTime.Now;
        }
        public void AddGoods(Goods goods,int count)
        {
            OrderItem oi = new OrderItem(goods, count);
            var query = from s in Goods
                        where s.Equals(oi)
                        select s;

            if (!query.Any()) Goods.Add(oi);
            else throw new Exception("添加货品重复");
        }
        public override bool Equals(Object obj)
        {
            Order or = obj as Order;
            if (or == null) return false;
            else return ID == or.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public double GetCount()
        {
            double Count = 0;
            foreach(OrderItem oi in Goods)
            {
                Count += oi.GetCount();
            }
            return Count;
        }
        public override string ToString()
        {
            
            string order = $"ID：{ID}             \n";
                order += "--------------------------------\n";
               order += $"货物    单价    数量     合计\n";
                foreach(OrderItem o in Goods)
            {
               order += $"{o}                      \n";
            }
                order += "--------------------------------\n";
               order += $"总价为 ${GetCount()}     \n";
                order += $"{Customer}\n";
                order += $"{Time}\n";
                order += "-------------------------------\n";

            return order;

        }
    }

    public class OrderService
    {
        public List<Order> Orders { set; get; }

        public OrderService()
        {
            Orders = new List<Order>();
        }

        //添加一张账单
        public bool AddOrder(Order o)
        {
            if (o == null) return false;
            if (Orders.Contains(o)) return false;
            else Orders.Add(o);
            return true;

        }

        //两种删除账单的方式
        //根据账单ID删除一张账单
        public bool DeleteOrderByID(int id)
        {
            int index = Orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            Orders.RemoveAt(index);
            return true;
        }
        //删除某个人的全部账单
        public bool DeleteCustomerOrder(Customer c)
        {
            int index = Orders.FindIndex(o => o.Customer == c);
            if (index == -1)
                return false;
            Orders.RemoveAt(index);
            return true;
        }


        //更改订单：这里提供了两种方式
        //更改一个订单的订单项
        public bool ChangeOrderItem(int id, List<OrderItem> oilist)
        {
            int index = Orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            else Orders[index].Goods = oilist;
            return true;

        }
        //更改一个订单的顾客
        public bool ChangeOrderCustomerAndAddress(int id, Customer c)
        {
            if (c == null) return false;
            int index = Orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            else Orders[index].Customer = c;
            return true;
        }


        /* 查询订单：这里提供了三种方式*/
        //查询某个订单的内容
        public bool QueryOrder(int id)
        {
            int index = Orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            else Console.WriteLine(Orders[index]);
            return true;

        }

        //查询所有的订单
        public bool QueryOrders()
        {
            if (Orders.ToArray().Length == 0) return false;
            IEnumerable<Order> query = from o in Orders 
                                        orderby o.GetCount()
                                        select o;
            List<Order> Orders2 = query.ToList();
            for (int i = 0; i < Orders2.ToArray().Length; i++)
                Console.WriteLine(Orders2[i]);
            return true;
        }


        //查询某个顾客的所有订单
        public bool QueryOrderByCustomer(Customer c)
        {
            bool flag = false;
            if (c == null) return false;
            if (Orders.ToArray().Length == 0) return false;
            for (int i = 0; i < Orders.ToArray().Length; i++)
                if (Orders[i].Customer.Equals(c))
                {
                    Console.WriteLine(Orders[i]);
                    flag = true;
                }

            return flag;

        }

        //按照ID进行排序
        public bool Sort()
        {
            if (Orders.ToArray().Length == 0) return false;
            var query = from s in Orders
                        orderby s.ID
                        select s;
            Orders = query.ToList();
            return true;
        }

        public void display()
        {
            foreach (Order o in Orders)
            {
                Console.WriteLine(o);
            }
        }


        //依据lambda表达式自定义排序
        public bool Sort(Func<Order, Order, int> func)
        {

            if (Orders.ToArray().Length == 0) return false;
            if (func == null) return false;
            Orders.Sort((a, b) => func(a, b));
            return true;

        }


        //将所有订单序列化为XML文件
        public void Export(string pass)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(pass + ".xml", FileMode.Create))
                xmlserializer.Serialize(fs, Orders);

        }

        //将所有订单反序列化
        public void Import(string pass)
        {

            XmlSerializer xmlserializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(pass + ".xml", FileMode.Open))
                Orders = (List<Order>)xmlserializer.Deserialize(fs);

        }

    }

    
}