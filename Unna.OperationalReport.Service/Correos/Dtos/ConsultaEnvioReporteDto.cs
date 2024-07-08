using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Correos.Dtos
{
    public class ConsultaEnvioReporteDto
    {
        public string? IdReporte { get; set; }
        public string? Id { get; set; }
        public string? Destinatario { get; set; }
        public string? Cc { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public bool AdjuntaReporte { get; set; }
        public bool TieneArchivoPdf { get; set; }
        public bool TieneArchivoExcel { get; set; }
        public bool FueEnviado { get; set; }
        public bool ReporteFueGenerado { get; set; }
        public string? MensajeAlert { get; set; }
    }
}
