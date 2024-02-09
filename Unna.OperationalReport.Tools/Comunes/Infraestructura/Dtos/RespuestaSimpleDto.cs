using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class RespuestaSimpleDto<T>
    {

        [JsonProperty(PropertyName = "mensaje")]
        public string? Mensaje { get; set; }

        [JsonProperty(PropertyName = "id")]
        public T? Id { get; set; }
    }
}
