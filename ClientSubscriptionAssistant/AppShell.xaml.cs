namespace ClientSubscriptionAssistant
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Настройка маршрутов
            Routing.RegisterRoute("login", typeof(Views.LoginPage));
            Routing.RegisterRoute("register", typeof(Views.RegisterPage));
            Routing.RegisterRoute("subscriptions", typeof(Views.SubscriptionPage));
            Routing.RegisterRoute("profile", typeof(Views.ProfilePage));
            GoToAsync("//LoginPage", false);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Проверка авторизации при запуске
            CheckAuthentication();
        }

        private async void CheckAuthentication()
        {
            // Можно добавить логику проверки токена
            await Task.Delay(100);
        }
    }
}
