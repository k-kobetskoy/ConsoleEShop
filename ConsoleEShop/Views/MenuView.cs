using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEShop.Views
{
    public class MenuView : IView
    {
        private readonly EShop context;
        private readonly IEnumerable<string> keys;

        public MenuView(EShop context, IEnumerable<string> keys)
        {
            this.context = context;
            this.keys = keys;
        }
        public string ShowViewData()
        {
            var userName = context.CurrentUser.Name ??= "guest";
            var sb = new StringBuilder("");
            sb.Append($"Hello {userName}! Avaliable commands:\n");
            if (context.Cart != null && context.Cart.ItemsCount > 0)
            {
                var cartInfo = $"Your cart contains {context.Cart.ItemsCount} item(s)";
                sb.Append(new string('-', cartInfo.Length));
                sb.Append(cartInfo);
                sb.Append(new string('-', cartInfo.Length));
            }

            var commandList = string.Join(", ", keys) + "\n";

            sb.Append(commandList);
            sb.Append(new string('-', commandList.Length)+"\n");
            return sb.ToString();
        }
    }
}
