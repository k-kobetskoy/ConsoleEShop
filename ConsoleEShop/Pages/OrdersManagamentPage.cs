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
                        {"status",SetOrderStatus},

                    };
                    }
                    default:
                        return new Dictionary<string, Action>();
                        
            }
        }

        private void SetOrderStatus()
        {
            var order = SelectOrder();
            if (order is null)
            {
                AbortOperation();
                return;
            }

            ioService.Highlight("Please enter № of status :");

            ioService.Write($"{01} - {OrderStatus.CanceledByAdministrator}");
            ioService.Write($"{02} - {OrderStatus.PaymentRecieved}");
            ioService.Write($"{03} - {OrderStatus.Sent}");
            ioService.Write($"{04} - {OrderStatus.Finished}");

            var newStatusNo = communicator.AskForNumber("Please enter desired new name", 4);

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
                ioService.Highlight("Status changed successfuly");
                return;
            }
            AbortOperation();
        }
        public Order SelectOrder()
        {
            var number = communicator.AskForNumber("Please enter index of product you want to change", Orders.Count);
            return number > 0 ? Orders[number - 1] : null;
        }

        public override IView ShowPageData()
        {

            return new OrdersView(dataService.GetOrders(),dataService);
            //Orders = dataService.GetOrders().ToList();
            //if (Orders.Count < 1)
            //{
            //    ioService.Write("There are no any orders");
            //    return;
            //}
            //var index = 1;
            //ioService.Highlight($"№   - Статус\t\tUser");
            //foreach (var order in Orders)
            //{
            //    var user = dataService.GetUserById(order.UserId);
            //    ioService.WriteInLine($"{index++:D2} - {order.Status}\t\t{user.Name}");
            //}
        }
    }
}
