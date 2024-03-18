using Microsoft.EntityFrameworkCore;

namespace File.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FileModel> Files { get; set; }
    }
}
