using CarShop.API.Data;
using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Services
{
    public class CarCategoryService : ICarCategoryService
    {
        private readonly DbSet<CarCategory> _categories;

        public CarCategoryService(AppDbContext context)
        {
            _categories = context.CarCategories;
        }
        public async Task<ResponseData<List<CarCategory>>> GetCategoryListAsync()
        {
            ResponseData<List<CarCategory>> response = new();
            try
            {
                response.Data = await _categories.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
