namespace ProjektAplikacjaBudowlanka.Models.DTO.Rezerwacja;
public class RezerwacjaAddDTO
{
    public int IdOferta { get; set; }
    public string Nazwa { get; set; }
    public string Opis { get; set; }
    public decimal Dniowka { get; set; }
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
    public decimal Koszt { get; set; }
}
