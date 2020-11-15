using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Lab03.Data
{
    public class ApplicationDBContext: DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }
        
        public DbSet<string> Files { get; set; }
    }
}