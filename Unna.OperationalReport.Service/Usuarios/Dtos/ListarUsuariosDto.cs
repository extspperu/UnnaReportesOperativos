using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Usuarios.Dtos
{
    public class ListarUsuariosDto
    {
        public string? IdUsuario { get; set; }
        public string? Username { get; set; }

        [JsonIgnore]
        public DateTime Creado { get; set; }

        [JsonIgnore]
        public DateTime? UltimoLogin { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EsAdministrador { get; set; }
        public string? Grupo { get; set; }
        public string? Documento { get; set; }
        public string? Nombres { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }


        [JsonProperty(PropertyName = "creado")]
        public string? CreadoCadena
        {
            get
            {
                return Creado.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }


        [JsonProperty(PropertyName = "ultimoLogin")]
        public string? UltimoLoginCadena
        {
            get
            {
                return UltimoLogin.HasValue ? UltimoLogin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null;
            }
        }
    }
}
