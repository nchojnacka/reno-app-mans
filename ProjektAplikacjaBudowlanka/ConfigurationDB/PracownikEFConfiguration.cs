using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class PracownikEFConfiguration : IEntityTypeConfiguration<Pracownik>
    {
        public void Configure(EntityTypeBuilder<Pracownik> builder)
        {
            builder.HasKey(e => e.idPracownik);
            builder.Property<string>(e => e.Imie);
            builder.Property<string>(e => e.Nazwisko);
        }
    }
}
