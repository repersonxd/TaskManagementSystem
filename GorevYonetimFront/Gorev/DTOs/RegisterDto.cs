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
                throw new ArgumentNullException(nameof(kullaniciAdi), "Kullanýcý adý boþ olamaz.");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email), "Email boþ olamaz.");
            if (string.IsNullOrEmpty(sifre))
                throw new ArgumentNullException(nameof(sifre), "Þifre boþ olamaz.");
            if (sifre != confirmSifre)
                throw new ArgumentException("Þifreler eþleþmiyor.");

            KullaniciAdi = kullaniciAdi;
            Email = email;
            Sifre = sifre;
            ConfirmSifre = confirmSifre;
        }
    }
}
