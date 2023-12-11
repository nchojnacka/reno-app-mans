namespace ProjektAplikacjaBudowlanka.DBModels;

public class Opinia
{
    public int idOpinia { get; set; }
    public string Opis { get; set; }
    public int Ocena { get; set; }
    public int idUslugodawca { get; set; }
    public int idUslugobiorca { get; set; }

    public virtual Uslugodawca NavigationUslugodawca { get; set; }
    public virtual Uslugobiorca NavigationUslugobiorca { get; set; }
}
