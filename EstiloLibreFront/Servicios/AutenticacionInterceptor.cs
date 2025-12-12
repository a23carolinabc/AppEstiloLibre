namespace EstiloLibreFront.Servicios;

public class AutenticacionInterceptor
    : DelegatingHandler
{
    private readonly ServicioDatosContexto _servicioDatosContexto;

    public AutenticacionInterceptor(
        ServicioDatosContexto servicioDatosContexto)
    {
        this._servicioDatosContexto = servicioDatosContexto;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? strToken;

        //Identificación.
        strToken = this._servicioDatosContexto.GetTokenCodificado();
        if (strToken != null && !request.Headers.Contains("Authorization"))
        {
            request.Headers.Add("Authorization", $"Bearer {this._servicioDatosContexto.GetTokenCodificado()}");
        }

        //Idioma actual.
        request.Headers.Add("X-EstiloLibre-CodigoIdioma", this._servicioDatosContexto.GetCodigoIdiomaActual());

        //Enviar la petición original.
        return base.SendAsync(request, cancellationToken);
    }
}
