namespace GorevY.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } // Add Id property for the update case
        public required string KullaniciAdi { get; set; }
        public required string Email { get; set; }
        public required string Sifre { get; set; }

        public UserDto() { }

        public UserDto(int id, string kullaniciAdi, string email, string sifre)
        {
            Id = id;
            KullaniciAdi = kullaniciAdi ?? throw new ArgumentNullException(nameof(kullaniciAdi));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Sifre = sifre ?? throw new ArgumentNullException(nameof(sifre));
        }
    }
}
