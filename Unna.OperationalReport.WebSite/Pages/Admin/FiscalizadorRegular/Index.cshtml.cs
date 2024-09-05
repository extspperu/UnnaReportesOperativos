using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorRegular
{
    public class IndexModel : PageModel
    {
        public List<DatoDto>? Datos { get; set; }
        public string? IdGrupo { get; set; }
        public bool PermitirEditar { get; set; }
        public string? Titulo { get; set; }

        private readonly IDatoServicio _datoServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public IndexModel(
            IUsuarioRepositorio usuarioRepositorio,
            IDatoServicio datoServicio,
            IDiaOperativoServicio diaOperativoServicio
            )
        {
            _datoServicio = datoServicio;
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task<IActionResult> OnGet(string Id)
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
                            if (idUsuario > 0)
                            {
                                var claimsIdentity = (ClaimsIdentity)User.Identity;

                                var existingClaim = claimsIdentity.FindFirst("IdUsuario");

                                if (existingClaim == null)
                                {
                                    claimsIdentity.AddClaim(new Claim("IdUsuario", idUsuario.ToString()));

                                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                                }
                            }
                        }
                    }
                }

            }

            var operacion = await _datoServicio.ListarPorTipoAsync(TiposFiscalizadores.Regular);
            if (operacion != null || operacion.Resultado != null)
            {
                Datos = operacion.Completado ? operacion.Resultado : new List<DatoDto>();
            }
            IdGrupo = RijndaelUtilitario.EncryptRijndaelToUrl((int)TipoGrupos.FiscalizadorRegular);


            switch (Id)
            {
                case TiposAcciones.Registro:
                    Titulo = $"REGISTRO DE DATOS";                    
                    break;
                case TiposAcciones.Editar:
                    Titulo = $"EDICIÓN DE DATOS";                    
                    break;
                default:
                    return RedirectToPage("/Admin/Error");
            }
            var operacionExisteRegistro = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(idUsuario, FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1)), (int)TipoGrupos.FiscalizadorRegular, null);
            if (operacionExisteRegistro ==null || !operacionExisteRegistro.Completado || operacionExisteRegistro.Resultado == null)
            {
                PermitirEditar = true;
            }
            else
            {
                PermitirEditar = operacionExisteRegistro.Resultado.DatoValidado == true ? false : true;
            }            

            return Page();
            
        }
    }
}
