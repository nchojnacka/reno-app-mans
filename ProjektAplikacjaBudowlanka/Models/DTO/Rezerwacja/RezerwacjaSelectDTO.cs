namespace ProjektAplikacjaBudowlanka.Models.DTO.Rezerwacja;
public class RezerwacjaSelectDTO
{
    public int IdOferta { get; set; }
    public int IdRezerwacja { get; set; }
    public string Ekipa { get; set; }
    public string Nazwa { get; set; }
    public string Opis { get; set; }
    public string Status { get; set; }
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
    public decimal Koszt { get; set; }
    public bool CzyZaplacone { get; set; }
}

