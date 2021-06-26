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
        public ProductsManagamentPage(IIOService ioService, IDataService dataService, IClient client) : base(ioService, dataService,client)
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
                        {"name",SetProductName},
                        {"price",SetProductPrice},
                        {"category", SetProductCategory},
                        {"description", SetProductDescription},
                        {"add", AddNewProduct},

                    };
                }
                  default: return new Dictionary<string, Func<string>>();
            }
        }

        private string AddNewProduct()
        {
            var name = communicator.AskForString("Please enter name of new product:", "name",3);
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }
            var price = (decimal)communicator.AskForNumber("Please enter price of new product:");
            if (price < 1)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }
            ioService.Highlight("List of categories:");
            var categories = dataService.GeAllCategories().ToArray();
            
            for (int i = 0; i < categories.Count(); i++)
            {
                ioService.Write($"{i+1:D2} - {categories[i].Name}");
            }

            var categoryId = communicator.AskForNumber("Please enter desired No for category", categories.Length);
            if (categoryId<1)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }
            var desctription = communicator.AskForString("Please enter description for new product:", "desctription", 3);
            if (string.IsNullOrWhiteSpace(desctription))
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }
            var product = new Product()
            {
                Name = name,
                Price = price,
                CategoryId = categoryId,
                Description = desctription,
            };
            dataService.AddNewProduct(product);
            ShowWelcomeInfo();
            return "Product added successfuly";
            
        }

        private string SetProductDescription()
        {
            var product = SelectProduct();
            if (product is null)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }

            var newDesctiption = communicator.AskForString("Please enter desired new name", "descripion", 3);

            if (!string.IsNullOrWhiteSpace(newDesctiption))
            {

                product.Description = newDesctiption;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                return "Name changed successfuly";
                
            }
            ShowWelcomeInfo();
            return "Operation canceled";
        }

        private string SetProductCategory()
        {
            var product = SelectProduct();
            if (product is null)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }

            var categories = dataService.GeAllCategories().ToArray();
            ioService.Highlight("List of categories:");
            for (int i = 1; i <= categories.Count(); i++)
            {
                ioService.Write($"{i:D2} - {categories[i].Name}");
            }

            var newCategoryNo = communicator.AskForNumber("Please enter desired No for category",  categories.Length);

            if (newCategoryNo>0)
            {

                product.CategoryId = categories[newCategoryNo].Id;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                return "Name changed successfuly";
                
            }
            ShowWelcomeInfo();
            return "Operation canceled";
        }

        private string SetProductPrice()
        {
            var product = SelectProduct();
            if (product is null)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }

            var newPrice = communicator.AskForNumber("Please enter desired new price");

            if (newPrice>0)
            {

                product.Price = newPrice;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                return "Price changed successfuly";
               
            }
            ShowWelcomeInfo();
            return "Operation canceled";
        }

        private string SetProductName()
        {
            var product = SelectProduct();
            if (product is null)
            {
                ShowWelcomeInfo();
                return "Operation canceled";
            }

            var newName = communicator.AskForString("Please enter desired new name", "name", 3);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                
                product.Name = newName;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                return "Name changed successfuly";
            }
            ShowWelcomeInfo();
            return "Operation canceled";
        }

        public IView ShowPageData()
        {


            Products = dataService.GetProducts().ToList();
            return new ProductsView(Products);

        }

        public Product SelectProduct()
        {
            var number = communicator.AskForNumber("Please enter index of product you want to change", Products.Count);
            return number > 0 ? Products[number - 1] : null;
        }

        

    }
}
