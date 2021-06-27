using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    public class HomePage :BasePage, IPage
    {
        public HomePage( IDataService dataService, IClient client) :base( dataService, client)
        {
            
        }

        private bool firstTime = true;
        
        public override IView ShowPageData()
        {
            
            if (firstTime)
            {
                firstTime = false;
                var menu = new MenuView(context, Commands.Keys.ToList());
                return new PageData(menu, ShowPageData());
            }
            return new StringView("Home Page");
            
        }
        public override void SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    {
                        Commands = new Dictionary<string, Func<string>>
                        {
                            {"register", Register},
                            {"login", Login},
                            {"product", () => ShowProductPage(Param)},
                            {"products", ShowAllProductsPage},
                        };
                        break;
                    }
                    
                case Roles.RegisteredUser:
                    {
                        Commands = new Dictionary<string, Func<string>>
                        {
                            {"product", () => ShowProductPage(Param)},
                            {"products", ShowAllProductsPage},
                            {"cart", ShowMyCartPage},
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfoPage}
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
                            {"logout", Logout},
                            {"orders", ShowMyOrdersPage},
                            {"user info", ShowMyInfoPage},
                            {"m users", ShowManageUsersPage},
                            {"m orders", ShowManageOrdersPage},
                            {"m products", ShowManageProductsPage}
                        };
                        break;
                    }
                    
                default:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
            }
        }
    }
}
