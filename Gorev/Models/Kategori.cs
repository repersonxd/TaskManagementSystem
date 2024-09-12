using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GorevY.Models
{
    public class Kategori
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı gereklidir.")]
        public required string Adi { get; set; }

        public required ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>(); // Boş bir liste olarak başlatıyoruz
    }
}
