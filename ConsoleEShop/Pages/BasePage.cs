using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using ConsoleEShop.Models;
using ConsoleEShop.Views;


namespace ConsoleEShop.Pages
{

    //Login register
    public abstract partial class BasePage
    {
        public string Logout()
        {
            context.SetCurrentUser();
            context.SetCurrentPage(new HomePage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string Login()
        {

            var login = client.AskForString("Please enter login or press Escape to abort operation", "login", 3);
            if (string.IsNullOrWhiteSpace(login))
                return "Operation was canceled";

            var user = dataService.GetUserByName(login);
            if (user is null)
                return "No users with this login";

            return CheckPassword(user);
        }
        public string Register()
        {
            var login = client.AskForString("Enter your login", "login", 3); ;
            if (login is null)
                return ShowAbortOperationMessage("Operation was canceled");



            var password = client.AskForString("Enter your password:", "password", 3);
            if (password is null)
                return ShowAbortOperationMessage("Operation was canceled");


            var user = dataService.AddUser(login, password);
            context.SetCurrentUser(user);
            return ShowWelcomeInfo($"Hello, {user.Name}!");
        }
        private string CheckPassword(User user)
        {
            var pass = client.AskForString("Enter your password", "password", 3);

            if (pass is null)
                return ShowAbortOperationMessage("Operation was canceled");


            if (user.Password == pass)
            {
                context.SetCurrentUser(user);
                return ShowWelcomeInfo("Login successfull");
            }

            return ShowAbortOperationMessage("Wrong password. Try again or press Escape to return");
        }

    }

    //Products | Product
    public abstract partial class BasePage
    {
        public string SetQuantity(Product product)
        {

            var number = client.AskForNumber($"How many {product.Name}'s do you want to purchase?");
            if (number < 1)
            {

                return ShowAbortOperationMessage("Operation was canceled");

            }

            if (context.Cart is null)
                context.SetCart();

            var existingCartItem = context.Cart.Items.FirstOrDefault(p => p.ProductId == product.Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += number;
            }
            else
            {
                var cartItem = new CartItem { ProductId = product.Id, Quantity = number };
                context.Cart.Items.Add(cartItem);
            }
            return ShowWelcomeInfo("Item added to cart");

        }
    }

    public abstract partial class BasePage
    {
        public EShop context;

        protected readonly IDataService dataService;
        protected readonly IClient client;

        public Dictionary<string, Func<string>> Commands { get; set; }
        public string Param { get; set; }
        protected BasePage(IDataService dataService, IClient client)
        {
            this.dataService = dataService;
            this.client = client;
        }

        public abstract IView ShowPageData();
        public abstract void SetCommands();
        public void SetContext(EShop context)
        {
            this.context = context;
            SetCommands();
        }

        public string ShowWelcomeInfo(string message = null)
        {
            var menu = new MenuView(context, Commands.Keys.ToList());
            return new PageData(menu, ShowPageData()).ShowViewData() + $"\n{message}";
        }

        public string ShowAbortOperationMessage(string message)
        {
            return ShowPageData().ShowViewData() + $"\n{message}";
        }

        //Page transitions
        public string ShowMyOrdersPage()
        {
            context.SetCurrentPage(new OrdersPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string ShowMyCartPage()
        {
            context.SetCurrentPage(new CartPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string ShowAllProductsPage()
        {
            context.SetCurrentPage(new ProductsPage(dataService, client));
            return ShowWelcomeInfo();
        }
        public string ShowProductPage(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = client.AskForString(
                    "Please enter name of the product you are searching. Or press Escape to return", "product name", 3);

                return !string.IsNullOrWhiteSpace(name)
                    ? ShowProductPage(name)
                    : ShowAbortOperationMessage("Operation was canceled");

            }


            var product = dataService.GetProductByName(name);
            if (product is null)
            {
                return ShowAbortOperationMessage("Can't find this product");

            }
            context.SetCurrentPage(new ProductPage(dataService, product, client));
            return ShowWelcomeInfo();
        }
        public string ShowMyInfoPage()
        {
            context.SetCurrentPage(new PersonalInfoPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string ShowManageUsersPage()
        {
            context.SetCurrentPage(new UserManagementPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string ShowManageProductsPage()
        {
            context.SetCurrentPage(new ProductsManagamentPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
        public string ShowManageOrdersPage()
        {
            context.SetCurrentPage(new OrdersManagamentPage(dataService, client));
            return context.currentPage.ShowPageData().ShowViewData();
        }
    }
}