using System.Timers;

namespace EstiloLibreFront.Servicios
{
    public class ServicioMensajeInfo : IDisposable
    {
        #region PROPIEDADES
        public event Action<string>? Mostrar;
        public event Action? Ocultar;
        private System.Timers.Timer? _temporizador;
        #endregion

        #region MÉTODOS
        public void TextoMensaje(string texto, int iDuracion = 3000)
        {
            Mostrar?.Invoke(texto);
            IniciarTemporizador(iDuracion);

        }
        public void OcultarMensaje(object? source, ElapsedEventArgs args)
        {
            Ocultar?.Invoke();
        }

        public void SetTemporizador(int iDuracion)
        {
            if (_temporizador != null)
            {
                return;
            }
            else
            {
                _temporizador = new System.Timers.Timer(iDuracion);
                _temporizador.Elapsed += OcultarMensaje;
                _temporizador.AutoReset = false;
            }
        }

        public void IniciarTemporizador(int iDuracion)
        {
            SetTemporizador(iDuracion);
            if(_temporizador!.Enabled)
            {
                _temporizador.Stop();
                _temporizador.Start();
            } else
            {
                _temporizador.Start();
            }
        }

        public void Dispose()
        {
            _temporizador?.Dispose();
        }
        #endregion
    }
}
