namespace GorevY.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public string? Detailed { get; set; } // Nullable, böylece Detailed null olabilir
    }
}
