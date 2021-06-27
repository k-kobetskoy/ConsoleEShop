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
                             {"recieved", SetRecievedStatus},
                             {"cancel", CancelOrder}
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
                             {"recieved", SetRecievedStatus},
                             {"cancel", CancelOrder}
                         };
                    }
                default:
                    return new Dictionary<string, Func<string>>();
            }
        }


        public string CancelOrder()
        {
            var number = client.AskForNumber("Please enter No of order you want to cancel", Orders.Count);
            if (number < 1)
                return ShowAbortOperationMessage("Operation canceled");
              
           

            var order = Orders[number - 1];
            order.Status = OrderStatus.CanceledByUser;
            dataService.UpdateOrderStatus(order);
          return ShowAbortOperationMessage("Order was canceled");
        }

        public string SetRecievedStatus()
        {
            if (!Orders.Any())
                return ShowAbortOperationMessage("There is no orders");
             
            
            var orderIndex = client.AskForNumber("Enter № of order you recieved", Orders.Count);
            if (orderIndex < 1)
                return ShowAbortOperationMessage("Operation canceled");

            var order = Orders[orderIndex - 1];
            order.Status = OrderStatus.Recieved;
            dataService.UpdateOrderStatus(order);
          return  ShowWelcomeInfo("Status changed succesfuly");
           
        }


        public override IView ShowPageData()
        {
            Orders = dataService.GetUserOrders(context.CurrentUser.Id).OrderBy(o => o.OrderId)?.ToList();
            return new OrdersView(Orders, dataService);
        }


    }
}
