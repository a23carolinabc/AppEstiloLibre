using EstiloLibreFront.Base;
using EstiloLibreFront.Objetos;
using EstiloLibreFront.Objetos.Prendas;
using EstiloLibreFront.Objetos.Usuarios;
using System.Net.Http.Json;
using System.Text.Json;

namespace EstiloLibreFront.Servicios
{
    public class ServicioPrendas : ServicioBase
    {
        #region ***** PROPIEDADES *****
        private string _urlSaveData = "api/Prendas/savedata";
        private string _urlShowData = "api/Prendas/showdata/";
        private string _urlAddNew = "api/Prendas/addnew";
        private string _urlDelete = "api/Prendas/delete/";
        private string _urlPrendasUsuario = "api/Prendas/prendasUsuario/";
        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioPrendas(IHttpClientFactory factoriaClientesHttp, ServicioDatosContexto servicioDatosContexto)
                : base(factoriaClientesHttp, servicioDatosContexto)
        {
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
               
        public async Task<PrendaData?> AddNew()
        {
            PrendaData? prendaData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            prendaData = new PrendaData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlAddNew);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            prendaData = JsonSerializer.Deserialize<PrendaData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return prendaData;
        }

        public async Task<PrendaData?> ShowData(int iPrendaId)
        {
            PrendaData? prendaData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            prendaData = new PrendaData();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlShowData+iPrendaId);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            prendaData = JsonSerializer.Deserialize<PrendaData>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return prendaData;
        }

        public async Task<int> SaveData(PrendaDTO prenda)
        {
            JsonElement datos;
            HttpResponseMessage respuestaHttp;
            int prendaId;

            //Serializar a json.
            datos = JsonSerializer.SerializeToElement<PrendaDTO>(prenda);

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                .PostAsJsonAsync(this._urlSaveData, datos);

            //Comprobar status 200.
            prendaId = this.ProcesarRespuestaTexto<int>(respuestaHttp);

            //Devolver ID de la prenda creada.
            return prendaId;
        }

        public async Task<List<PrendaResumenDTO>> GetPrendasUsuario(int iConjuntoId)
        {
            List<PrendaResumenDTO>? conjuntoData;
            HttpResponseMessage respuestaHttp;
            string strDatos;

            conjuntoData = new List<PrendaResumenDTO>();

            //Enviar petición al servidor.
            respuestaHttp = await this._factoriaClientesHttp.CreateClient("API")
                    .GetAsync(this._urlPrendasUsuario + iConjuntoId);

            //Comprobar status 200.
            strDatos = this.ProcesarRespuestaTexto<string>(respuestaHttp);

            //Deserializar y devolver respuesta.
            conjuntoData = JsonSerializer.Deserialize<List<PrendaResumenDTO>>(strDatos, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return conjuntoData ?? new();
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