using Microsoft.EntityFrameworkCore;
using taskmanager_back_repo_qas.Models;

namespace taskmanager_back_repo_qas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {}

        public DbSet<User> Users { get; set; }
    }
}
