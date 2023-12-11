using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models;
using ProjektAplikacjaBudowlanka.Models.DTO.Oferta;
using ProjektAplikacjaBudowlanka.Models.DTO.Rezerwacja;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class RezerwacjaController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;
        private readonly ILogger<HomeController> _logger;

        public RezerwacjaController(BudowlankaDBContext budowlankaContext, ILogger<HomeController> logger)
        {
            _budowlankaContext = budowlankaContext;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            TempData["ErrorMessage"] = null;
            TempData["SuccessMessage"] = null;
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            List<RezerwacjaSelectDTO> list = new List<RezerwacjaSelectDTO>();
            if (User.FindFirstValue(ClaimTypes.Role) == "Firma")
            {
                foreach (var o in await _budowlankaContext.Oferta.Where(us => us.idUslugodawca == u.idUslugodawca).ToListAsync())
                {
                    foreach (var r in await _budowlankaContext.Rezerwacja.Where(r => r.idOferta == o.idOferta).ToListAsync())
                    {
                        var ekipa = await _budowlankaContext.Uslugodawca.Where(uss => uss.idUslugodawca == u.idUslugodawca).FirstOrDefaultAsync();
                        RezerwacjaSelectDTO rez = new RezerwacjaSelectDTO();
                        rez.IdOferta = o.idOferta;
                        rez.IdRezerwacja = r.idRezerwacja;
                        rez.Ekipa = ekipa.Nazwa;
                        rez.Nazwa = o.Nazwa;
                        rez.Opis = o.Opis;
                        rez.Status = r.Status;
                        rez.DataDo = r.DataDo;
                        rez.DataOd = r.DataOd;
                        rez.Koszt = r.Koszt;
                        rez.CzyZaplacone = r.CzyZaplacone;
                        list.Add(rez);
                    }
                }
            }
            else
            {
                foreach (var r in await _budowlankaContext.Rezerwacja.Where(us => us.idUslugobiorca == u.idUslugobiorca).ToListAsync())
                {
                    var o = await _budowlankaContext.Oferta.Where(o => o.idOferta == r.idOferta).FirstOrDefaultAsync();
                    var ekipa = await _budowlankaContext.Uslugodawca.Where(uss => uss.idUslugodawca == o.idUslugodawca).FirstOrDefaultAsync();
                    RezerwacjaSelectDTO rez = new RezerwacjaSelectDTO();
                    rez.IdOferta = o.idOferta;
                    rez.IdRezerwacja = r.idRezerwacja;
                    rez.Ekipa = ekipa.Nazwa;
                    rez.Nazwa = o.Nazwa;
                    rez.Opis = o.Opis;
                    rez.Status = r.Status;
                    rez.DataDo = r.DataDo;
                    rez.DataOd = r.DataOd;
                    rez.Koszt = r.Koszt;
                    rez.CzyZaplacone = r.CzyZaplacone;
                    list.Add(rez);
                }
            }
            return View(list);
        }

        [Authorize(Roles = "Firma")]
        [HttpGet("/startRezerwacja")]
        [ActionName("StartRezerwacja")]
        public async Task<IActionResult> StartRezerwacja(int id)
        {
            var oferta = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == id).FirstOrDefaultAsync();
            if (oferta == null)
            {
                TempData["ErrorMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Rezerwacja");
            }
            oferta.Status = "Rozpoczęto";
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Zaktualizowano status";
            else
                TempData["ErrorMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Rezerwacja");
        }

        [Authorize]
        [HttpGet("/cancelRezerwacja")]
        [ActionName("CancelRezerwacja")]
        public async Task<IActionResult> CancelRezerwacja(int id)
        {
            var oferta = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == id).FirstOrDefaultAsync();
            if (oferta == null)
            {
                TempData["ErrorMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Rezerwacja");
            }
            oferta.Status = "Anulowano";
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Zaktualizowano status";
            else
                TempData["ErrorMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Rezerwacja");
        }

        [Authorize(Roles = "Firma")]
        [HttpGet("/endRezerwacja")]
        [ActionName("EndRezerwacja")]
        public async Task<IActionResult> EndRezerwacja(int id)
        {
            var oferta = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == id).FirstOrDefaultAsync();
            if (oferta == null)
            {
                TempData["ErrorMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Rezerwacja");
            }
            oferta.Status = "Zakończono";
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Zaktualizowano status";
            else
                TempData["ErrorMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Rezerwacja");
        }

        [Authorize(Roles = "Firma")]
        [HttpGet("/deleteRezerwacja")]
        [ActionName("DeleteRezerwacja")]
        public async Task<IActionResult> DeleteRezerwacja(int id)
        {
            var oferta = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == id).FirstOrDefaultAsync();
            if (oferta == null)
            {
                TempData["ErrorMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Rezerwacja");
            }
            _budowlankaContext.Rezerwacja.Remove(oferta);
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Usunięto rezerwację";
            else
                TempData["ErrorMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Rezerwacja");
        }

        [Authorize(Roles = "Klient")]
        [HttpGet("/payRezerwacja")]
        [ActionName("PayRezerwacja")]
        public async Task<IActionResult> PayRezerwacja(int id)
        {
            var rezerwacja = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == id).FirstOrDefaultAsync();
            if (rezerwacja == null)
            {
                return RedirectToAction("Index", "Rezerwacja");
            }
            var oferta = await _budowlankaContext.Oferta.Where(o => o.idOferta == rezerwacja.idOferta).FirstOrDefaultAsync();
            if (oferta == null)
            {
                return RedirectToAction("Index", "Rezerwacja");
            }

            RezerwacjaPayDTO r = new RezerwacjaPayDTO();
            r.idRezerwacja = id;
            r.Nazwa = oferta.Nazwa;
            r.Koszt = rezerwacja.Koszt;
            r.Platnosc = "CreditCard";
            return View(r);
        }
        [Authorize(Roles = "Klient")]
        [HttpPost("/payRezerwacja")]
        [ActionName("PayRezerwacja")]
        public async Task<IActionResult> PayRezerwacja([FromForm] RezerwacjaPayDTO rezerwacja)
        {
            var r = await _budowlankaContext.Rezerwacja.Where(p => p.idRezerwacja == rezerwacja.idRezerwacja).FirstOrDefaultAsync();
            if (r == null)
            {
                return RedirectToAction("index", "rezerwacja");
            }
            r.CzyZaplacone = true;
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Usunięto rezerwację";
            else
                TempData["ErrorMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Rezerwacja");
        }

    }

}