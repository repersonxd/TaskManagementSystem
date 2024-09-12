namespace GorevY.DTOs
{
    public class LoginDto
    {
        public required string KullaniciAdi { get; set; }
        public required string Sifre { get; set; }

        public LoginDto() { } // Parametresiz oluşturucu

        public LoginDto(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
                throw new ArgumentNullException(nameof(kullaniciAdi), "Kullanıcı adı boş olamaz.");
            if (string.IsNullOrEmpty(sifre))
                throw new ArgumentNullException(nameof(sifre), "Şifre boş olamaz.");
          

            KullaniciAdi = kullaniciAdi;
           
            Sifre = sifre;
        }
    }
}
