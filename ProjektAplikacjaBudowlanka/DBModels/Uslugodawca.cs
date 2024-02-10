
namespace ProjektAplikacjaBudowlanka.DBModels;
    public class Uslugodawca
    {
        public int idUslugodawca { get; set; }
        public string Nazwa { get; set; }
        public string NIP { get; set; }

        public virtual ICollection<Oferta> NavigationOferty { get; set; }
        public virtual ICollection<Zdjecie> NavigationZdjecia { get; set; }
        public virtual ICollection<Opinia> NavigationOpinie { get; set; }
        public virtual ICollection<Uslugodawca_Pracownik> NavigationUslugodawca_Pracownicy { get; set; }
        public virtual ICollection<User> NavigationUsers { get; set; } // zakładamy, że jest tylko 1
    }
