namespace ConsoleEShop.Views
{
    public class PageData : IView
    {
        private readonly IView menuView;
        private readonly IView pageView;

        public PageData(IView menuView, IView pageView)
        {
            this.menuView = menuView;
            this.pageView = pageView;
        }
        public string ShowViewData()
        {
            return menuView.ShowViewData() + pageView.ShowViewData();
        }
    }
}