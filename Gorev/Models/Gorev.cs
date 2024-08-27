using System.ComponentModel.DataAnnotations;

namespace GorevY.Models
{
    public class Gorev
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Görev adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Görev adı en fazla 100 karakter olabilir.")]
        public string GorevAdi { get; set; }

        [Required(ErrorMessage = "Görev açıklaması gereklidir.")]
        public string Aciklama { get; set; }

        public bool Tamamlandi { get; set; }

        // Diğer özellikler
    }
}
