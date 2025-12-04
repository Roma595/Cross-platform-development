namespace KotkovAPI;

using System.Text;
using Microsoft.IdentityModel.Tokens;
public static class AuthOptions
{
    public const string ISSUER = "Issuer";
    public const string AUDIENCE = "Audience";   
    private const string KEY = "fN7!xG2pZq@9Rk5Vt#1LmC8w$Hd3By4e"; 
    public const int LIFETIME = 60;          

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}