using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using login.Models;
using Microsoft.IdentityModel.Tokens;

public static class JwtHelper
{
    public static string GenerateJwtToken(Usuario usuario, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
            new Claim(ClaimTypes.Email, usuario.Correo ?? ""),
            new Claim(ClaimTypes.Role, usuario.Rol ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
