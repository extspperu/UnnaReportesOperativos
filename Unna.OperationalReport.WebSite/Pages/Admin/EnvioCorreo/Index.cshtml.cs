using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Unna.OperationalReport.WebSite.Pages.Admin.EnvioCorreo
{
    public class IndexModel : PageModel
    {

        public string? Fecha { get; set; }
        public List<ReporteDto>? reportes { get; set; }

        private readonly IReporteServicio _reporteServicio;
        public IndexModel(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }

       

        public async Task OnGet()
        {
            Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy");
            var operacion = await _reporteServicio.ListarAsync();
            if (operacion == null || operacion.Resultado == null)
            {
                reportes = new List<ReporteDto>();
            }
            else
            {
                reportes = operacion.Resultado;
            }
            
            
        }

    }
}
