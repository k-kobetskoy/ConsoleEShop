using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ConsoleEShop.Models;
using ConsoleEShop.Pages;

namespace ConsoleEShop
{

    public class EShop : IEShop
    {
        private readonly IIOService ioService;
        private readonly IDataService dataService;
        private readonly IClient client;
        
        
        public IPage currentPage { get; set; }
        public Cart Cart { get; private set; }
        public User CurrentUser { get; set; }
        public event EventHandler ContextChanged;

        public EShop(IIOService ioService, IDataService dataService, IClient client)
        {
            this.ioService = ioService;
            this.dataService = dataService;
            this.client = client;
            this.CurrentUser = new User { Role = Roles.Guest };
            client.RequestRecieved += Handle;

            SetCurrentPage(new HomePage(ioService, dataService, client));

            client.Response(currentPage.ShowPageData());
        }



        public void SetCart()
        {
            Cart = new Cart();
        }
        public void SetCurrentUser(User user = null)
        {
            if (user == null)
            {
                CurrentUser = new User { Role = Roles.Guest };
                return;
            }
            CurrentUser = user;
            SetCart();
            ContextChanged?.Invoke(this, EventArgs.Empty);
            
        }
        public void SetCurrentPage(IPage page)
        {
            currentPage = page ?? throw new ArgumentNullException(nameof(page));
            currentPage.SetContext(this);
            
            ContextChanged?.Invoke(this, EventArgs.Empty);
            //currentPage.Commands = currentPage.SetCommands();
        }


        public void Handle(object? sender, ClientRequestArgs args)
        {

            string param = null;
            string command = args.Command;
            if (!currentPage.Commands.ContainsKey(args.Command))
            {
                var complexCheck = CheckComplexRequest(args.Command, out command, out param);
                if (!complexCheck)
                {
                    client.Response("there is no such command");
                    return;
                }
            }

            currentPage.Param = param;

            client.Response(currentPage.Commands[command].Invoke());
        }

        private bool CheckComplexRequest(string request, out string command, out string param)
        {
            if (request.Trim().Contains(' '))
            {
                var splitedRequest = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                command = splitedRequest[0];
                param = request.Replace(command, "").Trim();
                return true;
            }

            command = null;
            param = null;
            return false;
        }

    }
}
