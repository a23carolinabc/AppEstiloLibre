using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Objetos.Prendas
{
    public class PrendaData
    {
        public PrendaDTO? Prenda { get; set; }
        public List<ControlItem> Estaciones { get; set; }
        public List<ControlItem> Marcas { get; set; }
        public List<ControlItem> Materiales { get; set; }
        public List<ControlItem> Tallas { get; set; }
        public List<ControlItem> Categorias { get; set; }
        public List<ControlItem> Colores { get; set; }
        public List<ControlItem> Estados { get; set; }
    }
}
