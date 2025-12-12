using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Base;

public class ProcesadorDeComandoBase
{
    #region ***** PROPIEDADES *****

    protected Conexion con { get; private set; }

    #endregion

    #region ***** CONSTRUCTORES *****

    public ProcesadorDeComandoBase(Conexion conexion)
    {
        this.con = conexion;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    public void VericarPermisos(ComandoBase comando)
    {
        if (comando.CodigosPermisos == null || !comando.CodigosPermisos.Any())
        {
            throw new ReglaNegocioParaUsuarioException("ERR_SinPermisos");
        }

        if (!this.con.UsuarioActualCumplePermisos(comando.CodigosPermisos))
        {
            throw new ReglaNegocioParaUsuarioException("ERR_NoAutorizado");
        }
    }

    public void VericarPermisosAlMenosUno(ComandoBase comando)
    {
        if (comando.CodigosPermisos == null || !comando.CodigosPermisos.Any())
        {
            throw new ReglaNegocioParaUsuarioException("ERR_SinPermisos");
        }

        foreach (string permiso in comando.CodigosPermisos)
        {
            if (this.con.UsuarioAutenticado.Permisos.Contains(permiso))
            {
                return;
            }
        }
        throw new ReglaNegocioParaUsuarioException("ERR_NoAutorizado");
    }
    #endregion
}
