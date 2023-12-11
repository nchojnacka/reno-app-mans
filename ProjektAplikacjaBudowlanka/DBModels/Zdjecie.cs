

namespace ProjektAplikacjaBudowlanka.DBModels;

public class Zdjecie
{
    public int idZdjecie { get; set; }
    public string NazwaPliku { get; set; }
    public int idUslugodawca { get; set; }

    public virtual Uslugodawca NavigationUslugodawca { get; set; }
}
