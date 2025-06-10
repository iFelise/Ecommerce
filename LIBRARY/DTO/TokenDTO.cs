using LIBRARY.Shared.Entity;

namespace LIBRARY.Shared.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
