using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.Lotes.Dtos
{
    public class UsuarioLoteDto
    {

        [JsonIgnore]
        public long? IdUsuario { get; set; }
        public string? IdLote { get; set; }
        public bool EstaActivo { get; set; }
        public string? IdGrupo { get; set; }
    }
}
