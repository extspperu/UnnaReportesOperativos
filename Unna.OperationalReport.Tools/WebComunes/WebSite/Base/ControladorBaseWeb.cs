using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores;

namespace Unna.OperationalReport.Tools.WebComunes.WebSite.Base
{
    public class ControladorBaseWeb : Controller
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
            long idUsuario = 0;
            if (claim != null)
            {
                if (!long.TryParse(claim.Value, out idUsuario) && claim?.Subject?.Claims != null)
                {

                    var claimIdUsuario = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "IdUsuario");

                    if (claimIdUsuario != null && !string.IsNullOrEmpty(claimIdUsuario.Value))
                    {
                        idUsuario = Convert.ToInt32(claimIdUsuario.Value);
                    }
                    else
                    {
                        Console.WriteLine("Claim IdUsuario no está presente o su valor es nulo.");
                    }
                }
                return idUsuario;
            }
            return new long();
        }


        protected IActionResult ObtenerResultadoOGenerarErrorDeSimpleRespuestaPeticionWebDto(SimpleRespuestaPeticionWebDto respuesta)
        {

            return respuesta.StatusCode switch
            {
                HttpStatusCode.OK => Ok(JsonConvert.DeserializeObject(respuesta.Respuesta ?? "")),
                HttpStatusCode.NotFound => NotFound(JsonConvert.DeserializeObject(respuesta.Respuesta ?? "")),
                HttpStatusCode.BadRequest => BadRequest(JsonConvert.DeserializeObject(respuesta.Respuesta ?? "")),
                HttpStatusCode.InternalServerError => throw new Exception("Error en la llamada"),
                _ => new EmptyResult(),
            };
        }


    }
}
