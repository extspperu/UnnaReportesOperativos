using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;

namespace Unna.OperationalReport.Service.Registros.Osinergmin.Dtos
{
    public class OsinergminDto
    {
        [JsonIgnore]
        public DateTime? Fecha { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public double? NacionalizadaM3 { get; set; }
        public double? NacionalizadaTn { get; set; }
        public double? NacionalizadaBbl { get; set; }
        public ArchivoRespuestaDto? Archivo { get; set; }

        [JsonIgnore]
        public DateTime Creado { get; set; }
        
        [JsonIgnore]
        public long? IdUsuario { get; set; }


        [JsonProperty(PropertyName = "fecha")]
        public string? FechaCadena
        {
            get
            {
                return Fecha.HasValue ? Fecha.Value.ToString("dd/MM/yyyy"):null;
            }
        }

    }
}
