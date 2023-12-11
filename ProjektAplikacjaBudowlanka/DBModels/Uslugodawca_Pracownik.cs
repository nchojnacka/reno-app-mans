namespace ProjektAplikacjaBudowlanka.DBModels
{
    public class Uslugodawca_Pracownik
    {
        public int idUslugodawca_Pracownik {  get; set; }
        public string Stanowisko { get; set; }
        public int idPracownik { get; set; }
        public int idUslugodawca { get; set; }
        public virtual Pracownik NavigationPracownik { get; set; }
        public virtual Uslugodawca NavigationUslugodawca { get; set; }
    }
}
