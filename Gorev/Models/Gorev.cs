using GorevY.Models;
using System.ComponentModel.DataAnnotations;

public class Gorev
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Görev başlığı gereklidir.")]
    public string Baslik { get; set; }

    [Required(ErrorMessage = "Görev açıklaması gereklidir.")]
    public string Aciklama { get; set; }

    public bool Tamamlandi { get; set; }

    public int KullaniciId { get; set; }

    // Kullanici ile ilişki
    public Kullanici Kullanici { get; set; }

    public Gorev(string baslik, string aciklama)
    {
        Baslik = baslik ?? throw new ArgumentNullException(nameof(baslik), "Görev başlığı boş olamaz.");
        Aciklama = aciklama ?? throw new ArgumentNullException(nameof(aciklama), "Görev açıklaması boş olamaz.");
    }
}
