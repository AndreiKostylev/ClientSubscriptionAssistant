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

        private ObservableCollection<SubscriptionDTO> _subscriptions;
        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<ServiceDTO> _services;
        private SubscriptionDTO _selectedSubscription;
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
            ISubscriptionService subscriptionService)
        {
            _authService = authService;
            _subscriptionService = subscriptionService;

            Subscriptions = new ObservableCollection<SubscriptionDTO>();
            Categories = new ObservableCollection<CategoryDTO>();
            Services = new ObservableCollection<ServiceDTO>();

     
            InitializeTestData();


            LoadDataCommand = new Command(async () => await LoadDataAsync());
            RefreshCommand = new Command(async () => await LoadDataAsync());
            NavigateToMainCommand = new Command(async () => await NavigateToMain());

    
            FilterAllCommand = new Command(() =>
            {
                Filter = "all";
                UpdateFilterColors();
                LoadDataCommand.Execute(null);
            });

            FilterActiveCommand = new Command(() =>
            {
                Filter = "active";
                UpdateFilterColors();
                LoadDataCommand.Execute(null);
            });

            FilterInactiveCommand = new Command(() =>
            {
                Filter = "inactive";
                UpdateFilterColors();
                LoadDataCommand.Execute(null);
            });

       
            ShowAddFormCommand = new Command(() =>
            {
                ShowAddForm = true;
                ResetAddForm();
            });

            HideAddFormCommand = new Command(() => ShowAddForm = false);
            AddSubscriptionCommand = new Command(async () => await AddSubscriptionAsync());

         
            LoadDataCommand.Execute(null);
        }

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

        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateToMainCommand { get; }
        public ICommand FilterAllCommand { get; }
        public ICommand FilterActiveCommand { get; }
        public ICommand FilterInactiveCommand { get; }
        public ICommand ShowAddFormCommand { get; }
        public ICommand HideAddFormCommand { get; }
        public ICommand AddSubscriptionCommand { get; }

        private void InitializeTestData()
        {
            
            var testCategories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "🎬 Стриминговые сервисы" },
                new CategoryDTO { Id = 2, Name = "💻 Программное обеспечение" },
                new CategoryDTO { Id = 3, Name = "☁️ Облачные сервисы" },
                new CategoryDTO { Id = 4, Name = "🎮 Игры" },
                new CategoryDTO { Id = 5, Name = "📚 Образование" }
            };

            Categories.Clear();
            foreach (var category in testCategories)
            {
                Categories.Add(category);
            }
            SelectedCategory = Categories.FirstOrDefault();

            // Тестовые сервисы
            var testServices = new List<ServiceDTO>
            {
                new ServiceDTO { Id = 1, Name = "Netflix", BasePrice = 599 },
                new ServiceDTO { Id = 2, Name = "Spotify", BasePrice = 299 },
                new ServiceDTO { Id = 3, Name = "YouTube Premium", BasePrice = 399 },
                new ServiceDTO { Id = 4, Name = "Microsoft 365", BasePrice = 799 },
                new ServiceDTO { Id = 5, Name = "Adobe Creative Cloud", BasePrice = 2499 }
            };

            Services.Clear();
            foreach (var service in testServices)
            {
                Services.Add(service);
            }
            SelectedService = Services.FirstOrDefault();
        }

        private void UpdateFilterColors()
        {
        
            AllFilterColor = Color.FromArgb("#6C757D");
            ActiveFilterColor = Color.FromArgb("#6C757D");
            InactiveFilterColor = Color.FromArgb("#6C757D");

         
            switch (Filter)
            {
                case "all":
                    AllFilterColor = Color.FromArgb("#007ACC");
                    break;
                case "active":
                    ActiveFilterColor = Color.FromArgb("#007ACC");
                    break;
                case "inactive":
                    InactiveFilterColor = Color.FromArgb("#007ACC");
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            if (!_authService.IsAuthenticated)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Для просмотра подписок необходимо войти в систему", "OK");
                await NavigateToMain();
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
                    // Фильтруем подписки
                    var filteredSubscriptions = FilterSubscriptions(result.Data);

                    Subscriptions.Clear();
                    foreach (var subscription in filteredSubscriptions)
                    {
                        Subscriptions.Add(subscription);
                    }

                    
                    CalculateTotalMonthlyCost();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка",
                        "Не удалось загрузить подписки", "OK");
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

        private IEnumerable<SubscriptionDTO> FilterSubscriptions(IEnumerable<SubscriptionDTO> subscriptions)
        {
            return Filter switch
            {
                "active" => subscriptions.Where(s => s.IsActive),
                "inactive" => subscriptions.Where(s => !s.IsActive),
                _ => subscriptions // "all"
            };
        }

        private void CalculateTotalMonthlyCost()
        {
            decimal total = 0;
            foreach (var subscription in Subscriptions.Where(s => s.IsActive))
            {
                if (subscription.BillingCycle == "monthly")
                    total += subscription.Price;
                else if (subscription.BillingCycle == "yearly")
                    total += subscription.Price / 12;
            }
            TotalMonthlyCost = total;
        }

        private async Task AddSubscriptionAsync()
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(NewSubscriptionName))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Введите название подписки", "OK");
                return;
            }

            if (NewSubscriptionPrice <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Цена должна быть больше 0", "OK");
                return;
            }

            if (SelectedCategory == null || SelectedService == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Выберите категорию и сервис", "OK");
                return;
            }

            if (!_authService.IsAuthenticated)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Для добавления подписки необходимо войти в систему", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser == null) return;

              
                await Task.Delay(500);

                var newSubscription = new SubscriptionDTO
                {
                    Id = Subscriptions.Count + 100, 
                    Name = NewSubscriptionName.Trim(),
                    Price = NewSubscriptionPrice,
                    StartDate = NewSubscriptionStartDate,
                    NextPaymentDate = NewSubscriptionStartDate.AddMonths(1),
                    BillingCycle = NewSubscriptionBillingCycle,
                    IsActive = true,
                    CategoryId = SelectedCategory.Id,
                    ServiceId = SelectedService.Id,
                    Category = SelectedCategory,
                    Service = SelectedService
                };

                Subscriptions.Insert(0, newSubscription);

                await Application.Current.MainPage.DisplayAlert("Успешно",
                    "Подписка добавлена", "OK");

                
                ShowAddForm = false;
                ClearAddForm();

                CalculateTotalMonthlyCost();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Не удалось добавить подписку: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ResetAddForm()
        {
            NewSubscriptionName = string.Empty;
            NewSubscriptionPrice = 0;
            NewSubscriptionStartDate = DateTime.Today;
            NewSubscriptionBillingCycle = "monthly";
            SelectedCategory = Categories.FirstOrDefault();
            SelectedService = Services.FirstOrDefault();
        }

        private void ClearAddForm()
        {
            ResetAddForm();
        }

       
        public async Task DeleteSubscriptionAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Удаление подписки",
                $"Вы уверены, что хотите удалить подписку \"{subscription.Name}\"?",
                "Да", "Нет");

            if (!confirm) return;

            IsBusy = true;
            try
            {
                var result = await _subscriptionService.DeleteSubscriptionAsync(subscription.Id);
                if (result.IsSuccess && result.Data)
                {
                    await Application.Current.MainPage.DisplayAlert("Успешно",
                        "Подписка удалена", "OK");
                    await LoadDataAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка",
                        "Не удалось удалить подписку", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Ошибка при удалении: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DeactivateSubscriptionAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Деактивация подписки",
                $"Вы уверены, что хотите деактивировать подписку \"{subscription.Name}\"?",
                "Да", "Нет");

            if (!confirm) return;

            IsBusy = true;
            try
            {
                var result = await _subscriptionService.DeactivateSubscriptionAsync(subscription.Id);
                if (result.IsSuccess && result.Data)
                {
                    await Application.Current.MainPage.DisplayAlert("Успешно",
                        "Подписка деактивирована", "OK");
                    await LoadDataAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка",
                        "Не удалось деактивировать подписку", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Ошибка при деактивации: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ShowSubscriptionDetailsAsync(SubscriptionDTO subscription)
        {
            if (subscription == null) return;

            await Application.Current.MainPage.DisplayAlert(
                subscription.Name,
                $"Сервис: {subscription.Service?.Name ?? "Не указан"}\n" +
                $"Категория: {subscription.Category?.Name ?? "Не указана"}\n" +
                $"Цена: {subscription.Price:C}\n" +
                $"Цикл оплаты: {subscription.BillingCycle}\n" +
                $"Дата начала: {subscription.StartDate:dd.MM.yyyy}\n" +
                $"След. оплата: {subscription.NextPaymentDate:dd.MM.yyyy}\n" +
                $"Статус: {(subscription.IsActive ? "Активна" : "Неактивна")}",
                "OK");
        }

        private async Task NavigateToMain()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
