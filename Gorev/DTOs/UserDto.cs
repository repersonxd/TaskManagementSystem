namespace GorevY.DTOs
{
    public class UserDto
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        // Oluşturucu ekleyin
        public UserDto(string kullaniciAdi, string sifre)
        {
            KullaniciAdi = kullaniciAdi ?? throw new ArgumentNullException(nameof(kullaniciAdi));
            Sifre = sifre ?? throw new ArgumentNullException(nameof(sifre));
        }
    }
}
