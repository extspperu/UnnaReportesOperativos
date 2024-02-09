using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Reporte
{
    [Route("api/admin/reporte/[controller]")]
    [ApiController]
    public class RegistroSupervisorController : ControladorBase
    {
        private readonly IRegistroSupervisorServicio _registroSupervisorServicio;
        public RegistroSupervisorController(IRegistroSupervisorServicio registroSupervisorServicio)
        {
            _registroSupervisorServicio = registroSupervisorServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(RegistroSupervisorDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            peticion.Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _registroSupervisorServicio.GuardarAsync("",peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("GuardarCargarArchivos")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarCargarArchivosAsync(RegistroSupervisorDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            peticion.Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _registroSupervisorServicio.GuardarAsync("CargarArchivo", peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("ObtenerPorFecha")]
        [RequiereAcceso()]
        public async Task<RegistroSupervisorDto?> ObtenerPorFechaAsync()
        {
            var fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _registroSupervisorServicio.ObtenerPorFechaAsync(fecha);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("SubirArchivos")]
        [RequiereAcceso()]
        public async Task<List<AdjuntoSupervisorDto>?> SubirArchivosAsync(List<IFormFile> file)
        {
            var operacion = await _registroSupervisorServicio.ValidarArhivosAsync(file);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
