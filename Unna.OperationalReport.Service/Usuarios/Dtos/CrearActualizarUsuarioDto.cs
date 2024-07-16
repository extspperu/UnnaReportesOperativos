using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Usuarios.Dtos
{
    public class CrearActualizarUsuarioDto
    {
     
        public string? IdUsuario { get; set; }
        public string? Username { get; set; }
        public string? IdGrupo { get; set; }
        public bool EstaHabilitado { get; set; }      
        public bool EsAdministrador { get; set; }      
        public string? Documento { get; set; }
        public string? Paterno { get; set; }
        public string? Materno { get; set; }
        public string? Nombres { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }


        [JsonIgnore]
        public long? IdUsuarioAdmin { get; set; }

    }
}
