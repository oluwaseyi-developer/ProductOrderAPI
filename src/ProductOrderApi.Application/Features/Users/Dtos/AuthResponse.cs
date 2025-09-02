namespace ProductOrderApi.Application.Features.Users.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserDto? User { get; set; }
    }
}
