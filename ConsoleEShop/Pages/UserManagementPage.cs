using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    internal class UserManagementPage : BasePage, IPage
    {
        private List<User> Users { get; set; }
        public UserManagementPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService,client)
        {
           
        }

        public override Dictionary<string, Func<string>> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    return new Dictionary<string, Func<string>>();
                case Roles.RegisteredUser:
                    return new Dictionary<string, Func<string>>();
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
                        {"name",SetUserName},
                        {"pass",SetUserPassword},
                        {"role", SetUserRole},

                    };
                    }
                default:
                    return new Dictionary<string, Func<string>>();
                    
            }
        }

        public override IView ShowPageData()
        {
            Users = dataService.GetAllUsers().ToList();

            return new UsersView(Users);
        }


        public string SetUserName()
        {
            var number = client.AskForNumber("Please enter user's index wich name you want to change", Users.Count);
            if (number > 0)
            {
                var newName = client.AskForString("Please enter desired new name", "name", 3);

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    var user = Users[number - 1];
                    user.Name = newName;
                    dataService.UpdateUserName(user);
                   return ShowWelcomeInfo("Name changed successfuly");
                   
                }
            }

           return ShowAbortOperationMessage("Operation canceled");
           

        }
        public string SetUserPassword()
        {
            var number = client.AskForNumber("Please enter user's index wich password you want to change", Users.Count);
            if (number > 0)
            {
                var newPass = client.AskForString("Please enter desired new password", "password", 3);

                if (!string.IsNullOrWhiteSpace(newPass))
                {
                    var user = Users[number - 1];
                    user.Password = newPass;
                    dataService.UpdateUserPassword(user);
                   return ShowWelcomeInfo("Password set successfuly");
                    
                    
                }
            }

           return ShowAbortOperationMessage("Operation canceled");
        }
        public string SetUserRole()
        {
            var number = client.AskForNumber("Please enter user's index wich role you want to change");
            if (number > 0 && number <= Users.Count)
            {
                var i = 1;
                foreach (var role in Enum.GetValues(typeof(Roles)))
                {
                   ioService.Write($"{i++:D2} - {role}");
                }
                var roleNumber = client.AskForNumber("Please enter desired role's number", i);
                if (roleNumber>0)
                {
                    var user = Users[number - 1];
                    user.Role = (Roles) roleNumber;
                    dataService.UpdateUserRole(user);
                   return ShowWelcomeInfo("Role set successfuly");
                     
                    
                }
            }

           return ShowAbortOperationMessage("Operation canceled");
             
        }


    }
}
