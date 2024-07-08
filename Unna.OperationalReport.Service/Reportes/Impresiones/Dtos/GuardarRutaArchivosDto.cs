using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Dtos
{
    public class GuardarRutaArchivosDto
    {
        public int IdReporte { get; set; }
        public string? RutaPdf { get; set; }
        public string? RutaExcel { get; set; }
    }
}
