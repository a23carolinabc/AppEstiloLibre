using EstiloLibreFront.Base;
using EstiloLibreFront.Excepciones;
using EstiloLibreFront.Objetos;
using EstiloLibreFront.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EstiloLibreFront.Servicios;

public class ServicioAutentificacion
    : AuthenticationStateProvider
{
    #region ***** PROPIEDADES *****

    private readonly string _strUrlAutenticar = "api/Seguridad/autenticar";
    private readonly IHttpClientFactory _factoriaClientesHttp;
    private readonly ServicioDatosContexto _srvDatosContexto;

    #endregion

    #region ***** CONSTRUCTOR *****

    public ServicioAutentificacion(
        IHttpClientFactory factoriaClientesHttp,
        ServicioDatosContexto srvDatosContexto)
    {
        this._factoriaClientesHttp = factoriaClientesHttp;
        this._srvDatosContexto = srvDatosContexto;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        UtilsTokenJwt.Token? tokenDecodificado;

        tokenDecodificado = this._srvDatosContexto.GetTokenDecodificado();

        //Si no hay token almacenado, entonces darlo como no autenticado.
        if (tokenDecodificado == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        //Comprobar que el token no esté expirado.
        if (tokenDecodificado.EstaExpirado())
        {
            await this._srvDatosContexto.VaciarAsync();
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        //Token encontrado y válido.
        return new AuthenticationState(tokenDecodificado.GetClaimsPrincipal());
    }

    public async Task<bool> AutentificarAsync(string login, string contraseña)
    {
        string strRespuesta;
        HttpClient clienteHttp;
        UtilsTokenJwt.Token tokenDecodificado;
        ResultadoAutentificacion? resultadoAutentificacion;

        try
        {
            //Enviar petición al servidor.
            clienteHttp = this._factoriaClientesHttp.CreateClient("API");
            this.SetHeader(clienteHttp, login, contraseña);
            strRespuesta = await clienteHttp.GetStringAsync(this._strUrlAutenticar);
            if (string.IsNullOrEmpty(strRespuesta))
            {
                throw new Exception("La respuesta de servidor está vacía");
            }

            //Deserializar la respuesta.
            resultadoAutentificacion = JsonSerializer.Deserialize<ResultadoAutentificacion>(strRespuesta, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (resultadoAutentificacion == null)
            {
                throw new Exception("No se ha recibido una respuesta correcta del servidor");
            }

            //Decodificar el token para obtener la información almacenada en él.
            tokenDecodificado = UtilsTokenJwt.DecodificarToken(resultadoAutentificacion.Token);

            //Actualizar los datos de contexto.
            await this._srvDatosContexto.ActualizarAsync(
                resultadoAutentificacion.DatosDeSesion.CodigoIdioma,
                tokenDecodificado, resultadoAutentificacion.Token
            );

            //Avisar de que ha cambiado el estado de autenticación.
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(tokenDecodificado.GetClaimsPrincipal())));

            //Resultado correcto.
            return true;
        }
        catch (Exception excp)
        {
            Console.WriteLine(excp.Message);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
            throw new EstiloLibreException("ERR_Autentificacion", excp);
        }
    }

    public async Task QuitarAutentificacionAsync()
    {
        await this._srvDatosContexto.VaciarAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }

    #endregion

    #region ***** MÉTODOS PRIVADOS *****

    private void SetHeader(HttpClient clienteHttp, string login, string contraseña)
    {
        string header = this.Btoa(login + ":" + contraseña);
        AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Basic", header);
        clienteHttp.DefaultRequestHeaders.Authorization = headerValue;
    }

    private string Btoa(string toEncode)
    {
        byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
        string toReturn = Convert.ToBase64String(bytes);
        return toReturn;
    }

    #endregion

    #region "***** CLASES INTERNAS *****"

    private class ResultadoAutentificacion
    {
        public required string Token { get; set; }
        public required DatosSesionDTO DatosDeSesion { get; set; }
    }

    #endregion

    public ClaimsPrincipal GetClaimsPrincipal()
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>(), "Auth");
        ClaimsPrincipal claimsUsuario = new ClaimsPrincipal(claimsIdentity);
        return claimsUsuario;
    }
}
