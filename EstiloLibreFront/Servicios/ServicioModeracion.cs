using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos.Administracion;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioModeracion : ServicioBase
    {
        #region ***** PROPIEDADES *****

        private readonly string _urlListadoUsuariosNormales = "api/Moderacion/listado";
        private readonly string _urlShowDataUsuarioNormal = "api/Moderacion/usuario/showdata/";
        private readonly string _urlEliminarPrenda = "api/Moderacion/delete/prenda/";
        private readonly string _urlEliminarConjunto = "api/Moderacion/delete/conjunto/";

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioModeracion(IHttpClientFactory factoriaClientesHttp,
                                      ServicioDatosContexto servicioDatosContexto)
            : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        /// <summary>
        /// Obtiene un listado de usuarios normales con búsqueda opcional
        /// </summary>
        public async Task<List<UsuarioNormalResumenDTO>> GetListadoUsuariosNormales(string? textoBusqueda = null, string? tipoBusqueda = null)
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;
            string url;
            string? tipoBusquedaMapeado;

            // Mapear tipo de búsqueda de número a string
            tipoBusquedaMapeado = tipoBusqueda switch
            {
                "1" => "Nombre",
                "2" => "Login",
                "3" => "CorreoE",
                _ => null
            };

            // Construir URL con parámetros de búsqueda
            url = this._urlListadoUsuariosNormales;
            if (!string.IsNullOrEmpty(textoBusqueda) || !string.IsNullOrEmpty(tipoBusquedaMapeado))
            {
                List<string> parametros;

                parametros = new List<string>();

                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    parametros.Add($"textoBusqueda={Uri.EscapeDataString(textoBusqueda)}");
                }

                if (!string.IsNullOrEmpty(tipoBusquedaMapeado))
                {
                    parametros.Add($"tipoBusqueda={Uri.EscapeDataString(tipoBusquedaMapeado)}");
                }

                url += "?" + string.Join("&", parametros);
            }

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(url);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<List<UsuarioNormalResumenDTO>>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new List<UsuarioNormalResumenDTO>();
        }

        /// <summary>
        /// Obtiene los datos completos de un usuario normal con sus prendas y conjuntos
        /// </summary>
        public async Task<UsuarioNormalShowDataDTO?> GetDatosUsuarioNormal(int iUsuarioId)
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(this._urlShowDataUsuarioNormal + iUsuarioId);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<UsuarioNormalShowDataDTO>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Elimina una prenda de un usuario normal
        /// </summary>
        public async Task EliminarPrenda(int iPrendaId)
        {
            HttpResponseMessage respuestaHttp;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .DeleteAsync(this._urlEliminarPrenda + iPrendaId);

            // Comprobar status 200
            this.ProcesarRespuestaTexto<string>(respuestaHttp);
        }

        /// <summary>
        /// Elimina un conjunto de un usuario normal
        /// </summary>
        public async Task EliminarConjunto(int iConjuntoId)
        {
            HttpResponseMessage respuestaHttp;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .DeleteAsync(this._urlEliminarConjunto + iConjuntoId);

            // Comprobar status 200
            this.ProcesarRespuestaTexto<string>(respuestaHttp);
        }

        #endregion
    }
}