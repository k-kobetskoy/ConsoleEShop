using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleEShop.Models;
using ConsoleEShop.Views;


namespace ConsoleEShop.Pages
{
    //Orders
    public abstract partial class BasePage
    {
        public void ShowMyOrdersPage()
        {
            context.SetCurrentPage(new OrdersPage(ioService, dataService, client));
        }
    
        public void SetRecievedStatus(List<Order> orders, string errorMessage = null)
        {
            var ordersCount = orders.Count;
            var message = string.IsNullOrWhiteSpace(errorMessage) ? "Enter № of order you recieved" : errorMessage;
            var recievedOrderIndex = ReadNumber(message);
            if (recievedOrderIndex > ordersCount || recievedOrderIndex == 0)
            {
                SetRecievedStatus(orders, "There is no such order. Please enter correct № or press Esc to abort operation");
                return;
            }
            if (recievedOrderIndex < 0)
            {
                ShowWelcomeInfo();
                return;
            }

            var order = orders[recievedOrderIndex - 1];
            order.Status = OrderStatus.Recieved;
            dataService.UpdateOrderStatus(order);
            ShowWelcomeInfo();
            ioService.Highlight("Status changed succesfuly");
        }
    }
    //Cart
    public abstract partial class BasePage
    {
        public void ShowMyCart()
        {
            context.SetCurrentPage(new CartPage(ioService, dataService, client));
        }
        public void AddToCart(Product product)
        {
            ioService.Write("Add to cart command");
        }
        public void AddToCart(string index = null)
        {
            if (string.IsNullOrWhiteSpace(index))
            {
                ioService.Write("Please enter product's index");
                AddToCart(ioService.ReadOrAbort());
                return;
            }

            var parseResult = int.TryParse(index, out var productIndex);
            if (!parseResult)
            {
                ioService.Write("Please enter correct product's index");
                AddToCart(ioService.ReadOrAbort());

            }
            else
            {
                var product = dataService.GetProductById(productIndex);
                if (product is null)
                {
                    ioService.Write("Didn't find such product. Please enter correct product's index");
                    AddToCart(ioService.ReadOrAbort());

                }

                SetQuantity(product);
            }


        }

        private void SetQuantity(Product product, string errorMessage = null)
        {



            var message = string.IsNullOrWhiteSpace(errorMessage)
                 ? $"How many {product.Name}'s do you want to purchase? Please enter a number, or press Enter to buy 1 item"
                 : errorMessage;


            int quantity;
            ioService.Write(message);
            var response = ioService.ReadOrAbort();

            if (string.IsNullOrWhiteSpace(response))
                quantity = 1;

            else if (!int.TryParse(response, out quantity))
            {
                errorMessage = "Please enter  correct number. Or press Esc to abort operation";
                SetQuantity(product, errorMessage);
            }

            if (context.Cart is null)
                context.SetCart();

            var existingCartItem = context.Cart.Items.FirstOrDefault(p => p.ProductId == product.Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem { ProductId = product.Id, Quantity = quantity };
                context.Cart.Items.Add(cartItem);
            }


            ioService.Write("Item added to cart succesfuly");
            ShowWelcomeInfo();
        }

        public void EditQuantity(string index = null)
        {
            if (index is null)
            {
                ioService.Write("Please enter № of product in cart you want to edit");
                EditQuantity(ioService.ReadOrAbort());
                return;
            }
            var parseResult = int.TryParse(index, out var productIndex);

            if (parseResult && productIndex > 0)
            {
                var newQuantity = ReadNumber("Please enter desired new quantity");

                if (newQuantity < 1)
                {
                    ShowWelcomeInfo();
                    return;
                }

                context.Cart.Items[productIndex - 1].Quantity = newQuantity;


            }
            else
            {
                ioService.Write("Please enter correct product's №");
                EditQuantity(ioService.ReadOrAbort());
            }

            ShowWelcomeInfo();


        }

