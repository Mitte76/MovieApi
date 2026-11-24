using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(x => x.Genre).HasMaxLength(50);

            modelBuilder.Entity<Movie>().Property(x => x.Title).HasMaxLength(200);

            modelBuilder.Entity<MovieDetails>().Property(x => x.Language).HasMaxLength(50);

            modelBuilder.Entity<MovieDetails>().Property(x => x.Synopsis).HasMaxLength(2000);

            modelBuilder.Entity<Review>().Property(x => x.ReviewerName).HasMaxLength(100);

            modelBuilder
                .Entity<Review>()
                .ToTable(t =>
                    t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5")
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
