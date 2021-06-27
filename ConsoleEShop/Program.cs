using System;
using System.Threading.Tasks;
using ConsoleEShop.Pages;

namespace ConsoleEShop
{
    class Program
    {
        static void Main(string[] args)
        {

            
            var dataService = new DataService();
           
            var client = new Client();
           
            var shop = new EShop(dataService, client);
            
            
            client.StartListen();
        }
    }
}
