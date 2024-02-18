using Microsoft.EntityFrameworkCore;
using StoreIT.Model;

namespace StoreIT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
