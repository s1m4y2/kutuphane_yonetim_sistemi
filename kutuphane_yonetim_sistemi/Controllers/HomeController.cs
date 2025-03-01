using Microsoft.AspNetCore.Mvc;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.EntityFrameworkCore;
using kutuphane_yonetim_sistemi.Data;

namespace kutuphane_yonetim_sistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryDbContext _context; // DbContext nesnesi ekleniyor

        // Constructor - DbContext'i almak i�in
        public HomeController(ILogger<HomeController> logger, LibraryDbContext context)
        {
            _logger = logger;
            _context = context; // _context'i constructor'dan al�yoruz
        }

        // Ana sayfa (statik i�erikler ve tan�t�mlar)
        public IActionResult Index()
        {
            // Tan�t�c� yaz�lar
            ViewBag.Tanitim1 = "Kitaplar�m�z�n tamam� sizler i�in se�ildi!";
            ViewBag.Tanitim2 = "�d�n� almak i�in giri� yapman�z yeterli.";
            ViewBag.Tanitim3 = "Yeni kitaplar ekleniyor. Takipte kal�n!";

            return View();
        }

        // Kitap listesi sayfas�n� AJAX ile y�klemek i�in
        public IActionResult GetKitaplar()
        {
            var kitaplar = _context.Kitaplar
                .Include(k => k.Yazar)
                .Include(k => k.Kategori)
                .Include(k => k.Yayinevi)
                .ToList();

            // Modeli _KitaplarPartial'a g�nderece�iz.
            return PartialView("_KitaplarPartial", kitaplar);
        }

    }
}
