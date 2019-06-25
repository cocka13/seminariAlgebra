using System.Data.Entity;

namespace Aplikacija.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("dbBaza")
        {

        }

        public DbSet<Seminar> Seminar { get; set; }
        public DbSet<Predbiljezba> Predbiljezba { get; set; }
        public DbSet<Zaposlenik> Zaposlenik { get; set; }
        public DbSet<Greske> Greske { get; set; }
    }
}