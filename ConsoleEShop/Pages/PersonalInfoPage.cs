using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    class PersonalInfoPage:BasePage, IPage
    {
        public PersonalInfoPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService,client)
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
                        {"change name", ChangeName},
                        {"change pass", ChangePassword},

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
                    };
                }
                  default: return new Dictionary<string, Action>();
            }
        }

        public override IView ShowPageData()
        {
            return new UserView(context.CurrentUser);
            //ioService.Write($"Name: {context.CurrentUser.Name}\n" +
            //                $"Role: {context.CurrentUser.Role}");
        }
    }
}
