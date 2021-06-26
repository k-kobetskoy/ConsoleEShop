using ConsoleEShop.Models;
using System.Collections.Generic;

namespace ConsoleEShop
{
    public interface IDataService
    {
        void AddNewProduct(Product product);
        void AddOrder(Order order);
        User AddUser(string name, string password);
        IEnumerable<Category> GeAllCategories();
        IEnumerable<User> GetAllUsers();
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrders();
        Product GetProductById(int id);
        Product GetProductByName(string name);
        IEnumerable<Product> GetProducts();
        User GetUserById(int id);
        IEnumerable<Order> GetUserOrders(int userId);
        void UpdateOrderStatus(Order ord);
        void UpdateProduct(Product p);
        void UpdateUserName(User u);
        void UpdateUserPassword(User u);
        void UpdateUserRole(User u);
        User GetUserByName(string name);
    }
}