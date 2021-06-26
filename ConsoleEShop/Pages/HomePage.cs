using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    public class HomePage :BasePage, IPage
    {
        public HomePage(IIOService ioService, IDataService dataService, IClient client) :base(ioService, dataService, client)
        {
            
        }

        private bool firstTime = true;
        
        public IView ShowPageData()
        {
            
            if (firstTime)
            {
                firstTime = false;
                var menu = new MenuView(context, Commands.Keys.ToList());
                return new PageData(menu, ShowPageData());
            }
            return new StringView("Home Page");
            
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
                            {"login", Login},
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
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfoPage}
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
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfoPage},
                            {"m users", ShowManageUsersPage},
                            {"m orders", ShowManageOrdersPage},
                            {"m products", ShowManageProductsPage}
                        };
                    }
                    
                default:
                    return new Dictionary<string, Func<string>>();
            }
        }




    }
}
