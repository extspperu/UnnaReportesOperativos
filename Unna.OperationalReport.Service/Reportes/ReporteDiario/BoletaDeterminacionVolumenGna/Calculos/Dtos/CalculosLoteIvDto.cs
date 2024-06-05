using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos
{
    public class CalculosLoteIvDto
    {
        public List<PropiedadesFisicasDto>? PropiedadesGpa { get; set; }
        public List<PropiedadesFisicasDto>? PropiedadesGpsa { get; set; }
        public List<ComponsicionGnaEntradaDto>? ComponsicionGnaEntrada { get; set; }
        public CantidadCalidadDto? CantidadCalidad { get; set; }


        // 3_FACTOR
        public DeterminacionFactorConvertirVolumenLgnDto? DeterminacionFactorConvertirVolumenLgn { get; set; }

    }
}
