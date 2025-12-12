using Microsoft.AspNetCore.Authorization;
using EstiloLibreFront.Base;
using System.Reflection;

namespace EstiloLibreFront.Utils
{
    public static class UtilsPermisos
    {
        public static IEnumerable<string> ObtenerTodosLosPermisos()
        {
            return typeof(CodigosPermisos)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!)!;
        }

        public static AuthorizationPolicy CrearPolitica(string codigoPermiso)
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireClaim(ClaimsUsuario.PERMISOS, codigoPermiso)
                                                   .Build();
        }

        public static AuthorizationPolicy CrearPolitica(params string[] codigosPermiso)
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireClaim(ClaimsUsuario.PERMISOS, codigosPermiso)
                                                   .Build();
        }
    }
}
