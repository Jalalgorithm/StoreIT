using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreIT.Data;

namespace StoreIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
