namespace TestMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            MainPage.IsDetect = false;
            Shell.Current.Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

    }
}
