using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class VolumenDeltaVDto
    {
        public long Id { get; set; }
        public string? NombreLote { get; set; }
        public double? Volumen { get; set; }
    }
}
