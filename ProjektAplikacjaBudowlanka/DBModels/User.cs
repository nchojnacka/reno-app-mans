

namespace ProjektAplikacjaBudowlanka.DBModels;

public class User
{
    public int idUser { get; set; }
    public string Login { get; set; }
    public string Haslo { get; set; }
    public int? idUslugodawca { get; set; }
    public int? idUslugobiorca { get; set; }

    public virtual Uslugobiorca? NavigationUslugobiorca { get; set; }
    public virtual Uslugodawca? NavigationUslugodawca { get; set; }

}
