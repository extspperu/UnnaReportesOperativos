using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class DatoCgnDto
    {
        public long Id { get; set; }
        public string? Tanque { get; set; }
        public double? Centaje { get; set; }
        public double? Volumen { get; set; }
    }
}
