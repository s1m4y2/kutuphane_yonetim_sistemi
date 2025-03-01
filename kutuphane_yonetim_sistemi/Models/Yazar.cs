using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace kutuphane_yonetim_sistemi.Models
{
    public class Yazar
    {
        [Key]
        public int YazarId { get; set; }
        [Required]
        public string AdSoyad { get; set; }
        public List<Kitap> Kitaplar { get; set; }
    }
}
