using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
    class PersonalInfoPage : BasePage, IPage
    {
        public PersonalInfoPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService, client)
        {
        }

        public override void SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    Commands  = new Dictionary<string, Func<string>>();
                    break;
                case Roles.RegisteredUser:
                    {
                        Commands  =new Dictionary<string, Func<string>>
                    {
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                        {"cart", ShowMyCartPage},
                        {"orders", ShowMyOrdersPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"change name", ChangeName},
                        {"change pass", ChangePassword},

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
                    };
                        break;
                    }
                default:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
            }
        }


        public string ChangeName()
        {
            var name = client.AskForString("Write desired new name", "name", 3);

            if (string.IsNullOrWhiteSpace(name))
            
              return ShowAbortOperationMessage("Operation canceled");
              
            

            context.CurrentUser.Name = name;
            dataService.UpdateUserName(context.CurrentUser);
            return ShowWelcomeInfo("Name changed successful");
             
        }

        public string ChangePassword()
        {
            var password = client.AskForString("Write desired new password", "password", 3);

            if (string.IsNullOrWhiteSpace(password))
            {
                return ShowAbortOperationMessage("Operation canceled");
                 
            }

            context.CurrentUser.Password = password;
            dataService.UpdateUserPassword(context.CurrentUser);
            return ShowWelcomeInfo("Password changed successful");
        }

        public override IView ShowPageData()
        {
            return new UserView(context.CurrentUser);
        }
    }
}
