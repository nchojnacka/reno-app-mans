using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models;
using ProjektAplikacjaBudowlanka.Models.DTO.Oferta;
using ProjektAplikacjaBudowlanka.Models.DTO.Pracownik;
using ProjektAplikacjaBudowlanka.Models.DTO.Rezerwacja;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class OfertaController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;

        public OfertaController(BudowlankaDBContext budowlankaContext, ILogger<HomeController> logger)
        {
            _budowlankaContext = budowlankaContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            TempData["SuccessMessage"] = null;
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            List<OfertaSelectDTO> list = new List<OfertaSelectDTO>();

                foreach (var o in await _budowlankaContext.Oferta.ToListAsync())
                {
                    var ilosc = _budowlankaContext.Rezerwacja.Where(r => r.idOferta == o.idOferta).Count();
                    var ekipa = await _budowlankaContext.Uslugodawca.Where(u => u.idUslugodawca == o.idUslugodawca).FirstOrDefaultAsync();
                    list.Add(new OfertaSelectDTO()
                    {
                        Id = o.idOferta,
                        Nazwa = o.Nazwa,
                        Ekipa = ekipa.Nazwa,
                        Opis = o.Opis,
                        Dniowka = o.Dniowka,
                        IloscRezerwacji = ilosc
                    });
                }
            return View(list);
        }
        [Authorize(Roles = "Firma")]
        [HttpGet("/updateOferta")]
        [ActionName("UpdateOferta")]
        public async Task<IActionResult> UpdateOferta(int id)
        {
            TempData["SuccessMessage"] = null;
            var o = await _budowlankaContext.Oferta.Where(of => of.idOferta == id).FirstOrDefaultAsync();
            OfertaUpdateDTO oferta = new OfertaUpdateDTO();
            oferta.Opis = o.Opis;
            oferta.Nazwa = o.Nazwa;
            oferta.Dniowka = o.Dniowka;
            oferta.Id = o.idOferta;
            return View(oferta);
        }
        [Authorize(Roles = "Firma")]
        [HttpPost("/updateOferta")]
        [ActionName("UpdateOferta")]
        public async Task<IActionResult> UpdateOferta([FromForm] OfertaUpdateDTO oferta)
        {
            var o = await _budowlankaContext.Oferta.Where(o => o.idOferta == oferta.Id).FirstOrDefaultAsync();
            if (o == null)
            {
                TempData["SuccessMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Oferta");
            }
            if (oferta.Dniowka <= 0)
            {
                ViewData["DniowkaSpan"] = "Dniowka nie może równa lub mniejsza od zera";
                return View(oferta);
            }
            o.Opis = oferta.Opis;
            o.Nazwa = oferta.Nazwa;
            o.Dniowka = oferta.Dniowka;
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Zaktualizowan ofertę";
            else
                TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Oferta");
        }

        [Authorize(Roles = "Firma")]
        [HttpGet("/deleteOferta")]
        [ActionName("DeleteOferta")]
        public async Task<IActionResult> DeleteOferta(int id)
        {
            var oferta = await _budowlankaContext.Oferta.Where(p => p.idOferta == id).FirstOrDefaultAsync();
            if (oferta == null)
            {
                TempData["SuccessMessage"] = "Coś poszło nie tak";
                return RedirectToAction("Index", "Oferta");
            }
            _budowlankaContext.Oferta.Remove(oferta);
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Usunięto ofertę";
            else
                TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Oferta");
        }

        [Authorize(Roles = "Firma")]
        [HttpGet("/addOferta")]
        [ActionName("AddOferta")]
        public async Task<IActionResult> AddOferta()
        {
            TempData["SuccessMessage"] = null;
            return View();
        }

        [Authorize(Roles = "Firma")]
        [HttpPost("/addOferta")]
        [ActionName("AddOferta")]
        public async Task<IActionResult> AddOferta([FromForm] OfertaAddDTO oferta)
        {
            if (oferta.Nazwa == null)
                ViewData["NazwaSpan"] = "Nazwa nie może być pusta";
            if (oferta.Opis == null)
                ViewData["OpisSpan"] = "Opis nie może być pusty";
            if (oferta.Dniowka <=0)
                ViewData["DniowkaSpan"] = "Dniowka nie może równa lub mniejsza od zera";
            if (oferta.Nazwa == null || oferta.Opis == null || oferta.Dniowka <= 0)
            {
                return View(oferta);
            }
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            var idO = _budowlankaContext.Oferta.Count() == 0 ? 1 : _budowlankaContext.Oferta.Max(o => o.idOferta) + 1;
            Oferta o = new Oferta();
            o.idOferta = idO;
            o.Nazwa = oferta.Nazwa;
            o.Opis = oferta.Opis;
            o.Dniowka = oferta.Dniowka;
            o.idUslugodawca = (int)u.idUslugodawca!;
            await _budowlankaContext.Oferta.AddAsync(o);
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Dodano ofertę.";
            else
                TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Oferta");
        }

        [Authorize(Roles = "Klient")]
        [HttpGet("/bookOferta")]
        [ActionName("BookOferta")]
        public async Task<IActionResult> BookOferta(int id)
        {
            TempData["SuccessMessage"] = null;
            var o = await _budowlankaContext.Oferta.Where(of => of.idOferta == id).FirstOrDefaultAsync();
            RezerwacjaAddDTO oferta = new RezerwacjaAddDTO();
            oferta.Opis = o.Opis;
            oferta.Nazwa = o.Nazwa;
            oferta.Dniowka = o.Dniowka;
            oferta.IdOferta = o.idOferta;
            oferta.DataOd = DateTime.Now;
            oferta.DataDo = DateTime.Now.AddDays(1);
            oferta.Koszt = o.Dniowka;
            return View(oferta);
        }
        [Authorize(Roles = "Klient")]
        [HttpPost("/bookOferta")]
        [ActionName("BookOferta")]
        public async Task<IActionResult> BookOferta([FromForm] RezerwacjaAddDTO rezerwacja)
        {
            if(rezerwacja.DataOd> rezerwacja.DataDo)
            {
                TempData["SuccessMessage"] = "Okres jest niepoprawny";
            }
            else if(rezerwacja.DataDo.Date < DateTime.Now)
            {
                ViewData["DataDoSpan"] = "Data do jest niepoprawna";
            }
            else if (rezerwacja.DataOd < DateTime.Now)
            {
                ViewData["DataOdSpan"] = "Data od jest niepoprawna";
            }
            if (rezerwacja.DataOd < DateTime.Now || rezerwacja.DataDo < DateTime.Now || rezerwacja.DataOd > rezerwacja.DataDo)
            {
                return View(rezerwacja);
            }
            var canBook = await CanBook(rezerwacja.IdOferta,rezerwacja.DataOd,rezerwacja.DataDo);
            if(!canBook)
            {
                TempData["SuccessMessage"] = "W zadanym okresie znajduje się już rezerwacja";
                return View(rezerwacja);
            }

            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            var idR = _budowlankaContext.Rezerwacja.Count() == 0 ? 1 : _budowlankaContext.Rezerwacja.Max(o => o.idRezerwacja) + 1;
            
            Rezerwacja r = new Rezerwacja();
            r.idRezerwacja = idR;
            r.DataOd = rezerwacja.DataOd;
            r.DataDo = rezerwacja.DataDo;
            r.Koszt = rezerwacja.Koszt;
            r.CzyZaplacone = false;
            r.idUslugobiorca = (int)u.idUslugobiorca;
            r.idOferta = rezerwacja.IdOferta;
            r.Status = "Utworzono";
            await _budowlankaContext.Rezerwacja.AddAsync(r);
            int ok = await _budowlankaContext.SaveChangesAsync();
            return RedirectToAction("Index", "Oferta");
        }
        private async Task<bool> CanBook(int idOferta, DateTime dataOd, DateTime dataDo)
        {
            var rs = await _budowlankaContext.Rezerwacja.Where(r => r.idOferta == idOferta).ToListAsync();
            foreach(var r in rs)
            {
                if ((dataOd >= r.DataOd && dataOd <= r.DataDo) ||
                   (dataDo >= r.DataOd && dataDo <= r.DataDo) ||
                   (dataOd <= r.DataOd && dataDo >= r.DataDo))
                {
                    return false;
                }
            }
            return true;
        }

    }
}