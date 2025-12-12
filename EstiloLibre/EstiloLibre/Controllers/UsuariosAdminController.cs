using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Comandos.CmdUsuariosAdminSaveData.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuariosAdmin.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosAdminController : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasUsuariosAdmin _consultasUsuariosAdmin;

    #endregion

    #region ***** CONSTRUCTORES *****

    public UsuariosAdminController(IMediator mediator,
                                   ConsultasUsuariosAdmin consultasUsuariosAdmin)
    {
        this._mediador = mediator;
        this._consultasUsuariosAdmin = consultasUsuariosAdmin;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    
    /// Obtiene los datos necesarios para crear un nuevo usuario administrador    
    [Route("addnew")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioAdminAddNewDTO), (int)HttpStatusCode.OK)]
    public IActionResult AddNew()
    {
        UsuarioAdminAddNewDTO objeto;

        // Recuperar datos necesarios para el addnew
        objeto = this._consultasUsuariosAdmin.GetDatosAddNew();

        // Devolver el resultado de la ejecución
        return Ok(objeto);
    }

    
    /// Obtiene los datos completos de un usuario administrador para edición    
    [Route("showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioAdminShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ShowData([FromRoute] int id)
    {
        UsuarioAdminShowDataDTO objeto;

        // Recuperar datos necesarios para el showdata
        objeto = await this._consultasUsuariosAdmin.GetDatosShowData(id);

        // Devolver el resultado de la ejecución
        return Ok(objeto);
    }

    
    /// Guarda o actualiza un usuario administrador    
    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SaveData([FromBody] UsuarioAdminSaveDataDTO usuario)
    {
        CmdUsuariosAdminSaveData comando;
        int iUsuarioId;

        comando = new CmdUsuariosAdminSaveData(usuario);
        iUsuarioId = await this._mediador.Send(comando);

        return Ok(iUsuarioId);
    }

    
    /// Elimina un usuario administrador    
    [Route("delete/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        CmdUsuariosAdminDelete comando;

        comando = new CmdUsuariosAdminDelete(id);
        await this._mediador.Send(comando);

        return Ok();
    }

    
    /// Obtiene un listado de usuarios administradores con búsqueda opcional
    
    [Route("listado")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioAdminResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetListadoUsuariosAdmin([FromQuery] string? textoBusqueda, [FromQuery] string? tipoBusqueda)
    {
        IEnumerable<UsuarioAdminResumenDTO> lista;

        // Recuperar listado de usuarios admin
        lista = await this._consultasUsuariosAdmin.GetListadoUsuariosAdmin(textoBusqueda, tipoBusqueda);

        // Devolver el resultado de la ejecución
        return Ok(lista);
    }

    #endregion
}
