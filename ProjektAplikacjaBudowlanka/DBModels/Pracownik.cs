namespace ProjektAplikacjaBudowlanka.DBModels;

public class Pracownik
{
    public int idPracownik { get; set; }
    public string Imie { get; set; }
    public string Nazwisko{ get; set; }

    public virtual ICollection<Uslugodawca_Pracownik> NavigationUslugodawcaPracownik { get; set; }
}
