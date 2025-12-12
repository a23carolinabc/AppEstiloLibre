using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos.Exploracion;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioExploracion : ServicioBase
    {
        #region ***** PROPIEDADES *****

        private readonly string _urlDatos = "api/Exploracion/datos";
        private readonly string _urlConjuntos = "api/Exploracion/conjuntos";
        private readonly string _urlPrendas = "api/Exploracion/prendas";
        private readonly string _urlCopiarPrenda = "api/Exploracion/copiar/prenda/";
        private readonly string _urlCopiarConjunto = "api/Exploracion/copiar/conjunto/";

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioExploracion(IHttpClientFactory factoriaClientesHttp, ServicioDatosContexto servicioDatosContexto)
            : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        /// <summary>
        /// Obtiene datos necesarios para filtros de búsqueda
        /// </summary>
        public async Task<DatosExploracionDTO> GetDatosExploracion()
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(this._urlDatos);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<DatosExploracionDTO>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
                ?? new DatosExploracionDTO();
        }

        /// <summary>
        /// Obtiene conjuntos públicos con filtros opcionales
        /// </summary>
        public async Task<List<ConjuntoPublicoResumenDTO>> GetConjuntosPublicos(int? tipoBusqueda = null, int? valorBusqueda = null)
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;
            string url;

            // Construir URL con parámetros
            url = this._urlConjuntos;

            if (tipoBusqueda.HasValue && valorBusqueda.HasValue)
            {
                url += $"?tipoBusqueda={tipoBusqueda.Value}&valorBusqueda={valorBusqueda.Value}";
            }

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(url);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<List<ConjuntoPublicoResumenDTO>>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
                ?? new List<ConjuntoPublicoResumenDTO>();
        }

        /// <summary>
        /// Obtiene prendas públicas con filtros opcionales
        /// </summary>
        public async Task<List<PrendaPublicaResumenDTO>> GetPrendasPublicas(int? tipoBusqueda = null, int? valorBusqueda = null)
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;
            string url;

            // Construir URL con parámetros
            url = this._urlPrendas;

            if (tipoBusqueda.HasValue && valorBusqueda.HasValue)
            {
                url += $"?tipoBusqueda={tipoBusqueda.Value}&valorBusqueda={valorBusqueda.Value}";
            }

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(url);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<List<PrendaPublicaResumenDTO>>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
                ?? new List<PrendaPublicaResumenDTO>();
        }

        /// <summary>
        /// Copia una prenda pública al armario del usuario
        /// </summary>
        public async Task<int> CopiarPrendaPublica(int prendaId)
        {
            HttpResponseMessage respuestaHttp;
            int iNuevaPrendaId;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsync(this._urlCopiarPrenda + prendaId, null);

            // Comprobar status 200
            iNuevaPrendaId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            return iNuevaPrendaId;
        }

        /// <summary>
        /// Copia un conjunto público (con sus prendas) al armario del usuario
        /// </summary>
        public async Task<int> CopiarConjuntoPublico(int conjuntoId)
        {
            HttpResponseMessage respuestaHttp;
            int iNuevoConjuntoId;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsync(this._urlCopiarConjunto + conjuntoId, null);

            // Comprobar status 200
            iNuevoConjuntoId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            return iNuevoConjuntoId;
        }

        #endregion
    }
}
