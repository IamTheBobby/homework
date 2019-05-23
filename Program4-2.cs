using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordertest
{
    class Customer
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public Customer(uint id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"customerId:{Id}, CustomerName:{Name}";
        }
    }

    class Goods
    {
        private double price;

        public Goods(uint id, string name, double value)
        {
            Id = id;
            Name = name;
            Price = value;
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public double Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value must >= 0!");
                price = value;
            }
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, Value:{Price}";
        }
    }

    class OrderDetail
    {
        public OrderDetail(uint id, Goods goods, uint quantity)
        {
            this.Id = id;
            this.Goods = goods;
            this.Quantity = quantity;
        }

        public uint Id { get; set; }

        public Goods Goods { get; set; }

        public uint Quantity { get; set; }

        public override bool Equals(object obj)
        {
            var detail = obj as OrderDetail;
            return detail != null &&
                Goods.Id == detail.Goods.Id &&
                Quantity == detail.Quantity;
        }

        public override int GetHashCode()
        {
            var hashCode = 1522631281;
            hashCode = hashCode * -1521134295 + Goods.Name.GetHashCode();
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            string result = "";
            result += $"orderDetailId:{Id}:  ";
            result += Goods + $", quantity:{Quantity}";
            return result;
        }


    }

    class Order
    {

        private List<OrderDetail> details = new List<OrderDetail>();

        public Order(uint orderId, Customer customer)
        {
            Id = orderId;
            Customer = customer;
        }

        public uint Id { get; set; }

        public Customer Customer { get; set; }

        public List<OrderDetail> Details
        {
            get => this.details;
        }

        public void AddDetails(OrderDetail orderDetail)
        {
            if (this.Details.Contains(orderDetail))
            {
                throw new Exception($"orderDetails-{orderDetail.Id} is already existed!");
            }
            details.Add(orderDetail);
        }

        public void RemoveDetails(uint orderDetailId)
        {
            details.RemoveAll(d => d.Id == orderDetailId);
        }

        public override string ToString()
        {
            String result = $"orderId:{Id}, customer:({Customer})";
            details.ForEach(detail => result += "\n\t" + detail);
            return result;
        }
    }

    class OrderService
    {

        private List<Order> orderList;

        public OrderService()
        {
            orderList = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            foreach (Order o in orderList)
            {
                if (o.Id.Equals(order.Id))
                {
                    throw new Exception($"order-{order.Id} is already existed!");
                }
            }
            orderList.Add(order);
        }

        public Order GetById(uint orderId)
        {
            foreach (Order o in orderList)
            {
                if (o.Id == orderId)
                {
                    return o;
                }
            }
            return null;
        }

        public void RemoveOrder(uint orderId)
        {
            Order order = GetById(orderId);
            if (order == null) return;
            orderList.Remove(order);
        }

        public List<Order> QueryAllOrders()
        {
            return orderList;
        }

        public List<Order> QueryByGoodsName(string goodsName)
        {
            List<Order> result = new List<Order>();
            foreach (Order order in orderList)
            {
                foreach (OrderDetail detail in order.Details)
                {
                    if (detail.Goods.Name == goodsName)
                    {
                        result.Add(order);
                        break;
                    }
                }
            }
            return result;
        }

        public List<Order> QueryByCustomerName(string customerName)
        {
            var query = orderList
                .Where(order => order.Customer.Name == customerName);
            return query.ToList();
        }
    }

    class MainClass
    {
        public static void Main()
        {
            try
            {
                Customer customer1 = new Customer(1, "Customer1");
                Customer customer2 = new Customer(2, "Customer2");

                Goods milk = new Goods(1, "Milk", 69.9);
                Goods eggs = new Goods(2, "eggs", 4.99);
                Goods apple = new Goods(3, "apple", 5.59);

                OrderDetail orderDetails1 = new OrderDetail(1, apple, 8);
                OrderDetail orderDetails2 = new OrderDetail(2, eggs, 2);
                OrderDetail orderDetails3 = new OrderDetail(3, milk, 1);

                Order order1 = new Order(1, customer1);
                Order order2 = new Order(2, customer2);
                Order order3 = new Order(3, customer2);
                order1.AddDetails(orderDetails1);
                order1.AddDetails(orderDetails2);
                order1.AddDetails(orderDetails3);
                //order1.AddOrderDetails(orderDetails3);
                order2.AddDetails(orderDetails2);
                order2.AddDetails(orderDetails3);
                order3.AddDetails(orderDetails3);

                OrderService os = new OrderService();
                os.AddOrder(order1);
                os.AddOrder(order2);
                os.AddOrder(order3);

                Console.WriteLine("GetAllOrders");
                List<Order> orders = os.QueryAllOrders();
                foreach (Order order in orders)
                    Console.WriteLine(order.ToString());

                Console.WriteLine("GetOrdersByCustomerName:'Customer2'");
                orders = os.QueryByCustomerName("Customer2");
                foreach (Order order in orders)
                    Console.WriteLine(order.ToString());

                Console.WriteLine("GetOrdersByGoodsName:'apple'");
                orders = os.QueryByGoodsName("apple");
                foreach (Order order in orders)
                    Console.WriteLine(order);

                Console.WriteLine("Remove order(id=2) and qurey all");
                os.RemoveOrder(2);
                os.QueryAllOrders().ForEach(
                    od => Console.WriteLine(od));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
