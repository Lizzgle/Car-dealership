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
    public class CarCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ICarCategoryService _categoryService;

        public CarCategoriesController(ICarCategoryService categoryService)
        {
            _categoryService = categoryService;

        }

        // GET: api/CarCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarCategory>>> GetCarCategories()
        {
          if (_categoryService == null)
          {
              return NotFound();
          }
            var responce = await _categoryService.GetCategoryListAsync();
            return Ok(responce);
        }

    //    // GET: api/CarCategories/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<CarCategory>> GetCarCategory(int id)
    //    {
    //      if (_context.CarCategories == null)
    //      {
    //          return NotFound();
    //      }
    //        var carCategory = await _context.CarCategories.FindAsync(id);

    //        if (carCategory == null)
    //        {
    //            return NotFound();
    //        }

    //        return carCategory;
    //    }

    //    // PUT: api/CarCategories/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutCarCategory(int id, CarCategory carCategory)
    //    {
    //        if (id != carCategory.Id)
    //        {
    //            return BadRequest();
    //        }

    //        _context.Entry(carCategory).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!CarCategoryExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/CarCategories
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<CarCategory>> PostCarCategory(CarCategory carCategory)
    //    {
    //      if (_context.CarCategories == null)
    //      {
    //          return Problem("Entity set 'AppDbContext.CarCategories'  is null.");
    //      }
    //        _context.CarCategories.Add(carCategory);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetCarCategory", new { id = carCategory.Id }, carCategory);
    //    }

    //    // DELETE: api/CarCategories/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteCarCategory(int id)
    //    {
    //        if (_context.CarCategories == null)
    //        {
    //            return NotFound();
    //        }
    //        var carCategory = await _context.CarCategories.FindAsync(id);
    //        if (carCategory == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.CarCategories.Remove(carCategory);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool CarCategoryExists(int id)
    //    {
    //        return (_context.CarCategories?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    }
}
