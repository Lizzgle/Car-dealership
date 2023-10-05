using CarShop.Domain.Entities;
using CarShop.Domain.Models;

namespace CarShop.API.Services
{
    public interface ICarCategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<CarCategory>>> GetCategoryListAsync();
    }
}
