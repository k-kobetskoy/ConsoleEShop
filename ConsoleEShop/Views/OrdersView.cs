using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEShop.Views
{
    public class OrdersView : IView
    {
        private readonly IEnumerable<Order> orders;
        private readonly IDataService dataService;

        public OrdersView(IEnumerable<Order> orders, IDataService dataService)
        {
            this.orders = orders;
            this.dataService = dataService;
        }
        public string ShowViewData()
        {
            if (orders is null || !orders.Any())
            {
                return "There are no any orders";
            }

            var sb = new StringBuilder("");
            var index = 1;

            var heading = $"№   - Статус\t\tUser\n";

            sb.Append(heading);
            sb.Append(new string('_', heading.Length));

            foreach (var order in orders)
            {
                var user = dataService.GetUserById(order.UserId);
                sb.Append($"{index++:D2} - {order.Status}\t\t{user.Name}\n");
            }

            return sb.ToString();
        }
    }
}