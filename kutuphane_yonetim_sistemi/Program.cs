using kutuphane_yonetim_sistemi.Data;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optivem.Framework.Core.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Veritaban� ba�lant�s�n� g�ncelleme
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Identity yap�land�rmas�
builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    // Kimlik do�rulama ayarlar�
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<LibraryDbContext>()  // LibraryDbContext'i kullan�yoruz
    .AddDefaultTokenProviders();  // Token sa�lay�c�lar� ekliyoruz

// MVC ve Razor Page servisleri
builder.Services.AddControllersWithViews();

// Uygulama ba�lang�c�n� yap�yoruz
var app = builder.Build();

// Hata y�netimi ve middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Hata sayfas�
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS (HTTP Strict Transport Security)
}

// HTTPS y�nlendirmesi ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();  // Statik dosyalar� kullan�yoruz

// Routing ve kimlik do�rulama
app.UseRouting();
app.UseAuthentication();  // Kimlik do�rulama
app.UseAuthorization();   // Yetkilendirme

// Controller route yap�land�rmas�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ba�lang��ta �al��t�r�lacak i�lemleri burada tan�mlayabilirsiniz (�rne�in, veri eklemek gibi)
app.Run();
