using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IApiService _apiService;

        public SubscriptionService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetUserSubscriptionsAsync(int userId)
        {
            return await _apiService.GetAsync<IEnumerable<SubscriptionDTO>>($"api/subscriptions/user/{userId}");
        }

        public async Task<ApiResponse<SubscriptionDTO>> GetSubscriptionByIdAsync(int id)
        {
            return await _apiService.GetAsync<SubscriptionDTO>($"api/subscriptions/{id}");
        }

        public async Task<ApiResponse<SubscriptionDTO>> CreateSubscriptionAsync(int userId, CreateSubscriptionDTO subscriptionDto)
        {
            return await _apiService.PostAsync<SubscriptionDTO>($"api/subscriptions/user/{userId}", subscriptionDto);
        }

        public async Task<ApiResponse<bool>> DeactivateSubscriptionAsync(int id)
        {
            return await _apiService.PutAsync<bool>($"api/subscriptions/{id}/deactivate", new { });
        }

        public async Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetExpiringSubscriptionsAsync(int daysBeforeExpiry)
        {
            return await _apiService.GetAsync<IEnumerable<SubscriptionDTO>>($"api/subscriptions/expiring/{daysBeforeExpiry}");
        }

        public async Task<ApiResponse<bool>> DeleteSubscriptionAsync(int id)
        {
            return await _apiService.DeleteAsync($"api/subscriptions/{id}");
        }

        public async Task<ApiResponse<SubscriptionDTO>> UpdateSubscriptionAsync(int id, UpdateSubscriptionDTO subscriptionDto)
        {
            return await _apiService.PutAsync<SubscriptionDTO>($"api/subscriptions/{id}", subscriptionDto);
        }
    }
}
