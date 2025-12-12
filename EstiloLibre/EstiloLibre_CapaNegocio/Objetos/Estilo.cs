using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Estilos)]
    public class Estilo : ObjetoBD
    {
        #region ****** PROPIEDADES *****

        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Estilo() : base() { }

        public Estilo(DAO<Estilo> dao) : base(dao) { }

        #endregion
    }
}