using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos.Administracion;
using System.Net.Http.Json;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioUsuariosAdmin : ServicioBase
    {
        #region ***** PROPIEDADES *****

        private readonly string _urlAddNew = "api/UsuariosAdmin/addnew";
        private readonly string _urlShowData = "api/UsuariosAdmin/showdata/";
        private readonly string _urlSaveData = "api/UsuariosAdmin/savedata";
        private readonly string _urlDelete = "api/UsuariosAdmin/delete/";
        private readonly string _urlListado = "api/UsuariosAdmin/listado";

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioUsuariosAdmin(IHttpClientFactory factoriaClientesHttp,
                                     ServicioDatosContexto servicioDatosContexto)
            : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        /// <summary>
        /// Obtiene los datos necesarios para crear un nuevo usuario administrador
        /// </summary>
        public async Task<UsuarioAdminData?> AddNew()
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(this._urlAddNew);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<UsuarioAdminData>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Obtiene los datos completos de un usuario administrador para edición
        /// </summary>
        public async Task<UsuarioAdminData?> ShowData(int iUsuarioId)
        {
            HttpResponseMessage respuestaHttp;
            string strDatos;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .GetAsync(this._urlShowData + iUsuarioId);

            // Comprobar status 200
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            // Deserializar y devolver respuesta
            return JsonSerializer.Deserialize<UsuarioAdminData>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Guarda o actualiza un usuario administrador
        /// </summary>
        public async Task<int> SaveData(UsuarioAdminSaveDataDTO usuario)
        {
            JsonElement datos;
            HttpResponseMessage respuestaHttp;
            int iObjetoId;

            // Serializar a json
            datos = JsonSerializer.SerializeToElement(usuario);

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsJsonAsync(this._urlSaveData, datos);

            // Comprobar status 200
            iObjetoId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            return iObjetoId;
        }

        /// <summary>
        /// Elimina un usuario administrador
        /// </summary>
        public async Task Delete(int iUsuarioId)
        {
            HttpResponseMessage respuestaHttp;

            // Enviar petición al servidor
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .DeleteAsync(this._urlDelete + iUsuarioId);

            // Comprobar status 200
            this.ProcesarRespuestaTexto<string>(respuestaHttp);
        }

        /// <summary>
        /// Obtiene un listado de usuarios administradores con búsqueda opcional
        /// </summary>
        public async Task<List<UsuarioAdminResumenDTO>> GetListado(string? textoBusqueda = null, string? tipoBusqueda = null)
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
            url = this._urlListado;
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
            return JsonSerializer.Deserialize<List<UsuarioAdminResumenDTO>>(strDatos,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new List<UsuarioAdminResumenDTO>();
        }

        #endregion
    }
}