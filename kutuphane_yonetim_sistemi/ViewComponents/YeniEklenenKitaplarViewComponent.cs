using Microsoft.AspNetCore.Mvc;
using kutuphane_yonetim_sistemi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace kutuphane_yonetim_sistemi.ViewComponents
{
    public class YeniEklenenKitaplarViewComponent : ViewComponent
    {
        private readonly LibraryDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YeniEklenenKitaplarViewComponent(LibraryDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            // Kullanıcı giriş yaptı mı kontrolü
            bool isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                var yeniKitaplar = _context.Kitaplar
                    .Include(k => k.Yazar)
                    .Include(k => k.Yayinevi)
                    .OrderByDescending(k => k.KitapId)
                    .Take(3)
                    .ToList();
                return View(yeniKitaplar);
            }

            // Kullanıcı giriş yapmamışsa boş bir view dönebiliriz
            return Content(string.Empty);
        }
    }
}
