using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
    }
}
