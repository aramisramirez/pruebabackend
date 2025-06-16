using System.Security.Cryptography;
using System.Text;
using login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly proyectoDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(proyectoDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == login.Correo);

        if (usuario == null)
        {
            return Unauthorized("Correo incorrecto");
        }

        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(login.Contrasena, usuario.Contrasena);

        if (!passwordCorrect)
            return Unauthorized("Contraseña incorrecta");

        var token = JwtHelper.GenerateJwtToken(usuario, _config);


        return Ok(new
        {
            token,
            usuario = new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol
            }
        });
    }

}
