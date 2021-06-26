using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Models;

namespace ConsoleEShop.Views
{


    public class ProductsView : IView
    {
        private readonly IEnumerable<Product> products;

        public ProductsView(IEnumerable<Product> products)
        {
            this.products = products;
        }


        public string ShowViewData()
        {
            if (products is null || !products.Any())
                return "There are no any producs to show";

            var sb = new StringBuilder("");
            var heading = $"№  - Название{new string(' ', 22)}Цена\n";
            sb.AppendLine(heading);
            sb.AppendLine(new string('_', heading.Length) + "\n");
            foreach (var product in products)
            {
                sb.AppendLine($"{product.Id:D2} - {product.Name}{new string(' ', 30 - product.Name.Length)}{product.Price}" + "\n");
            }

            return sb.ToString();
        }
    }
    public class ProductView : IView
    {
        private readonly Product product;

        public ProductView(Product product)
        {
            this.product = product;
        }
        public string ShowViewData()
        {
            if (product is null)
            {
                return "There are no products to show";
            }

            var sb = new StringBuilder("");
            sb.Append($"Товар: {product.Name}\n" +
                      $"Цена:{product.Price:F} USD\n" +
                      $"Описание:{product.Description}");
            return sb.ToString();
        }
    }
    public class CartView : IView
    {
        private readonly Cart cart;
        private readonly IDataService dataService;

        public CartView(Cart cart, IDataService dataService)
        {
            this.cart = cart;
            this.dataService = dataService;
        }
        public string ShowViewData()
        {
            if (cart is null || cart.ItemsCount < 1)
            {
                return "There are no products in cart";
            }

            var sb = new StringBuilder("");
            var heading = $"№  - Название{new string(' ', 22)}Цена/ед    х Кол-во\t Цена сумм.\n";
            sb.Append(heading);
            sb.Append(new string('_', heading.Length) + "\n");


            decimal totalPrice = 0;
            var index = 1;
            foreach (var cartItem in cart.Items)
            {
                var product = dataService.GetProductById(cartItem.ProductId);
                sb.Append($"{index++:D2} - " +
                          $"{product.Name}{new string(' ', 30 - product.Name.Length)}" +
                          $"{product.Price}{new string(' ', 10 - product.Price.ToString().Length)} х " +
                          $"{cartItem.Quantity:D2}\t" +
                          $"{product.Price * cartItem.Quantity}");
                totalPrice += product.Price * cartItem.Quantity;
            }

            var footer = $"\t\t\t\t\t\t\tОбщая цена\n\t\t\t\t\t\t\t{totalPrice}";
            sb.Append(new string('_', footer.Length) + "\n");
            sb.Append(footer);

            return sb.ToString();
        }
    }
    public class StringView : IView
    {
        private readonly string message;

        public StringView(string message)
        {
            this.message = message;
        }
        public string ShowViewData()
        {
            return message;
        }
    }
    public class OrdersView : IView
    {
        private readonly IEnumerable<Order> orders;
        private readonly IDataService dataService;

        public OrdersView(IEnumerable<Order> orders, IDataService dataService)
        {
            this.orders = orders;
            this.dataService = dataService;
        }
        public string ShowViewData()
        {
            if (orders is null || !orders.Any())
            {
                return "There are no any orders";
            }

            var sb = new StringBuilder("");
            var index = 1;

            var heading = $"№   - Статус\t\tUser\n";

            sb.Append(heading);
            sb.Append(new string('_', heading.Length));

            foreach (var order in orders)
            {
                var user = dataService.GetUserById(order.UserId);
                sb.Append($"{index++:D2} - {order.Status}\t\t{user.Name}\n");
            }

            return sb.ToString();
        }
    }
    public class UsersView : IView
    {
        private readonly IEnumerable<User> users;

        public UsersView(IEnumerable<User> users)
        {
            this.users = users;
        }
        public string ShowViewData()
        {
            if (users is null || !users.Any())
            {
                return "There are no users to show";
            }

            var sb = new StringBuilder("");
            var heading = $"№    Имя{new string(' ', 22)} Роль \t\tПароль\n";
            sb.Append(heading);
            sb.Append(new string('_', heading.Length) + "\n");
            var index = 1;
            foreach (var user in users)
            {
                sb.Append($"{index++:D2} - {user.Name}{new string(' ', 24 - user.Name.Length)}" +
                          $"{user.Role}{new string(' ', 15 - user.Role.ToString().Length)}" +
                          $"\t{user.Password}\n");
            }

            return sb.ToString();
        }
    }
    public class UserView : IView
    {
        private readonly User user;

        public UserView(User user)
        {
            this.user = user;
        }
        public string ShowViewData()
        {
            if (user is null)
            {
                return "There are no users to show";
            }

            var sb = new StringBuilder("");
            sb.Append($"Name: {user.Name}\n" +
                      $"Role: {user.Role}");
            return sb.ToString();
        }
    }

    public class MenuView : IView
    {
        private readonly EShop context;
        private readonly IEnumerable<string> keys;

        public MenuView(EShop context, IEnumerable<string> keys)
        {
            this.context = context;
            this.keys = keys;
        }
        public string ShowViewData()
        {
            var userName = context.CurrentUser.Name ??= "guest";
            var sb = new StringBuilder("");
            sb.Append($"Hello {userName}! Avaliable commands:\n");
            if (context.Cart != null && context.Cart.ItemsCount > 0)
            {
                var cartInfo = $"Your cart contains {context.Cart.ItemsCount} item(s)";
                sb.Append(new string('-', cartInfo.Length));
                sb.Append(cartInfo);
                sb.Append(new string('-', cartInfo.Length));
            }

            var commandList = string.Join(", ", keys) + "\n";

            sb.Append(commandList);
            sb.Append(new string('-', commandList.Length)+"\n");
            return sb.ToString();
        }
    }

    public class PageData : IView
    {
        private readonly IView menuView;
        private readonly IView pageView;

        public PageData(IView menuView, IView pageView)
        {
            this.menuView = menuView;
            this.pageView = pageView;
        }
        public string ShowViewData()
        {
            return menuView.ShowViewData() + pageView.ShowViewData();
        }
    }


    public interface IView
    {
        string ShowViewData();
    }


}
