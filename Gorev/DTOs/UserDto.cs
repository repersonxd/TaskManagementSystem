namespace GorevY.DTOs
{
    public class UserDto
    {
        public required string KullaniciAdi { get; set; }
        public required string Sifre { get; set; }

        public UserDto(string kullaniciAdi, string sifre)
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
