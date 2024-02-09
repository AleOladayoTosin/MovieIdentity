using Microsoft.EntityFrameworkCore;
using Movie.API.Configuration;

namespace Movie.API.Data
{
    public class MovieAPIContext : DbContext
    {
        public MovieAPIContext(DbContextOptions<MovieAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Movie.API.Models.Movie> Movie { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new MovieConfiguration());
        }
    }
}
