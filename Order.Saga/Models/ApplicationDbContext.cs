using Microsoft.EntityFrameworkCore;

namespace Order.Saga.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Order> Orders { get; set; }
    }
}
