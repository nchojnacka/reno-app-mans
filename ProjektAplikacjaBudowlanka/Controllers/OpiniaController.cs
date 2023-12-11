using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models;
using ProjektAplikacjaBudowlanka.Models.DTO.Opinia;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class OpiniaController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;

        public OpiniaController(BudowlankaDBContext budowlankaContext)
        {
            _budowlankaContext = budowlankaContext;
        }
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            
            var l = await _budowlankaContext.Opinia.Where(o => o.idUslugodawca == id).ToListAsync();
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var us = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            List<OpiniaSelectDTO> opinie = new List<OpiniaSelectDTO>();
            foreach (var o in l)
            {
                OpiniaSelectDTO op = new OpiniaSelectDTO();
                op.Opis = o.Opis;
                op.Ocena= o.Ocena;
                op.idUslugodawca = o.idUslugodawca;
                op.idOpinia = o.idOpinia;
                op.Login = us.Login;
                opinie.Add(op);
            }
            ViewBag.Id = id;
            return View(opinie);
        }

        [Authorize(Roles = "Klient")]
        [HttpGet]
        [ActionName("AddOpinia")]
        public async Task<IActionResult> AddOpinia(int id)
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var us = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            OpiniaAddDTO opiniaAddDTO = new OpiniaAddDTO();
            opiniaAddDTO.idUslugodawca = id;
            opiniaAddDTO.idUslugobiorca = (int)us.idUslugobiorca;
            opiniaAddDTO.Login = us.Login;
            return View(opiniaAddDTO);
        }

        [Authorize(Roles = "Klient")]
        [HttpPost]
        [ActionName("AddOpinia")]
        public async Task<IActionResult> AddOpinia([FromForm] OpiniaAddDTO opinia)
        {
            if(opinia.Opis == null)
            {
                ViewData["OpisSpan"] = "Opis nie może być pusty";
                return View(opinia);
            }
            var idO = _budowlankaContext.Opinia.Count() == 0 ? 1 : _budowlankaContext.Opinia.Max(o => o.idOpinia) + 1;
            Opinia o = new Opinia();
            o.idOpinia = idO;
            o.Opis = opinia.Opis;
            o.idUslugobiorca = opinia.idUslugobiorca;
            o.idUslugodawca = opinia.idUslugodawca;
            o.Ocena = opinia.Ocena;
            await _budowlankaContext.Opinia.AddAsync(o);
            int ok = await _budowlankaContext.SaveChangesAsync();
            if (ok > 0)
                TempData["SuccessMessage"] = "Dodano opinię.";
            else
                TempData["SuccessMessage"] = "Coś poszło nie tak";
            return RedirectToAction("Index", "Uslugodawca");
        }
    }
}