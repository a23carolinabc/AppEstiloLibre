using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;
using System.Data;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasExploracion.DTOs;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDPrendasPublicas : Vista
    {
        #region ***** PROPIEDADES *****

        public DataTable TablaPrendas { get; set; }
        public Estilos Estilos { get; set; }
        public Estaciones Estaciones { get; set; }
        public Colores Colores { get; set; }
        public Categorias Categorias { get; set; }
        public Marcas Marcas { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDPrendasPublicas(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            string strFiltro;

            strFiltro = "";

            return @$"
                SELECT p.*, 
                       u.Login as NombreUsuario,
                       u.Id as UsuarioId,
                       c.Nombre as CategoriaNombre, 
                       col.Nombre as ColorNombre, 
                       m.Nombre as MarcaNombre,
                       est.Nombre as EstacionNombre
                FROM {TablasBD.Prendas} p
                INNER JOIN {TablasBD.Usuarios} u ON p.UsuarioId = u.Id
                LEFT JOIN {TablasBD.Categorias} c ON p.CategoriaId = c.Id
                LEFT JOIN {TablasBD.Colores} col ON p.ColorId = col.Id
                LEFT JOIN {TablasBD.Marcas} m ON p.MarcaId = m.Id
                LEFT JOIN {TablasBD.Estaciones} est ON p.EstacionId = est.Id
                WHERE u.Publico = 1
                {strFiltro}
                ORDER BY p.Id DESC;

                SELECT * FROM {TablasBD.Estilos};

                SELECT * FROM {TablasBD.Estaciones};

                SELECT * FROM {TablasBD.Colores};

                SELECT * FROM {TablasBD.Categorias};

                SELECT * FROM {TablasBD.Marcas};
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { "PrendasPublicas", TablasBD.Estilos, TablasBD.Estaciones,
                                TablasBD.Colores, TablasBD.Categorias, TablasBD.Marcas };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(BusquedaPrendasPublicasDTO? parametrosBusqueda = null)
        {
            DataSet dataSet;

            // Agregar filtros según parámetros de búsqueda
            if (parametrosBusqueda != null && parametrosBusqueda.TipoBusqueda != null && parametrosBusqueda.ValorBusqueda != null)
            {
                switch (parametrosBusqueda.TipoBusqueda)
                {
                    case TipoBusquedaPrenda.Categoria:
                        this.AgregarParametro("iCategoriaId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                    case TipoBusquedaPrenda.Color:
                        this.AgregarParametro("iColorId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                    case TipoBusquedaPrenda.Estacion:
                        this.AgregarParametro("iEstacionId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                    case TipoBusquedaPrenda.Marca:
                        this.AgregarParametro("iMarcaId", parametrosBusqueda.ValorBusqueda.Value, MySqlDbType.Int32);
                        break;
                }
            }

            // Ejecutar consulta
            dataSet = this.EjecutarConsulta();

            // Mapear colecciones
            this.TablaPrendas = this.GetTabla("PrendasPublicas") ?? new();
            this.Estilos = new Estilos(this.MapearLista<Estilo>(TablasBD.Estilos));
            this.Estaciones = new Estaciones(this.MapearLista<Estacion>(TablasBD.Estaciones));
            this.Colores = new Colores(this.MapearLista<Color>(TablasBD.Colores));
            this.Categorias = new Categorias(this.MapearLista<Categoria>(TablasBD.Categorias));
            this.Marcas = new Marcas(this.MapearLista<Marca>(TablasBD.Marcas));
        }

        #endregion
    }
}
