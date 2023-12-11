using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektAplikacjaBudowlanka.DBModels;

namespace ProjektAplikacjaBudowlanka.ConfigurationDB
{
    public class UserEFConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.idUser);
            builder.Property<string>(e => e.Login);
            builder.Property<string>(e => e.Haslo);
            builder.HasOne(e => e.NavigationUslugobiorca)
                .WithMany(e => e.NavigationUsers)
                .HasForeignKey(e => e.idUslugobiorca);
            builder.HasOne(e => e.NavigationUslugodawca)
           .WithMany(e => e.NavigationUsers)
           .HasForeignKey(e => e.idUslugodawca);
        }
    }
}
