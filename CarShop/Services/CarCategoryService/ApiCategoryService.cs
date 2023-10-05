using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using CarShop.Services.CarCategoryService;
using CarShop.Services.CarService;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CarShop.Services.CarCategoryService
{
    public class ApiCategoryService : ICarCategoryService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiCarService> _logger;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCarService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<List<CarCategory>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}carcategories/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                    .ReadFromJsonAsync<ResponseData<List<CarCategory>>>
                    (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<CarCategory>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(Категории). Error: {response.StatusCode}");
            return new ResponseData<List<CarCategory>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}"
            };
        }
    }
}
