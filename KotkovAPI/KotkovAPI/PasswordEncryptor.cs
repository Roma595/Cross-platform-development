using System.Security.Cryptography;
using System.Text;

namespace KotkovAPI;
public static class PasswordEncryptor
{
    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool Verify(string password, string hash)
    {
        var newHash = HashPassword(password);
        return newHash == hash;
    }
}