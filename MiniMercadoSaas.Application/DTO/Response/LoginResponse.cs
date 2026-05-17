namespace MiniMercadoSaas.Application.DTO.Response;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public UserDataResponse UserData { get; set; } = null!;

    public class UserDataResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        
    }
}