﻿using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarShop.Services.CarService
{
    public class ApiCarService : ICarService
    {
        private HttpClient _httpClient;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiCarService> _logger;
        private readonly HttpContext _httpContext;

        public ApiCarService(HttpClient httpClient, IConfiguration configuration,
                            ILogger<ApiCarService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ResponseData<Car>> CreateProductAsync(Car product, IFormFile? formFile)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Cars");
            var response = await _httpClient.PostAsJsonAsync(uri,
                                                            product,
                                                            _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Car>>(_serializerOptions);
                
                if (formFile != null)
                {
                    await SaveImageAsync(data!.Data.Id, formFile);
                }
                return data;
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

        async Task ICarService.DeleteProductAsync(int id)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.DeleteAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}cars/{id}"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Данные не получены от сервера(машина по id). Error: {response.StatusCode}");
            }
        }

        async Task<ResponseData<Car>> ICarService.GetProductByIdAsync(int id)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}cars/{id}"));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                                        .ReadFromJsonAsync<ResponseData<Car>>
                                        (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Car>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(М по id). Error: {response.StatusCode}");

            return new ResponseData<Car>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера(Мебель по id). Error:{response.StatusCode}"
            };
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
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

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

        async Task ICarService.UpdateProductAsync(int id, Car product, IFormFile? formFile)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}cars/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

            if (formFile != null)
            {
                await SaveImageAsync(id, formFile);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Ответ не получен oт сервера(изменение машины). Error: {response.StatusCode}");
            }

        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Cars/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
    }
}
}
