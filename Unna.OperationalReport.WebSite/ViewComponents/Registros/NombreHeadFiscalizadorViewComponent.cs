using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

namespace Unna.OperationalReport.WebSite.ViewComponents.Registros
{
   
    public class NombreHeadFiscalizadorViewComponent : ViewComponent
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public readonly IUsuarioLoteServicio _usuarioLoteServicio;
        public NombreHeadFiscalizadorViewComponent(
            IUsuarioRepositorio usuarioRepositorio,
            IUsuarioLoteServicio usuarioLoteServicio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioLoteServicio = usuarioLoteServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string descripcion)
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                if (!long.TryParse(claim.Value, out idUsuario) && claim?.Subject?.Claims != null)
                {
                    var emailClaim = claim.Subject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                    if (emailClaim != null)
                    {
                        string email = emailClaim.Value;

                        var resultado = await _usuarioRepositorio.VerificarUsuarioAsync(email);

                        if (resultado.Existe)
                        {
                            idUsuario = resultado.IdUsuario ?? 0;
                        }
                    }
                }
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
