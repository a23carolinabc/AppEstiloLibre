using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasModeracion.DTOs;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosDetalle : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }
        public Idiomas Idiomas { get; set; }
        public List<PrendaAdminDTO> Prendas { get; set; }
        public List<ConjuntoAdminDTO> Conjuntos { get; set; }
        public DataTable TablaPrendas { get; set; }
        public DataTable TablaConjuntos { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosDetalle(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Usuarios}
                WHERE Id = @iUsuarioId;

                SELECT *
                FROM {TablasBD.Idiomas};

                SELECT p.*, 
                       c.Nombre as CategoriaNombre, 
                       col.Nombre as ColorNombre, 
                       m.Nombre as MarcaNombre
                FROM {TablasBD.Prendas} p
                LEFT JOIN {TablasBD.Categorias} c ON p.CategoriaId = c.Id
                LEFT JOIN {TablasBD.Colores} col ON p.ColorId = col.Id
                LEFT JOIN {TablasBD.Marcas} m ON p.MarcaId = m.Id
                WHERE p.UsuarioId = @iUsuarioId
                ORDER BY p.Id DESC;

                SELECT co.*, 
                       e.Nombre as EstiloNombre,
                       (SELECT COUNT(*) FROM {TablasBD.PrendasConjuntos} pc WHERE pc.ConjuntoId = co.Id) as CantidadPrendas
                FROM {TablasBD.Conjuntos} co
                LEFT JOIN {TablasBD.Estilos} e ON co.EstiloId = e.Id
                WHERE co.UsuarioId = @iUsuarioId
                ORDER BY co.Id DESC;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios, TablasBD.Idiomas, "PrendasConInfo", "ConjuntosConInfo" };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear objetos y colecciones básicas
            this.Usuario = this.MapearObjeto<Usuario>(TablasBD.Usuarios) ?? new();
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));

            this.Prendas = new List<PrendaAdminDTO>();
            if (this.TablaTieneDatos("PrendasConInfo"))
            {
                this.TablaPrendas = this.GetTabla("PrendasConInfo")!;
                                
                if (this.TablaPrendas != null && this.TablaPrendas.Rows.Count > 0)
                {
                    foreach (DataRow row in this.TablaPrendas.Rows)
                    {
                        PrendaAdminDTO prendaDTO;
                        int iPrendaId;

                        prendaDTO = new PrendaAdminDTO();
                        iPrendaId = UtilsConversion.GetInt(row["Id"]) ?? 0;
                        prendaDTO.Id = iPrendaId;
                        prendaDTO.CategoriaNombre = UtilsConversion.GetString(row["CategoriaNombre"]);
                        prendaDTO.ColorNombre = UtilsConversion.GetString(row["ColorNombre"]);
                        prendaDTO.MarcaNombre = UtilsConversion.GetString(row["MarcaNombre"]);


                        this.Prendas.Add(prendaDTO);
                    }
                }
            }

            this.Conjuntos = new List<ConjuntoAdminDTO>();
            if (this.TablaTieneDatos("ConjuntosConInfo"))
            {
                this.TablaConjuntos = this.GetTabla("ConjuntosConInfo")!;

                if (this.TablaConjuntos != null && this.TablaConjuntos.Rows.Count > 0)
                {
                    foreach (DataRow row in this.TablaConjuntos.Rows)
                    {
                        ConjuntoAdminDTO conjuntoDTO;
                        int iConjuntoId;

                        conjuntoDTO = new ConjuntoAdminDTO();
                        iConjuntoId = UtilsConversion.GetInt(row["Id"]) ?? 0;
                        conjuntoDTO.Id = iConjuntoId;
                        conjuntoDTO.Descripcion = UtilsConversion.GetString(row["Descripcion"]);
                        conjuntoDTO.EstilonNombre = UtilsConversion.GetString(row["EstiloNombre"]);
                        conjuntoDTO.EsFavorito = UtilsConversion.GetBool(row["EsFavorito"]);
                        conjuntoDTO.CantidadPrendas = UtilsConversion.GetInt(row["CantidadPrendas"]) ?? 0;

                        this.Conjuntos.Add(conjuntoDTO);
                    }
                }
            }
        }

        #endregion
    }
}
