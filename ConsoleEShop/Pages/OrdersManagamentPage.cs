using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    class OrdersManagamentPage : BasePage, IPage
    {
        private List<Order> Orders { get; set; }
        public OrdersManagamentPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService, client)
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
                        {"status",SetOrderStatus},

                    };
                    }
                    default:
                        return new Dictionary<string, Func<string>>();
                        
            }
        }

        private string SetOrderStatus()
        {
            var order = SelectOrder();
            if (order is null)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }

            ioService.Highlight("Please enter № of status :");

            ioService.Write($"{01} - {OrderStatus.CanceledByAdministrator}");
            ioService.Write($"{02} - {OrderStatus.PaymentRecieved}");
            ioService.Write($"{03} - {OrderStatus.Sent}");
            ioService.Write($"{04} - {OrderStatus.Finished}");

            var newStatusNo = communicator.AskForNumber("Please enter No of new status", 4);

            if (newStatusNo > 0)
            {


                switch (newStatusNo)
                {
                    case 1:
                        order.Status = OrderStatus.CanceledByAdministrator;
                        break;
                    case 2:
                        order.Status = OrderStatus.PaymentRecieved;
                        break;
                    case 3:
                        order.Status = OrderStatus.Sent;
                        break;
                    case 4:
                        order.Status = OrderStatus.Finished;
                        break;

                }
                dataService.UpdateOrderStatus(order);
                ShowWelcomeInfo();
                return "Status changed successfuly";
                
            }
            ShowWelcomeInfo();
            return "Operation canceled";
        }
        public Order SelectOrder()
        {
            var number = communicator.AskForNumber("Please enter index of product you want to change", Orders.Count);
            return number > 0 ? Orders[number - 1] : null;
        }

        public IView ShowPageData()
        {

            return new OrdersView(dataService.GetOrders(),dataService);
        }
    }
}
