using ClientSubscriptionAssistant.Services;
using ClientSubscriptionAssistant.ViewModels;
using ClientSubscriptionAssistant.Views;
using Microsoft.Extensions.Logging;

namespace ClientSubscriptionAssistant
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Регистрация сервисов
            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();

            // Регистрация ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<SubscriptionViewModel>();

            // Регистрация страниц
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
          
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<SubscriptionPage>();
            builder.Services.AddTransient<ProfilePage>();

            // Регистрация AppShell
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
