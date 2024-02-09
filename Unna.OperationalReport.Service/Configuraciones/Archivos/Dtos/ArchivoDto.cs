using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos
{
    public class ArchivoDto
    {
        public string? Id { get; set; }

        [JsonIgnore]
        public byte[]? Contenido { get; set; }

        [JsonIgnore]
        public string? TipoMime { get; set; }

        [JsonIgnore]
        public string? Nombre { get; set; }

        [JsonIgnore]
        public string? Ruta { get; set; }

        public string? Url { get; set; }
    }
}
