using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores;

namespace Unna.OperationalReport.Tools.WebComunes.ApiWeb.Base
{
    public class ApiControladorBase : ControllerBase
    {

        protected void GenerarBadRequestError(int codigoError, List<string>? errores)
        {
            throw new ApiError(HttpStatusCode.BadRequest, codigoError, errores);
        }

        protected void GenerarNotFoundError(List<string>? errores)
        {
            throw new ApiError(HttpStatusCode.NotFound, (int)HttpStatusCode.NotFound, errores);
        }

        protected void VerificarIfEsBuenJson<T>(T objeto)
            where T : class
        {
            if (objeto == default(T))
            {
                GenerarBadRequestError(400, new List<string>() { "Verifica que el json sigue el modelo establecido" });
            }
        }
        protected T? ObtenerResultadoOGenerarErrorDeOperacion<T>(OperacionDto<T> operacion)
        {

            if (!operacion.Completado)
            {
                switch (operacion.Codigo)
                {
                    case CodigosOperacionDto.NoExiste:
                        GenerarNotFoundError(operacion.Mensajes);
                        break;
                    default:
                        GenerarBadRequestError((int)operacion.Codigo, operacion.Mensajes);
                        break;
                }
            }


            return operacion.Resultado;
        }

        protected long? ObtenerIdUsuarioActual()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return Convert.ToInt64(claim.Value);
            }
            return new long?();
        }
    }
}
