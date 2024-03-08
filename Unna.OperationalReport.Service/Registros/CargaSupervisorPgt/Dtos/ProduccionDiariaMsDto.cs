using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class ProduccionDiariaMsDto
    {
        public long Id { get; set; }
        public string? Producto { get; set; }
        public double? MedidoresMasicos { get; set; }
    }
}
