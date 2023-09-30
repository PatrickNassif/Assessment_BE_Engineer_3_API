using Assessment_BE_Engineer_3_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment_BE_Engineer_3_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<FileModel> Files { get; set; }        
    }
}
