

namespace ProjektAplikacjaBudowlanka.DBModels;

public class Rezerwacja
{
    public int idRezerwacja { get; set; }
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
    public decimal Koszt { get; set; }
    public string Status { get; set; }
    public bool CzyZaplacone { get; set; }
    public int idUslugobiorca{ get; set; }
    public int idOferta{ get; set; }

    public virtual Uslugobiorca NavigationUslugobiorca { get; set; }
    public virtual Oferta NavigationOferta { get; set; }
}
