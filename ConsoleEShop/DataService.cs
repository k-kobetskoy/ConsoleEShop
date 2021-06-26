using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ConsoleEShop.Models;

namespace ConsoleEShop
{
    public class DataService : IDataService
    {
        public User GetUserById(int id)
        {
            return Data.Users.FirstOrDefault(u => u.Id == id);
        }
        public void UpdateUserName(User u)
        {
            var user = GetUserById(u.Id);
            user.Name = u.Name;
        }

        public void AddNewProduct(Product product)
        {
            product.Id = Data.Products.Max(p => p.Id) + 1;
            Data.Products.Add(product);
        }
        public void UpdateUserPassword(User u)
        {
            var user = GetUserById(u.Id);
            user.Password = u.Password;
        }

        public void UpdateUserRole(User u)
        {
            var user = GetUserById(u.Id);
            user.Role = u.Role;
        }

        public void UpdateProduct(Product p)
        {
            var product = GetProductById(p.Id);
            product.Name = p.Name;
            product.CategoryId = p.CategoryId;
            product.Description = p.Description;
            product.Price = p.Price;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Data.Users;
        }
        public void UpdateOrderStatus(Order ord)
        {
            var order = GetOrderById(ord.OrderId);
            order.Status = ord.Status;
        }

        public Order GetOrderById(int id)
        {
            return Data.Orders.FirstOrDefault(o => o.OrderId == id);
        }
        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return Data.Orders.Where(o => o.UserId == userId);
        }
        public User AddUser(string name, string password)
        {
            var user = new User
            {
                Name = name,
                Role = Roles.RegisteredUser,
                Password = password,
                Id = Data.Users.Max(u => u.Id) + 1,
            };
            Data.Users.Add(user);
            return user;
        }

        public void AddOrder(Order order)
        {
            int id = 1;
            if (Data.Orders.Any())
                id = Data.Orders.Max(o => o.OrderId) + 1;

            order.OrderId = id;
            Data.Orders.Add(order);
        }
        public Product GetProductByName(string name)
        {
            return Data.Products.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<Product> GetProducts()
        {
            return Data.Products;
        }
        public Product GetProductById(int id)
        {
            return Data.Products.FirstOrDefault(p => p.Id == id);
        }


        public IEnumerable<Category> GeAllCategories()
        {
            return Data.Categories;
        }

        public IEnumerable<Order> GetOrders()
        {
            return Data.Orders;
        }

        public User GetUserByName(string name)
        {
            return Data.Users.FirstOrDefault(u => u.Name == name);
        }
    }
}
