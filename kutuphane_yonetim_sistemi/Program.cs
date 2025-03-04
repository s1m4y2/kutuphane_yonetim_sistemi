using kutuphane_yonetim_sistemi.Data;
using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optivem.Framework.Core.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Veritabanı bağlantısını güncelleme
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Identity yapılandırması
builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    // Kimlik doğrulama ayarları
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<LibraryDbContext>()  // LibraryDbContext'i kullanıyoruz
    .AddDefaultTokenProviders();  // Token sağlayıcıları ekliyoruz

// MVC ve Razor Page servisleri
builder.Services.AddControllersWithViews();

// Uygulama başlangıcını yapıyoruz
var app = builder.Build();

// Hata yönetimi ve middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Hata sayfası
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS (HTTP Strict Transport Security)
}

// HTTPS yönlendirmesi ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();  // Statik dosyaları kullanıyoruz

// Routing ve kimlik doğrulama
app.UseRouting();
app.UseAuthentication();  // Kimlik doğrulama
app.UseAuthorization();   // Yetkilendirme

// Controller route yapılandırması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Başlangıçta çalıştırılacak işlemleri burada tanımlayabilirsiniz (örneğin, veri eklemek gibi)
app.Run();
