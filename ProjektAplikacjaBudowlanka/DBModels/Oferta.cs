
namespace ProjektAplikacjaBudowlanka.DBModels;

public class Oferta
{
    public int idOferta { get; set; }
    public string Nazwa{ get; set; }
    public string Opis{ get; set; }
    public decimal Dniowka{ get; set; }
    public int idUslugodawca{ get; set; }

    public virtual Uslugodawca NavigationUslugodawca { get; set; }
    public virtual ICollection<Rezerwacja> NavigationRezerwacje  { get; set; }
}
