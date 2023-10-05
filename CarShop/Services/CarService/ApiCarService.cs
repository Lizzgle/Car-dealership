using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CarShop.Services.CarService
{
    public class ApiCarService : ICarService
    {
        private readonly HttpClient _httpClient;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiCarService> _logger;

        public ApiCarService(HttpClient httpClient, IConfiguration configuration,
                            ILogger<ApiCarService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Car>> CreateProductAsync(Car product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Cars");
            var response = await _httpClient.PostAsJsonAsync(uri,
                                                            product,
                                                            _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                .Content
                .ReadFromJsonAsync<ResponseData<Car>>
                (_serializerOptions);
                return data; // dish;
            }
            _logger
            .LogError($"-----> object not created. Error:" +
            $"{ response.StatusCode.ToString()}");
            return new ResponseData<Car>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:" +
                $"{ response.StatusCode.ToString() }"};
        }

        Task ICarService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Car>> ICarService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Car>>> GetProductListAsync(string? categoryNormalizedName,
                                                                            int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}cars/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno={pageNo}/");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            // отправить запрос к API
            var response = await _httpClient.GetAsync(
                                        new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                        .ReadFromJsonAsync<ResponseData<ListModel<Car>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Car>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:" +
                $"{ response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Car>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера1. Error:" +
                $"{ response.StatusCode.ToString() }"
            };
        }

        Task ICarService.UpdateProductAsync(int id, Car product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
