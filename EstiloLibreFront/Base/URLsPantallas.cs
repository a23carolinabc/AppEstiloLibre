namespace EstiloLibreFront.Base
{
    public class URLsPantallas
    {
        public const string Inicio = "/";
        public const string Conocenos = "conocenos/";
        public const string PoliticaPrivacidad = "https://www.boe.es/eli/es/lo/2018/12/05/3";
        public const string Login = "login/";
        public const string Registro = "registro/";

        public const string Prendas = "prendas/";
        public const string Conjuntos = "conjuntos/";
        public const string ListadoPrendas = "listadoPrendas/";
        public const string ListadoConjuntos = "listadoConjuntos/";
        public const string Configuracion = "configuracion/";

        // Rutas de exploración pública
        public const string ExplorarConjuntos = "explorar/conjuntos/";
        public const string ExplorarPrendas = "explorar/prendas/";

        // Rutas de administración - Usuarios Admin (requiere permiso PLUS)
        public const string AdminMenu = "admin/menu/";
        public const string AdminUsuarios = "admin/usuarios/";
        public const string AdminUsuariosListado = "admin/usuarios/listado/";

        // Rutas de administración - Usuarios Normales (requiere permiso ADMIN)
        public const string AdminUsuariosNormales = "admin/usuarios-normales/";
        public const string AdminUsuariosNormalesListado = "admin/usuarios-normales/listado/";
    }
}