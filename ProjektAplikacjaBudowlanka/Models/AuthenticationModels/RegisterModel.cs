using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjektAplikacjaBudowlanka.Models.AuthenticationModels
{
    public class RegisterModel
    {
        public string? Login { get; set; }
        public string? Haslo { get; set; }
        public string? Rodzaj { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? NazwaFirmy { get; set; }
        public string? NIP { get; set; }
    }
    public enum UserType
    {
        Klient,Firma
    }

}
