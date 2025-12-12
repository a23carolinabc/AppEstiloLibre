using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using System.Data;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasExploracion.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasExploracion
    {
        #region ***** PROPIEDADES *****

        private readonly Conexion _con;
        private readonly ServicioCombos _servicioCombos;
        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ConsultasExploracion(Conexion con)
        {
            this._con = con;
            this._servicioCombos = new ServicioCombos();
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        /// <summary>
        /// Obtiene datos necesarios para los filtros de búsqueda
        /// </summary>
        public DatosExploracionDTO GetDatosExploracion()
        {
            CDConjuntosPublicos cd;
            DatosExploracionDTO objeto;

            cd = new CDConjuntosPublicos(this._con);
            cd.Cargar();

            objeto = new DatosExploracionDTO();
            objeto.Estilos = this._servicioCombos.GetListaElementosCombo(cd.Estilos, true, o => o.Id, o => o.Nombre);
            objeto.Estaciones = this._servicioCombos.GetListaElementosCombo(cd.Estaciones, true, o => o.Id, o => o.Nombre);
            objeto.Colores = this._servicioCombos.GetListaElementosCombo(cd.Colores, true, o => o.Id, o => o.Nombre);
            objeto.Categorias = this._servicioCombos.GetListaElementosCombo(cd.Categorias, true, o => o.Id, o => o.Nombre);
            objeto.Marcas = this._servicioCombos.GetListaElementosCombo(cd.Marcas, true, o => o.Id, o => o.Nombre);

            return objeto;
        }

        /// <summary>
        /// Obtiene conjuntos públicos con filtros opcionales
        /// </summary>
        public async Task<List<ConjuntoPublicoResumenDTO>> GetConjuntosPublicos(BusquedaConjuntosPublicosDTO? parametrosBusqueda = null)
        {
            CDConjuntosPublicos cd;
            List<ConjuntoPublicoResumenDTO> lista;
            Adjuntos adjuntos;

            cd = new CDConjuntosPublicos(this._con);
            cd.Cargar(parametrosBusqueda);

            lista = new List<ConjuntoPublicoResumenDTO>();

            if (cd.TablaConjuntos != null && cd.TablaConjuntos.Rows.Count > 0)
            {
                foreach (DataRow fila in cd.TablaConjuntos.Rows)
                {
                    ConjuntoPublicoResumenDTO dto;
                    int iConjuntoId;

                    dto = new ConjuntoPublicoResumenDTO
                    {
                        Id = Convert.ToInt32(fila["Id"]),
                        UsuarioId = Convert.ToInt32(fila["UsuarioId"]),
                        NombreUsuario = fila["NombreUsuario"].ToString() ?? string.Empty,
                        EstiloNombre = fila["EstiloNombre"] != DBNull.Value ? fila["EstiloNombre"].ToString() : null,
                        EstacionNombre = fila["EstacionNombre"] != DBNull.Value ? fila["EstacionNombre"].ToString() : null,
                        Descripcion = fila["Descripcion"] != DBNull.Value ? fila["Descripcion"].ToString() : null,
                        CantidadPrendas = Convert.ToInt32(fila["CantidadPrendas"])
                    };

                    iConjuntoId = dto.Id;

                    // Cargar imagen del conjunto
                    adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, iConjuntoId);
                    if (adjuntos != null && adjuntos.Any())
                    {
                        dto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                    }

                    lista.Add(dto);
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene prendas públicas con filtros opcionales
        /// </summary>
        public async Task<List<PrendaPublicaResumenDTO>> GetPrendasPublicas(BusquedaPrendasPublicasDTO? parametrosBusqueda = null)
        {
            CDPrendasPublicas cd;
            List<PrendaPublicaResumenDTO> lista;
            Adjuntos adjuntos;

            cd = new CDPrendasPublicas(this._con);
            cd.Cargar(parametrosBusqueda);

            lista = new List<PrendaPublicaResumenDTO>();

            if (cd.TablaPrendas != null && cd.TablaPrendas.Rows.Count > 0)
            {
                foreach (DataRow fila in cd.TablaPrendas.Rows)
                {
                    PrendaPublicaResumenDTO dto;
                    int iPrendaId;

                    dto = new PrendaPublicaResumenDTO
                    {
                        Id = Convert.ToInt32(fila["Id"]),
                        UsuarioId = Convert.ToInt32(fila["UsuarioId"]),
                        NombreUsuario = fila["NombreUsuario"].ToString() ?? string.Empty,
                        CategoriaNombre = fila["CategoriaNombre"] != DBNull.Value ? fila["CategoriaNombre"].ToString() : null,
                        ColorNombre = fila["ColorNombre"] != DBNull.Value ? fila["ColorNombre"].ToString() : null,
                        MarcaNombre = fila["MarcaNombre"] != DBNull.Value ? fila["MarcaNombre"].ToString() : null,
                        EstacionNombre = fila["EstacionNombre"] != DBNull.Value ? fila["EstacionNombre"].ToString() : null
                    };

                    iPrendaId = dto.Id;

                    // Cargar imagen de la prenda
                    adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, iPrendaId);
                    if (adjuntos != null && adjuntos.Any())
                    {
                        dto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                    }

                    lista.Add(dto);
                }
            }

            return lista;
        }

        #endregion
    }
}
