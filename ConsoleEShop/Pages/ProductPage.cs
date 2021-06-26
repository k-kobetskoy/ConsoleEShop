using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleEShop.Views;
using static ConsoleEShop.User;


namespace ConsoleEShop.Pages
{
    public class ProductPage : BasePage,  IPage
    {
        private readonly Product product;
        public ProductPage(IIOService ioService, IDataService dataService,  Product product, IClient client) :base(ioService, dataService,client)
        {
            this.product = product;

           
        }
        public override Dictionary<string, Action> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                {
                    return new Dictionary<string, Action>
                    {
                        {"register", Register},
                        {"login", ()=> Login()},
                        {"product", () => ShowProduct(Param)},
                        {"products", ShowAllProducts},
                    };
                }
                   
                case Roles.RegisteredUser:
                {
                    return new Dictionary<string, Action>
                    {
                        {"product", () => ShowProduct(Param)},
                        {"products", ShowAllProducts},
                        {"cart", ShowMyCart},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"orders", ShowMyOrdersPage},
                        {"byu", () => { AddToCart(product);}},
                    };
                }
                   
                case
                    Roles.Administrator:
                {
                    return new Dictionary<string, Action>
                    {
                        {"product", () => ShowProduct(Param)},
                        {"products", ShowAllProducts},
                        {"cart", ShowMyCart},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"orders", ShowMyOrdersPage},
                        {"m users", ManageUsers},
                        {"m orders", ManageOrders},
                        {"m products", ManageProducts},
                        {"byu", () => { AddToCart(product);}},
                    };
                }
                    
                default:
                    return new Dictionary<string, Action>();
            }
        }

        public override IView ShowPageData()
        {
            return new ProductView(product);
        }

       
      
        
    }
}
