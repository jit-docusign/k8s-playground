using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderApi.Services
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered
    }

    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        Order CreateOrder(int productId, int quantity, decimal productPrice);
    }

    public class OrderService : IOrderService
    {
        private static List<Order> _orders = new();

        public List<Order> GetAllOrders() => _orders;

        public Order GetOrderById(int id) => _orders.FirstOrDefault(o => o.Id == id);

        public Order CreateOrder(int productId, int quantity, decimal productPrice)
        {
            var order = new Order
            {
                Id = _orders.Count + 1,
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = productPrice * quantity,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _orders.Add(order);
            return order;
        }
    }
}
