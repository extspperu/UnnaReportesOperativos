using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos
{
    public class RegistroDto
    {
        public string? Id { get; set; }
        public double? Valor { get; set; }
        public bool? EsConciliado { get; set; }        
        public int? IdDato { get; set; }
        public string? IdDiaOperativo { get; set; }        
        public bool? EsValido { get; set; }
        public DateTime? FechaValido { get; set; }
        public bool? EsEditadoPorProceso { get; set; }
        public bool? EsDevuelto { get; set; }

        [JsonIgnore]
        public DateTime Creado { get; set; }

    }
}
