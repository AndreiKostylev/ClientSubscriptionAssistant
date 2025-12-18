using ClientSubscriptionAssistant.ViewModels;

namespace ClientSubscriptionAssistant.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public RegisterPage(RegisterViewModel viewModel)
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
            _viewModel.Username = string.Empty;
            _viewModel.Email = string.Empty;
            _viewModel.Password = string.Empty;
            _viewModel.ConfirmPassword = string.Empty;
            _viewModel.ErrorMessage = string.Empty;
        }
    }

    private void OnConfirmPasswordCompleted(object sender, EventArgs e)
    {
        if (_viewModel != null &&
            !string.IsNullOrEmpty(_viewModel.Username) &&
            !string.IsNullOrEmpty(_viewModel.Email) &&
            !string.IsNullOrEmpty(_viewModel.Password) &&
            !string.IsNullOrEmpty(_viewModel.ConfirmPassword))
        {
            if (_viewModel.RegisterCommand.CanExecute(null))
            {
                _viewModel.RegisterCommand.Execute(null);
            }
        }
    }
}