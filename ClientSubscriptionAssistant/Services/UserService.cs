using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        private readonly ISecureStorageService _secureStorage;

        public UserService(IApiService apiService, ISecureStorageService secureStorage)
        {
            _apiService = apiService;
            _secureStorage = secureStorage;
        }

        /// <summary>
        /// Получить всех пользователей (только для Admin)
        /// </summary>
        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            return await _apiService.GetAsync<IEnumerable<UserDTO>>("api/users");
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        public async Task<ApiResponse<UserDTO>> GetUserByIdAsync(int id)
        {
            return await _apiService.GetAsync<UserDTO>($"api/users/{id}");
        }

        /// <summary>
        /// Получить профиль текущего пользователя
        /// </summary>
        public async Task<ApiResponse<UserDTO>> GetCurrentUserProfileAsync()
        {
            return await _apiService.GetAsync<UserDTO>("api/users/profile");
        }

        /// <summary>
        /// Создать нового пользователя (для Admin)
        /// </summary>
        public async Task<ApiResponse<UserDTO>> CreateUserAsync(CreateUserDTO userDto)
        {
            return await _apiService.PostAsync<UserDTO>("api/users", userDto);
        }

        /// <summary>
        /// Удалить пользователя (для Admin)
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            return await _apiService.DeleteAsync($"api/users/{id}");
        }

        /// <summary>
        /// Обновить роль пользователя (для Admin)
        /// </summary>
        public async Task<ApiResponse<UserDTO>> UpdateUserRoleAsync(int id, int roleId)
        {
            var roleDto = new { RoleId = roleId };
            return await _apiService.PutAsync<UserDTO>($"api/users/{id}/role", roleDto);
        }

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        public async Task<ApiResponse<UserDTO>> UpdateUserAsync(int id, UpdateUserDTO userDto)
        {
            return await _apiService.PutAsync<UserDTO>($"api/users/{id}", userDto);
        }

        /// <summary>
        /// Изменить пароль пользователя
        /// </summary>
        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            return await _apiService.PostAsync<bool>("api/users/change-password", changePasswordDto);
        }

        /// <summary>
        /// Получить подписки пользователя
        /// </summary>
        public async Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetUserSubscriptionsAsync(int userId)
        {
            return await _apiService.GetAsync<IEnumerable<SubscriptionDTO>>($"api/users/{userId}/subscriptions");
        }

        /// <summary>
        /// Получить ежемесячные расходы пользователя
        /// </summary>
        public async Task<ApiResponse<decimal>> GetUserMonthlySpendingAsync(int userId)
        {
            return await _apiService.GetAsync<decimal>($"api/users/{userId}/monthly-spending");
        }

        /// <summary>
        /// Получить статистику пользователя
        /// </summary>
        public async Task<ApiResponse<UserStatisticsDTO>> GetUserStatisticsAsync(int userId)
        {
            return await _apiService.GetAsync<UserStatisticsDTO>($"api/users/{userId}/statistics");
        }

        /// <summary>
        /// Сохранить текущего пользователя локально
        /// </summary>
        public async Task SaveCurrentUserLocallyAsync(UserDTO user)
        {
            var userJson = JsonSerializer.Serialize(user);
            await _secureStorage.SetAsync("current_user", userJson);
        }

        /// <summary>
        /// Загрузить текущего пользователя из локального хранилища
        /// </summary>
        public async Task<UserDTO> LoadCurrentUserFromStorageAsync()
        {
            var userJson = await _secureStorage.GetAsync("current_user");
            if (!string.IsNullOrEmpty(userJson))
            {
                return JsonSerializer.Deserialize<UserDTO>(userJson);
            }
            return null;
        }

        /// <summary>
        /// Очистить данные пользователя из локального хранилища
        /// </summary>
        public async Task ClearUserDataAsync()
        {
            await _secureStorage.RemoveAsync("current_user");
        }
    }
}
