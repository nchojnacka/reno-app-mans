

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB;

public class OfertaEFConfiguration : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> builder)
    {
        builder.HasKey(e => e.idOferta);
        builder.Property<string>(e => e.Nazwa);
        builder.Property<string>(e => e.Opis);
        builder.Property<decimal>(e => e.Dniowka);
        builder.HasOne(e => e.NavigationUslugodawca)
            .WithMany(e => e.NavigationOferty)
            .HasForeignKey(e => e.idUslugodawca);
    }
}