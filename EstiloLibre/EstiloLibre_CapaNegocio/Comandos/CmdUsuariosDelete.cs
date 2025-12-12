using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public class CmdUsuariosDelete 
        :ComandoBase, IRequest
    {
        public int UsuarioId { get; set; }
        public CmdUsuariosDelete(int iUsuarioId) : base()
        {
            this.UsuarioId = iUsuarioId;
        }
    }

    public class PcmdUsuariosDelete 
        : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosDelete>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdUsuariosDelete(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        public Task Handle(CmdUsuariosDelete comando, CancellationToken cancellationToken)
        {
            Usuario? Usuario;
            Adjuntos adjuntos;

            try
            {
                con.BeginTrans();

                Usuario = con.CargarUsuario(comando.UsuarioId);
                if (Usuario is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }


                //Eliminar permisos
                this.con.EliminarPermisosDeUsuario(Usuario.Id);

                //Eliminar usuario.
                Usuario.Eliminar();

                //Confirmar la transacción.
                con.CommitTrans();

                //Devolver el resultado de la ejecución del comando.
                return Task.FromResult(Unit.Value);
            }
            catch
            {
                //Fallo detectado. Deshacer transacción y relanzar la excepción.
                con.RollBackTrans();
                throw;
            }    
        }   
    }
}
