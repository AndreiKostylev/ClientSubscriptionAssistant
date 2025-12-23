using ClientSubscriptionAssistant.Models.DTO;
using ClientSubscriptionAssistant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientSubscriptionAssistant.ViewModels
{
    public class SubscriptionViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IApiService _apiService;

        private ObservableCollection<SubscriptionDTO> _subscriptions = new();
        private ObservableCollection<CategoryDTO> _categories = new();
        private ObservableCollection<ServiceDTO> _services = new();

        private string _filter = "all";
        private decimal _totalMonthlyCost;
        private bool _showAddForm = false;

        private string _newSubscriptionName = string.Empty;
        private decimal _newSubscriptionPrice;
        private DateTime _newSubscriptionStartDate = DateTime.Today;
        private string _newSubscriptionBillingCycle = "monthly";

        private CategoryDTO _selectedCategory;
        private ServiceDTO _selectedService;

        private Color _allFilterColor = Color.FromArgb("#007ACC");
        private Color _activeFilterColor = Color.FromArgb("#6C757D");
        private Color _inactiveFilterColor = Color.FromArgb("#6C757D");

        public SubscriptionViewModel(
            IAuthService authService,
            ISubscriptionService subscriptionService,
            IApiService apiService)
        {
            _authService = authService;
            _subscriptionService = subscriptionService;
            _apiService = apiService;

            InitializeCommands();
            LoadDataCommand.Execute(null);
        }

        #region Properties
        public ObservableCollection<SubscriptionDTO> Subscriptions
        {
            get => _subscriptions;
            set => SetProperty(ref _subscriptions, value);
        }

        public ObservableCollection<CategoryDTO> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ObservableCollection<ServiceDTO> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public decimal TotalMonthlyCost
        {
            get => _totalMonthlyCost;
            set => SetProperty(ref _totalMonthlyCost, value);
        }

        public bool ShowAddForm
        {
            get => _showAddForm;
            set => SetProperty(ref _showAddForm, value);
        }

        public string NewSubscriptionName
        {
            get => _newSubscriptionName;
            set => SetProperty(ref _newSubscriptionName, value);
        }

        public decimal NewSubscriptionPrice
        {
            get => _newSubscriptionPrice;
            set => SetProperty(ref _newSubscriptionPrice, value);
        }

        public DateTime NewSubscriptionStartDate
        {
            get => _newSubscriptionStartDate;
            set => SetProperty(ref _newSubscriptionStartDate, value);
        }

        public string NewSubscriptionBillingCycle
        {
            get => _newSubscriptionBillingCycle;
            set => SetProperty(ref _newSubscriptionBillingCycle, value);
        }

        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public ServiceDTO SelectedService
        {
            get => _selectedService;
            set => SetProperty(ref _selectedService, value);
        }

        public Color AllFilterColor
        {
            get => _allFilterColor;
            set => SetProperty(ref _allFilterColor, value);
        }

        public Color ActiveFilterColor
        {
            get => _activeFilterColor;
            set => SetProperty(ref _activeFilterColor, value);
        }

        public Color InactiveFilterColor
        {
            get => _inactiveFilterColor;
            set => SetProperty(ref _inactiveFilterColor, value);
        }
        #endregion

        #region Commands
        public ICommand LoadDataCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand NavigateToMainCommand { get; private set; }
        public ICommand FilterAllCommand { get; private set; }
        public ICommand FilterActiveCommand { get; private set; }
        public ICommand FilterInactiveCommand { get; private set; }
        public ICommand ShowAddFormCommand { get; private set; }
        public ICommand HideAddFormCommand { get; private set; }
        public ICommand AddSubscriptionCommand { get; private set; }
        #endregion

        #region Private Methods
        private void InitializeCommands()
        {
            LoadDataCommand = new Command(async () => await LoadDataAsync());
            RefreshCommand = new Command(async () => await LoadDataAsync());
            NavigateToMainCommand = new Command(async () => await NavigateToMain());

            FilterAllCommand = new Command(() => ApplyFilter("all"));
            FilterActiveCommand = new Command(() => ApplyFilter("active"));
            FilterInactiveCommand = new Command(() => ApplyFilter("inactive"));

            ShowAddFormCommand = new Command(async () =>
            {
                await LoadCategoriesAndServices();
                ShowAddForm = true;
                ResetAddForm();
            });

            HideAddFormCommand = new Command(() => ShowAddForm = false);
            AddSubscriptionCommand = new Command(async () => await AddSubscriptionAsync());
        }

        private void ApplyFilter(string filter)
        {
            Filter = filter;
            UpdateFilterColors();
            LoadDataCommand.Execute(null);
        }

        private async Task LoadCategoriesAndServices()
        {
            if (!_authService.IsAuthenticated) return;

            IsBusy = true;
            try
            {
                await LoadCategories();
                await LoadServices();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Не удалось загрузить данные: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadCategories()
        {
            var result = await _apiService.GetAsync<IEnumerable<CategoryDTO>>("api/categories");

            if (result.IsSuccess && result.Data != null)
            {
                Categories.Clear();
                foreach (var category in result.Data)
                {
                    Categories.Add(category);
                }
            }
            else
            {
                await LoadDefaultCategories();
            }

            SelectedCategory = Categories.FirstOrDefault();
        }

        private async Task LoadServices()
        {
            var result = await _apiService.GetAsync<IEnumerable<ServiceDTO>>("api/services");

            if (result.IsSuccess && result.Data != null)
            {
                Services.Clear();
                foreach (var service in result.Data)
                {
                    Services.Add(service);
                }
            }
            else
            {
                await LoadDefaultServices();
            }

            SelectedService = Services.FirstOrDefault();
        }

        private async Task LoadDefaultCategories()
        {
            var defaultCategories = new List<CategoryDTO>
            {
                new() { Id = 1, Name = "🎬 Стриминговые сервисы", Description = "Видео и музыкальные платформы" },
                new() { Id = 2, Name = "💻 Программное обеспечение", Description = "Подписки на ПО и приложения" },
                new() { Id = 3, Name = "☁️ Облачные сервисы", Description = "Хранилища и облачные решения" },
                new() { Id = 4, Name = "🎮 Игры", Description = "Игровые подписки и сервисы" },
                new() { Id = 5, Name = "📚 Образование", Description = "Онлайн-курсы и обучающие платформы" }
            };

            Categories.Clear();
            foreach (var category in defaultCategories)
            {
                Categories.Add(category);
            }
        }

        private async Task LoadDefaultServices()
        {
            var defaultServices = new List<ServiceDTO>
            {
                new() { Id = 1, Name = "Netflix", BasePrice = 599 },
                new() { Id = 2, Name = "Spotify", BasePrice = 299 },
                new() { Id = 3, Name = "YouTube Premium", BasePrice = 399 },
                new() { Id = 4, Name = "Microsoft 365", BasePrice = 799 },
                new() { Id = 5, Name = "Adobe Creative Cloud", BasePrice = 2499 }
            };

            Services.Clear();
            foreach (var service in defaultServices)
            {
                Services.Add(service);
            }
        }

        private void UpdateFilterColors()
        {
            AllFilterColor = Color.FromArgb("#6C757D");
            ActiveFilterColor = Color.FromArgb("#6C757D");
            InactiveFilterColor = Color.FromArgb("#6C757D");

            var activeColor = Color.FromArgb("#007ACC");

            switch (Filter)
            {
                case "all": AllFilterColor = activeColor; break;
                case "active": ActiveFilterColor = activeColor; break;
                case "inactive": InactiveFilterColor = activeColor; break;
            }
        }

        private async Task LoadDataAsync()
        {
            if (!_authService.IsAuthenticated)
            {
                await ShowAuthError();
                return;
            }

            IsBusy = true;
            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser == null) return;

                var result = await _subscriptionService.GetUserSubscriptionsAsync(currentUser.Id);

                if (result.IsSuccess && result.Data != null)
                {
                    UpdateSubscriptions(result.Data);
                    CalculateTotalMonthlyCost();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка",
                        result.ErrorMessage ?? "Не удалось загрузить подписки", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Не удалось загрузить данные: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateSubscriptions(IEnumerable<SubscriptionDTO> subscriptions)
        {
            var filtered = Filter switch
            {
                "active" => subscriptions.Where(s => s.IsActive),
                "inactive" => subscriptions.Where(s => !s.IsActive),
                _ => subscriptions
            };

            Subscriptions.Clear();
            foreach (var subscription in filtered)
            {
                Subscriptions.Add(subscription);
            }
        }

        private void CalculateTotalMonthlyCost()
        {
            decimal total = 0;
            foreach (var subscription in Subscriptions.Where(s => s.IsActive))
            {
                total += subscription.BillingCycle == "monthly"
                    ? subscription.Price
                    : subscription.Price / 12;
            }
            TotalMonthlyCost = total;
        }

        private async Task AddSubscriptionAsync()
        {
            if (!ValidateForm())
                return;

            if (!_authService.IsAuthenticated)
            {
                await ShowAuthError();
                return;
            }

            IsBusy = true;
            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser == null) return;

                var subscriptionDto = new CreateSubscriptionDTO
                {
                    Name = NewSubscriptionName.Trim(),
                    Price = NewSubscriptionPrice,
                    StartDate = NewSubscriptionStartDate.ToUniversalTime(), // UTC формат
                    BillingCycle = NewSubscriptionBillingCycle,
                    CategoryId = SelectedCategory?.Id,
                    ServiceId = SelectedService.Id
                };

                var result = await _subscriptionService.CreateSubscriptionAsync(
                    currentUser.Id,
                    subscriptionDto
                );

                if (result.IsSuccess && result.Data != null)
                {
                    await ShowSuccess("Подписка добавлена");
                    ShowAddForm = false;
                    ClearAddForm();
                    await LoadDataAsync();
                }
                else
                {
                    await ShowError(result.ErrorMessage ?? "Не удалось добавить подписку");
                }
            }
            catch (Exception ex)
            {
                await ShowError($"Не удалось добавить подписку: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(NewSubscriptionName))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Введите название подписки", "OK");
                return false;
            }

            if (NewSubscriptionPrice <= 0)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Цена должна быть больше 0", "OK");
                return false;
            }

            if (SelectedService == null)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Выберите сервис", "OK");
                return false;
            }

            return true;
        }

        private void ResetAddForm()
        {
            NewSubscriptionName = string.Empty;
            NewSubscriptionPrice = 0;
            NewSubscriptionStartDate = DateTime.Today;
            NewSubscriptionBillingCycle = "monthly";

            if (Categories.Count > 0 && SelectedCategory == null)
                SelectedCategory = Categories.FirstOrDefault();

            if (Services.Count > 0 && SelectedService == null)
                SelectedService = Services.FirstOrDefault();
        }

        private void ClearAddForm()
        {
            ResetAddForm();
        }

        public async Task DeleteSubscriptionAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Удаление подписки",
                $"Вы уверены, что хотите удалить подписку \"{subscription.Name}\"?",
                "Да", "Нет");

            if (!confirm) return;

            await ExecuteWithBusy(async () =>
            {
                var result = await _subscriptionService.DeleteSubscriptionAsync(subscription.Id);
                if (result.IsSuccess && result.Data)
                {
                    await ShowSuccess("Подписка удалена");
                    await LoadDataAsync();
                }
                else
                {
                    await ShowError(result.ErrorMessage ?? "Не удалось удалить подписку");
                }
            });
        }

        public async Task DeactivateSubscriptionAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Деактивация подписки",
                $"Вы уверены, что хотите деактивировать подписку \"{subscription.Name}\"?",
                "Да", "Нет");

            if (!confirm) return;

            await ExecuteWithBusy(async () =>
            {
                var result = await _subscriptionService.DeactivateSubscriptionAsync(subscription.Id);
                if (result.IsSuccess && result.Data)
                {
                    await ShowSuccess("Подписка деактивирована");
                    await LoadDataAsync();
                }
                else
                {
                    await ShowError(result.ErrorMessage ?? "Не удалось деактивировать подписку");
                }
            });
        }

        public async Task ShowSubscriptionDetailsAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            var details = $"Сервис: {subscription.Service?.Name ?? "Не указан"}\n" +
                         $"Категория: {subscription.Category?.Name ?? "Не указана"}\n" +
                         $"Цена: {subscription.Price:C}\n" +
                         $"Цикл оплаты: {subscription.BillingCycle}\n" +
                         $"Дата начала: {subscription.StartDate:dd.MM.yyyy}\n" +
                         $"След. оплата: {subscription.NextPaymentDate:dd.MM.yyyy}\n" +
                         $"Статус: {(subscription.IsActive ? "Активна" : "Неактивна")}";

            await Application.Current.MainPage.DisplayAlert(subscription.Name, details, "OK");
        }

        private async Task ExecuteWithBusy(Func<Task> action)
        {
            IsBusy = true;
            try
            {
                await action();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ShowAuthError()
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка",
                "Для просмотра подписок необходимо войти в систему", "OK");
            await NavigateToMain();
        }

        private async Task ShowSuccess(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Успешно", message, "OK");
        }

        private async Task ShowError(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", message, "OK");
        }

        private async Task NavigateToMain()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        #endregion
    }
}
