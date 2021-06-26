using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    class CartPage : BasePage, IPage
    {
        
      
        public CartPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService, client)
        {
        }

        public override Dictionary<string, Action> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    return new Dictionary<string, Action>();
                case Roles.RegisteredUser:
                    {
                        return new Dictionary<string, Action>
                    {
                        {"product", () => ShowProduct(Param)},
                        {"products", ShowAllProducts},
                        {"cart", ShowMyCart},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"checkout", Checkout},
                        {"cancel",ClearCart},
                        {"quantity",()=> EditQuantity()},
                        {"remove",()=> RemoveItemFromCart()},
                    };
                    }
                    break;
                case
                    Roles.Administrator:
                    {
                        return new Dictionary<string, Action>
                    {
                        {"product", () => ShowProduct(Param)},
                        {"products", ShowAllProducts},
                        {"cart", ShowMyCart},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"m users", ManageUsers},
                        {"m orders", ManageOrders},
                        {"m products", ManageProducts},
                        {"checkout", Checkout},
                        {"cancel",ClearCart},
                        {"quantity",()=> EditQuantity()},
                        {"remove",()=> RemoveItemFromCart()},
                    };
                    }
                    default:
                        return new Dictionary<string, Action>();
            }
        }

        public override IView ShowPageData()
        {

            return new CartView(context.Cart, dataService);

            //if (context.Cart == null||context.Cart?.ItemsCount<1)
            //{
            //    ioService.Write("There are no items in your cart yet");
            //    return;
            //}
            //ioService.Highlight($"№  - Название{new string(' ', 22)}Цена/ед    х Кол-во\t Цена сумм.");
            //decimal totalPrice = 0;
            //var index = 1;
            //foreach (var cartItem in context.Cart.Items)
            //{
            //    var product = dataService.GetProductById(cartItem.ProductId);
            //    ioService.Write($"{index++:D2} - " +
            //                    $"{product.Name}{new string(' ', 30 - product.Name.Length)}" +
            //                    $"{product.Price}{new string(' ', 10 - product.Price.ToString().Length)} х " +
            //                    $"{cartItem.Quantity:D2}\t" +
            //                    $"{product.Price * cartItem.Quantity}");
            //    totalPrice += product.Price * cartItem.Quantity;
            //}
            //ioService.Highlight($"\t\t\t\t\t\t\tОбщая цена\n\t\t\t\t\t\t\t{totalPrice}");
        }

        
    }
}
