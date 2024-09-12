namespace GorevY.DTOs
{
    public class RegisterDto
    {
        public required string KullaniciAdi { get; set; }
        public required string Email { get; set; }
        public required string Sifre { get; set; }
        public required string ConfirmSifre { get; set; }

        // Basic constructor
        public RegisterDto() { }

        public RegisterDto(string kullaniciAdi, string email, string sifre, string confirmSifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
                throw new ArgumentNullException(nameof(kullaniciAdi), "Kullanıcı adı boş olamaz.");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email), "Email boş olamaz.");
            if (string.IsNullOrEmpty(sifre))
                throw new ArgumentNullException(nameof(sifre), "Şifre boş olamaz.");
            if (sifre != confirmSifre)
                throw new ArgumentException("Şifreler eşleşmiyor.");

            KullaniciAdi = kullaniciAdi;
            Email = email;
            Sifre = sifre;
            ConfirmSifre = confirmSifre;
        }
    }
}
