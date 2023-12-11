using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models;
using ProjektAplikacjaBudowlanka.Models.DTO.Pracownik;
using ProjektAplikacjaBudowlanka.Models.DTO.Uslugodawca;
using ProjektAplikacjaBudowlanka.Models.DTO.Zdjecie;
using System.Diagnostics;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class UslugodawcaController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;

        public UslugodawcaController(BudowlankaDBContext budowlankaContext)
        {
            _budowlankaContext = budowlankaContext;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<UslugodawcaSelectDTO> ekipy = new List<UslugodawcaSelectDTO>();
            foreach(var u in await _budowlankaContext.Uslugodawca.ToListAsync())
            {
                List<ZdjecieSelectDTO> zdjecia = new List<ZdjecieSelectDTO>(); 
                foreach (var z in await _budowlankaContext.Zdjecie.Where(zd => zd.idUslugodawca == u.idUslugodawca).ToListAsync())
                {
                    ZdjecieSelectDTO zd = new ZdjecieSelectDTO();
                    zd.idZdjecie = z.idZdjecie;
                    zd.NazwaPliku = z.NazwaPliku;
                    zdjecia.Add(zd);
                }
                List<PracownikSelectDTO> pracownicy = new List<PracownikSelectDTO>();
                foreach(var up in await _budowlankaContext.Uslugodawca_Pracownik.Where(ups=>ups.idUslugodawca == u.idUslugodawca).ToListAsync())
                {
                    var prac = await _budowlankaContext.Pracownik.Where(p => p.idPracownik == up.idPracownik).FirstOrDefaultAsync();
                    PracownikSelectDTO p = new PracownikSelectDTO();
                    p.Id = up.idPracownik;
                    p.Stanowisko = up.Stanowisko;
                    p.Nazwisko = prac.Nazwisko;
                    p.Imie = prac.Imie;
                    pracownicy.Add(p);
                }
                UslugodawcaSelectDTO us = new UslugodawcaSelectDTO();
                us.idUslugodawca = u.idUslugodawca;
                us.Nazwa = u.Nazwa;
                us.NIP = u.NIP;
                us.Zdjecia = zdjecia;
                us.Pracownicy = pracownicy;
                ekipy.Add(us);
            }

            return View(ekipy);
        }
    }
}