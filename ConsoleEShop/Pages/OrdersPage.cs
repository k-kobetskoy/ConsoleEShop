using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ConsoleEShop.Views;
using static ConsoleEShop.User;

namespace ConsoleEShop.Pages
{
   public class OrdersPage : BasePage, IPage
    {
        private List<Order> Orders { get; set; }
        public OrdersPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService, client)
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
                        {"recieved", ()=>SetRecievedStatus(Orders)},
                        {"cancel", CancelOrder}
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
                        {"recieved", ()=>SetRecievedStatus(Orders)},
                        {"cancel", CancelOrder}
                    };
                    }
                    default:
                        return new Dictionary<string, Action>();
            }
        }


        public void CancelOrder()
        {
            var number = communicator.AskForNumber("Please enter No of order you want to cancel", Orders.Count);
            if (number<1)
            {
                AbortOperation();
                return;
            }

            var order = Orders[number - 1];
            order.Status = OrderStatus.CanceledByUser;
            dataService.UpdateOrderStatus(order);
            ShowWelcomeInfo();
            ioService.Highlight("Order was canceled");
        }
        public override IView ShowPageData()
        {
            Orders = dataService.GetUserOrders(context.CurrentUser.Id).OrderBy(o => o.OrderId)?.ToList();

            return new OrdersView(Orders, dataService);
            
            //if (Orders.Count < 1)
            //{
            //    ioService.Write("You dont have any orders yet");
            //    return;
            //}
            //var index = 1;
            //ioService.Highlight($"№  - Статус\t\tТовары");
            //foreach (var order in Orders)
            //{
            //    ioService.WriteInLine($"{index++:D2} - {order.Status}\t\t");
            //    var gap = 0;
            //    foreach (var orderOrderItem in order.OrderItems)
            //    {
            //        var product = dataService.GetProductById(orderOrderItem.ProductId);
            //        ioService.Write($"{new string(' ', gap)}{product.Name}(x{orderOrderItem.Quantity})\t");
            //        gap = 24;
            //    }
            //}

        }


    }
}
