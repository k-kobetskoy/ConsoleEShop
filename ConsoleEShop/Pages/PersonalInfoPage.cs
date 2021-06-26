﻿using System;
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

        public override Dictionary<string, Func<string>> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    return new Dictionary<string, Func<string>>();
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
                        {"change name", ChangeName},
                        {"change pass", ChangePassword},

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
                    };
                    }
                default:
                    return new Dictionary<string, Func<string>>();
            }
        }


        public string ChangeName()
        {
            var name = communicator.AskForString("Write desired new name", "name", 3);

            if (string.IsNullOrWhiteSpace(name))
            
              return ShowWelcomeInfo("Operation canceled");
              
            

            context.CurrentUser.Name = name;
            dataService.UpdateUserName(context.CurrentUser);
            return ShowWelcomeInfo("Name changed successful");
             
        }

        public string ChangePassword()
        {
            var password = communicator.AskForString("Write desired new password", "password", 3);

            if (string.IsNullOrWhiteSpace(password))
            {
                return ShowWelcomeInfo("Operation canceled");
                 
            }

            context.CurrentUser.Password = password;
            dataService.UpdateUserPassword(context.CurrentUser);
            return ShowWelcomeInfo("Password changed successful");
        }

        public IView ShowPageData()
        {
            return new UserView(context.CurrentUser);
        }
    }
}
