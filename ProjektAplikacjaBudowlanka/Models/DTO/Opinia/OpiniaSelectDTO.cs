namespace ProjektAplikacjaBudowlanka.Models.DTO.Opinia
{
    public class OpiniaSelectDTO
    {
        public int idOpinia{ get; set; }
        public int idUslugodawca{ get; set; }
        public string Opis { get; set; }
        public int Ocena{ get; set; }
        public string Login{ get; set; }
    }
}
