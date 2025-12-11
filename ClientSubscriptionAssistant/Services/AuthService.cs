using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly ISecureStorageService _secureStorage;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public string Token { get; private set; }
        public UserDTO CurrentUser { get; private set; }

        public AuthService(IApiService apiService, ISecureStorageService secureStorage)
        {
            _apiService = apiService;
            _secureStorage = secureStorage;
            InitializeAsync().ConfigureAwait(false);
        }

        private async Task InitializeAsync()
        {
            Token = await _secureStorage.GetAsync("auth_token");
            var userJson = await _secureStorage.GetAsync("current_user");

            if (!string.IsNullOrEmpty(Token))
            {
                _apiService.SetAuthToken(Token);
            }

            if (!string.IsNullOrEmpty(userJson))
            {
                CurrentUser = JsonSerializer.Deserialize<UserDTO>(userJson);
            }
        }

        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var response = await _apiService.PostAsync<AuthResponse>("api/auth/login", loginRequest);

            if (response.IsSuccess && response.Data != null)
            {
                await SaveAuthData(response.Data);
            }

            return response;
        }

        public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            var response = await _apiService.PostAsync<AuthResponse>("api/auth/register", registerRequest);

            if (response.IsSuccess && response.Data != null)
            {
                await SaveAuthData(response.Data);
            }

            return response;
        }

        private async Task SaveAuthData(AuthResponse authResponse)
        {
            Token = authResponse.Token;
            CurrentUser = authResponse.User;

            await _secureStorage.SetAsync("auth_token", Token);
            await _secureStorage.SetAsync("current_user",
                JsonSerializer.Serialize(CurrentUser));

            _apiService.SetAuthToken(Token);
        }

        public async Task LogoutAsync()
        {
            Token = null;
            CurrentUser = null;

            await _secureStorage.RemoveAsync("auth_token");
            await _secureStorage.RemoveAsync("current_user");

            _apiService.SetAuthToken(null);
        }
    }
}
