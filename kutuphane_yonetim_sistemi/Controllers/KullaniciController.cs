using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using kutuphane_yonetim_sistemi.Models;
using System.Threading.Tasks;

namespace kutuphane_yonetim_sistemi.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor
        public KullaniciController(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Kayıt ekranı (GET)
        public IActionResult Register() => View();

        // Kayıt işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Register(Kullanici model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new Kullanici
                {
                    UserName = model.Email ?? model.AdSoyad,
                    Email = model.Email,
                    AdSoyad = model.AdSoyad
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Rol kontrolü
                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                // Hataları daha kullanıcı dostu şekilde göster
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Login ekranı (GET)
        public IActionResult Login() => View();

        // Giriş işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
            }

            return View();
        }

        // Çıkış işlemi (Logout)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["ShowLogoutButton"] = true; // Çıkış butonunu göster

            var model = new ProfilViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                AdSoyad = user.AdSoyad
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfilViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Kullanıcı adı ve e-posta değişikliği
                if (model.UserName != user.UserName)
                {
                    var userNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                    if (!userNameResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Kullanıcı adı değiştirilemedi.");
                        return View(model);
                    }
                }

                if (model.Email != user.Email)
                {
                    var emailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!emailResult.Succeeded)
                    {
                        ModelState.AddModelError("", "E-posta adresi değiştirilemedi.");
                        return View(model);
                    }
                }

                // Ad Soyad değişikliği
                if (model.AdSoyad != user.AdSoyad)
                {
                    user.AdSoyad = model.AdSoyad;
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Ad Soyad değiştirilemedi.");
                        return View(model);
                    }
                }

                // Şifre değişikliği
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmNewPassword)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    ModelState.AddModelError("", "Yeni şifreler eşleşmiyor.");
                    return View(model);
                }

                // Profil güncelleme başarılı
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


    }
}
