using System.ComponentModel.DataAnnotations;
using EstiloLibreFront.Validaciones;

namespace EstiloLibreFront.Objetos.Prendas
{
    public class PrendaDTO
    {
        public int Id { get; set; }

        [IdObligatorio]
        public int ColorId { get; set; }

        [IdObligatorio] 
        public int CategoriaId { get; set; }

        [IdObligatorio]
        public int EstadoId { get; set; }

        [IdObligatorio]
        public int TallaId { get; set; }

        [IdObligatorio]
        public int MaterialId { get; set; }

        public int? MarcaId { get; set; }
        public int? EstacionId { get; set; }

        public decimal? Precio { get; set; }

        public string? EnlaceCompra { get; set; }

        public DateTime? FechaCompra { get; set; }

        [CampoObligatorio]
        public string ImagenBase64 { get; set; } = string.Empty;    
    }
}
