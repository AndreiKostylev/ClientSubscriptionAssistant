using ClientSubscriptionAssistant.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;

namespace ClientSubscriptionAssistant.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

       
        if (_viewModel != null)
        {
            _viewModel.Login = string.Empty;
            _viewModel.Password = string.Empty;
            _viewModel.ErrorMessage = string.Empty;
        }
    }


    private void OnPasswordCompleted(object sender, EventArgs e)
    {
        if (_viewModel != null &&
            !string.IsNullOrEmpty(_viewModel.Login) &&
            !string.IsNullOrEmpty(_viewModel.Password))
        {
            if (_viewModel.LoginCommand.CanExecute(null))
            {
                _viewModel.LoginCommand.Execute(null);
            }
        }
    }
}
