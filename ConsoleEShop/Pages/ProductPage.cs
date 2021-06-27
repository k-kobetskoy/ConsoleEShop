using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleEShop.Views;
using static ConsoleEShop.User;


namespace ConsoleEShop.Pages
{
    public class ProductPage : BasePage,  IPage
    {
        private readonly Product product;
        public ProductPage(IIOService ioService, IDataService dataService,  Product product, IClient client) :base(ioService, dataService,client)
        {
            this.product = product;

           
        }
        public override Dictionary<string, Func<string>> SetCommands()
        {
            switch (context.CurrentUser.Role)
            {
                case Roles.Guest:
                {
                    return new Dictionary<string, Func<string>>
                    {
                        {"register", Register},
                        {"login", Login},
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                    };
                }
                   
                case Roles.RegisteredUser:
                {
                    return new Dictionary<string, Func<string>>
                    {
                        {"product", () => ShowProductPage(Param)},
                        {"products", ShowAllProductsPage},
                        {"cart", ShowMyCartPage},
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"orders", ShowMyOrdersPage},
                        {"byu", ()=>  AddToCart(product)},
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
                        {"logout", Logout},
                        {"user info", ShowMyInfoPage},
                        {"orders", ShowMyOrdersPage},
                        {"m users", ShowManageUsersPage},
                        {"m orders", ShowManageOrdersPage},
                        {"m products", ShowManageProductsPage},
                        {"byu", () =>  AddToCart(product)},
                    };
                }
                    
                default:
                    return new Dictionary<string, Func<string>>();
            }
        }

        private string AddToCart(Product product)
        {
            return SetQuantity(product);
        }

        public override IView ShowPageData()
        {
            return new ProductView(product);
        }

       
      
        
    }
}
