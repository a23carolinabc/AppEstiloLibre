namespace EstiloLibreFront.Objetos.Conjuntos
{
    public class ConjuntoDTO
    {
        public int Id { get; set; }
        public int? EstacionId { get; set; }
        public int? EstiloId { get; set; }
        public string? Descripcion { get; set; }
        public bool EsFavorito { get; set; }
        public string DatosComposicion { get; set; }
        public string? NotasPersonales { get; set; }
        public string? ImagenBase64 { get; set; }
        public List<int>? PrendasIds { get; set; }
    }
}
