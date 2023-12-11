using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class ZdjecieEFConfiguration : IEntityTypeConfiguration<Zdjecie>
    {
        public void Configure(EntityTypeBuilder<Zdjecie> builder)
        {
            builder.HasKey(e => e.idZdjecie);
            builder.Property<string>(e => e.NazwaPliku);
            builder.HasOne(e => e.NavigationUslugodawca)
           .WithMany(e => e.NavigationZdjecia)
           .HasForeignKey(e => e.idUslugodawca);
        }
    }
}
