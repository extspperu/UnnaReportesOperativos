using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos
{
    public class ResumenEntregaDto
    {
        public int Item { get; set; }
        public string? Fecha { get; set; }
        public double? Cnpc { get; set; }
        public double? Total { get; set; }
        public double? NoProcesado { get; set; }
        public double? PlantaFs { get; set; }
        public double? AjustePlantaFs { get; set; }
        public double? Procesado { get; set; }
        public double? ProcesadoHoras { get; set; }

    }
}
