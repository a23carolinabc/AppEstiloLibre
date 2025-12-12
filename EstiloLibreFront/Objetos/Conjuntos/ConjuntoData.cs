using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Objetos.Conjuntos
{
    public class ConjuntoData
    {
        public ConjuntoDTO? Conjunto { get; set; }
        public List<ControlItem> Estaciones { get; set; }
        public List<ControlItem> Estilos { get; set; }
        public List<ControlItem> Colores { get; set; }
    }
}
