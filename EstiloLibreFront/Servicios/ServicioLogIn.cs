using EstiloLibreFront.Base;
using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Servicios
{
    public class ServicioLogIn
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAutentificacion _servicioAutentificacion;
        private readonly ServicioNavegacion _navegacion;
        private readonly ServicioDatosContexto _servicioDatosContexto;

        #endregion

        #region ***** CONSTRUCTOR ***** 

        public ServicioLogIn(
            ServicioNavegacion navigation,
            ServicioAutentificacion authenticationService,
            ServicioDatosContexto servicioDatosContexto)
        {
            this._servicioAutentificacion = authenticationService;
            this._navegacion = navigation;
            this._servicioDatosContexto = servicioDatosContexto;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<bool> LogIn(string login, string password)
        {
            try
            {
                await this._servicioAutentificacion.AutentificarAsync(login, password);

                // Después de autentificar exitosamente, redirigir según permisos
                this.RedirigirSegunPermisos();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this._navegacion.NavegarA(URLsPantallas.Login); 
                return false;
            }
        }

        public async void LogOut()
        {    
            this._navegacion.NavegarA(URLsPantallas.Login);        
            await this._servicioAutentificacion.QuitarAutentificacionAsync();            
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private void RedirigirSegunPermisos()
        {
            IEnumerable<string> permisos;

            // Obtener los permisos del usuario desde el token JWT decodificado
            permisos = this._servicioDatosContexto.GetPermisosUsuario();

            // Redirigir según los permisos del usuario
            // Administradores
            if (permisos.Contains(CodigosPermisos.MODERADOR) || permisos.Contains(CodigosPermisos.ADMIN))
            {
                this._navegacion.NavegarA(URLsPantallas.AdminUsuariosNormalesListado);
            }
            else
            {
                //Usuarios normales
                this._navegacion.NavegarA(URLsPantallas.ListadoPrendas);
            }
        }

        #endregion
    }
}
