using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EstiloLibreFront.Servicios;
using System.Globalization;

namespace EstiloLibreFront
{
    public static class Extensiones
    {
        public async static Task InicializarContexto(this WebAssemblyHost host)
        {
            ServicioDatosContexto servicio;
            CultureInfo cultureInfo;

            servicio = host.Services.GetRequiredService<ServicioDatosContexto>();
            await servicio.InicializarAsync();

            if (!string.IsNullOrWhiteSpace(servicio.GetCodigoIdiomaActual()))
            {
                cultureInfo = new CultureInfo(servicio.GetCodigoIdiomaActual()!);
            }
            else
            {
                cultureInfo = new CultureInfo(Idiomas.Predeterminado.Cultura);
            }
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
