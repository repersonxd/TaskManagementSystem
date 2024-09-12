using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GorevY.Models;

namespace GorevY.Models
{
    public class Gorev
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Görev adı gereklidir.")]
        public required string GorevAdi { get; set; }

        [Required(ErrorMessage = "Görev açıklaması gereklidir.")]
        public required string Aciklama { get; set; }

        public bool Tamamlandi { get; set; }

        [DataType(DataType.Date)]
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? TamamlanmaTarihi { get; set; }

        // Foreign Key for Kullanici
        [ForeignKey("KullaniciId")]
        public int KullaniciId { get; set; }

        // Navigation Property
        public required Kullanici Kullanici { get; set; }
    }
}
