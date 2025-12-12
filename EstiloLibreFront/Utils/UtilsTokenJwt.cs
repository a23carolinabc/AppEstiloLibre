using EstiloLibreFront.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EstiloLibreFront.Utils;

public class UtilsTokenJwt
{
    public static Token DecodificarToken(string strToken)
    {
        JwtSecurityTokenHandler manejadorToken;
        JwtSecurityToken token;

        manejadorToken = new JwtSecurityTokenHandler();
        token = manejadorToken.ReadJwtToken(strToken);

        return new Token(token);
    }

    public class Token
    {
        private JwtSecurityToken _token;

        public bool EstaExpirado()
        {
            return this._token.ValidTo <= DateTime.UtcNow;
        }

        public string? GetLoginUsuario()
        {
            return this._token.Claims.FirstOrDefault(c => c.Type == ClaimsUsuario.LOGIN)?.Value;
        }

        public string? GetUsuarioId()
        {
            return this._token.Claims.FirstOrDefault(c => c.Type == ClaimsUsuario.ID)?.Value;
        }

        public string? GetNombrePersona()
        {
            return this._token.Claims.FirstOrDefault(c => c.Type == ClaimsUsuario.NOMBRE)?.Value;
        }

        public string? GetApellidos()
        {
            return this._token.Claims.FirstOrDefault(c => c.Type == ClaimsUsuario.APELLIDOS)?.Value;
        }

        public IEnumerable<string> GetPermisos()
        {
            return this._token.Claims
                .Where(c => c.Type == ClaimsUsuario.PERMISOS)
                .Select(c => c.Value);
        }

        public ClaimsPrincipal GetClaimsPrincipal()
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(this._token.Claims, "Auth");
            ClaimsPrincipal claimsUsuario = new ClaimsPrincipal(claimsIdentity);
            return claimsUsuario;
        }

        public Token(JwtSecurityToken token)
        {
            this._token = token;
        }
    }
}
