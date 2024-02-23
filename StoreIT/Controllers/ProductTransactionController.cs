using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreIT.Data;

namespace StoreIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOutProduct (int productId , int UserId)
        {
            var transaction = _context.ProductTransactions.ToList();

        }
    }
}
