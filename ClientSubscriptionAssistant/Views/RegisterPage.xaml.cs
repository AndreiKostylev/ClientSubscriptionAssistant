namespace ClientSubscriptionAssistant.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        LoadingIndicator.IsRunning = true;
        ErrorLabel.IsVisible = false;

  
        if (string.IsNullOrEmpty(UsernameEntry.Text) ||
            string.IsNullOrEmpty(EmailEntry.Text) ||
            string.IsNullOrEmpty(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Заполните все поля";
            ErrorLabel.IsVisible = true;
            LoadingIndicator.IsRunning = false;
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ErrorLabel.Text = "Пароли не совпадают";
            ErrorLabel.IsVisible = true;
            LoadingIndicator.IsRunning = false;
            return;
        }

 
        await Task.Delay(1000); 

        LoadingIndicator.IsRunning = false;

       
        await Shell.Current.GoToAsync("//main");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}