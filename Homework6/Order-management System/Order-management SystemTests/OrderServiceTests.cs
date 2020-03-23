using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order_management_System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_management_System.Tests
{

    

    [TestClass()]
    public class OrderServiceTests
    {

        OrderService os = new OrderService();

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


        [TestInitialize]
        public void Initialize()
        {
            Order order = new Order(1, Mike);
            order.AddGoods(apple, 2);
            order.AddGoods(banana, 3);
            order.AddGoods(cake, 1);

            os.AddOrder(order);
        }
        



        //添加重复编号订单
        [TestMethod()]
        
        public void AddOrderTest()
        {
            Order order = new Order(1, LiHua);
            Assert.AreEqual(os.AddOrder(order), false);
              
        }

        //加入一个空订单
        [TestMethod()]
        public void AddOrderTest2()
        {
            Order order = null;
            Assert.AreEqual(os.AddOrder(order), false);

        }

        //正常情况希望加入成功
        [TestMethod()]     
        public void AddOrderTest3()
        {
            Order order1 = new Order(6, Mike);
            os.AddOrder(order1);                                      
            Assert.AreEqual( os.Orders.Contains(order1),true);
            
        }

        //正常情况希望删除成功
        [TestMethod()]
        public void DeleteOrderByIDTest()
        {
            Assert.AreEqual(os.DeleteOrderByID(1), true);

        }

        //删除不存在的订单
        [TestMethod()]
        public void DeleteOrderByIDTest2()
        {
            Assert.AreEqual(os.DeleteOrderByID(8), false);

        }

        //正常情况希望根据顾客删除订单
        [TestMethod()]
        public void DeleteCustomerOrderTest()
        {
            Assert.AreEqual(os.DeleteCustomerOrder(Mike),true);
        }

        //顾客不存在
        [TestMethod()]
        public void DeleteCustomerOrderTest2()
        {
            Assert.AreEqual(os.DeleteCustomerOrder(Koya), false);
        }

        //正常情况希望成功改变表项
        [TestMethod()]
        public void ChangeOrderItemTest()
        {
            OrderItem oi1 = new OrderItem(pencil, 5);
            OrderItem oi2 = new OrderItem(water, 3);
            List<OrderItem> oi = new List<OrderItem>();
            oi.Add(oi1);
            oi.Add(oi2);
            Assert.AreEqual(os.ChangeOrderItem(1, oi), true);


        }

        //正常情况希望成功改变顾客
        [TestMethod()]
        public void ChangeOrderCustomerAndAddressTest()
        {
            Assert.AreEqual(os.ChangeOrderCustomerAndAddress(1, Koya), true);

         }

        //改变的顾客不能为空
        [TestMethod()]
        public void ChangeOrderCustomerAndAddressTest2()
        {
            Assert.AreEqual(os.ChangeOrderCustomerAndAddress(1, null), false);

        }

        //订单不存在时
        [TestMethod()]
        public void QueryOrderTest()
        {
            Assert.AreEqual(os.QueryOrder(2), false);
        }

        //没有任何订单时
        [TestMethod()]
        public void QueryOrdersTest()
        {                    
            OrderService os2 = new OrderService();
            Assert.AreEqual(os2.QueryOrders(), false);   
        }

        //没有这样顾客的订单时
        [TestMethod()]
        public void QueryOrderByCustomerTest()
        {
            Assert.AreEqual(os.QueryOrderByCustomer(Ammy),false);

        }

        //顾客为空值时
        [TestMethod()]
        public void QueryOrderByCustomerTest2()
        {
            Mike = null;
            Assert.AreEqual(os.QueryOrderByCustomer(Mike), false);

        }

        //正常情况，希望按照id排序
        [TestMethod()]
        public void SortTest()
        {
            OrderService os2 = new OrderService();
            Order o1 = new Order(1, Mike); os2.AddOrder(o1);
            Order o3 = new Order(3, Koya); os2.AddOrder(o3);
            Order o2 = new Order(2, Ammy); os2.AddOrder(o2);
            os2.Sort();
            Assert.AreEqual(os2.Orders[1].ID==o2.ID, true);
            
        }
        

      
        //正常情况希望按照id排序
        [TestMethod()]
        public void SortTest1()
        {
            OrderService os2 = new OrderService();
            Order o1 = new Order(1, Mike); os2.AddOrder(o1);
            Order o3 = new Order(3, Koya); os2.AddOrder(o3);
            Order o2 = new Order(2, Ammy); os2.AddOrder(o2);
            os2.Sort((a,b)=>a.ID-b.ID);
            Assert.AreEqual(os2.Orders[1].ID == o2.ID, true);

        }

        //当lambda表达式为空
        [TestMethod()]
        public void SortTest2()
        {
            OrderService os2 = new OrderService();
            Order o1 = new Order(1, Mike); os2.AddOrder(o1);
            Order o3 = new Order(3, Koya); os2.AddOrder(o3);
            Order o2 = new Order(2, Ammy); os2.AddOrder(o2);
            os2.Sort(null);
            Assert.AreEqual(os2.Orders[1].ID == o2.ID, false);
        }


        //成功序列化
        [TestMethod()]
        public void ExportTest()
        {
            os.Export("order");
            FileInfo fileInfo = new FileInfo("order.xml");
            Assert.IsTrue(fileInfo.Exists);
        }

        
        //成功反序列化
        [TestMethod()]
        public void ImportTest()
        {
            os.Export("test");
            OrderService os2 = new OrderService();
            os2.Import("test");
            Assert.AreEqual(os.Orders.ToArray().Length, os2.Orders.ToArray().Length);
            for(int i=0; i< os.Orders.ToArray().Length;i++)
            Assert.AreEqual(os.Orders[i], os2.Orders[i]);
        }

    }
}