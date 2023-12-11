using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class UslugobiorcaEFConfiguration : IEntityTypeConfiguration<Uslugobiorca>
    {
        public void Configure(EntityTypeBuilder<Uslugobiorca> builder)
        {
            builder.HasKey(e => e.idUslugobiorca);
            builder.Property<string>(e => e.Imie);
            builder.Property<string>(e => e.Nazwisko);
        }
    }
}
