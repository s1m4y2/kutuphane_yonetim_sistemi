using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace kutuphane_yonetim_sistemi.Models
{
    public class Kullanici : IdentityUser
    {
        [Required]
        public string AdSoyad { get; set; }



    }
}
