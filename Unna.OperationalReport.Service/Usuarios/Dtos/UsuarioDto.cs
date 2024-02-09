using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Usuarios.Dtos
{
    public class UsuarioDto
    {
        [JsonIgnore]
        public long IdUsuario { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PaswordSalt { get; set; }
        public int? IdGrupo { get; set; }
        public bool EstaHabilitado { get; set; }
        public DateTime Creado { get; set; }        
        public DateTime? UltimoLogin { get; set; }
    }
}
