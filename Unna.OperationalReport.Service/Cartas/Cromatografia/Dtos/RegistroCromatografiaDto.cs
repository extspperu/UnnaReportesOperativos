using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos
{
    public class RegistroCromatografiaDto
    {
        public string? Id { get; set; }
        public DateTime? Periodo { get; set; }
        public TimeSpan? HoraMuestreo { get; set; }
        public string? Tipo { get; set; }
        public int? IdLote { get; set; }
        public string? Lote { get; set; }
        public string? Tanque { get; set; }

        [JsonIgnore]
        public DateTime Creado { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
        
        public List<RegistroGnaGnsDto>? GnaGns { get; set; }
        public List<RegistroGlpDto>? Glp { get; set; }
        public List<RegistroAnalisiDespachoCgnDto>? Cgn { get; set; }

        public int? Anio
        {
            get
            {
                return Periodo.HasValue ? Periodo.Value.Year : new int?();
            }
        }

        public string? Mes
        {
            get
            {
                return Periodo.HasValue ? FechasUtilitario.ObtenerNombreMes(Periodo.Value) : null;
            }
        }
    }
}
