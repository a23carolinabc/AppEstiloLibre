using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Objetos.Administracion
{
    public class UsuarioAdminResumenDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoE { get; set; } = string.Empty;
        public List<string> PermisosAsignados { get; set; } = new();
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO para datos necesarios al crear nuevo usuario admin
    /// </summary>
    public class UsuarioAdminData
    {
        public UsuarioAdminDTO? Usuario { get; set; }
        public List<ControlItem> Idiomas { get; set; } = new();
        public List<PermisoDTO> PermisosDisponibles { get; set; } = new();
    }

    /// <summary>
    /// DTO con datos de usuario administrador
    /// </summary>
    public class UsuarioAdminDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string? Apellido2 { get; set; }
        public string CorreoE { get; set; } = string.Empty;
        public int IdiomaId { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public List<int> PermisosIds { get; set; } = new();
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO para guardar usuario administrador
    /// </summary>
    public class UsuarioAdminSaveDataDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string? Contraseña { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string? Apellido2 { get; set; }
        public string CorreoE { get; set; } = string.Empty;
        public int IdiomaId { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public List<int> PermisosIds { get; set; } = new();
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO de permiso con flag de asignación
    /// </summary>
    public class PermisoDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Asignado { get; set; }
    }

    /// <summary>
    /// Parámetros de búsqueda de usuarios
    /// </summary>
    public class BusquedaUsuariosDTO
    {
        public string? TextoBusqueda { get; set; }
        public TipoBusquedaUsuario TipoBusqueda { get; set; }
    }

    /// <summary>
    /// Tipos de búsqueda de usuarios
    /// </summary>
    public enum TipoBusquedaUsuario
    {
        Todos = 0,
        Nombre = 1,
        Login = 2,
        Email = 3
    }

    public class UsuarioNormalResumenDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoE { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int CantidadPrendas { get; set; }
        public int CantidadConjuntos { get; set; }
        public bool Publico { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO para mostrar datos completos de usuario normal con prendas y conjuntos
    /// </summary>
    public class UsuarioNormalShowDataDTO
    {
        public UsuarioNormalDTO Usuario { get; set; } = new();
        public List<ControlItem> Idiomas { get; set; } = new();
        public List<PrendaAdminDTO> Prendas { get; set; } = new();
        public List<ConjuntoAdminDTO> Conjuntos { get; set; } = new();
    }

    /// <summary>
    /// DTO con datos de usuario normal
    /// </summary>
    public class UsuarioNormalDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string? Apellido2 { get; set; }
        public string CorreoE { get; set; } = string.Empty;
        public int IdiomaId { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public bool Publico { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO de prenda para vista de administración
    /// </summary>
    public class PrendaAdminDTO
    {
        public int Id { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? ColorNombre { get; set; }
        public string? MarcaNombre { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO de conjunto para vista de administración
    /// </summary>
    public class ConjuntoAdminDTO
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public string? EstilonNombre { get; set; }
        public bool EsFavorito { get; set; }
        public int CantidadPrendas { get; set; }
        public string? ImagenBase64 { get; set; }
    }
}