using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class CorreoPeticionSimpleDto<T>
    {
        public CorreoPeticionSimpleDto(T id, string plantilla)
        {
            Id = id;
            Plantilla = plantilla;
        }

        [JsonProperty(PropertyName = "id")]
        public T Id { get; set; }

        [JsonProperty(PropertyName = "plantilla")]
        public string Plantilla { get; set; }
    }
}
