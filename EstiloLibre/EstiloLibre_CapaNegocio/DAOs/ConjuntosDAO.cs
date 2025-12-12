using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class ConjuntosDAO : DAO<Conjunto>
    {
        #region ***** CONSTRUCTORES *****
        public ConjuntosDAO(Conexion conexion) : base(conexion, TablasBD.Conjuntos) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new Conjunto(this);
        }
        public Conjunto? CargarConjunto(int iConjuntoId)
        {
            return (Conjunto?)this.CargarObjetoBD(iConjuntoId);
        }

        public Conjuntos CargarConjuntos(int iUsuarioId)
        {
            Conjuntos conjuntos;

            conjuntos = new(this.CargarObjetosBD($"UsuarioId = {iUsuarioId}"));

            return conjuntos;
        }

        public Conjuntos CargarConjuntos(List<int> lsIds)
        {
            Conjuntos conjuntos;

            conjuntos = new(this.CargarObjetosBD(lsIds));

            return conjuntos;
        }
        #endregion
    }    
}
