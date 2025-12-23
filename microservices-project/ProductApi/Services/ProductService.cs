using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductApi.Services
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        Product CreateProduct(string name, decimal price, string description);
    }

    public class ProductService : IProductService
    {
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1299.99m, Description = "High-performance laptop", CreatedAt = DateTime.UtcNow },
            new Product { Id = 2, Name = "Mouse", Price = 29.99m, Description = "Wireless mouse", CreatedAt = DateTime.UtcNow },
            new Product { Id = 3, Name = "Keyboard", Price = 79.99m, Description = "Mechanical keyboard", CreatedAt = DateTime.UtcNow }
        };

        public List<Product> GetAllProducts() => _products;
        public Product GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product CreateProduct(string name, decimal price, string description)
        {
            var product = new Product
            {
                Id = _products.Max(p => p.Id) + 1,
                Name = name,
                Price = price,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
            _products.Add(product);
            return product;
        }
    }
}
