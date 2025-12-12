using EstiloLibreFront.Utils;

namespace EstiloLibreFront.Objetos.Usuarios
{
    public partial class UsuarioData
    {
        public UsuarioDTO? Usuario { get; set; }
        public List<ControlItem>? Idiomas { get; set; }
    }
}
