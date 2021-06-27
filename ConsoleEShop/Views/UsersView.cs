using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEShop.Views
{
    public class UsersView : IView
    {
        private readonly IEnumerable<User> users;

        public UsersView(IEnumerable<User> users)
        {
            this.users = users;
        }
        public string ShowViewData()
        {
            if (users is null || !users.Any())
            {
                return "There are no users to show";
            }

            var sb = new StringBuilder("");
            var heading = $"№    Имя{new string(' ', 22)} Роль \t\tПароль\n";
            sb.Append(heading);
            sb.Append(new string('_', heading.Length) + "\n");
            var index = 1;
            foreach (var user in users)
            {
                sb.Append($"{index++:D2} - {user.Name}{new string(' ', 24 - user.Name.Length)}" +
                          $"{user.Role}{new string(' ', 15 - user.Role.ToString().Length)}" +
                          $"\t{user.Password}\n");
            }

            return sb.ToString();
        }
    }
}