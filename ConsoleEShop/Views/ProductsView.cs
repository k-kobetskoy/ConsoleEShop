using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEShop.Views
{
    public class ProductsView : IView
    {
        private readonly IEnumerable<Product> products;

        public ProductsView(IEnumerable<Product> products)
        {
            this.products = products;
        }


        public string ShowViewData()
        {
            if (products is null || !products.Any())
                return "There are no any producs to show";

            var sb = new StringBuilder("");
            var heading = $"№  - Название{new string(' ', 22)}Цена\n";
            sb.AppendLine(heading);
            sb.AppendLine(new string('_', heading.Length) + "\n");
            foreach (var product in products)
            {
                sb.AppendLine($"{product.Id:D2} - {product.Name}{new string(' ', 30 - product.Name.Length)}{product.Price}" + "\n");
            }

            return sb.ToString();
        }
    }
}