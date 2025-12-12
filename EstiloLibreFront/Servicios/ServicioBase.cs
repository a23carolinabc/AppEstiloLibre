using EstiloLibreFront.Servicios;
using System.Net;
using System.Net.Http.Json;

namespace EstiloLibreFront.Base
{
    public class ServicioBase
    {
        #region ***** PROPIEDADES *****

        protected readonly IHttpClientFactory _factoriaClientesHttp;
        protected readonly ServicioDatosContexto _servicioDatosContexto;

        #endregion

        #region ***** CONSTRUCTOR *****

        public ServicioBase(IHttpClientFactory factoriaClientesHttp, ServicioDatosContexto servicioDatosContexto)
        {
            this._factoriaClientesHttp = factoriaClientesHttp;
            this._servicioDatosContexto = servicioDatosContexto;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        protected T ProcesarRespuestaTexto<T>(HttpResponseMessage respuestaServidor)
        {
            T resultado;
            RespuestaError? respuestaError;

            respuestaError = null;

            if (respuestaServidor.IsSuccessStatusCode)
            {
                resultado = (T)Convert.ChangeType(respuestaServidor.Content.ReadAsStringAsync().Result, typeof(T));
                return resultado;
            }
            else if (respuestaServidor.StatusCode == HttpStatusCode.InternalServerError)
            {
                try
                {
                    respuestaError = respuestaServidor.Content.ReadFromJsonAsync<RespuestaError>().Result;
                }
                catch
                {
                }
                if (respuestaError != null)
                {
                    throw new ApplicationException(respuestaError.Mensaje);
                }
            }
            else if (respuestaServidor.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ApplicationException("ERR_Servidor_403_Prohibido");
            }
            throw new ApplicationException("ERR_Servidor");
        }

        protected async Task<string> ProcesarRespuestaFichero(HttpResponseMessage respuestaServidor)
        {
            RespuestaError? respuestaError;

            respuestaError = null;

            if (respuestaServidor.IsSuccessStatusCode)
            {
                var contenido = await respuestaServidor.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(contenido);
                return base64;
            }
            else if (respuestaServidor.StatusCode == HttpStatusCode.InternalServerError)
            {
                try
                {
                    respuestaError = respuestaServidor.Content.ReadFromJsonAsync<RespuestaError>().Result;
                }
                catch
                {
                }
                if (respuestaError != null)
                {
                    throw new ApplicationException(respuestaError.Mensaje);
                }
            }
            else if (respuestaServidor.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ApplicationException("ERR_Servidor_403_Prohibido");
            }
            throw new ApplicationException("ERR_Servidor");
        }

        public string GenerarStringDescarga(string contenido, string strNombreFichero, HttpResponseMessage respuestaServidor, string strExtension)
        {
            var mimeType = respuestaServidor.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

            string js = $"var link = document.createElement('a');" +
                 $"link.href = 'data:{mimeType};base64,{contenido}';" +
                 $"link.download = '" + strNombreFichero + $"{strExtension}';" +
                 $"link.click();";

            return js;
        }
        #endregion

        #region ***** CLASES INTERNAS *****

        protected class RespuestaError
        {
            public required string Mensaje { get; set; }
        }

        #endregion
    }
}
