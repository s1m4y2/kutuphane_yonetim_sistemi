using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace kutuphane_yonetim_sistemi.Models
{
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }
        [Required]
        public string Ad { get; set; }

        public ICollection<Kitap> Kitaplar { get; set; }
    }
}
