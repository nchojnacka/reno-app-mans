using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class UslugodawcaEFConfiguration : IEntityTypeConfiguration<Uslugodawca>
    {
        public void Configure(EntityTypeBuilder<Uslugodawca> builder)
        {
            builder.HasKey(e => e.idUslugodawca);
            builder.Property<string>(e => e.Nazwa);
            builder.Property<string>(e => e.NIP);
        }
    }
}
