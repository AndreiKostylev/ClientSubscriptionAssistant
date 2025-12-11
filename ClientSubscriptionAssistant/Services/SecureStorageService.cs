using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        public async Task<string> GetAsync(string key)
        {
            try
            {
                return await SecureStorage.Default.GetAsync(key);
            }
            catch
            {
                return null;
            }
        }

        public async Task SetAsync(string key, string value)
        {
            try
            {
                await SecureStorage.Default.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to secure storage: {ex.Message}");
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                SecureStorage.Default.Remove(key);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing from secure storage: {ex.Message}");
            }
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            try
            {
                var value = await GetAsync(key);
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                return false;
            }
        }
    }
}
