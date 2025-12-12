using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasModeracion.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasModeracion
    {
        #region ***** PROPIEDADES INTERNAS *****

        private readonly Conexion _con;
        private readonly UsuariosDAO _dao;
        private readonly ServicioCombos _servicioCombos;
        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTOR *****

        public ConsultasModeracion(Conexion con,
                                       ServicioCombos servicioCombos,
                                       ServicioAlmacenamiento servicioAlmacenamiento)
        {
            this._con = con;
            this._dao = new UsuariosDAO(con);
            this._servicioCombos = servicioCombos;
            this._servicioAlmacenamiento = servicioAlmacenamiento;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<IEnumerable<UsuarioNormalResumenDTO>> GetListadoUsuarios(string? strTextoBusqueda, string? strTipoBusqueda)
        {
            CDUsuariosListado cd;
            List<UsuarioNormalResumenDTO> lista;

            if (!this._con.UsuarioActualCumplePermisos([Codigos.Permisos.ADMIN]))
            {
                if (!this._con.UsuarioActualCumplePermisos([Codigos.Permisos.MODERADOR]))
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_NoAutorizado");
                }
            }

            cd = new CDUsuariosListado(this._con);
            cd.Cargar(strTextoBusqueda, strTipoBusqueda);

            lista = await this.GetDatosParaListado(cd);

            return lista;
        }

        public async Task<UsuarioNormalShowDataDTO> GetDatosDetalleUsuario(int iUsuarioId)
        {
            CDUsuariosDetalle cd;
            UsuarioNormalShowDataDTO dto;

            if (!this._con.UsuarioActualCumplePermisos([Codigos.Permisos.ADMIN]))
            {
                if (!this._con.UsuarioActualCumplePermisos([Codigos.Permisos.MODERADOR]))
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_NoAutorizado");
                }
            }

            cd = new CDUsuariosDetalle(this._con);
            cd.Cargar(iUsuarioId);

            dto = await this.GetDatosParaShowData(cd);
            return dto;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private async Task<UsuarioNormalShowDataDTO> GetDatosParaShowData(CDUsuariosDetalle cd)
        {
            UsuarioNormalShowDataDTO objeto;
            List<PrendaAdminDTO> prendasDTO;
            List<ConjuntoAdminDTO> conjuntosDTO;
            Adjuntos adjuntos;

            objeto = new UsuarioNormalShowDataDTO();
            objeto.Usuario = new UsuarioNormalDTO(cd.Usuario);
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);


            // Procesar prendas
            prendasDTO = new List<PrendaAdminDTO>();
            foreach (PrendaAdminDTO prenda in cd.Prendas)
            {
                // Cargar imagen de la prenda
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prenda.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    prenda.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                prendasDTO.Add(prenda);
            }
            objeto.Prendas = prendasDTO;


            // Procesar conjuntos
            conjuntosDTO = new List<ConjuntoAdminDTO>();
            foreach (ConjuntoAdminDTO conjunto in cd.Conjuntos)
            {
                // Cargar imagen del conjunto
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    conjunto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                conjuntosDTO.Add(conjunto);
            }            
            objeto.Conjuntos = conjuntosDTO;

            return objeto;
        }

        private async Task<List<UsuarioNormalResumenDTO>> GetDatosParaListado(CDUsuariosListado cd)
        {
            List<UsuarioNormalResumenDTO> lista;
            Adjuntos adjuntos;
            Prendas prendas;
            Conjuntos conjuntos;

            lista = new List<UsuarioNormalResumenDTO>();

            foreach (Usuario usuario in cd.Usuarios)
            {
                UsuarioNormalResumenDTO dto;
                dto = new UsuarioNormalResumenDTO(usuario);

                // Contar prendas del usuario
                prendas = this._con.CargarPrendas(usuario.Id);
                dto.CantidadPrendas = prendas.Count();

                // Contar conjuntos del usuario
                conjuntos = this._con.CargarConjuntos(usuario.Id);
                dto.CantidadConjuntos = conjuntos.Count();

                // Cargar imagen del usuario
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, usuario.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    dto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                lista.Add(dto);
            }

            return lista;
        }

        #endregion
    }
}
