using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_management_System
{
    class Program
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

            Console.WriteLine("---查询所有账单，按照消费金额排序---\n");
            os.QueryOrders();
            Console.WriteLine("---查询Ammy的所有订单---\n");
            os.QueryOrderByCustomer(Ammy);
            Console.WriteLine("---将账单按照账单号排序---\n");
            os.Sort((a,b)=>a.ID-b.ID);
            os.display();
            Console.WriteLine("---将账单1的收件人改为李华---\n");
            os.ChangeOrderCustomerAndAddress(1,LiHua);
            os.QueryOrder(1);
            Console.WriteLine("---删除所有李华的订单---\n");
            os.DeleteCustomerOrder(LiHua);
            os.display();
            Console.WriteLine("---四号订单现在的货物是---\n");
            os.QueryOrder(4);
            Console.WriteLine("---货物更改成100个苹果后---\n");
            List<OrderItem> list = new List<OrderItem>();
            OrderItem oi = new OrderItem(apple, 100);
            list.Add(oi);
            os.ChangeOrderItem(4, list);
            os.QueryOrder(4);
            Console.ReadKey();
            

            
           

        }
    }


    class Goods
    {
        public string GoodsName { set; get; }
        public double GoodsPrice { set; get; }

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

    class Customer
    {
        public string name { set; get; }
        public string address { set; get; }

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

    class OrderItem
    {
        public Goods Good { set; get; }
        public int Count { set; get; }

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
            return Good.GetHashCode()+Count;
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

    class Order
    {
        public int ID { set; get; }
        public List<OrderItem> Goods { set; get; }
        public Customer Customer { set; get; }
        public DateTime Time { set; get; }

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

    class OrderService
    {
        public List<Order> Orders { set; get; }

        public OrderService()
        {
            Orders = new List<Order>();
        }

        //添加一张账单
        public void AddOrder(Order o) 
        {
            var query = from s in Orders
                        where s.Equals(o)
                        select s;
            if (!query.Any()) Orders.Add(o);
            else throw new Exception("添加失败");
        }

        //两种删除账单的方式
        //根据账单ID删除一张账单
        public void DeleteOrderByID(int id)
        {
            var query = from s in Orders
                        where s.ID==id
                        select s;
            if (query.Any()) Orders.Remove(query.First());
            else throw new Exception("没有这样的订单");
        }
        //删除某个人的全部账单
        public void DeleteCustomerOrder(Customer c)
        {
            var query = from s in Orders
                        where s.Customer.name == c.name
                        select s;
            if (query.Any()) Orders = Orders.Where(x => x.Customer.name != c.name).ToList();
            else throw new Exception("没有这样的人");
        }


        //更改订单：这里提供了两种方式
        //更改一个订单的订单项
        public void ChangeOrderItem(int id, List<OrderItem> oilist)
        {
            var query = from s in Orders
                        where s.ID == id
                        select s;
            if (query.Any())
            {
                Order or = query.First();
                or.Goods = oilist;
                Orders = Orders.Where(x => x.ID != id).ToList();
                Orders.Add(or);
            }
            else throw new Exception("没有这样的订单");
        }
        //更改一个订单的顾客
        public void ChangeOrderCustomerAndAddress(int id, Customer c)
        {
            var query = from s in Orders
                        where s.ID == id
                        select s;
            if (query.Any())
            {
                Order or = query.First();
                or.Customer = c;
                Orders = Orders.Where(x => x.ID != id).ToList();
                Orders.Add(or);
            }
            else throw new Exception("没有这样的订单");
        }


        /* 查询订单：这里提供了三种方式*/
        //查询某个订单的内容
        public void QueryOrder(int id)
        {
            var query = from s in Orders
                        where s.ID == id
                        select s;
            if (query.Any()) Console.WriteLine(query.First());
            else throw new Exception("未查询到");

        }
        //查询所有的订单
        public void QueryOrders()
        {
            var query = from s in Orders
                        orderby s.GetCount()
                        select s;
            List<Order> ord = query.ToList();
            foreach (Order o in ord)
            {
                Console.WriteLine(o);
            }
        }
        //查询某个顾客的所有订单
        public void QueryOrderByCustomer(Customer c)
        {
            var query = from s in Orders
                        where s.Customer.name == c.name
                        select s;
            List<Order> ol=query.ToList();
            foreach(Order o in ol)
            {
                Console.WriteLine(o);
            }
        }
          
        //按照ID进行排序
        public void Sort()
        {
            var query = from s in Orders
                        orderby s.ID
                        select s;
            Orders = query.ToList();
          

        }

        public void display()
        {
            foreach(Order o in Orders)
            {
                Console.WriteLine(o);
            }
        }
        //依据lambda表达式自定义排序
        public void Sort(Func<Order,Order,int> func)
        {
            Orders.Sort((a,b)=>func(a,b));
            
        }

    }
}