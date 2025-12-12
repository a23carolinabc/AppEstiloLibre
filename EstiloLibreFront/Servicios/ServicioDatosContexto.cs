using Blazored.LocalStorage;
using EstiloLibreFront.Utils;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Timers;

namespace EstiloLibreFront.Servicios;

public class ServicioDatosContexto
{
    #region ***** VARIABLES INTERNAS *****

    public event Action ExpiracionToken;
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpClientFactory _factoriaClientesHttp;
    private DatosContexto? _datosContexto;
    private System.Timers.Timer _timerToken;
    private string strUrlActualizarDatosDeSesion = "usuarios/actualizarDatosSesion";

    #endregion

    #region ***** CONSTRUCTORES *****

    public ServicioDatosContexto(ILocalStorageService localStorage, IHttpClientFactory factoriaClientesHttp)
    {
        this._localStorage = localStorage;
        this._factoriaClientesHttp = factoriaClientesHttp;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    public async Task ActualizarAsync(
        string strCodigoIdioma,
        UtilsTokenJwt.Token tokenDecodificado,
        string tokenCodificado)
    {
        this._datosContexto = new DatosContexto()
        {
            CodigoIdioma = strCodigoIdioma,
            TokenDecodificado = tokenDecodificado,
            TokenCodificado = tokenCodificado
        };
                        
        //Iniciamos el temporizador para el aviso de la expiración del token.
        this._timerToken = IniciarTemporizador(tokenDecodificado);

        //Almacenar en navegador.
        await this._localStorage.SetItemAsync("DatosContexto", this._datosContexto);
    }

    public async Task ActualizarIdiomaAsync(string strCodigoIdioma)
    {
        //Actualizar el idioma del usuario en la base de datos.
        await this._factoriaClientesHttp.CreateClient("API")
            .PostAsync(this.strUrlActualizarDatosDeSesion, new StringContent($"{{\"NuevoCodigoIdioma\": \"{strCodigoIdioma}\"}}", Encoding.UTF8, "application/json"));

        //Actualizar internamente.
        if (this._datosContexto != null)
        {
            this._datosContexto.CodigoIdioma = strCodigoIdioma;

            //Almacenar en navegador.
            await this._localStorage.SetItemAsync("DatosContexto", this._datosContexto);
        }
    }

    public async Task VaciarAsync()
    {
        this._datosContexto = null;
        await this._localStorage.RemoveItemAsync("DatosContexto");
    }

    public UtilsTokenJwt.Token? GetTokenDecodificado()
    {
        return this._datosContexto?.TokenDecodificado;
    }

    public string? GetTokenCodificado()
    {
        return this._datosContexto?.TokenCodificado;
    }

    public string? GetCodigoIdiomaActual()
    {
        return this._datosContexto?.CodigoIdioma;
    }

    public async Task InicializarAsync()
    {
        DatosContexto? datosContexto;

        //Leer del almacenamiento local.
        datosContexto = await this._localStorage.GetItemAsync<DatosContexto>("DatosContexto");

        //Si no hay nada almacenado, no hay nada que inicializar.
        if (datosContexto == null)
        {
            return;
        }

        //Decodificar token. Este no puede ser serializado a json decodificado.
        datosContexto.TokenDecodificado = UtilsTokenJwt.DecodificarToken(datosContexto.TokenCodificado!);

        //Almacenar.
        this._datosContexto = datosContexto;
    }

    public string GetNombreApellidosUsuario()
    {
        string strNombre;
        string strApellidos;
        
        strNombre = this._datosContexto?.TokenDecodificado?.GetNombrePersona() ?? "";
        strApellidos = this._datosContexto?.TokenDecodificado?.GetApellidos() ?? "";

        return strNombre + " " + strApellidos;
    }

    public string GetLoginUsuario()
    {
        return this._datosContexto?.TokenDecodificado?.GetLoginUsuario() ?? "";
    }

    public List<string> GetPermisosUsuario()
    {
        return this._datosContexto?.TokenDecodificado?.GetPermisos().ToList()?? new();
    }

    public int GetUsuarioId()
    {
        return Convert.ToInt32(this._datosContexto?.TokenDecodificado?.GetUsuarioId());
    }

    #endregion

    #region ***** MÉTODOS PRIVADOS *****
    private System.Timers.Timer IniciarTemporizador(UtilsTokenJwt.Token tokenCodificado)
    {
        long dExpiracion;
        TimeSpan tiempoParaExpiracion;
        double dTiempoParaAviso;
        System.Timers.Timer timer;

        timer = new System.Timers.Timer();

        //Obtenemos fecha de experixación del token.
        dExpiracion = long.Parse(tokenCodificado.GetClaimsPrincipal().FindFirst("exp").Value, CultureInfo.InvariantCulture);
        tiempoParaExpiracion = DateTime.UnixEpoch.AddSeconds(dExpiracion).AddHours(1).Subtract(DateTime.Now);
        
        //Ponemos de aviso 5 minutos antes de la fehca de expiración.
        dTiempoParaAviso = tiempoParaExpiracion.Subtract(TimeSpan.FromMinutes(5)).TotalMilliseconds;
        
        timer.Elapsed += AvisarUsuario;
        timer.Interval = dTiempoParaAviso;
        timer.AutoReset = false;
        timer.Enabled = true;

        return timer;
    }

    private void AvisarUsuario(object? sender, ElapsedEventArgs e)
    {
        //Invocamos la función asociada en el MainLayout.
        ExpiracionToken?.Invoke();

        //Desechamos el temporizador.
        this._timerToken.Dispose();
    }
    #endregion

    private class DatosContexto
    {
        public required string CodigoIdioma { get; set; }
        [JsonIgnore]
        public UtilsTokenJwt.Token? TokenDecodificado { get; set; }
        public required string? TokenCodificado { get; set; }
    }
}
