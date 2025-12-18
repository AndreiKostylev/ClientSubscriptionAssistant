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
    public class MainViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;

        private string _userName = "Гость";
        private string _totalSubscriptions = "0";
        private string _monthlyCost = "0 ₽";

        private ObservableCollection<SubscriptionDTO> _subscriptions;
        private ObservableCollection<CategoryDTO> _categories;

        public MainViewModel(
            IAuthService authService,
            IUserService userService,
            ISubscriptionService subscriptionService)
        {
            _authService = authService;
            _userService = userService;
            _subscriptionService = subscriptionService;

            // Инициализируем коллекции
            Subscriptions = new ObservableCollection<SubscriptionDTO>();
            Categories = new ObservableCollection<CategoryDTO>();

            // Команды
            NavigateToLoginCommand = new Command(async () => await NavigateToLogin());
            NavigateToRegisterCommand = new Command(async () => await NavigateToRegister());
            NavigateToSubscriptionsCommand = new Command(async () => await NavigateToSubscriptions());
            NavigateToProfileCommand = new Command(async () => await NavigateToProfile());
            RefreshCommand = new Command(async () => await LoadDataAsync());

            // Загружаем данные при создании
            LoadDataCommand = new Command(async () => await LoadDataAsync());
        }

        public ICommand LoadDataCommand { get; }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string TotalSubscriptions
        {
            get => _totalSubscriptions;
            set => SetProperty(ref _totalSubscriptions, value);
        }

        public string MonthlyCost
        {
            get => _monthlyCost;
            set => SetProperty(ref _monthlyCost, value);
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

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        public ICommand NavigateToSubscriptionsCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand RefreshCommand { get; }

        private async Task LoadDataAsync()
        {
            if (!_authService.IsAuthenticated)
            {
                // Если не авторизован, показываем данные гостя
                ResetToGuestData();
                return;
            }

            IsBusy = true;

            try
            {
                // Получаем текущего пользователя
                var currentUser = _authService.CurrentUser;
                if (currentUser != null)
                {
                    UserName = currentUser.Username;

                    // Загружаем подписки пользователя
                    var subscriptionsResult = await _subscriptionService.GetUserSubscriptionsAsync(currentUser.Id);
                    if (subscriptionsResult.IsSuccess && subscriptionsResult.Data != null)
                    {
                        var subscriptions = subscriptionsResult.Data.ToList();
                        TotalSubscriptions = subscriptions.Count.ToString();

                        // Рассчитываем месячные расходы
                        decimal monthlyCost = 0;
                        foreach (var sub in subscriptions.Where(s => s.IsActive))
                        {
                            if (sub.BillingCycle == "monthly")
                                monthlyCost += sub.Price;
                            else if (sub.BillingCycle == "yearly")
                                monthlyCost += sub.Price / 12;
                        }

                        MonthlyCost = $"{monthlyCost:F0} ₽";

                        // Обновляем коллекцию подписок
                        Subscriptions.Clear();
                        foreach (var subscription in subscriptions.Take(5)) // Берем только 5 последних
                        {
                            Subscriptions.Add(subscription);
                        }
                    }

                    // Загружаем категории (пока заглушка или через API если есть)
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка",
                    $"Не удалось загрузить данные: {ex.Message}", "OK");
                ResetToGuestData();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadCategories()
        {
            // Здесь можно загрузить категории из API
            // Пока используем статические данные
            Categories.Clear();

            var defaultCategories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "🎬 Стриминговые сервисы", Description = "Видео и музыкальные платформы" },
                new CategoryDTO { Id = 2, Name = "💻 Программное обеспечение", Description = "Подписки на ПО и приложения" },
                new CategoryDTO { Id = 3, Name = "☁️ Облачные сервисы", Description = "Хранилища и облачные решения" },
                new CategoryDTO { Id = 4, Name = "🎮 Игры", Description = "Игровые подписки и сервисы" },
                new CategoryDTO { Id = 5, Name = "📚 Образование", Description = "Онлайн-курсы и обучающие платформы" }
            };

            foreach (var category in defaultCategories)
            {
                Categories.Add(category);
            }
        }

        private void ResetToGuestData()
        {
            UserName = "Гость";
            TotalSubscriptions = "0";
            MonthlyCost = "0 ₽";
            Subscriptions.Clear();
            LoadCategories(); // Все равно показываем категории для гостя
        }

        private async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }

        private async Task NavigateToSubscriptions()
        {
            if (!_authService.IsAuthenticated)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }
            await Shell.Current.GoToAsync("//SubscriptionPage");
        }

        private async Task NavigateToProfile()
        {
            if (!_authService.IsAuthenticated)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }
            await Shell.Current.GoToAsync("//ProfilePage");
        }
    }
}
