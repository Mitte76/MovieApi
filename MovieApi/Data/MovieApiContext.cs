using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data
{
    public class MovieApiContext(DbContextOptions<MovieApiContext> options) : DbContext(options)
    {
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Actor> Actors { get; set; } = default!;
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().Property(x => x.GenreName).HasMaxLength(50);

            modelBuilder.Entity<Movie>().Property(x => x.Title).HasMaxLength(200);

            modelBuilder.Entity<MovieDetails>().Property(x => x.Language).HasMaxLength(50);

            modelBuilder.Entity<MovieDetails>().Property(x => x.Synopsis).HasMaxLength(2000);

            modelBuilder.Entity<Review>().Property(x => x.ReviewerName).HasMaxLength(100);

            modelBuilder
                .Entity<Review>()
                .ToTable(t =>
                    t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5")
                );

            modelBuilder
                .Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity("MovieActor");

            base.OnModelCreating(modelBuilder);
        }
    }
}
