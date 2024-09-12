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
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public required string Sifre { get; set; }

        // Kullanıcının görevleri (boş bir liste ile başlatılıyor)
        public required ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();
    }
}
