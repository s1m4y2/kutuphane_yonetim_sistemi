using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace kutuphane_yonetim_sistemi.Models
{
    public class Kitap
    {
        public int KitapId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Yazar seçilmelidir.")]
        public int YazarId { get; set; }

        [Required(ErrorMessage = "Yayınevi seçilmelidir.")]
        public int YayineviId { get; set; }
        public string KullaniciId { get; set; }

        public Kategori Kategori { get; set; }
        public Yazar Yazar { get; set; }
        public Yayinevi Yayinevi { get; set; }
        public Kullanici Kullanici { get; set; }

        public ICollection<Odunc> OduncIslemleri { get; set; }
    }





}
