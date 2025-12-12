using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasModeracion.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModeracionController : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasModeracion _consultasAdministracion;

    #endregion

    #region ***** CONSTRUCTORES *****

    public ModeracionController(IMediator mediator,
                                    ConsultasModeracion consultasAdministracion)
    {
        this._mediador = mediator;
        this._consultasAdministracion = consultasAdministracion;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    
    /// Obtiene un listado de usuarios no administradores con búsqueda opcional    
    [Route("listado")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioNormalResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetListadoUsuarios([FromQuery] string? textoBusqueda, [FromQuery] string? tipoBusqueda)
    {
        IEnumerable<UsuarioNormalResumenDTO> lista;

        // Recuperar listado de usuarios normales
        lista = await this._consultasAdministracion.GetListadoUsuarios(textoBusqueda, tipoBusqueda);

        // Devolver el resultado de la ejecución
        return Ok(lista);
    }

    
    /// Obtiene los datos completos de un usuario normal
    [Route("usuario/showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioNormalShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDatosDetalleUsuario([FromRoute] int id)
    {
        UsuarioNormalShowDataDTO objeto;

        // Recuperar datos del usuario normal con sus prendas y conjuntos
        objeto = await this._consultasAdministracion.GetDatosDetalleUsuario(id);

        // Devolver el resultado de la ejecución
        return Ok(objeto);
    }

    
    /// Elimina una prenda de un usuario normal    
    [Route("delete/prenda/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> EliminarPrenda([FromRoute] int id)
    {
        CmdAdminEliminarPrenda comando;

        comando = new CmdAdminEliminarPrenda(id);
        await this._mediador.Send(comando);

        return Ok();
    }

    
    /// Elimina un conjunto de un usuario normal    
    [Route("delete/conjunto/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> EliminarConjunto([FromRoute] int id)
    {
        CmdAdminEliminarConjunto comando;

        comando = new CmdAdminEliminarConjunto(id);
        await this._mediador.Send(comando);

        return Ok();
    }

    #endregion
}
