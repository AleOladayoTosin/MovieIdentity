using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieClient.Models;

namespace MovieClient.Data
{
    public class MovieClientContext : DbContext
    {
        public MovieClientContext (DbContextOptions<MovieClientContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
    }
}
