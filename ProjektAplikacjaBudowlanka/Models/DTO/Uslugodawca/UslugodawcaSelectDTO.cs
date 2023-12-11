using ProjektAplikacjaBudowlanka.Models.DTO.Pracownik;
using ProjektAplikacjaBudowlanka.Models.DTO.Zdjecie;

namespace ProjektAplikacjaBudowlanka.Models.DTO.Uslugodawca;

public class UslugodawcaSelectDTO
{
    public int idUslugodawca {  get; set; }
    public string Nazwa { get; set; }
    public string NIP { get; set; }
    public List<PracownikSelectDTO> Pracownicy { get; set; }
    public List<ZdjecieSelectDTO> Zdjecia { get; set; }


}
