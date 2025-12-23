using ClientSubscriptionAssistant.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private string _baseUrl = "https://localhost:7093/";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public void SetAuthToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                    return new ApiResponse<T>
                    {
                        IsSuccess = true,
                        Data = data,
                        StatusCode = (int)response.StatusCode
                    };
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 0
                };
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                    return new ApiResponse<T>
                    {
                        IsSuccess = true,
                        Data = result,
                        StatusCode = (int)response.StatusCode
                    };
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 0
                };
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Пытаемся десериализовать, но если не получится - все равно возвращаем success
                    try
                    {
                        var result = JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                        return new ApiResponse<T>
                        {
                            IsSuccess = true,
                            Data = result,
                            StatusCode = (int)response.StatusCode
                        };
                    }
                    catch (JsonException)
                    {
                        // Игнорируем ошибку JSON - операция прошла успешно
                        return new ApiResponse<T>
                        {
                            IsSuccess = true,
                            Data = typeof(T) == typeof(bool) ? (T)(object)true : default,
                            StatusCode = (int)response.StatusCode
                        };
                    }
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 0
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return new ApiResponse<bool>
                {
                    IsSuccess = response.IsSuccessStatusCode,
                    Data = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 0
                };
            }
        }
    }
}
