using Microsoft.AspNetCore.Mvc;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using ProjektAplikacjaBudowlanka.DBModels;
    using ProjektAplikacjaBudowlanka.Models.AuthenticationModels;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;
        private readonly IConfiguration _configuration;

        public AuthController(BudowlankaDBContext budowlankaDBContext,IConfiguration configuration)
        {
            _budowlankaContext=budowlankaDBContext;
            _configuration=configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        private string GenerateJwtToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        [HttpGet("/login")]
        [ActionName("Login")]
        public IActionResult Login()
        {
            ViewData["Title"] = "Logowanie";
            return View();
        }
        [HttpPost("/login")]
        [ActionName("Login")]
        public async Task<IActionResult> Login([FromForm]LoginModel model)
        {
            if(model.Password is null)
                ViewData["PasswordSpan"] = "Hasło nie może być puste";
            if (model.Login is null)
                ViewData["LoginSpan"] = "Login nie może być pusty";
            if(model.Password is null || model.Login is null)
                return View(model);
            if (ModelState.IsValid)
            {
                var l = await _budowlankaContext.User.ToListAsync();
                var user = l.Where(u => u.Login == model.Login && u.Haslo == model.Password).FirstOrDefault();
                if (user is not null)
                {
                    string role = user.idUslugobiorca == null ? "Firma" : "Klient";
                    var jwtToken = GenerateJwtToken();
                    var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Role, role),new Claim(ClaimTypes.Sid,user.idUser.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    Response.Cookies.Append("jwt", jwtToken);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
                return View(model);
                
            }
            ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
            return View(model);
        }
        [HttpPost("/logout")]
        [ActionName("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/register")]
        [ActionName("Register")]
        public IActionResult Register()
        {
            ViewData["Title"] = "Rejestracja";
            return View();
        }
        [HttpPost("/register")]
        [ActionName("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            if(model.Rodzaj is null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
                return View(model);
            }
            if (model.Haslo is null)
                ViewData["HasloSpan"] = "Hasło nie może być puste";
            if (model.Login is null)
                ViewData["LoginSpan"] = "Login nie może być pusty";
            if(model.Rodzaj =="Klient")
            {
                if(model.Imie is null)
                    ViewData["ImieSpan"] = "Imie nie może być puste";
                if (model.Nazwisko is null)
                    ViewData["NazwiskoSpan"] = "Nazwisko nie może być puste";
                if (model.Nazwisko is null || model.Imie is null || model.Login is null || model.Haslo is null)
                    return View(model);
            }
            else
            {
                if (model.NazwaFirmy is null)
                    ViewData["NazwaFirmySpan"] = "Nazwa Firmy nie może być puste";
                if (model.NIP is null)
                    ViewData["NIPSpan"] = "NIP nie może być puste";
                if (model.NIP.StartsWith("-"))
                    ViewData["NIPSpan"] = "NIP nie może być ujemny";
                if (model.NazwaFirmy is null || model.NIP is null || model.Login is null || model.Haslo is null || model.NIP.StartsWith("-"))
                    return View(model);
            }
            
            User u = new User();
            var idU = _budowlankaContext.User.Count() == 0 ? 1 : _budowlankaContext.User.Max(us => us.idUser) + 1;
            u.idUser= idU;
            u.Login = model.Login;
            u.Haslo = model.Haslo;

            if(model.Rodzaj == "Klient")
            {
                var idK = _budowlankaContext.Uslugobiorca.Count() == 0?1: _budowlankaContext.Uslugobiorca.Max(us => us.idUslugobiorca) + 1;
                Uslugobiorca klient = new Uslugobiorca();
                klient.idUslugobiorca = idK;
                klient.Imie = model.Imie!;
                klient.Nazwisko = model.Nazwisko!;
                u.idUslugobiorca = idK;
                await _budowlankaContext.Uslugobiorca.AddAsync(klient);
            }
            else
            {

                var idF = _budowlankaContext.Uslugodawca.Count()==0?1:_budowlankaContext.Uslugodawca.Max(us => us.idUslugodawca) + 1;
                Uslugodawca firma = new Uslugodawca();
                firma.idUslugodawca = idF;
                firma.Nazwa = model.NazwaFirmy!;
                firma.NIP = model.NIP!;
                u.idUslugodawca = idF;
                await _budowlankaContext.Uslugodawca.AddAsync(firma);
            }
            await _budowlankaContext.User.AddAsync(u);
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Zapisano zmiany pomyślnie.";
            else
                TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Login", "Auth");
        }
    }
}
