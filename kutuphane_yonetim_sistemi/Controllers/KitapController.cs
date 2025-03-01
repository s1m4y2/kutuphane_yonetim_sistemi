using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_yonetim_sistemi.Data;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using kutuphane_yonetim_sistemi.ViewModels;

namespace kutuphane_yonetim_sistemi.Controllers
{
    [Authorize]  // Sadece giriş yapmış kullanıcılar bu controller'daki aksiyonlara erişebilir
    public class KitapController : Controller
    {
        private readonly LibraryDbContext _context;

        // Constructor
        public KitapController(LibraryDbContext context)
        {
            _context = context;
        }

        // Kitaplar listesi
        public IActionResult Index()
        {


            var kitaplar = _context.Kitaplar.Include(k => k.Yazar).Include(k => k.Kategori).Include(k => k.Yayinevi).ToList();
            var yeniKitaplar = _context.Kitaplar
                .OrderByDescending(k => k.KitapId)
                .Take(5)
                .Include(k => k.Yazar)
                .ToList();


            var oduncKitaplar = _context.OduncIslemleri
                .Where(o => o.IadeTarihi == null)
                .Select(o => o.KitapId)
                .ToList();

            int totalBooksCount = GetTotalBooksCount();                            //SP GetTotalBooksCount
            ViewBag.TotalBooks = totalBooksCount; // Toplam kitap sayısı
            ViewBag.YeniKitaplar = yeniKitaplar; // Yeni eklenen kitaplar
            ViewBag.OduncKitaplar = oduncKitaplar;
            ViewBag.BookCountByCategory = GetBookCountByCategory();

            return View(kitaplar); // Kitapları View'a gönderiyoruz
        }

        public Dictionary<string, int> GetBookCountByCategory()                 //FONKSYİON GetBookCountByCategory
        {
            var bookCountByCategory = _context.Kategoriler
                .Include(k => k.Kitaplar)  // Kategorilere ait kitapları dahil et
                .ToDictionary(k => k.Ad, k => k.Kitaplar.Count); // Kategoriyi anahtar, kitap sayısını değer olarak al

            return bookCountByCategory;
        }



        // Kitap ekleme formu (GET)
        public IActionResult Create()
        {
            var model = new KitapViewModel
            {
                Kategoriler = _context.Kategoriler.Select(k => new SelectListItem
                {
                    Value = k.KategoriId.ToString(),
                    Text = k.Ad
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KitapViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kullanıcıyı al
                var userId = User.Identity.Name;

                // Saklı prosedürle kitap başlığı kontrolü
                using (var connection = new MySqlConnection("Server=localhost;Database=kutuphanedb;User=root;"))
                {
                    connection.Open();

                    using (var command = new MySqlCommand("CALL CheckBookExistence(@Baslik)", connection))         //SP CheckBookExistence
                    {
                        command.Parameters.AddWithValue("@Baslik", model.Kitap.Baslik);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                TempData["ErrorMessage"] = "Bu başlıkla bir kitap zaten var.";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

                // Yeni yazar ekleme
                var yazar = _context.Yazarlar.FirstOrDefault(y => y.AdSoyad == model.YazarAdi);
                if (yazar == null)
                {
                    yazar = new Yazar { AdSoyad = model.YazarAdi };
                    _context.Yazarlar.Add(yazar);
                    _context.SaveChanges();
                }

                // Yeni yayınevi ekleme
                var yayinevi = _context.Yayinevleri.FirstOrDefault(y => y.Ad == model.YayineviAdi);
                if (yayinevi == null)
                {
                    yayinevi = new Yayinevi { Ad = model.YayineviAdi };
                    _context.Yayinevleri.Add(yayinevi);
                    _context.SaveChanges();
                }

                // Kitap nesnesini elle oluştur
                var kitap = new Kitap
                {
                    Baslik = model.Kitap.Baslik,
                    KategoriId = model.Kitap.KategoriId,
                    YazarId = yazar.YazarId,
                    YayineviId = yayinevi.YayineviId,
                    KullaniciId = userId  // Associate the book with the logged-in user
                };

                // Kitabı veritabanına ekle
                _context.Kitaplar.Add(kitap);
                _context.SaveChanges();

                TempData["Message"] = "Kitap başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            // Hatalı formdan dönerse kategorileri tekrar yükle
            model.Kategoriler = _context.Kategoriler.Select(k => new SelectListItem
            {
                Value = k.KategoriId.ToString(),
                Text = k.Ad
            }).ToList();

            return View(model);
        }

        // Kitap güncelleme formu
        // GET: Kitap/Edit/5
        // Kitap düzenleme sayfası (GET)
        public IActionResult Edit(int id)
        {
            var kitap = _context.Kitaplar
                .Include(k => k.Yazar)
                .Include(k => k.Kategori)
                .Include(k => k.Yayinevi)
                .FirstOrDefault(k => k.KitapId == id);

            if (kitap == null || kitap.KullaniciId != User.Identity.Name) // Check if the current user is the owner
            {
                return Unauthorized();  // Return Unauthorized if the user doesn't own the book
            }

            // Kategorileri ve diğer verileri ViewBag'e ekliyoruz
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "KategoriId", "Ad", kitap.KategoriId);
            ViewBag.Yazarlar = new SelectList(_context.Yazarlar, "YazarId", "AdSoyad", kitap.YazarId);
            ViewBag.Yayinevleri = new SelectList(_context.Yayinevleri, "YayineviId", "Ad", kitap.YayineviId);

            return View(kitap); // Kitap modelini View'a gönderiyoruz
        }

        // Kitap güncelleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kitap kitap)
        {
            if (!ModelState.IsValid)
            {
                var existingKitap = _context.Kitaplar.FirstOrDefault(k => k.KitapId == kitap.KitapId);

                if (existingKitap != null && existingKitap.KullaniciId == User.Identity.Name)  // Only allow editing if the user is the owner
                {
                    existingKitap.Baslik = kitap.Baslik;
                    existingKitap.KategoriId = kitap.KategoriId;
                    existingKitap.YazarId = kitap.YazarId;
                    existingKitap.YayineviId = kitap.YayineviId;

                    _context.Kitaplar.Update(existingKitap);
                    _context.SaveChanges();

                    TempData["Message"] = "Kitap başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Kitap bulunamadı veya bu işlemi yapma yetkiniz yok.";
                    return RedirectToAction("Index");
                }
            }

            // Eğer model geçerli değilse, kategorileri tekrar yükle ve formu göster
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "KategoriId", "Ad", kitap.KategoriId);
            ViewBag.Yazarlar = new SelectList(_context.Yazarlar, "YazarId", "AdSoyad", kitap.YazarId);
            ViewBag.Yayinevleri = new SelectList(_context.Yayinevleri, "YayineviId", "Ad", kitap.YayineviId);

            return View(kitap);  // Hatalıysa formu tekrar göster
        }

        // Kitap silme işlemi
        public IActionResult Delete(int id)
        {
            var kitap = _context.Kitaplar.Find(id);
            if (kitap != null && kitap.KullaniciId == User.Identity.Name)
            {
                _context.Kitaplar.Remove(kitap);
                _context.SaveChanges();
                TempData["Message"] = "Kitap başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kitap bulunamadı veya bu işlemi yapma yetkiniz yok.";
            }
            return RedirectToAction("Index");
        }
        public int GetTotalBooksCount()
        {
            using (var connection = new MySqlConnection("Server=localhost;Database=kutuphanedb;User=root;"))
            {
                connection.Open();
                using (var command = new MySqlCommand("CALL GetTotalBooksCount()", connection))
                {
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

    }
}
