namespace EstiloLibreFront
{
    public class Idioma
    {
        public string Nombre { get; set; }
        public string Cultura { get; set; }

        public Idioma(string nombre, string cultura)
        {
            Nombre = nombre;
            Cultura = cultura;
        }
    }

    public class Idiomas
    {
        public static Idioma Predeterminado = new Idioma("gallego", "gl-ES");

        public static readonly List<Idioma> ListaIdiomas = new List<Idioma>()
        {
            new Idioma("gallego", "gl-ES"),
            new Idioma("español (España)", "es-ES")
        };
    }
}
