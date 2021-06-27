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
        public OrdersManagamentPage( IDataService dataService, IClient client) : base( dataService, client)
        {
            
        }

        public override void SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    Commands= new Dictionary<string, Func<string>>();
                    break;
                case Roles.RegisteredUser:
                    Commands =  new Dictionary<string, Func<string>>();
                    break;
                case
                    Roles.Administrator:
                    {
                        Commands= new Dictionary<string, Func<string>>
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
                        break;
                    }
                    default:
                        Commands = new Dictionary<string, Func<string>>();
                        break;
                        
            }
        }

        private string SetOrderStatus()
        {
            var order = SelectOrder();
            if (order is null)
            {
               return ShowAbortOperationMessage("Operation canceled");
                
            }

            client.Write("Please enter № of status :");

            client.Write($"{01} - {OrderStatus.CanceledByAdministrator}");
            client.Write($"{02} - {OrderStatus.PaymentRecieved}");
            client.Write($"{03} - {OrderStatus.Sent}");
            client.Write($"{04} - {OrderStatus.Finished}");

            var newStatusNo = client.AskForNumber("Please enter No of new status", 4);

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
               return ShowWelcomeInfo("Status changed successfuly");
               
                
            }
           return ShowAbortOperationMessage("Operation canceled");
           
        }
        public Order SelectOrder()
        {
            var number = client.AskForNumber("Please enter index of product you want to change", Orders.Count);
            return number > 0 ? Orders[number - 1] : null;
        }

        public override IView  ShowPageData()
        {

            return new OrdersView(dataService.GetOrders(),dataService);
        }
    }
}
