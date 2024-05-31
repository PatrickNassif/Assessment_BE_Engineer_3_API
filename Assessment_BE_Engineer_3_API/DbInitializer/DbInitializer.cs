using Assessment_BE_Engineer_3_API.Data;
using Assessment_BE_Engineer_3_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assessment_BE_Engineer_3_API.IDbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {
            _db.Database.EnsureCreated();
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                //_db.Database.Migrate();                
            }          

        }
    }
}
