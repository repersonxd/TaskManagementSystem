using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // DataAnnotations için gerekli namespace

namespace GorevY.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public required string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Email gereklidir.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public required string Sifre { get; set; }

        // Kullanıcının görevleri
        public required ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();
    }
}
