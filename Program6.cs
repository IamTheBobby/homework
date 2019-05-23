using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ordertest
{
    public class Customer
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public Customer() { }

        public Customer(uint id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"customerId:{Id}, CustomerName:{Name}";
        }

        public override bool Equals(object obj)
        {
            var customer = obj as Customer;
            return customer != null &&
                   Id == customer.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }

    public class Goods
    {

        private float price;

        public Goods() { }

        public Goods(uint id, string name, float price)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public float Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value must >= 0!");
                price = value;
            }
        }

        public override bool Equals(object obj)
        {
            var goods = obj as Goods;
            return goods != null &&
                   Id == goods.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, Value:{Price}";
        }
    }

    [Serializable]
    public class Order : IComparable<Order>
    {

        private List<OrderDetail> details = new List<OrderDetail>();

        public Order(){
        }

        public Order(int orderId, Customer customer)
        {
            Id = orderId;
            Customer = customer;
        }

        public int Id { get; set; }

        public Customer Customer { get; set; }

        public float TotalAmount
        {
            get => details.Sum(d => d.Amount);
        }

        public List<OrderDetail> Details
        {
            get => this.details;
        }

        public void AddDetails(OrderDetail orderDetail)
        {
            if (this.Details.Contains(orderDetail))
            {
                throw new Exception($"orderDetail of the goods ({orderDetail.Goods.Name}) exists in order {Id}");
            }
            details.Add(orderDetail);
        }

        public int CompareTo(Order other)
        {
            if (other == null) return 1;
            return Id - other.Id;
        }

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null &&
                   Id == order.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public void RemoveDetails(int num)
        {
            details.RemoveAt(num);
        }

        public override string ToString()
        {
            String result = $"orderId:{Id}, customer:({Customer})";
            details.ForEach(detail => result += "\n\t" + detail);
            return result;
        }


    }

    public class OrderDetail
    {
        public OrderDetail() { }

        public OrderDetail(Goods goods, uint quantity)
        {
            this.Goods = goods;
            this.Quantity = quantity;
        }

        public Goods Goods { get; set; }

        public uint Quantity { get; set; }

        public float Amount
        {
            get => Goods.Price * Quantity;
        }

        public override bool Equals(object obj)
        {
            var detail = obj as OrderDetail;
            return detail != null &&
                   EqualityComparer<Goods>.Default.Equals(Goods, detail.Goods);
        }

        public override int GetHashCode()
        {
            return 785010553 + EqualityComparer<Goods>.Default.GetHashCode(Goods);
        }

        public override string ToString()
        {
            return $"orderDetail:{Goods},{Quantity}";
        }


    }

    public class OrderService
    {

        private List<Order> orderList;

        public OrderService()
        {
            orderList = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (orderList.Contains(order))
            {
                throw new Exception($"the orderList contains an order with ID {order.Id} !");
            }
            orderList.Add(order);
        }

        public void Update(Order order)
        {
            RemoveOrder(order.Id);
            orderList.Add(order);
        }

        public Order GetById(int orderId)
        {
            return orderList.FirstOrDefault(o => o.Id == orderId);
        }

        public void RemoveOrder(int orderId)
        {
            orderList.RemoveAll(o => o.Id == orderId);
        }

        public List<Order> QueryAll()
        {
            return orderList;
        }

        public List<Order> QueryByGoodsName(string goodsName)
        {
            var query = orderList.Where(
              o => o.Details.Exists(
                d => d.Goods.Name == goodsName));
            return query.ToList();
        }

        public List<Order> QueryByTotalAmount(float totalAmount)
        {
            var query = orderList.Where(o => o.TotalAmount >= totalAmount);
            return query.ToList();
        }

        public List<Order> QueryByCustomerName(string customerName)
        {
            var query = orderList
                .Where(o => o.Customer.Name == customerName);
            return query.ToList();
        }

        public void Sort(Comparison<Order> comparison)
        {
            orderList.Sort(comparison);
        }

        public void Export(String fileName)
        {
            if (Path.GetExtension(fileName) != ".xml")
                throw new ArgumentException("the exported file must be a xml file!");
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, this.orderList);
            }
        }

        public List<Order> Import(string path)
        {
            if (Path.GetExtension(path) != ".xml")
                throw new ArgumentException($"{path} isn't a xml file!");
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            List<Order> result = new List<Order>();
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return (List<Order>)xs.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("import error:" + e.Message);
            }

        }

    }

    public class MainClass
    {
        public static void Main()
        {
            try
            {
                Customer customer1 = new Customer(1, "Customer1");
                Customer customer2 = new Customer(2, "Customer2");

                Goods milk = new Goods(1, "Milk", 69.9f);
                Goods eggs = new Goods(2, "eggs", 4.99f);
                Goods apple = new Goods(3, "apple", 5.59f);

                Order order1 = new Order(1, customer1);
                order1.AddDetails(new OrderDetail(apple, 8));
                order1.AddDetails(new OrderDetail(eggs, 10));
                // order1.AddDetails(new OrderDetail(eggs, 8));
                order1.AddDetails(new OrderDetail(milk, 10));

                Order order2 = new Order(2, customer2);
                order2.AddDetails(new OrderDetail(eggs, 10));
                order2.AddDetails(new OrderDetail(milk, 10));

                Order order3 = new Order(3, customer2);
                order3.AddDetails(new OrderDetail(milk, 100));

                OrderService orderService = new OrderService();
                orderService.AddOrder(order1);
                orderService.AddOrder(order2);
                orderService.AddOrder(order3);

                Console.WriteLine("\n GetById");
                Console.WriteLine(orderService.GetById(1));
                Console.WriteLine(orderService.GetById(5) == null);

                Console.WriteLine("\nGetAllOrders");
                List<Order> orders = orderService.QueryAll();
                orders.ForEach(o => Console.WriteLine(o));

                Console.WriteLine("\nGetOrdersByCustomerName:'Customer2'");
                orders = orderService.QueryByCustomerName("Customer2");
                orders.ForEach(o => Console.WriteLine(o));

                Console.WriteLine("\nGetOrdersByGoodsName:'eggs'");
                orders = orderService.QueryByGoodsName("eggs");
                orders.ForEach(o => Console.WriteLine(o));

                Console.WriteLine("\nGetOrdersTotalAmount:1000");
                orders = orderService.QueryByTotalAmount(1000);
                orders.ForEach(o => Console.WriteLine(o));

                Console.WriteLine("\nRemove order(id=2) and qurey all");
                orderService.RemoveOrder(2);
                orderService.QueryAll().ForEach(
                    o => Console.WriteLine(o));

                Console.WriteLine("\n order by Amount");
                orderService.Sort(
                  (o1, o2) => o2.TotalAmount.CompareTo(o1.TotalAmount));
                orderService.QueryAll().ForEach(
                    o => Console.WriteLine(o));



                Console.WriteLine("\n export and import orders");
                orderService.Export(@".\order.xml");
                List<Order> importedOrders = orderService.Import(@".\order.xml");
                importedOrders.ForEach(o => Console.WriteLine(o));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
