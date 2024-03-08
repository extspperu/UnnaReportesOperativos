using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class VolumenDespachoDto
    {
        public long Id { get; set; }
        public string? Tanque { get; set; }
        public string? Cliente { get; set; }
        public string? Placa { get; set; }
        public double? Volumen { get; set; }
        public string? Tipo { get; set; }
    }
}
