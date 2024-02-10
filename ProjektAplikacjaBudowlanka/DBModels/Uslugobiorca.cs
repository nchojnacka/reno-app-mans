namespace ProjektAplikacjaBudowlanka.DBModels
{
    public class Uslugobiorca
    {
        public int idUslugobiorca { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public virtual ICollection<Rezerwacja> NavigationRezerwacje { get; set; }
        public virtual ICollection<Opinia> NavigationOpinie { get; set; }
        public virtual ICollection<User> NavigationUsers { get; set; } //zakładamy, że jest tylko jeden jednak relacja jest 1 - *
    }
}
