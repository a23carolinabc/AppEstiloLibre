using EstiloLibre.Base;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Comandos.CmdUsuariosSaveData.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuarios.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController
    : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ServicioIdentidadTokenJwt _servicioIdentidad;
    private readonly ConsultasUsuarios _consultasUsuarios;

    #endregion

    #region ***** CONSTRUCTORES *****

    public UsuariosController(IMediator mediator,
                              ServicioIdentidadTokenJwt servicioIdentidad,
                              ConsultasUsuarios consultasUsuarios)
    {
        this._mediador = mediator;
        this._servicioIdentidad = servicioIdentidad;
        this._consultasUsuarios = consultasUsuarios;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    [Route("actualizarDatosSesion")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ActualizarDatosDeSesion([FromBody] CmdActualizarDatosSesion comando)
    {
        //Enviar el comando al mediador para su procesamiento.
        await _mediador.Send(comando);

        //Devolver el resultado de la ejecución.
        return Ok();
    }

    [Route("addnew")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioAddNewDTO), (int)HttpStatusCode.OK)]
    public IActionResult AddNew()
    {
        UsuarioAddNewDTO objeto;

        //Recuperar datos necesarios para el addnew.
        objeto = this._consultasUsuarios.GetDatosAddNew();

        //Devolver el resultado de la ejecución.
        return Ok(objeto);
    }

    [Route("showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ShowData([FromRoute] int id)
    {
        UsuarioShowDataDTO objeto;

        //Recuperar datos necesarios para el showdata.
        objeto = await this._consultasUsuarios.GetDatosShowData(id);

        //Devolver el resultado de la ejecución.
        return Ok(objeto);
    }

    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SaveData([FromBody] UsuarioSaveDataDTO Usuario)
    {
        CmdUsuariosSaveData comando;
        int resultado;

        comando = new CmdUsuariosSaveData(Usuario);
        resultado = await this._mediador.Send(comando);
        return Ok(resultado);
    }

    [Route("delete/{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        CmdUsuariosDelete comando;

        comando = new CmdUsuariosDelete(id);
        await this._mediador.Send(comando);
        return Ok();
    }
    #endregion
}
