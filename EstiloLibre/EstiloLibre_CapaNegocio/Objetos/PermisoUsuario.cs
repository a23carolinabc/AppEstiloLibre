using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.PermisosUsuarios)]
    public class PermisoUsuario : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public int UsuarioId { get; set; }
        public int PermisoId { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public PermisoUsuario() : base() { }

        public PermisoUsuario(PermisosUsuariosDAO dao) : base(dao) { }

        #endregion
    }
}
