using kutuphane_yonetim_sistemi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace kutuphane_yonetim_sistemi.ViewModels
{
    public class KitapViewModel
    {
        public Kitap Kitap { get; set; }
        public List<SelectListItem> Kategoriler { get; set; }
        public string YazarAdi { get; set; }  // Yazar adı manuel giriş
        public string YayineviAdi { get; set; }  // Yayinevi adı manuel giriş
    }

}
