using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace KotkovAPI.Data.Services
{
    public class AccountService
    {
        private readonly KotkovAPIContext _context;

        public AccountService(KotkovAPIContext context)
        {
            _context = context;
        }

        private ClaimsIdentity? GetIdentity(AccountDTO accountDTO)
        {
            var person = _context.People.FirstOrDefault(p => p.Login == accountDTO.Login && p.Password == PasswordEncryptor.HashPassword(accountDTO.Password));
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        public TokenDTO? Token(AccountDTO accountDTO)
        {
            var identity = GetIdentity(accountDTO);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            TokenDTO response = new TokenDTO
            {
                Access_Token = encodedJwt,
                Username = identity.Name!
            };

            return response;
        }
        
        public bool RegisterPerson(RegisterPersonDTO registerPersonDTO)
        {
            var existingPerson = _context.People.FirstOrDefault(p => p.Login == registerPersonDTO.Login);
            if (existingPerson != null)
            {
                return false;
            }
            var validRoles = new[] { Roles.Admin, Roles.Student, Roles.Teacher };
            if (!validRoles.Contains(registerPersonDTO.Role))
            {
                return false;
            }

            Person person = new Person
            {
                Login = registerPersonDTO.Login,
                Password = PasswordEncryptor.HashPassword(registerPersonDTO.Password),
                Role = registerPersonDTO.Role
            };
            
            _context.People.Add(person);
            _context.SaveChanges();

            return true;
        }
    }
}