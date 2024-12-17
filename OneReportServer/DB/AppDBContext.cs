using Microsoft.EntityFrameworkCore;
using OneReportServer.DB.Model;

namespace OneReportServer.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<UserDetailsEntity> UserDetails { get; set; }
        public DbSet<ReportDetailsEntity> ReportDetails { get; set; }


    }
}