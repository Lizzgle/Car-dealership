using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop.API.Data;
using CarShop.Domain.Entities;
using CarShop.API.Services;

namespace CarShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/Cars
        [HttpGet]
        [Route("")]
        [Route("{category}/pageno={pageno:int}/pagesize={pagesize:int}")]
        [Route("{category}/pageno={pageno:int}")]
        [Route("{category}/pagesize={pagesize:int}")]
        [Route("pageno={pageno:int}/pagesize={pagesize:int}")]
        [Route("pageno={pageno:int}")]
        [Route("{category}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string? category,
                                                                  int pageNo = 1,
                                                                  int pageSize = 3)
        {
            var response = await _carService.GetProductListAsync(category, pageNo, pageSize);
            return Ok(response);
        }

        // GET: api/Cars/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var response = await _carService.GetProductByIdAsync(id);

            if (!response.Success)
                return NotFound();

            var car = response.Data;
            if (car == null)
                return NotFound();

            return Ok(response);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            try
            {
                await _carService.UpdateProductAsync(id, car);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            await _carService.CreateProductAsync(car);

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteProductAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool CarExists(int id)
        {
            var response = _carService.GetProductByIdAsync(id).Result;
            if (!response.Success || response.Data == null)
                return false;

            return true;
        }
    }
}
