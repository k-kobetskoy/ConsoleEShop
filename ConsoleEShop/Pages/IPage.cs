using System;
using System.Collections.Generic;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    public interface IPage
    {
        Dictionary<string, Func<string>> Commands { get; set; }
        string Param { get; set; }
        void ShowWelcomeInfo();
        void SetContext(EShop context);
        void ShowErrorMessage(string message);
        Dictionary<string, Func<string>> SetCommands();


    }
}