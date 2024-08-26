namespace GorevY.DTOs
{
    public class LoginDto
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        // Oluşturucu ekleyin
        public LoginDto(string kullaniciAdi, string sifre)
        {
            KullaniciAdi = kullaniciAdi ?? throw new ArgumentNullException(nameof(kullaniciAdi));
            Sifre = sifre ?? throw new ArgumentNullException(nameof(sifre));
        }
    }
}
