using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Registro
{
    [Route("api/admin/registro/[controller]")]
    [ApiController]
    public class DiaOperativoController : ControladorBase
    {

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public DiaOperativoController(IDiaOperativoServicio diaOperativoServicio)
        {
            _diaOperativoServicio = diaOperativoServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(DiaOperativoDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            peticion.Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _diaOperativoServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("Obtener/{id}")]
        [RequiereAcceso()]
        public async Task<DiaOperativoDto?> ObtenerAsync(string id)
        {
            var operacion = await _diaOperativoServicio.ObtenerAsync(id);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("ObtenerPorUsuarioFiscalizadorRegular")]
        [RequiereAcceso()]
        public async Task<DiaOperativoDto?> ObtenerPorUsuarioFiscalizadorRegularAsync()
        {
            DateTime fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(ObtenerIdUsuarioActual()??0,fecha, (int)TipoGrupos.FiscalizadorRegular,null);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("ObtenerPorUsuarioFiscalizadorEnel/{orden}")]
        [RequiereAcceso()]
        public async Task<DiaOperativoDto?> ObtenerPorUsuarioFiscalizadorEnelAsync(int? orden)
        {
            DateTime fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            var operacion = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(ObtenerIdUsuarioActual() ?? 0, fecha, (int)TipoGrupos.FiscalizadorEnel, orden);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
