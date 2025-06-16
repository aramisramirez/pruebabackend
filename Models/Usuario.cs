using System;
using System.Collections.Generic;

namespace login.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? Rol { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
