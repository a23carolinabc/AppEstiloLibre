using Microsoft.AspNetCore.Components;

namespace EstiloLibreFront.Servicios;

public class ServicioNavegacion
{
    private readonly IConfiguration _configuracion;
    private readonly NavigationManager _navigationManager;

    public ServicioNavegacion(IConfiguration configuration, NavigationManager navigationManager)
    {
        this._configuracion = configuration;
        this._navigationManager = navigationManager;
    }

    public void NavegarA(string strUri, bool bForzarCarga = false)
    {
        string strUrlBase;

        strUrlBase = this._configuracion["UrlBase"] ?? "";
        this._navigationManager.NavigateTo(Path.Combine(strUrlBase, strUri), forceLoad: bForzarCarga);
    }

    public string ObtenerRuta(string strUri)
    {
        string strUrlBase;

        strUrlBase = this._configuracion["UrlBase"] ?? "";
        return Path.Combine(strUrlBase, strUri);
    }

    public string GetUriActual()
    {
        return this._navigationManager.Uri;
    }
}

