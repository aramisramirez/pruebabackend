using System.Security.Cryptography;
using System.Text;
using login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly proyectoDbContext _context;

    public UsuariosController(proyectoDbContext context)
    {
        _context = context;
    }


    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios(
    string? nombre = null,
    string? rol = null,
    string? correo = null,
    int pageNumber = 1,
    int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        IQueryable<Usuario> query = _context.Usuarios;

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(u => u.Nombre.Contains(nombre));

        if (!string.IsNullOrEmpty(rol))
            query = query.Where(u => u.Rol.Contains(rol));

        if (!string.IsNullOrEmpty(correo))
            query = query.Where(u => u.Correo.Contains(correo));

        var totalRecords = await query.CountAsync();

        var usuarios = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Opcional: devolver datos con metadatos de paginación
        var response = new
        {
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Data = usuarios.Select(u =>
            {
                u.Contrasena = null;
                return u;
            })
        };

        return Ok(response);
    }


    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        usuario.Contrasena = null; // No enviar la contraseña

        return usuario;
    }


    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {

        if (!string.IsNullOrEmpty(usuario.Contrasena))
        {
            usuario.Contrasena = HashPassword(usuario.Contrasena);
        }

        usuario.FechaCreacion = DateTime.UtcNow;

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            usuario.Id,
            usuario.Nombre,
            usuario.Correo,
            usuario.Rol,
            usuario.FechaCreacion
        });
    }

    // PUT: api/Usuarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {



        var usuarioExistente = await _context.Usuarios.FindAsync(id);
        if (usuarioExistente == null)
            return NotFound();

        usuarioExistente.Nombre = usuario.Nombre;
        usuarioExistente.Correo = usuario.Correo;
        usuarioExistente.Rol = usuario.Rol;

        if (!string.IsNullOrEmpty(usuario.Contrasena))
        {
            usuarioExistente.Contrasena = HashPassword(usuario.Contrasena);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {


        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}
