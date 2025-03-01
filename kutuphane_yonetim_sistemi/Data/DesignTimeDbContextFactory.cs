using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace kutuphane_yonetim_sistemi.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

            // appsettings.json dosyasını kullanarak bağlantı dizesini ayarlıyoruz
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Proje dizinini alır
                .AddJsonFile("appsettings.json")  // appsettings.json dosyasını yükler
                .Build();

            // Veritabanı bağlantı dizesini MySQL için ayarlıyoruz
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new LibraryDbContext(optionsBuilder.Options);  // DbContext nesnesini döndürüyoruz
        }
    }
}
