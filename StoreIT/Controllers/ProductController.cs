using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreIT.ApiModel;
using StoreIT.Data;
using StoreIT.Model;

namespace StoreIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]ProductCreate createProduct)
        {
            var product = new Product()
            {
                Title = createProduct.Title,
                Brand = createProduct.Brand,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok("Product has been created Successfully");
        }
        [HttpGet("{id}")]
        public async Task <ActionResult<ProductGetDto>> GetProduct([FromRoute] int id)
        {
            var product = await _context.Products
                .Select( displayproduct => new ProductGetDto
                {
                    Id = displayproduct.Id,
                    Title = displayproduct.Title,
                    Brand = displayproduct.Brand,
                    CreatedAt = displayproduct.CreatedAt,
                    UpdatedAt = displayproduct.UpdatedAt
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(product); 
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGetDto>>> GetProducts ()
        {
            var products = await _context.Products
                .Select(displayproduct=> new ProductGetDto
                {
                    Id = displayproduct.Id,
                    Brand = displayproduct.Brand,
                    Title = displayproduct.Title,
                    CreatedAt = displayproduct.CreatedAt,
                    UpdatedAt = displayproduct.UpdatedAt,
                })
                .ToListAsync();

            if (products.Count <1)
            {
                return BadRequest();
            }

            return Ok(products);    

            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct ([FromRoute]int id , [FromBody]ProductCreate updateProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return BadRequest();
            }

            product.Brand = updateProduct.Brand;
            product.Title = updateProduct.Title;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok("Product Successfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct ([FromRoute]int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return BadRequest();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product has been deleted");
        }
    }
}
