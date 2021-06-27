using System.Text;
using ConsoleEShop.Models;

namespace ConsoleEShop.Views
{
    public class CartView : IView
    {
        private readonly Cart cart;
        private readonly IDataService dataService;

        public CartView(Cart cart, IDataService dataService)
        {
            this.cart = cart;
            this.dataService = dataService;
        }
        public string ShowViewData()
        {
            if (cart is null || cart.ItemsCount < 1)
            {
                return "There are no products in cart";
            }

            var sb = new StringBuilder("");
            var heading = $"№  - Название{new string(' ', 22)}Цена/ед    х Кол-во\t Цена сумм.\n";
            sb.Append(heading);
            sb.Append(new string('_', heading.Length) + "\n");


            decimal totalPrice = 0;
            var index = 1;
            foreach (var cartItem in cart.Items)
            {
                var product = dataService.GetProductById(cartItem.ProductId);
                sb.Append($"{index++:D2} - " +
                          $"{product.Name}{new string(' ', 30 - product.Name.Length)}" +
                          $"{product.Price}{new string(' ', 10 - product.Price.ToString().Length)} х " +
                          $"{cartItem.Quantity:D2}\t" +
                          $"{product.Price * cartItem.Quantity}");
                totalPrice += product.Price * cartItem.Quantity;
            }

            var footer = $"\t\t\t\t\t\t\tОбщая цена\n\t\t\t\t\t\t\t{totalPrice}";
            sb.Append(new string('_', footer.Length) + "\n");
            sb.Append(footer);

            return sb.ToString();
        }
    }
}