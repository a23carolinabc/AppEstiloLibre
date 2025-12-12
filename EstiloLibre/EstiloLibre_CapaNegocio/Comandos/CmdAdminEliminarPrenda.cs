using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    
    /// Comando para que un administrador elimine una prenda de un usuario
    
    public class CmdAdminEliminarPrenda : ComandoBase, IRequest
    {
        public int PrendaId { get; set; }

        public CmdAdminEliminarPrenda(int iPrendaId) : base([Codigos.Permisos.MODERADOR, Codigos.Permisos.ADMIN])
        {
            this.PrendaId = iPrendaId;
        }
    }

    public class PcmdAdminEliminarPrenda : ProcesadorDeComandoBase, IRequestHandler<CmdAdminEliminarPrenda>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdAdminEliminarPrenda(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public Task Handle(CmdAdminEliminarPrenda comando, CancellationToken cancellationToken)
        {
            Prenda? prenda;
            Adjuntos adjuntos;
            PrendasConjuntos prendasConjuntos;
            List<int> conjuntosIds;
            Conjuntos conjuntos;

            try
            {
                this.con.BeginTrans();

                this.VericarPermisosAlMenosUno(comando);

                // Cargar la prenda
                prenda = this.con.CargarPrenda(comando.PrendaId);
                if (prenda is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Obtener IDs de conjuntos que usan esta prenda
                prendasConjuntos = this.con.CargarPrendasConjuntosPorPrenda(comando.PrendaId);
                conjuntosIds = prendasConjuntos.Select(pc => pc.ConjuntoId).Distinct().ToList();

                // Eliminar los conjuntos completos que usan esta prenda
                if (conjuntosIds.Any())
                {
                    conjuntos = this.con.CargarConjuntos(conjuntosIds);
                    foreach (Conjunto conjunto in conjuntos)
                    {
                        // Eliminar todas las relaciones de este conjunto con otras prendas
                        PrendasConjuntos relacionesConjunto;

                        relacionesConjunto = this.con.CargarPrendasConjuntosPorConjunto(conjunto.Id);
                        foreach (PrendaConjunto relacion in relacionesConjunto)
                        {
                            relacion.Eliminar();
                        }

                        // Eliminar adjuntos del conjunto
                        adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);
                        foreach (Adjunto adjunto in adjuntos)
                        {
                            this._servicioAlmacenamiento.EliminarArchivo(adjunto);
                            adjunto.Eliminar();
                        }

                        // Eliminar el conjunto
                        conjunto.Eliminar();
                    }
                }

                // Eliminar adjuntos físicos de la prenda
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prenda.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }

                // Eliminar prenda
                prenda.Eliminar();

                // Confirmar la transacción
                this.con.CommitTrans();

                return Task.FromResult(Unit.Value);
            }
            catch
            {
                // Fallo detectado. Deshacer transacción y relanzar la excepción
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}
