using kutuphane_yonetim_sistemi.Data;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optivem.Framework.Core.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Veritabaný baðlantýsýný güncelleme
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Identity yapýlandýrmasý
builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    // Kimlik doðrulama ayarlarý
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<LibraryDbContext>()  // LibraryDbContext'i kullanýyoruz
    .AddDefaultTokenProviders();  // Token saðlayýcýlarý ekliyoruz

// MVC ve Razor Page servisleri
builder.Services.AddControllersWithViews();

// Uygulama baþlangýcýný yapýyoruz
var app = builder.Build();

// Hata yönetimi ve middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Hata sayfasý
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS (HTTP Strict Transport Security)
}

// HTTPS yönlendirmesi ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();  // Statik dosyalarý kullanýyoruz

// Routing ve kimlik doðrulama
app.UseRouting();
app.UseAuthentication();  // Kimlik doðrulama
app.UseAuthorization();   // Yetkilendirme

// Controller route yapýlandýrmasý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Baþlangýçta çalýþtýrýlacak iþlemleri burada tanýmlayabilirsiniz (örneðin, veri eklemek gibi)
app.Run();
