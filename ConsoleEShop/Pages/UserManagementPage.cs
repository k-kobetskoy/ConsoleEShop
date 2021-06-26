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

        public override Dictionary<string, Action> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    return new Dictionary<string, Action>();
                case Roles.RegisteredUser:
                    return new Dictionary<string, Action>();
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
                        {"name",SetUserName},
                        {"pass",SetUserPassword},
                        {"role", SetUserRole},

                    };
                    }
                default:
                    return new Dictionary<string, Action>();
                    
            }
        }

        public override IView ShowPageData()
        {
            Users = dataService.GetAllUsers().ToList();

            return new UsersView(Users);
            
            
            
            //if (Users.Count < 1)
            //{
            //    ioService.Write("There are no users to show");
            //    return;
            //}

            //ioService.Highlight($"№    Имя{new string(' ', 22)} Роль \t\tПароль");

            //var index = 1;
            //foreach (var user in Users)
            //{
            //    ioService.Write($"{index++:D2} - {user.Name}{new string(' ', 24 - user.Name.Length)}" +
            //                    $"{user.Role}{new string(' ', 15 - user.Role.ToString().Length)}" +
            //                    $"\t{user.Password}");
            //}
        }


        public void SetUserName()
        {
            var number = communicator.AskForNumber("Please enter user's index wich name you want to change", Users.Count);
            if (number > 0)
            {
                var newName = communicator.AskForString("Please enter desired new name", "name", 3);

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    var user = Users[number - 1];
                    user.Name = newName;
                    dataService.UpdateUserName(user);
                    ShowWelcomeInfo();
                    ioService.Highlight("Name changed successfuly");
                    return;
                }
            }

            AbortOperation();

        }
        public void SetUserPassword()
        {
            var number = communicator.AskForNumber("Please enter user's index wich password you want to change", Users.Count);
            if (number > 0)
            {
                var newPass = communicator.AskForString("Please enter desired new password", "password", 3);

                if (!string.IsNullOrWhiteSpace(newPass))
                {
                    var user = Users[number - 1];
                    user.Password = newPass;
                    dataService.UpdateUserPassword(user);
                    ShowWelcomeInfo();
                    ioService.Highlight("PAssword set successfuly");
                    return;
                }
            }

            AbortOperation();
        }
        public void SetUserRole()
        {
            var number = communicator.AskForNumber("Please enter user's index wich role you want to change");
            if (number > 0 && number <= Users.Count)
            {
                var i = 1;
                foreach (var role in Enum.GetValues(typeof(Roles)))
                {
                   ioService.Write($"{i++:D2} - {role}");
                }
                var roleNumber = communicator.AskForNumber("Please enter desired role's number", i);
                if (roleNumber>0)
                {
                    var user = Users[number - 1];
                    user.Role = (Roles) roleNumber;
                    dataService.UpdateUserRole(user);
                    ShowWelcomeInfo();
                    ioService.Highlight("Role set successfuly");
                    return;
                }
            }

            AbortOperation();
        }


    }
}
