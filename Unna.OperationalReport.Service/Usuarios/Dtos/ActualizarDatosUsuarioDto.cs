using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Usuarios.Dtos
{
    public class ActualizarDatosUsuarioDto
    {
        [JsonIgnore]
        public long IdUsuario { get; set; }
        public string? Documento { get; set; }
        public string? Paterno { get; set; }
        public string? Materno { get; set; }
        public string? Nombres { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
    }
}
