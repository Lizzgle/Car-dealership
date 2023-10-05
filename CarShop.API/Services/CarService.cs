using CarShop.API.Data;
using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Services
{
    public class CarService : ICarService
    {
        private readonly int _maxPageSize = 20;
        private DbSet<Car> _car;

        public CarService(AppDbContext dbContext) 
        {
            _car = dbContext.Cars;
        }
        public Task<ResponseData<Car>> CreateProductAsync(Car product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Car>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Car>>> GetProductListAsync(string? categoryNormalizedName,
                                                                                int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _car.AsQueryable();
            var dataList = new ListModel<Car>();

            query = query.Where(d => categoryNormalizedName == null
            || d.Category.NormalizedName.Equals(categoryNormalizedName));
            
            // количество элементов в списке
            var count = await query.CountAsync();
            if (count == 0)
            {
                return new ResponseData<ListModel<Car>>
                {
                    Data = dataList
                };
            }
            
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Car>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };
            dataList.Items = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            
            var response = new ResponseData<ListModel<Car>>
            {
                Data = dataList
            };
            return response;
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Car product)
        {
            throw new NotImplementedException();
        }
    }
}
