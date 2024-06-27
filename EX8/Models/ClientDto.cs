namespace EX8.Models // Zmień na rzeczywiste namespace Twoich modeli
{
    public class ClientDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Pesel { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
    }
}