        public void RemoveItemFromCart(string index = null)
        {
            var message = "Please enter № of product in cart you want to remove";
            int itemIndexToRemove = 0;
            if (index != null)
            {
                var parse = int.TryParse(index, out itemIndexToRemove);
                if (!parse)
                {
                    message = "Please enter correct № of product in cart you want to remove";
                }
                else
                {
                    if (itemIndexToRemove > 0)
                    {
                        context.Cart.Items.RemoveAt(itemIndexToRemove - 1);
                    }
                    else
                    {
                        ShowWelcomeInfo();
                        return;
                    }
                }
            }


            itemIndexToRemove = ReadNumber(message);
            if (itemIndexToRemove < 1)
            {
                ShowWelcomeInfo();
                return;
            }
            context.Cart.Items.RemoveAt(itemIndexToRemove - 1);


            ShowWelcomeInfo();
        }

        public void Checkout()
        {
            if (context.Cart == null || context.Cart.ItemsCount == 0)
            {
                ShowWelcomeInfo();
                ioService.Write("You have nothing to checkout");
                return;
            }


            var order = new Order()
            {
                OrderItems = context.Cart.Items,
                Status = OrderStatus.New,
                UserId = context.CurrentUser.Id
            };
            dataService.AddOrder(order);
            context.SetCart();
            ShowWelcomeInfo();
            ioService.Highlight("Order was made successfully");

        }

        public void ClearCart()
        {

            if (context.Cart == null || context.Cart.ItemsCount == 0)
            {
                ShowWelcomeInfo();
                return;
            }

            context.Cart.Items = new List<CartItem>();
            ShowWelcomeInfo();
            ioService.Write("All items was removed from your cart");
        }
    }

    //Personal Info
    public abstract partial class BasePage
    {
        public void ShowMyInfo()
        {
            context.SetCurrentPage(new PersonalInfoPage(ioService, dataService, client));
        }

        public void ChangeName()
        {
            ioService.Write("Write desired new name");
            var newName = ioService.ReadOrAbort();
            if (newName == null)
            {
                ShowWelcomeInfo();
                return;
            }
            if (string.IsNullOrWhiteSpace(newName) || newName.Length < 2)
            {
                ioService.Write("Name must contain at least 2 symbols\n" +
                                "Enter correct name or press Escape to abort operation");
                ChangeName();
                return;
            }

            context.CurrentUser.Name = newName;
            ShowWelcomeInfo();
            ioService.Write("Name changed successful");
        }

        public void ChangePassword()
        {
            ioService.Write("Write desired new password");
            var newPass = ioService.ReadOrAbort();
            if (newPass == null)
            {
                ShowWelcomeInfo();
                return;
            }
            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 2)
            {
                ioService.Write("Password must contain at least 2 symbols\n" +
                                "Enter correct password or press Escape to abort operation");
                ChangePassword();
                return;
            }

