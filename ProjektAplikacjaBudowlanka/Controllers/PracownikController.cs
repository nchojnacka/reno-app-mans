using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models.DTO;
using ProjektAplikacjaBudowlanka.Models.DTO.Pracownik;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers;

public class PracownikController : Controller
{
    private readonly BudowlankaDBContext _budowlankaContext;
    public PracownikController(BudowlankaDBContext budowlankaContext)
    {
        _budowlankaContext = budowlankaContext;
    }

    [Authorize(Roles = "Firma")]
    public async Task<IActionResult> Index()
    {
        TempData["SuccessMessage"] = null;
        var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
        var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
        List<PracownikSelectDTO> list = new List<PracownikSelectDTO>();
        foreach(var up in await _budowlankaContext.Uslugodawca_Pracownik.Where(us=>us.idUslugodawca==u.idUslugodawca).ToListAsync()) 
        {
            var p = await _budowlankaContext.Pracownik.Where(pr => pr.idPracownik == up.idPracownik).FirstOrDefaultAsync();
            list.Add(new PracownikSelectDTO()
            {
                Imie = p.Imie,
                Nazwisko = p.Nazwisko,
                Stanowisko = up.Stanowisko,
                Id = p.idPracownik
            });
        }
        return View(list);
    }
    [Authorize(Roles = "Firma")]
    [HttpGet("/addPracownik")]
    [ActionName("AddPracownik")]
    public async Task<IActionResult> AddPracownik()
    {
        TempData["SuccessMessage"] = null;
        return View();
    }
    [Authorize(Roles = "Firma")]
    [HttpPost("/addPracownik")]
    [ActionName("AddPracownik")]
    public async Task<IActionResult> AddPracownik([FromForm] PracownikAddDTO pracownik)
    {
        if (pracownik.Nazwisko == null)
            ViewData["NazwiskoSpan"] = "Nazwisko nie może być puste";
        if (pracownik.Imie == null)
            ViewData["ImieSpan"] = "Imie nie może być puste";
        if (pracownik.Stanowisko == null)
            ViewData["StanowiskoSpan"] = "Stanowisko nie może być puste";
        if(pracownik.Nazwisko == null || pracownik.Imie == null || pracownik.Stanowisko == null)
        {
            return View(pracownik);
        }
        var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
        var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
        Pracownik p = new Pracownik();
        var idP = _budowlankaContext.Pracownik.Count() == 0 ? 1 : _budowlankaContext.Pracownik.Max(us => us.idPracownik) + 1;
        p.idPracownik = idP;
        p.Imie = pracownik.Imie;
        p.Nazwisko = pracownik.Nazwisko;
        Uslugodawca_Pracownik up = new Uslugodawca_Pracownik();
        var idUP = _budowlankaContext.Uslugodawca_Pracownik.Count() == 0 ? 1 : _budowlankaContext.Uslugodawca_Pracownik.Max(us => us.idUslugodawca_Pracownik) + 1;
        up.idUslugodawca_Pracownik = idUP;
        up.idPracownik = idP;
        up.idUslugodawca = (int)u.idUslugodawca!;
        up.Stanowisko = pracownik.Stanowisko;

        await _budowlankaContext.Pracownik.AddAsync(p);
        await _budowlankaContext.Uslugodawca_Pracownik.AddAsync(up);
        int ok = await _budowlankaContext.SaveChangesAsync();
        if (ok > 0)
            TempData["SuccessMessage"] = "Dodano pracownika";
        else
            TempData["SuccessMessage"] = "Coś poszło nie tak";
        return RedirectToAction("Index", "Pracownik");
    }

    [Authorize(Roles = "Firma")]
    [HttpGet("/deletePracownik")]
    [ActionName("DeletePracownik")]
    public async Task<IActionResult> DeletePracownik(int id)
    {
        var pracownik = await _budowlankaContext.Pracownik.Where(p => p.idPracownik == id).FirstOrDefaultAsync();
        var UP = await _budowlankaContext.Uslugodawca_Pracownik.Where(p => p.idPracownik == id).FirstOrDefaultAsync();
        if (pracownik == null || UP ==null)
        {
            TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Pracownik");
        }
        _budowlankaContext.Uslugodawca_Pracownik.Remove(UP!);
        _budowlankaContext.Pracownik.Remove(pracownik);
        int ok = await _budowlankaContext.SaveChangesAsync();
        if (ok > 0)
            TempData["SuccessMessage"] = "Usunięto pracownika";
        else
            TempData["SuccessMessage"] = "Coś poszło nie tak";
        return RedirectToAction("Index", "Pracownik");
    }
}