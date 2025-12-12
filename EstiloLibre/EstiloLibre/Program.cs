using Serilog;

namespace EstiloLibre
{
    public class Program
    {
        public static readonly string EspacioDeNombres = typeof(Program).Namespace!;
        public static readonly string NombreAplicacion = EspacioDeNombres;

        public static int Main(string[] args)
        {
            IConfiguration configuracionGeneral;
            IHost host;

            //Cargar la configuraci�n general de la aplicaci�n (appsettings.json).
            configuracionGeneral = CargarConfiguracionGeneral();

            try
            {
                //Crear el host.
                host = BuildHostBuilder(args, configuracionGeneral);

                //Arrancar la aplicaci�n.
                host.Run();                

                //Salir sin indicar error al sistema operativo.
                return 0;
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
                return 1;
            }
        }

        public static IHost BuildHostBuilder(string[] args, IConfiguration configuracionGeneral)
        {
            string? strPort;
            string strUrls;
            
            // CRÍTICO PARA RAILWAY: Leer el puerto de la variable de entorno
            strPort = Environment.GetEnvironmentVariable("PORT");
            
            if (!string.IsNullOrEmpty(strPort))
            {
                strUrls = $"http://0.0.0.0:{strPort}";
            }
            else
            {
                strUrls = "http://0.0.0.0:80";
            }
            
            return Host.CreateDefaultBuilder(args)
                //Especificar que la configuración de la clase Startup sea la recibida por argumento en lugar
                //de que .NET la cargue de nuevo por su cuenta.
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuracionGeneral);
                })
                //Indicar la clase Startup.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                    // CRÍTICO: Configurar el puerto dinámico de Railway
                    webBuilder.UseUrls(strUrls);
                })
                //Borrar todos los registros de los loggers que vienen prerregistrados.
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
            .Build();
        }

        private static IConfiguration CargarConfiguracionGeneral()
        {
            IConfigurationBuilder configBuilder;
            string strPathBase;
            string? strEntorno;
            
            // Obtener entorno
            strEntorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            //En el entorno de PRD la ubicación del fichero de configuración puede estar en una variable 
            //de entorno (PRD).
            strPathBase = Environment.GetEnvironmentVariable("apps_config_path") ?? Directory.GetCurrentDirectory();
            
            //Cargar el fichero appsettings.json.
            configBuilder = new ConfigurationBuilder()
                .SetBasePath(strPathBase)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{strEntorno}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            //Devolver el objeto de configuración.
            return configBuilder.Build();
        }
    }
}
