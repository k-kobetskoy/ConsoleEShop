using System;
using System.Threading.Tasks;
using ConsoleEShop.Pages;

namespace ConsoleEShop
{
    class Program
    {
        static void Main(string[] args)
        {

            var ioService = new IOService();
            var dataService = new DataService();
           
            var client = new Client(ioService);
           
            var shop = new EShop(ioService, dataService, client);
            //shop.SetCurrentPage(new HomePage(ioService, dataService));
            
            client.StartListen();
        }
    }
}
