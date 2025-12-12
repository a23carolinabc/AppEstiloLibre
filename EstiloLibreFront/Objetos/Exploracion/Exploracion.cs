using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Objetos.Exploracion
{
    /// <summary>
    /// DTO para mostrar resumen de conjunto público
    /// </summary>
    public class ConjuntoPublicoResumenDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string? EstiloNombre { get; set; }
        public string? EstacionNombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenBase64 { get; set; }
        public int CantidadPrendas { get; set; }
    }

    /// <summary>
    /// DTO para mostrar resumen de prenda pública
    /// </summary>
    public class PrendaPublicaResumenDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string? CategoriaNombre { get; set; }
        public string? ColorNombre { get; set; }
        public string? MarcaNombre { get; set; }
        public string? EstacionNombre { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    /// <summary>
    /// DTO con datos para filtros de búsqueda
    /// </summary>
    public class DatosExploracionDTO
    {
        public List<ControlItem> Estilos { get; set; } = new();
        public List<ControlItem> Estaciones { get; set; } = new();
        public List<ControlItem> Colores { get; set; } = new();
        public List<ControlItem> Categorias { get; set; } = new();
        public List<ControlItem> Marcas { get; set; } = new();
    }

    /// <summary>
    /// Tipos de búsqueda de conjuntos
    /// </summary>
    public enum TipoBusquedaConjunto
    {
        Todos = 0,
        Estilo = 1,
        Estacion = 2,
        Color = 3
    }

    /// <summary>
    /// Tipos de búsqueda de prendas
    /// </summary>
    public enum TipoBusquedaPrenda
    {
        Todos = 0,
        Categoria = 1,
        Color = 2,
        Estacion = 3,
        Marca = 4
    }
}
