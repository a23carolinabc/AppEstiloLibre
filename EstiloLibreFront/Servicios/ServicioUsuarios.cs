using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos.Usuarios;
using System.Net.Http.Json;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioUsuarios : ServicioBase
    {
        #region ***** PROPIEDADES *****
        private string _urlSaveData = "api/Usuarios/savedata";
        private string _urlSaveDataNuevoUsuario = "api/Seguridad/login/savedata";
        private string _urlAddNew = "api/Seguridad/login/addnew";
        private string _urlShowData = "api/Usuarios/showdata/";
        private string _urlDelete = "api/Usuarios/delete/";
        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioUsuarios(IHttpClientFactory factoriaClientesHttp, ServicioDatosContexto servicioDatosContexto)
                : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
               
        public async Task<UsuarioData?> AddNew()
        {
            UsuarioData? usuarioDataDB;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            usuarioDataDB = new UsuarioData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlAddNew);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            usuarioDataDB = JsonSerializer.Deserialize<UsuarioData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return usuarioDataDB;
        }

        public async Task<UsuarioData?> ShowData(int iUsuarioId)
        {
            UsuarioData? usuarioDataDB;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            usuarioDataDB = new UsuarioData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlShowData + iUsuarioId);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            usuarioDataDB = JsonSerializer.Deserialize<UsuarioData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return usuarioDataDB;
        }

        public async Task<int> SaveData(UsuarioDTO usuario)
        {
            JsonElement datos;
            HttpResponseMessage respuestaHttp;
            int iObjetoId;

            //Serializar a json.
            datos = JsonSerializer.SerializeToElement<UsuarioDTO>(usuario);

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsJsonAsync(this._urlSaveData, datos);

            //Comprobar status 200.
            iObjetoId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            //Devolver respuesta.
            return iObjetoId;
        }

        public async Task<int> SaveDataNuevoUsuario(NuevoUsuarioDTO usuario)
        {
            JsonElement datos;
            HttpResponseMessage respuestaHttp;
            int iObjetoId;

            //Serializar a json.
            datos = JsonSerializer.SerializeToElement<NuevoUsuarioDTO>(usuario);

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsJsonAsync(this._urlSaveDataNuevoUsuario, datos);

            //Comprobar status 200.
            iObjetoId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            //Devolver respuesta.
            return iObjetoId;
        }

        public async Task<string> Delete(int idUsuario)
        {
            HttpResponseMessage respuestaHttp;
            string strRespuesta;

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .DeleteAsync(this._urlDelete + idUsuario);

            //Comprobar status 200.
            strRespuesta = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Devolver respuesta.
            return strRespuesta;
        }

        #endregion
    }
}