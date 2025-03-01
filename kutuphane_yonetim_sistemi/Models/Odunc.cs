using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace kutuphane_yonetim_sistemi.Models
{
    public class Odunc
    {
        [Key]
        public int OduncId { get; set; }
        public int KitapId { get; set; }

        public virtual Kitap Kitap { get; set; }

        [Required]
        [ForeignKey("Kullanici")]
        public string KullaniciId { get; set; }

        public virtual Kullanici Kullanici { get; set; }
        public DateTime AlisTarihi { get; set; } = DateTime.Now;
        public DateTime? IadeTarihi { get; set; }
    }
}
