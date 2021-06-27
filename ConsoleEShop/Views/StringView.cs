namespace ConsoleEShop.Views
{
    public class StringView : IView
    {
        private readonly string message;

        public StringView(string message)
        {
            this.message = message;
        }
        public string ShowViewData()
        {
            return message;
        }
    }
}