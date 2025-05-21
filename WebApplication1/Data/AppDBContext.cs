using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> option) : base(option)
        {

        }

        public DbSet<Creator> Creators { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}
