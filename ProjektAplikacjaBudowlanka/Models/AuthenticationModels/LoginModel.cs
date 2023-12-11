using System.ComponentModel.DataAnnotations;

namespace ProjektAplikacjaBudowlanka.Models.AuthenticationModels
{
    public class LoginModel
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
    }
}
