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
        private string _imagesPath;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public CarService(AppDbContext dbContext,  
            IWebHostEnvironment env, 
            IHttpContextAccessor httpContextAccessor) 
        {
            _dbContext = dbContext;
            _car = dbContext.Cars;
            _imagesPath = Path.Combine(env?.WebRootPath ?? "", "Images");
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseData<Car>> CreateProductAsync(Car product)
        {
            await _car.AddAsync(product);
            _dbContext.SaveChanges();

            return new ResponseData<Car>() { Data = product };
        }

        public async Task DeleteProductAsync(int id)
        {
            var elem = await _car.FirstOrDefaultAsync(x => x.Id == id);

            if (elem is not null)
            {
                _car.Remove(elem);
                _dbContext.SaveChanges();
            }
        }

        public async Task<ResponseData<Car>> GetProductByIdAsync(int id)
        {
            var query = _car.AsQueryable().Include(f => f.Category);
            var elem = await query.FirstOrDefaultAsync(x => x.Id == id);
            ResponseData<Car> response = new ResponseData<Car>();

            if (elem is not null)
            {
                response.Data = elem;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Элемента с таким id не существет";
            }

            return response;
        }

        public async Task<ResponseData<ListModel<Car>>> GetProductListAsync(string? categoryNormalizedName,
                                                                                int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _car.Include(f => f.Category).AsQueryable();
            var dataList = new ListModel<Car>();

            query = query.Where(d => categoryNormalizedName == null
            || d.Category!.NormalizedName.Equals(categoryNormalizedName));
            
            // количество элементов в списке
            var count = query.Count();
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

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var car = await _car.FindAsync(id);
            if (car == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!String.IsNullOrEmpty(car.Image))
                {
                    var prevImage = Path.GetFileName(car.Image);
                    File.Delete(Path.Combine(_imagesPath, prevImage));
                }
                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                // Сохранить файл
                using (var fs = new FileStream(Path.Combine(_imagesPath, fName), FileMode.Create))
                {
                    await formFile.CopyToAsync(fs);
                }


                // Указать имя файла в объекте
                car.Image = $"{host}/Images/{fName}";
                await _dbContext.SaveChangesAsync();
            }
            responseData.Data = car.Image;
            return responseData;
        }

        public async Task UpdateProductAsync(int id, Car product)
        {
            var car = await _car.FirstOrDefaultAsync(f => f.Id == id);

            if (car is null)
                return;
            car.Price = product.Price;
            car.Name = product.Name;
            if (!String.IsNullOrEmpty(product.Image))
                car.Image = product.Image;
            if (product.CategoryId != null)
                car.CategoryId = product.CategoryId;
            //_furnitures.Entry(furniture).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
