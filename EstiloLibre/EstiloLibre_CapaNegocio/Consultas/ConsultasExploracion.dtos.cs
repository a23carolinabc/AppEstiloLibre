using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasExploracion
    {
        public class DTOs
        {
            /// <summary>
            /// DTO para parámetros de búsqueda de conjuntos públicos
            /// </summary>
            public class BusquedaConjuntosPublicosDTO
            {
                public TipoBusquedaConjunto? TipoBusqueda { get; set; }
                public int? ValorBusqueda { get; set; }
            }

            /// <summary>
            /// DTO para parámetros de búsqueda de prendas públicas
            /// </summary>
            public class BusquedaPrendasPublicasDTO
            {
                public TipoBusquedaPrenda? TipoBusqueda { get; set; }
                public int? ValorBusqueda { get; set; }
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

            /// <summary>
            /// DTO para mostrar resumen de conjunto público con datos del usuario
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
            /// DTO para mostrar resumen de prenda pública con datos del usuario
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
            /// DTO con datos necesarios para búsquedas
            /// </summary>
            public class DatosExploracionDTO
            {
                public IEnumerable<ControlItem> Estilos { get; set; } = new List<ControlItem>();
                public IEnumerable<ControlItem> Estaciones { get; set; } = new List<ControlItem>();
                public IEnumerable<ControlItem> Colores { get; set; } = new List<ControlItem>();
                public IEnumerable<ControlItem> Categorias { get; set; } = new List<ControlItem>();
                public IEnumerable<ControlItem> Marcas { get; set; } = new List<ControlItem>();
            }
        }
    }
}
