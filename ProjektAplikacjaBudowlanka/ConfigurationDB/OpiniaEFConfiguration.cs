using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class OpiniaEFConfiguration : IEntityTypeConfiguration<Opinia>
    {
        public void Configure(EntityTypeBuilder<Opinia> builder)
        {
            builder.HasKey(e => e.idOpinia);
            builder.Property<string>(e => e.Opis);
            builder.Property<int>(e => e.Ocena);
            builder.HasOne(e => e.NavigationUslugodawca)
                .WithMany(e => e.NavigationOpinie)
                .HasForeignKey(e => e.idUslugodawca);
            builder.HasOne(e => e.NavigationUslugobiorca)
              .WithMany(e => e.NavigationOpinie)
              .HasForeignKey(e => e.idUslugobiorca);
        }
    }
}
