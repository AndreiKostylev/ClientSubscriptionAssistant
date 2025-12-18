using ClientSubscriptionAssistant.ViewModels;
using System.Globalization;

namespace ClientSubscriptionAssistant.Views;


public partial class SubscriptionPage : ContentPage
{
    private readonly SubscriptionViewModel _viewModel;

    public SubscriptionPage(SubscriptionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

 
        if (_viewModel != null && _viewModel.LoadDataCommand.CanExecute(null))
        {
            _viewModel.LoadDataCommand.Execute(null);
        }
    }

    private async void OnSubscriptionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.DTO.SubscriptionDTO selectedSubscription)
        {
            await _viewModel.ShowSubscriptionDetailsAsync(selectedSubscription);

        
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Models.DTO.SubscriptionDTO subscription)
        {
            await _viewModel.DeleteSubscriptionAsync(subscription);
        }
    }

    private async void OnDeactivateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Models.DTO.SubscriptionDTO subscription)
        {
            await _viewModel.DeactivateSubscriptionAsync(subscription);
        }
    }
}
