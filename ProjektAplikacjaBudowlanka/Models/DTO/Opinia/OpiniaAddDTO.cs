namespace ProjektAplikacjaBudowlanka.Models.DTO.Opinia
{
    public class OpiniaAddDTO
    {
        public int idUslugodawca { get; set; }
        public int idUslugobiorca { get; set; }
        public string Opis { get; set; }
        public int Ocena { get; set; }
        public string Login { get; set; }
    }
}
