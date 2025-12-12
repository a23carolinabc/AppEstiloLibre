using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasExploracion.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExploracionController : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasExploracion _consultasExploracion;

    #endregion

    #region ***** CONSTRUCTORES *****

    public ExploracionController(IMediator mediator, ConsultasExploracion consultasExploracion)
    {
        this._mediador = mediator;
        this._consultasExploracion = consultasExploracion;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    [Route("datos")]
    [HttpGet]
    [ProducesResponseType(typeof(DatosExploracionDTO), (int)HttpStatusCode.OK)]
    public IActionResult GetDatosExploracion()
    {
        DatosExploracionDTO objeto;

        objeto = this._consultasExploracion.GetDatosExploracion();

        return Ok(objeto);
    }

    [Route("conjuntos")]
    [HttpGet]
    [ProducesResponseType(typeof(List<ConjuntoPublicoResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetConjuntosPublicos(
        [FromQuery] int? tipoBusqueda = null,
        [FromQuery] int? valorBusqueda = null)
    {
        List<ConjuntoPublicoResumenDTO> lista;
        BusquedaConjuntosPublicosDTO? parametros;

        parametros = null;

        if (tipoBusqueda.HasValue && valorBusqueda.HasValue)
        {
            parametros = new BusquedaConjuntosPublicosDTO
            {
                TipoBusqueda = (TipoBusquedaConjunto)tipoBusqueda.Value,
                ValorBusqueda = valorBusqueda.Value
            };
        }

        lista = await this._consultasExploracion.GetConjuntosPublicos(parametros);

        return Ok(lista);
    }

    [Route("prendas")]
    [HttpGet]
    [ProducesResponseType(typeof(List<PrendaPublicaResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPrendasPublicas(
        [FromQuery] int? tipoBusqueda = null,
        [FromQuery] int? valorBusqueda = null)
    {
        List<PrendaPublicaResumenDTO> lista;
        BusquedaPrendasPublicasDTO? parametros;

        parametros = null;

        if (tipoBusqueda.HasValue && valorBusqueda.HasValue)
        {
            parametros = new BusquedaPrendasPublicasDTO
            {
                TipoBusqueda = (TipoBusquedaPrenda)tipoBusqueda.Value,
                ValorBusqueda = valorBusqueda.Value
            };
        }

        lista = await this._consultasExploracion.GetPrendasPublicas(parametros);

        return Ok(lista);
    }

    [Route("copiar/prenda/{id}")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CopiarPrendaPublica([FromRoute] int id)
    {
        CmdCopiarPrendaPublica comando;
        int iPrendaIdNueva;

        comando = new CmdCopiarPrendaPublica(id);
        iPrendaIdNueva = await this._mediador.Send(comando);

        return Ok(iPrendaIdNueva);
    }

    [Route("copiar/conjunto/{id}")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CopiarConjuntoPublico([FromRoute] int id)
    {
        CmdCopiarConjuntoPublico comando;
        int iConjuntoIdNuevo;

        comando = new CmdCopiarConjuntoPublico(id);
        iConjuntoIdNuevo = await this._mediador.Send(comando);

        return Ok(iConjuntoIdNuevo);
    }

    #endregion
}
