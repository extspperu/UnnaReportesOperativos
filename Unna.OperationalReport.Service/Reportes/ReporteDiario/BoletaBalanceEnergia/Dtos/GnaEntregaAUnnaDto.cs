using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos
{
    public class GnaEntregaAUnnaDto
    {

        public string? Entrega { get; set; }
        public double Volumen { get; set; }
        public double PoderCalorifico { get; set; }
        public double Energia { get; set; }
        public double Riqueza { get; set; }
    }
}
