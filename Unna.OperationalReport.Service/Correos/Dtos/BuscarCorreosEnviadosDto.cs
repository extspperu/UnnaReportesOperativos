using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Correos.Dtos
{
    public class BuscarCorreosEnviadosDto
    {
        public DateTime? DiaOperativo { get; set; }
        public string? Grupo { get; set; }
        public string? IdReporte { get; set; }
    }
}
