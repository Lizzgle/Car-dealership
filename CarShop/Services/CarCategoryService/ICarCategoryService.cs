using CarShop.Domain.Models;
using CarShop.Domain.Entities;

namespace CarShop.Services.CarCategoryService
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
