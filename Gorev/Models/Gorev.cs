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

    public Gorev(string baslik, string aciklama)
    {
        if (string.IsNullOrEmpty(baslik))
        {
            throw new ArgumentNullException(nameof(baslik), "Görev başlığı boş olamaz.");
        }

        if (string.IsNullOrEmpty(aciklama))
        {
            throw new ArgumentNullException(nameof(aciklama), "Görev açıklaması boş olamaz.");
        }

        Baslik = baslik;
        Aciklama = aciklama;
        Tamamlandi = false;
    }
}