            context.CurrentUser.Password = newPass;
            ShowWelcomeInfo();
            ioService.Write("Password changed successful");
        }

    }

    //Administrator
    public abstract partial class BasePage
    {

        
        public void ManageUsers()
        {
            context.SetCurrentPage(new UserManagementPage(ioService, dataService, client));
        }

        public void ManageProducts()
        {
            context.SetCurrentPage(new ProductsManagamentPage(ioService, dataService, client));
        }
        public void ManageOrders()
        {
            context.SetCurrentPage(new OrdersManagamentPage(ioService, dataService, client));
        }
    }

    //Login register
    public abstract partial class BasePage
    {
        public void Logout()
        {
            context.SetCurrentUser();
            context.SetCurrentPage(new HomePage(ioService, dataService, client));
        }
        public void Login(string errorMessage = null)
        {

            var login = communicator.AskForString("Please enter login or press Escape to abort operation", "login", 3);
            if (string.IsNullOrWhiteSpace(login))
            {
                AbortOperation();
            }

            var user = dataService.GetUserByName(login);
            if (user is null)
            {
                AbortOperation();
            }

            CheckPassword(user);


            //var message = string.IsNullOrWhiteSpace(errorMessage)
            //    ? "Please enter login"
            //    : errorMessage;


            //User user = null;
            //ioService.Write(message);

            //var login = ioService.ReadOrAbort();
            //if (login is null)
            //{
            //    ShowWelcomeInfo();
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(login))
            //{
            //    Login("Please enter login or press Escape to abort operation");
            //}
            //user = Data.Users.FirstOrDefault(u => u.Name == login);


            //if (user != null)
            //    CheckPassword(user);

        }

        public string Register()
        {
            ioService.Write("Enter your login:");
            var login = ioService.ReadOrAbort();
            if (login is null)
            {
                ShowWelcomeInfo();
                return null;
            }
            ioService.Write("Enter your password:");
            var password = ioService.ReadOrAbort();
            if (password is null)
            {
                ShowWelcomeInfo();
                return null;
            }

            var user = dataService.AddUser(login, password);
            context.SetCurrentUser(user);
            SetCommands();
            return null;
        }

        private void CheckPassword(User user)
        {
            ioService.Write("Enter your password");

            while (true)
            {
                var password = ioService.ReadOrAbort();
                if (password is null)
                {
                    ShowWelcomeInfo();
                    break;
                }

                if (user.Password == password)
                {
                    context.SetCurrentUser(user);
                    SetCommands();
                    ioService.Write($"Login successfull");
                    ShowWelcomeInfo();
                    return;
                }

                ioService.Write("Wrong password. Try again or press Escape to return");
            }
        }


    }

    //Products | Product
    public abstract partial class BasePage
    {
        public void ShowAllProducts()
        {
            context.SetCurrentPage(new ProductsPage(ioService, dataService, client));
        }
        
        public void ShowProduct(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = communicator.AskForString(
                    "Please enter name of the product you are searching. Or press Escape to return", "product name", 3);

                if (!string.IsNullOrWhiteSpace(name))
                    ShowProduct(name);
                else
                    AbortOperation();
                
            }
            else
            {
                var product = dataService.GetProductByName(name);
                if (product is null)
                {
                    ioService.Write("Can't find this product");
                    AbortOperation();
                }
                context.SetCurrentPage(new ProductPage(ioService, dataService, product, client));
            }
        }
    }

    public abstract partial class BasePage
    {
        
        public EShop context;
        protected readonly IIOService ioService;
        protected readonly IDataService dataService;
        private readonly IClient client;
        protected Communicator communicator;
        
        public Dictionary<string, Func<string>> Commands { get; set; }
        public string Param { get; set; }
        protected BasePage(IIOService ioService, IDataService dataService, IClient client)
        {
            communicator = new Communicator(ioService);
            this.ioService = ioService;
            this.dataService = dataService;
            this.client = client;
        }

        public abstract Dictionary<string,Action> SetCommands();
        public abstract IView ShowPageData();
        public void SetContext(EShop context)
        {
            this.context = context;
        }
       
        public void ShowWelcomeInfo()
        {
            ioService.Clear();
            var menu = new MenuView(context, Commands.Keys.ToList());
            ioService.Write(new PageData(menu, ShowPageData()).ShowViewData());
        }


        protected int ReadNumber(string message = null)
        {

            message = string.IsNullOrWhiteSpace(message)
                ? "enter number"
                : message;


            ioService.Write(message);
            var response = ioService.ReadOrAbort();
            if (response is null)
                return -1;

            var parse = int.TryParse(response, out var result);
            if (!parse || result < 1)
            {
                ReadNumber("please enter correct number or pres Escape to abort operation");
            }
            else
            {
                return result;
            }

            return -1;
        }

        public void ShowErrorMessage(string message)
        {
            ioService.Write(message);
        }

        public void AbortOperation()
        {
            ShowWelcomeInfo();
        }
    }
}