using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearHouse.Data;
using WearHouse.Models;

namespace WearHouse.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : Controller
    {
        private readonly WearHouseDbContext dbContext;

        public CategoriesController(WearHouseDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await dbContext.Categories.ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            var category = await dbContext.Categories.Where(c => c.CategoryID == id).SingleOrDefaultAsync();
            if (category == null) {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request) {
            var category = new Category()
            {
                UserID = request.UserID,
                CategoryName = request.CategoryName,
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return Created("", category);
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, UpdateCategoryRequest request)
        {
            var category = await dbContext.Categories.Where(c => c.CategoryID == id).FirstOrDefaultAsync();

            if (category != null)
            {
                category.CategoryName = request.CategoryName;
                await dbContext.SaveChangesAsync();

                return Ok(category);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var category = await dbContext.Categories.Where(c => c.CategoryID == id).FirstOrDefaultAsync();

            if (category != null)
            {
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync();

                return NoContent();
            }

            return NotFound();
        }
    }
}
