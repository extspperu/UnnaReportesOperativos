using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class DespachoGlpEnvasadoDto
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public double? Envasado { get; set; }
        public double? Granel { get; set; }
    }
}
