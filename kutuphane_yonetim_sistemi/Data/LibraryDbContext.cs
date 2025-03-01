using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using kutuphane_yonetim_sistemi.Models;


namespace kutuphane_yonetim_sistemi.Data
{
    public class LibraryDbContext : IdentityDbContext<Kullanici>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Odunc> OduncIslemleri { get; set; }
        public DbSet<Yazar> Yazarlar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Yayinevi> Yayinevleri { get; set; }

        public int GetBorrowedBooksCount(string kullaniciId)
        {
            var sql = "SELECT GetBorrowedBooksCount(@kullaniciId)";        //FONKSİYON GetBorrowedBooksCount
            var parameter = new MySqlParameter("@kullaniciId", MySqlDbType.VarChar);
            parameter.Value = kullaniciId;

            var result = this.Database.ExecuteSqlRaw(sql, parameter);
            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { KategoriId = 1, Ad = "Roman" },
                new Kategori { KategoriId = 2, Ad = "Bilim Kurgu" },
                new Kategori { KategoriId = 3, Ad = "Tarih" },
                new Kategori { KategoriId = 4, Ad = "Felsefe" }
            );

            modelBuilder.Entity<Odunc>()
            .HasOne(o => o.Kitap)      // Odunc bir Kitap ile ilişkilidir
            .WithMany(k => k.OduncIslemleri)  // Kitap birden fazla Odunc ile ilişkili
            .HasForeignKey(o => o.KitapId);  // Odunc, KitapId ile ilişkilidir

            // Odunc ve Kullanici ilişkisini belirt
            modelBuilder.Entity<Odunc>()
                .HasOne(o => o.Kullanici)
                .WithMany() // Kullanıcı birden fazla ödünç işlemi alabilir
                .HasForeignKey(o => o.KullaniciId);



        }
    }
}
