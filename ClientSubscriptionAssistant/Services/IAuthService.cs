using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);
        Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest);
        Task LogoutAsync();
        bool IsAuthenticated { get; }
        string Token { get; }
        UserDTO CurrentUser { get; }
    }
}
