using System.Text.Json;

namespace EstiloLibreFront.Objetos
{
    public class Canvas
    {
        public class PrendaEnCanvas
        {
            public int PrendaId { get; set; }
            public string? ImagenBase64 { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Ancho { get; set; }
            public double Alto { get; set; }
            public double Rotacion { get; set; }
            public int ZIndex { get; set; }
            public bool EstaSeleccionada { get; set; }

            public PrendaEnCanvas()
            {
                X = 0;
                Y = 0;
                Ancho = 150;
                Alto = 150;
                Rotacion = 0;
                ZIndex = 0;
                EstaSeleccionada = false;
            }
        }

        public class ComposicionCanvas
        {
            public double Ancho { get; set; }
            public double Alto { get; set; }
            public List<PrendaEnCanvas> Prendas { get; set; }

            public ComposicionCanvas()
            {
                Ancho = 800;
                Alto = 1000;
                Prendas = new List<PrendaEnCanvas>();
            }

            public string SerializarAJson()
            {
                string json;

                json = JsonSerializer.Serialize(this);
                return json;
            }

            public static ComposicionCanvas? DeserializarDesdeJson(string strJson)
            {
                ComposicionCanvas? composicion;

                if (string.IsNullOrWhiteSpace(strJson))
                {
                    return new ComposicionCanvas();
                }

                composicion = JsonSerializer.Deserialize<ComposicionCanvas>(strJson);
                return composicion ?? new ComposicionCanvas();
            }
        }
    }
}