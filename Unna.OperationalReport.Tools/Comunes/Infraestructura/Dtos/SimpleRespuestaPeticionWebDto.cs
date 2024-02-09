using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class SimpleRespuestaPeticionWebDto
    {

        public SimpleRespuestaPeticionWebDto(
            HttpStatusCode statusCode, string? respuesta
            )
        {
            StatusCode = statusCode;
            Respuesta = respuesta;
        }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty(PropertyName = "respuesta")]
        public string? Respuesta { get; set; }
    }
}
