using Chartix.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chartix.Infrastructure.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Metric> Metrics { get; set; }

        public DbSet<Value> Values { get; set; }

        public DbSet<ProcessedUpdate> Updates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metric>()
                        .HasOne(m => m.Source)
                        .WithMany(u => u.Metrics)
                        .HasForeignKey(p => p.SourceId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Source>()
                        .Metadata
                        .FindNavigation(nameof(Source.Metrics))
                        .SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Value>()
                        .HasOne(v => v.Metric)
                        .WithMany(m => m.Values)
                        .HasForeignKey(v => v.MetricId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Source>().HasIndex(p => p.ExternalId);

            modelBuilder.Entity<Source>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Metric>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Value>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ProcessedUpdate>().HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
