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

        [JsonIgnore]
        public int? IdGrupo { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EsAdministrador { get; set; }
        public DateTime Creado { get; set; }        
        public DateTime? UltimoLogin { get; set; }
        public string? UrlFirma { get; set; }
        public string? IdPersona { get; set; }
        public string? Documento { get; set; }
        public string? Paterno { get; set; }
        public string? Materno { get; set; }
        public string? Nombres { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? IdGrupoCifrado { get; set; }
        public string? IdUsuarioCifrado { get; set; }
        public bool EsUsuarioExterno { get; set; }

        [JsonIgnore]
        public string? RutaFirma { get; set; }
        
        public string? IdLote { get; set; }
    }
}
