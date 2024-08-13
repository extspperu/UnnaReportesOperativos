using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class ResumenBalanceEnergiaLGNResult
    {
        public Int32 Dia { get; set; }
        public decimal GASNaturalAsociadoMedido { get; set; }
        public decimal GasCombustibleMedidoSeco { get; set; }
        public decimal VolumenGasEquivalenteLGN { get; set; }
    }
}
