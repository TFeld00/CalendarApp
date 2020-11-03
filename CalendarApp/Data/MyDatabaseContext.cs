using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Models
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext (DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<DotNetCoreSqlDb.Models.Resource> Resources { get; set; }
        public DbSet<DotNetCoreSqlDb.Models.Task> Tasks { get; set; }
    }
}
