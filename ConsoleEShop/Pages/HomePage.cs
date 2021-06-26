using System;
using System.Collections.Generic;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    public class HomePage :BasePage, IPage
    {
        public HomePage(IIOService ioService, IDataService dataService, IClient client) :base(ioService, dataService, client)
        {
           
        }
        public override IView ShowPageData()
        {
            
            return new StringView("Home Page!");

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
                            {"login", ()=> Login()},
                            {"product", () => ShowProduct(Param)},
                            {"products", ShowAllProducts},
                        };
                    }
                    break;
                case Roles.RegisteredUser:
                    {
                        return new Dictionary<string, Func<string>>
                        {
                            {"product", () => ShowProduct(Param)},
                            {"products", ShowAllProducts},
                            {"cart", ShowMyCart},
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfo}
                        };
                    }
                    break;
                case
                    Roles.Administrator:
                    {
                        return new Dictionary<string, Func<string>>
                        {
                            {"product", () => ShowProduct(Param)},
                            {"products", ShowAllProducts},
                            {"cart", ShowMyCart},
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfo},
                            {"m users", ManageUsers},
                            {"m orders", ManageOrders},
                            {"m products", ManageProducts}
                        };
                    }
                    break;
                default:
                    return new Dictionary<string, Func<string>>();
            }
        }




    }
}
