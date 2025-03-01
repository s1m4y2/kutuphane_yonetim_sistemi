using Microsoft.AspNetCore.Mvc;
using kutuphane_yonetim_sistemi.Data;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace kutuphane_yonetim_sistemi.Controllers
{
    [Authorize]  // Sadece giriş yapmış kullanıcılar bu controller'daki aksiyonlara erişebilir
    public class OduncController : Controller
    {
        private readonly LibraryDbContext _context;

        public OduncController(LibraryDbContext context)
        {
            _context = context;
        }

        // Ödünç alınan kitapları listele (giriş yapmayanlar da görebilir)
        public IActionResult Index()
        {



            var oduncKitaplar = _context.OduncIslemleri.Include(o => o.Kitap).Include(o => o.Kullanici).ToList();
            return View(oduncKitaplar);
        }

        // Kullanıcıya ait ödünç alınan kitapları listele
        public void OduncAlinanlar()
        {
            var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini doğru alıyoruz
                                                                              // Burada SQL fonksiyonunu çağırarak ödünç kitap sayısını alıyoruz
            var borrowedBooksCount = _context.GetBorrowedBooksCount(kullaniciId);                             //FONKSYİON GetBorrowedBooksCount
            // Modeli View'a gönderiyoruz
            ViewBag.BorrowedBooksCount = borrowedBooksCount;

            var oduncKitaplar = _context.OduncIslemleri
                .Where(o => o.KullaniciId == kullaniciId) // Kullanıcıya ait ödünç kitapları al
                .Include(o => o.Kitap)  // Kitap ile ilişkiyi dahil et
                .Include(o => o.Kullanici) // Kullanıcı bilgilerini de dahil et
                .ToList();



            //return View(oduncKitaplar);
        }

        // Ödünç işlemi (giriş yapmış kullanıcılar için)
        [HttpPost]
        public IActionResult OduncAl(int KitapId)
        {
            var kitap = _context.Kitaplar.Find(KitapId);

            if (kitap == null)
            {
                TempData["ErrorMessage"] = "Kitap bulunamadı.";
                return RedirectToAction("Index", "Kitap");
            }

            if (_context.OduncIslemleri.Any(o => o.KitapId == KitapId && o.IadeTarihi == null))
            {
                TempData["ErrorMessage"] = "Kitap şu anda ödünçte.";
                return RedirectToAction("Index", "Kitap");
            }
            var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var odunc = new Odunc
            {
                KitapId = KitapId,
                KullaniciId = kullaniciId // veya kullanıcının Id'sini doğru şekilde al
            };

            _context.OduncIslemleri.Add(odunc);
            _context.SaveChanges();

            TempData["Message"] = "Kitap ödünç alındı.";
            return RedirectToAction("Index", "Kitap");
        }

        [HttpPost]
        public IActionResult IadeEt(int KitapId)
        {
            try
            {
                var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // İade işlemi yapılacak ödünç kaydını buluyoruz
                var odunc = _context.OduncIslemleri.FirstOrDefault(o => o.KitapId == KitapId);
                if (odunc == null)
                {
                    TempData["ErrorMessage"] = "Ödünç kaydı bulunamadı.";
                    return RedirectToAction("Index", "Kitap");
                }

                // Eğer kitap zaten iade edilmişse, hata mesajı gösteriyoruz
                if (odunc.IadeTarihi.HasValue)
                {
                    TempData["ErrorMessage"] = "Kitap zaten iade edilmiştir.";
                    return RedirectToAction("Index", "Kitap");
                }

                // İade tarihini güncelliyoruz
                odunc.IadeTarihi = DateTime.Now;
                _context.SaveChanges();

                TempData["Message"] = "Kitap başarıyla iade edildi.";
                return RedirectToAction("Index", "Kitap");
            }
            catch (Exception ex)
            {
                // Hata mesajı
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Kitap");
            }
        }

    }
}
