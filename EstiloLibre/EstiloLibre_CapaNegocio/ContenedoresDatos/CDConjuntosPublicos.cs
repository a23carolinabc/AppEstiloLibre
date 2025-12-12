using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;
using System.Data;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasExploracion.DTOs;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDConjuntosPublicos : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuarios Usuarios { get; set; }
        public DataTable TablaConjuntos { get; set; }
        public Estilos Estilos { get; set; }
        public Estaciones Estaciones { get; set; }
        public Colores Colores { get; set; }
        public Categorias Categorias { get; set; }
        public Marcas Marcas { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDConjuntosPublicos(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            string strFiltro;

            strFiltro = "";

            return @$"
                SELECT co.*, 
                       u.Login as NombreUsuario,
                       u.Id as UsuarioId,
                       e.Nombre as EstiloNombre,
                       est.Nombre as EstacionNombre,
                       (SELECT COUNT(*) FROM {TablasBD.PrendasConjuntos} pc WHERE pc.ConjuntoId = co.Id) as CantidadPrendas
                FROM {TablasBD.Conjuntos} co
                INNER JOIN {TablasBD.Usuarios} u ON co.UsuarioId = u.Id
                LEFT JOIN {TablasBD.Estilos} e ON co.EstiloId = e.Id
                LEFT JOIN {TablasBD.Estaciones} est ON co.EstacionId = est.Id
                WHERE u.Publico = 1
                {strFiltro}
                ORDER BY co.Id DESC;

                SELECT * FROM {TablasBD.Estilos};

                SELECT * FROM {TablasBD.Estaciones};

                SELECT * FROM {TablasBD.Colores};

                SELECT * FROM {TablasBD.Categorias};

                SELECT * FROM {TablasBD.Marcas};
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { "ConjuntosPublicos", TablasBD.Estilos, TablasBD.Estaciones,
                                TablasBD.Colores, TablasBD.Categorias, TablasBD.Marcas };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(BusquedaConjuntosPublicosDTO? parametrosBusqueda = null)
        {
            DataSet dataSet;

            // Agregar filtros según parámetros de búsqueda
            if (parametrosBusqueda != null && parametrosBusqueda.TipoBusqueda != null && parametrosBusqueda.ValorBusqueda != null)
            {
                switch (parametrosBusqueda.TipoBusqueda)
                {
                    case TipoBusquedaConjunto.Estilo:
                        this.AgregarParametro("iEstiloId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                    case TipoBusquedaConjunto.Estacion:
                        this.AgregarParametro("iEstacionId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                    case TipoBusquedaConjunto.Color:
                        this.AgregarParametro("iColorId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                }
            }

            // Ejecutar consulta
            dataSet = this.EjecutarConsulta();

            // Mapear colecciones
            this.TablaConjuntos = this.GetTabla("ConjuntosPublicos")??new();
            this.Estilos = new Estilos(this.MapearLista<Estilo>(TablasBD.Estilos));
            this.Estaciones = new Estaciones(this.MapearLista<Estacion>(TablasBD.Estaciones));
            this.Colores = new Colores(this.MapearLista<Color>(TablasBD.Colores));
            this.Categorias = new Categorias(this.MapearLista<Categoria>(TablasBD.Categorias));
            this.Marcas = new Marcas(this.MapearLista<Marca>(TablasBD.Marcas));
        }

        #endregion
    }
}
