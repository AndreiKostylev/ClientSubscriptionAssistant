namespace ClientSubscriptionAssistant.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        LoadingIndicator.IsRunning = true;
        ErrorLabel.IsVisible = false;

   
        await Task.Delay(1000); 

        LoadingIndicator.IsRunning = false;

       
        await Shell.Current.GoToAsync("//main");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
    }
}