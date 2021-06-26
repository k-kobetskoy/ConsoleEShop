using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ConsoleEShop.Models;
using ConsoleEShop.Pages;

namespace ConsoleEShop
{

    public class EShop
    {
        private readonly IIOService ioService;
        private readonly IDataService dataService;
        private readonly IClient client;
        public IPage currentPage;
        
        
        public Cart Cart { get; private set; }
        public User CurrentUser { get; set; }

        public EShop(IIOService ioService, IDataService dataService, IClient client)
        {
            this.ioService = ioService;
            this.dataService = dataService;
            this.client = client;
            this.CurrentUser = new User { Role = Roles.Guest };
            client.RequestRecieved +=Handle;

            SetCurrentPage(new HomePage(ioService, dataService, client));
        }

 

        public void SetCart()
        {
            Cart = new Cart();
        }
        public void SetCurrentUser(User user=null)
        {
            if (user==null)
            {
                CurrentUser = new User {Role = Roles.Guest};
                return;
            }
            CurrentUser = user;
        }
        public void SetCurrentPage(IPage page)
        {
            currentPage = page ?? throw new ArgumentNullException(nameof(page));
            currentPage.SetContext(this);
            currentPage.Commands=  currentPage.SetCommands();
            currentPage.ShowWelcomeInfo();
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
                    currentPage.ShowErrorMessage("there is no such command");
                    return;
                }
            }

            currentPage.Param = param;
            currentPage.Commands[command].Invoke();
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
