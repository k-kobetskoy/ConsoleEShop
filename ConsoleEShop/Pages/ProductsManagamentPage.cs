using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    public class ProductsManagamentPage:BasePage, IPage
    {
        private List<Product> Products { get; set; }
        public ProductsManagamentPage( IDataService dataService, IClient client) : base( dataService,client)
        {
            
        }

        public override void SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
                case Roles.RegisteredUser:
                    Commands = new Dictionary<string, Func<string>>();
                    break;
                case
                    Roles.Administrator:
                {
                    Commands = new Dictionary<string, Func<string>>
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
                        {"name",SetProductName},
                        {"price",SetProductPrice},
                        {"category", SetProductCategory},
                        {"description", SetProductDescription},
                        {"add", AddNewProduct},

                    };break;
                }
                  default:
                      Commands = new Dictionary<string, Func<string>>();
                      break;
            }
        }

        private string AddNewProduct()
        {
            var name = client.AskForString("Please enter name of new product:", "name",3);
            if (string.IsNullOrWhiteSpace(name))
                return ShowAbortOperationMessage("Operation canceled");
            
            var price = (decimal)client.AskForNumber("Please enter price of new product:");
            if (price < 1)
                return ShowAbortOperationMessage("Operation canceled");
           
            client.Write("List of categories:");
            var categories = dataService.GeAllCategories().ToArray();
            
            for (int i = 0; i < categories.Count(); i++)
            {
                client.Write($"{i+1:D2} - {categories[i].Name}");
            }

            var categoryId = client.AskForNumber("Please enter desired No for category", categories.Length);
            if (categoryId<1)
                return ShowAbortOperationMessage("Operation canceled");

            var desctription = client.AskForString("Please enter description for new product:", "desctription", 3);
            if (string.IsNullOrWhiteSpace(desctription))
                return ShowAbortOperationMessage("Operation canceled");

            var product = new Product()
            {
                Name = name,
                Price = price,
                CategoryId = categoryId,
                Description = desctription,
            };
            dataService.AddNewProduct(product);
           return ShowWelcomeInfo("Product added successfuly");
           
            
        }

        private string SetProductDescription()
        {
            var product = SelectProduct();
            if (product is null)
                return ShowAbortOperationMessage("Operation canceled");

            var newDesctiption = client.AskForString("Please enter desired new name", "descripion", 3);

            if (!string.IsNullOrWhiteSpace(newDesctiption))
            {

                product.Description = newDesctiption;
                dataService.UpdateProduct(product);
               return ShowWelcomeInfo("Name changed successfuly");
               
                
            }
            return ShowAbortOperationMessage("Operation canceled");
        }

        private string SetProductCategory()
        {
            var product = SelectProduct();
            if (product is null)
                return ShowAbortOperationMessage("Operation canceled");

            var categories = dataService.GeAllCategories().ToArray();
            client.Write("List of categories:");
            for (int i = 1; i <= categories.Count(); i++)
            {
                client.Write($"{i:D2} - {categories[i].Name}");
            }

            var newCategoryNo = client.AskForNumber("Please enter desired No for category",  categories.Length);

            if (newCategoryNo>0)
            {

                product.CategoryId = categories[newCategoryNo].Id;
                dataService.UpdateProduct(product);
               return ShowWelcomeInfo("Name changed successfuly");
               
                
            }
            return ShowAbortOperationMessage("Operation canceled");
        }

        private string SetProductPrice()
        {
            var product = SelectProduct();
            if (product is null)
                return ShowAbortOperationMessage("Operation canceled");

            var newPrice = client.AskForNumber("Please enter desired new price");

            if (newPrice>0)
            {

                product.Price = newPrice;
                dataService.UpdateProduct(product);
               return ShowWelcomeInfo("Price changed successfuly");
                
               
            }
            return ShowAbortOperationMessage("Operation canceled");
        }

        private string SetProductName()
        {
            var product = SelectProduct();
            if (product is null)
                return ShowAbortOperationMessage("Operation canceled");

            var newName = client.AskForString("Please enter desired new name", "name", 3);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                
                product.Name = newName;
                dataService.UpdateProduct(product);
               return ShowWelcomeInfo("Name changed successfuly");
                
            }
            return ShowAbortOperationMessage("Operation canceled");
        }

        public override IView ShowPageData()
        {


            Products = dataService.GetProducts().ToList();
            return new ProductsView(Products);

        }

        public Product SelectProduct()
        {
            var number = client.AskForNumber("Please enter index of product you want to change", Products.Count);
            return number > 0 ? Products[number - 1] : null;
        }

        

    }
}
