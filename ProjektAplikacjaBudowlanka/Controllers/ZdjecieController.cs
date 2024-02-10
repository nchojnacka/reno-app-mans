using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models.DTO.Zdjecie;
using System.Security.Claims;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class ZdjecieController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public ZdjecieController(BudowlankaDBContext budowlankaContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _budowlankaContext = budowlankaContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles="Firma")]
        public async Task<IActionResult> Index()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            var l = await _budowlankaContext.Zdjecie.Where(z => z.idUslugodawca == u.idUslugodawca).ToListAsync();
            List<ZdjecieSelectDTO> zdjecia = new List<ZdjecieSelectDTO>();
            foreach(var z in l)
            {
                ZdjecieSelectDTO zdj = new ZdjecieSelectDTO();
                zdj.NazwaPliku = z.NazwaPliku;
                zdj.idZdjecie = z.idZdjecie;
                zdj.idUslugodawca = z.idUslugodawca;
                zdjecia.Add(zdj);
            }
            return View(zdjecia);
        }
        [Authorize(Roles = "Firma")]
        [HttpGet]
        [ActionName("AddZdjecie")]
        public async Task<IActionResult> AddZdjecie()
        {
            return View();
        }


        [Authorize(Roles = "Firma")]
        [HttpPost]
        [ActionName("AddZdjecie")]
        public async Task<IActionResult> AddZdjecie(ZdjecieAddDTO zdjecie)
        {
            //Console.WriteLine(zdjecie.FileUpload);
            //try
            //{
                
            //    var idUser = int.Parse(User.FindFirstValue(ClaimTypes.Sid)!);
            //    var u = await _budowlankaContext.User.Where(u => u.idUser == idUser).FirstOrDefaultAsync();
            //    if (formFile != null && formFile.Length > 0)
            //    {
            //        // Przetwarzanie przesłanego pliku
            //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            //        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "lib/Zdjecia", u.Login + "_" + fileName);

            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await formFile.CopyToAsync(stream);
            //        }
            //        var idZ = _budowlankaContext.Zdjecie.Count() == 0 ? 1 : _budowlankaContext.Zdjecie.Max(z => z.idZdjecie) + 1;
            //        Zdjecie z = new Zdjecie();
            //        z.idZdjecie = idZ;
            //        z.NazwaPliku = u.Login + "_" + fileName;
            //        z.idUslugodawca = (int)u.idUslugodawca;
            //        await _budowlankaContext.Zdjecie.AddAsync(z);
            //        int ok = await _budowlankaContext.SaveChangesAsync();
            //        if (ok > 0)
            //            TempData["SuccessMessage"] = "Dodano zdjęcie";
            //        else
            //            TempData["SuccessMessage"] = "Coś poszło nie tak";
            //        return RedirectToAction("Index", "Zdjecie");
            //    }
            //    TempData["ErrorMessage"] = "Błąd podczas przesyłania zdjęcia.";
            //    return RedirectToAction("Index", "Zdjecie");
            //}
            //catch (Exception ex)
            //{
            //    TempData["ErrorMessage"] = "Wystąpił błąd: " + ex.Message;
            //    return RedirectToAction("Index", "Zdjecie");
            //}
            return View();
        }

    }
}