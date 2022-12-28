using GoogleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GoogleApi.Data
{
    public class GoogleDbContext : DbContext
    {
        public GoogleDbContext(DbContextOptions<GoogleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GoogleEmail> GoogleEmails { get; set; }

    }
}
