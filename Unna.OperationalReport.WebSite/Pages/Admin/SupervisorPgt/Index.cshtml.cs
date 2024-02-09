using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.SupervisorPgt
{
    public class IndexModel : PageModel
    {
        private readonly IAdjuntoServicio _adjuntoServicio;
        public IndexModel(IAdjuntoServicio adjuntoServicio)
        {
            _adjuntoServicio = adjuntoServicio;
        }

        public string? Fecha { get; set; }
        public List<AdjuntoDto>? Adjuntos { get; set; }
        public async Task OnGet()
        {
            Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1)).ToString("dd/MM/yyyy");
            var operacion = await _adjuntoServicio.ListarPorGrupoAsync(TipoGrupoAdjuntos.SupervisorPgt);
            Adjuntos = operacion.Completado ? operacion.Resultado : new List<AdjuntoDto>();
        }
    }
}
