namespace KotkovAPI.DTOs
{
    public class AccountDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
    public class TokenDTO
    {
        public required string Access_Token { get; set; }
        public required string Username { get; set; }
    }
    public class RegisterPersonDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}