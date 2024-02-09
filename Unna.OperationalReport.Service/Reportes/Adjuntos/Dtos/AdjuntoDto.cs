using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos
{
    public class AdjuntoDto
    {
        public int Id { get; set; }
        public string? Grupo { get; set; }
        public string? Nomenclatura { get; set; }
        public string? Extension { get; set; }
    }
}
