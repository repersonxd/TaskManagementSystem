namespace GorevY.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public required string KullaniciAdi { get; set; }
        public required string Email { get; set; }
        public required string Sifre { get; set; }
        public ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();

        public Kullanici() { }

        public Kullanici(string kullaniciAdi, string email, string sifre)
        {
            KullaniciAdi = kullaniciAdi ?? throw new ArgumentNullException(nameof(kullaniciAdi));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Sifre = sifre ?? throw new ArgumentNullException(nameof(sifre));
        }
    }
}
