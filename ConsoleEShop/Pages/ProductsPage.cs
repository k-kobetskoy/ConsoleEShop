using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using ConsoleEShop.Models;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    public class ProductsPage:BasePage, IPage
    {
        private readonly List<Product> products;
       
        public ProductsPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService,client)
        {
            products = dataService.GetProducts().ToList();
            
        }

        public override Dictionary<string, Func<string>> SetCommands()
        {
            
            
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                {
                    return new Dictionary<string, Func<string>>
                    {
                        {"register", Register},
                        {"login",()=> Login()},
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                    };
                }
                    
                case Roles.RegisteredUser:
                {
                    return new Dictionary<string, Func<string>>
                    {
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                        {"cart", ShowMyCartPage},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"byu", ()=> AddToCart(Param)}
                    };
                }
                    
                case
                    Roles.Administrator:
                {
                    return new Dictionary<string, Func<string>>
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
                        {"byu", () =>  AddToCart(Param)},
                    };
                }
                    
                default:
                    return new Dictionary<string, Func<string>> ();
                    
            }
        }


        public string AddToCart(string index = null)
        {
            if (string.IsNullOrWhiteSpace(index))
                index = AskCartItemIndex();

            var parseResult = int.TryParse(index, out var productIndex);
            if (!parseResult)
                return AddToCart();


            if (productIndex>products.Count)
            
               return ShowAbortOperationMessage("There is no product with such index");
               
            

            var product = products[productIndex - 1];
            return SetQuantity(product);
        }

       
        private string AskCartItemIndex()
        {
            var number = client.AskForNumber("Please enter product's index", products.Count);

            if (number < 1)
                return ShowAbortOperationMessage("Operation canceled");
            

            return AddToCart(number.ToString());
        }

        public override IView ShowPageData()
        {
            return new ProductsView(products);
        }
    }
}

   

