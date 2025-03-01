# Kütüphane Yönetim Sistemi

Bu proje, bir kütüphane yönetim sisteminin web tabanlı bir çözümüdür. Kullanıcılar, kitap ekleme, düzenleme, silme ve ödünç alma işlemleri gerçekleştirebilir. Sistemde kullanıcı giriş/çıkışı, kitap kategorileri, yazarlar, yayınevleri gibi temel özellikler bulunmaktadır.

## Teknolojiler

- **Backend:** ASP.NET Core 9.0
- **Frontend:** HTML, CSS, JavaScript, jQuery, Bootstrap
- **Veritabanı:** MySQL
- **ORM:** Entity Framework Core
- **Kullanıcı Doğrulama:** ASP.NET Core Identity
- **AJAX:** Kitapların dinamik olarak yüklendiği AJAX kullanımı
- **Diğer:** LINQ, SQL Stored Procedures (SP)

## Özellikler

- **Kitap Yönetimi:** Kitap ekleyebilir, düzenleyebilir ve silebilirsiniz.
- **Kategori Yönetimi:** Kitaplar kategorilerle ilişkilendirilebilir.
- **Kullanıcı Yönetimi:** Giriş yapan kullanıcılar kitapları ekleyebilir ve düzenleyebilir.
- **Ödünç İşlemleri:** Kitaplar ödünç alınabilir ve iade edilebilir.
- **Yeni Kitaplar:** Kullanıcı giriş yaptıysa, en son eklenen kitaplar dinamik olarak görüntülenebilir.

## Kurulum

### Adım 1: Projeyi Klonlayın
Projeyi GitHub üzerinden klonlayarak yerel makinenize indirin:

```bash
git clone https://github.com/s1m4y2/kituphane_yonetim_sistemi.git
```

### Adım 2: Veritabanı Bağlantısını Yapılandırın
appsettings.json dosyasındaki ConnectionStrings kısmını düzenleyin ve veritabanı bağlantı bilgilerinizi girin:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=kutuphanedb;User=root;Password=yourpassword;"
  }
}

### Adım 3: Veritabanı Migrate İşlemi
Veritabanını oluşturmak için Entity Framework Core'un migrasyon komutlarını çalıştırın:
dotnet ef database update

### Adım 4: Projeyi Çalıştırın
Projeyi Visual Studio veya komut satırı kullanarak çalıştırabilirsiniz: 
dotnet run

## Kullanıcı Girişi
Projede kullanıcı doğrulama işlemleri ASP.NET Core Identity ile yapılmaktadır. Kullanıcılar giriş yaparak kitapları yönetebilirler.

## Görsel Arayüz
Ana sayfada kullanıcıları karşılayan bir hoş geldiniz mesajı ve bazı tanıtıcı içerikler yer almaktadır. Kullanıcılar, kitap ekleyebilir, düzenleyebilir ve silebilir. Ayrıca, yeni kitaplar ve ödünç alınan kitaplar gibi dinamik veriler sayfada gösterilmektedir.

## Kullanılan MySQL Stored Procedures
GetTotalBooksCount()
Veritabanındaki toplam kitap sayısını döndüren saklı prosedür.

CheckBookExistence(@Baslik)
Yeni kitap eklerken, kitap başlığının veritabanında var olup olmadığını kontrol eden saklı prosedür.
