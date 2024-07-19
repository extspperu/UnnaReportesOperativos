using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos
{
    public class BuscarGnaGnsDto
    {
        public DateTime? Periodo { get; set; }
        public string? Tipo { get; set; }
        public int? IdLote { get; set; }
    }
}
