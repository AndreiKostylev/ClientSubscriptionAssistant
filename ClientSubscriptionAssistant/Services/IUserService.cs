using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<ApiResponse<UserDTO>> GetUserByIdAsync(int id);
        Task<ApiResponse<UserDTO>> GetCurrentUserProfileAsync();
        Task<ApiResponse<UserDTO>> CreateUserAsync(CreateUserDTO userDto);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<UserDTO>> UpdateUserRoleAsync(int id, int roleId);
        Task<ApiResponse<UserDTO>> UpdateUserAsync(int id, UpdateUserDTO userDto);
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto);
        Task<ApiResponse<IEnumerable<SubscriptionDTO>>> GetUserSubscriptionsAsync(int userId);
        Task<ApiResponse<decimal>> GetUserMonthlySpendingAsync(int userId);
        Task<ApiResponse<UserStatisticsDTO>> GetUserStatisticsAsync(int userId);
    }
}
