using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.Models.AuthenticationModels;
using ProjektAplikacjaBudowlanka.Models.DTO;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;
        public UserController(BudowlankaDBContext budowlankaContext)
        {
            _budowlankaContext = budowlankaContext;
        }

        [Authorize] //(Roles = "Klient")
        [HttpGet("/edit")]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit()
        {
            TempData["SuccessMessage"] = null;
            var id = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(us => us.idUser == id).FirstOrDefaultAsync();
            UserSelectDTO usDTO = new UserSelectDTO();
            usDTO.Login = u.Login;
            usDTO.Password = u.Haslo;
            if(User.FindFirstValue(ClaimTypes.Role) == "Firma")
            {
                //usDTO.Role = "Firma";
                var firma = await _budowlankaContext.Uslugodawca.Where(us => us.idUslugodawca == u.idUslugodawca).FirstOrDefaultAsync();
                usDTO.Nazwa = firma.Nazwa;
                usDTO.NIP = firma.NIP;
                usDTO.idFirma = firma.idUslugodawca;
            }
            else
            {
                //usDTO.Role = "Klient";
                var klient = await _budowlankaContext.Uslugobiorca.Where(us => us.idUslugobiorca == u.idUslugobiorca).FirstOrDefaultAsync();
                usDTO.Imie = klient.Imie;
                usDTO.Nazwisko = klient.Nazwisko;
                usDTO.idKlient = klient.idUslugobiorca;
            }
            return View(usDTO);
        }
        [Authorize]
        [HttpPost("/edit")]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit([FromForm] UserSelectDTO user)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
                var u = await _budowlankaContext.User.Where(u=>u.idUser==id).FirstOrDefaultAsync();
                if (user.Password is null)
                    ViewData["HasloSpan"] = "Hasło nie może być puste";
                if (user.Login is null)
                    ViewData["LoginSpan"] = "Login nie może być pusty";
                if (user.idFirma == null)
                {
                    if (user.Imie is null)
                        ViewData["ImieSpan"] = "Imie nie może być puste";
                    if (user.Nazwisko is null)
                        ViewData["NazwiskoSpan"] = "Nazwisko nie może być puste";
                    if (user.Nazwisko is null || user.Imie is null || user.Login is null || user.Password is null)
                        return View(user);
                }
                else
                {
                    if (user.Nazwa is null)
                        ViewData["NazwaFirmySpan"] = "Nazwa Firmy nie może być puste";
                    if (user.NIP is null)
                        ViewData["NIPSpan"] = "NIP nie może być puste";
                    if (user.Nazwa is null || user.NIP is null || user.Login is null || user.Password is null)
                        return View(user);
                }
                u.Login = user.Login;
                u.Haslo = user.Password;
                if(user.idFirma != null) //firma
                {
                    var firma = await _budowlankaContext.Uslugodawca.Where(us => us.idUslugodawca == u.idUslugodawca).FirstOrDefaultAsync();
                    firma.NIP = user.NIP;
                    firma.Nazwa = user.Nazwa;
                }
                else
                {
                    var klient = await _budowlankaContext.Uslugobiorca.Where(us => us.idUslugobiorca == u.idUslugobiorca).FirstOrDefaultAsync();
                    klient.Imie = user.Imie;
                    klient.Nazwisko = user.Nazwisko;
                    
                }
               
                int ok = await _budowlankaContext.SaveChangesAsync();
                if(ok>0)
                    TempData["SuccessMessage"] = "Zapisano zmiany pomyślnie.";
                else
                    TempData["SuccessMessage"] = "Coś poszło nie tak";
                return View();
            }
            return View();
        }
    }
}
