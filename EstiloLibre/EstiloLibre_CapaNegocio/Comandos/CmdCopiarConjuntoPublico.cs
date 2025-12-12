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
    /// Comando para copiar un conjunto público al armario del usuario
    /// Copia el conjunto y todas sus prendas asociadas
    /// </summary>
    public class CmdCopiarConjuntoPublico : ComandoBase, IRequest<int>
    {
        public int ConjuntoId { get; set; }

        public CmdCopiarConjuntoPublico(int conjuntoId) : base()
        {
            this.ConjuntoId = conjuntoId;
        }
    }

    public class PcmdCopiarConjuntoPublico : ProcesadorDeComandoBase, IRequestHandler<CmdCopiarConjuntoPublico, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdCopiarConjuntoPublico(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdCopiarConjuntoPublico comando, CancellationToken cancellationToken)
        {
            Conjunto? conjuntoOriginal;
            Conjunto conjuntoNuevo;
            Usuario? usuarioPropietario;
            PrendasConjuntos prendasConjuntosOriginales;
            Dictionary<int, int> mapaPrendas; // Mapeo de ID original -> ID copia
            Adjuntos adjuntosConjuntoOriginal;
            Adjunto adjuntoNuevo;
            byte[] byImagen;

            try
            {
                // Iniciar transacción
                this.con.BeginTrans();

                // Cargar conjunto original
                conjuntoOriginal = this.con.CargarConjunto(comando.ConjuntoId);
                if (conjuntoOriginal == null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Verificar que el conjunto sea de un usuario público
                usuarioPropietario = this.con.CargarUsuario(conjuntoOriginal.UsuarioId);
                if (usuarioPropietario == null || !usuarioPropietario.Publico)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ConjuntoNoPublico");
                }

                // Inicializar diccionario para mapear prendas
                mapaPrendas = new Dictionary<int, int>();

                // Copiar todas las prendas del conjunto
                prendasConjuntosOriginales = this.con.CargarPrendasConjuntos(conjuntoOriginal.Id);
                foreach (PrendaConjunto prendaConjunto in prendasConjuntosOriginales)
                {
                    Prenda? prendaOriginal;
                    Prenda prendaNueva;
                    Adjuntos adjuntosPrendaOriginal;
                    int iPrendaIdOriginal;
                    int iPrendaIdNueva;

                    iPrendaIdOriginal = prendaConjunto.PrendaId;

                    // Verificar si ya copiamos esta prenda
                    if (!mapaPrendas.ContainsKey(iPrendaIdOriginal))
                    {
                        // Cargar prenda original
                        prendaOriginal = this.con.CargarPrenda(iPrendaIdOriginal);
                        if (prendaOriginal != null)
                        {
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

                            iPrendaIdNueva = prendaNueva.Id;

                            // Copiar imagen si existe
                            adjuntosPrendaOriginal = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, iPrendaIdOriginal);
                            if (adjuntosPrendaOriginal != null && adjuntosPrendaOriginal.Any())
                            {
                                Adjunto adjuntoPrendaOriginal;
                                Adjunto adjuntoPrendaNueva;
                                byte[] byImagenPrenda;

                                adjuntoPrendaOriginal = adjuntosPrendaOriginal.First();

                                // Leer imagen original
                                byImagenPrenda = await this._servicioAlmacenamiento.ObtenerContenidoArchivo(adjuntoPrendaOriginal);

                                // Crear nuevo adjunto
                                adjuntoPrendaNueva = this.con.CrearAdjunto();
                                adjuntoPrendaNueva.ClaseObjetoId = Codigos.ClasesObjetos.Prenda;
                                adjuntoPrendaNueva.ObjetoId = iPrendaIdNueva;
                                adjuntoPrendaNueva.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                                adjuntoPrendaNueva.Guid = UtilsVarios.GenerarGuid();

                                // Guardar adjunto
                                adjuntoPrendaNueva.Guardar();

                                // Guardar archivo físico
                                await this._servicioAlmacenamiento.GuardarArchivo(adjuntoPrendaNueva, byImagenPrenda);
                            }

                            // Agregar al mapa
                            mapaPrendas.Add(iPrendaIdOriginal, iPrendaIdNueva);
                        }
                    }
                }

                // Crear nuevo conjunto copiando propiedades
                conjuntoNuevo = this.con.CrearConjunto();
                conjuntoNuevo.EstacionId = conjuntoOriginal.EstacionId;
                conjuntoNuevo.EstiloId = conjuntoOriginal.EstiloId;
                conjuntoNuevo.Descripcion = conjuntoOriginal.Descripcion;
                conjuntoNuevo.EsFavorito = false; // Por defecto no es favorito
                conjuntoNuevo.DatosComposicion = conjuntoOriginal.DatosComposicion;
                conjuntoNuevo.NotasPersonales = null; // Las notas no se copian
                conjuntoNuevo.UsuarioId = this.con.UsuarioAutenticado.Id;

                // Guardar nuevo conjunto
                conjuntoNuevo.Guardar();

                // Crear relaciones con las prendas copiadas
                foreach (var parMapaPrenda in mapaPrendas)
                {
                    PrendaConjunto prendaConjuntoNueva;
                    int iPrendaIdNueva;

                    iPrendaIdNueva = parMapaPrenda.Value;

                    prendaConjuntoNueva = this.con.CrearPrendaConjunto();
                    prendaConjuntoNueva.ConjuntoId = conjuntoNuevo.Id;
                    prendaConjuntoNueva.PrendaId = iPrendaIdNueva;
                    prendaConjuntoNueva.Guardar();
                }

                // Copiar imagen del conjunto si existe
                adjuntosConjuntoOriginal = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjuntoOriginal.Id);
                if (adjuntosConjuntoOriginal != null && adjuntosConjuntoOriginal.Any())
                {
                    Adjunto adjuntoConjuntoOriginal;

                    adjuntoConjuntoOriginal = adjuntosConjuntoOriginal.First();

                    // Leer imagen original
                    byImagen = await this._servicioAlmacenamiento.ObtenerContenidoArchivo(adjuntoConjuntoOriginal);

                    // Crear nuevo adjunto
                    adjuntoNuevo = this.con.CrearAdjunto();
                    adjuntoNuevo.ClaseObjetoId = Codigos.ClasesObjetos.Conjunto;
                    adjuntoNuevo.ObjetoId = conjuntoNuevo.Id;
                    adjuntoNuevo.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjuntoNuevo.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto
                    adjuntoNuevo.Guardar();

                    // Guardar archivo físico
                    await this._servicioAlmacenamiento.GuardarArchivo(adjuntoNuevo, byImagen);
                }

                // Confirmar transacción
                this.con.CommitTrans();

                return conjuntoNuevo.Id;
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
