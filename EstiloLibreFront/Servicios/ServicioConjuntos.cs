using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos.Conjuntos;
using EstiloLibreFront.Objetos.Usuarios;
using System.Net.Http.Json;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioConjuntos : ServicioBase
    {
        #region ***** PROPIEDADES *****
        private string _urlSaveData = "api/Conjuntos/savedata";
        private string _urlShowData = "api/Conjuntos/showdata/";
        private string _urlAddNew = "api/Conjuntos/addnew";
        private string _urlDelete = "api/Conjuntos/delete/";
        private string _urlConjuntosUsuario = "api/Conjuntos/conjuntosUsuario/";
        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioConjuntos(IHttpClientFactory factoriaClientesHttp, ServicioDatosContexto servicioDatosContexto)
                : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<ConjuntoData?> AddNew()
        {
            ConjuntoData? conjuntoData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            conjuntoData = new ConjuntoData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlAddNew);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            conjuntoData = JsonSerializer.Deserialize<ConjuntoData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return conjuntoData;
        }

        public async Task<ConjuntoData?> ShowData(int iConjuntoId)
        {
            ConjuntoData? conjuntoData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            conjuntoData = new ConjuntoData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlShowData + iConjuntoId);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            conjuntoData = JsonSerializer.Deserialize<ConjuntoData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return conjuntoData;
        }

        public async Task<int> SaveData(ConjuntoDTO conjunto)
        {
            JsonElement datos;
            HttpResponseMessage respuestaHttp;
            int conjuntoId;

            //Serializar a json.
            datos = JsonSerializer.SerializeToElement<ConjuntoDTO>(conjunto);

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsJsonAsync(this._urlSaveData, datos);

            //Comprobar status 200.
            conjuntoId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            //Devolver ID de la conjunto creada.
            return conjuntoId;
        }

        public async Task<List<ConjuntoDTO>> GetConjuntosUsuario(int iConjuntoId)
        {
            List<ConjuntoDTO>? conjuntoData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            conjuntoData = new List<ConjuntoDTO>();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlConjuntosUsuario + iConjuntoId);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            conjuntoData = JsonSerializer.Deserialize<List<ConjuntoDTO>>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return conjuntoData??new();
        }
               
        public async Task<string> Delete(int iConjuntoId)
        {
            HttpResponseMessage respuestaHttp;
            string strRespuesta;

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .DeleteAsync(this._urlDelete + iConjuntoId);

            //Comprobar status 200.
            strRespuesta = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Devolver respuesta.
            return strRespuesta;
        }
        #endregion
    }
}