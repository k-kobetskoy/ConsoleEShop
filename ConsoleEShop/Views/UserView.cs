using System.Text;

namespace ConsoleEShop.Views
{
    public class UserView : IView
    {
        private readonly User user;

        public UserView(User user)
        {
            this.user = user;
        }
        public string ShowViewData()
        {
            if (user is null)
            {
                return "There are no users to show";
            }

            var sb = new StringBuilder("");
            sb.Append($"Name: {user.Name}\n" +
                      $"Role: {user.Role}");
            return sb.ToString();
        }
    }
}