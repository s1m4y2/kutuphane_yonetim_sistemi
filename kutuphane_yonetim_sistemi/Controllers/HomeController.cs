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

        // Constructor - DbContext'i almak için
        public HomeController(ILogger<HomeController> logger, LibraryDbContext context)
        {
            _logger = logger;
            _context = context; // _context'i constructor'dan alýyoruz
        }

        // Ana sayfa (statik içerikler ve tanýtýmlar)
        public IActionResult Index()
        {
            // Tanýtýcý yazýlar
            ViewBag.Tanitim1 = "Kitaplarýmýzýn tamamý sizler için seçildi!";
            ViewBag.Tanitim2 = "Ödünç almak için giriþ yapmanýz yeterli.";
            ViewBag.Tanitim3 = "Yeni kitaplar ekleniyor. Takipte kalýn!";

            return View();
        }

        // Kitap listesi sayfasýný AJAX ile yüklemek için
        public IActionResult GetKitaplar()
        {
            var kitaplar = _context.Kitaplar
                .Include(k => k.Yazar)
                .Include(k => k.Kategori)
                .Include(k => k.Yayinevi)
                .ToList();

            // Modeli _KitaplarPartial'a göndereceðiz.
            return PartialView("_KitaplarPartial", kitaplar);
        }

    }
}
