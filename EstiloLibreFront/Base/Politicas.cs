using Microsoft.AspNetCore.Authorization;

namespace EstiloLibreFront.Base
{
    public class Politicas
    {
        public static AuthorizationPolicy CrearPolitica(string codigoPermiso)
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireClaim(ClaimsUsuario.PERMISOS, codigoPermiso)
                                                   .Build();
        }
    }
}
