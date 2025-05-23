using Microsoft.EntityFrameworkCore;
using EnterpriseAPI.Models;

namespace EnterpriseAPI
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
