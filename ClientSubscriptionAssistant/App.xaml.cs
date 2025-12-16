namespace ClientSubscriptionAssistant
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            this.RequestedThemeChanged += OnRequestedThemeChanged;
        }
        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            
        }
        protected override void OnStart()
        {
            base.OnStart();
            
        }
        protected override void OnSleep()
        {
            base.OnSleep();
            
        }
        protected override void OnResume()
        {
            base.OnResume();
          
        }
    }
}
