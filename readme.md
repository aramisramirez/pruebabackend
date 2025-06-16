> **⚠️ Importante:**  
> Asegúrate de tener las siguientes versiones de .Net para evitar problemas:
>
> - **.Net:** `9.0.300`

> **💡 Consejos:**  
> Comandos de ejecución del proyecto:
>
> ```sh
> dotnet run      # Para ejecutar el proyecto
> dotnet publish -c Release     # Para general el compilado del proyecto
>
>
> SCAFFOLD SQLSERVER
> dotnet ef dbcontext scaffold "Server=localhost;Database=prueba;User Id=sa;Password=Junior177!;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c proyectoDbContext -f --use-database-names
>
> INSTALACIONES
> dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.6
> dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.6
> dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.6
> dotnet add package Swashbuckle.AspNetCore --version 9.0.1
> dotnet add package Microsoft.AspNetCore.Authorization --version 9.0.6
> dotnet add package BCrypt.Net-Next --version 4.0.3
> dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.6
> dotnet add package System.IdentityModel.Tokens.Jwt --version 8.12.0
> ```
>
> **💡 Obsevaciones:**

Esto es el backend de la aplicación de restaurante. Aqui se lleva toda la logica del proyecto 🚀
