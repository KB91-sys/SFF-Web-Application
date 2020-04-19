using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API_Web_application.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Filmstudio> Filmstudios { get; set; }

        public DbSet<Loan> FilmstudioLoans { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Label> PostLabels {get;set;}

    }

}
