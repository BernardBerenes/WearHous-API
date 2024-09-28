using Microsoft.EntityFrameworkCore;
using WearHouse.Models;

namespace WearHouse.Data
{
    public class WearHouseDbContext: DbContext
    {
        public WearHouseDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
