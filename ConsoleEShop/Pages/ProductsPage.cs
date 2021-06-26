using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    public class ProductsPage:BasePage, IPage
    {
        private readonly List<Product> products;
        public User CurrentUser { get; set; }
        public ProductsPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService,client)
        {
            products = dataService.GetProducts().ToList();
            
        }

        public override Dictionary<string, Action> SetCommands()
        {
            CurrentUser = context.CurrentUser;
            
            switch (CurrentUser.Role)
            {
                case Roles.Guest:
                {
                    return new Dictionary<string, Action>
                    {
                        {"register", Register},
                        {"login",()=> Login()},
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
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"byu", () => { AddToCart(Param);}},
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
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfo},
                        {"m users", ManageUsers},
                        {"m orders", ManageOrders},
                        {"m products", ManageProducts},
                        {"byu", () => { AddToCart(Param);}},
                    };
                }
                    
                default:
                    return new Dictionary<string, Action>();
                    
            }
        }

        public override IView ShowPageData()
        {
            return new ProductsView(products);
        }
    }
}

   

