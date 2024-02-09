using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores;

namespace Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos
{
    public class RequiereAccesoAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly List<int> _permisos;

        public RequiereAccesoAttribute(params int[] permisos)
        {
            _permisos = permisos.ToList();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Delay(0);
            if (_permisos == null || _permisos.Count == 0)
            {
                return;
            }

            var token = context.HttpContext.Request.Headers["token-acceso"];

            var tokenAccesoServicioObjeto = context.HttpContext.RequestServices.GetService(typeof(ITokenAccesoServicio));

            if (tokenAccesoServicioObjeto == null)
            {
                throw new ApiError(HttpStatusCode.Forbidden, (int)CodigosOperacionDto.AccesoInvalido, new List<string>() { $"Ud. no tiene permiso para  acceder" });
            }

            var tokenAccesoServicio = (ITokenAccesoServicio)tokenAccesoServicioObjeto;

            var operacionToken = tokenAccesoServicio.ObtenerTokenAccesoDeCadena(token);

            if (!operacionToken.Completado || operacionToken.Resultado == null)
            {
                throw new ApiError(HttpStatusCode.Forbidden, (int)CodigosOperacionDto.AccesoInvalido, new List<string>() { $"Error en optener Token" });
            }

            //var tienePermiso = true;
            //var permisoServicioObjeto = context.HttpContext.RequestServices.GetService(typeof(IPermisosServicio));
            //if (permisoServicioObjeto == null)
            //{
            //    throw new ApiError(HttpStatusCode.Forbidden, (int)CodigosOperacionDto.AccesoInvalido, new List<string>() { $"Ud. no tiene permiso para  acceder" });
            //}

            //var permisosServicio = (IPermisosServicio)permisoServicioObjeto;

            //foreach (var permiso in this._permisos)
            //{
            //    if (!(await permisosServicio.UsuarioTienePermisoAsync(operacionToken.Resultado.IdUsuario, permiso)))
            //    {
            //        tienePermiso = false;
            //    }
            //}

            //if (!tienePermiso)
            //{
            //    throw new ApiError(HttpStatusCode.Forbidden, (int)CodigosOperacionDto.AccesoInvalido, new List<string>() { $"Ud. no tiene permiso para  acceder" });
            //}

        }


    }


}
