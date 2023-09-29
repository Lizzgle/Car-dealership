using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using CarShop.Services.CarCategoryService;
using System.Collections.Generic;

namespace CarShop.Services.CarService
{
    public class MemoryCarService : ICarService
    {
        private List<Car> _cars;
        private readonly List<CarCategory> _categories;
        // private readonly IConfiguration _configuration;
        public MemoryCarService(ICarCategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
                .Result
                .Data;
            SetupData();
        }
        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _cars = new List<Car>
{
            new Car {Id = 1, Name="Tesla Model 3",
                Price =30000, Image="Images/tesla_model_3.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Id = 2, Name="Tesla Model Y",
                Price =39000, Image="Images/tesla_model_y.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Id = 3, Name="Tesla Model S",
                Price =71000, Image="Images/tesla_model_s.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Id = 4, Name="Tesla Model X",
                Price =69000, Image="Images/tesla_model_x.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("tesla"))},

            new Car {Id = 5, Name="BMW i4",
                Price =59000, Image="Images/bmw_i4.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Id = 6, Name="BMW M4 Coupe",
                Price =136000, Image="Images/bmw_m4_coupe.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Id = 7, Name="BMW X6 M",
                Price =229000, Image="Images/bmw_x6_m.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Id = 8, Name="BMW Vision M NEXT",
                Price =444000, Image="Images/bmw_vision_m_next.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("bmw"))},

            new Car {Id = 9, Name="Porshe 718 Cayman",
                Price =66000, Image="Images/porshe_718_cayman.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Id = 10, Name="Porshe 718 SPYDER",
                Price =110000, Image="Images/porshe_718_spyder.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Id = 11, Name="Porshe 911 GT3 RS",
                Price =260000, Image="Images/porshe_911_gt3_rs.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Id = 12, Name="Porshe Taycan",
                Price = 84000, Image="Images/porshe_taycan.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("porshe"))},

            new Car {Id = 13, Name="Lamborghini HURACAN EVO SPYDER",
                Price =220000, Image="Images/lamborghini_huracan_evo_spyder.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Id = 14, Name="Lamborghini HURACAN TECNICA",
                Price =249000, Image="Images/lamborghini_huracan_tecnica.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Id = 15, Name="Lamborghini REVUELTO",
                Price =500000, Image="Images/lamborghini_revuelto.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Id = 16, Name="Lamborghini URUS S",
                Price =233000, Image="Images/lamborghini_urus_s.png",
                Category = _categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
        };
        }

       
        public Task<ResponseData<Car>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Car product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Car>> CreateProductAsync(Car product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Car>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            ResponseData<ListModel<Car>> response = new() { Data = new() };


            try
            {
                if (categoryNormalizedName is null)
                {
                    response.Data.Items = _cars;
                }
                else
                {
                    response.Data.Items = _cars.Where(
                        f => f.Category?.NormalizedName.Equals(categoryNormalizedName) ?? false)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }


            return Task.FromResult(response);
        }

        //Task<ResponseData<ListModel<Car>>> ICarService.GetProductListAsync(string? categoryNormalizedName, int pageNo)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
