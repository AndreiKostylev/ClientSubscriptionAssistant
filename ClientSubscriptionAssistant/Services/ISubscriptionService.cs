using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public interface ISubscriptionService
    {
        Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetUserSubscriptionsAsync(int userId);
        Task<ApiResponse<SubscriptionDTO>> GetSubscriptionByIdAsync(int id);
       
        Task<ApiResponse<bool>> DeactivateSubscriptionAsync(int id);
        Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetExpiringSubscriptionsAsync(int daysBeforeExpiry);
        Task<ApiResponse<bool>> DeleteSubscriptionAsync(int id);
        Task<ApiResponse<SubscriptionDTO>> UpdateSubscriptionAsync(int id, UpdateSubscriptionDTO subscriptionDto);
    }

}
