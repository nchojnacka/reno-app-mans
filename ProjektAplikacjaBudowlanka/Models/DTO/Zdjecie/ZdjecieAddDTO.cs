using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ProjektAplikacjaBudowlanka.Models.DTO.Zdjecie
{
    public class ZdjecieAddDTO
    {
        [BindProperty]
        public FileUpload FileUpload { get; set; }
    }

    public class FileUpload
    {
        [Required(ErrorMessage = "Proszę wybrać plik.")]
        [Display(Name = "Wybierz plik")]
        public IFormFile FormFile { get; set; }
    }
}
