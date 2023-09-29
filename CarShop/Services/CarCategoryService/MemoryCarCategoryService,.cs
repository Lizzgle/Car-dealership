using CarShop.Domain.Entities;
using CarShop.Domain.Models;

namespace CarShop.Services.CarCategoryService
{
    public class MemoryCarCategoryService : ICarCategoryService
    {
        public Task<ResponseData<List<CarCategory>>>
        GetCategoryListAsync()
        {
            var categories = new List<CarCategory>
            {
                new CarCategory {Id=1, Name="Tesla",
                    NormalizedName="tesla"},
                new CarCategory {Id=2, Name="BMW",
                    NormalizedName="bmw"},
                new CarCategory {Id=3, Name="Porshe",
                    NormalizedName="porshe"},
                new CarCategory {Id=4, Name="Lamborghini",
                    NormalizedName="lamborghini"},
            };
            var result = new ResponseData<List<CarCategory>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
