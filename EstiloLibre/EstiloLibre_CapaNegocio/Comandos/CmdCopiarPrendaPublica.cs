using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    /// <summary>
    /// Comando para copiar una prenda pública al armario del usuario
    /// </summary>
    public class CmdCopiarPrendaPublica : ComandoBase, IRequest<int>
    {
        public int PrendaId { get; set; }

        public CmdCopiarPrendaPublica(int prendaId) : base()
        {
            this.PrendaId = prendaId;
        }
    }

    public class PcmdCopiarPrendaPublica : ProcesadorDeComandoBase, IRequestHandler<CmdCopiarPrendaPublica, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdCopiarPrendaPublica(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdCopiarPrendaPublica comando, CancellationToken cancellationToken)
        {
            Prenda? prendaOriginal;
            Prenda prendaNueva;
            Usuario? usuarioPropietario;
            Adjuntos adjuntosOriginales;
            Adjunto adjuntoNuevo;
            byte[] byImagen;

            try
            {
                // Iniciar transacción
                this.con.BeginTrans();

                // Cargar prenda original
                prendaOriginal = this.con.CargarPrenda(comando.PrendaId);
                if (prendaOriginal == null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Verificar que la prenda sea de un usuario público
                usuarioPropietario = this.con.CargarUsuario(prendaOriginal.UsuarioId);
                if (usuarioPropietario == null || !usuarioPropietario.Publico)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_PrendaNoPublica");
                }

                // Crear nueva prenda copiando propiedades
                prendaNueva = this.con.CrearPrenda();
                prendaNueva.ColorId = prendaOriginal.ColorId;
                prendaNueva.CategoriaId = prendaOriginal.CategoriaId;
                prendaNueva.EstadoId = prendaOriginal.EstadoId;
                prendaNueva.TallaId = prendaOriginal.TallaId;
                prendaNueva.MaterialId = prendaOriginal.MaterialId;
                prendaNueva.MarcaId = prendaOriginal.MarcaId;
                prendaNueva.EstacionId = prendaOriginal.EstacionId;
                prendaNueva.Precio = prendaOriginal.Precio;
                prendaNueva.EnlaceCompra = prendaOriginal.EnlaceCompra;
                prendaNueva.FechaCompra = prendaOriginal.FechaCompra;
                prendaNueva.UsuarioId = this.con.UsuarioAutenticado.Id;

                // Guardar nueva prenda
                prendaNueva.Guardar();

                // Copiar imagen si existe
                adjuntosOriginales = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prendaOriginal.Id);
                if (adjuntosOriginales != null && adjuntosOriginales.Any())
                {
                    Adjunto adjuntoOriginal;

                    adjuntoOriginal = adjuntosOriginales.First();

                    // Leer imagen original
                    byImagen = await this._servicioAlmacenamiento.ObtenerContenidoArchivo(adjuntoOriginal);

                    // Crear nuevo adjunto para la copia
                    adjuntoNuevo = this.con.CrearAdjunto();
                    adjuntoNuevo.ClaseObjetoId = Codigos.ClasesObjetos.Prenda;
                    adjuntoNuevo.ObjetoId = prendaNueva.Id;
                    adjuntoNuevo.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjuntoNuevo.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto
                    adjuntoNuevo.Guardar();

                    // Guardar archivo físico
                    await this._servicioAlmacenamiento.GuardarArchivo(adjuntoNuevo, byImagen);
                }

                // Confirmar transacción
                this.con.CommitTrans();

                return prendaNueva.Id;
            }
            catch
            {
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}
