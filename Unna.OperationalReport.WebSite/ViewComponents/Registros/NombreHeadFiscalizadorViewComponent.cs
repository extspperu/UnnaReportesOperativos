using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Registros
{
   
    public class NombreHeadFiscalizadorViewComponent : ViewComponent
    {
        public readonly IUsuarioLoteServicio _usuarioLoteServicio;
        public NombreHeadFiscalizadorViewComponent(
            IUsuarioLoteServicio usuarioLoteServicio
            )
        {
            _usuarioLoteServicio = usuarioLoteServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string descripcion)
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _usuarioLoteServicio.ObtenerLotePorIdUsuarioAsync(idUsuario);
            
            return View(new NombreHeadFiscalizadorDtoViewComponent()
            {
                Lote = operacion.Completado ? operacion.Resultado : null,
                Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1)).ToString("dd/MM/yyyy"),
                Descripcion = descripcion
            }); 
        }

        public class NombreHeadFiscalizadorDtoViewComponent
        {
            public LoteDto? Lote { get; set; }
            public string? Fecha { get; set; }
            public string? Descripcion { get; set; }
        }
    }
}
