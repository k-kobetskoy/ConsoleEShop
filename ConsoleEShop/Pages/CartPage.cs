using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Models;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    public class CartPage : BasePage, IPage
    {


        public CartPage( IDataService dataService, IClient client) : base(dataService, client)
        {
        }

        public override void SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
                case Roles.RegisteredUser:
                    {
                        Commands = new Dictionary<string, Func<string>>
                    {
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                        {"cart", ShowMyCartPage},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"checkout", Checkout},
                        {"cancel",ClearCart},
                        {"quantity",()=> EditQuantity()},
                        {"remove",()=> RemoveItemFromCart()},
                    };
                        break;
                    }
                
                case
                    Roles.Administrator:
                    {
                        Commands = new Dictionary<string, Func<string>>
                    {
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                        {"cart", ShowMyCartPage},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"m users", ShowManageUsersPage},
                        {"m orders", ShowManageOrdersPage},
                        {"m products", ShowManageProductsPage},
                        {"checkout", Checkout},
                        {"cancel",ClearCart},
                        {"quantity",()=> EditQuantity()},
                        {"remove",()=> RemoveItemFromCart()},
                    };
                        break;
                    }
                default:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
            }
        }
        public string EditQuantity(string index = null)
        {
            if (string.IsNullOrWhiteSpace(index))
                index = client.AskForNumber("Please enter № of product in cart you want to edit", context.Cart.Items.Count).ToString();
            if (string.IsNullOrWhiteSpace(index))
                return ShowAbortOperationMessage( "Operation canceled");
               
            

            var parseResult = int.TryParse(index, out var productIndex);
            if (productIndex > context.Cart.Items.Count)
                return ShowAbortOperationMessage("There is no product with such index"); 
            

            if (parseResult && productIndex > 0)
            {
                var newQuantity = client.AskForNumber("Please enter desired new quantity");

                if (newQuantity < 1)
                
                   return ShowAbortOperationMessage("Operation canceled");
                

                context.Cart.Items[productIndex - 1].Quantity = newQuantity;
            }
            return ShowAbortOperationMessage("Incorrect input"); 
        }

        public string RemoveItemFromCart(string index = null)
        {
            if (string.IsNullOrWhiteSpace(index))
                index = client.AskForNumber("Please enter № of product in cart you want to remove", context.Cart.Items.Count).ToString();

            if (string.IsNullOrWhiteSpace(index))
            {
                return ShowAbortOperationMessage("Operation canceled");
            }

            var parse = int.TryParse(index, out int itemIndexToRemove);
            if (!parse || itemIndexToRemove < 1)
            {
                return ShowAbortOperationMessage("Incorrect input"); 
            }

            if (itemIndexToRemove > context.Cart.Items.Count)
            {
                return ShowAbortOperationMessage("There is no product with such index");
            }


            context.Cart.Items.RemoveAt(itemIndexToRemove - 1);
           return ShowWelcomeInfo("Item removed successfuly");
        }
        public string Checkout()
        {
            if (context.Cart == null || context.Cart.ItemsCount == 0)
                return ShowAbortOperationMessage("You have nothing to checkout");
               
            

            var order = new Order()
            {
                OrderItems = context.Cart.Items,
                Status = OrderStatus.New,
                UserId = context.CurrentUser.Id
            };
            dataService.AddOrder(order);
            context.SetCart();
           return ShowWelcomeInfo("Order was made successfully");
        }

        public string ClearCart()
        {
            if (context.Cart == null || context.Cart.ItemsCount == 0)
            {
               return ShowWelcomeInfo("Cart is empty");
                
            }

            context.Cart.Items = new List<CartItem>();
          return  ShowWelcomeInfo("All items was removed from your cart");
           
        }
        public override IView ShowPageData()
        {
            return new CartView(context.Cart, dataService);
        }


    }
}
