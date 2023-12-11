using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjektAplikacjaBudowlanka.ConfigurationDB;
using ProjektAplikacjaBudowlanka.DBModels;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;

namespace ProjektAplikacjaBudowlanka
{
    public class BudowlankaDBContext : DbContext
    {
        public DbSet<Oferta> Oferta { get; set; }
        public DbSet<Opinia> Opinia { get; set; }
        public DbSet<Pracownik> Pracownik { get; set; }
        public DbSet<Rezerwacja> Rezerwacja { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Uslugobiorca> Uslugobiorca { get; set; }
        public DbSet<Uslugodawca> Uslugodawca { get; set; }
        public DbSet<Uslugodawca_Pracownik> Uslugodawca_Pracownik { get; set; }
        public DbSet<Zdjecie> Zdjecie { get; set; }

        public BudowlankaDBContext(DbContextOptions opt)
        : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OfertaEFConfiguration());
            modelBuilder.ApplyConfiguration(new OpiniaEFConfiguration());
            modelBuilder.ApplyConfiguration(new PracownikEFConfiguration());
            modelBuilder.ApplyConfiguration(new RezerwacjaEFConfiguration());
            modelBuilder.ApplyConfiguration(new UserEFConfiguration());
            modelBuilder.ApplyConfiguration(new UslugodawcaEFConfiguration());
            modelBuilder.ApplyConfiguration(new UslugobiorcaEFConfiguration());
            modelBuilder.ApplyConfiguration(new Uslugodawca_PracownikEFConfiguration());
            modelBuilder.ApplyConfiguration(new ZdjecieEFConfiguration());
        }
    }
}
