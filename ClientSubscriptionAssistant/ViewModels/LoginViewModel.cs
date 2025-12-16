using ClientSubscriptionAssistant.Models.DTO;
using ClientSubscriptionAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientSubscriptionAssistant.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await NavigateToRegisterAsync());
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Пожалуйста, заполните все поля";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var loginRequest = new LoginRequest
                {
                    Login = Login,
                    Password = Password
                };

                var result = await _authService.LoginAsync(loginRequest);

                if (result.IsSuccess && result.Data != null)
                {
                    // Успешный вход
                    await Shell.Current.GoToAsync("//MainPage");
                    ClearFields();
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToRegisterAsync()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }

        private void ClearFields()
        {
            Login = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}
