namespace EstiloLibreFront.Servicios
{
    public interface IServicioCargador
    {
        event Action AlAbrir;
        event Action AlCerrar;

        void Abrir();
        void Cerrar();
    }

    public class ServicioCargador : IServicioCargador
    {
        public event Action AlAbrir;
        public event Action AlCerrar;

        public void Abrir()
        {
            AlAbrir?.Invoke();
        }

        public void Cerrar()
        {
            AlCerrar?.Invoke();
        }
    }
}
