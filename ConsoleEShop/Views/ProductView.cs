using System.Text;

namespace ConsoleEShop.Views
{
    public class ProductView : IView
    {
        private readonly Product product;

        public ProductView(Product product)
        {
            this.product = product;
        }
        public string ShowViewData()
        {
            if (product is null)
            {
                return "There are no products to show";
            }

            var sb = new StringBuilder("");
            sb.Append($"Товар: {product.Name}\n" +
                      $"Цена:{product.Price:F} USD\n" +
                      $"Описание:{product.Description}");
            return sb.ToString();
        }
    }
}