using System;
using System.Collections.Generic;
using ConsoleEShop.Views;

namespace ConsoleEShop.Pages
{
    public interface IPage
    {
        Dictionary<string, Func<string>> Commands { get; set; }
        string Param { get; set; }
        void SetContext(EShop context);
        Dictionary<string, Func<string>> SetCommands();
        public IView ShowPageData();

    }
}