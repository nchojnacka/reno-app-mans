using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class Uslugodawca_PracownikEFConfiguration : IEntityTypeConfiguration<Uslugodawca_Pracownik>
    {
        public void Configure(EntityTypeBuilder<Uslugodawca_Pracownik> builder)
        {
            builder.HasKey(e => e.idUslugodawca_Pracownik);
            builder.Property<string>(e => e.Stanowisko);
            builder.HasOne(e => e.NavigationUslugodawca)
           .WithMany(e => e.NavigationUslugodawca_Pracownicy)
           .HasForeignKey(e => e.idUslugodawca);
            builder.HasOne(e => e.NavigationPracownik)
           .WithMany(e => e.NavigationUslugodawcaPracownik)
           .HasForeignKey(e => e.idPracownik);
        }
    }
}
