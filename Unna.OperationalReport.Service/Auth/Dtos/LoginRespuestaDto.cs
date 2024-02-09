using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Auth.Dtos
{
    public class LoginRespuestaDto
    {
        [JsonProperty(PropertyName = "suceso")]
        public bool Suceso { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string? Mensaje { get; set; }

        public string? TokenAcceso { get; set; }

        public long? IdUsuario { get; set; }
    }
}
