using ConsoleEShop.Models;
using ConsoleEShop.Pages;

namespace ConsoleEShop
{
    public interface IEShop
    {
        Cart Cart { get; }
        User CurrentUser { get; set; }
        IPage currentPage { get; set; }
        void Handle(object sender, ClientRequestArgs args);
        void SetCart();
        void SetCurrentPage(IPage page);
        void SetCurrentUser(User user = null);
    }
}