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
                        {"name",SetProductName},
                        {"price",SetProductPrice},
                        {"category", SetProductCategory},
                        {"description", SetProductDescription},
                        {"add", AddNewProduct},

                    };
                }
                  default: return new Dictionary<string, Action>();
            }
        }

        private void AddNewProduct()
        {
            var name = communicator.AskForString("Please enter name of new product:", "name",3);
            if (string.IsNullOrWhiteSpace(name))
            {
                AbortOperation();
                return;
            }
            var price = (decimal)communicator.AskForNumber("Please enter price of new product:");
            if (price < 1)
            {
                AbortOperation();
                return;
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
                AbortOperation();
                return;
            }
            var desctription = communicator.AskForString("Please enter description for new product:", "desctription", 3);
            if (string.IsNullOrWhiteSpace(desctription))
            {
                AbortOperation();
                return;
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
            ioService.Highlight("Product added successfuly");
            
        }

        private void SetProductDescription()
        {
            var product = SelectProduct();
            if (product is null)
            {
                AbortOperation();
                return;
            }

            var newDesctiption = communicator.AskForString("Please enter desired new name", "descripion", 3);

            if (!string.IsNullOrWhiteSpace(newDesctiption))
            {

                product.Description = newDesctiption;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                ioService.Highlight("Name changed successfuly");
                return;
            }
            AbortOperation();
        }

        private void SetProductCategory()
        {
            var product = SelectProduct();
            if (product is null)
            {
                AbortOperation();
                return;
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
                ioService.Highlight("Name changed successfuly");
                return;
            }
            AbortOperation();
        }

        private void SetProductPrice()
        {
            var product = SelectProduct();
            if (product is null)
            {
                AbortOperation();
                return;
            }

            var newPrice = communicator.AskForNumber("Please enter desired new price");

            if (newPrice>0)
            {

                product.Price = newPrice;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                ioService.Highlight("Price changed successfuly");
                return;
            }
            AbortOperation();

        }

        private void SetProductName()
        {
            var product = SelectProduct();
            if (product is null)
            {
                AbortOperation();
                return;
            }

            var newName = communicator.AskForString("Please enter desired new name", "name", 3);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                
                product.Name = newName;
                dataService.UpdateProduct(product);
                ShowWelcomeInfo();
                ioService.Highlight("Name changed successfuly");
                return;
            }
            AbortOperation();
        }

        public override IView ShowPageData()
        {


            Products = dataService.GetProducts().ToList();
            return new ProductsView(Products);
            
            //ioService.Highlight($"№  - Название{new string(' ', 22)}Цена");
            //foreach (var product in Products)
            //{
            //    ioService.Write($"{product.Id:D2} - {product.Name}{new string(' ', 30 - product.Name.Length)}{product.Price}");
            //}

        }

        public Product SelectProduct()
        {
            var number = communicator.AskForNumber("Please enter index of product you want to change", Products.Count);
            return number > 0 ? Products[number - 1] : null;
        }

        

    }
}
