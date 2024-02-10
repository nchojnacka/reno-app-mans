using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class RezerwacjaEFConfiguration : IEntityTypeConfiguration<Rezerwacja>
    {
        public void Configure(EntityTypeBuilder<Rezerwacja> builder)
        {
            builder.HasKey(e => e.idRezerwacja);
            builder.Property<DateTime>(e => e.DataDo);
            builder.Property<DateTime>(e => e.DataOd);
            builder.Property<decimal>(e => e.Koszt);
            builder.Property<bool>(e => e.CzyZaplacone);
            builder.Property<string>(e => e.Status);
            builder.HasOne(e => e.NavigationUslugobiorca)
                .WithMany(e => e.NavigationRezerwacje)
                .HasForeignKey(e => e.idUslugobiorca);
            builder.HasOne(e => e.NavigationOferta)
           .WithMany(e => e.NavigationRezerwacje)
           .HasForeignKey(e => e.idOferta);
        }
    }
}
